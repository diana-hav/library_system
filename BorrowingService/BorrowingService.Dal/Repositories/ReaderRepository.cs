using System.Collections.Generic;
using System.Threading.Tasks;
using BorrowingService.Domain.Entities;
using Dapper;
using Npgsql;
using BorrowingService.Dal.Interfaces;

namespace BorrowingService.Dal.Repositories
{
    public class ReaderRepository : BaseRepository, IReaderRepository
    {
        public ReaderRepository(NpgsqlConnection connection) : base(connection) { }

        public async Task<IEnumerable<Reader>> GetAllAsync()
        {
            const string sql = @"SELECT id, full_name AS FullName, email AS Email, created_at AS CreatedAt
                                 FROM readers;";
            return await Connection.QueryAsync<Reader>(sql, transaction: Transaction);
        }

        public async Task<Reader?> GetByIdAsync(int id)
        {
            const string sql = @"SELECT id, full_name AS FullName, email AS Email, created_at AS CreatedAt
                                 FROM readers WHERE id = @id;";
            return await Connection.QuerySingleOrDefaultAsync<Reader>(sql, new { id }, Transaction);
        }

        public async Task<int> AddAsync(Reader reader)
        {
            const string sql = @"INSERT INTO readers (full_name, email) 
                                 VALUES (@FullName, @Email) RETURNING id;";
            return await Connection.ExecuteScalarAsync<int>(sql, reader, Transaction);
        }
    }
}
