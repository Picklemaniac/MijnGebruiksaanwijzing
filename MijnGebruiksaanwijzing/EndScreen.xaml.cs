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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Xml;

namespace MijnGebruiksaanwijzing
{
    /// <summary>
    /// Interaction logic for EndScreen.xaml
    /// </summary>
    public partial class EndScreen : Window
    {
        public EndScreen()
        {
            InitializeComponent();
        }

        private void btn_terug_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void btn_export_Click(object sender, RoutedEventArgs e)
        {
            doWork();
        }

        private void doWork()
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(@"..\..\XML\Game.xml");
            var kanker = xmldoc.SelectSingleNode("Game").SelectNodes("Cards");

            //Sample XML
            var xml = xmldoc;

            //File to write to
            var testFile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.pdf");

            //Standard PDF creation, nothing special here
            using (var fs = new FileStream(testFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var doc = new Document())
                {
                    using (var writer = PdfWriter.GetInstance(doc, fs))
                    {
                        doc.Open();

                        var t = new PdfPTable(3);

                        //Flag that the first row should be repeated on each page break
                        t.HeaderRows = 1;

                        t.AddCell("Belemmering");
                        t.AddCell("Oplossing");
                        t.AddCell("Wie kan mij daarbij helpen?");
                        t.CompleteRow();

                        //Loop through each CD row (this is so we can call complete later on)
                        foreach (XmlNode CD in xml.SelectSingleNode("Game").SelectNodes("Cards"))
                        {
                            var Cards = new Dictionary<string, string>
                                {
                                    { "RedCard", "" },
                                    { "YellowCard", "" },
                                    { "BlueCard", "" }
                                };

                            //Loop through each child of the current CD. Limit the number of children to our initial count just in case there are extra nodes.
                            foreach (XmlNode node in CD.ChildNodes)
                            {
                                Cards[node.Name] += node.InnerText + System.Environment.NewLine;
                            }

                            t.AddCell(Cards["RedCard"]);
                            t.AddCell(Cards["YellowCard"]);
                            t.AddCell(Cards["BlueCard"]);

                            //Just in case any rows have too few cells fill in any blanks
                            t.CompleteRow();
                        }

                        //Add the table to the document
                        doc.Add(t);

                        doc.Close();
                    }
                }
            }
        }
    }
}
