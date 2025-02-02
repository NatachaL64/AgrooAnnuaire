using AgrooAnnauireModel.Dto;
using AgrooAnnuaireWPF.ViewModels;
using System;
using System.Collections.Generic;
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

namespace AgrooAnnuaireWPF.Views
{
    public partial class Services : UserControl
    {
        public Services()
        {
            InitializeComponent();
            this.DataContext = new ServicesViewModel();
        }

        private void CreerButton_Click(object sender, RoutedEventArgs e)
        {
            var ContextModel = DataContext as ServicesViewModel;
            ContextModel.CreerService();
        }

        private void ModifierButton_Click(object sender, RoutedEventArgs e)
        {
            var ContextModel = DataContext as ServicesViewModel;
            ContextModel.ModifierService();
        }

        private void SupprimerButton_Click(object sender, RoutedEventArgs e)
        {
            var ContextModel = DataContext as ServicesViewModel;
            ContextModel.SupprimerService();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var service= (sender as DataGrid)?.SelectedItem as ServicesDto;
            var ContextModel = DataContext as ServicesViewModel;
            ContextModel.ServiceSelected = service;
            ModifierButton.IsEnabled = service != null;
            SupprimerButton.IsEnabled = service != null;
        }
    }
}
