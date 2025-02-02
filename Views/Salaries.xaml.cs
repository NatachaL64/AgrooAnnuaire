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
    public partial class Salaries : UserControl
    {
        public Salaries()
        {
            InitializeComponent();
            var viewModel = new SalariesViewModel();
            this.DataContext = viewModel;

            // Appel à RechargerALL() après avoir bien initialisé le ViewModel
            viewModel.RechargerALL();
        }
        private void CreerButton_Click(object sender, RoutedEventArgs e)
        {
            var ContextModel = DataContext as SalariesViewModel;
            ContextModel.CreerSalarie();
        }
        private void DetailButton_Click(object sender, RoutedEventArgs e)
            {
                var ContextModel = DataContext as SalariesViewModel;
                ContextModel.DetailSalarie();
            }

            private void ModifierButton_Click(object sender, RoutedEventArgs e)
            {
                var ContextModel = DataContext as SalariesViewModel;
                ContextModel.ModifierSalarie();
            }

            private void SupprimerButton_Click(object sender, RoutedEventArgs e)
            {
                var ContextModel = DataContext as SalariesViewModel;
                ContextModel.SupprimerSalarie();
            }

            private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                var salarie = (sender as DataGrid)?.SelectedItem as UtilisateursDto;
                var ContextModel = DataContext as SalariesViewModel;
                ContextModel.SalarieSelected = salarie;
            DetailButton.IsEnabled = salarie != null;
            ModifierButton.IsEnabled = salarie != null;
                SupprimerButton.IsEnabled = salarie != null;
            }

        }
    }

