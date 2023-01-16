using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FootballPlayer.Models
{
    /// <summary>
    /// Club
    /// </summary>
    public class Club : INotifyPropertyChanged
    {
        private string _name;
        private string _country;

        public int Id { get; set; }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Country
        {
            get { return _country; }
            set
            {
                _country = value;
                OnPropertyChanged("Country");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
