using LinqToDB.Mapping;


namespace ProjectWorks.Models
{
    [Table("books")]
    public class BooksModel
    {
        [PrimaryKey, Identity]

        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public string Name { get; set; }

        [Column("status")]
        public bool Status { get; set; }

        [Association(ThisKey = nameof(BooksModel.Id), OtherKey = nameof(AuthorshipModel.BookId))]
        public IEnumerable<AuthorshipModel> Authorships { get; set; } = new List<AuthorshipModel>();

        public string OutputRecordingBooks()
        {
            var result = Id + "   " + Name + " - " ;
            foreach (var item in Authorships)
            {
                result += item.Authors?.FirstName+" "+ item.Authors?.LastName + ",";
            }

            return result.TrimEnd(',');

        }       

    }
}
