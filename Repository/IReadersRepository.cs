using ProjectWorks.Models;

namespace ProjectWorks.Repository
{
    public interface IReadersRepository
    {
        Task<ReadersModel?> GetReaderById(long id);
        Task<List<ReadersModel>> GetReaders();
        
        Task InsertReader(ReadersModel reader);
        Task UpdateReader(ReadersModel reader);
        Task DeleteReader(long id);

    }
}
