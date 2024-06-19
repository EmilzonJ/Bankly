export type Customer = {
  id: string;
  name: string;
  email: string;
  registeredAt: Date;
};

export type CustomerParams = {
  name?: string;
  email?: string;
  registeredAt?: string;
};

export type CreateCustomer = {
  name: string;
  email: string;
};

export type UpdateCustomer = {
  id: string;
} & CreateCustomer;
