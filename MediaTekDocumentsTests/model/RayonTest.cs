using System;
using MediaTekDocuments.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocumentsTests.model
{
    [TestClass()]
    public class RayonTests
    {
        private const string id = "RY001";
        private const string libelle = "Littérature";
        private readonly Rayon rayon = new Rayon(id, libelle);

        [TestMethod()]
        public void RayonTest()
        {
            Assert.AreEqual(id, rayon.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(libelle, rayon.Libelle, "devrait réussir : libelle valorisé");
        }
    }
}
