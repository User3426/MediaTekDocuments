using MediaTekDocuments.controller;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MediaTekDocuments.view
{
    public partial class FrmAuthentification: Form
    {
        private FrmAuthentificationController controller;

        public FrmAuthentification()
        {
            InitializeComponent();
            controller = new FrmAuthentificationController();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text;
            string pwd = txtPwd.Text;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(pwd))
            {
                MessageBox.Show("Tous les champs doivent être remplis.", "Information");
                return;
            }

            Utilisateur utilisateur = new Utilisateur(login, pwd);
            Utilisateur result = controller.ControleAuthentification(utilisateur);

            if (result == null)
            {
                MessageBox.Show("Login ou mot de passe incorrect.", "Erreur d'authentification");
            }
            else if (result.Service == "Culture")
            {
                MessageBox.Show("Vos droits ne sont pas suffisants pour accéder à cette application.", "Accès refusé");
                Application.Exit();
            }
            else
            {
                FrmMediatek frm = new FrmMediatek(result.Service);
                frm.ShowDialog();
            }
        }
    }
}
