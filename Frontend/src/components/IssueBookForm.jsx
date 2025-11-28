import { useState, useEffect } from 'react';
import { createBorrowing, getReaders, getBooks } from '../api/api';

const IssueBookForm = ({ onBorrowingCreated }) => {
  const [formData, setFormData] = useState({
    readerId: '',
    bookId: '',
  });
  const [readers, setReaders] = useState([]);
  const [books, setBooks] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(false);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [readersData, booksData] = await Promise.all([
          getReaders(),
          getBooks(),
        ]);
        setReaders(readersData);
        setBooks(booksData);
      } catch (err) {
        setError(err.message);
      }
    };
    fetchData();
  }, []);

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    setSuccess(false);

    try {
      await createBorrowing(formData);
      setSuccess(true);
      setFormData({
        readerId: '',
        bookId: '',
      });
      if (onBorrowingCreated) onBorrowingCreated();
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="bg-white p-4 md:p-6 border border-gray-300 rounded-lg shadow-sm max-w-2xl mx-auto">
      <h2 className="text-2xl font-bold mb-4 text-gray-800">Видача книги</h2>
      {error && (
        <div className="mb-4 p-3 bg-red-50 border border-red-200 text-red-700 rounded">
          {error}
        </div>
      )}
      {success && (
        <div className="mb-4 p-3 bg-green-50 border border-green-200 text-green-700 rounded">
          Книгу видано успішно!
        </div>
      )}
      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label className="block mb-2 font-medium text-gray-700">Reader</label>
          <select
            name="readerId"
            value={formData.readerId}
            onChange={handleChange}
            required
            className="w-full px-3 py-2 border border-gray-300 rounded-md bg-white text-gray-900 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
            disabled={loading}
          >
            <option value="">Оберіть читача</option>
            {readers.map((reader) => (
              <option key={reader.id} value={reader.id}>
                {reader.fullName || reader.name}
              </option>
            ))}
          </select>
        </div>
        <div>
          <label className="block mb-2 font-medium text-gray-700">Book</label>
          <select
            name="bookId"
            value={formData.bookId}
            onChange={handleChange}
            required
            className="w-full px-3 py-2 border border-gray-300 rounded-md bg-white text-gray-900 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
            disabled={loading}
          >
            <option value="">Оберіть книгу</option>
            {books.map((book) => (
              <option key={book.id} value={book.id}>
                {book.title} — {book.author}
              </option>
            ))}
          </select>
        </div>
        <button
          type="submit"
          disabled={loading}
          className="w-full md:w-auto px-6 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 disabled:bg-gray-400 disabled:cursor-not-allowed transition-colors font-medium"
        >
          {loading ? 'Видається...' : 'Видати книгу'}
        </button>
      </form>
    </div>
  );
};

export default IssueBookForm;

