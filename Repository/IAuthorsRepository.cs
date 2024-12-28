using ProjectWorks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWorks.Repository
{
    public interface IAuthorsRepository
    {
        Task<AuthorsModel?> GetAuthor(int id);
        Task<List<AuthorsModel>> GetAuthors();
        Task InsertAuthor(AuthorsModel name);
        Task UpdateAuthor(AuthorsModel name);
        Task DeleteAuthor(int id);
    }
}
