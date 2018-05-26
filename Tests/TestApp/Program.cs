using System;
using MSTest.ASCOM.Utilities;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Tests...");
            // RunUtilTests();
            // RunTLTests();
            RunRepositoryTests();
        }

        static void RunUtilTests()
        {
            UtilTest test = new UtilTest();
            Console.WriteLine("Running Util_Instance");
            test.Util_Instance();
            Console.WriteLine("Running Util_Properties");
            test.Util_Properties();
        }
        static void RunTLTests()
        {
            TraceLoggerTest test = new TraceLoggerTest();
            Console.WriteLine("Running TraceLogger_InstanceTests");
            test.TraceLogger_InstanceTests();
        }
        static void RunRepositoryTests()
        {
            SystemHelperTest test = new SystemHelperTest();

            Console.WriteLine("Running Repository Tests");
            Console.WriteLine("\tAll Keys");
            test.RepoNodePaths();
            //test.AscomValues();
            //test.ListUserValues();
            //test.SingleUserValues();
        }
    }
}
