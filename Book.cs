using System;

namespace BookManagementSystemwithDapper
{
    // Represents a book entity
    public class Book
    {
        // Unique identifier for the book
        public int BookID { get; set; }

        // Title of the book (cannot be empty)
        private string title;
        public string Title
        {
            get => title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Title cannot be empty.");
                title = value;
            }
        }

        // Author of the book (cannot be empty)
        private string author;
        public string Author
        {
            get => author;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Author cannot be empty.");
                author = value;
            }
        }

        // Published year (must be non-negative)
        private int publishedYear;
        public int PublishedYear
        {
            get => publishedYear;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Published Year cannot be negative.");
                publishedYear = value;
            }
        }

        // Genre of the book
        public string Genre { get; set; }

        // Constructor for initializing properties
        public Book(int bookID, string title, string author, int publishedYear, string genre)
        {
            BookID = bookID;
            Title = title;
            Author = author;
            PublishedYear = publishedYear;
            Genre = genre;
        }

        // Parameterless constructor for ORM tools
        public Book() { }
    }
}
