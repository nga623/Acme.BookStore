﻿using Volo.Abp.Settings;

namespace Acme.BookStore.Settings
{
    public class BookStoreSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
             context.Add(
            new SettingDefinition("Smtp.Host", "127.0.0.1"),
            new SettingDefinition("Smtp.Port", "25"),
            new SettingDefinition("Smtp.UserName"),
            new SettingDefinition("Smtp.Password", isEncrypted: true),
            new SettingDefinition("Smtp.EnableSsl", "false")
        );
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(BookStoreSettings.MySetting1));
        }
    }
}
