using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.view.Tests
{
    [TestClass()]
    public class FrmMediatekTests
    {
        // Cas où la date est avant l'abonnement
        [TestMethod()]
        public void ParutionDansAbonnement_AvantAbonnement_RetourneFaux()
        {
            DateTime dateCommande = new DateTime(2023, 1, 1);
            DateTime dateFin = new DateTime(2025, 12, 31);
            DateTime dateParution = new DateTime(2022, 6, 15);
            bool resultat = FrmMediatek.ParutionDansAbonnement(dateCommande, dateFin, dateParution);
            Assert.IsFalse(resultat);
        }

        // Cas où la date est après l'abonnement
        [TestMethod()]
        public void ParutionDansAbonnement_ApresAbonnement_RetourneFaux()
        {
            DateTime dateCommande = new DateTime(2023, 1, 1);
            DateTime dateFin = new DateTime(2025, 12, 31);
            DateTime dateParution = new DateTime(2026, 1, 1);
            bool resultat = FrmMediatek.ParutionDansAbonnement(dateCommande, dateFin, dateParution);
            Assert.IsFalse(resultat);
        }

        // Cas limite : exactement à la date de début
        [TestMethod()]
        public void ParutionDansAbonnement_DateDebutExacte_RetourneVrai()
        {
            DateTime dateCommande = new DateTime(2023, 1, 1);
            DateTime dateFin = new DateTime(2025, 12, 31);
            bool resultat = FrmMediatek.ParutionDansAbonnement(dateCommande, dateFin, dateCommande);
            Assert.IsTrue(resultat);
        }

        // Cas limite : exactement à la date de fin
        [TestMethod()]
        public void ParutionDansAbonnement_DateFinExacte_RetourneVrai()
        {
            DateTime dateCommande = new DateTime(2023, 1, 1);
            DateTime dateFin = new DateTime(2025, 12, 31);
            bool resultat = FrmMediatek.ParutionDansAbonnement(dateCommande, dateFin, dateFin);
            Assert.IsTrue(resultat);
        }
    }
}