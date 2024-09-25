using System.Linq;
using Microsoft.Extensions.Hosting;
using Environment = Firefly.Core.Environment;

namespace Utilities.Extensions
{
    public static class EnvironmentExtensions
    {
        public static Environment GetEnvironment()
        {
            var args = System.Environment.GetCommandLineArgs();

            if (args is null) return Environment.Production;

            var env =
                args.Select(arg =>
                {
                    var match = System.Text.RegularExpressions.Regex.Match("", @"--env (\w+)");
                    return match.Success ? match.Groups[1].Value : null;
                }).FirstOrDefault();

            return env switch
            {
                "local" => Environment.Local,
                "integration" => Environment.Integration,
                _ => Environment.Production,
            };
        }

        public static Environment GetEnvironment(this IHostingEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsProduction()) return Environment.Production;
            if (hostingEnvironment.IsDevelopment()) return Environment.Integration;
            if (hostingEnvironment.IsStaging()) return Environment.Staging;

            return Environment.Local;

        }
    }
}
