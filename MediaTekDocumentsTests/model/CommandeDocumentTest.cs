using System;
using MediaTekDocuments.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaTekDocumentsTests.model
{
	[TestClass]
	public class CommandeDocumentTests
	{

        private const string id = "COM02";
        private const decimal montant = 200.00m;
        private const int nbExemplaire = 3;
        private const string idLivreDvd = "00001";
        private const string idSuivi = "en_cours";
        private static readonly DateTime dateCommande = new DateTime(2025, 2, 5);
        private readonly CommandeDocument commandeDocument = new CommandeDocument(id,
            dateCommande, montant, nbExemplaire, idLivreDvd, idSuivi);

        [TestMethod()]
        public void CommandeDocumentTest()
        {
            Assert.AreEqual(id, commandeDocument.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(dateCommande, commandeDocument.DateCommande, "devrait réussir : date valorisée");
            Assert.AreEqual(montant, commandeDocument.Montant, "devrait réussir : montant valorisé");
            Assert.AreEqual(nbExemplaire, commandeDocument.NbExemplaire, "devrait réussir : nbExemplaire valorisé");
            Assert.AreEqual(idLivreDvd, commandeDocument.IdLivreDvd, "devrait réussir : idLivreDvd valorisé");
            Assert.AreEqual(idSuivi, commandeDocument.IdSuivi, "devrait réussir : idSuivi valorisé");
        }

    }
}
