# Frontend Engineering Report: Map Flow and Cross-Platform Readiness

## 1. Current Flow Analysis

- Web entrypoint (`src/index.tsx`) mounts `NavBar`, a static `Hello World`, then `AppRoutes`; routing is browser-driven via `BrowserRouter`.
- Route table (`src/Navigation/AppRoutes.tsx`) defines `/login`, `/map`, `/coffeeshop`, and fallback `* -> Login`.
- Navigation links (`src/Components/NavBar.tsx`) use mixed-case paths (`/Login`, `/Map`, `/CoffeeShops`) that do not match route definitions exactly.
- Map page (`src/Pages/Map.tsx`) renders a static Leaflet map centered on Boston with a base tile layer only.
- Coffee shop page (`src/Pages/CoffeeShops.tsx`) and login page (`src/Pages/Login.tsx`) are placeholders and currently do not participate in map-state workflow.
- Data exists as static seed objects (`src/Data/coffeeshops.ts`) but is not wired into map markers, list views, or URL state.
- Native entry (`index.js -> App.tsx`) still renders the React Native template screen and does not share the web route/map flow.

## 2. Performance Bottlenecks

- No marker virtualization or clustering path exists today; scaling beyond trivial point counts will force full marker render/update on pan/zoom.
- No route drawing pipeline (e.g., polyline simplification or segmentation) exists; adding naive route overlays later risks frame drops under interaction.
- Search-to-map synchronization is absent, which typically leads to duplicate fetch/render triggers once search is introduced.
- Leaflet CSS is imported twice in `Map.tsx`, adding avoidable bundle and parse overhead.
- `Map.tsx` imports `Marker`/`Popup` but does not use them, introducing dead import overhead and signaling unfinished render paths.
- `webpack.config.js` uses `ts-loader` with `transpileOnly: true`; this speeds builds but hides type regressions that can ship to runtime.
- Route/path casing mismatch in navigation can cause avoidable extra redirects/fallback renders and brittle behavior across environments.

## 3. Architectural Improvements

- Introduce a shared `MapFeature` module boundary:
  - `map-state` (viewport, selected shop, active filters/search query)
  - `map-data` (shop normalization, memoized geo-index)
  - `map-render` (tiles, markers, route overlay layers)
- Establish a single source of truth for map/list/search state (URL params on web + feature store abstraction for native/web parity).
- Add selector-based derivations (`visibleShops`, `selectedShop`, `displayRoute`) to prevent broad rerenders.
- Implement progressive map rendering strategy:
  - Phase A: clustered markers at low zoom
  - Phase B: expanded markers/details after zoom threshold
  - Phase C: route overlay only for active selection
- Add explicit async UI state contracts for map data and directions:
  - loading skeletons for map/list regions
  - recoverable error banners with retry actions
  - empty-state handling for zero results and out-of-bounds search
- Normalize route and link paths to lowercase constants to remove casing drift.

## 4. Mobile Readiness Notes

- Current architecture is bifurcated: native runtime uses `App.tsx` template UI while map flow lives only in web `src/` pages.
- `react-leaflet` is web-only; equivalent native map capability is not yet integrated, so parity is currently not achievable.
- To support React Native + web with one feature model:
  - move domain/state logic into platform-agnostic modules (`src/features/map/*`)
  - keep renderer adapters separate (`MapView.web.tsx` vs `MapView.native.tsx`)
  - standardize navigation contracts (screen params equivalent to web URL state)
- Safe-area handling exists in native template, but no map screen layout system is implemented for touch interactions, bottom sheets, or gesture conflict handling.
- No mobile-specific performance guardrails are present yet (frame budget monitoring, interaction throttling, low-end device testing).

## 5. Tradeoffs and Rollout Plan

1. Foundation (low risk, immediate)
- Unify route casing and link constants.
- Remove duplicate/dead imports.
- Define shared TypeScript models for shops, viewport, and route data.
- Tradeoff: little user-visible change, but strong reduction in future integration risk.

2. State and Sync Layer (medium risk)
- Implement centralized map/search state with deterministic URL synchronization on web.
- Add memoized selectors and render boundaries.
- Tradeoff: moderate refactor cost, enables predictable behavior and perf tuning.

3. Rendering Performance (medium-high risk)
- Add marker clustering and viewport-based filtering.
- Introduce route drawing pipeline with geometry simplification and controlled update cadence.
- Tradeoff: added complexity and dependency surface, major scalability gain.

4. Cross-Platform Adapter Split (high impact)
- Extract shared feature logic and build web/native renderer adapters.
- Replace native template screen with first map/list shell using shared state contracts.
- Tradeoff: initial velocity dip, unlocks true React Native + web parity.

5. Validation and Evidence
- Capture before/after metrics per step:
  - initial map render time
  - interaction FPS / dropped frames during pan+zoom
  - marker update count per viewport change
  - API/data call count per search interaction
- Gate rollout with simple thresholds (no regression on render time; reduced redundant updates).
