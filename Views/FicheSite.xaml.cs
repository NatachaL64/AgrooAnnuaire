using System.Windows;
using System.Windows.Controls;
using AgrooAnnuaireWPF.ViewModels;
using AgrooAnnauireModel.Dto;
using AgrooAnnauireModel.Entities;
using AgrooAnnuaireWPF.Services;

namespace AgrooAnnuaireWPF.Views;

public partial class FicheSite : UserControl
{
    public SitesDto SiteSelected { get; set; }

    public FicheSite(SitesDto siteSelected)
    {
        InitializeComponent();
        SiteSelected = siteSelected;
        DataContext = this;
    }

    private void Annuler_Click(object sender, RoutedEventArgs e)
    {
        // Fermer la page fiche du site et réafficher la liste des sites
        var mainWindow = Application.Current.MainWindow as MainWindow;
        if (mainWindow != null)
        {
            var control = new Views.Sites();
            var viewModel = new ViewModels.SitesViewModel();
            control.DataContext = viewModel;
            viewModel.ActualiserSite();
            mainWindow.ContentArea.Content = control;
        }
    }

    private async void Enregistrer_Click(object sender, RoutedEventArgs e)
    {
        if (SiteSelected.Id == 0)
        {
            await HttpAgrooAnnuaireServiceSite.CreateSite(SiteSelected);
        }
        else
        {
           await HttpAgrooAnnuaireServiceSite.UpdateSite(SiteSelected.Id, SiteSelected);
        }
        Annuler_Click(sender, e);
    }
}