using AgrooAnnuaireWPF.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AgrooAnnuaireWPF
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;

        }


        private void Open_Accueil(object sender, RoutedEventArgs e)
        {
            var accueilControl = _viewModel.OpenAccueil();
            if (accueilControl != null)
            {
                ContentArea.Content = accueilControl;
            }
        }

        private void Open_Sites(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = _viewModel.OpenSite();
        }

        private void Open_Services(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = _viewModel.OpenService();
        }

        private void Open_Salaries(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = _viewModel.OpenSalarie();
        }


    }
}