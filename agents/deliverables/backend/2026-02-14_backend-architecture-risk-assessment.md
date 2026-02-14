# Backend Architecture Risk Assessment

## 1. Current State

- Backend is an ASP.NET Core Web API (`net9.0`) using controllers and handler classes wired manually in DI (`Bike2Beans/Program.cs`).
- Persistence is MongoDB via `CoffeeShopRepository` with a single `coffeeshop` collection and basic `GetAllAsync`/`InsertAsync` operations (`Bike2Beans/Data/CoffeeShopRepository.cs`).
- External place discovery is split between:
  - Google Places SDK client (`PlacesClient`) for Nearby, Place Details, and Autocomplete (`Bike2Beans/Application/CoffeeShops/Queries/Search/*`, `.../Autocomplete/AutocompleteHandler.cs`).
  - Raw REST call for Text Search through `IPlacesRestGateway` + `GooglePlacesRestGateway` (`Bike2Beans/Infrastructure/GooglePlacesRestGateway.cs`).
- Google API key is server-side configured with startup validation (`Bike2Beans/Infrastructure/GoogleServiceExtension.cs`), but there is no endpoint authentication/authorization in the pipeline.
- Request validation, response versioning, caching, retries, and backend tests are currently absent.

## 2. Risks and Failure Modes

- **Config mismatch risk (high):** `Program.cs` binds `MongoDBSettings`, but `appsettings.Development.json` uses `MongoDB`; this can break dev DB initialization or silently use defaults (`Bike2Beans/Program.cs`, `Bike2Beans/appsettings.Development.json`).
- **Unbounded API input risk (high):** query params like `max`, `radiusMeters`, and `PageSize` have no clamps/validation, enabling excessive downstream Google API use and latency spikes (`Bike2Beans/Controllers/GooglePlacesController.cs`).
- **Contract inconsistency risk (high):** `AutocompleteHandler` returns `null` for empty input despite list return type, creating fragile client behavior (`Bike2Beans/Application/CoffeeShops/Queries/Autocomplete/AutocompleteHandler.cs`).
- **Data integrity/idempotency risk (high):** create flow does not enforce uniqueness (e.g., by `PlaceId`) and has no idempotency key support; duplicate records are possible (`Bike2Beans/Application/CoffeeShops/Commands/Create/CreateCoffeeShopHandler.cs`, `Bike2Beans/Data/CoffeeShopRepository.cs`).
- **Security exposure risk (high):** all endpoints are public; no authN/authZ or per-client rate controls in middleware (`Bike2Beans/Program.cs`).
- **External dependency resilience risk (medium):** Google calls lack explicit timeout budget, retry policy shaping, and circuit-breaker behavior, so transient upstream issues propagate directly.
- **API contract drift risk (medium):** route naming/casing (`Api/places`, `Nearby`, `Text`, etc.) and defaults are inconsistent and not versioned, raising long-term client compatibility risk.

## 3. Cost and Performance Impact

- **Google Places cost pressure:**
  - No caching for repeated text/nearby/detail/autocomplete queries causes duplicate billable requests.
  - `SearchPlaceById` requests `photos` in field mask but current DTO omits photos, adding unnecessary payload/cost.
  - Autocomplete does not use session tokens end-to-end even though query model has `SessionToken`, reducing billing efficiency opportunities.
- **Backend latency/perf pressure:**
  - All external lookups are pass-through with no cache layer or stale-while-revalidate path.
  - `GetAllAsync` returns full collection with no pagination, risking memory and response growth as data scales.
- **Operational cost pressure:**
  - No guardrails (max page size/radius/result caps) means users can trigger high-cost queries.

## 4. Recommended Direction

- Introduce a **contract-first v1 API boundary** with validation and stable response semantics.
- Add **input guardrails** immediately (caps for radius/max/page size, required text length, standardized error responses).
- Implement **cost controls for Google Places**:
  - Cache hot queries (text/autocomplete/details) with short TTL.
  - Use session token flow for autocomplete + follow-up detail calls.
  - Align field masks strictly to returned DTO fields.
- Strengthen **data reliability** by adding idempotent create semantics and a uniqueness constraint/index on provider place ID.
- Add **security baseline**: endpoint auth (or signed internal token if this is internal), request throttling, and per-route quotas.
- Normalize configuration keys (`MongoDBSettings`, `GooglePlaces`) across environments and fail fast with actionable startup logs.

## 5. Tradeoffs

- Adding caching and validation increases implementation complexity and introduces cache invalidation decisions, but sharply reduces Google API spend and p95 latency.
- Enforcing auth/rate limits can slow client integration initially, but materially lowers abuse risk and uncontrolled cost growth.
- Idempotency + uniqueness may require migration/backfill for existing duplicates, but prevents persistent data drift.
- API versioning adds maintenance overhead, but protects consumers from breaking changes as contracts evolve.

## 6. Implementation Plan (small, testable steps)

1. **Config consistency fix**
- Unify config section names (`MongoDBSettings`, `GooglePlaces`) in all appsettings files.
- Add startup checks that log missing required sections clearly.
- Test: app boots in Development and Production profiles without configuration exceptions.

2. **Request validation + bounds**
- Add validation for `lat/lng`, `radiusMeters`, `max`, `PageSize`, and non-empty text.
- Return `400` with structured validation payload.
- Test: controller tests for invalid/edge inputs and bound enforcement.

3. **Contract hardening**
- Make autocomplete return `[]` (not `null`) for empty query.
- Standardize route casing and add explicit API version prefix (`/api/v1/...`).
- Test: snapshot/integration tests for response shapes.

4. **Google cost controls**
- Add in-memory/distributed cache for text, nearby, details, autocomplete by normalized key + TTL.
- Remove unused `photos` from field masks unless required by DTO.
- Thread `SessionToken` through autocomplete and subsequent detail fetch path.
- Test: repeated identical requests hit cache; external call count reduced.

5. **Data integrity/idempotency**
- Add unique index on `PlaceId`; populate `PlaceId` in create path.
- Support optional `Idempotency-Key` header for create endpoint.
- Test: duplicate create attempts return deterministic response without extra inserts.

6. **Security and resilience baseline**
- Add auth middleware/policy, rate limiting, and per-endpoint timeout budgets.
- Add retry/circuit behavior around Google REST path.
- Test: unauthorized requests rejected; burst traffic throttled; transient upstream failures handled predictably.

7. **Observability for measurable impact**
- Instrument: external API call count, cache hit rate, p95 latency, and 4xx/5xx rates per route.
- Define acceptance target: reduce external calls for repeated queries by >=40% and reduce p95 for cached flows by >=30%.
- Test: metrics appear in local telemetry and are queryable by endpoint.
