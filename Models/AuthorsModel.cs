using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProjectWorks.Models
{
    [Table("authors")]
    public class AuthorsModel
    {
        [PrimaryKey, Identity]
        [Column("id")]
        public int Id { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("middle_name")]
        public string MiddleName { get; set; }

        public string OutputRecordingAuthors()
        {
            return Id + " " + LastName + " " + FirstName + " " + MiddleName;
        }


    }
}

            
            

