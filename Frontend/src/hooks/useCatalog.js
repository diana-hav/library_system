// hooks/useCatalog.js
import { useState, useEffect, useCallback, useRef } from "react";
import {
  getCatalogBooks,
  createCatalogBook,
  updateCatalogBook,
  deleteCatalogBook,
  getAuthors,
  getGenres,
  getReaders,
  createBorrowing,
} from "../api/api";

function useDebounced(value, delay = 400) {
  const [debounced, setDebounced] = useState(value);
  useEffect(() => {
    const t = setTimeout(() => setDebounced(value), delay);
    return () => clearTimeout(t);
  }, [value, delay]);
  return debounced;
}

export default function useCatalog(initial = {}) {
  // --- state ---
  const [books, setBooks] = useState([]);
  const [authors, setAuthors] = useState([]);
  const [genres, setGenres] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const [page, setPage] = useState(initial.page || 1);
  const [pageSize, setPageSize] = useState(initial.pageSize || 10);
  const [totalCount, setTotalCount] = useState(0);
  const [totalPages, setTotalPages] = useState(1);
  const [sortBy, setSortBy] = useState(initial.sortBy || "title");
  const [sortDir, setSortDir] = useState(initial.sortDir || "asc");

  const [filterTitle, setFilterTitle] = useState("");
  const [filterAuthor, setFilterAuthor] = useState("");
  const [filterGenre, setFilterGenre] = useState("");

  const debouncedTitle = useDebounced(filterTitle, 400);
  const requestIdRef = useRef(0);

  const [formState, setFormState] = useState({
    showForm: false,
    editing: null,
    data: { title: "", authorId: "", genreId: "" },
  });

  const [issueState, setIssueState] = useState({
    open: false,
    loading: false,
    error: null,
    success: false,
    book: null,
    readers: [],
    readerId: "",
  });

  // --- load authors / genres once ---
  const loadAuthors = useCallback(async () => {
    try {
      const a = await getAuthors();
      setAuthors(a || []);
    } catch (err) {
      console.warn("Failed to load authors:", err);
    }
  }, []);

  const loadGenres = useCallback(async () => {
    try {
      const g = await getGenres();
      setGenres(g || []);
    } catch (err) {
      console.warn("Failed to load genres:", err);
    }
  }, []);

  useEffect(() => {
    loadAuthors();
    loadGenres();
  }, [loadAuthors, loadGenres]);

  // --- load books with full params ---
  const loadBooks = useCallback(
    async (opts = {}) => {
      const reqId = ++requestIdRef.current;
      setLoading(true);
      setError(null);

      const params = {
        page: opts.page ?? page,
        pageSize: opts.pageSize ?? pageSize,
        title: opts.title ?? debouncedTitle,
        authorId: (opts.authorId ?? filterAuthor) || undefined,
        genreId: (opts.genreId ?? filterGenre) || undefined,
        sortBy: opts.sortBy ?? sortBy,
        sortDir: opts.sortDir ?? sortDir,
      };

      try {
        const data = await getCatalogBooks(params);
        if (reqId !== requestIdRef.current) return;

        setBooks(data.items || []);
        setPage(data.page || params.page);
        setPageSize(data.pageSize || params.pageSize);
        setTotalCount(data.totalCount ?? (data.items?.length || 0));
        setTotalPages(data.totalPages ?? Math.max(1, Math.ceil((data.totalCount || (data.items?.length || 0)) / (params.pageSize || 1))));
      } catch (err) {
        setError(err.message || "Не вдалося завантажити книги");
      } finally {
        setLoading(false);
      }
    },
    [debouncedTitle, filterAuthor, filterGenre, page, pageSize, sortBy, sortDir]
  );

  // --- initial load ---
  useEffect(() => {
    loadBooks({ page, pageSize });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  // --- reload on filters or sort change ---
  useEffect(() => {
    loadBooks({
      page: 1,
      pageSize,
      title: debouncedTitle,
      authorId: filterAuthor || undefined,
      genreId: filterGenre || undefined,
      sortBy,
      sortDir,
    });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [debouncedTitle, filterAuthor, filterGenre, sortBy, sortDir]);

  // --- reload on page / pageSize change ---
  useEffect(() => {
    loadBooks({ page, pageSize });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [page, pageSize]);

  // --- form handlers ---
  const openCreateForm = () =>
    setFormState({ showForm: true, editing: null, data: { title: "", authorId: "", genreId: "" } });

  const openEditForm = (book) =>
    setFormState({
      showForm: true,
      editing: book,
      data: { title: book.title, authorId: String(book.authorId), genreId: String(book.genreId) },
    });

  const closeForm = () =>
    setFormState({ showForm: false, editing: null, data: { title: "", authorId: "", genreId: "" } });

  const setFormField = (name, value) =>
    setFormState((s) => ({ ...s, data: { ...s.data, [name]: value } }));

  const submitForm = async () => {
    const dto = {
      title: formState.data.title.trim(),
      authorId: parseInt(formState.data.authorId, 10),
      genreId: parseInt(formState.data.genreId, 10),
    };
    if (!dto.title || !dto.authorId || !dto.genreId) throw new Error("Заповніть всі обов'язкові поля");

    setLoading(true);
    setError(null);
    try {
      if (formState.editing) {
        await updateCatalogBook(formState.editing.id, dto);
      } else {
        await createCatalogBook(dto);
      }
      await loadBooks({ page });
      closeForm();
    } catch (err) {
      setError(err.message || "Помилка при збереженні");
      throw err;
    } finally {
      setLoading(false);
    }
  };

  const removeBook = async (id) => {
    if (!window.confirm("Ви впевнені, що хочете видалити цю книгу?")) return;
    setLoading(true);
    setError(null);
    try {
      await deleteCatalogBook(id);
      const newPage = Math.max(1, page);
      await loadBooks({ page: newPage, pageSize });
    } catch (err) {
      setError(err.message || "Помилка при видаленні");
    } finally {
      setLoading(false);
    }
  };

  // --- issue handlers ---
  const openIssue = async (book) => {
    setIssueState({ open: true, loading: false, error: null, success: false, book, readers: [], readerId: "" });
    try {
      const readers = await getReaders();
      setIssueState((s) => ({ ...s, readers: readers || [] }));
    } catch (err) {
      setIssueState((s) => ({ ...s, error: err.message || "Не вдалося завантажити читачів" }));
    }
  };

  const closeIssue = () =>
    setIssueState({ open: false, loading: false, error: null, success: false, book: null, readers: [], readerId: "" });

  const setIssueField = (name, value) => setIssueState((s) => ({ ...s, [name]: value }));

  const submitIssue = async () => {
    if (!issueState.book || !issueState.readerId) {
      setIssueState((s) => ({ ...s, error: "Оберіть читача" }));
      return;
    }
    setIssueState((s) => ({ ...s, loading: true, error: null, success: false }));
    try {
      await createBorrowing({ readerId: parseInt(issueState.readerId, 10), bookId: parseInt(issueState.book.id, 10) });
      setIssueState((s) => ({ ...s, loading: false, success: true }));
      await loadBooks({ page });
      setTimeout(closeIssue, 800);
    } catch (err) {
      setIssueState((s) => ({ ...s, loading: false, error: err.message || "Не вдалося видати книгу" }));
    }
  };

  // --- sorting toggle ---
  const toggleSort = (col) => {
    if (sortBy === col) setSortDir((d) => (d === "asc" ? "desc" : "asc"));
    else {
      setSortBy(col);
      setSortDir("asc");
    }
  };

  const reload = useCallback(() => loadBooks({ page, pageSize }), [loadBooks, page, pageSize]);

  return {
    books,
    authors,
    genres,
    loading,
    error,
    page,
    pageSize,
    totalCount,
    totalPages,
    sortBy,
    sortDir,
    filterTitle,
    filterAuthor,
    filterGenre,
    setPage,
    setPageSize,
    setFilterTitle,
    setFilterAuthor,
    setFilterGenre,
    toggleSort,
    formState,
    openCreateForm,
    openEditForm,
    closeForm,
    setFormField,
    submitForm,
    removeBook,
    issueState,
    openIssue,
    closeIssue,
    setIssueField,
    submitIssue,
    reload,
  };
}
