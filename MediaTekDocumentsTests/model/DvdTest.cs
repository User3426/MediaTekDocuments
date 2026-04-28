using System;
using MediaTekDocuments.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocumentsTests.model
{
	[TestClass]
	public class DvdTests
	{

        private const string id = "00100";
        private const string titre = "Intouchables";
        private const string image = "images/intouchables.jpg";
        private const int duree = 112;
        private const string realisateur = "Olivier Nakache";
        private const string synopsis = "Deux hommes que tout oppose.";
        private const string idGenre = "GN002";
        private const string genre = "Comédie";
        private const string idPublic = "PB001";
        private const string lePublic = "Adulte";
        private const string idRayon = "RY002";
        private const string rayon = "DVD";
        private readonly Dvd dvd = new Dvd(id, titre, image, duree, realisateur,
            synopsis, idGenre, genre, idPublic, lePublic, idRayon, rayon);

        [TestMethod()]
        public void DvdTest()
        {
            Assert.AreEqual(id, dvd.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(titre, dvd.Titre, "devrait réussir : titre valorisé");
            Assert.AreEqual(duree, dvd.Duree, "devrait réussir : duree valorisée");
            Assert.AreEqual(realisateur, dvd.Realisateur, "devrait réussir : realisateur valorisé");
            Assert.AreEqual(synopsis, dvd.Synopsis, "devrait réussir : synopsis valorisé");
        }

    }
}
