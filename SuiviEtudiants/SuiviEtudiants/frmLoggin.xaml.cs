using System;
using System.ComponentModel;
using System.Windows;
using System.Data.SqlClient;
using System.Configuration;

namespace SuiviEtudiants
{
    /// <summary>
    /// Logique d'interaction pour frmLoggin.xaml
    /// </summary>
    public partial class frmLoggin : Window
    {
        SqlConnection connexion;
        SqlCommand commande;
        bool ouverture = false;

        public frmLoggin()
        {
            InitializeComponent();
            connexion = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
            txtUtilisateur.Focus();
            ouverture = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                // Création de la requête sélection.
                string authentification = "SELECT * FROM tblUtilisateurs WHERE NomUtilisateur = '" + txtUtilisateur.Text + "' AND MotPasse = '" + txtMotPasse.Password + "'";
                // Création de notre objet SqlCommand.
                commande = new SqlCommand(authentification, connexion);
                // Ouverture de notre connexion.
                connexion.Open();
                // Lecture de l'enregistrement.
                SqlDataReader lecteur = commande.ExecuteReader();
                // Si notre lecteur contient un enregistrement.
                if (lecteur.Read())
                {
                    // Création de l'utilisateur actif. 
                    UtilisateurActif utilisateur = new UtilisateurActif();
                    // Récupération des informations de notre utilisateur. 
                    utilisateur.IdUtilisateur = lecteur["IdUtilisateur"].ToString();
                    utilisateur.Prenom = lecteur["Prenom"].ToString();
                    utilisateur.Nom = lecteur["Nom"].ToString();
                    // Affichage d'un message de bienvenue.
                    MessageBox.Show("Bienvenue " + utilisateur.Prenom + " " + utilisateur.Nom);
                    // On crée notre nouvelle fenêtre. 
                    SuiviEtudiantsUI gestionEtudiant = new SuiviEtudiantsUI(utilisateur);
                    //On affiche notre fenêtre de travail principale.
                    gestionEtudiant.Show();
                    // On modifie la valeur de notre variable ouverture. 
                    ouverture = false;
                    // On cache la fenêtre d'accès. 
                    this.Hide();
                }
                // Si le lecteur est vide. 
                else
                {
                    // Affichage d'un message d'erreur. 
                    MessageBox.Show("Les informations saisies ne me permet pas de vous authentifier.");
                    // Préparation de la fenêtre pour la saisie. 
                    txtUtilisateur.Text = string.Empty;
                    txtMotPasse.Password = string.Empty;
                    txtUtilisateur.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // Fermeture de notre connexion.
                connexion.Close();
            }
        }

        // Événement associé au clic sur le bouton Annuler de l'interface.
        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void txtUtilisateur_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (txtMotPasse.Password != null)
            {
                btnOk.IsEnabled = true;
            }
        }

        private void txtMotPasse_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtUtilisateur.Text != null)
            {
                btnOk.IsEnabled = true;
            }
        }

        private void frmLoggin_Closing(object sender, CancelEventArgs e)
        {
            if (ouverture)
            {
                // On s'assure que c'est bien l'intention de l'utilisateur de quitter l'application
                if (MessageBox.Show("Désirez-vous réellement quitter cette application ?", "Attention !", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                // Si tel est le cas, on met fin à l'application.
                {
                    Application.Current.Shutdown();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
