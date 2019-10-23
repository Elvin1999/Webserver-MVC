using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Services
{
   public static class BookService
    {
        public static List<Book> Books = new List<Book>();
        
        public static void AddBook(Book book)
        {
            Books.Add(book);
        }
    }
}
