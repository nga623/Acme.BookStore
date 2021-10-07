using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace Acme.BookStore.Books
{
    public class Book : AuditedAggregateRoot<Guid> , ISoftDelete//, IMultiTenant
    {
        public string Name { get; set; }

        public BookType Type { get; set; }

        public DateTime PublishDate { get; set; }

        public float Price { get; set; }
        public bool IsDeleted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        //public Guid? TenantId => throw new NotImplementedException();

        public void ChangeStockCount(int newCount)
        {
         //   StockCount = newCount;

            //ADD an EVENT TO BE PUBLISHED
            AddLocalEvent(
                new StockCountChangedEvent
                {
                    ProductId = Id,
                    NewCount = newCount
                }
            );
        }
    }
    public class StockCountChangedEvent
    {
        public Guid ProductId { get; set; }

        public int NewCount { get; set; }
    }
    public class MyHandler
        : ILocalEventHandler<StockCountChangedEvent>,
          ITransientDependency
    {
        public async Task HandleEventAsync(StockCountChangedEvent eventData)
        {
            //TODO: your code that does somthing on the event
        }
    }
    public class MyHandler1
       : ILocalEventHandler<EntityCreatedEventData<Book>>,
         ITransientDependency
    {
        public async Task HandleEventAsync(
            EntityCreatedEventData<Book> eventData)
        {
            var userName = eventData.Entity.Name;
            
            //...
        }
    }
}
