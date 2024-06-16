export type Paginated<T> = {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
};

export type PaginatedParams = {
  pageNumber: number;
  pageSize: number;
}
