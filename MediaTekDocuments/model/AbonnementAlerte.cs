using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe pour les alertes d'abonnements proches de l'expiration
    /// </summary>
    public class AbonnementAlerte
    {
        public string Titre { get; set; }
        public DateTime DateFinAbonnement { get; set; }
    }
}
