# Frontend Guideline: Catch Up Web App to Backend (Mongo-First)

## 1. Current Flow Analysis

- Current web UI is mostly scaffolding (`/login`, `/map`, `/coffeeshop`) with no backend integration yet.
- Backend already exposes two useful surfaces:
  - Mongo-backed core CRUD/list: `GET /api/coffeeshops`, `POST /api/coffeeshops`
  - Google-backed discovery/search: `GET /Api/places/Nearby|Text|Id|Autocomplete`
- Existing DTOs from backend already match web map/list needs:
  - `id`, `name`, `address`, `lat`, `lng`, `rating`, `userRatingsTotal`
- Important setup note for local dev:
  - `Program.cs` binds `MongoDBSettings`, while `appsettings.Development.json` currently uses `MongoDB` key.
  - For reliable local Mongo behavior, align the dev config key to `MongoDBSettings`.

## 2. Performance Bottlenecks

- If frontend directly calls Google search on every keystroke/pan, API cost and latency will spike during development.
- No local cache or persistent seed path yet, so repeated map interactions can trigger redundant remote lookups.
- No UI state model exists for loading/error/empty states, which causes unstable map/list UX once real network calls begin.
- No request dedupe/debounce policy yet for typeahead and viewport changes.

## 3. Architectural Improvements

- Use a Mongo-first data strategy for web bootstrap:
  - Primary read path: `GET /api/coffeeshops`
  - Primary write path (manual seeding/admin): `POST /api/coffeeshops`
  - Google API only as explicit fallback action (user clicks “Search more from Google”).
- Frontend architecture baseline (web first):
  - `src/features/coffeeShops/api/*` for backend clients and DTO mapping
  - `src/features/coffeeShops/state/*` for query, filters, selected shop, fetch status
  - `src/features/map/*` for map rendering and viewport sync
  - `src/features/list/*` for list pane rendering + selection behavior
- Define a strict data contract in frontend:
  - One normalized `CoffeeShop` type used by map markers and list rows
  - Adapter layer maps backend DTOs to this type once at API boundary
- Add request policies early:
  - search debounce (300-500ms)
  - in-flight request cancellation on new query
  - client-side memoized filtering for local Mongo dataset

## 4. Mobile Readiness Notes

- Keep all domain/state logic platform-neutral now so web work is reusable later:
  - shared types/selectors/store in `features/*`
  - web renderer in `.web.tsx` components
  - reserve `.native.tsx` components for future mobile map/list UIs
- Avoid web-specific assumptions in data/state layer (URL parsing, DOM-only APIs).
- Add explicit state machine statuses (`idle|loading|success|error`) to support both web and native views consistently.

## 5. Tradeoffs and Rollout Plan

1. Deliverable: API Contract + Typed Client
- Build typed frontend clients for `GET /api/coffeeshops`, `POST /api/coffeeshops`, and optional Google endpoints.
- Output: `coffeeShopApi.ts` + runtime response guards.

2. Deliverable: Mongo-First Store and Screens
- Wire map/list pages to Mongo-backed `GET /api/coffeeshops`.
- Output: initial working list + markers from local Mongo data.

3. Deliverable: Seed Workflow for Dev Data
- Add a small script/page flow to insert starter shops through `POST /api/coffeeshops`.
- Output: repeatable local dataset without Google calls.

4. Deliverable: Search and Sync Behavior
- Implement search box that filters local Mongo dataset first.
- Sync list selection <-> map focus/marker highlight.
- Output: deterministic search-to-map behavior on web.

5. Deliverable: Controlled Google Fallback
- Add explicit button/feature flag to call Google text/nearby endpoints only when needed.
- Persist selected Google results into Mongo to reduce repeat external calls.
- Output: cost-controlled expansion path.

6. Deliverable: Error/Loading UX + Observability
- Add loading skeletons, empty state, error retry banners.
- Track API call counts and response times per endpoint in dev logs.
- Output: measurable baseline before scaling.

7. Deliverable: Stabilization and PR Gate
- Exit criteria before broad feature work:
  - web map+list fully powered by Mongo data
  - no automatic Google calls during normal browsing
  - stable loading/error/empty states
  - basic render/API metrics captured
