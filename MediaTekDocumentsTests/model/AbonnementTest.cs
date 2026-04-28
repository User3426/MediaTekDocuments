using System;
using MediaTekDocuments.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocumentsTests.model
{
	[TestClass]
	public class AbonnementTests
	{

        private const string id = "AB001";
        private const decimal montant = 300.00m;
        private const string idRevue = "10001";
        private static readonly DateTime dateCommande = new DateTime(2025, 1, 1);
        private static readonly DateTime dateFin = new DateTime(2025, 12, 31);
        private readonly Abonnement abonnement = new Abonnement(id, dateCommande,
            montant, dateFin, idRevue);

        [TestMethod()]
        public void AbonnementTest()
        {
            Assert.AreEqual(id, abonnement.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(dateCommande, abonnement.DateCommande, "devrait réussir : date commande valorisée");
            Assert.AreEqual(montant, abonnement.Montant, "devrait réussir : montant valorisé");
            Assert.AreEqual(dateFin, abonnement.DateFinAbonnement, "devrait réussir : date fin valorisée");
            Assert.AreEqual(idRevue, abonnement.IdRevue, "devrait réussir : idRevue valorisé");
        }

    }
}
