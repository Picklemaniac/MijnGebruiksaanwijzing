﻿using MijnGebruiksaanwijzing.Database;
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
            ShowCards();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var newScreen = new EndScreen();
            newScreen.Show();
            this.Close();
        }

        private void btn_terug_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ShowCards()
        {
            List<Canvas> CardListRed = new List<Canvas>();
            List<Canvas> CardListYellow = new List<Canvas>();
            List<Canvas> CardListBlue = new List<Canvas>();

            foreach (DataRowView row in conn.GetCards(Categorie))
            {
                Canvas imageCanvas = new Canvas();
                imageCanvas.Width = 150;
                imageCanvas.Height = 150;

                Image Image = new Image();
                Image.Source = new BitmapImage(new Uri("IMG/school/School_" + row[2].ToString() + ".png", UriKind.RelativeOrAbsolute));
                Image.Width = 150;
                Image.Height = 150;
                Image.Stretch = Stretch.Fill;
                imageCanvas.Children.Add(Image);

                TextBlock textBlock = new TextBlock();
                textBlock.Height = 110;
                textBlock.Width = 110;
                textBlock.TextAlignment = TextAlignment.Center;
                textBlock.TextWrapping = TextWrapping.Wrap;
                textBlock.Text = row[3].ToString();
                textBlock.Margin = new Thickness(20, 25, 0, 0);
                imageCanvas.Children.Add(textBlock);

                if (row[2].ToString() == "rood")
                {
                    CardListRed.Add(imageCanvas);
                }
                else if (row[2].ToString() == "geel")
                {
                    CardListYellow.Add(imageCanvas);
                }
                else if (row[2].ToString() == "blauw")
                {
                    CardListBlue.Add(imageCanvas);
                }

            }

            Rood_Cards.ItemsSource = CardListRed;
            Geel_Cards.ItemsSource = CardListYellow;
            Blauw_Cards.ItemsSource = CardListBlue;
        }
    }
}
