using System.Text;
using Microsoft.Extensions.Configuration;
using Utilities.Configurations;

namespace Utilities.Extensions
{
    public static class ResourceTypeExtensions
    {
        public static string GetConnectionStringCode(this ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.ServiceBus: return "sb";
                case ResourceType.EventGrid: return "eg";
                default: return "";
            }
        }

        public static string GetConnectionStringNamespace(this ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.ServiceBus: return "serviceebus";
            }

            return string.Empty;
        }

        public static string CreateConnectionString(this ResourceType resourceType, IConfiguration configuration, string configurationSection)
        {
            var section = configuration.GetSection(configurationSection);

            var sb = new StringBuilder("Endpoint=");
            sb.Append(resourceType.GetConnectionStringCode());
            sb.Append("resourceName");

            switch (resourceType)
            {
                case ResourceType.ServiceBus:
                    sb.Append("servicebus");
                    break;
            }

            // /subscriptions/a8b3f3c2-6772-4ae8-b717-46cf9608ada0/resourceGroups/service_bus_tests/providers/Microsoft.ServiceBus/namespaces/eavesdown/authorizationrules/RootManageSharedAccessKey
            sb.AppendJoin(";",
                [
                "windows.net/",
                "SharedAccessKeyName=",
                section.GetValue<string>("AccessKeyName"),
                "SharedAccessKey=",
                section.GetValue<string>("AccessKey"),

                ]
                );

            sb.Append("AccessKeyName");
            sb.Append(";");
            sb.Append("AccessKey");

            return sb.ToString();

        }
    }
}
