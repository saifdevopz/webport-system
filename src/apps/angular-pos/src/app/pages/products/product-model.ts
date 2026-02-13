export type Product = {
  id: number;
  title: string;
  price: number;
  description: string;
  category: string;
  image: string;
  rating: Rating;
};

export type WooProduct = {
  id: number;
  name: string;
  description: string;
  price: number;
};

export type Rating = {
  rate: number;
  count: number;
};
