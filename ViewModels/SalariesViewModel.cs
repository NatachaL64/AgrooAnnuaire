using AgrooAnnauireModel.Context;
using AgrooAnnauireModel.Dto;
using AgrooAnnuaireWPF.Services;
using AgrooAnnuaireWPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AgrooAnnuaireWPF.ViewModels
{
    class SalariesViewModel
    {
        public string Titre { get; set; } = "Les salariés";

        private ObservableCollection<UtilisateursDto> _listeSalaries = new();
        public ObservableCollection<UtilisateursDto> ListeSalaries
        {
            get => _listeSalaries;
            set
            {
                _listeSalaries = value;
                OnPropertyChanged();
            }
        }

        public UtilisateursDto SalarieSelected { get; set; }

        public Visibility FicheVisibility { get; set; } = Visibility.Collapsed;

        public void ActualiserSalarie()
        {
            Task.Run(async () =>
            {
                return await Services.HtppAgrooAnnuaireServiceSalarie.GetSalaries();
            }).ContinueWith(t =>
            {

                _listeSalaries.Clear();
                foreach (var salarie in t.Result)
                {
                    _listeSalaries.Add(salarie);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }


       

        private ObservableCollection<SitesDto> _listeSites = new();
        public ObservableCollection<SitesDto> ListeSites
        {
            get => _listeSites;
            set
            {
                if (_listeSites != value)
                {
                    _listeSites = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<ServicesDto> _listeServices = new();
        public ObservableCollection<ServicesDto> ListeServices
        {
            get => _listeServices;
            set
            {
                if (_listeServices != value)
                {
                    _listeServices = value;
                    OnPropertyChanged();
                }
            }
        }

        public async Task RechargerSite()
        {
            var sites = await HttpAgrooAnnuaireServiceSite.GetSites();

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                ListeSites.Clear();
                foreach (var site in sites)
                {
                    ListeSites.Add(site);
                }
            });
        }

        public async Task RechargerService()
        {
            var services = await HttpAgrooAnnuaireServiceService.GetServices();

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                ListeServices.Clear();
                foreach (var service in services)
                {
                    ListeServices.Add(service);
                }
            });
        }

        public async Task RechargerALL()
        {
            RechargerService();
            RechargerSite();

            
        }






        public void CreerSalarie()
        {
            var ficheSalarie = new FicheSalarie(new UtilisateursDto());
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ContentArea.Content = ficheSalarie;
            }
        }

        public void ModifierSalarie()
        {
            if (SalarieSelected != null)
            {
                var ficheSalarie = new FicheSalarie(SalarieSelected);
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.ContentArea.Content = ficheSalarie;
                }
            }
            else
            {
                MessageBox.Show("Aucun salarié n'a été sélectionné");
            }
        }

        public void DetailSalarie()
        {
            if (SalarieSelected != null)
            {
                var ficheSalarieDetail = new FicheSalarieDetail(SalarieSelected);
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.ContentArea.Content = ficheSalarieDetail;
                }
            }
            else
            {
                MessageBox.Show("Aucun salarié n'a été sélectionné");
            }
        }


        public async void SupprimerSalarie()
        {
            if (SalarieSelected != null)
            {
                MessageBoxResult result = MessageBox.Show($"Êtes-vous sûr de vouloir supprimer le salarié {SalarieSelected.Nom} ?",
                                                          "Confirmation de suppression",
                                                          MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {   
                    bool succes = await Services.HtppAgrooAnnuaireServiceSalarie.DeleteSalarie(SalarieSelected.Id);
                    if (succes)
                    {
                        _listeSalaries.Remove(SalarieSelected);

                        OnPropertyChanged(nameof(ListeSalaries));

                        MessageBox.Show($" Le salarié a été supprimé");
                    }
                }
            }
            else
            {
                MessageBox.Show("Aucun salarié n'a été sélectionné");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

