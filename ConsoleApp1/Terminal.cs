using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Wildfire
{
    class Terminal
    {
        static void Main(String[] args)
        {
            Console.Out.WriteLine("||==============================================||");
            Console.Out.WriteLine("||================= WILDFIRE ===================||");
            Console.Out.WriteLine("||==============================================||");
            Console.Out.WriteLine("");
            string[] files;

            if (args.Length == 0)
            {
                Console.Out.WriteLine("What is the path (e.g.: 'C:\file.txt') to the file I shall destroy?");
                Console.Out.WriteLine("For multiple files, delimitt paths by comma (',')");
                files = Console.ReadLine().Split(",");

                if (files.Length == 0)
                {
                    Console.Out.WriteLine("No files to destroy. Ending program.");
                    return;
                }
            }
            else
                files = args;

            Incinerator incinerator = new Incinerator(files);
            int files_deleted = incinerator.Incinerate();
            Console.Out.WriteLine("Files Destroyed: " + files_deleted);

        }
    }
}
