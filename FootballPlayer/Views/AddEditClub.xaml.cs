using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FootballPlayer.ViewModels;
using FootballPlayer.Models;

namespace FootballPlayer.Views
{
    /// <summary>
    /// Interaction logic for AddEditClub.xaml
    /// </summary>
    public partial class AddEditClub : Window
    {
        public AddEditClub()
        {
            InitializeComponent();
            AddEditClubViewModel.aec = this;
            DataContext = new AddEditClubViewModel();
        }
    }
}
