using MediaTekDocuments.dal;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.controller
{
    class FrmAuthentificationController
    {
        private readonly Access access;

        public FrmAuthentificationController()
        {
            access = Access.GetInstance();
        }

        public Utilisateur ControleAuthentification(Utilisateur utilisateur)
        {
            return access.ControleAuthentification(utilisateur);
        }
    }
}
