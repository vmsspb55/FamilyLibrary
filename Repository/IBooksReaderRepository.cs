using ProjectWorks.Models;

namespace ProjectWorks.Repository
{
    public interface IBooksReaderRepository
    {
        Task<BooksReaderModel?> GetListsbooksreaderById(int id);
        Task<List<BooksReaderModel>> GetListsbooksreader();
        Task InsertBooksReader(BooksReaderModel entity);
        Task UpdateBooksReader(BooksReaderModel entity);
        Task<List<BooksReaderModel>> GetBooksReaderByFilter(long readerId);
    }
}
