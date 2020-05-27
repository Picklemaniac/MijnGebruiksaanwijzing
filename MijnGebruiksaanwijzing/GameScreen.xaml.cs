using MijnGebruiksaanwijzing.Database;
using System;
using System.Collections.Generic;
using System.Data;
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
        DBConnection conn = new DBConnection();
        string Categorie;

        public GameScreen(string categorie, string mentorEmail, string studentEmail)
        {
            InitializeComponent();
            Categorie = categorie;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var newScreen = new EndScreen();
            newScreen.Show();
            this.Close();
        }

        private void btn_terug_Click(object sender, RoutedEventArgs e)
        {
            List<Canvas> yellowCards = new List<Canvas>();

            foreach (DataRowView row in conn.GetCards(Categorie, "geel")) {
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
                textBlock.Text = row[3].ToString();
                textBlock.Margin = new Thickness(20, 25, 0, 0);
                imageCanvas.Children.Add(textBlock);

                yellowCards.Add(imageCanvas);
            }
            Yellow_Cards.ItemsSource = yellowCards;
        }
    }
}
