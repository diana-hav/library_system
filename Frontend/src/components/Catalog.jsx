import React, { useEffect, useRef } from "react";
import useCatalog from "../hooks/useCatalog";

const Spinner = () => (
  <div className="inline-block w-4 h-4 border-2 border-current border-t-transparent rounded-full animate-spin" />
);

export default function Catalog() {
  const {
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
  } = useCatalog();

  const modalRef = useRef(null);

  useEffect(() => {
    if (issueState.open && modalRef.current) {
      modalRef.current.querySelector("select")?.focus();
    }
  }, [issueState.open]);

  return (
    <div className="bg-white p-4 md:p-6 border border-gray-300 rounded-lg shadow-sm">
      <div className="flex justify-between items-center mb-4">
        <h2 className="text-2xl font-bold text-gray-800">Каталог книг</h2>
        <div className="flex items-center gap-2">
          <button
            onClick={openCreateForm}
            className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg font-medium"
          >
            Додати книгу
          </button>
          <button
            onClick={() => reload()}
            className="bg-gray-200 hover:bg-gray-300 text-gray-800 px-3 py-2 rounded"
            title="Оновити"
            disabled={loading}
          >
            {loading ? <Spinner /> : "Оновити"}
          </button>
        </div>
      </div>

      {error && (
        <div className="mb-4 p-3 bg-red-100 border border-red-400 text-red-700 rounded">
          {error}
        </div>
      )}

      {/* FORM */}
      {formState.showForm && (
        <div className="mb-6 p-4 bg-gray-50 border border-gray-300 rounded-lg">
          <h3 className="text-lg font-semibold mb-3 text-gray-800">
            {formState.editing ? "Редагувати книгу" : "Додати нову книгу"}
          </h3>
          <form
            onSubmit={async (e) => {
              e.preventDefault();
              try { await submitForm(); } catch {}
            }}
            className="space-y-4"
          >
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Назва книги *</label>
              <input
                type="text"
                name="title"
                value={formState.data.title}
                onChange={(e) => setFormField("title", e.target.value)}
                required
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Введіть назву книги"
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Автор *</label>
              <select
                name="authorId"
                value={formState.data.authorId}
                onChange={(e) => setFormField("authorId", e.target.value)}
                required
                className="w-full px-3 py-2 border border-gray-300 rounded-lg"
              >
                <option value="">Оберіть автора</option>
                {authors.map((a) => (
                  <option key={a.id} value={a.id}>{a.name}</option>
                ))}
              </select>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Жанр *</label>
              <select
                name="genreId"
                value={formState.data.genreId}
                onChange={(e) => setFormField("genreId", e.target.value)}
                required
                className="w-full px-3 py-2 border border-gray-300 rounded-lg"
              >
                <option value="">Оберіть жанр</option>
                {genres.map((g) => (
                  <option key={g.id} value={g.id}>{g.name}</option>
                ))}
              </select>
            </div>

            <div className="flex gap-2">
              <button type="submit" className="bg-green-600 hover:bg-green-700 text-white px-4 py-2 rounded-lg">
                {formState.editing ? "Зберегти" : "Додати"}
              </button>
              <button type="button" onClick={closeForm} className="bg-gray-400 hover:bg-gray-500 text-white px-4 py-2 rounded-lg">
                Скасувати
              </button>
            </div>
          </form>
        </div>
      )}

      {/* Filters */}
      <div className="mb-4 p-4 bg-gray-50 border border-gray-300 rounded-lg">
        <h3 className="text-lg font-semibold mb-3 text-gray-800">Фільтри</h3>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <input
            type="text"
            value={filterTitle}
            onChange={(e) => setFilterTitle(e.target.value)}
            placeholder="Пошук за назвою..."
            className="w-full px-3 py-2 border border-gray-300 rounded-lg"
          />
          <select value={filterAuthor} onChange={(e) => setFilterAuthor(e.target.value)} className="w-full px-3 py-2 border border-gray-300 rounded-lg">
            <option value="">Всі автори</option>
            {authors.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
          </select>
          <select value={filterGenre} onChange={(e) => setFilterGenre(e.target.value)} className="w-full px-3 py-2 border border-gray-300 rounded-lg">
            <option value="">Всі жанри</option>
            {genres.map((g) => <option key={g.id} value={g.id}>{g.name}</option>)}
          </select>
        </div>
        {(filterTitle || filterAuthor || filterGenre) && (
          <button onClick={() => { setFilterTitle(""); setFilterAuthor(""); setFilterGenre(""); }} className="mt-3 text-blue-600 hover:text-blue-800 text-sm font-medium">
            Очистити фільтри
          </button>
        )}
      </div>

      {/* Table */}
      <div className="overflow-x-auto">
        <table className="min-w-full border-collapse border border-gray-300">
          <thead>
            <tr className="bg-gray-100">
              {["ID","title","author","genre"].map((col) => (
                <th
                  key={col}
                  className={`border px-4 py-2 text-left ${col !== "ID" ? "cursor-pointer" : ""}`}
                  onClick={col !== "ID" ? () => toggleSort(col) : undefined}
                >
                  {col.charAt(0).toUpperCase() + col.slice(1)} {sortBy === col && (sortDir==="asc"?"↑":"↓")}
                </th>
              ))}
              <th className="border px-4 py-2 text-left">Дії</th>
            </tr>
          </thead>
          <tbody>
            {!loading && books.length === 0 ? (
              <tr>
                <td colSpan="5" className="border px-4 py-4 text-center text-gray-500">
                  {totalCount === 0 ? "Немає книг в каталозі" : "Книги не знайдено за фільтрами"}
                </td>
              </tr>
            ) : (
              books.map((book) => (
                <tr key={book.id} className="hover:bg-gray-50">
                  <td className="border px-4 py-2">{book.id}</td>
                  <td className="border px-4 py-2">{book.title}</td>
                  <td className="border px-4 py-2">{book.authorName}</td>
                  <td className="border px-4 py-2">{book.genreName}</td>
                  <td className="border px-4 py-2 flex gap-2">
                    <button onClick={() => openEditForm(book)} className="bg-blue-500 hover:bg-blue-600 text-white px-3 py-1 rounded text-sm">Редагувати</button>
                    <button onClick={() => openIssue(book)} className="bg-amber-500 hover:bg-amber-600 text-white px-3 py-1 rounded text-sm">Взяти</button>
                    <button onClick={() => removeBook(book.id)} className="bg-red-500 hover:bg-red-600 text-white px-3 py-1 rounded text-sm">Видалити</button>
                  </td>
                </tr>
              ))
            )}
            {loading && (
              <tr>
                <td colSpan="5" className="border px-4 py-4 text-center">
                  <div className="flex items-center justify-center gap-2">
                    <Spinner /> Завантаження...
                  </div>
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>

      {/* Pagination */}
      <div className="mt-4 flex flex-col md:flex-row items-center justify-between gap-3">
        <div className="text-sm text-gray-600">
          Всього: {totalCount} — сторінка {page} з {totalPages}
        </div>
        <div className="flex items-center gap-2">
          <button onClick={() => setPage(1)} disabled={page === 1 || loading} className="px-3 py-1 bg-gray-200 rounded disabled:opacity-50">Перша</button>
          <button onClick={() => setPage(Math.max(1, page-1))} disabled={page===1||loading} className="px-3 py-1 bg-gray-200 rounded disabled:opacity-50">Назад</button>
          <div className="px-2 text-sm">{page}/{totalPages}</div>
          <button onClick={() => setPage(Math.min(totalPages, page+1))} disabled={page>=totalPages||loading} className="px-3 py-1 bg-gray-200 rounded disabled:opacity-50">Далі</button>
          <button onClick={() => setPage(totalPages)} disabled={page>=totalPages||loading} className="px-3 py-1 bg-gray-200 rounded disabled:opacity-50">Остання</button>
          <select value={pageSize} onChange={(e) => setPageSize(parseInt(e.target.value,10))} className="ml-3 px-2 py-1 border rounded">
            {[5,10,20,50].map(s=><option key={s} value={s}>{s} / стор.</option>)}
          </select>
        </div>
      </div>

      {/* Issue modal */}
      {issueState.open && (
        <div ref={modalRef} className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-40">
          <div className="bg-white p-6 rounded-lg shadow-lg w-full max-w-md">
            <h3 className="text-lg font-semibold mb-3">Взяти книгу: {issueState.book?.title}</h3>
            {issueState.error && <div className="mb-3 p-2 bg-red-50 border border-red-200 text-red-700 rounded">{issueState.error}</div>}
            {issueState.success && <div className="mb-3 p-2 bg-green-50 border border-green-200 text-green-700 rounded">Книгу успішно видано.</div>}
            <form onSubmit={async e=>{e.preventDefault(); await submitIssue();}} className="space-y-3">
              <select name="readerId" value={issueState.readerId} onChange={e=>setIssueField("readerId",e.target.value)} required className="w-full px-3 py-2 border border-gray-300 rounded-lg" disabled={issueState.loading}>
                <option value="">Оберіть читача</option>
                {issueState.readers.map(r=><option key={r.id} value={r.id}>{r.fullName||r.name}</option>)}
              </select>
              <div className="flex justify-end gap-2">
                <button type="button" onClick={closeIssue} className="px-4 py-2 bg-gray-300 rounded">Скасувати</button>
                <button type="submit" disabled={issueState.loading} className="px-4 py-2 bg-amber-500 text-white rounded">{issueState.loading ? "Видається..." : "Взяти"}</button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
