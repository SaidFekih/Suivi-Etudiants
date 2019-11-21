using System;
using System.Windows;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

namespace SuiviEtudiants
{
    /// <summary>
    /// Logique d'interaction pour SuiviEtudiantsUI.xaml
    /// </summary>
    public partial class SuiviEtudiantsUI : Window
    {
        bool ouverture = false;
        SqlConnection connexion;
        UtilisateurActif utilisateur;
        SqlCommand commande;
        Etudiant etudiant;
        List<Etudiant> etudiants = new List<Etudiant>();
        public SuiviEtudiantsUI(UtilisateurActif actif)
        {
            InitializeComponent();
            utilisateur = actif;
            // Affichage du nom de l'utilisateur actif dans la barre de titre de l'application.
            Title += utilisateur.Prenom + " " + utilisateur.Nom;
            connexion = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
            EmplirListeEtudiants();
            ouverture = true;




            //  connexion = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
            try
            {
              //  connexion.Open();
                MessageBox.Show("Connexion à la base de données réussie.");
              //  connexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void EmplirListeEtudiants()
        {
            // Création de la requête sélection. 
            string selectEtudiant = "SELECT IdEtudiant, Prenom, Nom FROM tblEtudiants WHERE IdInstructeur = '" + utilisateur.IdUtilisateur + "' ORDER BY Nom";
            try
            {
                // Création de notre objet SqlCommand. 
                commande = new SqlCommand(selectEtudiant, connexion);

                // Ouverture de notre connexion.
                connexion.Open();

                // Création de notre lecteur d'enregistrements. 
                SqlDataReader lecteur = commande.ExecuteReader();

                // Boucle qui effectue la lecture de nos enregistrements. 
                while (lecteur.Read())
                {
                    // Création d'un nouvel étudiant. 
                    etudiant = new Etudiant();
                    // Récupération des informations de l'étudiant. 
                    etudiant.idEtudiant = lecteur["IdEtudiant"].ToString();
                    etudiant.prenom = lecteur["Prenom"].ToString();
                    etudiant.nom = lecteur["Nom"].ToString();
                    // Ajoût de l'étudiant dans la liste. 
                    etudiants.Add(etudiant);
                }
                ListeEtudiants.DataContext = etudiants;
            }
            catch (Exception ex)
            {
                // Affichage de l'erreur en cas de connexion non réussie.
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // Fermeture de notre connexion.
                connexion.Close();
            }
        }


        private void mnuQuitter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mnuAjouterEtudiant_Click(object sender, RoutedEventArgs e)
        {
            // On crée notre nouvelle fenêtre. 
            frmNouvelEtudiant ajoutEtudiant = new frmNouvelEtudiant();
            // On affiche notre fenêtre de travail en mode dialogue. 
            ajoutEtudiant.ShowDialog();
            // On efface le contenu de notre liste d'étudiants. 
            etudiants.Clear();
            // On recrée notre liste d'étudiants. 
            EmplirListeEtudiants();
            ListeEtudiants.Items.Refresh();
        }

        private void ListeEtudiants_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(ListeEtudiants.SelectedIndex == -1)
            {
                return;
            }
            // Récupération de l'étudiant dans notre collection selon l'index sélectionné.
            Etudiant sEtudiant = etudiants[ListeEtudiants.SelectedIndex];
            // Création de notre requête sélection.
            string selectEtudiant = "SELECT * FROM tblEtudiants WHERE IdEtudiant = '" + sEtudiant.idEtudiant + "'";
            // Création de notre objet SqlCommand.
            commande = new SqlCommand(selectEtudiant, connexion);
            // Ouverture de notre connexion. 
            connexion.Open();
            // Création et exécution de notre lecteur d'enregistrement. 
            SqlDataReader lecteur = commande.ExecuteReader();
            // Si l'enregistrement a été trouvé. 
            if (lecteur.Read())
            {
                // Affichage des informations dans les différents éléments de notre formulaire. 
                lblID.Content = lecteur["IdEtudiant"];
                txtPrenom.Text = lecteur["Prenom"].ToString();
                txtNom.Text = lecteur["Nom"].ToString();
                txtAdresse.Text = lecteur["Adresse"].ToString();
                txtVille.Text = lecteur["Ville"].ToString();
                txtProvince.Text = lecteur["Province"].ToString();
                txtCodePostal.Text = lecteur["CodePostal"].ToString();
                txtTelephone.Text = lecteur["Telephone"].ToString();
                // Sélection du bouton radio selon la valeur récupérée du champ Statut. 
                switch (lecteur["Statut"].ToString())
                {
                    case "0": rbActif.IsChecked = true;
                        break;
                    case "1": rbArret.IsChecked = true;
                        break;
                    case "2": rbGradue.IsChecked = true;
                        break;
                }

            }
            connexion.Close();
        }
        private bool VerifierSaisie()
        {
            bool OK = true;
            if
                (
                lblID.Content.ToString().Trim() == string.Empty ||
                txtPrenom.Text.Trim() == string.Empty ||
                txtNom.Text.Trim() == string.Empty ||
                txtAdresse.Text.Trim() == string.Empty ||
                txtVille.Text.Trim() == string.Empty ||
                txtProvince.Text.Trim() == string.Empty ||
                txtCodePostal.Text.Trim() == string.Empty ||
                txtTelephone.Text.Trim() == string.Empty 
                //||cmbInstructeur.SelectedIndex == -1 ||
                //cmbProgramme.SelectedIndex == -1
                )

            {
                OK = false;
            }
            return OK;
        }

        private void Enregistrer_Click(object sender, RoutedEventArgs e)
        {
            int statut = -1;
            // Récupération de la valeur du Statut de l'étudiant.
            statut = (rbActif.IsChecked == true ? 0 : rbArret.IsChecked == true ? 1 :rbGradue.IsChecked == true ? 2 : -1);
            
            // Si la valeur de statut est différente de -1.
            if (statut != -1)
            {
                // Si nos éléments TextBox contiennent des valeurs,nous pouvons enregistrer les modifications. 
                if (VerifierSaisie())
                {
                    try
                    {
                        // Ouverture de notre connexion. 
                        connexion.Open();
                        try
                        {
                            // Création de notre requête de mise-à-jour. 
                            string enregistrerEtudiant = "UPDATE tblEtudiants SET Prenom = '" + txtPrenom.Text + "', Nom = '" + txtNom.Text + "', Adresse = '" + txtAdresse.Text + "', Ville = '" + txtVille.Text + "', Province = '" + txtProvince.Text + "', CodePostal = '" + txtCodePostal.Text + "', Telephone = '" + txtTelephone.Text + "', Statut = '" + statut + "' WHERE IdEtudiant = '" + lblID.Content + "'";
                            // Création de notre objet SqlCommand. 
                            commande = new SqlCommand(enregistrerEtudiant, connexion);
                            // Exécution de la requête. 
                            commande.ExecuteNonQuery(); MessageBox.Show("Mise-à-jour réussie.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        // Si une erreur s'est produite lors de l'enregistrement, on avise l'utilisateur. 
                        catch
                        {
                            MessageBox.Show("La province de résidence de l'étudiant doit être saisie sur deux caractères, Qc par exemple.", "Attention", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }

                    catch (Exception ex)
                    {
                        // Affichage de l'erreur s'il y a erreur. 
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        // Fermeture de la connexion. 
                        connexion.Close();
                    }

                }
                // Si un de nos éléments TextBox ne contient pas de valeurs, affichage d'un message d'erreur à l'utilisateur. 
                else
                {
                    MessageBox.Show("Vous devez saisir toutes les informations requises.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }

            }
            // si la variable de statut est -1, affichage d'un message à l'utilisateur. 
            else
            {
                MessageBox.Show("Vous devez sélectionner le statut de l'étudiant" + Environment.NewLine + "L'enregistrement des données ne sera pas effectué.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void BtnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            if (ListeEtudiants.SelectedIndex != -1)
            {
                if (MessageBox.Show($"Désirez-vous réellement supprimer l'enregistrement de {txtPrenom.Text} {txtNom.Text} ?" + Environment.NewLine + "Cette opération est irréversible.", "Attention", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Ouverture de la connexion à notre base de données. 
                        connexion.Open();
                        // Création de la requête de suppression.
                        string suppression = $"DELETE FROM tblEtudiants WHERE IdEtudiant= '{ lblID.Content.ToString()}'";
                        commande = new SqlCommand(suppression, connexion);
                        // Exécution de notre objet SqlCommand.
                        int ligne = commande.ExecuteNonQuery();
                        if (ligne != 0)
                        {
                            // Affichage d’un message à l’utilisateur. 
                            MessageBox.Show($"Suppression de {txtPrenom.Text} {txtNom.Text} réussie.");
                            // On ferme la connexion à notre base de données. 
                            connexion.Close();
                            // Nous supprimons la source des Items de notre ComboBox. 
                          //  ListeEtudiants.ItemsSource = null;
                            // On efface le contenu de notre liste d'étudiants. 
                            etudiants.Clear();
                            // On recrée notre liste d'étudiants. 
                            EmplirListeEtudiants();

                            // On réaffecte la source de nos Items à notre ComboBox. 
                            ListeEtudiants.ItemsSource = etudiants;

                            ListeEtudiants.Items.Refresh();

                            // Finalement, on efface le contenu de notre fenêtre. 
                            lblID.Content = "";
                            txtPrenom.Text = txtNom.Text = txtAdresse.Text = string.Empty;
                            txtVille.Text = txtProvince.Text = string.Empty;
                            txtCodePostal.Text = txtTelephone.Text = string.Empty;
                        }
                    }
                    // Affichage d’un message d’erreur s’il y a un problème d’exécution. 
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        // Fermeture de la connexion à notre base de données. 
                        connexion.Close();
                    }
                }
            }
        }
    }
}
