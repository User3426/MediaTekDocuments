using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe qui modélise un utilisateur de l'application
    /// </summary>
    public class Utilisateur
    {
        public string Login { get; }
        public string Pwd { get; }
        public string Service { get; set; }

        public Utilisateur(string login, string pwd)
        {
            Login = login;
            Pwd = pwd;
        }
    }
}
