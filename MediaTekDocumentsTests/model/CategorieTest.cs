using System;
using MediaTekDocuments.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocumentsTests.model
{
	[TestClass]
	public class CategorieTests
	{
        private const string id = "GN001";
        private const string libelle = "Horreur";
        private readonly Genre genre = new Genre(id, libelle);

        [TestMethod()]
        public void CategorieTest()
        {
            Assert.AreEqual(id, genre.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(libelle, genre.Libelle, "devrait réussir : libelle valorisé");
        }

        [TestMethod()]
        public void ToStringTest()
        {
            Assert.AreEqual(libelle, genre.ToString(), "devrait réussir : libelle retourné");
        }
    }
}
