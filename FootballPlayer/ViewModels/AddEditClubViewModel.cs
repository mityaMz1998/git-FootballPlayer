using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using FootballPlayer.Models;
using FootballPlayer.Commands;
using FootballPlayer.DataContext;
using FootballPlayer.Views;
using System.Windows;
using System.ComponentModel;

namespace FootballPlayer.ViewModels
{
    /// <summary>
    /// Class for adding and editing a club
    /// </summary>
    public class AddEditClubViewModel : IDataErrorInfo
    {
        public static AddEditClub aec;
        FootballPlayerContext db = new FootballPlayerContext();
        object lineClub;
        bool flgEditClub;

        public string Name { get; set; }
        public string Country { get; set; }
        public ObservableCollection<Club> Clubs { get; set; }
        public AddEditClubViewModel()
        {
            MainWindowViewModel.LoadCountriesInComboBox(aec.cmbClubCountry);
            db.Clubs.Load();
            Clubs = db.Clubs.Local.ToObservableCollection();
        }

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "Name":
                        if (String.IsNullOrEmpty(Name))
                        {
                            error = "The name of the club should not be empty";
                        }
                        break;
                    case "Country":
                        if (String.IsNullOrEmpty(Country))
                        {
                            error = "The country of the club should not be empty";
                        }
                        break;
                }
                return error;
            }
        }
        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        private RelayCommand _addClubCommand;
        /// <summary>
        /// Command for adding a club
        /// </summary>
        public RelayCommand AddClubCommand
        {
            get
            {
                return _addClubCommand ??
                    (_addClubCommand = new RelayCommand(obj =>
                    {
                        Club c = new Club()
                        {
                           Name = aec.txtBoxNameClub.Text,
                           Country = aec.cmbClubCountry.Text
                        };
                        db.Clubs.Add(c);
                        db.SaveChanges();
                        ClearAllElements();
                    },obj1 => AddClubCommandCanExecute()));
            }
        }

        /// <summary>
        /// Method for determining availability for a command 'AddClubCommand'
        /// </summary>
        private bool AddClubCommandCanExecute()
        {
            if (!String.IsNullOrEmpty(aec.txtBoxNameClub.Text) && !String.IsNullOrEmpty(aec.cmbClubCountry.Text) && flgEditClub == false)
                return true;
            return false;
        }

        private RelayCommand _editClubCommand;
        /// <summary>
        /// Command for editing a club
        /// </summary>
        public RelayCommand EditClubCommand
        {
            get
            {
                return _editClubCommand ??
                    (_editClubCommand = new RelayCommand(obj =>
                    {
                        Club c;
                        c = aec.listClub.SelectedItem as Club;
                        if (c != null)
                        {
                           lineClub = aec.listClub.SelectedItems[0];
                           aec.txtBoxNameClub.Text = (lineClub as Club).Name;
                           aec.cmbClubCountry.Text = (lineClub as Club).Country;
                        }
                        flgEditClub = true;
                    },obj1 => EditClubCommandCanExecute(aec)));
            }
        }

        /// <summary>
        /// Method for determining availability for a command 'EditClubCommand'
        /// </summary>
        private bool EditClubCommandCanExecute(AddEditClub parameter)
        {
            if (parameter.listClub.SelectedItem != null)
                return true;
            return false;
        }

        private RelayCommand _deleteClubCommand;
        /// <summary>
        /// Command for deleting a club
        /// </summary>
        public RelayCommand DeleteClubCommand
        {
            get
            {
                return _deleteClubCommand ??
                    (_deleteClubCommand = new RelayCommand(obj =>
                    {
                        if (aec.listClub.Items.Count != 0)
                        {
                            var message = MessageBox.Show("Do you really want to delete this club?",
                                          "Delete object", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (message == MessageBoxResult.Yes)
                            {
                                var line = aec.listClub.SelectedItems[0];
                                db.Clubs.Remove(line as Club);
                                db.SaveChanges();
                            }
                        }
                    },obj1 => DeleteClubCommandCanExecute(aec)));
            }
        }

        /// <summary>
        /// Method for determining availability for a command 'DeleteClubCommand'
        /// </summary>
        private bool DeleteClubCommandCanExecute(AddEditClub parameter)
        {
            if (parameter.listClub.SelectedItem != null && flgEditClub == false)
                return true;
            return false;
        }

        private RelayCommand _saveClubCommand;
        /// <summary>
        /// Command to save editable club data
        /// </summary>
        public RelayCommand SaveClubCommand
        {
            get
            {
                return _saveClubCommand ??
                    (_saveClubCommand = new RelayCommand(obj =>
                    {
                        int Id = (aec.listClub.SelectedItems[0] as Club).Id;
                        (lineClub as Club).Name = aec.txtBoxNameClub.Text;
                        (lineClub as Club).Country = aec.cmbClubCountry.Text;
                        db.SaveChanges();
                        ClearAllElements();
                        aec.listClub.SelectedItem = null;
                    }, obj1 => SaveClubCommandCanExecute()));
            }
        }

        /// <summary>
        /// Method for determining availability for a command 'SaveClubCommand'
        /// </summary>
        private bool SaveClubCommandCanExecute()
        {
            if (!String.IsNullOrEmpty(aec.txtBoxNameClub.Text) && !String.IsNullOrEmpty(aec.cmbClubCountry.Text) && flgEditClub == true)
                return true;
            flgEditClub = false;
            return false;
        }

        private RelayCommand _cancelClubCommand;
        /// <summary>
        /// Command to cancel (exit) the form
        /// </summary>
        public RelayCommand CancelClubCommand
        {
            get
            {
                return _cancelClubCommand ??
                    (_cancelClubCommand = new RelayCommand(obj =>
                    {
                        aec.Close();
                    }));
            }
        }

        /// <summary>
        /// Method for clearing all elements in the form
        /// </summary>
        private void ClearAllElements()
        {
            aec.txtBoxNameClub.Clear();
            aec.cmbClubCountry.Text = null;
        }
    }
}
