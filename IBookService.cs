namespace BookManagementSystemwithDapper
{
    public interface IBookService
    {
        // Method to insert a new book into the database
        void InsertNewBook();

        // Method to retrieve all books from the database
        void RetrieveAllBooks();

        // Method to retrieve books by a specific author
        void RetrieveBooksByAuthor();

        // Method to update details of an existing book
        void UpdateBook();

        // Method to delete a book from the database
        void DeleteBook();

        // Method to retrieve books with optional filters for genre and sorting,
        // and with pagination support
        void RetrieveBooks(string genreFilter = null, string sortBy = null, int page = 1, int pageSize = 10);
    }
}
