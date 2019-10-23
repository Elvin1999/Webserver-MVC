using ConsoleApp1.Models;
using ConsoleApp1.Server;
using ConsoleApp1.Services;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Controllers
{
    public class BookController
    {
        public BookController()
        {
                BookService.Books = new List<Book>() {
                new Book()
                {
                     Name="Harry Potter",
                      Author="John"
                },
                new Book()
                {
                     Name="Best Book",
                      Author="Mike"
                }

            };
        }
        [HttpAttribute("GET")]
        public string All()
        {
           

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<html><head></head><body>");
            stringBuilder.Append("<ul>");
            foreach (var item in BookService.Books)
            {
                stringBuilder.Append($"<li>{item.Author} = > {item.Name} </li>");
            }
            stringBuilder.Append("</ul>");
            stringBuilder.Append("</body>");
            stringBuilder.Append("</html>");
            return stringBuilder.ToString();
        }
        [HttpAttribute("GET")]
        public string Add()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<html><head></head><body>");
            stringBuilder.Append("<ul>");
            foreach (var item in BookService.Books)
            {
                stringBuilder.Append($"<li>{item.Author} = > {item.Name} </li>");
            }
            stringBuilder.Append("</ul>");
            stringBuilder.Append("<form action='/book/AddBook' method='post'>");
            stringBuilder.Append("<input type='text' name='name' />");
            stringBuilder.Append("<input type='text' name='author' />");
            stringBuilder.Append("<input type='submit' value='add' />");
            stringBuilder.Append("</form>");
            stringBuilder.Append("</body>");
            stringBuilder.Append("</html>");
            return stringBuilder.ToString();
        }
        [HttpAttribute("POST")]
        public string AddBook(string name, string author)
        {
            BookService.AddBook(new Book() { Author = author, Name = name });
            return All();
        }

        public string Detail(int id)
        {
            return $"{id} is good book";
        }
    }
}