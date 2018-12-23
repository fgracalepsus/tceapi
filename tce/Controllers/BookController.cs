using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TceApi.Models;

namespace TceApi.Controllers
{
    [Route("api/book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookContext _context;

        public BookController(BookContext context)
        {
            
            _context = context;
            
            if (_context.Books.Count() == 0)
            {
                // Toda vez que a coleção de livros estiver vazia, mantém um livro fake pra manter a lista com itens
                _context.Books.Add(new Book {
                    Name = "Livro de exemplo inicial",
                    Price = 100.5,
                    ISBN = "123456",
                    Published = new DateTime(1970,12,31,23,0,0),
                    Avatar = "https://vignette.wikia.nocookie.net/2007scape/images/7/7a/Mage%27s_book_detail.png/revision/latest/scale-to-width-down/130?cb=20180310083825"
                });
                _context.SaveChanges();
            }
            
        }

        private bool existsByISBN(Book book)
        {
            return _context.Books
                        .Where(b => b.ISBN == book.ISBN)
                        .Where(b => b.Id != book.Id)
                        .Any();
        }

        /*
        // GET: api/Book
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            return await _context.Books.ToListAsync();
        }
        */

        // GET: api/Book/sortField/sortDirection/ISBN/Name/Price/Published
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks(string sortField, string sortDirection, string ISBN, string Name, string Price, string Published)
        {
            // return await _context.Books.ToListAsync();

            IQueryable<Book> bookIQ = from s in _context.Books
                                      select s;

            // Order
            var _sortField = String.IsNullOrEmpty(sortField) ? "name" : sortField;
            var _sortDirection = String.IsNullOrEmpty(sortDirection) ? "asc" : sortDirection;

            if(_sortField == "ISBN")
            {
                if(_sortDirection == "desc")
                {
                    bookIQ = bookIQ.OrderByDescending(s => s.ISBN);
                }
                else
                {
                    bookIQ = bookIQ.OrderBy(s => s.ISBN);
                }
            }else if (_sortField == "Price")
            {
                if (_sortDirection == "desc")
                {
                    bookIQ = bookIQ.OrderByDescending(s => s.Price);
                }
                else
                {
                    bookIQ = bookIQ.OrderBy(s => s.Price);
                }
            }
            else if(_sortField == "Published")
            {
                if (_sortDirection == "desc")
                {
                    bookIQ = bookIQ.OrderByDescending(s => s.Published);
                }
                else
                {
                    bookIQ = bookIQ.OrderBy(s => s.Published);
                }
            }
            else
            {
                if (_sortDirection == "desc")
                {
                    bookIQ = bookIQ.OrderByDescending(s => s.Name);
                }
                else
                {
                    bookIQ = bookIQ.OrderBy(s => s.Name);
                }
            }

            // Where
            if (!String.IsNullOrEmpty(ISBN))
            {
                bookIQ = bookIQ.Where(s => s.ISBN == ISBN);
            }
            if (!String.IsNullOrEmpty(Name))
            {
                bookIQ = bookIQ.Where(s => s.Name.Contains(Name));
            }
            if (!String.IsNullOrEmpty(Price))
            {
                bookIQ = bookIQ.Where(s => s.Price == Convert.ToDouble(Price));
            }
            if (!String.IsNullOrEmpty(Published))
            {
                bookIQ = bookIQ.Where(s => s.Published == Convert.ToDateTime(Published));
            }

            return await bookIQ.AsNoTracking().ToListAsync();
        }

        // GET: api/Book/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(long id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // POST: api/Book
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            if (existsByISBN(book))
            {
                return BadRequest();
            }
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // PUT: api/Book/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(long id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }
            if (existsByISBN(book))
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Book/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteBook(long id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return book;
        }
    }
}
