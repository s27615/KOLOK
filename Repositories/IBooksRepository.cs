using Tutorial6.Models.DTOs;

namespace Tutorial6.Repositories;

public interface IBooksRepository
{
    Task<AuthorDTO> ShowAuthors(String title);
    Task<int> AddBook(String title, String firstName, String lastName);
}