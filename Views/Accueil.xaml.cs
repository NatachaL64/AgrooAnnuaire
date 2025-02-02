using AgrooAnnuaireWPF.ViewModels;

using System.Windows.Controls;



namespace AgrooAnnuaireWPF.Views
{
    /// <summary>
    /// Logique d'interaction pour Accueil.xaml
    /// </summary>
    public partial class Accueil : UserControl
    {
        public Accueil()
        {
            InitializeComponent();
            this.DataContext = new AccueilViewModel();
        }
    }
}
