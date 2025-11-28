import { useState } from 'react';
import ReaderList from './components/ReaderList';
import BorrowingList from './components/BorrowingList';
import AddReaderForm from './components/AddReaderForm';
import Catalog from './components/Catalog';
function App() {
  const [activeTab, setActiveTab] = useState('readers');
  const [refreshKey, setRefreshKey] = useState(0);

  const handleReaderAdded = () => {
    setRefreshKey((prev) => prev + 1);
  };

  const handleBorrowingCreated = () => {
    setRefreshKey((prev) => prev + 1);
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <header className="bg-blue-600 text-white shadow-md">
        <div className="container mx-auto px-4 py-4">
          <h1 className="text-2xl md:text-3xl font-bold">Система управління бібліотекою</h1>
        </div>
      </header>

      <nav className="bg-white shadow-md sticky top-0 z-10">
        <div className="container mx-auto px-4">
          <div className="flex flex-wrap gap-2 py-2 md:py-4">
            <button
              onClick={() => setActiveTab('readers')}
              className={`px-3 py-2 text-sm md:text-base rounded transition-colors ${
                activeTab === 'readers'
                  ? 'bg-blue-500 text-white shadow-md'
                  : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
              }`}
            >
              Читачі
            </button>
            <button
              onClick={() => setActiveTab('borrowings')}
              className={`px-3 py-2 text-sm md:text-base rounded transition-colors ${
                activeTab === 'borrowings'
                  ? 'bg-blue-500 text-white shadow-md'
                  : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
              }`}
            >
              Позичання
            </button>
            <button
              onClick={() => setActiveTab('catalog')}
              className={`px-3 py-2 text-sm md:text-base rounded transition-colors ${
                activeTab === 'catalog'
                  ? 'bg-blue-500 text-white shadow-md'
                  : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
              }`}
            >
              Каталог
            </button>
            <button
              onClick={() => setActiveTab('add-reader')}
              className={`px-3 py-2 text-sm md:text-base rounded transition-colors ${
                activeTab === 'add-reader'
                  ? 'bg-blue-500 text-white shadow-md'
                  : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
              }`}
            >
              Додати читача
            </button>
            {/* Issue Book tab removed — issuing is now done from the Catalog */}
          </div>
        </div>
      </nav>

      <main className="container mx-auto px-4 py-4 md:py-6">
        {activeTab === 'readers' && <ReaderList key={refreshKey} />}
        {activeTab === 'borrowings' && <BorrowingList key={refreshKey} />}
        {activeTab === 'catalog' && <Catalog key={refreshKey} />}
        {activeTab === 'add-reader' && <AddReaderForm onReaderAdded={handleReaderAdded} />}
        {/* IssueBookForm removed; issuing handled inside Catalog component */}
      </main>
    </div>
  );
}

export default App;

