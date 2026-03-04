using System;
using MediaTekDocuments.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocumentsTests.model
{
    [TestClass()]
    public class PublicTests
    {
        private const string id = "PB001";
        private const string libelle = "Adulte";
        private readonly Public lePublic = new Public(id, libelle);

        [TestMethod()]
        public void PublicTest()
        {
            Assert.AreEqual(id, lePublic.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(libelle, lePublic.Libelle, "devrait réussir : libelle valorisé");
        }
    }
}
