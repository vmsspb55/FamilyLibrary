using LinqToDB;
using ProjectWorks.LinqToDb.DataAccess;
using ProjectWorks.Models;
using Telegram.Bot.Types;

namespace ProjectWorks.Repository
{

    public class ReadersRepository : IReadersRepository
    {
        private readonly ApiDataContext _db;

        public ReadersRepository(ApiDataContext db)
        {
            _db = db;
        }
        public Task<ReadersModel?> GetReaderById(long id)
        {
            return _db.Readers.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<ReadersModel>> GetReaders()
        {
            return await _db.Readers.ToListAsync();
        }

        public Task InsertReader(ReadersModel reader)
        {
            return _db.InsertWithIdentityAsync(reader);
        }

        public Task UpdateReader(ReadersModel reader)
        {
            return _db.UpdateAsync(reader);
        }
        public Task DeleteReader(long entity)
        {
            return _db.DeleteAsync(entity);
          
        }



    }
}
