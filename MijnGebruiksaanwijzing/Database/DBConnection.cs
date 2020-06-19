using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;

namespace MijnGebruiksaanwijzing.Database
{
    class DBConnection
    {
        MySqlConnection _conn;
        List<string> previousCards = new List<string>();

        public DBConnection()
        {
            string connectie = "Server=localhost;Database=Cards;Uid=root;Pwd=;";
            _conn = new MySqlConnection(connectie);
        }

        public DataView GetCards(string categorie)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"..\..\XML\" + categorie + ".xml");

            foreach (XmlNode CD in doc.SelectSingleNode("Game").SelectNodes("Cards"))
            {
                foreach (XmlNode node in CD.ChildNodes)
                {
                    if (node.Name == "RedCard")
                    {
                        previousCards.Add(node.Attributes[0].InnerText);
                    }
                }
            }

            _conn.Open();

            MySqlCommand command = _conn.CreateCommand();

            string commandText = "SELECT * FROM cards WHERE Categorie = @categorie";

            foreach(string ID in previousCards)
            {
                commandText += " AND id !=" + ID;
            }

            command.CommandText = commandText;
            command.Parameters.AddWithValue("@categorie", categorie);

            MySqlDataReader reader = command.ExecuteReader();
            Console.WriteLine(reader.ToString());
            DataTable firstdatatable = new DataTable();
            firstdatatable.Load(reader);

            _conn.Close();

            return firstdatatable.DefaultView;
        }
    }
}
