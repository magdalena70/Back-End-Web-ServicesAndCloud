using BookShopSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopSystem.Client
{
    public class BookShopMain
    {
        static void Main()
        {
            var context = new BookShopSystemContext();
            var authorCount = context.Authors.Count();
            Console.WriteLine(authorCount);

            /*var books = context.Books
                .Take(3)
                .ToList();
            books[0].RelatedBooks.Add(books[1]);
            books[1].RelatedBooks.Add(books[0]);
            books[0].RelatedBooks.Add(books[2]);
            books[2].RelatedBooks.Add(books[0]);

            context.SaveChanges();*/
        }
    }
}
