using System.Windows;
using System.Windows.Controls;
using AgrooAnnuaireWPF.ViewModels;
using AgrooAnnauireModel.Dto;
using AgrooAnnauireModel.Entities;
using AgrooAnnuaireWPF.Services;

namespace AgrooAnnuaireWPF.Views;

public partial class FicheService : UserControl
{
    public ServicesDto ServiceSelected { get; set; }

    public FicheService(ServicesDto serviceSelected)
    {
        InitializeComponent();
        ServiceSelected = serviceSelected;
        DataContext = this;
    }

    private void Annuler_Click(object sender, RoutedEventArgs e)
    {
        // Fermer la page fiche du service et réafficher la liste des service
        var mainWindow = Application.Current.MainWindow as MainWindow;
        if (mainWindow != null)
        {
            var control = new Views.Services();
            var viewModel = new ViewModels.ServicesViewModel();
            control.DataContext = viewModel;
            viewModel.ActualiserService();
            mainWindow.ContentArea.Content = control;
        }
    }

    private async void Enregistrer_Click(object sender, RoutedEventArgs e)
    {
        if (ServiceSelected.Id == 0)
        {
            await HttpAgrooAnnuaireServiceService.CreateService(ServiceSelected);
        }
        else
        {
           await HttpAgrooAnnuaireServiceService.UpdateService(ServiceSelected.Id, ServiceSelected);
        }
        Annuler_Click(sender, e);
    }
}