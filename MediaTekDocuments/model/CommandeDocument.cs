using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    public class CommandeDocument : Commande
    {
        public int NbExemplaire { get; set; }
        public string IdLivreDvd { get; set; }
        public string IdSuivi { get; set; }
        public string LibelleSuivi { get; set; }

        public CommandeDocument(int id, DateTime dateCommande, decimal montant,
            int nbExemplaire, string idLivreDvd, string idSuivi, string libelleSuivi)
            : base(id, dateCommande, montant)
        {
            NbExemplaire = nbExemplaire;
            IdLivreDvd = idLivreDvd;
            IdSuivi = idSuivi;
            LibelleSuivi = libelleSuivi;
        }
    }
}
