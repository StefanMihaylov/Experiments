using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DummyTestsCore.Encryption
{
    public class StringCipher
    {
        private int keySize;
        private string initializationVector;

        public StringCipher(int keySize, string initializationVector)
        {
            this.keySize = keySize;
            this.initializationVector = initializationVector;
        }   

        // This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.

        public string Encrypt(string plainText, string passPhrase)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            using (var password = new PasswordDeriveBytes(passPhrase, null))
            {
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Key = password.GetBytes(this.keySize / 8);
                    symmetricKey.IV = Encoding.ASCII.GetBytes(this.initializationVector);

                    using (var encryptor = symmetricKey.CreateEncryptor())
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();

                                byte[] cipherTextBytes = memoryStream.ToArray();
                                string cipherText = Convert.ToBase64String(cipherTextBytes);
                                return cipherText;
                            }
                        }
                    }
                }
            }
        }

        public string Decrypt(string cipherText, string passPhrase)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            using (var password = new PasswordDeriveBytes(passPhrase, null))
            {
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Key = password.GetBytes(this.keySize / 8);
                    symmetricKey.IV = Encoding.ASCII.GetBytes(this.initializationVector);

                    using (var decryptor = symmetricKey.CreateDecryptor())
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                                string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                                return plainText;
                            }
                        }
                    }
                }
            }
        }
    }
}
