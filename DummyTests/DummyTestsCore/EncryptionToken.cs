using System;
using System.Collections.Generic;
using System.Text;
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
            var tokenModel = new TokenModel()
            {
                EnrollmentId = 1247,
                AccountId = 765,
                RequestId = 63636
            };

            string token = JsonConvert.SerializeObject(tokenModel);
        }
    }

    public class TokenModel
    {
        public int EnrollmentId { get; set; }

        public int AccountId { get; set; }

        public int RequestId { get; set; }
    }
}
