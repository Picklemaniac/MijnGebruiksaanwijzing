using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        
        public bool IsValidEmailAddress(string s)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(s);
        }

        private void btn_doorgaan_Click(object sender, RoutedEventArgs e)
        {
            if (txt_eigenemail.Text == "" || txt_mentoremail.Text == "")
            {
                MessageBox.Show("Voer de email van uzelf en uw mentor in");
            }
            else
            {
                if (IsValidEmailAddress(txt_mentoremail.Text) && IsValidEmailAddress(txt_eigenemail.Text))
                {
                    var newScreen = new StartScreen(txt_mentoremail.Text, txt_eigenemail.Text);
                    newScreen.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Dit zijn geen geldige e-mail adressen,");
                }
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
