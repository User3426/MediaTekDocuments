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
        [TestMethod()]
        public void ParutionDansAbonnementTest()
        {
            DateTime dateCommande = new DateTime(2023, 1, 1);
            DateTime dateFin = new DateTime(2025, 12, 31);
            DateTime dateParution = new DateTime(2025, 6, 15);

            bool resultat = FrmMediatek.ParutionDansAbonnement(dateCommande, dateFin, dateParution);

            Assert.IsTrue(resultat);
        }
    }
}