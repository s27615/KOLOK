using Microsoft.AspNetCore.Mvc;
using Tutorial6.Repositories;
using Tutorial6.Models.DTOs;

namespace Tutorial6.Controllers;

[Route("api/books")]
[ApiController]
public class BooksController : Controller
{
    private readonly IBooksRepository _booksRepository;
    
    public BooksController(IBooksRepository booksRepository)
    {
        _booksRepository = booksRepository;
    }
    // GET
    [HttpGet("{id}/authors")]
    public async Task<IActionResult> showAuthors(String bookName)
    {
        if (await _booksRepository.ShowAuthors(bookName) == null)
            return BadRequest("Nie ma takiej książki");

        var author = _booksRepository.ShowAuthors(bookName);
            
        return Ok(author);
    }
    
    [HttpPost("1/authors")]
    public async Task<IActionResult> addBook(String title, String firstName, String lastName)
    {
        if (await _booksRepository.AddBook(title, firstName, lastName) == 0)
            return Ok("Instrukcję wykonano");
            
        return Ok("Nie wykonano instrukcji");
    }
}