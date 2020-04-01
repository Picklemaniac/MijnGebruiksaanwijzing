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

namespace MijnGebruiksaanwijzing
{
    /// <summary>
    /// Interaction logic for GameScreen.xaml
    /// </summary>
    public partial class GameScreen : Window
    {
        List<string> List = new List<string>(); 

        public GameScreen()
        {
            InitializeComponent();

            for (int i = 0; i < 100; i++)
            {
                List.Add(i.ToString());
            }

            Test.ItemsSource = List;
        }
    }
}
