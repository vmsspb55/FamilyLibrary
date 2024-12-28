using LinqToDB;
using ProjectWorks.LinqToDb.DataAccess;
using ProjectWorks.Models;
using System.Xml.Linq;

namespace ProjectWorks.Repository
{
    public class BooksRepository : IBooksRepository
    {
        private readonly ApiDataContext _db;

        public BooksRepository(ApiDataContext db)
        {
            _db = db;
        }

        public int id { get; private set; }

        public async Task<BooksModel?> GetBook(int id)
        {
            return await _db.Books.FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task<BooksModel?> GetBookDetailed(int id)
        {
            return await _db.Books.LoadWith(b => b.Authorships).ThenLoad(a => a.Authors).FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<BooksModel>> GetBooks()
        {
            return await _db.Books.LoadWith(b => b.Authorships).ThenLoad(x => x.Authors).OrderBy(s =>s.Id).ToListAsync();
        }

        public async Task UpdateBook(BooksModel entity)
        {
             await _db.UpdateAsync(entity);
        }

    }
}
