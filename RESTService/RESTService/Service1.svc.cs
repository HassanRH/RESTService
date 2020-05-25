using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RESTService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public IList<Passager> GetAllpassager()
        {
            const string sqlstring = "SELECT * from Passager order by id"; // constant (const) string fordi den ikke ændre sig

            using (var sqlConnection = new SqlConnection(GetConnectionString())) //hvis man bruger using så behøves man ikke og lukke sin connection den gøre det af sig selv 
            {
                sqlConnection.Open();// åbner forbindelsen til databasen
                using (var sqlCommand = new SqlCommand(sqlstring, sqlConnection))
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        var liste = new List<Passager>(); // blivdr sat ind i listen
                        while (reader.Read()) // henter fra i bunden så vi ikke skal skrive dem ved de andre
                        {
                            var _Passager = ReadPassager(reader);
                            liste.Add(_Passager);
                        }
                        return liste;
                    }
                }
            }
        }


        public IList<Passager> GetpassagerByNavn(string navn)
        {
            var sqlstring = $"Select * from passager where Navn = '{navn}'";

            using (var sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                using (var sqlcommand = new SqlCommand(sqlstring, sqlConnection))
                {
                    using (var reader = sqlcommand.ExecuteReader())
                    {
                        var liste = new List<Passager>();
                        while (reader.Read())
                        {
                            var _passager = ReadPassager(reader);
                            liste.Add(_passager);
                        }
                        return liste;
                    }
                }
            }
        }


        public Passager GetPassagerById(string Id)
        {
            var sqlstring = $"Select * from Passager where Id = {Id}";

            using (var sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                using (var sqlcommand = new SqlCommand(sqlstring, sqlConnection))
                {
                    using (var reader = sqlcommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var _Passager = ReadPassager(reader);
                            return _Passager;
                        }
                        return null;
                    }
                }
            }
        }


        public void DeletePassager(string id)
        {
            var sqlstring = $"DELETE FROM Passager where id = {id}";

            using (var sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                using (var sqlcommand = new SqlCommand(sqlstring, sqlConnection))
                {
                    sqlcommand.ExecuteNonQuery();
                }
            }
        }


        public int PostPassager(Passager passger)
        {
            const string sqlstring = "INSERT INTO Passager (Navn, BagageVaegt, Efternavn, BagageAntal, FlyNummer) values (@Navn, @BagageVaegt, @Efternavn, @BagageAntal, @FlyNummer)"; // "@" udskifter det nede underfor at undgå sqlinjections

            using (var DBconnection = new SqlConnection(GetConnectionString()))
            {
                DBconnection.Open();

                using (var addstuCommand = new SqlCommand(sqlstring, DBconnection))
                {
                    addstuCommand.Parameters.AddWithValue("@Navn", passger.Navn); //bliver udskifter med det fra vores objekt
                    addstuCommand.Parameters.AddWithValue("@BagageVaegt", passger.BagageVaegt);//bliver udskifter med det fra vores objekt
                    addstuCommand.Parameters.AddWithValue("@Efternavn", passger.Efternavn);//bliver udskifter med det fra vores objekt
                    addstuCommand.Parameters.AddWithValue("@BagageAntal", passger.BagageAntal);//bliver udskifter med det fra vores objekt
                    addstuCommand.Parameters.AddWithValue("@FlyNummer", passger.FlyNummer); //giver en int med hvor mange rækker der er ændret
                    var rowsaffected = addstuCommand.ExecuteNonQuery();
                    return rowsaffected; //her retuner den
                }
            }
        }


        public int UpdatePassager(string id, Passager passager)
        {
            SqlConnection conn = new SqlConnection(GetConnectionString());
            conn.Open();
            string sql = $"UPDATE Passager SET Navn = @Navn, BagageVaegt = @BagageVaegt, Efternavn = @Efternavn, BagageAntal = @BagageAntal, FlyNummer = @FlyNummer WHERE id = {id}"; 

            using (SqlCommand command = new SqlCommand(sql, conn))
            {
                command.Parameters.AddWithValue("@Navn", passager.Navn);
                command.Parameters.AddWithValue("@BagageVaegt", passager.BagageVaegt);
                command.Parameters.AddWithValue("@Efternavn", passager.Efternavn);
                command.Parameters.AddWithValue("@BagageAntal", passager.BagageAntal);
                command.Parameters.AddWithValue("@FlyNummer", passager.FlyNummer);
                return command.ExecuteNonQuery(); //den udføre comandoen
            }
        }

        //string til at hente vores connection string
        private static string GetConnectionString()
        {
            var connectionStringSettingsCollection = ConfigurationManager.ConnectionStrings;
            var connStringSettings = connectionStringSettingsCollection["MikDatabaseAzure"];
            return connStringSettings.ConnectionString;
        }

        //vores læse metode vi kan bruge sammen med reader - den sætter bare læste værdier ind på nyt obj
        private static Passager ReadPassager(IDataRecord reader)
        {
            var Id = reader.GetInt32(0); // henter fra tabellen som starter fra 0
            var Navn = reader.GetString(1);
            var BagageVaegt = reader.GetDouble(2);
            var Efternavn = reader.GetString(3);
            var BagageAntal = reader.GetInt32(4);
            var FlyNummer = reader.GetInt32(5);

            var i = new Passager { Id = Id, BagageAntal = BagageAntal, BagageVaegt = BagageVaegt, Efternavn = Efternavn, FlyNummer = FlyNummer, Navn = Navn }; //bliver lavet en ny passager

            return i; //ny passager
        }

    }
}
