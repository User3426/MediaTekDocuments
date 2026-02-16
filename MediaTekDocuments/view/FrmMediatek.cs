using System;
using System.Windows.Forms;
using MediaTekDocuments.model;
using MediaTekDocuments.controller;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;

namespace MediaTekDocuments.view

{
    /// <summary>
    /// Classe d'affichage
    /// </summary>
    public partial class FrmMediatek : Form
    {
        #region Commun
        private readonly FrmMediatekController controller;
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();

        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        internal FrmMediatek()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
        }

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories">liste des objets de type Genre ou Public ou Rayon</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        public void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }
        #endregion

        #region Onglet Livres
        private readonly BindingSource bdgLivresListe = new BindingSource();
        private List<Livre> lesLivres = new List<Livre>();
        private ModeLivre modeLivreActuel = ModeLivre.Consultation;
        private readonly BindingSource bdgGenresModif = new BindingSource();
        private readonly BindingSource bdgPublicsModif = new BindingSource();
        private readonly BindingSource bdgRayonsModif = new BindingSource();

        /// <summary>
        /// Modes d'édition possibles pour un livre
        /// </summary>
        private enum ModeLivre
        {
            Consultation,
            Modification,
            Ajout
        }

        /// <summary>
        /// Ouverture de l'onglet Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresRayons);

