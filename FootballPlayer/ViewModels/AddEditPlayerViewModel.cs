using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using FootballPlayer.Models;
using FootballPlayer.Commands;
using FootballPlayer.DataContext;
using FootballPlayer.Views;
using System.IO;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;

namespace FootballPlayer.ViewModels
{
    /// <summary>
    /// Class for adding and editing a player
    /// </summary>
    public class AddEditPlayerViewModel : IDataErrorInfo
    {
        public static AddEditPlayer aep;
        FootballPlayerContext db = new FootballPlayerContext();
        bool flgEditPlayer;
        object linePlayer;
        public string Name { get; set; }
        public int Age { get; set; }
        public string Position { get; set; }
        public string Nationality { get; set; }
        public int ClubId { get; set; }
        public ObservableCollection<Player> Players { get; set; }
        public AddEditPlayerViewModel()
        {
            MainWindowViewModel.LoadCountriesInComboBox(aep.cmbPlayerNationality);
            var namesClubs = db.Clubs.OrderBy(c => c.Id).ToList();
            foreach (Club n in namesClubs)
                aep.cmbPlayerClub.Items.Add(n.Id);

            db.Players.Load();
            Players = db.Players.Local.ToObservableCollection();
        }
        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "Age":
                        if ((Age < 0) || (Age > 100))
                        {
                            error = "The age is entered incorrectly!";
                        }
                        break;
                    case "Name":
                        if (String.IsNullOrEmpty(Name))
                        {
                            error = "The name should not be empty";
                        }
                        break;
                    case "Position":
                        if (String.IsNullOrEmpty(Position))
                        {
                            error = "The position should not be empty";
                        }
                        break;
                    case "Nationality":
                        if (String.IsNullOrEmpty(Nationality))
                        {
                            error = "The nationality should not be empty";
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

        private RelayCommand _addPlayerCommand;
        /// <summary>
        /// Command for adding a player
        /// </summary>
        public RelayCommand AddPlayerCommand
        {
            get
            {
                return _addPlayerCommand ??
                    (_addPlayerCommand = new RelayCommand(obj =>
                    {
                        Player p = new Player()
                        {
                            Name = aep.txtBoxNamePlayer.Text,
                            Age = int.Parse(aep.txtBoxAgePlayer.Text),
                            Nationality = aep.cmbPlayerNationality.Text,
                            Position = aep.cmbPlayerPosition.Text,
                        };
                        db.Players.Add(p);
                        db.SaveChanges();
                        ClearAllElementsPlayer();
                    }, obj1 => AddPlayerCommandCanExecute()));
            }
        }

        /// <summary>
        /// Method for determining availability for a command 'AddPlayerCommand'
        /// </summary>
        private bool AddPlayerCommandCanExecute()
        {
            if (!String.IsNullOrEmpty(aep.txtBoxNamePlayer.Text) && 
                !String.IsNullOrEmpty(aep.txtBoxAgePlayer.Text) &&
                !String.IsNullOrEmpty(aep.cmbPlayerNationality.Text) &&
                !String.IsNullOrEmpty(aep.cmbPlayerPosition.Text) &&
                !String.IsNullOrEmpty(aep.cmbPlayerClub.Text) &&
                flgEditPlayer == false)
                return true;
            return false;
        }

        private RelayCommand _editPlayerCommand;
        /// <summary>
        /// Command for editing a player
        /// </summary>
        public RelayCommand EditPlayerCommand
        {
            get
            {
                return _editPlayerCommand ??
                    (_editPlayerCommand = new RelayCommand(obj =>
                    {
                        Player p;
                        p = aep.listPlayer.SelectedItem as Player;
                        if (p != null)
                        {
                            linePlayer = aep.listPlayer.SelectedItems[0];
                            aep.txtBoxNamePlayer.Text = (linePlayer as Player).Name;
                            aep.txtBoxAgePlayer.Text = (linePlayer as Player).Age.ToString();
                            aep.cmbPlayerNationality.Text = (linePlayer as Player).Nationality;
                            aep.cmbPlayerPosition.Text = (linePlayer as Player).Position;
                            aep.cmbPlayerClub.Text = (linePlayer as Player).ClubId.ToString();
                        }
                        flgEditPlayer = true;
                    }, obj1 => EditPlayerCommandCanExecute(aep)));
            }
        }

        /// <summary>
        /// Method for determining availability for a command 'EditPlayerCommand'
        /// </summary>
        private bool EditPlayerCommandCanExecute(AddEditPlayer parameter)
        {
            if (parameter.listPlayer.SelectedItem != null)
                return true;
            return false;
        }

        private RelayCommand _deletePlayerCommand;
        /// <summary>
        /// Command for deleting a player
        /// </summary>
        public RelayCommand DeletePlayerCommand
        {
            get
            {
                return _deletePlayerCommand ??
                    (_deletePlayerCommand = new RelayCommand(obj =>
                    {
                        if (aep.listPlayer.Items.Count != 0)
                        {
                            var message = MessageBox.Show("Do you really want to delete this player?",
                                          "Delete object", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (message == MessageBoxResult.Yes)
                            {
                                var line = aep.listPlayer.SelectedItems[0];
                                db.Players.Remove(line as Player);
                                db.SaveChanges();
                            }
                        }
                    }, obj1 => DeletePlayerCommandCanExecute(aep)));
            }
        }

        /// <summary>
        /// Method for determining availability for a command 'DeletePlayerCommand'
        /// </summary>
        private bool DeletePlayerCommandCanExecute(AddEditPlayer parameter)
        {
            if (parameter.listPlayer.SelectedItem != null && flgEditPlayer == false)
                return true;
            return false;
        }

        private RelayCommand _savePlayerCommand;
        /// <summary>
        /// Command to save editable player data
        /// </summary>
        public RelayCommand SavePlayerCommand
        {
            get
            {
                return _savePlayerCommand ??
                    (_savePlayerCommand = new RelayCommand(obj =>
                    {
                        int Id = (aep.listPlayer.SelectedItems[0] as Player).Id;
                        (linePlayer as Player).Name = aep.txtBoxNamePlayer.Text;
                        (linePlayer as Player).Age = int.Parse(aep.txtBoxAgePlayer.Text);
                        (linePlayer as Player).Nationality = aep.cmbPlayerNationality.Text;
                        (linePlayer as Player).Position = aep.cmbPlayerPosition.Text;
                        (linePlayer as Player).ClubId = int.Parse(aep.cmbPlayerClub.Text);
                        db.SaveChanges();
                        ClearAllElementsPlayer();
                        aep.listPlayer.SelectedItem = null;
                    }, obj1 => SavePlayerCommandCanExecute()));
            }
        }

        /// <summary>
        /// Method for determining availability for a command 'SavePlayerCommand'
        /// </summary>
        private bool SavePlayerCommandCanExecute()
        {
            if (!String.IsNullOrEmpty(aep.txtBoxNamePlayer.Text) && 
                !String.IsNullOrEmpty(aep.txtBoxAgePlayer.Text) &&
                !String.IsNullOrEmpty(aep.cmbPlayerPosition.Text) &&
                !String.IsNullOrEmpty(aep.cmbPlayerNationality.Text) &&
                !String.IsNullOrEmpty(aep.cmbPlayerClub.Text) &&
                flgEditPlayer == true)
                return true;
            flgEditPlayer = false;
            return false;
        }

        private RelayCommand _cancelPlayerCommand;
        /// <summary>
        /// Command to cancel (exit) the form
        /// </summary>
        public RelayCommand CancelPlayerCommand
        {
            get
            {
                return _cancelPlayerCommand ??
                    (_cancelPlayerCommand = new RelayCommand(obj =>
                    {
                        aep.Close();
                    }));
            }
        }

        /// <summary>
        /// Method for clearing all elements in the form
        /// </summary>
        private void ClearAllElementsPlayer()
        {
            aep.txtBoxNamePlayer.Clear();
            aep.txtBoxAgePlayer.Clear();
            aep.cmbPlayerPosition.Text = null;
            aep.cmbPlayerNationality.Text = null;
            aep.cmbPlayerClub.Text = null;
        }
    }
}
