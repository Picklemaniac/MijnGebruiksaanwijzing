using System.Windows;
using System.Windows.Controls;

namespace MijnGebruiksaanwijzing
{
    /// <summary>
    /// Interaction logic for StartScreen.xaml
    /// </summary>
    public partial class StartScreen : Window
    {
        string mEmail = "";
        string sEmail = "";

        public StartScreen(string mentorEmail, string studentEmail)
        {
            InitializeComponent();
            sEmail = studentEmail;
            mEmail = mentorEmail;

            txt_eigenEmail.Text = "Eigen email: " + studentEmail;
            txt_mentorEmail.Text = "Mentor email: " + mentorEmail;
        }

        private void Choose_Gamemode(object sender, RoutedEventArgs e)
        {
            try
            {
                GameScreen newScreen = new GameScreen(((Button)sender).Tag.ToString(), mEmail, sEmail);
                newScreen.Show();
                this.Close();
            }
            catch 
            {
                this.Close();
            }
        }

        private void btn_terug_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newScreen = new MainWindow();
            newScreen.Show();
            this.Close();
        }
    }
}
