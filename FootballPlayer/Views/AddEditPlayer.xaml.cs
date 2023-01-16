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

namespace FootballPlayer.Views
{
    /// <summary>
    /// Interaction logic for AddEditPlayer.xaml
    /// </summary>
    public partial class AddEditPlayer : Window
    {
        public AddEditPlayer()
        {
            InitializeComponent();
            AddEditPlayerViewModel.aep = this;
            DataContext = new AddEditPlayerViewModel();
        }
    }
}
