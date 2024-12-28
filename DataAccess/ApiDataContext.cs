using LinqToDB;
using LinqToDB.Data;
using ProjectWorks.Models;

namespace ProjectWorks.LinqToDb.DataAccess
{
    public class ApiDataContext : DataConnection
    {
        public ApiDataContext(DataOptions<ApiDataContext> options) : base(options.Options) { }
        public ITable<AuthorsModel> Authors => this.GetTable<AuthorsModel>();
        public ITable<BooksModel> Books => this.GetTable<BooksModel>();
        public ITable<ReadersModel> Readers => this.GetTable<ReadersModel>();
        public ITable<AuthorshipModel> Authorships => this.GetTable<AuthorshipModel>();
        public ITable<BooksReaderModel> BooksReader => this.GetTable<BooksReaderModel>();

    }
}
