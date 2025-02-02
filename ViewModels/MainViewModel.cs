using AgrooAnnauireModel.Entities;
using AgrooAnnauireModel.Dto;
using AgrooAnnuaireWPF.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Sites = AgrooAnnuaireWPF.Views.Sites;
using Service = AgrooAnnuaireWPF.Views.Services;
using Salarie = AgrooAnnuaireWPF.Views.Salaries;


namespace AgrooAnnuaireWPF.ViewModels
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            MenuNavigationVisibility = Visibility.Collapsed;
        }

        public Visibility MenuNavigationVisibility { get; set; }



        public UserControl OpenAccueil()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                if (mainWindow.MenuNavigation.Visibility == Visibility.Visible)
                {
                    var control = new Accueil();
                    control.DataContext = new AccueilViewModel();
                    return control;
                }
            }

            return null;
        }

        public UserControl OpenSite()
        {
            var control = new Sites();
            var ficheSiteView = new SitesViewModel();
            control.DataContext = ficheSiteView;
            ficheSiteView.ActualiserSite();
            return control;
        }

        public UserControl OpenService()
        {
            var control = new Service();
            var ficheSiteView = new ServicesViewModel();
            control.DataContext = ficheSiteView;
            ficheSiteView.ActualiserService();
            return control;
        }

        public UserControl OpenSalarie()
        {
            var control = new Salarie();
            var ficheSalarieView = new SalariesViewModel();
            control.DataContext = ficheSalarieView;
            ficheSalarieView.ActualiserSalarie();
            return control;
        }
    }
}

