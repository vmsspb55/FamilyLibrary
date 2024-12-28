using LinqToDB.Mapping;

namespace ProjectWorks.Models
{
    [Table("authorship")]
    public class AuthorshipModel
    {
        [PrimaryKey(), Identity]
        [Column("id")]
        public int Id { get; set; }

        [Column("book_id")]
        public int BookId { get; set; }

        [Column("author_id")]
        public int AuthorId { get; set; }

        [Association(ThisKey = nameof(AuthorshipModel.AuthorId), OtherKey = nameof(AuthorsModel.Id))]
        public AuthorsModel? Authors { get; set; }
        

       
    }
}
