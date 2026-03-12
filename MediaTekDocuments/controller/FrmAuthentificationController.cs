using MediaTekDocuments.dal;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmAuthentification
    /// </summary>
    class FrmAuthentificationController
    {
        /// <summary>
        /// Objet d'accès aux donnés
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux donnés
        /// </summary>
        public FrmAuthentificationController()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// Vérifie les identifiants de l'utilisateur et retourne l'objet correspondant
        /// </summary>
        /// <param name="utilisateur">objet Utilisateur contenant le login et le mot de passe</param>
        /// <returns></returns>
        public Utilisateur ControleAuthentification(Utilisateur utilisateur)
        {
            return access.ControleAuthentification(utilisateur);
        }
    }
}
