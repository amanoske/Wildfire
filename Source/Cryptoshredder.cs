using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections;
using System.Collections.Generic;


namespace Wildfire
{
    public class Cryptoshredder
    {
        public Cryptoshredder()
        {
            file_paths = new List<string>();
        }
        public Cryptoshredder (string file)
        {
            file_paths = new List<string>();
            file_paths.Add(file);
        }
        public Cryptoshredder(string[] file_paths_input)
        {
            file_paths = new List<string>();
            file_paths.AddRange(file_paths_input);
        }

        private AesGcm GenerateCipher()
        {
            //key material
            byte[] byte_key = new byte[32]; //32 bytes  = 256 bits for AES 256 GCM
                                            
           //generate random keys
            RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();
            rand.GetBytes(byte_key);
           
            //generate cipher and encryptor
            return new AesGcm(byte_key);
         }

        private string EncryptString(string plain, AesGcm cipher)
        {
            //convert plaintext to bytes
            byte[] plaintext = Encoding.UTF8.GetBytes(plain);
            byte[] ciphertext = new byte[plaintext.Length];
            int cipherLengthSize = plaintext.Length;

            //setup RNG and return value
            RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();

            //key material sizes 
            int nonce_size = AesGcm.NonceByteSizes.MaxSize;
            int tagSize = AesGcm.TagByteSizes.MaxSize;


            //setup nonce and tag
            byte[] nonce = new byte[nonce_size];
            byte[] tag = new byte[tagSize];
            rand.GetBytes(nonce);
            rand.GetBytes(tag);

            cipher.Encrypt(nonce, plaintext, ciphertext, tag);

            return Convert.ToBase64String(ciphertext);
        }
        private byte[] EncryptBytes(byte[] plain, AesGcm cipher)
        {
            //convert plaintext to bytes
            byte[] plaintext = plain;
            byte[] ciphertext = new byte[plaintext.Length];
            int cipherLengthSize = plaintext.Length;

            //setup RNG and return value
            RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();

            //key material sizes 
            int nonce_size = AesGcm.NonceByteSizes.MaxSize;
            int tagSize = AesGcm.TagByteSizes.MaxSize;


            //setup nonce and tag
            byte[] nonce = new byte[nonce_size];
            byte[] tag = new byte[tagSize];
            rand.GetBytes(nonce);
            rand.GetBytes(tag);

           cipher.Encrypt(nonce, plaintext, ciphertext, tag);
           System.GC.Collect();
           return ciphertext;
        }

        private void CryptoshredFile(string path)
        {
            try
            {
                byte[] file_plaintext = File.ReadAllBytes(path);
                AesGcm cipher = GenerateCipher();
                byte[] file_ciphertext = EncryptBytes(file_plaintext, cipher);
                File.WriteAllBytesAsync(path, file_ciphertext);
            }
            catch(Exception e)
            {
                Console.Out.WriteLine("Encryption Failure: " + e.ToString());
            }
            File.Delete(path);
        }

        public int Cryptoshred()
        {
            int files_destroyed = 0;
            for (int i = 0; i < file_paths.Count; i++)
            {
                try
                {
                    CryptoshredFile(file_paths[i]);
                    Console.Out.WriteLine("Destroyed file: " + file_paths[i]);
                    files_destroyed++;
                }
                //in the event the user has given a relative path vs an absolute path. 
                catch (FileNotFoundException e)
                {
                    try
                    {
                        CryptoshredFile(Directory.GetCurrentDirectory() + "\\" + file_paths[i]); //attempt to delete the file based on relative path
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
