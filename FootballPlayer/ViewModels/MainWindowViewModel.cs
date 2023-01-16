using System;
using System.Linq;
using System.Text;
using FootballPlayer.Commands;
using FootballPlayer.Views;
using System.IO;
using System.Windows.Controls;

namespace FootballPlayer.ViewModels
{
    /// <summary>
    /// Class for performing operations on club and player objects
    /// </summary>
    public class MainWindowViewModel
    {
        public static MainWindow mw;
        /// <summary>
        /// Method for reading a file 'country.csv'
        /// </summary>
        public static void LoadCountriesInComboBox(ComboBox comboBox)
        {
            string basePath = "D:/C#/C# обучение/Разбор тем по C#/repos/WPF/FootballPlayer/FootballPlayer/Files";
            string file = "country.csv";
            string stringPath = Path.Combine(basePath, file);
            using (StreamReader sr = new StreamReader(stringPath, Encoding.UTF8))
            {
                int lineNumber = 1;
                string[] str = File.ReadAllLines(stringPath, Encoding.UTF8);
                string[] eachColumn = str.Select(s => s.Split(';')[2].Trim('"', '"')).ToArray();
                foreach (string s in eachColumn)
                {
                    if (lineNumber != 1)
                        comboBox.Items.Add(s);
                    lineNumber++;
                }
            }
        }

        private RelayCommand _openClubCommand;
        /// <summary>
        /// Class for opening the form 'AddEditClub'
        /// </summary>
        public RelayCommand OpenClubCommand
        {
            get
            {
                return _openClubCommand ??
                    (_openClubCommand = new RelayCommand(obj =>
                    {
                        AddEditClub addEditClub = new AddEditClub();
                        addEditClub.Owner = mw;
                        addEditClub.ShowDialog();
                    }));
            }
        }

        private RelayCommand _openPlayerCommand;
        /// <summary>
        /// Class for opening the form 'AddEditPlayer'
        /// </summary>
        public RelayCommand OpenPlayerCommand
        {
            get
            {
                return _openPlayerCommand ??
                    (_openPlayerCommand = new RelayCommand(obj =>
                    {
                        AddEditPlayer addEditPlayer = new AddEditPlayer();
                        addEditPlayer.Owner = mw;
                        addEditPlayer.ShowDialog();
                    }));
            }
        }

        private RelayCommand _exitMainWindowCommand;
        /// <summary>
        /// Command to exit the form (сompletion of the process)
        /// </summary>
        public RelayCommand ExitMainWindowCommand
        {
            get
            {
                return _exitMainWindowCommand ??
                    (_exitMainWindowCommand = new RelayCommand(obj =>
                    {
                        mw.Close();
                    }));
            }
        }
    }
}
