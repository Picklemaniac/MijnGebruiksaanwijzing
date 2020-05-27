using MijnGebruiksaanwijzing.Database;
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

        public GameScreen(string categorie, string mentorEmail, string studentEmail)
        {
            InitializeComponent();
            DBConnection conn = new DBConnection();

            conn.GetCards(categorie, "geel");
            conn.GetCards(categorie, "blauw");

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var newScreen = new EndScreen();
            newScreen.Show();
            this.Close();
        }

        private void btn_terug_Click(object sender, RoutedEventArgs e)
        {
            Canvas imageCanvas = new Canvas();
            imageCanvas.Width = 150;
            imageCanvas.Height = 150;

            Image imageYellow = new Image();
            imageYellow.Source = new BitmapImage(new Uri("IMG/school/School_YellowCard.png", UriKind.RelativeOrAbsolute));
            imageYellow.Width = 150;
            imageYellow.Height = 150;
            imageYellow.Stretch = Stretch.Fill;
            imageCanvas.Children.Add(imageYellow);

            TextBlock textBlock = new TextBlock();
            textBlock.Height = 110;
            textBlock.Width = 110;
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.Text = "Dit is een test";
            imageCanvas.Children.Add(textBlock);

            List<Canvas> yellowCards = new List<Canvas>();

            yellowCards.Add(imageCanvas);
            Yellow_Cards.ItemsSource = yellowCards;
        }
    }
}
