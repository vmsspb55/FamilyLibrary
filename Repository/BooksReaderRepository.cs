using LinqToDB;
using ProjectWorks.LinqToDb.DataAccess;
using ProjectWorks.Models;

namespace ProjectWorks.Repository
{
    public class BooksReaderRepository : IBooksReaderRepository
    {
        private readonly ApiDataContext _db;

        public BooksReaderRepository(ApiDataContext db)
        {
            _db = db;
        }

        public Task<List<BooksReaderModel>> GetListsbooksreader()
        {
            return _db.BooksReader.ToListAsync();
        }

        public Task<BooksReaderModel?> GetListsbooksreaderById(int id)
        {
            return _db.BooksReader.FirstOrDefaultAsync(b  => b.ReaderId == id);
            
        }
        public Task<List<BooksReaderModel>> GetBooksReaderByFilter(long readerId)
        {
            var query = _db.BooksReader.Where(s => s.IsReadered && s.ReaderId.Equals(readerId)).OrderBy(s => s.BookId);        
            return query.ToListAsync();
        }

        public Task InsertBooksReader(BooksReaderModel entity)
        {
            return  _db.InsertWithIdentityAsync(entity);
        }

        public  Task UpdateBooksReader(BooksReaderModel entity)
        {
            return _db.UpdateAsync(entity);
        }
    }
}
