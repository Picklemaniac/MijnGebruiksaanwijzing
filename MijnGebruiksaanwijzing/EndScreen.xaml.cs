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
        public EndScreen(string mEmail, string sEmail)
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

            //Sample XML
            var xml = xmldoc;

            //File to write to
            var testFile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "MijnUitwerking.pdf");

            //Standard PDF creation, nothing special here
            using (var fs = new FileStream(testFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var doc = new Document())
                {
                    using (var writer = PdfWriter.GetInstance(doc, fs))
                    {
                        doc.Open();

                        var t = new PdfPTable(4);

                        //Flag that the first row should be repeated on each page break
                        t.HeaderRows = 1;

                        BaseColor redtxtColor    = new BaseColor(255, 144, 150);
                        BaseColor yellowtxtColor = new BaseColor(250, 220, 60);
                        BaseColor bluetxtColor   = new BaseColor(140, 170, 255);
                        BaseColor opmtxtColor    = new BaseColor(175, 230, 230);
                        BaseColor normaltxtColor = new BaseColor(0, 0, 0);

                        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

                        Font redHelvetica    = new Font(bfTimes, 12, Font.BOLD, redtxtColor);
                        Font yellowHelvetica = new Font(bfTimes, 12, Font.BOLD, yellowtxtColor);
                        Font blueHelvetica   = new Font(bfTimes, 12, Font.BOLD, bluetxtColor);
                        Font opmHelvetica    = new Font(bfTimes, 12, Font.BOLD, opmtxtColor);
                        Font normalHelvetica = new Font(bfTimes, 12, Font.NORMAL, normaltxtColor);

                        t.AddCell(new Phrase("Belemmering", redHelvetica));
                        t.AddCell(new Phrase("Oplossing", yellowHelvetica));
                        t.AddCell(new Phrase("Wie kan mij daarbij helpen?", blueHelvetica));
                        t.AddCell(new Phrase("Opmerking", opmHelvetica));
                        t.CompleteRow();

                        //Loop through each CD row (this is so we can call complete later on)
                        foreach (XmlNode CD in xml.SelectSingleNode("Game").SelectNodes("Cards"))
                        {
                            var Cards = new Dictionary<string, string>
                                {
                                    { "RedCard", "" },
                                    { "YellowCard", "" },
                                    { "BlueCard", "" },
                                    { "Opmerking", "" }
                                };

                            //Loop through each child of the current CD. Limit the number of children to our initial count just in case there are extra nodes.
                            foreach (XmlNode node in CD.ChildNodes)
                            {
                                Cards[node.Name] += node.InnerText + System.Environment.NewLine;
                            }

                            BaseColor redColor    = new BaseColor(255, 195, 195);
                            BaseColor yellowColor = new BaseColor(255, 255, 195);
                            BaseColor blueColor   = new BaseColor(195, 210, 255);
                            BaseColor opmColor    = new BaseColor(195, 255, 255);

                            PdfPCell redCell    = new PdfPCell();
                            PdfPCell yellowCell = new PdfPCell();
                            PdfPCell blueCell   = new PdfPCell();
                            PdfPCell opmCell    = new PdfPCell();

                            redCell.BackgroundColor    = redColor;
                            yellowCell.BackgroundColor = yellowColor;
                            blueCell.BackgroundColor   = blueColor;
                            opmCell.BackgroundColor    = opmColor;

                            redCell.AddElement(new Phrase(Cards["RedCard"], normalHelvetica));
                            yellowCell.AddElement(new Phrase(Cards["YellowCard"], normalHelvetica));
                            blueCell.AddElement(new Phrase(Cards["BlueCard"], normalHelvetica));
                            opmCell.AddElement(new Phrase(Cards["Opmerking"], normalHelvetica));

                            t.AddCell(redCell);
                            t.AddCell(yellowCell);
                            t.AddCell(blueCell);
                            t.AddCell(opmCell);

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
