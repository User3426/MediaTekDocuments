using System;
using MediaTekDocuments.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocumentsTests.model
{
    [TestClass()]
    public class CommandeTests
    {
        private const string id = "COM01";
        private const decimal montant = 150.00m;  
        private static readonly DateTime dateCommande = new DateTime(2025, 1, 10);
        private readonly Commande commande = new Commande(id, dateCommande, montant);

        [TestMethod()]
        public void CommandeTest()
        {
            Assert.AreEqual(id, commande.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(dateCommande, commande.DateCommande, "devrait réussir : date valorisée");
            Assert.AreEqual(montant, commande.Montant, "devrait réussir : montant valorisé");
        }
    }
}
