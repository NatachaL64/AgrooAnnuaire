using AgrooAnnuaireWPF;
using AgrooAnnauireModel.Dto;
using AgrooAnnauireModel.Entities;
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
using AgrooAnnuaireWPF.Services;
using System.Runtime.InteropServices;

namespace AgrooAnnuaireWPF.ViewModels
{
    internal class SitesViewModel
    {

        public string Titre { get; set; } = "Les sites";

        private ObservableCollection<SitesDto> _listeSites = new();
        public ObservableCollection<SitesDto> ListeSite
        {
            get => _listeSites;
            set
            {
                _listeSites = value;
                OnPropertyChanged();
            }
        }

        public SitesDto SiteSelected { get; set; }

        public Visibility FicheVisibility { get; set; } = Visibility.Collapsed;

        public void ActualiserSite()
        {
            Task.Run(async () =>
            {
                return await HttpAgrooAnnuaireServiceSite.GetSites();
            }).ContinueWith(t =>
            {

                _listeSites.Clear();
                foreach (var site in t.Result)
                {
                    _listeSites.Add(site);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void CreerSite()
        {
            var ficheSite = new FicheSite(new SitesDto());
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ContentArea.Content = ficheSite;
            }
        }

        public void ModifierSite()
        {
            if (SiteSelected != null)
            {
                var ficheSite = new FicheSite(SiteSelected);
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.ContentArea.Content = ficheSite;
                }
            }
            else
            {
                MessageBox.Show("Aucun site a été sélectionné");
            }
        }


        public async void SupprimerSite()
        {
            if (SiteSelected != null)
            {
                MessageBoxResult result = MessageBox.Show($"Êtes-vous sûr de vouloir supprimer ce site{SiteSelected.NomVille} ?",
                                                          "Confirmation de suppression",
                                                          MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    int nbSalaries = await HttpAgrooAnnuaireServiceSite.GetNombreUtilisateursBySiteId(SiteSelected.Id);
                   

                    if (nbSalaries != 0)
                    {
                        MessageBox.Show($"Des salariés travaillent toujours sur ce site {SiteSelected.NomVille}, il ne peut donc pas etre supprimé");
                        await HttpAgrooAnnuaireServiceSite.GetSites() ;

                    }

                    if (nbSalaries == 0)
                    {
                        bool succes = await HttpAgrooAnnuaireServiceSite.DeleteSite(SiteSelected.Id);
                        if (succes)
                        {
                            _listeSites.Remove(SiteSelected);

                            OnPropertyChanged(nameof(_listeSites));

                            MessageBox.Show($"le site {SiteSelected.NomVille} supprimé");
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
