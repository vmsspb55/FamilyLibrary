using LinqToDB;
using ProjectWorks.LinqToDb.DataAccess;
using ProjectWorks.Models;
using Telegram.Bot.Types;

namespace ProjectWorks.Repository
{
    public class AuthorshipRepository : IAuthorshipRepository
    {
        private readonly ApiDataContext _db;

        public AuthorshipRepository(ApiDataContext db)
        {
            _db = db;
        }
        public Task DeleteAuthorship(int id)
        {
            throw new NotImplementedException();
        }
        public Task<AuthorshipModel?> GetAuthorship(int id)
        {
            throw new NotImplementedException();
        }
        public Task<List<AuthorshipModel>> GetAuthorships()
        {
            throw new NotImplementedException();
        }
        public Task InsertAuthorship(AuthorshipModel entity)
        {
            return  _db.InsertWithIdentityAsync(entity);
        }
        public Task UpdateAuthorship(AuthorshipModel name)
        {
            throw new NotImplementedException();
        }
    }
}
