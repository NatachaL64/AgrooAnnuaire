using System.Windows;
using System.Windows.Controls;
using AgrooAnnuaireWPF.ViewModels;
using AgrooAnnauireModel.Dto;
using AgrooAnnauireModel.Entities;
using AgrooAnnuaireWPF.Services;

namespace AgrooAnnuaireWPF.Views;

public partial class FicheSalarie : UserControl
{
    public UtilisateursDto SalarieSelected { get; set; }

    public FicheSalarie(UtilisateursDto salarieSelected)
    {
        InitializeComponent();
        SalarieSelected = salarieSelected;
        DataContext = this;
    }
    private void Annuler_Click(object sender, RoutedEventArgs e)
        {
            // Fermer la page FicheSalarie et réafficher salaries
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                var control = new Views.Salaries();
                var salariesViewModel = new ViewModels.SalariesViewModel();
                control.DataContext = salariesViewModel;
                salariesViewModel.ActualiserSalarie();
                mainWindow.ContentArea.Content = control;
            }
        }

        private async void Enregistrer_Click(object sender, RoutedEventArgs e)
        {
            if (SalarieSelected.Id == 0)
            {
                await HtppAgrooAnnuaireServiceSalarie.CreateSalarie(SalarieSelected);
            }
            else
            {
                await HtppAgrooAnnuaireServiceSalarie.UpdateSalarie(SalarieSelected.Id, SalarieSelected);
            }
            Annuler_Click(sender, e);
        }
}

