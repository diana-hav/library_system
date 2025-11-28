import { useEffect, useState } from 'react';
import { getBorrowings, returnBorrowing } from '../api/api';

const BorrowingList = () => {
  const [borrowings, setBorrowings] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [statusFilter, setStatusFilter] = useState('all');
  const [titleFilter, setTitleFilter] = useState('');
  const [returningId, setReturningId] = useState(null);

  useEffect(() => {
    const fetchBorrowings = async () => {
      try {
        setLoading(true);
        const data = await getBorrowings();
        setBorrowings(data);
        setError(null);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchBorrowings();
  }, []);

  if (loading) {
    return (
      <div className="bg-white p-6 border border-gray-300 rounded-lg shadow-sm text-center">
        <div className="text-gray-600">Завантаження позичань...</div>
      </div>
    );
  }
  
  if (error) {
    return (
      <div className="bg-white p-6 border border-red-300 rounded-lg shadow-sm">
        <div className="text-red-600">Error: {error}</div>
      </div>
    );
  }

  // Apply filters client-side
  const filtered = borrowings.filter(b => {
    const statusMatch = statusFilter === 'all' ? true : (statusFilter === 'active' ? b.status === 'active' : b.status === 'returned');
    const titleMatch = !titleFilter ? true : (b.bookTitle || '').toLowerCase().includes(titleFilter.toLowerCase());
    return statusMatch && titleMatch;
  });

  return (
    <div className="bg-white p-4 md:p-6 border border-gray-300 rounded-lg shadow-sm">
      <h2 className="text-2xl font-bold mb-4 text-gray-800">Позичання</h2>
      <div className="mb-4 grid grid-cols-1 md:grid-cols-3 gap-3">
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">Фільтр за статусом</label>
          <select value={statusFilter} onChange={(e) => setStatusFilter(e.target.value)} className="w-full px-3 py-2 border border-gray-300 rounded-md">
            <option value="all">Усі</option>
            <option value="active">Активні</option>
            <option value="returned">Повернені</option>
          </select>
        </div>
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">Фільтр за назвою книги</label>
          <input type="text" value={titleFilter} onChange={(e) => setTitleFilter(e.target.value)} placeholder="Пошук за назвою..." className="w-full px-3 py-2 border border-gray-300 rounded-md" />
        </div>
        <div className="flex items-end">
          <button onClick={() => { setStatusFilter('all'); setTitleFilter(''); }} className="px-4 py-2 bg-gray-200 rounded-md">Очистити</button>
        </div>
      </div>
      <div className="overflow-x-auto">
        <table className="min-w-full border-collapse border border-gray-300">
          <thead>
            <tr className="bg-gray-100">
              <th className="border border-gray-300 px-3 py-2 text-left text-gray-700 font-semibold text-sm md:text-base">ID</th>
              <th className="border border-gray-300 px-3 py-2 text-left text-gray-700 font-semibold text-sm md:text-base">Читач</th>
              <th className="border border-gray-300 px-3 py-2 text-left text-gray-700 font-semibold text-sm md:text-base">Книга</th>
              <th className="border border-gray-300 px-3 py-2 text-left text-gray-700 font-semibold text-sm md:text-base">Дата взяття</th>
              <th className="border border-gray-300 px-3 py-2 text-left text-gray-700 font-semibold text-sm md:text-base">Дата повернення</th>
              <th className="border border-gray-300 px-3 py-2 text-left text-gray-700 font-semibold text-sm md:text-base">Статус</th>
            </tr>
          </thead>
          <tbody>
            {filtered.length === 0 ? (
              <tr>
                <td colSpan="6" className="border border-gray-300 px-4 py-4 text-center text-gray-500">
                  Записів не знайдено
                </td>
              </tr>
            ) : (
              filtered.map((borrowing) => (
                <tr key={borrowing.id} className="hover:bg-gray-50">
                  <td className="border border-gray-300 px-3 py-2 text-gray-900 text-sm md:text-base">{borrowing.id}</td>
                  <td className="border border-gray-300 px-3 py-2 text-gray-900 text-sm md:text-base">{borrowing.readerName || 'Н/Д'}</td>
                  <td className="border border-gray-300 px-3 py-2 text-gray-900 text-sm md:text-base">{borrowing.bookTitle || 'Н/Д'}</td>
                  <td className="border border-gray-300 px-3 py-2 text-gray-900 text-sm md:text-base">
                    {borrowing.borrowDate
                      ? new Date(borrowing.borrowDate).toLocaleDateString()
                      : 'Н/Д'}
                  </td>
                  <td className="border border-gray-300 px-3 py-2 text-gray-900 text-sm md:text-base">
                    {borrowing.returnDate
                      ? new Date(borrowing.returnDate).toLocaleDateString()
                      : 'Не повернено'}
                  </td>
                  <td className="border border-gray-300 px-3 py-2 text-sm md:text-base">
                    <div className="flex items-center gap-2">
                      <span className={`px-2 py-1 rounded text-xs font-medium ${
                        borrowing.status === 'active' 
                          ? 'bg-blue-100 text-blue-800' 
                          : borrowing.status === 'returned'
                          ? 'bg-green-100 text-green-800'
                          : 'bg-gray-100 text-gray-800'
                      }`}>
                        {borrowing.status === 'active' ? 'Активна' : borrowing.status === 'returned' ? 'Повернено' : (borrowing.status || 'Н/Д')}
                      </span>
                      {borrowing.status === 'active' && (
                        <button
                          onClick={async () => {
                            if (!window.confirm('Ви впевнені, що хочете повернути цю книгу?')) return;
                            try {
                              setReturningId(borrowing.id);
                              await returnBorrowing(borrowing.id);
                              // refresh
                              const data = await getBorrowings();
                              setBorrowings(data);
                            } catch (err) {
                              setError(err.message);
                            } finally {
                              setReturningId(null);
                            }
                          }}
                          disabled={returningId === borrowing.id}
                          className="px-2 py-1 bg-green-500 hover:bg-green-600 text-white rounded text-xs"
                        >
                          {returningId === borrowing.id ? 'Повертається...' : 'Повернути'}
                        </button>
                      )}
                    </div>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default BorrowingList;

