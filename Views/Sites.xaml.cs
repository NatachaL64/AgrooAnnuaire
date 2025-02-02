using System.Windows;
using System.Windows.Controls;
using AgrooAnnauireModel.Dto;

using AgrooAnnuaireWPF.ViewModels;

namespace AgrooAnnuaireWPF.Views
{
    public partial class Sites : UserControl
    {
        public Sites()
        {
            InitializeComponent();
            this.DataContext = new SitesViewModel();
        }

        private void CreerButton_Click(object sender, RoutedEventArgs e)
        {
            var ContextModel = DataContext as SitesViewModel;
            ContextModel.CreerSite();
        }

        private void ModifierButton_Click(object sender, RoutedEventArgs e)
        {
            var ContextModel = DataContext as SitesViewModel;
            ContextModel.ModifierSite();
        }

        private void SupprimerButton_Click(object sender, RoutedEventArgs e)
        {
            var ContextModel = DataContext as SitesViewModel;
            ContextModel.SupprimerSite();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var site = (sender as DataGrid)?.SelectedItem as SitesDto;
            var ContextModel = DataContext as SitesViewModel;
            ContextModel.SiteSelected = site;
            ModifierButton.IsEnabled = site != null;
            SupprimerButton.IsEnabled = site != null;
        }
    }
}
