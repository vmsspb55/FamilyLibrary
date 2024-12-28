using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWorks.Models
{
    [Table("listsbooksreader")]
    public class BooksReaderModel
    {
        [PrimaryKey, Identity]
        [Column("id")]
        public int Id { get; set; }
        
        [Column("reader_id")]
        public long ReaderId { get; set; }
        
        [Column("book_id")]
        public int BookId { get; set; }
        
        [Column("readered")]
        public bool IsReadered { get; set; }
        
        [Column("dateofissue")]
        public DateTime DateOfIssue { get; set; }
        
        [Column("returndate")]
        public DateTime ReturnDate { get; set; }








    }
}
