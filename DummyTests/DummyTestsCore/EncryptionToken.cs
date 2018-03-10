using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using DummyTestsCore.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DummyTestsCore
{
    [TestClass]
    public class EncryptionToken
    {
        [TestMethod]
        public void TokenTest()
        {
            var cypher = new StringCipher(256, "FPxkQ9BNgaP4Xg9d");
            //var cypher = new StringCipher(256, "tu89geji340t89u2");
            //var cypher = new StringCipher(128, "FPxkQ9BNgaP4Xg9d");

            string salt = "hjk66hx3zMp4cn4vvzk7B";

            var tokenModel = new TokenModel()
            {
                EnrollmentId = 1247,
                AccountId = 765,
                RequestId = 63636
            };

            string tokenPlainText = JsonConvert.SerializeObject(tokenModel);
            string tokenEncrypted = cypher.Encrypt(tokenPlainText, salt);
            string token = tokenEncrypted.Substring(0, tokenEncrypted.Length - 2);

            string tokenEncoded = HttpUtility.UrlEncode(token);

            string tokenDecrypted = cypher.Decrypt($"{token}==", salt);
            Assert.AreEqual(tokenPlainText, tokenDecrypted);
        }
    }
}
