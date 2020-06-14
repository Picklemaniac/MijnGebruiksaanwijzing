﻿using iTextSharp.xmp.impl;
using Microsoft.SqlServer.Server;
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
using System.Xml;

namespace MijnGebruiksaanwijzing
{
    /// <summary>
    /// Interaction logic for GameScreen.xaml
    /// </summary>
    public partial class GameScreen : Window
    {
        DBConnection conn = new DBConnection();

        List<RedCard> Rood_Selected = new List<RedCard>();
        List<string> Geel_Selected  = new List<string>();
        List<string> Blauw_Selected = new List<string>();

        List<Canvas> CardListRed    = new List<Canvas>();
        List<Canvas> CardListYellow = new List<Canvas>();
        List<Canvas> CardListBlue   = new List<Canvas>();

        string Categorie;
        string mEmail;
        string sEmail;

        public GameScreen(string categorie, string mentorEmail, string studentEmail)
        {
            InitializeComponent();
            Categorie = categorie;
            mEmail = mentorEmail;
            sEmail = studentEmail;
            ShowCards();
        }

        private void Volgende_Click(object sender, RoutedEventArgs e)
        {
            if (Rood_Cards.SelectedItems.Count == 0 || Geel_Cards.SelectedItems.Count == 0 || Blauw_Cards.SelectedItems.Count == 0)
            {
                MessageBox.Show("U heeft niet alles gekozen.", "Fout", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                MessageBoxResult r = MessageBox.Show("Wilt u meer kaarten selecteren?", "Nog een keer?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

                if (r == MessageBoxResult.Yes)
                {
                    GetSelected();
                    WriteToXML();
                    EndTurn();
                }
                else
                {
                    GetSelected();
                    WriteToXML();

                    var newScreen = new EndScreen(Categorie, mEmail, sEmail);
                    newScreen.Show();
                    this.Close();
                }
            }
        }

        private void btn_terug_Click(object sender, RoutedEventArgs e)
        {
            var StartScreen = new StartScreen(mEmail, sEmail);
            StartScreen.Show();
            this.Close();
        }

        private void ShowCards()
        {
            foreach (DataRowView row in conn.GetCards(Categorie))
            {
                Canvas imageCanvas = new Canvas();
                imageCanvas.Width = 150;
                imageCanvas.Height = 150;

                Image Image = new Image();
                Image.Source = new BitmapImage(new Uri("IMG/" + Categorie.ToLower() + "/" + Categorie + "_" + row[2].ToString() + ".png", UriKind.RelativeOrAbsolute));
                Image.Width = 150;
                Image.Height = 150;
                Image.Stretch = Stretch.Fill;
                imageCanvas.Children.Add(Image);

                TextBlock textBlock = new TextBlock();
                textBlock.Height = 110;
                textBlock.Width = 110;
                textBlock.Tag = row[0].ToString();
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

            if (CardListRed.Count == 0)
            {
                MessageBox.Show("U heeft het " + Categorie + " spel uitgespeeld." + Environment.NewLine + "U word naar het eind scherm gebracht.", "klaar", MessageBoxButton.OK);

                var newScreen = new EndScreen(Categorie, mEmail, sEmail);
                newScreen.Show();
                this.Close();
            }
            else
            {
                Rood_Cards.ItemsSource = CardListRed;
                Geel_Cards.ItemsSource = CardListYellow;
                Blauw_Cards.ItemsSource = CardListBlue;
            }
        }

        public void GetSelected()
        {
            TextBlock red_Textblock = ((TextBlock)((Canvas)Rood_Cards.SelectedItem).Children[1]);
            Rood_Selected.Add(new RedCard { ID = red_Textblock.Tag.ToString(), Text = red_Textblock.Text });

            foreach (Canvas item in Geel_Cards.SelectedItems)
            {
                Geel_Selected.Add(((TextBlock)item.Children[1]).Text);
            }
            foreach (Canvas item in Blauw_Cards.SelectedItems)
            {
                Blauw_Selected.Add(((TextBlock)item.Children[1]).Text);
            }
        }

        public void WriteToXML()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"..\..\XML\" + Categorie + ".xml");
            XmlElement Cards = doc.CreateElement("Cards");

            XmlElement RedCard = doc.CreateElement("RedCard");
            RedCard.SetAttribute("ID", Rood_Selected[0].ID);
            RedCard.InnerText = Rood_Selected[0].Text;
            Cards.AppendChild(RedCard);

            foreach (string GeelCard in Geel_Selected)
            {
                XmlElement YellowCard = doc.CreateElement("YellowCard");
                YellowCard.InnerText = GeelCard;
                Cards.AppendChild(YellowCard);
            }

            foreach (string BlauwCard in Blauw_Selected)
            {
                XmlElement BlueCard = doc.CreateElement("BlueCard");
                BlueCard.InnerText = BlauwCard;
                Cards.AppendChild(BlueCard);
            }

            if (txt_opmerking.Text != "")
            {
                XmlElement Opmerking = doc.CreateElement("Opmerking");
                Opmerking.InnerText = txt_opmerking.Text;
                Cards.AppendChild(Opmerking);
            }

            doc.DocumentElement.AppendChild(Cards);

            doc.Save(@"..\..\XML\" + Categorie + ".xml");
        }

        public void EndTurn()
        {
            Rood_Selected.Clear();
            Geel_Selected.Clear();
            Blauw_Selected.Clear();

            CardListRed.Remove((Canvas)Rood_Cards.SelectedItem);
            Rood_Cards.ItemsSource = null;
            Rood_Cards.ItemsSource = CardListRed;

            Rood_Cards.SelectedItem = null;
            Geel_Cards.SelectedItem = null;
            Blauw_Cards.SelectedItem = null;
            txt_opmerking.Text = "";

            if (CardListRed.Count == 0)
            {
                MessageBox.Show("U heeft het " + Categorie + " spel uitgespeeld." + Environment.NewLine + "U word naar het eind scherm gebracht.", "klaar", MessageBoxButton.OK);

                var newScreen = new EndScreen(Categorie, mEmail, sEmail);
                newScreen.Show();
                this.Close();
            }
        }

        class RedCard
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }
    }
}
