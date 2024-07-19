using Dapper; // Import Dapper for lightweight ORM functionality
using MySql.Data.MySqlClient; // Import MySQL client library for database operations

namespace BookManagementSystemwithDapper
{
    public class DapperBookService : IBookService
    {
        // Connection string for MySQL database
        private string connectionString = "Server=localhost;Port=3306;Database=Library;Uid=root;Pwd=isaj10;";

        // Method to insert a new book into the database
        public void InsertNewBook()
        {
            // Collect book details from user input
            Console.Write("Enter book title: ");
            string title = Console.ReadLine();
            Console.Write("Enter book author: ");
            string author = Console.ReadLine();
            Console.Write("Enter published year: ");
            int publishedYear = int.Parse(Console.ReadLine());
            Console.Write("Enter book genre: ");
            string genre = Console.ReadLine();

            try
            {
                // Establish a connection to the MySQL database
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    // Define the SQL query to insert a new book
                    string query = "INSERT INTO Books (Title, Author, PublishedYear, Genre) VALUES (@Title, @Author, @PublishedYear, @Genre)";
                    // Create a parameter object for the query
                    var parameters = new { Title = title, Author = author, PublishedYear = publishedYear, Genre = genre };
                    // Execute the query
                    connection.Execute(query, parameters);
                    Console.WriteLine("Book inserted successfully.");
                }
            }
            catch (MySqlException ex)
            {
                // Handle MySQL specific exceptions
                Console.WriteLine($"MySQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Method to retrieve and display all books from the database
        public void RetrieveAllBooks()
        {
            try
            {
                // Establish a connection to the MySQL database
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    // Define the SQL query to select all books
                    string query = "SELECT * FROM Books";
                    // Execute the query and retrieve the books
                    var books = connection.Query<Book>(query).ToList();
                    Console.WriteLine("Books:");
                    // Display each book's details
                    foreach (var book in books)
                    {
                        Console.WriteLine($"{book.BookID}, {book.Title}, {book.Author}, {book.PublishedYear}, {book.Genre}");
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Handle MySQL specific exceptions
                Console.WriteLine($"MySQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Method to retrieve books by a specific author
        public void RetrieveBooksByAuthor()
        {
            // Collect author name from user input
            Console.Write("Enter author name: ");
            string author = Console.ReadLine();

            try
            {
                // Establish a connection to the MySQL database
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    // Define the SQL query to select books by the specified author
                    string query = "SELECT * FROM Books WHERE Author = @Author";
                    // Create a parameter object for the query
                    var parameters = new { Author = author };
                    // Execute the query and retrieve the books
                    var books = connection.Query<Book>(query, parameters).ToList();
                    Console.WriteLine($"Books by {author}:");
                    // Display each book's details
                    foreach (var book in books)
                    {
                        Console.WriteLine($"{book.BookID}, {book.Title}, {book.Author}, {book.PublishedYear}, {book.Genre}");
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Handle MySQL specific exceptions
                Console.WriteLine($"MySQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void UpdateBook()
        {
            // Prompt the user to enter the BookID of the book they want to update
            Console.Write("Enter BookID of the book to update: ");
            int bookID = int.Parse(Console.ReadLine());

            try
            {
                // Establish a connection to the MySQL database
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Query to check if the book exists in the database
                    string checkQuery = "SELECT COUNT(*) FROM Books WHERE BookID = @BookID";
                    var checkParameters = new { BookID = bookID };
                    int count = connection.ExecuteScalar<int>(checkQuery, checkParameters);

                    // If the book doesn't exist, notify the user and exit the method
                    if (count == 0)
                    {
                        Console.WriteLine("Book not found.");
                        return;
                    }

                    // Collect new details for the book from user input
                    Console.Write("Enter new book title: ");
                    string title = Console.ReadLine();
                    Console.Write("Enter new book author: ");
                    string author = Console.ReadLine();
                    Console.Write("Enter new published year (leave empty to keep current): ");
                    string publishedYearInput = Console.ReadLine();
                    int? publishedYear = null;

                    // Parse the published year if provided
                    if (!string.IsNullOrEmpty(publishedYearInput) && int.TryParse(publishedYearInput, out int parsedYear))
                    {
                        publishedYear = parsedYear;
                    }

                    Console.Write("Enter new book genre: ");
                    string genre = Console.ReadLine();

                    // Start building the SQL update query
                    string query = "UPDATE Books SET Title = @Title, Author = @Author, Genre = @Genre";

                    // Prepare parameters for the query
                    var parameters = new DynamicParameters();
                    parameters.Add("Title", title);
                    parameters.Add("Author", author);
                    parameters.Add("Genre", genre);
                    parameters.Add("BookID", bookID);

                    // Add the PublishedYear to the query if it was provided
                    if (publishedYear.HasValue)
                    {
                        query += ", PublishedYear = @PublishedYear";
                        parameters.Add("PublishedYear", publishedYear.Value);
                    }

                    // Complete the query with the WHERE clause
                    query += " WHERE BookID = @BookID";

                    // Execute the update query
                    connection.Execute(query, parameters);
                    Console.WriteLine("Book updated successfully.");
                }
            }
            catch (MySqlException ex)
            {
                // Handle MySQL specific exceptions
                Console.WriteLine($"MySQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Method to delete a book from the database
        public void DeleteBook()
        {
            // Collect the BookID of the book to be deleted
            Console.Write("Enter BookID of the book to delete: ");
            int bookID = int.Parse(Console.ReadLine());

            try
            {
                // Establish a connection to the MySQL database
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if the book exists in the database
                    string checkQuery = "SELECT COUNT(*) FROM Books WHERE BookID = @BookID";
                    var checkParameters = new { BookID = bookID };
                    int count = connection.ExecuteScalar<int>(checkQuery, checkParameters);

                    // If the book doesn't exist, notify the user and exit
                    if (count == 0)
                    {
                        Console.WriteLine("Book not found.");
                        return;
                    }

                    // Confirm the deletion with the user
                    Console.Write("Are you sure you want to delete this book? (yes/no): ");
                    string confirmation = Console.ReadLine();
                    if (confirmation.ToLower() != "yes")
                    {
                        Console.WriteLine("Deletion cancelled.");
                        return;
                    }

                    // Define the SQL query to delete the book
                    string query = "DELETE FROM Books WHERE BookID = @BookID";
                    var parameters = new { BookID = bookID };
                    // Execute the query
                    connection.Execute(query, parameters);
                    Console.WriteLine("Book deleted successfully.");
                }
            }
            catch (MySqlException ex)
            {
                // Handle MySQL specific exceptions
                Console.WriteLine($"MySQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Method to retrieve books with optional filters and pagination
        public void RetrieveBooks(string genreFilter = null, string sortBy = null, int page = 1, int pageSize = 10)
        {
            try
            {
                // Establish a connection to the MySQL database
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    // Define the base SQL query
                    string query = "SELECT * FROM Books";
                    var parameters = new DynamicParameters();

                    // Add genre filter if provided
                    if (!string.IsNullOrEmpty(genreFilter))
                    {
                        query += " WHERE Genre = @Genre";
                        parameters.Add("@Genre", genreFilter);
                    }

                    // Add sorting if provided
                    if (!string.IsNullOrEmpty(sortBy))
                    {
                        query += $" ORDER BY {sortBy}";
                    }

                    // Add pagination
                    query += " LIMIT @Offset, @PageSize";
                    parameters.Add("@Offset", (page - 1) * pageSize);
                    parameters.Add("@PageSize", pageSize);

                    // Execute the query and retrieve the books
                    var books = connection.Query<Book>(query, parameters).ToList();
                    Console.WriteLine("Books:");
                    // Display each book's details
                    foreach (var book in books)
                    {
                        Console.WriteLine($"{book.BookID}, {book.Title}, {book.Author}, {book.PublishedYear}, {book.Genre}");
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Handle MySQL specific exceptions
                Console.WriteLine($"MySQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
