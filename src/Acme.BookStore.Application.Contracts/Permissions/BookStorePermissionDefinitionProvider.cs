using Acme.BookStore.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Acme.BookStore.Permissions
{
    public class BookStorePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(BookStorePermissions.GroupName);
           // myGroup.AddPermission("BookStore_Author_Create");


            var authorManagement = myGroup.AddPermission("Author_Management");
            authorManagement.AddChild("Author_Management_Create_Books");
            authorManagement.AddChild("Author_Management_Edit_Books");
            authorManagement.AddChild("Author_Management_Delete_Books");
            //Define your own permissions here. Example:
            //myGroup.AddPermission(BookStorePermissions.MyPermission1, L("Permission:MyPermission1"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<BookStoreResource>(name);
        }
    }
}
