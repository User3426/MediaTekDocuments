using System;
using MediaTekDocuments.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocumentsTests.model
{
    [TestClass()]
    public class DocumentTests
    {
        private const string id = "00001";
        private const string titre = "Les Misérables";
        private const string image = "images/miserables.jpg";
        private const string idGenre = "GN001";
        private const string genre = "Roman";
        private const string idPublic = "PB001";
        private const string lePublic = "Adulte";
        private const string idRayon = "RY001";
        private const string rayon = "Littérature";
        private readonly Document document = new Document(id, titre, image,
            idGenre, genre, idPublic, lePublic, idRayon, rayon);

        [TestMethod()]
        public void DocumentTest()
        {
            Assert.AreEqual(id, document.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(titre, document.Titre, "devrait réussir : titre valorisé");
            Assert.AreEqual(image, document.Image, "devrait réussir : image valorisée");
            Assert.AreEqual(idGenre, document.IdGenre, "devrait réussir : idGenre valorisé");
            Assert.AreEqual(genre, document.Genre, "devrait réussir : genre valorisé");
            Assert.AreEqual(idPublic, document.IdPublic, "devrait réussir : idPublic valorisé");
            Assert.AreEqual(lePublic, document.Public, "devrait réussir : public valorisé");
            Assert.AreEqual(idRayon, document.IdRayon, "devrait réussir : idRayon valorisé");
            Assert.AreEqual(rayon, document.Rayon, "devrait réussir : rayon valorisé");
        }
    }
}
