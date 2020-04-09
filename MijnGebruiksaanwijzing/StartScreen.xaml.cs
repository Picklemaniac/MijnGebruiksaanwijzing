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
        public StartScreen(string mentorEmail, string studentEmail)
        {
            InitializeComponent();
            txt_eigenEmail.Text = "Eigen email: " + studentEmail;
            txt_mentorEmail.Text = "Mentor email: " + mentorEmail;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var newScreen = new GameScreen();
            newScreen.Show();
            this.Close();
        }
    }
}
