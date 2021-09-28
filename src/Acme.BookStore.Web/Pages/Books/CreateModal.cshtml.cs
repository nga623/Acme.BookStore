using System.Threading.Tasks;
using Acme.BookStore.Books;
using Acme.BookStore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Acme.BookStore.Web.Pages.Books
{
    public class CreateModalModel : BookStorePageModel
    {
        [BindProperty]
        public CreateUpdateBookDto Book { get; set; }
        private readonly MyOptions _options;
        private readonly IBookAppService _bookAppService;
        private readonly ITaxCalculator _taxCalculator;
        private readonly MyService _myService;
        private readonly IStringLocalizer<BookStoreResource> _localizer;
        public CreateModalModel(IBookAppService bookAppService, IStringLocalizer<BookStoreResource> localizer,MyService myService, IOptions<MyOptions> options, ITaxCalculator taxCalculator)
        {
            _bookAppService = bookAppService;
            _options = options.Value; //Notice the options.Value usage!
            _taxCalculator = taxCalculator; //构造函数注入 
            _myService = myService;
            _localizer = localizer;
        }

        public void OnGet()
        {
            Book = new CreateUpdateBookDto();
            var str = _localizer["AreYouSure"];
            _myService.DoSomething();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _bookAppService.CreateAsync(Book);
            return NoContent();
        }
    }
}
