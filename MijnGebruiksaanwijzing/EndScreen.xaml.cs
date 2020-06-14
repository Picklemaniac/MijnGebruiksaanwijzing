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
using System.Net.Mail;
using Renci.SshNet.Messages;
using System.Xml.Linq;

namespace MijnGebruiksaanwijzing
{
    /// <summary>
    /// Interaction logic for EndScreen.xaml
    /// </summary>
    public partial class EndScreen : Window
    {
        string StudentEmail;
        string MentorEmail;
        string categorie;
        string documentName = "";
        int exported;

        public EndScreen(string Categorie, string mEmail, string sEmail)
        {
            InitializeComponent();
            StudentEmail = sEmail;
            MentorEmail = mEmail;
            categorie = Categorie;
            exported = 0;
        }

        private void btn_terug_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btn_export_Click(object sender, RoutedEventArgs e)
        {
            doWork();
        }

        private void doWork()
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(@"..\..\XML\" + categorie  + ".xml");

            //Sample XML
            var xml = xmldoc;

            documentName = DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss");

            //File to write to
            var testFile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), documentName + ".pdf");

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
            exported = 1;
            MessageBox.Show("Succesvol geëxporteerd naar uw bureaublad");
        }

        private void SendEmail()
        {
            try
            {
                if (documentName != "")
                {
                    MailMessage mail = new MailMessage();

                    SmtpClient SmtpServer = new SmtpClient("smtp.mail.com");

                    mail.From = new MailAddress("MijnGebruiksAanwijzing@myself.com");

                    mail.To.Add(MentorEmail);
                    mail.CC.Add(StudentEmail);

                    mail.Subject = "Resultaten Onderzoek";

                    StringBuilder sbBody = new StringBuilder();

                    sbBody.AppendLine("Beste Heer / Mevrouw,");
                    sbBody.AppendLine("Hierbij de resultaten van Mijn Gebruiksaanwijzing");
                    sbBody.AppendLine("Deze resultaten zijn verzonden door: " + StudentEmail);
                    sbBody.AppendLine("Met vriendelijke groet,");
                    sbBody.AppendLine("Mijn Gebruiksaanwijzing");

                    mail.Body = sbBody.ToString();

                    string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), documentName + ".pdf");
                    System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(path);

                    mail.Attachments.Add(attachment);

                    SmtpServer.Credentials = new System.Net.NetworkCredential("MijnGebruiksAanwijzing@myself.com", "Summacollege123");

                    SmtpServer.Port = 587;


                    SmtpServer.Send(mail);
                    MessageBox.Show("De email is verzonden naar uw mentor");
                }
                else
                {
                    MessageBox.Show("Gelieve eerst te exporteren voor het verzenden.", "Fout", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Momenteel kunnen er maar een gelimiteerd aantal emails per 30 minuten verzonden worden. Sorry voor het ongemak.", "Fout", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void BackToHome()
        {
            if (exported != 1)
            {
                MessageBoxResult r = MessageBox.Show("U heeft de resultaten nog niet geexporteerd. Als u terug gaat naar het hoofd menu moet u het spel opnieuw spelen. Weet u zeker dat u dit wilt doen?", "Terug naar het hoofd menu,",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

                if (r == MessageBoxResult.Yes)
                {
                    ResetGame();
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
            }
            else
            {
                ResetGame();
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }

        private void ResetGame()
        {
            File.Delete(@"..\..\XML\" + categorie  + ".xml");
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";


            XmlWriter writer = XmlWriter.Create(@"..\..\XML\" + categorie + ".xml", settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("Game");
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Dispose();
        }

        private void btn_stuurmentor_Click(object sender, RoutedEventArgs e)
        {
            SendEmail();
        }

        private void btn_skip_Click(object sender, RoutedEventArgs e)
        {
            BackToHome();
        }
    }
}
