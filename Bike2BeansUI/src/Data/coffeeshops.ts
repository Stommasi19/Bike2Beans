export type CoffeeShop = {
  id: string;
  name: string;
  rating: number;
  lat: number;
  lng: number;
};

export const coffeeShops: CoffeeShop[] = [
  { id: "1", name: "Victrola Coffee", rating: 4.6, lat: 47.6145, lng: -122.3191 },
  { id: "2", name: "Anchorhead Coffee", rating: 4.5, lat: 47.6163, lng: -122.3355 },
  { id: "3", name: "Espresso Vivace", rating: 4.7, lat: 47.6236, lng: -122.3382 },
];