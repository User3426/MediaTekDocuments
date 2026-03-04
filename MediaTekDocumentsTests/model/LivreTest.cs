using System;
using MediaTekDocuments.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocumentsTests.model
{
	[TestClass]
	public class LivreTest
	{
        [TestClass()]
        public class LivreTests
        {
            private const string id = "00001";
            private const string titre = "Les Misérables";
            private const string image = "images/miserables.jpg";
            private const string isbn = "978-2-07-036024-5";
            private const string auteur = "Victor Hugo";
            private const string collection = "Classiques";
            private const string idGenre = "GN001";
            private const string genre = "Roman";
            private const string idPublic = "PB001";
            private const string lePublic = "Adulte";
            private const string idRayon = "RY001";
            private const string rayon = "Littérature";
            private readonly Livre livre = new Livre(id, titre, image, isbn, auteur,
                collection, idGenre, genre, idPublic, lePublic, idRayon, rayon);

            [TestMethod()]
            public void LivreTest()
            {
                Assert.AreEqual(id, livre.Id, "devrait réussir : id valorisé");
                Assert.AreEqual(titre, livre.Titre, "devrait réussir : titre valorisé");
                Assert.AreEqual(isbn, livre.Isbn, "devrait réussir : isbn valorisé");
                Assert.AreEqual(auteur, livre.Auteur, "devrait réussir : auteur valorisé");
                Assert.AreEqual(collection, livre.Collection, "devrait réussir : collection valorisée");
            }
        }
    }
}
