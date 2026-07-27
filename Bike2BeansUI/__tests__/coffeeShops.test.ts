import { toCoffeeShopList } from "../src/Api/coffeeShops";

const shop = {
    id: "1",
    placeId: "place-1",
    name: "Test Cafe",
    rating: 4.5,
    address: "1 Test St",
    userRatingsTotal: 10,
    lat: 47.6,
    lng: -122.3,
};

describe("toCoffeeShopList", () => {
    test("keeps array responses and unwraps paginated location responses", () => {
        expect(toCoffeeShopList([shop])).toEqual([shop]);
        expect(toCoffeeShopList({ locations: [shop] })).toEqual([shop]);
        expect(toCoffeeShopList({ Locations: [shop] })).toEqual([shop]);
    });

    test("returns an empty list for non-list responses", () => {
        expect(toCoffeeShopList({ error: "not ready" })).toEqual([]);
        expect(toCoffeeShopList("<html />")).toEqual([]);
    });
});
