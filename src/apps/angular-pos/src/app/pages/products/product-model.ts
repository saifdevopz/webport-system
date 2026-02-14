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
  title: string;
  name: string;
  description: string;
  price: number;
  category: string;
  image: string;
  rating: Rating;
};

export type Rating = {
  rate: number;
  count: number;
};
