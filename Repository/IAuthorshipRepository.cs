using ProjectWorks.Models;

namespace ProjectWorks.Repository
{
    public interface IAuthorshipRepository
    {
        Task<AuthorshipModel?> GetAuthorship(int id);
        Task<List<AuthorshipModel>> GetAuthorships();
        Task InsertAuthorship(AuthorshipModel entity);
        Task UpdateAuthorship(AuthorshipModel entity);
        Task DeleteAuthorship(int id);
    }
}
