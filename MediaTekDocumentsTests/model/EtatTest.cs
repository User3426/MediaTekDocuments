using System;
using MediaTekDocuments.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocumentsTests.model
{
    [TestClass()]
    public class EtatTests
    {
        private const string id = "ETAT01";
        private const string libelle = "Neuf";
        private readonly Etat etat = new Etat(id, libelle);

        [TestMethod()]
        public void EtatTest()
        {
            Assert.AreEqual(id, etat.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(libelle, etat.Libelle, "devrait réussir : libelle valorisé");
        }
    }
}
