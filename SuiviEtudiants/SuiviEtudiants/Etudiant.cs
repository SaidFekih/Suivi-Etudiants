using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiviEtudiants
{
    public class Etudiant
    {
        public string idEtudiant { get; set; }
        public string prenom { get; set; }
        public string nom { get; set; }
        public string nomComplet
        {
            get
            {
                return nom.ToUpper() + ", " + prenom;
            }
        }
    }
}
