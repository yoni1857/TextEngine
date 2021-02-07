using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YDK;
using System.IO;

namespace PackageCreatorTool
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length <= 5)
            {
                Package package = new Package();
                Console.Write("Author: ");
                package.Author = Console.ReadLine();
                Console.Write("Extract Path: ");
                package.ExtractPath = Console.ReadLine();
                Console.Write("Package Name: ");
                package.Name = Console.ReadLine();
                Console.Write("Package Version: ");
                package.Version = Console.ReadLine();
                Console.Write("Package Files Folder: ");
                package.GeneratePackage(Console.ReadLine(), "./");
            } else
            {
                Package package = new Package();
                package.Author = args[1];
                package.ExtractPath = args[2];
                package.Name = args[3];
                package.Version = args[5];
                package.GeneratePackage(args[6], "./");
            }
        }
    }
}
