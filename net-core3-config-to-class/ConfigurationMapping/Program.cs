using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace ConfigurationMapping
{
    class Program
    {
        static void Main(string[] args)
        {
            Json_AppSettings();
            Json_AppSettings_Colon();
            Commandline_arguments();
            //Commandline_arguments_dict();
        }

        private static void Json_AppSettings()
        {
            //Keys are case-insensitive
            var config = new ConfigurationBuilder().AddJsonFile("rootconfig.json").Build();
            var rootConfig = config.Get<RootConfig>();

            Console.WriteLine(nameof(Json_AppSettings));
            Console.WriteLine(rootConfig.Section0.Key0);
            Console.WriteLine(rootConfig.Section0.Key1);
            Console.WriteLine(rootConfig.Section1.Key0);
            Console.WriteLine(rootConfig.Section1.Key1);
            Console.WriteLine();
        }

        private static void Json_AppSettings_Colon()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var reportConfig = config.Get<ReportConfig>();

            Console.WriteLine(nameof(Json_AppSettings_Colon));
            Console.WriteLine(reportConfig.ReportSheet.OutPutPath);
            Console.WriteLine(reportConfig.ReportSheet.PackageInfoList);
            Console.WriteLine(reportConfig.ReportSheet.ProjectsPackagesList);
            Console.WriteLine();
        }
        
        private static void Commandline_arguments()
        {
            var args = new string[] { "--user", "ghost", "--password=123456", "address=tw", "/mail", "ghost@everwhere.com", "/comment=ok" };

            //Keys are case-insensitive
            var config = new ConfigurationBuilder().AddCommandLine(args).Build();
            var rootConfig = config.Get<CommandlineArgumentsConfig>();

            Console.WriteLine(nameof(Commandline_arguments));
            Console.WriteLine(rootConfig.User);
            Console.WriteLine(rootConfig.Password);
            Console.WriteLine(rootConfig.Address);
            Console.WriteLine(rootConfig.Mail);
            Console.WriteLine(rootConfig.Comment);
            Console.WriteLine();
        }

        //private static void Commandline_arguments_dict()
        //{
        //    var args = new string[] { "-a=dictvalue0", "-b=dictvalue1" };

        //    var result = new Dictionary<string, string> { { "-a", "123" }, { "-b", "321" } };
        //    var config = new ConfigurationBuilder().AddCommandLine(args, result).Build();

        //    Console.WriteLine(nameof(Commandline_arguments_dict));
        //    foreach (var pair in result)
        //    {
        //        Console.WriteLine(pair);
        //    }
        //    Console.WriteLine();
        //}

    }
}
