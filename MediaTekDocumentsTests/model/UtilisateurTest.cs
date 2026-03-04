using System;
using MediaTekDocuments.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocumentsTests.model
{
	[TestClass]
	public class UtilisateurTest
	{
        [TestClass()]
        public class UtilisateurTests
        {
            private const string login = "jdupont";
            private const string pwd = "MonPwd123!";
            private readonly Utilisateur utilisateur = new Utilisateur(login, pwd);

            [TestMethod()]
            public void UtilisateurTest()
            {
                Assert.AreEqual(login, utilisateur.Login, "devrait réussir : login valorisé");
                Assert.AreEqual(pwd, utilisateur.Pwd, "devrait réussir : pwd valorisé");
            }
        }
    }
}
