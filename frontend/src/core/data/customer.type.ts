export type Customer = {
    id: string,
    name: string,
    email: string,
    registeredAt: Date
}

export type CustomerParams = {
    name?: string,
    email?: string,
    registeredAt?: string
}