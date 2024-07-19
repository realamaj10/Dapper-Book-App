using System;

namespace BookManagementSystemwithDapper
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create an instance of the DapperBookService
            DapperBookService bookService = new DapperBookService();
            bool exit = false; // Flag to control the application loop

            while (!exit)
            {
                // Displaying the main menu
                Console.WriteLine("\nBook Management Application");
                Console.WriteLine("1. Add a new book");
                Console.WriteLine("2. View all books");
                Console.WriteLine("3. View books by author");
                Console.WriteLine("4. Update a book's details");
                Console.WriteLine("5. Delete a book");
                Console.WriteLine("6. Search books by genre");
                Console.WriteLine("7. Sort and paginate books");
                Console.WriteLine("8. Exit");
                Console.Write("Choose an option: ");

                // Reading user's choice
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        // Add a new book
                        bookService.InsertNewBook();
                        break;
                    case "2":
                        // View all books
                        bookService.RetrieveAllBooks();
                        break;
                    case "3":
                        // View books by a specific author
                        bookService.RetrieveBooksByAuthor();
                        break;
                    case "4":
                        // Update a book's details
                        bookService.UpdateBook();
                        break;
                    case "5":
                        // Delete a book
                        bookService.DeleteBook();
                        break;
                    case "6":
                        // Search books by genre
                        Console.Write("Enter genre to search: ");
                        string genre = Console.ReadLine();
                        bookService.RetrieveBooks(genreFilter: genre);
                        break;
                    case "7":
                        // Sort and paginate books
                        Console.Write("Enter sorting criteria (Title, Author, PublishedYear): ");
                        string sortBy = Console.ReadLine();
                        Console.Write("Enter page number: ");
                        int page;
                        if (int.TryParse(Console.ReadLine(), out page))
                        {
                            bookService.RetrieveBooks(sortBy: sortBy, page: page);
                        }
                        else
                        {
                            Console.WriteLine("Invalid page number. Please enter a valid number.");
                        }
                        break;
                    case "8":
                        // Exit the application
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }

            Console.WriteLine("Goodbye!");
        }
    }
}
