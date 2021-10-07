using Acme.BookStore.Books;
using Acme.BookStore.Books.Repository;
using Acme.BookStore.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Acme.BookStore.Repository
{
    public class BookRepository: EfCoreRepository<BookStoreDbContext,Book,Guid>,IBookRepository
    {
        public BookRepository(IDbContextProvider<BookStoreDbContext> bookStoreDbContext) :base(bookStoreDbContext)
        { 
        
        }
    }
}
