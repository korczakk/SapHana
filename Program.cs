using System;
using Sap.Data.Hana;
namespace dotNETQuery
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Start..");
            try
            {
                // Option 1, retrieve the connection parameters from the hdbuserstore
                // User1UserKey retrieved from hdbuserstore contains server:port, UID and PWD
                //using (var conn = new HanaConnection("key=User1UserKey;encrypt=true;sslValidateCertificate=false"))

                //Option2, specify the connection parameters
                using (var conn = new HanaConnection("Server=10.7.168.11:39015;UID=User1;PWD=Password1;encrypt=true;sslValidateCertificate=false"))

                // encrypt and sslValidateCertificate should be true for HANA Cloud connections
                // As of SAP HANA Client 2.6, connections on port 443 enable encryption by default
                // sslValidateCertificate should be set to false when connecting
                // to an SAP HANA, express edition instance that uses a self-signed certificate.

                {
                    conn.Open();
                    Console.WriteLine("Connected");
                    var query = "SELECT TITLE, FIRSTNAME, NAME FROM HOTEL.CUSTOMER";
                    using (var cmd = new HanaCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine("Query result:");
                        // Print column names
                        var sbCol = new System.Text.StringBuilder();
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            sbCol.Append(reader.GetName(i).PadRight(20));
                        }
                        Console.WriteLine(sbCol.ToString());
                        // Print rows
                        while (reader.Read())
                        {
                            var sbRow = new System.Text.StringBuilder();
                            for (var i = 0; i < reader.FieldCount; i++)
                            {
                                sbRow.Append(reader[i].ToString().PadRight(20));
                            }
                            Console.WriteLine(sbRow.ToString());
                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
        }
    }
}