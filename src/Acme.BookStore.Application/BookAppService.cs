using Acme.BookStore.BackgroundJobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Dynamic.Core;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Auditing;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Users;
using Volo.Abp.Validation;

namespace Acme.BookStore.Books
{
    //public class BookAppService :
    //    CrudAppService<
    //        Book, //The Book entity
    //        BookDto, //Used to show books
    //        Guid, //Primary key of the book entity
    //        PagedAndSortedResultRequestDto, //Used for paging/sorting
    //        CreateUpdateBookDto>, //Used to create/update a book
    //    IBookAppService //implement the IBookAppService
    //{
    //    public BookAppService(IRepository<Book, Guid> repository)
    //        : base(repository)
    //    {

    //    }
    //}

    [Authorize("Author_Management")]
    public class BookAppService : ApplicationService, IBookAppService, IAuditingStore
    {
       // private readonly ProductManager _productManager;
        private readonly IRepository<Book, Guid> _bookRepository;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly EmailSendingJob _EmailSendingJob;
        private readonly IDataFilter _dataFilter;
        public BookAppService(IDataFilter dataFilter, IRepository<Book, Guid> repository, IBackgroundJobManager backgroundJobManager, EmailSendingJob emailSendingJob)
        {
             
               _dataFilter = dataFilter;
            //  _productManager = productManager;
            _backgroundJobManager = backgroundJobManager;
            _bookRepository = repository;
            _EmailSendingJob = emailSendingJob;
            EmailSendingArgs args = new EmailSendingArgs() { Body = "123", EmailAddress = "623444771@qq.com", Subject = "123" };
            _EmailSendingJob.Execute(args);
         //   var s=  RegisterAsync("623444771", "623444771@qq.com", "GL123@huawei");
        }
        public async Task<List<Book>> GetAllBooksIncludingDeletedAsync()
        {
            //Temporary disable the ISoftDelete filter
            using (_dataFilter.Disable<ISoftDelete>())
            {
                return await _bookRepository.GetListAsync();
            }
        }
        public async Task RegisterAsync(string userName, string emailAddress, string password)
        {
            //TODO: 创建一个新用户到数据库中...

            await _backgroundJobManager.EnqueueAsync(
                new EmailSendingArgs
                {
                    EmailAddress = emailAddress,
                    Subject = "You've successfully registered!",
                    Body = "..."
                }
            );
        }


        [Authorize("Author_Management_Create_Books")]
        public async Task<BookDto> CreateAsync(CreateUpdateBookDto input)
        {
             
            var book= await _bookRepository.InsertAsync(
                 new Book() { Price=input.Price,Type=input.Type, PublishDate=input.PublishDate, Name=input.Name }
             );
            book.SetProperty("Title", "My custom title value!");
            // await _bookRepository.UpdateAsync(book);
            // book.ChangeStockCount(1);

            //Task t = T1();
            //t.Wait();
           
            return ObjectMapper.Map<Book, BookDto>(book);
            


            //await _bookRepository.InsertAsync(book);
            //await UnitOfWorkManager.Current.SaveChangesAsync();
            //var id = book.Id;

        }
        //public   Task T1()
        //{
             
        //   return new Task(T2);
        //}
        //public void T2()
        //{ 
        
        //}
        public async Task DeleteAsync(Guid id)
        {
            await _bookRepository.DeleteAsync(id);
        }

        public async   Task<BookDto> GetAsync(Guid id)
        {
            var book = await _bookRepository.GetAsync(id);

            return ObjectMapper.Map<Book, BookDto>(book);
        }

        public async Task<ListResultDto<BookDto>> GetListAsync()
        {
            var books = await _bookRepository.GetListAsync();

            var bookList = ObjectMapper.Map<List<Book>, List<BookDto>>(books);

            return new ListResultDto<BookDto>(bookList);
            // throw new NotImplementedException();
        }
        private async Task NormalizeMaxResultCountAsync(PagedAndSortedResultRequestDto input)
        {
            //var maxPageSize = (await SettingProvider.GetOrNullAsync(ProductManagementSettings.MaxPageSize))?.To<int>();
            //if (maxPageSize.HasValue && input.MaxResultCount > maxPageSize.Value)
            //{
            //    input.MaxResultCount = maxPageSize.Value;
            //}
        }
        public async Task<PagedResultDto<BookDto>> GetListPagedAsync(PagedAndSortedResultRequestDto input)
        {
         //   await NormalizeMaxResultCountAsync(input);

            //var books = await _bookRepository
            //    .OrderBy(input.Sorting ?? "Name")
            //    .Skip(input.SkipCount)
            //    .Take(input.MaxResultCount)
            //    .ToListAsync();

            //var totalCount = await _bookRepository.GetCountAsync();

            //var dtos = ObjectMapper.Map<List<Book>, List<BookDto>>(books);

            //return new PagedResultDto<BookDto>(totalCount, dtos);
               throw new NotImplementedException();
        }

        public async Task<BookDto> UpdateAsync(Guid id, CreateUpdateBookDto input)
        {
            var book = await _bookRepository.GetAsync(id);

            book.Name = input.Name;
            book.Price = input.Price;
            book.Type = input.Type;
            book.PublishDate = input.PublishDate;
            

            return ObjectMapper.Map<Book, BookDto>(book);
        }

        public Task SaveAsync(AuditLogInfo auditInfo)
        {
             
            throw new NotImplementedException();
        }
    }

    public class MyAuditLogContributor : AuditLogContributor
    {
        public override void PreContribute(AuditLogContributionContext context)
        {
            var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();
            context.AuditInfo.SetProperty(
                "MyCustomClaimValue",
                currentUser.FindClaimValue("MyCustomClaim")
            );
        }

        public override void PostContribute(AuditLogContributionContext context)
        {
            context.AuditInfo.Comments.Add("Some comment...");
        }
    }

    
    [ExposeServices(typeof(IIdentityUserAppService), typeof(IdentityUserAppService), typeof(MyIdentityUserAppService))]
    public class MyIdentityUserAppService : IdentityUserAppService
    {
        //...
        public MyIdentityUserAppService(
            IdentityUserManager userManager,
            IIdentityUserRepository userRepository,
            IIdentityRoleRepository guidGenerator
        ):base(userManager, userRepository, guidGenerator,null)
        {
        }

        public async override Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
        {
            if (input.PhoneNumber.IsNullOrWhiteSpace())
            {
                throw new AbpValidationException(
                    "Phone number is required for new users!",
                    new List<ValidationResult>
                    {
                    new ValidationResult(
                        "Phone number can not be empty!",
                        new []{"PhoneNumber"}
                    )
                    }
                );
            }

            return await base.CreateAsync(input);
        }
    }


}
