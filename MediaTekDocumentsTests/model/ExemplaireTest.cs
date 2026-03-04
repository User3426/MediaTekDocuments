using System;
using MediaTekDocuments.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocumentsTests.model
{
	[TestClass]
	public class ExemplaireTest
	{
        [TestClass()]
        public class ExemplaireTests
        {
            private const int numero = 1;
            private const string photo = "images/ex001.jpg";
            private const string idEtat = "ETAT01";
            private const string idDocument = "10001";
            private static readonly DateTime dateAchat = new DateTime(2024, 6, 15);
            private readonly Exemplaire exemplaire = new Exemplaire(numero, dateAchat,
                photo, idEtat, idDocument);

            [TestMethod()]
            public void ExemplaireTest()
            {
                Assert.AreEqual(numero, exemplaire.Numero, "devrait réussir : numéro valorisé");
                Assert.AreEqual(dateAchat, exemplaire.DateAchat, "devrait réussir : date valorisée");
                Assert.AreEqual(photo, exemplaire.Photo, "devrait réussir : photo valorisée");
                Assert.AreEqual(idEtat, exemplaire.IdEtat, "devrait réussir : idEtat valorisé");
                Assert.AreEqual(idDocument, exemplaire.Id, "devrait réussir : idDocument valorisé");
            }
        }
    }
}
