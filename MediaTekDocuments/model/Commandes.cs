using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Commande
    /// </summary>
    public class Commande
    {
        public int Id { get; set; }
        public DateTime DateCommande { get; set; }
        public decimal Montant { get; set; }

        public Commande(int id, DateTime dateCommande, decimal montant)
        {
            Id = id;
            DateCommande = dateCommande;
            Montant = montant;
        }
    }
}
