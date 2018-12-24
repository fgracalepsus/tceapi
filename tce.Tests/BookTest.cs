using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tce.Controllers;
using tce.Models;

namespace tce.Tests
{
    [TestClass]
    public class BookTestController
    {
        private List<Book> GetTestBooks()
        {
            var test = new List<Book>();
            test.Add(new Book { Id = 1, ISBN = "123" , Name = "Livro Teste 1", Price = 10.3, Publishe = new Date(2010,10,11), Avatar = "http://img.com/img1.png" });
            test.Add(new Book { Id = 2, ISBN = "456" , Name = "Livro Teste 2", Price = 40.7, Publishe = new Date(2010,11,12), Avatar = "http://img.com/img2.png" });
            test.Add(new Book { Id = 3, ISBN = "789" , Name = "Livro Teste 3", Price = 36.9, Publishe = new Date(2010,12,13), Avatar = "http://img.com/img3.png" });
            test.Add(new Book { Id = 4, ISBN = "1011", Name = "Livro Teste 4", Price = 51.0, Publishe = new Date(2010,09,14), Avatar = "http://img.com/img4.png" });

            return test;
        }

        [TestMethod]
        public void Books_ShouldReturnAll()
        {
            var test = GetTestBooks();
            var controller = new BookController(test);

            var result = controller.GetBooks("","","","","","") as List<Book>;
            Assert.AreEqual(test.Count, result.Count);
        }

        [TestMethod]
        public void Books_ShouldReturnOne()
        {
            var test = GetTestBooks();
            var controller = new BookController(test);

            var result = controller.GetBooks("", "", "456", "", "", "") as List<Book>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 1);
        }

        [TestMethod]
        public void Books_ShouldReturnNone()
        {
            var test = GetTestBooks();
            var controller = new BookController(test);

            var result = controller.GetBooks("", "", "4560", "", "", "") as List<Book>;
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Books_ShouldReturnAllOrderByISBNDesc()
        {
            var test = GetTestBooks();
            var controller = new BookController(test);

            var result = controller.GetBooks("ISBN", "desc", "", "", "", "") as List<Book>;
            Assert.IsNotNull(result);
            Assert.AreEqual(test[3].ISBN, result.Content[0].ISBN);
        }

        [TestMethod]
        public void Books_ShouldReturnById()
        {
            var test = GetTestBooks();
            var controller = new BookController(test);

            var result = controller.GetBook(1) as List<Book>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 1);
        }

        [TestMethod]
        public void Books_AddNew()
        {
            var test = GetTestBooks();
            var controller = new BookController(test);

            var result = controller.PostBook(new Book(ISBN = "9999", Name = "Teste New", Price = 999.99, Published = new DateTime(2009,10,09))) as List<Book>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(result.Content.ISBN, "9999");
        }

        [TestMethod]
        public void Books_AddNewDupISBN()
        {
            var test = GetTestBooks();
            var controller = new BookController(test);

            var result = controller.PostBook(new Book(ISBN = "9999", Name = "Teste New DUP", Price = 999.99, Published = new DateTime(2009, 10, 09))) as List<Book>;
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Books_UpdateItem()
        {
            var test = GetTestBooks();
            var controller = new BookController(test);

            var result = controller.PutBook(1, new Book(Id = 1, ISBN = "123", Name = "Teste Update", Price = 999.99, Published = new DateTime(2009, 10, 09))) as List<Book>;
            Assert.AreEqual(result.status, 204);
        }

        [TestMethod]
        public void Books_UpdateNotExistent()
        {
            var test = GetTestBooks();
            var controller = new BookController(test);

            var result = controller.PutBook(100, new Book(Id = 100, ISBN = "123", Name = "Teste Update", Price = 999.99, Published = new DateTime(2009, 10, 09))) as List<Book>;
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Books_DeleteItem()
        {
            var test = GetTestBooks();
            var controller = new BookController(test);

            var result = controller.DeleteBook(3) as List<Book>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, 3);
        }
    }
}
