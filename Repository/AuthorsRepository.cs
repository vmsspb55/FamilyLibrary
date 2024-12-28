using LinqToDB;
using ProjectWorks.LinqToDb.DataAccess;
using ProjectWorks.Models;
using System.Xml.Linq;

namespace ProjectWorks.Repository
{
    public class AuthorsRepository : IAuthorsRepository
    {
        private readonly ApiDataContext _db;

        public AuthorsRepository(ApiDataContext db)
        {
            _db = db;
        }

        public Task<AuthorsModel?> GetAuthor(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AuthorsModel>> GetAuthors()
        {
            return await _db.Authors.ToListAsync();
        }

        public Task DeleteAuthor(int id)
        {
            throw new NotImplementedException();
        }

        public Task InsertAuthor(AuthorsModel id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAuthor(AuthorsModel id)
        {
            throw new NotImplementedException();
        }


    }
}
