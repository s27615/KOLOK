using Microsoft.Data.SqlClient;
using Tutorial6.Models.DTOs;

namespace Tutorial6.Repositories;

public class BooksRepository : IBooksRepository
{
    private readonly IConfiguration _configuration;
    public BooksRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<AuthorDTO> ShowAuthors(string title)
    {
            var query = @"select authors.pk, first_name, last_name from authors 
                        inner join books_authors ba on authors.PK = ba.FK_author 
                        inner join books b on ba.FK_book = b.PK
                        WHERE title = @TITLE;
";
        
            using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
            using SqlCommand command = new SqlCommand();
        
            command.Connection = connection;
            command.CommandText = query;
            command.Parameters.AddWithValue("@TITLE", title);
        
            await connection.OpenAsync();

            var reader = await command.ExecuteReaderAsync();
        
            await reader.ReadAsync();

            if (!reader.HasRows) throw new Exception();

            var authorPKOrdinal = reader.GetOrdinal("PK");
            var first_naOrdinal = reader.GetOrdinal("first_name");
            var last_naOrdinal = reader.GetOrdinal("last_name");
        

            var authorDTO = new AuthorDTO()
            {
                IdAuthor = reader.GetInt32(authorPKOrdinal),
                FirstName = reader.GetString(first_naOrdinal),
                LastName = reader.GetString(last_naOrdinal),
            };

            return authorDTO;
        }

    public async Task<int> AddBook(String title, String firstName, String lastName)
    {
        var query = "INSERT INTO books(title) VALUES (@TITLE)";
        using SqlConnection connectionI = new SqlConnection(_configuration.GetConnectionString("Default"));
        
        using SqlCommand commandI = new SqlCommand();
    
        commandI.Connection = connectionI;
        commandI.CommandText = query;
        commandI.Parameters.AddWithValue("@TITLE", title);
        await connectionI.OpenAsync();
        await commandI.ExecuteNonQueryAsync();
            
            
        query = "INSERT INTO authors(first_name, last_name) VALUES (@FIRST_NAME, @LAST_NAME) ";
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        
        using SqlCommand command = new SqlCommand();
    
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@FIRST_NAME", firstName);
        command.Parameters.AddWithValue("@LAST_NAME", lastName);
        await connection.OpenAsync();
        object resultObj = await command.ExecuteScalarAsync();
        
        var query2 = "insert into books_authors(fk_book, fk_author) values ((Select PK FROM BOOKS WHERE TITLE = @TITLE), (Select PK FROM AUTHOR WHERE FIRST_NAME = @FIRST_NAME AND LAST_NAME = @LAST_NAME));";

        using SqlConnection connection2 = new SqlConnection(_configuration.GetConnectionString("Default"));
        
        using SqlCommand command2 = new SqlCommand();
    
        command2.Connection = connection2;
        command2.CommandText = query2;
        await connection2.OpenAsync();
        resultObj = await command.ExecuteScalarAsync();
        
        if (resultObj != null)
        {
            int result = Convert.ToInt32(resultObj);
            return result;
        }
        return 0;
    }
}