using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;

namespace BookShopApi.Entities
{
    public class MyDapper
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public MyDapper(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public async Task ExecDeleteBookPrecedure(int bookId)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@bookId", bookId, DbType.Int32);

                await connection.ExecuteAsync("dbo.SoftDeleteBook", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<List<Book>> ExecFindBooksByAuthorProcedure(string author)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var booksDictionary = new Dictionary<int, Book>();
                var books = connection.Query<Book, BookCategories, Category, Book>(
                    "findBooksByAuthor",
                    (book, bookCategory, category) =>
                    {
                        if (!booksDictionary.TryGetValue(book.Id, out var bookEntry))
                        {
                            bookEntry = book;
                            bookEntry.Categories = new List<BookCategories>();
                            booksDictionary.Add(bookEntry.Id, bookEntry);
                        }
                        if (category != null)
                        {
                            bookCategory.Category = category;
                            bookEntry.Categories.Add(bookCategory);
                        }
                        return bookEntry;
                    },
                    new { author },
                    splitOn: "Id,CategoryId",
                    commandType: CommandType.StoredProcedure
                )
                .Distinct()
                .ToList();
                return books;
            }
        }
        public async Task ExecUserStaffRoleAssignProc(int userId)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@userId", userId, DbType.Int32);

                await connection.ExecuteAsync("dbo.AssignStaffRoleToUser", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
