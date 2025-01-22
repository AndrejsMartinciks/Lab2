using System.ComponentModel.DataAnnotations;

namespace Lab2.DataAccess
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string Name { get; set; }

        [Required]
        [StringLength(50)]
        public required string Surname { get; set; }

        [Required]
        [StringLength(50)]
        public required string Country { get; set; }

        public List<Book> Books { get; } = new();
    }
}