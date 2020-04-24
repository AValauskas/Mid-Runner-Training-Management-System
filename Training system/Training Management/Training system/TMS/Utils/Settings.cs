using CodeMash.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS
{
    public class Settings
    {
            public static Guid ProjectId { get; set; } = Guid.Parse("813cac02-a70f-48c7-899c-67fc84676b64");
            public static string ApiKey { get; set; } = "KC6GsNe9v7g3U1jQPIqHCOfWWyplQB6A";

        //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

            public static string SendGridApiKeynežinau { get; set; } = "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";
            public static string SendGridApiKey { get; set; } = "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";
            public static string EmailConfirmationTemplateId { get; set; } = "d-2adb5e065a824d11bcf7e3e99deb3871";
            public static string PasswordResetEmailTemplateId { get; set; } = "PasswordResetEmailTemplateId";       
            public static string SenderEmailAddress { get; set; } = "aurimas19970704@gmail.com";
            public static string SenderEmailAddressName { get; set; } = "Aurimas";

    }
    
}
