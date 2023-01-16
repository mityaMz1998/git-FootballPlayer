using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FootballPlayer.Models
{
    /// <summary>
    /// Player
    /// </summary>
    public class Player : INotifyPropertyChanged
    {
        private string _name;
        private int _age;
        private string _nationality;
        private string _position;
        public int _clubId;

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
        public int Age
        {
            get { return _age; }
            set
            {
                _age = value;
                OnPropertyChanged("Age");
            }
        }
        public string Nationality
        {
            get { return _nationality; }
            set
            {
                _nationality = value;
                OnPropertyChanged("Nationality");
            }
        }
        public string Position
        {
            get { return _position; }
            set
            {
                _position = value;
                OnPropertyChanged("Position");
            }
        }
        public int ClubId
        {
            get { return _clubId; }
            set
            {
                _clubId = value;
                OnPropertyChanged("ClubId");
            }
        }
        public Club Club { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
