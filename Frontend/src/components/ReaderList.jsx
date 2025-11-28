import { useEffect, useState } from 'react';
import { getReaders } from '../api/api';

const ReaderList = () => {
  const [readers, setReaders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchReaders = async () => {
      try {
        setLoading(true);
        const data = await getReaders();
        setReaders(data);
        setError(null);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchReaders();
  }, []);

  if (loading) {
    return (
      <div className="bg-white p-6 border border-gray-300 rounded-lg shadow-sm text-center">
        <div className="text-gray-600">Завантаження читачів...</div>
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

  return (
    <div className="bg-white p-4 md:p-6 border border-gray-300 rounded-lg shadow-sm">
      <h2 className="text-2xl font-bold mb-4 text-gray-800">Читачі</h2>
      <div className="overflow-x-auto">
        <table className="min-w-full border-collapse border border-gray-300">
          <thead>
            <tr className="bg-gray-100">
              <th className="border border-gray-300 px-4 py-2 text-left text-gray-700 font-semibold">ID</th>
              <th className="border border-gray-300 px-4 py-2 text-left text-gray-700 font-semibold">ПІБ</th>
              <th className="border border-gray-300 px-4 py-2 text-left text-gray-700 font-semibold">Ел. пошта</th>
            </tr>
          </thead>
          <tbody>
            {readers.length === 0 ? (
              <tr>
                  <td colSpan="3" className="border border-gray-300 px-4 py-4 text-center text-gray-500">
                  Читачів не знайдено
                </td>
              </tr>
            ) : (
              readers.map((reader) => (
                <tr key={reader.id} className="hover:bg-gray-50">
                  <td className="border border-gray-300 px-4 py-2 text-gray-900">{reader.id}</td>
                  <td className="border border-gray-300 px-4 py-2 text-gray-900">{reader.fullName}</td>
                  <td className="border border-gray-300 px-4 py-2 text-gray-900">{reader.email}</td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default ReaderList;

