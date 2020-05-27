using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MijnGebruiksaanwijzing.Database
{
    class DBConnection
    {
        MySqlConnection _conn;
        public DBConnection()
        {
            string connectie = "Server=localhost;Database=Cards;Uid=root;Pwd=;";
            _conn = new MySqlConnection(connectie);
        }

        public DataView GetCards(string categorie, string kleur)
        {
            _conn.Open();

            MySqlCommand command = _conn.CreateCommand();

            command.CommandText = "SELECT * FROM cards WHERE Categorie = @categorie AND Kleur = @kleur;";
            command.Parameters.AddWithValue("@kleur", kleur);
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
