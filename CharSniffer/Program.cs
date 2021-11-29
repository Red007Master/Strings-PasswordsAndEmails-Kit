using System;

namespace CharSniffer2._0
{
    class Program
    {
        static void Main(string[] args)
        {
            Other.Initialization();

            Work.StartMain();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(new string('/', 40) + $"END|Counter=[{Publics.Counter}]" + new string(Convert.ToChar(@"\"), 40));
            Console.ReadLine();
        }
    }
}
