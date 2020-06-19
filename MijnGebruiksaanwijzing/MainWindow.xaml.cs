using System.Text.RegularExpressions;
using System.Windows;

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
                MessageBox.Show("Voer het emailadres van uzelf en uw mentor in.");
            }
            else
            {
                if (IsValidEmailAddress(txt_mentoremail.Text) && IsValidEmailAddress(txt_eigenemail.Text))
                {
                    StartScreen newScreen = new StartScreen(txt_mentoremail.Text, txt_eigenemail.Text);
                    newScreen.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("De ingevulde emailadressen zijn ongeldig.");
                }
            }

        }
    }
}
