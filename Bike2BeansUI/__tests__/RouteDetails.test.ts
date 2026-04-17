import { normalizeSavedRoutePreview } from "../src/Api/RouteDetails";

describe("normalizeSavedRoutePreview", () => {
  test("maps backend RouteDetailsDto casing to the web preview shape", () => {
    expect(
      normalizeSavedRoutePreview({
        Id: "route-123",
        Name: "Morning Loop",
      })
    ).toEqual({
      id: "route-123",
      name: "Morning Loop",
    });
  });

  test("returns null when the backend payload does not include an id", () => {
    expect(
      normalizeSavedRoutePreview({
        Name: "Nameless Route",
      })
    ).toBeNull();
  });
});
