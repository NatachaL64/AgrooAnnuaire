using AgrooAnnauireModel.Dto;
using AgrooAnnuaireWPF.Services;
using AgrooAnnuaireWPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AgrooAnnuaireWPF.ViewModels
{
    internal class ServicesViewModel
    {

        public string Titre { get; set; } = "Les services";

        private ObservableCollection<ServicesDto> _listeServices = new();
        public ObservableCollection<ServicesDto> ListeServices
        {
            get => _listeServices;
            set
            {
                _listeServices = value;
                OnPropertyChanged();
            }
        }

        public ServicesDto ServiceSelected { get; set; }

        public Visibility FicheVisibility { get; set; } = Visibility.Collapsed;

        public void ActualiserService()
        {
            Task.Run(async () =>
            {
                return await HttpAgrooAnnuaireServiceService.GetServices();
            }).ContinueWith(t =>
            {

                _listeServices.Clear();
                foreach (var service in t.Result)
                {
                    _listeServices.Add(service);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void CreerService()
        {
            var ficheService = new FicheService(new ServicesDto());
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ContentArea.Content = ficheService;
            }
        }

        public void ModifierService()
        {
            if (ServiceSelected != null)
            {
                var ficheService = new FicheService(ServiceSelected);
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.ContentArea.Content = ficheService;
                }
            }
            else
            {
                MessageBox.Show("Aucun service n'a été sélectionné");
            }
        }


        public async void SupprimerService()
        {
            if (ServiceSelected != null)
            {
                MessageBoxResult result = MessageBox.Show($"Êtes-vous sûr de vouloir supprimer ce service {ServiceSelected.Nom} ?",
                                                          "Confirmation de suppression",
                                                          MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    int nbSalaries = await HttpAgrooAnnuaireServiceService.GetNombreUtilisateursByServiceId(ServiceSelected.Id);
                   

                    if (nbSalaries != 0)
                    {
                        MessageBox.Show($"Des salariés travaillent toujours sur ce site {ServiceSelected.Nom}, il ne peut donc pas etre supprimé");
                        await HttpAgrooAnnuaireServiceService.GetServices();

                    }

                    if (nbSalaries == 0)
                    {
                        bool succes = await HttpAgrooAnnuaireServiceService.DeleteService(ServiceSelected.Id);
                        if (succes)
                        {
                            _listeServices.Remove(ServiceSelected);

                            OnPropertyChanged(nameof(_listeServices));

                            MessageBox.Show($"le service {ServiceSelected.Nom} supprimé");
                        }
                    }

                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
