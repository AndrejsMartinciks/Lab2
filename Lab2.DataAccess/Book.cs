using System.ComponentModel.DataAnnotations;

namespace Lab2.DataAccess
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Title cannot exceed 50 characters.")]
        public string Title { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Year must be a positive number.")]
        public int Year { get; set; }

        [Range(0.0, 5.0, ErrorMessage = "Grade must be between 0 and 5.")]
        public double Grade { get; set; }

        public Author Author { get; set; }
        public int AuthorId { get; set; }
    }
}
