using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACWorkShop
{
    public static class Configuration
    {
        public static string DataSource { get; set; }
        public static string InitialCatalog { get; set; }
        public static bool IntegratedSecurity { get; set; }
        public static string UserID { get; set; }
        public static string Password { get; set; }
        public static int ConnectionTimeout { get; set; }

        public static string ConnectionString { get; set; }
        public static bool Error { get; set; }
        public static string ErrorMessage { get; set; }

        public static bool TestCon()
        {

            MySqlConnectionStringBuilder sqlBuilder = new MySqlConnectionStringBuilder()
            {
                Server = DataSource,
                Database = InitialCatalog,
                IntegratedSecurity = IntegratedSecurity,
                UserID = UserID,
                Password = Password
            };
            ConnectionString = sqlBuilder.ConnectionString;

            try
            {
                using (MySqlConnection con = new MySqlConnection(ConnectionString))
                {
                    con.Open();
                    con.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
        public static string GetPath()
        {
            string fullPathName = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string executableName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".exe";
            int x = fullPathName.IndexOf(executableName);

            return fullPathName.Remove(x);
        }
        public static bool SaveDetails()
        {
            if (!TestCon())
            {
                Error = true;
                ErrorMessage = "Connection did not open sucessfully. Please try again.";
                return false;
            }
            var FileName = "";
#if DEBUG
            FileName = "DBConfigDev";
#else
            FileName = "DBConfig";
#endif

            string path = Path.Combine(GetPath(), FileName);
            string constring = ConnectionString;

            try
            {
                File.WriteAllText(path, Crypto.Encrypt(constring));
                return true;
            }
            catch (Exception ex)
            {
                Error = true;
                ErrorMessage = ex.Message;
                return false;
            }
        }

        public static bool ReadDetails()
        {
            var FileName = "";
#if DEBUG
            FileName = "DBConfigDev";
#else
            FileName = "DBConfig";
#endif

            if (!File.Exists(Path.Combine(GetPath(), FileName)))
            {
                return false;
            }
            string contents = string.Empty;
            Password = string.Empty;
            UserID = string.Empty;
            DataSource = string.Empty;
            InitialCatalog = string.Empty;
            IntegratedSecurity = false;
            try
            {
#if DEBUG
                contents = File.ReadAllText(Path.Combine(GetPath(), "DBConfigDev"));
#else
                    contents = File.ReadAllText(Path.Combine(GetPath(), "DBConfig"));
#endif
            }
            catch (Exception err)
            {
                Error = true;
                ErrorMessage = err.Message;
                return false;

            }
            contents = Crypto.Decrypt(contents);
            string[] pieces = new string[10];
            string[] parts = new string[2];
            pieces = contents.Split(';');
            //var Test = Crypto.Decrypt("cpjk30dwSEMpfsNgIJYRTg==");
            for (int i = 0; i < pieces.Length; i++)
            {
                parts = pieces[i].Split('=');
                if (parts[0].ToLower() == "database")
                {
                    InitialCatalog = parts[1];
                }
                else if (parts[0].ToLower() == "server")
                {
                    DataSource = parts[1];
                }
                else if (parts[0].ToLower() == "integrated security")
                {
                    IntegratedSecurity = Convert.ToBoolean(parts[1]);
                }
                else if (parts[0].ToLower() == "user id")
                {
                    UserID = parts[1];
                }
                else if (parts[0].ToLower() == "password")
                {
                    Password = parts[1];
                }
            }
            return true;
        }
    }
}
