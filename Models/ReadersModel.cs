using LinqToDB.Mapping;


namespace ProjectWorks.Models
{
    [Table("readers")]
    public class ReadersModel
    {
        [PrimaryKey]
        [Column("id")]
        public long Id { get; set; }

        [Column("readername")]
        public string? Name { get; set; }

        [Column("role")] 
        public bool IsAdmin  { get; set; }

        [Column("isused")]
        public bool IsUsed { get; set; }

    }
}
