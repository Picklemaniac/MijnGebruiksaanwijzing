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
                var newScreen = new GameScreen(((Button)sender).Tag.ToString(), mEmail, sEmail);
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
            var newScreen = new MainWindow();
            newScreen.Show();
            this.Close();
        }
    }
}
