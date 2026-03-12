
namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Document (réunit les infomations communes à tous les documents : Livre, Revue, Dvd)
    /// </summary>
    public class Document
    {
        public string Id { get; }
        public string Titre { get; }
        public string Image { get; }
        public string IdGenre { get; }
        public string Genre { get; }
        public string IdPublic { get; }
        public string Public { get; }
        public string IdRayon { get; }
        public string Rayon { get; }

        /// <summary>
        /// Constructeur : initialise les informations communes à tous les documents
        /// </summary>
        /// <param name="id">identifiant unique du document</param>
        /// <param name="titre">titre du document</param>
        /// <param name="image">chemin vers l'image du document</param>
        /// <param name="idGenre">identifiant du genre</param>
        /// <param name="genre">libellé du genre</param>
        /// <param name="idPublic">identifiant du public cible</param>
        /// <param name="lePublic">libellé du public cible</param>
        /// <param name="idRayon">identifiant du rayon</param>
        /// <param name="rayon">llibellé du rayon</param>
        public Document(string id, string titre, string image, string idGenre, string genre, string idPublic, string lePublic, string idRayon, string rayon)
        {
            Id = id;
            Titre = titre;
            Image = image;
            IdGenre = idGenre;
            Genre = genre;
            IdPublic = idPublic;
            Public = lePublic;
            IdRayon = idRayon;
            Rayon = rayon;
        }
    }
}
