using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Week8Assignment.Data;
using Week8Assignment.Models;

namespace Week8Assignment.Controllers;

public class BookController : Controller
{
    private readonly LibraryDbContext _context;

    public BookController(LibraryDbContext context)
    {
        _context = context;
    }

    public IActionResult List()
    {
        var books = _context.Books.OrderBy(b => b.BookId).ToList();
        return View(books);
    }

    public IActionResult Details(int id)
    {
        var book = _context.Books.Find(id);
        if (book == null)
            return NotFound();
        return View(book);
    }

    [HttpGet]
    public IActionResult Create() => View(new Book());

    [HttpPost]
    public IActionResult Create(Book book)
    {
        if (string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.Author))
        {
            ModelState.AddModelError(string.Empty, "Title and Author are required.");
            return View(book);
        }
        _context.Books.Add(book);
        _context.SaveChanges();
        return RedirectToAction(nameof(List));
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var book = _context.Books.Find(id);
        if (book == null)
            return NotFound();
        return View(book);
    }

    [HttpPost]
    public IActionResult DeletePost(int id)
    {
        var book = _context.Books.Find(id);
        if (book == null)
            return NotFound();
        _context.Books.Remove(book);
        _context.SaveChanges();
        return RedirectToAction(nameof(List));
    }
}
