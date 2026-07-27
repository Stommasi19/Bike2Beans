# Production Setup

Bike2Beans has two deployable services:

- `Bike2BeansUI`: React web app built once with webpack and served from `dist`.
- `Bike2Beans/Api`: ASP.NET Core API that talks to Firebase, Google Places, Mapbox, and MongoDB.

## Web Service

Use `Bike2BeansUI` as the Railway service root.

Required Railway settings:

- Build command: `npm run build`
- Start command from repo root: `npm start`
- Start command from `Bike2BeansUI`: `npm run start:web`
- Healthcheck path: `/health`
- Public networking target port: match the app log, usually `8080`

Required environment variables at build time:

- `API_BASE_URL`: public HTTPS URL for the API service, for example `https://your-api-service.up.railway.app`
- `MAPBOX_ACCESS_TOKEN`: public Mapbox token used by the browser map

The production build intentionally fails if either variable is missing. Without `API_BASE_URL`, the browser bundle falls back to the local development API URL and breaks for real users.

## API Service

Recommended Railway settings:

- Service root: `Bike2Beans`
- Build with `Dockerfile`
- Healthcheck path: `/health`
- Public networking target port: match the app log, usually `8080`

Required environment variables:

- `ASPNETCORE_ENVIRONMENT=Production`
- `CORS_ALLOWED_ORIGINS=https://your-web-service.up.railway.app`
- `FIREBASE_PROJECT_ID=bike2beans-1d091`
- `FIREBASE_ADMIN_SERVICE_ACCOUNT_JSON={...}`
- `GOOGLE_PLACES_API_KEY=...`
- `MAPBOX_ACCESS_TOKEN=...`
- `MongoDBSettings__ConnectionString=...`
- `MongoDBSettings__DatabaseName=bike2beans`

## Common Failure Modes

- `Application failed to respond`: the app is not listening on the public networking target port, or Railway is targeting the wrong port.
- Web deploy succeeds but app calls `localhost:5165`: `API_BASE_URL` was missing when the web bundle was built.
- Browser API calls fail with CORS errors: `CORS_ALLOWED_ORIGINS` does not include the exact web origin.
- API starts but data calls fail: MongoDB is still using a local connection string or the production connection string is missing.