            RemplirComboCategorie(controller.GetAllGenres(), bdgGenresModif, cbxModifLivreGenre);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublicsModif, cbxModifLivrePublic);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayonsModif, cbxModifLivreRayon);

            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="livres">liste de livres</param>
        private void RemplirLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresListe.DataSource = bdgLivresListe;
            dgvLivresListe.Columns["isbn"].Visible = false;
            dgvLivresListe.Columns["idRayon"].Visible = false;
            dgvLivresListe.Columns["idGenre"].Visible = false;
            dgvLivresListe.Columns["idPublic"].Visible = false;
            dgvLivresListe.Columns["image"].Visible = false;
            dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresListe.Columns["id"].DisplayIndex = 0;
            dgvLivresListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRecherche.Text.Equals(""))
            {
                txbLivresTitreRecherche.Text = "";
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRecherche.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>() { livre };
                    RemplirLivresListe(livres);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbLivresTitreRecherche.Text.Equals(""))
            {
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                txbLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxLivresGenres.SelectedIndex < 0 && cbxLivresPublics.SelectedIndex < 0 && cbxLivresRayons.SelectedIndex < 0
                    && txbLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheLivresInfos(Livre livre)
        {
            txbLivresAuteur.Text = livre.Auteur;
            txbLivresCollection.Text = livre.Collection;
            txbLivresImage.Text = livre.Image;
            txbLivresIsbn.Text = livre.Isbn;
            txbLivresNumero.Text = livre.Id;
            txbLivresGenre.Text = livre.Genre;
            txbLivresPublic.Text = livre.Public;
            txbLivresRayon.Text = livre.Rayon;
            txbLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void VideLivresInfos()
        {
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresNumero.Text = "";
            txbLivresGenre.Text = "";
            txbLivresPublic.Text = "";
            txbLivresRayon.Text = "";
            txbLivresTitre.Text = "";
            pcbLivresImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresGenres.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresPublics.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresRayons.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListe(livres);
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheLivresInfos(livre);
                }
                catch
                {
                    VideLivresZones();
                }
            }
            else
            {
                VideLivresInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des livres
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres);
            VideLivresZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZones()
        {
            cbxLivresGenres.SelectedIndex = -1;
            cbxLivresRayons.SelectedIndex = -1;
            cbxLivresPublics.SelectedIndex = -1;
            txbLivresNumRecherche.Text = "";
            txbLivresTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList);
        }

        /// <summary>
        /// Demande suppression d'un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprLivre_Click(object sender, EventArgs e)
        {
            if (dgvLivresListe.SelectedRows.Count > 0)
            {
                Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                if (MessageBox.Show("Voulez-vous vraiment supprimer " + livre.Titre + " ?", "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (controller.DelLivre(livre))
                    {
                        lesLivres = controller.GetAllLivres();
                        RemplirLivresListeComplete();
                    }
                    else
                    {
                        MessageBox.Show("Suppression impossible, exemplaire ou commande lié au document", "Erreur");
                    }
                }
            }
            else
            {
                MessageBox.Show("Une ligne doit être sélectionnée.");
            }
        }

        /// <summary>
        /// Demande la modification d'un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDemandeModifLivre_Click(object sender, EventArgs e)
        {
            if (dgvLivresListe.SelectedRows.Count > 0)
            {
                Livre livre = (Livre)bdgLivresListe[bdgLivresListe.Position];
                GererModeLivre(ModeLivre.Modification, livre);
            }
            else
            {
                MessageBox.Show("Une ligne doit être sélectionnée.", "Information");
            }
        }

        /// <summary>
        /// Modification d'affichage en fonction du mode (consultation, modification ou ajout)
        /// </summary>
        /// <param name="mode">Le mode d'édition souhaité</param>
        /// <param name="livre">Le livre concerné (null pour un ajout)</param>
        private void GererModeLivre(ModeLivre mode, Livre livre)
        {
            modeLivreActuel = mode;
            bool enEdition = (mode == ModeLivre.Modification || mode == ModeLivre.Ajout);

            grpLivresRecherche.Enabled = !enEdition;
            txbLivresTitre.ReadOnly = !enEdition;
            txbLivresIsbn.ReadOnly = !enEdition;
            txbLivresAuteur.ReadOnly = !enEdition;
            txbLivresCollection.ReadOnly = !enEdition;
            txbLivresImage.ReadOnly = !enEdition;
            txbLivresRayon.Visible = !enEdition;
            cbxModifLivreRayon.Visible = enEdition;
            txbLivresPublic.Visible = !enEdition;
            cbxModifLivrePublic.Visible = enEdition;
            txbLivresGenre.Visible = !enEdition;
            cbxModifLivreGenre.Visible = enEdition;
            btnModifLivreValider.Visible = enEdition;
            btnModifLivreValider.Enabled = enEdition;
            btnModifLivreAnnuler.Visible = enEdition;
            btnModifLivreAnnuler.Enabled = enEdition;
            txbLivresNumero.ReadOnly = (mode != ModeLivre.Ajout);

            switch (mode)
            {
                case ModeLivre.Consultation:
                    grpLivresInfos.Text = "Informations détaillées";
                    if (livre != null)
                    {
                        AfficheLivresInfos(livre);
                    }
                    break;

                case ModeLivre.Modification:
                    grpLivresInfos.Text = "Modifier un Livre";
                    if (livre != null)
                    {
                        AfficheLivresInfos(livre);
                        cbxModifLivreGenre.SelectedIndex = cbxModifLivreGenre.FindStringExact(livre.Genre);
                        cbxModifLivrePublic.SelectedIndex = cbxModifLivrePublic.FindStringExact(livre.Public);
                        cbxModifLivreRayon.SelectedIndex = cbxModifLivreRayon.FindStringExact(livre.Rayon);
                    }
                    txbLivresTitre.Focus();
                    break;

                case ModeLivre.Ajout:
                    grpLivresInfos.Text = "Ajouter un nouveau Livre";
                    VideLivresInfos();
                    // Pré-sélectionner les premiers éléments des combos (ou laisser vide)
                    cbxModifLivreGenre.SelectedIndex = -1;
                    cbxModifLivrePublic.SelectedIndex = -1;
                    cbxModifLivreRayon.SelectedIndex = -1;
                    txbLivresNumero.Focus();
                    break;
            }
        }

        /// <summary>
        /// Annule la modification ou l'ajout d'un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifLivreAnnuler_Click(object sender, EventArgs e)
        {
            if (modeLivreActuel == ModeLivre.Modification && dgvLivresListe.CurrentCell != null)
            {
                Livre livre = (Livre)bdgLivresListe[bdgLivresListe.Position];
                GererModeLivre(ModeLivre.Consultation, livre);
            }
            else
            {
                GererModeLivre(ModeLivre.Consultation, null);
            }
        }

        /// <summary>
        /// Valide la modification ou l'ajout d'un livre
        /// </summary>
        private void btnModifLivreValider_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbLivresTitre.Text))
            {
                MessageBox.Show("Le titre est obligatoire.", "Information");
                txbLivresTitre.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txbLivresNumero.Text))
            {
                MessageBox.Show("Le numéro est obligatoire.", "Information");
                txbLivresNumero.Focus();
                return;
            }

            if (cbxModifLivreGenre.SelectedIndex < 0 ||
                cbxModifLivrePublic.SelectedIndex < 0 ||
                cbxModifLivreRayon.SelectedIndex < 0)
            {
                MessageBox.Show("Veuillez sélectionner un genre, un public et un rayon.", "Information");
                return;
            }

            Genre genre = (Genre)cbxModifLivreGenre.SelectedItem;
            Rayon rayon = (Rayon)cbxModifLivreRayon.SelectedItem;
            Public publ = (Public)cbxModifLivrePublic.SelectedItem;

            if (modeLivreActuel == ModeLivre.Modification)
            {
                Livre ancienLivre = (Livre)bdgLivresListe[bdgLivresListe.Position];

                Livre livreModifie = new Livre(
                    ancienLivre.Id,
                    txbLivresTitre.Text,
                    txbLivresImage.Text,
                    txbLivresIsbn.Text,
                    txbLivresAuteur.Text,
                    txbLivresCollection.Text,
                    genre.Id,
                    genre.Libelle,
                    publ.Id,
                    publ.Libelle,
                    rayon.Id,
                    rayon.Libelle
                );

                if (controller.UpdateLivre(livreModifie))
                {
                    lesLivres = controller.GetAllLivres();
                    RemplirLivresListeComplete();
                    GererModeLivre(ModeLivre.Consultation, livreModifie);
                    MessageBox.Show("Livre modifié avec succès.", "Information");
                }
                else
                {
                    MessageBox.Show("Erreur lors de la modification.", "Erreur");
                }
            }
            else if (modeLivreActuel == ModeLivre.Ajout)
            {
                if (lesLivres.Any(l => l.Id == txbLivresNumero.Text))
                {
                    MessageBox.Show("Ce numéro de livre existe déjà.", "Erreur");
                    txbLivresNumero.Focus();
                    return;
                }

                Livre nouveauLivre = new Livre(
                    txbLivresNumero.Text,
                    txbLivresTitre.Text,
                    txbLivresImage.Text,
                    txbLivresIsbn.Text,
                    txbLivresAuteur.Text,
                    txbLivresCollection.Text,
                    genre.Id,
                    genre.Libelle,
                    publ.Id,
                    publ.Libelle,
                    rayon.Id,
                    rayon.Libelle
                );

                if (controller.CreerLivre(nouveauLivre))
                {
                    lesLivres = controller.GetAllLivres();
                    RemplirLivresListeComplete();
                    GererModeLivre(ModeLivre.Consultation, nouveauLivre);
                    MessageBox.Show("Livre ajouté avec succès.", "Information");
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout du livre.", "Erreur");
                }
            }
        }
        /// <summary>
        /// Demande l'ajout d'un nouveau livre
        /// met le formulaire en mode ajout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAjouterLivre_Click(object sender, EventArgs e)
        {
            GererModeLivre(ModeLivre.Ajout, null);
        }

        #endregion

        #region Onglet Dvd
        private readonly BindingSource bdgDvdListe = new BindingSource();
        private List<Dvd> lesDvd = new List<Dvd>();
        private ModeDvd modeDvdActuel = ModeDvd.Consultation;

        /// <summary>
        /// Ouverture de l'onglet Dvds : 
        /// appel des méthodes pour remplir le datagrid des dvd et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayons);
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenresModif, cbxModifDvdGenre);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublicsModif, cbxModifDvdPublic);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayonsModif, cbxModifDvdRayon);
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Modes d'édition possibles pour un livre
        /// </summary>
        private enum ModeDvd
        {
            Consultation,
            Modification,
            Ajout
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="Dvds">liste de dvd</param>
        private void RemplirDvdListe(List<Dvd> Dvds)
        {
            bdgDvdListe.DataSource = Dvds;
            dgvDvdListe.DataSource = bdgDvdListe;
            dgvDvdListe.Columns["idRayon"].Visible = false;
            dgvDvdListe.Columns["idGenre"].Visible = false;
            dgvDvdListe.Columns["idPublic"].Visible = false;
            dgvDvdListe.Columns["image"].Visible = false;
            dgvDvdListe.Columns["synopsis"].Visible = false;
            dgvDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdListe.Columns["id"].DisplayIndex = 0;
            dgvDvdListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du Dvd dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRecherche.Text.Equals(""))
            {
                txbDvdTitreRecherche.Text = "";
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> Dvd = new List<Dvd>() { dvd };
                    RemplirDvdListe(Dvd);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirDvdListeComplete();
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des Dvd dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbDvdTitreRecherche.Text.Equals(""))
            {
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                txbDvdNumRecherche.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txbDvdTitreRecherche.Text.ToLower()));
                RemplirDvdListe(lesDvdParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxDvdGenres.SelectedIndex < 0 && cbxDvdPublics.SelectedIndex < 0 && cbxDvdRayons.SelectedIndex < 0
                    && txbDvdNumRecherche.Text.Equals(""))
                {
                    RemplirDvdListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd">le dvd</param>
        private void AfficheDvdInfos(Dvd dvd)
        {
            txbDvdRealisateur.Text = dvd.Realisateur;
            txbDvdSynopsis.Text = dvd.Synopsis;
            txbDvdImage.Text = dvd.Image;
            txbDvdDuree.Text = dvd.Duree.ToString();
            txbDvdNumero.Text = dvd.Id;
            txbDvdGenre.Text = dvd.Genre;
            txbDvdPublic.Text = dvd.Public;
            txbDvdRayon.Text = dvd.Rayon;
            txbDvdTitre.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDvdImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du dvd
        /// </summary>
        private void VideDvdInfos()
        {
            txbDvdRealisateur.Text = "";
            txbDvdSynopsis.Text = "";
            txbDvdImage.Text = "";
            txbDvdDuree.Text = "";
            txbDvdNumero.Text = "";
            txbDvdGenre.Text = "";
            txbDvdPublic.Text = "";
            txbDvdRayon.Text = "";
            txbDvdTitre.Text = "";
            pcbDvdImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdGenres.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Genre genre = (Genre)cbxDvdGenres.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdPublics.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Public lePublic = (Public)cbxDvdPublics.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdRayons.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxDvdRayons.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell != null)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    AfficheDvdInfos(dvd);
                }
                catch
                {
                    VideDvdZones();
                }
            }
            else
            {
                VideDvdInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des Dvd
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd);
            VideDvdZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideDvdZones()
        {
            cbxDvdGenres.SelectedIndex = -1;
            cbxDvdRayons.SelectedIndex = -1;
            cbxDvdPublics.SelectedIndex = -1;
            txbDvdNumRecherche.Text = "";
            txbDvdTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDvdZones();
            string titreColonne = dgvDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListe(sortedList);
        }

        private void btnSupprDvd_Click(object sender, EventArgs e)
        {
            if (dgvDvdListe.SelectedRows.Count > 0)
            {
                Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                if (MessageBox.Show("Voulez-vous vraiment supprimer " + dvd.Titre + " ?", "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (controller.DelDvd(dvd))
                    {
                        lesDvd = controller.GetAllDvd();
                        RemplirDvdListeComplete();
                    }
                    else
                    {
                        MessageBox.Show("Suppression impossible, exemplaire ou commande lié au document", "Erreur");
                    }
                }
            }
            else
            {
                MessageBox.Show("Une ligne doit être sélectionnée.");
            }
        }

        /// <summary>
        /// Modification d'affichage en fonction du mode (consultation, modification ou ajout)
        /// </summary>
        /// <param name="mode">Le mode d'édition souhaité</param>
        /// <param name="dvd">Le dvd concerné (null pour un ajout)</param>
        private void GererModeDvd(ModeDvd mode, Dvd dvd)
        {
            modeDvdActuel = mode;
            bool enEdition = (mode == ModeDvd.Modification || mode == ModeDvd.Ajout);

            grpDvdRecherche.Enabled = !enEdition;
            txbDvdTitre.ReadOnly = !enEdition;
            txbDvdDuree.ReadOnly = !enEdition;
            txbDvdRealisateur.ReadOnly = !enEdition;
            txbDvdSynopsis.ReadOnly = !enEdition;
            txbDvdImage.ReadOnly = !enEdition;
            txbDvdRayon.Visible = !enEdition;
            cbxModifDvdRayon.Visible = enEdition;
            txbDvdPublic.Visible = !enEdition;
            cbxModifDvdPublic.Visible = enEdition;
            txbDvdGenre.Visible = !enEdition;
            cbxModifDvdGenre.Visible = enEdition;
            btnModifDvdValider.Visible = enEdition;
            btnModifDvdValider.Enabled = enEdition;
            btnModifDvdAnnuler.Visible = enEdition;
            btnModifDvdAnnuler.Enabled = enEdition;
            txbDvdNumero.ReadOnly = (mode != ModeDvd.Ajout);

            switch (mode)
            {
                case ModeDvd.Consultation:
                    grpDvdInfos.Text = "Informations détaillées";
                    if (dvd != null)
                    {
                        AfficheDvdInfos(dvd);
                    }
                    break;

                case ModeDvd.Modification:
                    grpDvdInfos.Text = "Modifier un Dvd";
                    if (dvd != null)
                    {
                        AfficheDvdInfos(dvd);
                        cbxModifDvdGenre.SelectedIndex = cbxModifDvdGenre.FindStringExact(dvd.Genre);
                        cbxModifDvdPublic.SelectedIndex = cbxModifDvdPublic.FindStringExact(dvd.Public);
                        cbxModifDvdRayon.SelectedIndex = cbxModifDvdRayon.FindStringExact(dvd.Rayon);
                    }
                    txbDvdTitre.Focus();
                    break;

                case ModeDvd.Ajout:
                    grpDvdInfos.Text = "Ajouter un nouveau Dvd";
                    VideDvdInfos();
                    cbxModifDvdGenre.SelectedIndex = -1;
                    cbxModifDvdPublic.SelectedIndex = -1;
                    cbxModifDvdRayon.SelectedIndex = -1;
                    txbDvdNumero.Focus();
                    break;
            }
        }

        /// <summary>
        /// Demande la modification d'un dvd
        /// ouvre le formulaire de modification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifDvd_Click(object sender, EventArgs e)
        {
            if (dgvDvdListe.SelectedRows.Count > 0)
            {
                Dvd dvd = (Dvd)bdgDvdListe[bdgDvdListe.Position];
                GererModeDvd(ModeDvd.Modification, dvd);
            }
            else
            {
                MessageBox.Show("Une ligne doit être sélectionnée.", "Information");
            }
        }

        private void btnModifDvdAnnuler_Click(object sender, EventArgs e)
        {
            if (modeDvdActuel == ModeDvd.Modification && dgvDvdListe.CurrentCell != null)
            {
                Dvd dvd = (Dvd)bdgDvdListe[bdgDvdListe.Position];
                GererModeDvd(ModeDvd.Consultation, dvd);
            }
            else
            {
                GererModeDvd(ModeDvd.Consultation, null);
            }
        }

        private void btnModifDvdValider_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbDvdTitre.Text))
            {
                MessageBox.Show("Le titre est obligatoire.", "Information");
                txbDvdTitre.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txbDvdNumero.Text))
            {
                MessageBox.Show("Le numéro est obligatoire.", "Information");
                txbDvdNumero.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txbDvdDuree.Text))
            {
                MessageBox.Show("La durée est obligatoire.", "Information");
                txbDvdDuree.Focus();
                return;
            }

            if (cbxModifDvdGenre.SelectedIndex < 0 ||
                cbxModifDvdPublic.SelectedIndex < 0 ||
                cbxModifDvdRayon.SelectedIndex < 0)
            {
                MessageBox.Show("Veuillez sélectionner un genre, un public et un rayon.", "Information");
                return;
            }

            Genre genre = (Genre)cbxModifDvdGenre.SelectedItem;
            Rayon rayon = (Rayon)cbxModifDvdRayon.SelectedItem;
            Public publ = (Public)cbxModifDvdPublic.SelectedItem;

            if (modeDvdActuel == ModeDvd.Modification)
            {
                Dvd ancienDvd = (Dvd)bdgDvdListe[bdgDvdListe.Position];

                Dvd dvdModifie = new Dvd(
                    ancienDvd.Id,
                    txbDvdTitre.Text,
                    txbDvdImage.Text,
                    int.Parse(txbDvdDuree.Text),
                    txbDvdRealisateur.Text,
                    txbDvdSynopsis.Text,
                    genre.Id,
                    genre.Libelle,
                    publ.Id,
                    publ.Libelle,
                    rayon.Id,
                    rayon.Libelle
                );

                if (controller.UpdateDvd(dvdModifie))
                {
                    lesDvd = controller.GetAllDvd();
                    RemplirDvdListeComplete();
                    GererModeDvd(ModeDvd.Consultation, dvdModifie);
                    MessageBox.Show("DVD modifié avec succès.", "Information");
                }
                else
                {
                    MessageBox.Show("Erreur lors de la modification.", "Erreur");
                }
            }
            else if (modeDvdActuel == ModeDvd.Ajout)
            {
                if (lesDvd.Any(d => d.Id == txbDvdNumero.Text))
                {
                    MessageBox.Show("Ce numéro de DVD existe déjà.", "Erreur");
                    txbDvdNumero.Focus();
                    return;
                }

                Dvd nouveauDvd = new Dvd(
                    txbDvdNumero.Text,
                    txbDvdTitre.Text,
                    txbDvdImage.Text,
                    int.Parse(txbDvdDuree.Text),
                    txbDvdRealisateur.Text,
                    txbDvdSynopsis.Text,
                    genre.Id,
                    genre.Libelle,
                    publ.Id,
                    publ.Libelle,
                    rayon.Id,
                    rayon.Libelle
                );

                if (controller.CreerDvd(nouveauDvd))
                {
                    lesDvd = controller.GetAllDvd();
                    RemplirDvdListeComplete();
                    GererModeDvd(ModeDvd.Consultation, nouveauDvd);
                    MessageBox.Show("DVD ajouté avec succès.", "Information");
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout du DVD.", "Erreur");
                }
            }
        }

        private void btnAjouterDvd_Click(object sender, EventArgs e)
        {
            GererModeDvd(ModeDvd.Ajout, null);
        }


        #endregion

        #region Onglet Revues
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private List<Revue> lesRevues = new List<Revue>();
        private ModeRevue modeRevueActuel = ModeRevue.Consultation;
        private readonly BindingSource bdgGenresModifRevue = new BindingSource();
        private readonly BindingSource bdgPublicsModifRevue = new BindingSource();
        private readonly BindingSource bdgRayonsModifRevue = new BindingSource();

        /// <summary>
        /// Modes d'édition possibles pour une revue
        /// </summary>
        private enum ModeRevue
        {
            Consultation,
            Modification,
            Ajout
        }

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayons);
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenresModifRevue, cbxModifRevueGenre);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublicsModifRevue, cbxModifRevuePublic);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayonsModifRevue, cbxModifRevueRayon);
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="revues"></param>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevuesListe.DataSource = bdgRevuesListe;
            dgvRevuesListe.Columns["idRayon"].Visible = false;
            dgvRevuesListe.Columns["idGenre"].Visible = false;
            dgvRevuesListe.Columns["idPublic"].Visible = false;
            dgvRevuesListe.Columns["image"].Visible = false;
            dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevuesListe.Columns["id"].DisplayIndex = 0;
            dgvRevuesListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumRecherche.Text.Equals(""))
            {
                txbRevuesTitreRecherche.Text = "";
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>() { revue };
                    RemplirRevuesListe(revues);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des revues dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbRevuesTitreRecherche.Text.Equals(""))
            {
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                txbRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbRevuesTitreRecherche.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxRevuesGenres.SelectedIndex < 0 && cbxRevuesPublics.SelectedIndex < 0 && cbxRevuesRayons.SelectedIndex < 0
                    && txbRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirRevuesListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheRevuesInfos(Revue revue)
        {
            txbRevuesPeriodicite.Text = revue.Periodicite;
            txbRevuesImage.Text = revue.Image;
            txbRevuesDateMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbRevuesNumero.Text = revue.Id;
            txbRevuesGenre.Text = revue.Genre;
            txbRevuesPublic.Text = revue.Public;
            txbRevuesRayon.Text = revue.Rayon;
            txbRevuesTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la reuve
        /// </summary>
        private void VideRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            txbRevuesImage.Text = "";
            txbRevuesDateMiseADispo.Text = "";
            txbRevuesNumero.Text = "";
            txbRevuesGenre.Text = "";
            txbRevuesPublic.Text = "";
            txbRevuesRayon.Text = "";
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxRevuesPublics.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxRevuesRayons.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    AfficheRevuesInfos(revue);
                }
                catch
                {
                    VideRevuesZones();
                }
            }
            else
            {
                VideRevuesInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
            VideRevuesZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevuesZones()
        {
            cbxRevuesGenres.SelectedIndex = -1;
            cbxRevuesRayons.SelectedIndex = -1;
            cbxRevuesPublics.SelectedIndex = -1;
            txbRevuesNumRecherche.Text = "";
            txbRevuesTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevuesZones();
            string titreColonne = dgvRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Revue> sortedList = new List<Revue>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                    break;
                case "Periodicite":
                    sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                    break;
                case "DelaiMiseADispo":
                    sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                    break;
                case "Genre":
                    sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirRevuesListe(sortedList);
        }

        private void btnSupprRevue_Click(object sender, EventArgs e)
        {
            if (dgvRevuesListe.SelectedRows.Count > 0)
            {
                Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                if (MessageBox.Show("Voulez-vous vraiment supprimer " + revue.Titre + " ?", "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (controller.DelRevue(revue))
                    {
                        lesRevues = controller.GetAllRevues();
                        RemplirRevuesListeComplete();
                    }
                    else
                    {
                        MessageBox.Show("Suppression impossible, exemplaire lié à la revue", "Erreur");
                    }
                }
            }
            else
            {
                MessageBox.Show("Une ligne doit être sélectionnée.");
            }
        }

        /// <summary>
        /// Modification d'affichage en fonction du mode (consultation, modification ou ajout)
        /// </summary>
        /// <param name="mode">Le mode d'édition souhaité</param>
        /// <param name="revue">La revue concernée (null pour un ajout)</param>
        private void GererModeRevue(ModeRevue mode, Revue revue)
        {
            modeRevueActuel = mode;
            bool enEdition = (mode == ModeRevue.Modification || mode == ModeRevue.Ajout);

            grpRevuesRecherche.Enabled = !enEdition;
            txbRevuesTitre.ReadOnly = !enEdition;
            txbRevuesPeriodicite.ReadOnly = !enEdition;
            txbRevuesDateMiseADispo.ReadOnly = !enEdition;
            txbRevuesImage.ReadOnly = !enEdition;

            txbRevuesRayon.Visible = !enEdition;
            cbxModifRevueRayon.Visible = enEdition;
            txbRevuesPublic.Visible = !enEdition;
            cbxModifRevuePublic.Visible = enEdition;
            txbRevuesGenre.Visible = !enEdition;
            cbxModifRevueGenre.Visible = enEdition;

            btnModifRevueValider.Visible = enEdition;
            btnModifRevueValider.Enabled = enEdition;
            btnModifRevueAnnuler.Visible = enEdition;
            btnModifRevueAnnuler.Enabled = enEdition;

            txbRevuesNumero.ReadOnly = (mode != ModeRevue.Ajout);

            switch (mode)
            {
                case ModeRevue.Consultation:
                    grpRevuesInfos.Text = "Informations détaillées";
                    if (revue != null)
                    {
                        AfficheRevuesInfos(revue);
                    }
                    break;

                case ModeRevue.Modification:
                    grpRevuesInfos.Text = "Modifier une Revue";
                    if (revue != null)
                    {
                        AfficheRevuesInfos(revue);
                        cbxModifRevueGenre.SelectedIndex = cbxModifRevueGenre.FindStringExact(revue.Genre);
                        cbxModifRevuePublic.SelectedIndex = cbxModifRevuePublic.FindStringExact(revue.Public);
                        cbxModifRevueRayon.SelectedIndex = cbxModifRevueRayon.FindStringExact(revue.Rayon);
                    }
                    txbRevuesTitre.Focus();
                    break;

                case ModeRevue.Ajout:
                    grpRevuesInfos.Text = "Ajouter une nouvelle Revue";
                    VideRevuesInfos();
                    cbxModifRevueGenre.SelectedIndex = -1;
                    cbxModifRevuePublic.SelectedIndex = -1;
                    cbxModifRevueRayon.SelectedIndex = -1;
                    txbRevuesNumero.Focus();
                    break;
            }
        }

        private void btnModifRevueAnnuler_Click(object sender, EventArgs e)
        {
            if (modeRevueActuel == ModeRevue.Modification && dgvRevuesListe.CurrentCell != null)
            {
                Revue revue = (Revue)bdgRevuesListe[bdgRevuesListe.Position];
                GererModeRevue(ModeRevue.Consultation, revue);
            }
            else
            {
                GererModeRevue(ModeRevue.Consultation, null);
            }
        }

        private void btnAjouterRevue_Click(object sender, EventArgs e)
        {
            GererModeRevue(ModeRevue.Ajout, null);
        }

        private void btnModifRevue_Click(object sender, EventArgs e)
        {
            if (dgvRevuesListe.SelectedRows.Count > 0)
            {
                Revue revue = (Revue)bdgRevuesListe[bdgRevuesListe.Position];
                GererModeRevue(ModeRevue.Modification, revue);
            }
            else
            {
                MessageBox.Show("Une ligne doit être sélectionnée.", "Information");
            }
        }

        /// <summary>
        /// Valide la modification ou l'ajout d'une revue
        /// </summary>
        private void btnModifRevueValider_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbRevuesTitre.Text))
            {
                MessageBox.Show("Le titre est obligatoire.", "Information");
                txbRevuesTitre.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txbRevuesNumero.Text))
            {
                MessageBox.Show("Le numéro est obligatoire.", "Information");
                txbRevuesNumero.Focus();
                return;
            }

            if (cbxModifRevueGenre.SelectedIndex < 0 ||
                cbxModifRevuePublic.SelectedIndex < 0 ||
                cbxModifRevueRayon.SelectedIndex < 0)
            {
                MessageBox.Show("Veuillez sélectionner un genre, un public et un rayon.", "Information");
                return;
            }

            // Validation du délai de mise à disposition (doit être un nombre)
            if (!int.TryParse(txbRevuesDateMiseADispo.Text, out int delaiMiseADispo))
            {
                MessageBox.Show("Le délai de mise à disposition doit être un nombre.", "Information");
                txbRevuesDateMiseADispo.Focus();
                return;
            }

            Genre genre = (Genre)cbxModifRevueGenre.SelectedItem;
            Rayon rayon = (Rayon)cbxModifRevueRayon.SelectedItem;
            Public publ = (Public)cbxModifRevuePublic.SelectedItem;

            if (modeRevueActuel == ModeRevue.Modification)
            {
                Revue ancienneRevue = (Revue)bdgRevuesListe[bdgRevuesListe.Position];

                Revue revueModifiee = new Revue(
                    ancienneRevue.Id,
                    txbRevuesTitre.Text,
                    txbRevuesImage.Text,
                    genre.Id,
                    genre.Libelle,
                    publ.Id,
                    publ.Libelle,
                    rayon.Id,
                    rayon.Libelle,
                    txbRevuesPeriodicite.Text,
                    delaiMiseADispo
                );

                if (controller.UpdateRevue(revueModifiee))
                {
                    lesRevues = controller.GetAllRevues();
                    RemplirRevuesListeComplete();
                    GererModeRevue(ModeRevue.Consultation, revueModifiee);
                    MessageBox.Show("Revue modifiée avec succès.", "Information");
                }
                else
                {
                    MessageBox.Show("Erreur lors de la modification.", "Erreur");
                }
            }
            else if (modeRevueActuel == ModeRevue.Ajout)
            {
                if (lesRevues.Any(r => r.Id == txbRevuesNumero.Text))
                {
                    MessageBox.Show("Ce numéro de revue existe déjà.", "Erreur");
                    txbRevuesNumero.Focus();
                    return;
                }

                Revue nouvelleRevue = new Revue(
                    txbRevuesNumero.Text,
                    txbRevuesTitre.Text,
                    txbRevuesImage.Text,
                    genre.Id,
                    genre.Libelle,
                    publ.Id,
                    publ.Libelle,
                    rayon.Id,
                    rayon.Libelle,
                    txbRevuesPeriodicite.Text,
                    delaiMiseADispo
                );

                if (controller.CreerRevue(nouvelleRevue))
                {
                    lesRevues = controller.GetAllRevues();
                    RemplirRevuesListeComplete();
                    GererModeRevue(ModeRevue.Consultation, nouvelleRevue);
                    MessageBox.Show("Revue ajoutée avec succès.", "Information");
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout de la revue.", "Erreur");
                }
            }
        }

        #endregion

        #region Onglet Paarutions
        private readonly BindingSource bdgExemplairesListe = new BindingSource();
        private List<Exemplaire> lesExemplaires = new List<Exemplaire>();
        const string ETATNEUF = "00001";

        /// <summary>
        /// Ouverture de l'onglet : récupère le revues et vide tous les champs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            txbReceptionRevueNumero.Text = "";
        }

        /// <summary>
        /// Remplit le dategrid des exemplaires avec la liste reçue en paramètre
        /// </summary>
        /// <param name="exemplaires">liste d'exemplaires</param>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            if (exemplaires != null)
            {
                bdgExemplairesListe.DataSource = exemplaires;
                dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;
                dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
                dgvReceptionExemplairesListe.Columns["id"].Visible = false;
                dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
                dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
            }
            else
            {
                bdgExemplairesListe.DataSource = null;
            }
        }

        /// <summary>
        /// Recherche d'un numéro de revue et affiche ses informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionRechercher_Click(object sender, EventArgs e)
        {
            if (!txbReceptionRevueNumero.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbReceptionRevueNumero.Text));
                if (revue != null)
                {
                    AfficheReceptionRevueInfos(revue);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue son aussi effacées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbReceptionRevueNumero_TextChanged(object sender, EventArgs e)
        {
            txbReceptionRevuePeriodicite.Text = "";
            txbReceptionRevueImage.Text = "";
            txbReceptionRevueDelaiMiseADispo.Text = "";
            txbReceptionRevueGenre.Text = "";
            txbReceptionRevuePublic.Text = "";
            txbReceptionRevueRayon.Text = "";
            txbReceptionRevueTitre.Text = "";
            pcbReceptionRevueImage.Image = null;
            RemplirReceptionExemplairesListe(null);
            AccesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée et les exemplaires
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheReceptionRevueInfos(Revue revue)
        {
            // informations sur la revue
            txbReceptionRevuePeriodicite.Text = revue.Periodicite;
            txbReceptionRevueImage.Text = revue.Image;
            txbReceptionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbReceptionRevueNumero.Text = revue.Id;
            txbReceptionRevueGenre.Text = revue.Genre;
            txbReceptionRevuePublic.Text = revue.Public;
            txbReceptionRevueRayon.Text = revue.Rayon;
            txbReceptionRevueTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbReceptionRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbReceptionRevueImage.Image = null;
            }
            // affiche la liste des exemplaires de la revue
            AfficheReceptionExemplairesRevue();
        }

        /// <summary>
        /// Récupère et affiche les exemplaires d'une revue
        /// </summary>
        private void AfficheReceptionExemplairesRevue()
        {
            string idDocuement = txbReceptionRevueNumero.Text;
            lesExemplaires = controller.GetExemplairesRevue(idDocuement);
            RemplirReceptionExemplairesListe(lesExemplaires);
            AccesReceptionExemplaireGroupBox(true);
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'un exemplaire
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces">true ou false</param>
        private void AccesReceptionExemplaireGroupBox(bool acces)
        {
            grpReceptionExemplaire.Enabled = acces;
            txbReceptionExemplaireImage.Text = "";
            txbReceptionExemplaireNumero.Text = "";
            pcbReceptionExemplaireImage.Image = null;
            dtpReceptionExemplaireDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Recherche image sur disque (pour l'exemplaire à insérer)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireImage_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                // positionnement à la racine du disque où se trouve le dossier actuel
                InitialDirectory = Path.GetPathRoot(Environment.CurrentDirectory),
                Filter = "Files|*.jpg;*.bmp;*.jpeg;*.png;*.gif"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            txbReceptionExemplaireImage.Text = filePath;
            try
            {
                pcbReceptionExemplaireImage.Image = Image.FromFile(filePath);
            }
            catch
            {
                pcbReceptionExemplaireImage.Image = null;
            }
        }

        /// <summary>
        /// Enregistrement du nouvel exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireValider_Click(object sender, EventArgs e)
        {
            if (!txbReceptionExemplaireNumero.Text.Equals(""))
            {
                try
                {
                    int numero = int.Parse(txbReceptionExemplaireNumero.Text);
                    DateTime dateAchat = dtpReceptionExemplaireDate.Value;
                    string photo = txbReceptionExemplaireImage.Text;
                    string idEtat = ETATNEUF;
                    string idDocument = txbReceptionRevueNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
                    if (controller.CreerExemplaire(exemplaire))
                    {
                        AfficheReceptionExemplairesRevue();
                    }
                    else
                    {
                        MessageBox.Show("numéro de publication déjà existant", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("le numéro de parution doit être numérique", "Information");
                    txbReceptionExemplaireNumero.Text = "";
                    txbReceptionExemplaireNumero.Focus();
                }
            }
            else
            {
                MessageBox.Show("numéro de parution obligatoire", "Information");
            }
        }

        /// <summary>
        /// Tri sur une colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvReceptionExemplairesListe.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Photo":
                    sortedList = lesExemplaires.OrderBy(o => o.Photo).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList);
        }

        /// <summary>
        /// affichage de l'image de l'exemplaire suite à la sélection d'un exemplaire dans la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null)
            {
                Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];
                string image = exemplaire.Photo;
                try
                {
                    pcbReceptionExemplaireRevueImage.Image = Image.FromFile(image);
                }
                catch
                {
                    pcbReceptionExemplaireRevueImage.Image = null;
                }
            }
            else
            {
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }





        #endregion

        #region Onglet Paarutions
        private readonly BindingSource bdgCommandes = new BindingSource();
        private List<CommandeDocument> lesCommandes = new List<CommandeDocument>();
        const string SUIVIENCOURS = "00001";

        private void tabCommandeLivre_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            txbReceptionRevueNumero.Text = "";
        }

        /// <summary>
        /// Receptionne et affiche les commandes d'un livre
        /// </summary>
        private void AfficheCommandesLivre()
        {
            string idLivre = txbCommandeLivreNumero.Text;
            lesCommandes = controller.GetCommandesLivre(idLivre);
            RemplirCommandesListe(lesCommandes);
        }

        private void RemplirCommandesListe(List<CommandeDocument> commandes)
        {
            if (commandes != null)
            {
                bdgCommandes.DataSource = commandes;
                dgvCommandeLivre.DataSource = bdgCommandes;
                dgvCommandeLivre.Columns["IdLivreDvd"].Visible = false;
                dgvCommandeLivre.Columns["IdSuivi"].Visible = false;
                dgvCommandeLivre.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvCommandeLivre.Columns["Id"].DisplayIndex = 0;
                dgvCommandeLivre.Columns["DateCommande"].DisplayIndex = 1;
                dgvCommandeLivre.Columns["Montant"].DisplayIndex = 2;
                dgvCommandeLivre.Columns["NbExemplaire"].DisplayIndex = 3;
                dgvCommandeLivre.Columns["LibelleSuivi"].DisplayIndex = 4;
            }
            else
            {
                bdgCommandes.DataSource = null;
            }
        }

        /// <summary>
        /// Permet ou interdit l'accès à la saisie d'une nouvelle commande
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces">true ou false</param>
        private void AccesCommandeGroupBox(bool acces)
        {
            grbNouvelleCommandeLivre.Enabled = acces;
            txbNumNewCommandeLivre.Text = "";
            txbNewMontantCommandeLivre.Text = "";
            txbNewNbExCommandeLivre.Text = "";
            dtpNewCommandeLivre.Value = DateTime.Now;
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheCommandeLivreInfos(Livre livre)
        {
            txbCommandeLivreAuteur.Text = livre.Auteur;
            txbCommandeLivreCollection.Text = livre.Collection;
            txbCommandeLivreCheminImg.Text = livre.Image;
            txbCommandeLivreIsbn.Text = livre.Isbn;
            txbCommandeLivreNumero.Text = livre.Id;
            txbCommandeLivreGenre.Text = livre.Genre;
            txbCommandeLivrePublic.Text = livre.Public;
            txbCommandeLivreRayon.Text = livre.Rayon;
            txbCommandeLivreTitre.Text = livre.Titre;
            AfficheCommandesLivre();
            AccesCommandeGroupBox(true);
        }

        private void btnCommandeLivreRechercher_Click(object sender, EventArgs e)
        {
            if (!txbCommandeLivreNumero.Text.Equals(""))
            {
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbCommandeLivreNumero.Text));
                if (livre != null)
                {
                    AfficheCommandeLivreInfos(livre);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        private void txbCommandeLivreNumero_TextChanged(object sender, EventArgs e)
        {
            txbCommandeLivreAuteur.Text = "";
            txbCommandeLivreCollection.Text = "";
            txbCommandeLivreCheminImg.Text = "";
            txbCommandeLivreIsbn.Text = "";
            txbCommandeLivreGenre.Text = "";
            txbCommandeLivrePublic.Text = "";
            txbCommandeLivreRayon.Text = "";
            txbCommandeLivreTitre.Text = "";
            RemplirCommandesListe(null);
            AccesCommandeGroupBox(false);
        }

        /// <summary>
        /// tri sur une colonne de la liste des commandes d'un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCommandeLivre_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvCommandeLivre.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocument> sortedList = new List<CommandeDocument>();

            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesCommandes.OrderBy(o => o.Id).ToList();
                    break;
                case "DateCommande":
                    sortedList = lesCommandes.OrderBy(o => o.DateCommande).Reverse().ToList();
                    break;
                case "Montant":
                    sortedList = lesCommandes.OrderBy(o => o.Montant).Reverse().ToList();
                    break;
                case "NbExemplaire":
                    sortedList = lesCommandes.OrderBy(o => o.NbExemplaire).Reverse().ToList();
                    break;
                case "LibelleSuivi":
                    sortedList = lesCommandes.OrderBy(o => o.LibelleSuivi).ToList();
                    break;
            }

            RemplirCommandesListe(sortedList);
        }

        private void btnValiderCommandeLivre_Click(object sender, EventArgs e)
        {
            // Vérification que tous les champs sont remplis
            if (txbNumNewCommandeLivre.Text == "" ||
                txbNewMontantCommandeLivre.Text == "" ||
                txbNewNbExCommandeLivre.Text == "")
            {
                MessageBox.Show("Tous les champs sont obligatoires", "Information");
                return;
            }
            
            if (txbCommandeLivreNumero.Text == "")
            {
                MessageBox.Show("Un livre doit être sélectionné !", "Informations");
                return;
            }

            try
            {
                string numCommande = txbNumNewCommandeLivre.Text;
                DateTime dateCommande = dtpNewCommandeLivre.Value;
                decimal montant = decimal.Parse(txbNewMontantCommandeLivre.Text);
                int nbExemplaire = int.Parse(txbNewNbExCommandeLivre.Text);
                string idLivreDvd = txbCommandeLivreNumero.Text;
                string idSuivi = SUIVIENCOURS;

                CommandeDocument commande = new CommandeDocument(numCommande, dateCommande, montant, nbExemplaire, idLivreDvd, idSuivi);

                if (controller.CreerCommandeDocument(commande))
                {
                    AfficheCommandesLivre();
                    MessageBox.Show("Commande enregistrée avec succès", "Information");
                }
                else
                {
                    MessageBox.Show("Numéro de commande déjà existant", "Erreur");
                }
            }
            catch
            {
                MessageBox.Show("Vérifiez les données saisies (montant et nombre d'exemplaires doivent être numériques)", "Erreur");
            }
        }

        private void btnSupprimerCommande_Click(object sender, EventArgs e)
        {
            if (dgvCommandeLivre.SelectedRows.Count > 0)
            {
                CommandeDocument commande = (CommandeDocument)bdgCommandes.List[bdgCommandes.Position];

                if (commande.LibelleSuivi.ToLower() == "livrée" || commande.LibelleSuivi.ToLower() == "livree")
                {
                    MessageBox.Show("Impossible de supprimer une commande déjà livrée.", "Suppression impossible");
                    return;
                }

                if (MessageBox.Show("Voulez-vous vraiment supprimer la commande n°" + commande.Id + " ?",
                    "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (controller.DelCommandeDocument(commande))
                    {
                        AfficheCommandesLivre();
                        MessageBox.Show("Commande supprimée avec succès.", "Suppression réussie");
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la suppression de la commande.", "Erreur");
                    }
                }
            }
            else
            {
                MessageBox.Show("Une ligne doit être sélectionnée.", "Information");
            }
        }

        /// <summary>
        /// Remplit la combobox des suivis avec uniquement les états autorisés
        /// selon l'état actuel de la commande sélectionnée
        /// </summary>
        /// <param name="commandeSelectionnee">La commande sélectionnée</param>
        private void RemplirCbxEtapeSuivi(CommandeDocument commandeSelectionnee)
        {
            cbxEtapeSuivi.Items.Clear();

            if (commandeSelectionnee == null)
            {
                return;
            }

            string suiviActuel = commandeSelectionnee.IdSuivi;

            // Règles de transition selon l'état actuel
            switch (suiviActuel)
            {
                case "00001":
                    cbxEtapeSuivi.Items.Add("en cours");
                    cbxEtapeSuivi.Items.Add("relancée");
                    cbxEtapeSuivi.Items.Add("livrée");
                    break;

                case "00002":
                    cbxEtapeSuivi.Items.Add("relancée");
                    cbxEtapeSuivi.Items.Add("livrée");
                    break;

                case "00003":
                    cbxEtapeSuivi.Items.Add("livrée");
                    cbxEtapeSuivi.Items.Add("réglée");
                    break;

                case "00004":
                    cbxEtapeSuivi.Items.Add("réglée");
                    break;
            }

            // Sélectionner le premier élément s'il y en a
            if (cbxEtapeSuivi.Items.Count > 0)
            {
                cbxEtapeSuivi.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// charge les étapes correspondant à la commande dans la combobox
        /// à chaque nouvelle sélection dans le datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCommandeLivre_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCommandeLivre.SelectedRows.Count > 0)
            {
                CommandeDocument commande = (CommandeDocument)bdgCommandes.List[bdgCommandes.Position];
                RemplirCbxEtapeSuivi(commande);
            }
        }

        /// <summary>
        /// Validation du changement d'étape de suivi d'une commande
        /// </summary>
        private void btnValiderModifEtape_Click(object sender, EventArgs e)
        {
            if (dgvCommandeLivre.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner une commande.", "Information");
                return;
            }

            if (cbxEtapeSuivi.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner une nouvelle étape.", "Information");
                return;
            }

            CommandeDocument commande = (CommandeDocument)bdgCommandes.List[bdgCommandes.Position];
            string nouveauLibelleSuivi = cbxEtapeSuivi.SelectedItem.ToString();
            string nouvelIdSuivi = ConvertirLibelleSuiviEnId(nouveauLibelleSuivi);

            if (nouvelIdSuivi == null)
            {
                MessageBox.Show("Erreur lors de la récupération de l'état de suivi.", "Erreur");
                return;
            }

            if (MessageBox.Show($"Voulez-vous vraiment passer la commande n°{commande.Id} à l'étape '{nouveauLibelleSuivi}' ?",
                "Confirmation de modification", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                commande.IdSuivi = nouvelIdSuivi;
                if (controller.UpdateCommandeDocument(commande))
                {
                    AfficheCommandesLivre();
                    MessageBox.Show("Étape de suivi modifiée avec succès.", "Succès");
                }
                else
                {
                    MessageBox.Show("Erreur lors de la modification de l'étape de suivi.", "Erreur");
                }
            }
        }

        /// <summary>
        /// Convertit un libellé de suivi en ID correspondant
        /// </summary>
        /// <param name="libelle">Libellé du suivi</param>
        /// <returns>ID du suivi ou null si non trouvé</returns>
        private string ConvertirLibelleSuiviEnId(string libelle)
        {
            switch (libelle.ToLower())
            {
                case "en cours":
                    return "00001";
                case "relancée":
                    return "00002";
                case "livrée":
                    return "00003";
                case "réglée":
                    return "00004";
                default:
                    return null;
            }
        }

        #endregion


    }
}
