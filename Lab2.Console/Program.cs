using Lab2.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Lab2.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var db = new AuthorDbContext();

            db.Database.EnsureCreated();

            // 1. Добавление авторов и книг, если их еще нет
            if (!db.Authors.Any())
            {
                var authors = new List<Author>
                {
                    new Author
                    {
                        Name = "J.K.",
                        Surname = "Rowling",
                        Country = "United Kingdom",
                        Books =
                        {
                            new Book { Title = "Harry Potter and the Philosopher's Stone", Year = 1997, Grade = 4.9 },
                            new Book { Title = "Harry Potter and the Chamber of Secrets", Year = 1998, Grade = 4.8 },
                            new Book { Title = "Harry Potter and the Prisoner of Azkaban", Year = 1999, Grade = 4.85 }
                        }
                    },
                    new Author
                    {
                        Name = "George",
                        Surname = "Orwell",
                        Country = "United Kingdom",
                        Books =
                        {
                            new Book { Title = "1984", Year = 1949, Grade = 4.8 },
                            new Book { Title = "Animal Farm", Year = 1945, Grade = 4.7 }
                        }
                    },
                    new Author
                    {
                        Name = "J.R.R.",
                        Surname = "Tolkien",
                        Country = "United Kingdom",
                        Books =
                        {
                            new Book { Title = "The Hobbit", Year = 1937, Grade = 4.9 },
                            new Book { Title = "The Lord of the Rings: The Fellowship of the Ring", Year = 1954, Grade = 4.95 },
                            new Book { Title = "The Lord of the Rings: The Two Towers", Year = 1954, Grade = 4.9 },
                            new Book { Title = "The Lord of the Rings: The Return of the King", Year = 1955, Grade = 4.95 }
                        }
                    },
                    new Author
                    {
                        Name = "F. Scott",
                        Surname = "Fitzgerald",
                        Country = "United States",
                        Books =
                        {
                            new Book { Title = "The Great Gatsby", Year = 1925, Grade = 4.7 },
                            new Book { Title = "Tender Is the Night", Year = 1934, Grade = 4.5 }
                        }
                    }
                };

                db.Authors.AddRange(authors);
                db.SaveChanges();
                System.Console.WriteLine("Authors and books added to the database.");
            }

            // 2. Запрос авторов и их стран
            var results = db.Authors
                .OrderBy(a => a.Surname)
                .Include(a => a.Books) // Подгружаем книги авторов
                .ToList();

            System.Console.WriteLine("\nList of Authors and their Countries:");
            foreach (var author in results)
            {
                System.Console.WriteLine($"Author: {author.Name} {author.Surname} ({author.Country})");
                foreach (var book in author.Books)
                {
                    System.Console.WriteLine($"  - Book: {book.Title} ({book.Year}) - Grade: {book.Grade}");
                }
            }

            // 3. Запрос книг по странам
            var booksByCountry = from author in db.Authors
                                 from book in author.Books
                                 where author.Country == "United Kingdom"
                                 orderby book.Year
                                 select new { author.Name, author.Surname, author.Country, book.Title, book.Year, book.Grade };

            System.Console.WriteLine("\nBooks by authors from United Kingdom:");
            foreach (var book in booksByCountry)
            {
                System.Console.WriteLine($"Author: {book.Name} {book.Surname} ({book.Country})");
                System.Console.WriteLine($"  - Book: {book.Title} ({book.Year}) - Grade: {book.Grade}");
            }

            // 4. Обновление страны автора
            var authorToUpdate = db.Authors.FirstOrDefault(a => a.Name == "J.K." && a.Surname == "Rowling");
            if (authorToUpdate != null)
            {
                authorToUpdate.Country = "UK";
                db.SaveChanges();
                System.Console.WriteLine("\nAuthor's country updated to 'UK'.");
            }

            // 5. Удаление книги
            var bookToDelete = db.Books.FirstOrDefault(b => b.Title == "1984");
            if (bookToDelete != null)
            {
                db.Books.Remove(bookToDelete);
                db.SaveChanges();
                System.Console.WriteLine("\nBook '1984' deleted.");
            }
        }
    }
}