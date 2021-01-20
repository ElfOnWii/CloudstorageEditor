using System;
using System.Diagnostics;

namespace CloudstorageEditorLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            var processes = Process.GetProcessesByName("Fiddler");
            if (processes.Length > 0)
                processes[0].Kill();

            Console.WriteLine("Cloudstorage Editor - @Ender#0001\n");
            Console.Write("Enter Exchange Code >");
            string exchange = Console.ReadLine();

            new Helper().Launch($"-AUTH_LOGIN=unused -AUTH_PASSWORD={exchange} -AUTH_TYPE=exchangecode").GetAwaiter().GetResult();
        }
    }
}