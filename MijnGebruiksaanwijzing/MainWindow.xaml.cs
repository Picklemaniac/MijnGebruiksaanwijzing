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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MijnGebruiksaanwijzing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_doorgaan_Click(object sender, RoutedEventArgs e)
        {
            if (txt_eigenemail.Text == "" || txt_mentoremail.Text == "")
            {
                MessageBox.Show("Voer de email van uzelf en uw mentor in");
            }
            else
            {
                var newScreen = new StartScreen(txt_mentoremail.Text, txt_eigenemail.Text);
                newScreen.Show();
                this.Close();
            }

        }

        private void TEST_Click(object sender, RoutedEventArgs e)
        {
            var newScreen = new StartScreen(txt_mentoremail.Text, txt_eigenemail.Text);
            newScreen.Show();
            this.Close();
        }
    }
}
