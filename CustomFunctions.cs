//Ver. 2.1.5
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using unirest_net.http;
using Newtonsoft.Json.Linq;

namespace SoutiesSandbox
{
    class CustomFunctions
    {
        //Creating a custom database connection
        public string CreateRemoteSQLConnection(string ServerAddress, string Port, string User, string Password, string DatabaseName)//Method to create dynamic external sql connection string
        {
            return "server=" + ServerAddress + ";port=" + Port + ";user id=" + User + ";Password=" + Password + ";database="
                + DatabaseName + "; pooling = false; convert zero datetime=True";
        }
        public (string[] StringArray, int ItemCount) ReadFromFile(string FileName)//Textfile method
        {
            string[] output = File.ReadAllLines(Application.StartupPath + FileName);
            int count = File.ReadAllLines(Application.StartupPath + FileName).Length;
            return (output, count);
        }
        public bool EmailVerification(string email)//Email address verification
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public PictureBox LoadImage(string ImageName)
        {
            PictureBox temp = new PictureBox();
            temp.ImageLocation = Application.StartupPath + "\\" + ImageName + ".png";
            return temp;
        }
        public void AppendToFile(string LineToAppend, string FileName)//Textfile method
        {
            System.IO.File.AppendAllText(Application.StartupPath + FileName, LineToAppend);
        }
		void AppendToFile(string LineToAppend, string FileName)//Textfile method for c# console application
		{
			File.AppendAllText(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + FileName, LineToAppend);
		}
        public void WriteToFile(string[] ArrayToWrite, string FileName)//Textfile method
        {
            System.IO.File.WriteAllLines(Application.StartupPath + FileName, ArrayToWrite);
        }
        public bool LuhnAlgorithm(string Number)//Method used to verify ID and Banking numbers
        {
            int nSum = 0; 
            bool isSecond = false;
            for (int i = Number.lenght() - 1; i >= 0; i--)
            {
                int d = Number[i] - '0';
                if (isSecond == true)
                {
                    d = d * 2;
                }
                nSum += d / 10;
                nSum += d % 10;
                isSecond = !isSecond;
            }
            return (nSum % 10 == 0);
        }
        public bool StudentNumberVerification(string Number)//Method used to verify a nwu student number
        {
            int iTemp;
            int cSum = 0, c = 8;
            if (Number.Length != 8)
            {
                return false;
            }
            else
            {
                if (!(int.TryParse(Number, out iTemp)))
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < 8; i++)
                    {
                        cSum += (Convert.ToInt32(Number[i].ToString()) * c);
                        c--;
                    }
                    if (cSum % 11 == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        public int GetCountSQL(string Command, string DatabaseConnection)//Returns row count from and sql statement
        {
            int temp = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseConnection))
                {
                    conn.Open();
                    MySqlCommand sqlCommand = new MySqlCommand(Command, conn);
                    temp = Convert.ToInt32(sqlCommand.ExecuteScalar());
                }
            }
            catch (MySqlException ex)
            {
                string errorMessages = "Index #" + "1" + "\n" +
                        "Message: " + ex.Message + "\n" +
                        "Stack Trace: " + ex.StackTrace + "\n" +
                        "Source: " + ex.Source + "\n" +
                        "Target Site: " + ex.TargetSite + "\n";
                MessageBox.Show(errorMessages, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return temp;

        }
        public string GetSingleStringSQL(string Command, string DatabaseConnection)//Returns an single value form an sql statement
        {
            string temp = "";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseConnection))
                {
                    conn.Open();
                    MySqlCommand sqlCommand = new MySqlCommand(Command, conn);
                    temp = sqlCommand.ExecuteScalar().ToString();
                }
            }
            catch (MySqlException ex)
            {
                string errorMessages = "Index #" + "1" + "\n" +
                        "Message: " + ex.Message + "\n" +
                        "Stack Trace: " + ex.StackTrace + "\n" +
                        "Source: " + ex.Source + "\n" +
                        "Target Site: " + ex.TargetSite + "\n";
                MessageBox.Show(errorMessages, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return temp;
        }
        public int GetSingleIntegerSQL(string Command, string DatabaseConnection)//Returns an single value form an sql statement
        {
            int temp = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseConnection))
                {
                    conn.Open();
                    MySqlCommand sqlCommand = new MySqlCommand(Command, conn);
                    temp = (int)sqlCommand.ExecuteScalar();
                }
            }
            catch (MySqlException ex)
            {
                string errorMessages = "Index #" + "1" + "\n" +
                        "Message: " + ex.Message + "\n" +
                        "Stack Trace: " + ex.StackTrace + "\n" +
                        "Source: " + ex.Source + "\n" +
                        "Target Site: " + ex.TargetSite + "\n";
                MessageBox.Show(errorMessages, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return temp;
        }
        public int GetSingleLongIntegerSQL(string Command, string DatabaseConnection)//Returns an single value form an sql statement
        {
            int temp = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseConnection))
                {
                    conn.Open();
                    MySqlCommand sqlCommand = new MySqlCommand(Command, conn);
                    temp = (int)(long)sqlCommand.ExecuteScalar();
                }
            }
            catch (MySqlException ex)
            {
                string errorMessages = "Index #" + "1" + "\n" +
                         "Message: " + ex.Message + "\n" +
                         "Stack Trace: " + ex.StackTrace + "\n" +
                         "Source: " + ex.Source + "\n" +
                         "Target Site: " + ex.TargetSite + "\n";
                MessageBox.Show(errorMessages, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return temp;
        }
        string[][] GetStringArraySQL(string Command, string DatabaseConnection, int AmountOfRows, int AmountOfColumns)//Returns an 2d array of sql data
{
    string[][] output = new string[AmountOfRows][];//latitude,longitude
    try
    {
        using (MySqlConnection conn = new MySqlConnection(DatabaseConnection))
        {
            int i = 0;
            conn.Open();
            MySqlCommand sqlCommand = new MySqlCommand(Command, conn);
            MySqlDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                for(int c = 0; c < AmountOfColumns; c++)//Could also use dataReader.FieldCount
                {
                    output[i][c] = dataReader.GetValue(c).ToString();
                }
                i++;
            }
        }
    }
    catch (MySqlException ex)
    {
        string errorMessages = "Index #" + "1" + "\n" +
                "Message: " + ex.Message + "\n" +
                "Stack Trace: " + ex.StackTrace + "\n" +
                "Source: " + ex.Source + "\n" +
                "Target Site: " + ex.TargetSite + "\n";
        Console.WriteLine(errorMessages);
        AppendToFile(errorMessages, "log.txt");
    }
    return output;
}
        public void NonQuerySQL(string Command, string DatabaseConnection)//Used for executing a non query sql statement
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseConnection))
                {
                    conn.Open();
                    MySqlCommand sqlCommand = new MySqlCommand(Command, conn);
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                string errorMessages = "Index #" + "1" + "\n" +
                        "Message: " + ex.Message + "\n" +
                        "Stack Trace: " + ex.StackTrace + "\n" +
                        "Source: " + ex.Source + "\n" +
                        "Target Site: " + ex.TargetSite + "\n";
                MessageBox.Show(errorMessages,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        public string RandomPasswordGenerator(int PasswordLength)//In the name
        {
            string PasswordChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            Random random = new Random();
            char[] chars = new char[PasswordLength];
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = PasswordChars[random.Next(0, PasswordChars.Length)];
            }
            return new string(chars);
        }
        public string UUIDGenerator()//Generates a unique 11 digit id
        {
            StringBuilder builder = new StringBuilder();
            Enumerable
               .Range(65, 26)
                .Select(e => ((char)e).ToString())
                .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                .OrderBy(e => Guid.NewGuid())
                .Take(11)
                .ToList().ForEach(e => builder.Append(e));
            return builder.ToString();
        }
        public string EncryptPlainTextToCipherText(string PlainText, string SecurityKey)//In the name
        {
            // Getting the bytes of Input String.
            byte[] toEncryptedArray = UTF8Encoding.UTF8.GetBytes(PlainText);
            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();
            //Gettting the bytes from the Security Key and Passing it to compute the Corresponding Hash Value.
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecurityKey));
            //De-allocatinng the memory after doing the Job.
            objMD5CryptoService.Clear();
            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();
            //Assigning the Security key to the TripleDES Service Provider.
            objTripleDESCryptoService.Key = securityKeyArray;
            //Mode of the Crypto service is Electronic Code Book.
            objTripleDESCryptoService.Mode = CipherMode.ECB;
            //Padding Mode is PKCS7 if there is any extra byte is added.
            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;
            var objCrytpoTransform = objTripleDESCryptoService.CreateEncryptor();
            //Transform the bytes array to resultArray
            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptedArray, 0, toEncryptedArray.Length);
            objTripleDESCryptoService.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public string DecryptCipherTextToPlainText(string CipherText, string SecurityKey)//In the name
        {
            byte[] toEncryptArray = Convert.FromBase64String(CipherText);
            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();
            //Gettting the bytes from the Security Key and Passing it to compute the Corresponding Hash Value.
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecurityKey));
            objMD5CryptoService.Clear();
            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();
            //Assigning the Security key to the TripleDES Service Provider.
            objTripleDESCryptoService.Key = securityKeyArray;
            //Mode of the Crypto service is Electronic Code Book.
            objTripleDESCryptoService.Mode = CipherMode.ECB;
            //Padding Mode is PKCS7 if there is any extra byte is added.
            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;
            var objCrytpoTransform = objTripleDESCryptoService.CreateDecryptor();
            //Transform the bytes array to resultArray
            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            objTripleDESCryptoService.Clear();
            //Convert and return the decrypted data/byte into string format.
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
		HttpResponse<string> UnirestLocationRequest(string city, string state)
{
    HttpResponse<string> response = null;
    try
    {
        response = Unirest.get("https://geocode.maps.co/search?q=" + city + "," + state).asJson<string>();

    }
    catch (HttpRequestException ex)
    {
        string errorMessages = "Index #" + "1" + "\n" +
                "Message: " + ex.Message + "\n" +
                "Stack Trace: " + ex.StackTrace + "\n" +
                "Source: " + ex.Source + "\n" +
                "Target Site: " + ex.TargetSite + "\n";
        Console.WriteLine(errorMessages);
        AppendToFile(errorMessages, "log.txt");
    }
    return response;
}
    }
    public static class DecimalExtension
    {
        private static readonly Dictionary<string, CultureInfo> ISOCurrenciesToACultureMap =
            CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Select(c => new { c, new RegionInfo(c.LCID).ISOCurrencySymbol })
                .GroupBy(x => x.ISOCurrencySymbol)
                .ToDictionary(g => g.Key, g => g.First().c, StringComparer.OrdinalIgnoreCase);

        public static string FormatCurrency(this decimal amount, string currencyCode)
        {
            CultureInfo culture;
            if (ISOCurrenciesToACultureMap.TryGetValue(currencyCode, out culture))
                return string.Format(culture, "{0:C}", amount);
            return amount.ToString("0.00");
        }
    }
}
