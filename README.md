<h1>MediaTekDocuments – Fonctionnalités ajoutées</h1>

Ce dépôt est une extension du dépôt d'origine disponible ici :<br>
<a href="https://github.com/CNED-SLAM/MediaTekDocuments">https://github.com/CNED-SLAM/MediaTekDocuments</a><br>
Le readme de ce dépôt présente l'application d'origine, son architecture et les fonctionnalités de base déjà implémentées.<br>
<br>
L'API REST nécessaire au fonctionnement de cette application est disponible ici :<br>
<a href="https://github.com/User3426/rest_mediatekdocuments">https://github.com/User3426/rest_mediatekdocuments</a><br>

<h1>Présentation des fonctionnalités ajoutées</h1>

<h2>Authentification et gestion des droits</h2>
Un formulaire de connexion a été ajouté comme point d'entrée de l'application. L'utilisateur doit saisir son login et son mot de passe, qui sont vérifiés via l'API REST (table <em>utilisateur</em> côté BDD).<br>
<br>
Selon le service auquel appartient l'utilisateur connecté, l'accès aux onglets est restreint :<br>
<ul>
  <li><strong>Service "Administratif"</strong> : accès complet à tous les onglets, y compris les commandes et abonnements.</li>
  <li><strong>Service "Prêts"</strong> : accès limité aux onglets Livres, DVD, Revues et Parutions des revues. Les onglets de commandes et d'abonnements sont masqués.</li>
  <li><strong>Service "Culture"</strong> : accès refusé, l'application se ferme après un message d'information.</li>
</ul>
<img width="262" height="119" alt="image" src="https://github.com/user-attachments/assets/b13a4098-0bef-47b0-be81-8f0c8d5bc3e0" />
<br>
<h2>Gestion des livres</h2>
En plus de la consultation existante, il est désormais possible de :<br>
<ul>
  <li><strong>Ajouter</strong> un nouveau livre en renseignant son numéro, titre, auteur, ISBN, collection, image, genre, public et rayon.</li>
  <li><strong>Modifier</strong> un livre sélectionné dans la liste : les champs deviennent éditables et les catégories (genre, public, rayon) sont modifiables via des listes déroulantes.</li>
  <li><strong>Supprimer</strong> un livre, sous réserve qu'aucun exemplaire ni aucune commande ne lui soit rattaché (contrôle effectué côté BDD).</li>
</ul>
<img width="877" height="712" alt="image" src="https://github.com/user-attachments/assets/68e050c0-9558-45dc-9ff8-f07497775b55" />


<h2>Gestion des DVD</h2>
Fonctionnement identique à la gestion des livres :<br>
<ul>
  <li>Ajout d'un nouveau DVD (titre, réalisateur, durée, synopsis, image, genre, public, rayon).</li>
  <li>Modification d'un DVD existant.</li>
  <li>Suppression d'un DVD, sous réserve d'absence d'exemplaires ou de commandes liés.</li>
</ul>
<img width="875" height="734" alt="image" src="https://github.com/user-attachments/assets/ca6e1b1f-fb3d-4614-836a-bbd228214c81" />

<h2>Gestion des revues</h2>
Fonctionnement identique, adapté aux spécificités des revues :<br>
<ul>
  <li>Ajout d'une nouvelle revue (titre, périodicité, délai de mise à disposition, image, genre, public, rayon).</li>
  <li>Modification d'une revue existante.</li>
  <li>Suppression d'une revue, sous réserve d'absence d'exemplaires liés.</li>
</ul>
<img width="874" height="720" alt="image" src="https://github.com/user-attachments/assets/19cc1e70-c16a-4bf7-bf8c-b463ce05481d" />

<h2>Onglet Commandes livres</h2>
Un nouvel onglet permet de gérer les commandes de livres :<br>
<ul>
  <li>Recherche d'un livre par son numéro pour afficher ses informations et la liste de ses commandes associées.</li>
  <li>Ajout d'une nouvelle commande (numéro, date, montant, nombre d'exemplaires).</li>
  <li>Modification de l'étape de suivi d'une commande sélectionnée, selon le workflow autorisé : <em>en cours → relancée → livrée → réglée</em>.</li>
  <li>Suppression d'une commande, impossible si elle est déjà à l'état "livrée".</li>
</ul>
<img width="877" height="786" alt="image" src="https://github.com/user-attachments/assets/7695a3ac-f34c-4d81-8024-332699972aa1" />


<h2>Onglet Commandes DVD</h2>
Fonctionnement identique à l'onglet Commandes livres, appliqué aux DVD.
<img width="881" height="788" alt="image" src="https://github.com/user-attachments/assets/941d26ff-f79c-464b-b35a-93b0b9414e54" />


<h2>Onglet Commandes revues (abonnements)</h2>
Un onglet dédié permet de gérer les abonnements aux revues :<br>
<ul>
  <li>Recherche d'une revue par son numéro pour afficher ses informations et ses abonnements existants.</li>
  <li>Création d'un nouvel abonnement (numéro, date de commande, montant, date de fin d'abonnement).</li>
  <li>Suppression d'un abonnement, impossible si des parutions (exemplaires) y sont rattachées (vérification par comparaison des dates).</li>
</ul>
<img width="877" height="683" alt="image" src="https://github.com/user-attachments/assets/4cce6561-d570-4704-8d35-81dec2b3cac6" />


<h2>Alerte abonnements proches de l'expiration</h2>
Au chargement de l'application, si l'utilisateur appartient au service "Administratif", une fenêtre d'alerte s'affiche automatiquement listant les abonnements dont la date de fin est dans moins de 30 jours.<br>
<img width="465" height="203" alt="image" src="https://github.com/user-attachments/assets/b88dcbc2-8426-43bb-96ef-c620b27315f6" />


<h1>Installation et configuration en local</h1>

<ul>
  <li>Installer <strong>Visual Studio 2019</strong> (ou version ultérieure) avec le support .NET Framework 4.7.2.</li>
  <li>Installer les extensions NuGet nécessaires : <strong>Newtonsoft.Json</strong> (déjà présent dans le projet) et <strong>Serilog</strong>.</li>
  <li>Télécharger et dézipper le code du projet, puis renommer le dossier en <code>mediatekdocuments</code>.</li>
  <li>Récupérer, installer et configurer l'API REST (voir le dépôt API correspondant et son readme).</li>
  <li>Vérifier la configuration dans le fichier <code>App.config</code> du projet :
    <ul>
      <li><code>uriApi</code> : adresse de l'API (par défaut <code>http://localhost/rest_mediatekdocuments/</code>)</li>
      <li><code>authenticationString</code> : identifiants de l'API au format <code>login:password</code> (par défaut <code>admin:adminpwd</code>)</li>
    </ul>
  </li>
  <li>Ouvrir la solution <code>MediaTekDocuments.sln</code> dans Visual Studio, compiler et lancer.</li>
  <li>Se connecter avec un login/mot de passe existant en base (voir la table <em>utilisateur</em> de la BDD).</li>
</ul>
