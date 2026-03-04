using System;
using MediaTekDocuments.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocumentsTests.model
{
	[TestClass]
	public class RevueTest
	{
        [TestClass()]
        public class RevueTests
        {
            private const string id = "10001";
            private const string titre = "Science & Vie";
            private const string image = "images/sciencevie.jpg";
            private const string idGenre = "GN003";
            private const string genre = "Sciences";
            private const string idPublic = "PB001";
            private const string lePublic = "Adulte";
            private const string idRayon = "RY003";
            private const string rayon = "Revues";
            private const string periodicite = "MS";
            private const int delaiMiseADispo = 52;
            private readonly Revue revue = new Revue(id, titre, image, idGenre, genre,
                idPublic, lePublic, idRayon, rayon, periodicite, delaiMiseADispo);

            [TestMethod()]
            public void RevueTest()
            {
                Assert.AreEqual(id, revue.Id, "devrait réussir : id valorisé");
                Assert.AreEqual(titre, revue.Titre, "devrait réussir : titre valorisé");
                Assert.AreEqual(periodicite, revue.Periodicite, "devrait réussir : periodicite valorisée");
                Assert.AreEqual(delaiMiseADispo, revue.DelaiMiseADispo, "devrait réussir : delai valorisé");
            }
        }
    }
}
