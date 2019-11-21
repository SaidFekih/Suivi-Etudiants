using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Configuration;

namespace SuiviEtudiants
{
    /// <summary>
    /// Logique d'interaction pour frmNouvelEtudiant.xaml
    /// </summary>
    public partial class frmNouvelEtudiant : Window
    {
        SqlConnection connexion;
        SqlCommand commande;

        string idInstructeur, idProgramme;

        private void CmbInstructeur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cmbInstructeur.SelectedIndex)
            {
                case 0: idInstructeur = "yd001";
                    break;
                case 1: idInstructeur = "ml001";
                    break;
            }
        }

        private void CmbProgramme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cmbProgramme.SelectedIndex)
            {
                case 0: idProgramme = "LEA.9C";
                    break;
                case 1: idProgramme = "LEA.AE";
                    break;
            }
        }
        private bool VerifierSaisie()
        {
            bool OK = true;
            if 
                (
                txtID.Text.Trim() == string.Empty || 
                txtPrenom.Text.Trim() == string.Empty || 
                txtNom.Text.Trim() == string.Empty || 
                txtAdresse.Text.Trim() == string.Empty || 
                txtVille.Text.Trim() == string.Empty || 
                txtProvince.Text.Trim() == string.Empty || 
                txtCodePostal.Text.Trim() == string.Empty || 
                txtTelephone.Text.Trim() == string.Empty || 
                cmbInstructeur.SelectedIndex == -1 || 
                cmbProgramme.SelectedIndex == -1
                )

            {
                OK = false;
            }
            return OK;
        }
        private void BtnEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            bool OK = VerifierSaisie();
            if (OK)
            {
                // Création d'une requête sélection 
                string verifie = $"SELECT IdEtudiant FROM tblEtudiants WHERE IdEtudiant = '{ txtID.Text}'";
                // Création de notre objet SqlCommand. 
                commande = new SqlCommand(verifie, connexion);
                try
                {
                    // Ouverture de notre connexion. 
                    connexion.Open();
                    // Création et exécution d'un objet SqlDataReader. 
                    SqlDataReader lecteur = commande.ExecuteReader();
                    // Si le lecteur contient une information, Affichage d'un message d'erreur à l'utilisateur. 
                    if (lecteur.Read())
                    {
                        MessageBox.Show("Ce numéro d'identification est déjà utilisé dans la table.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    // Sinon, on enregistre les informations saisies. 
                    else
                    {
                        // Fermeture du lecteur. 
                        lecteur.Close();
                        // Création de notre requête d'insertion. 
                        string insereEtudiant = $"INSERT INTO tblEtudiants(IdEtudiant, Prenom, Nom, Adresse, Ville, Province, CodePostal, Telephone, CodeProgramme, IdInstructeur) VALUES('{txtID.Text}', '{txtPrenom.Text}', '{txtNom.Text}', '{txtAdresse.Text}', '{txtVille.Text}', '{txtProvince.Text}', '{txtCodePostal.Text}', '{txtTelephone.Text}', '{idProgramme}', '{idInstructeur}')";
                        // Création de notre objet SqlCommand.
                        // Notre connexion est toujours ouverte. 
                        commande = new SqlCommand(insereEtudiant, connexion);
                        try
                        {
                            // Exécution de la requête. 
                            int ligne = commande.ExecuteNonQuery();
                            // Si l'enregistrement a réussi, on avise l'utilisateur.
                            if (ligne != 0)
                            {
                                if (MessageBox.Show("Enregistrement du nouvel étudiant réussi." + Environment.NewLine + "Désirez-vous enregistrer un autre étudiant ?", "Enregistrement réussi", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                                {
                                    // Si l'utilisateur a terminé, on ferme notre fenêtre. 
                                    this.Close();
                                }
                                else
                                {
                                    // Si l'utilisateur désire effectuer un autre enregistrement, on prépare la fenêtre. 
                                    txtID.Text = txtPrenom.Text = string.Empty;
                                    txtNom.Text = txtAdresse.Text = string.Empty;
                                    txtVille.Text = txtProvince.Text = string.Empty;
                                    txtCodePostal.Text = txtTelephone.Text = string.Empty;
                                    cmbInstructeur.SelectedIndex = -1;
                                    cmbProgramme.SelectedIndex = -1; txtID.Focus();
                                }
                            }
                           
                        }
                        catch (Exception er)
                        {
                            // Si la province contient plus de deux caractères,on avise l'utilisateur. 
                            MessageBox.Show("La province de résidence de l'étudiant doit être saisie sur deux caractères, Qc par exemple.", "Attention", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
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
            else
            {
                MessageBox.Show("Vous devez saisir toutes les informations requises", "Attention !", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        public frmNouvelEtudiant()
        {
            InitializeComponent();
            connexion = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
            cmbInstructeur.Items.Add("Yves Desharnais");
            cmbInstructeur.Items.Add("Michel Leduc");
            cmbProgramme.Items.Add("Programmeur-analyste - orienté Internet");
            cmbProgramme.Items.Add("Gestionnaire en réseautique - spécialiste de la sécurité");
        }
    }
}
