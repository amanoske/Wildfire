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
            Console.Out.WriteLine("--------");
            Console.Out.WriteLine("Select a destruction method: ");
            Console.Out.WriteLine("1: Zeroing");
            Console.Out.WriteLine("2: Multi-pass Zeroing (e.g.: n-times write)");
            Console.Out.WriteLine("3: Cryptoshred via AES 256-GCM");
            Sanitizer sanitizer = null;
            Cryptoshredder shredder = null;
            int files_deleted = -1;
            switch (Int32.Parse(Console.ReadLine()))
            {
                case 1: 
                        sanitizer = new Sanitizer(files); 
                        files_deleted = sanitizer.Sanitize(); 
                        break;
                case 2: 
                        sanitizer = new Sanitizer(files); 
                        Console.Out.WriteLine("How many times would you like to zero each file?"); 
                        files_deleted = sanitizer.Sanitize(Int32.Parse(Console.ReadLine())); 
                        break;
                case 3: shredder = new Cryptoshredder(files); 
                        files_deleted = shredder.Cryptoshred(); 
                        break;
            }
            if (files_deleted > 0)
                Console.Out.WriteLine("Files Destroyed: " + files_deleted);
            else
                Console.Out.WriteLine("No procedure specified, quitting...");
        }
    }
}
