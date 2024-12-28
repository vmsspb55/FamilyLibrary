using ProjectWorks.Models;

namespace ProjectWorks.Repository
{
    public interface IBooksRepository
    {
        Task<BooksModel?> GetBook(int id);
        Task<List<BooksModel>> GetBooks();
        Task<BooksModel?> GetBookDetailed(int id);
        Task UpdateBook(BooksModel entity);

        //Task InsertBook(BooksModel name);
        //Task DeleteBook(int id);
    }
}
