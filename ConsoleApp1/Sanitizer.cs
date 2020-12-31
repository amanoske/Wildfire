using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace Wildfire
{
    class Sanitizer
    {
        public Sanitizer()
        {
            file_paths = new List<string>();
        }
        public Sanitizer(string file)
        {
            file_paths = new List<string>();
            file_paths.Add(file);
        }
        public Sanitizer(string[] file_paths_input)
        {
            file_paths = new List<string>();
            file_paths.AddRange(file_paths_input);
        }

        private byte[] Zero(byte[] input)
        {
            byte[] res = new byte[input.Length];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = 0;
            }
            return res;
        }

        private void ZeroFile(string path)
        {
            byte[] file_data = File.ReadAllBytes(path);
            File.WriteAllBytesAsync(path, Zero(file_data));
        }

        public int Sanitize (int times)
        {
            int files_destroyed = 0;
            for (int n = 0; n < times; n++)
            {

                for (int i = 0; i < file_paths.Count; i++)
                {
                    try
                    {
                        ZeroFile(file_paths[i]);
                        Console.Out.WriteLine("Destroyed file: " + file_paths[i]);
                        files_destroyed++;
                    }
                    //in the event the user has given a relative path vs an absolute path. 
                    catch (FileNotFoundException e)
                    {
                        try
                        {
                            ZeroFile(Directory.GetCurrentDirectory() + "\\" + file_paths[i]); //attempt to delete the file based on relative path
                        }
                        catch (Exception ex)
                        {
                            Console.Out.WriteLine("Failed to destroy file: " + file_paths[i] + " - file not found or accessible");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Out.WriteLine("Failed to destroy file: " + file_paths[i] + "Error: ");
                    }
                }
            }
            return files_destroyed;
        }

        public int Sanitize()
        {
            int files_destroyed = 0;
            for (int i = 0; i < file_paths.Count; i++)
            {
                try
                {
                    ZeroFile(file_paths[i]);
                    Console.Out.WriteLine("Destroyed file: " + file_paths[i]);
                    files_destroyed++;
                }
                //in the event the user has given a relative path vs an absolute path. 
                catch (FileNotFoundException e)
                {
                    try
                    {
                        ZeroFile(Directory.GetCurrentDirectory() + "\\" + file_paths[i]); //attempt to delete the file based on relative path
                    }
                    catch (Exception ex)
                    {
                        Console.Out.WriteLine("Failed to destroy file: " + file_paths[i] + " - file not found or accessible");
                    }
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine("Failed to destroy file: " + file_paths[i] + "Error: ");
                }
            }
            return files_destroyed;
        }




        List<string> file_paths;
    }
}
