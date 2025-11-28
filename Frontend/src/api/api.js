const API_BASE_URL = '/api'; // Use relative URL to leverage Vite proxy

// Safely parse JSON responses. Handles empty responses (204 No Content)
const parseJsonSafe = async (response) => {
  const text = await response.text();
  if (!text) return null;
  try {
    return JSON.parse(text);
  } catch (err) {
    return { _raw: text };
  }
};

// --- Readers API ---
export const getReaders = async () => {
  const response = await fetch(`${API_BASE_URL}/readers`);
  if (!response.ok) throw new Error('Failed to fetch readers');
  return parseJsonSafe(response);
};

export const createReader = async (readerData) => {
  const response = await fetch(`${API_BASE_URL}/readers`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(readerData),
  });
  if (!response.ok) throw new Error('Failed to create reader');
  return parseJsonSafe(response);
};

// --- Borrowings API ---
export const getBorrowings = async () => {
  const response = await fetch(`${API_BASE_URL}/borrowings`);
  if (!response.ok) throw new Error('Failed to fetch borrowings');
  return parseJsonSafe(response);
};

export const returnBorrowing = async (id) => {
  const response = await fetch(`${API_BASE_URL}/borrowings/return/${id}`, {
    method: 'PUT',
  });
  if (!response.ok) {
    const errorData = await parseJsonSafe(response).catch(() => ({}));
    throw new Error((errorData && errorData.error) || 'Failed to return borrowing');
  }
  return parseJsonSafe(response);
};

export const createBorrowing = async (borrowingData) => {
  const response = await fetch(`${API_BASE_URL}/borrowings/issue`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      readerId: parseInt(borrowingData.readerId),
      bookId: parseInt(borrowingData.bookId),
    }),
  });
  if (!response.ok) {
    const errorData = await parseJsonSafe(response).catch(() => ({}));
    throw new Error((errorData && errorData.error) || 'Failed to create borrowing');
  }
  return parseJsonSafe(response);
};

// --- Books API (BorrowingService) ---
export const getBooks = async () => {
  console.log("[DEBUG] Fetching books from BorrowingService");
  const response = await fetch(`${API_BASE_URL}/books`);
  if (!response.ok) throw new Error('Failed to fetch books');
  return parseJsonSafe(response);
};

// --- Catalog API ---
const CATALOG_API_BASE_URL = '/catalog-api';

export const getCatalogBooks = async (params = {}) => {
  const query = new URLSearchParams({
    page: params.page ?? 1,
    pageSize: params.pageSize ?? 10,
    title: params.title ?? "",
    authorId: params.authorId ?? "",
    genreId: params.genreId ?? "",
    sortBy: params.sortBy ?? "title",
    sortDir: params.sortDir ?? "asc",
  });

  console.log("[DEBUG] Fetching catalog books paged:", query.toString());

  const response = await fetch(`http://localhost:5180/api/books/paged?${query}`);
  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to fetch catalog books: ${response.status} ${response.statusText}. ${errorText}`);
  }
  return parseJsonSafe(response);
};

export const getCatalogBook = async (id) => {
  const response = await fetch(`${CATALOG_API_BASE_URL}/api/books/${id}`);
  if (!response.ok) throw new Error('Failed to fetch book');
  return parseJsonSafe(response);
};

export const createCatalogBook = async (bookData) => {
  const response = await fetch(`${CATALOG_API_BASE_URL}/api/books`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(bookData),
  });
  if (!response.ok) {
    const errorData = await parseJsonSafe(response).catch(() => ({}));
    throw new Error((errorData && errorData.error) || 'Failed to create book');
  }
  return parseJsonSafe(response);
};

export const updateCatalogBook = async (id, bookData) => {
  const response = await fetch(`${CATALOG_API_BASE_URL}/api/books/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(bookData),
  });
  if (!response.ok) {
    const errorData = await parseJsonSafe(response).catch(() => ({}));
    throw new Error((errorData && errorData.error) || 'Failed to update book');
  }
  return parseJsonSafe(response);
};

export const deleteCatalogBook = async (id) => {
  const response = await fetch(`${CATALOG_API_BASE_URL}/api/books/${id}`, {
    method: 'DELETE',
  });
  if (!response.ok) {
    const errorData = await parseJsonSafe(response).catch(() => ({}));
    throw new Error((errorData && errorData.error) || 'Failed to delete book');
  }
};

// --- Authors & Genres ---
export const getAuthors = async () => {
  const response = await fetch(`${CATALOG_API_BASE_URL}/api/authors`);
  if (!response.ok) throw new Error('Failed to fetch authors');
  return parseJsonSafe(response);
};

export const getGenres = async () => {
  const response = await fetch(`${CATALOG_API_BASE_URL}/api/genres`);
  if (!response.ok) throw new Error('Failed to fetch genres');
  return parseJsonSafe(response);
};
