using Microsoft.EntityFrameworkCore;
using myList.Models;

public class BookController : IController<book>
{
    private readonly listContext _context;

    public BookController(listContext context)
    {
        _context = context;
    }

    public async Task<List<book>> GetAllAsync()
    {
        return await _context.book.ToListAsync();
    }

    public async Task<book> GetByIdAsync(int id)
    {
        return await _context.book.FindAsync(id);
    }

    public async Task AddAsync(book newBook)
    {
        _context.book.Add(newBook);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, book newBook)
    {
        var book = await _context.book.FindAsync(id);
        if (book != null)
        {
            _context.book.Update(newBook);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var book = await _context.book.FindAsync(id);
        if (book != null)
        {
            _context.book.Remove(book);
            await _context.SaveChangesAsync();
        }
    }
}
