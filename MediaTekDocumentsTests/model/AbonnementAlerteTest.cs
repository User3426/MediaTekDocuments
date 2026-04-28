using System;
using MediaTekDocuments.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocumentsTests.model
{
	[TestClass]
	public class AbonnementAlerteTests
	{

        private const string titre = "Le Monde";
        private static readonly DateTime dateFin = new DateTime(2025, 3, 20);
        private readonly AbonnementAlerte alerte = new AbonnementAlerte
        {
            Titre = titre,
            DateFinAbonnement = dateFin
        };

        [TestMethod()]
        public void AbonnementAlerteTest()
        {
            Assert.AreEqual(titre, alerte.Titre, "devrait réussir : titre valorisé");
            Assert.AreEqual(dateFin, alerte.DateFinAbonnement, "devrait réussir : date fin valorisée");
        }

    }
}
