//using ProjectWorks.LinqToDb.DataAccess;
using LinqToDB;
using LinqToDB.AspNet;
using ProjectWorks.LinqToDb.DataAccess;
using ProjectWorks.Repository;

namespace ProjectWorks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<Worker>();
            var connectionString = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddLinqToDBContext<ApiDataContext>((provider, options)
                => options
               .UsePostgreSQL(connectionString),
               lifetime: ServiceLifetime.Singleton);
            builder.Services.AddSingleton<IAuthorsRepository, AuthorsRepository>();
            builder.Services.AddSingleton<IBooksRepository, BooksRepository>();
            builder.Services.AddSingleton<IReadersRepository, ReadersRepository>();
            builder.Services.AddSingleton<IAuthorshipRepository, AuthorshipRepository>();
            builder.Services.AddSingleton<IBooksReaderRepository, BooksReaderRepository>();
            var host = builder.Build();
            host.Run();
        }
    }
}