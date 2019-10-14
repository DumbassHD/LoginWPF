using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UsingDatabase
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class login : Page
    {
        public MainWindow mainWindow;
        public login(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            string encrypt = "";
            if (email_login.Text.Length > 0) // перевіряємо чи введено емейл     
            {
                if (password_login.Password.Length > 0) // перевіряємо чи введено пароль       
                {             // ищем в базе данных пользователя с такими данными         
                              /*
                              byte[] salt = new byte[32];
                              System.Security.Cryptography.RNGCryptoServiceProvider.Create().GetBytes(salt);
                              byte[] plainTextBytes = System.Text.UnicodeEncoding.Unicode.GetBytes(password_login.Password);
                              // Append salt to pwd before hashing
                              byte[] combinedBytes = new byte[plainTextBytes.Length + salt.Length];
                              System.Buffer.BlockCopy(plainTextBytes, 0, combinedBytes, 0, plainTextBytes.Length);
                              System.Buffer.BlockCopy(salt, 0, combinedBytes, plainTextBytes.Length, salt.Length);
                              // Create hash for the pwd+salt
                              System.Security.Cryptography.HashAlgorithm hashAlgo = new System.Security.Cryptography.SHA256Managed();
                              byte[] hash = hashAlgo.ComputeHash(combinedBytes);

                              byte[] hashPlusSalt = new byte[hash.Length + salt.Length];
                              System.Buffer.BlockCopy(hash, 0, hashPlusSalt, 0, hash.Length);
                              System.Buffer.BlockCopy(salt, 0, hashPlusSalt, hash.Length, salt.Length);
                              StringBuilder sb = new StringBuilder();
                              foreach (var b in hashPlusSalt)
                              {
                                  sb.Append(b);
                              }
                              string password = sb.ToString();
                              */
                    //DataTable dt_user = mainWindow.Select("SELECT * FROM [dbo].[users] WHERE [email] = '" + email_login.Text + "' AND [password] = '" + protect + "'");
                    DataTable dt_user = mainWindow.Select("SELECT * FROM [dbo].[users] WHERE [email] = '" + email_login.Text + "'");
                    foreach (DataRow row in dt_user.Rows)
                    {
                        string name = row["password"].ToString();
                        encrypt = Unprotect(name);
                    }

                    if (encrypt == password_login.Password)
                    {
                        DataTable db_user = mainWindow.Select("SELECT * FROM [dbo].[users] WHERE [email] = '" + email_login.Text + "'");
                        if (db_user.Rows.Count > 0) // Якщо такий запис існує      
                        {
                            MessageBox.Show("Користувач авторизувався"); // говоримо, що авторизовались         
                        }
                        else MessageBox.Show("Невдалось авторизуватись, перевірте дані"); // виводимо помилку  
                    }
                }
                else MessageBox.Show("Введіть пароль"); // виводимо помилку  
            }
            else MessageBox.Show("Введіть логін"); // виводимо помилку  
        }

        private static string Unprotect(string str)
        {
            byte[] protectedData = Convert.FromBase64String(str);
            byte[] entropy = Encoding.ASCII.GetBytes(Assembly.GetExecutingAssembly().FullName);
            string data = Encoding.ASCII.GetString(ProtectedData.Unprotect(protectedData, entropy, DataProtectionScope.CurrentUser));
            return data;
        }
        public static string Protect(string str)
        {
            byte[] entropy = Encoding.ASCII.GetBytes(Assembly.GetExecutingAssembly().FullName);
            byte[] data = Encoding.ASCII.GetBytes(str);
            string protectedData = Convert.ToBase64String(ProtectedData.Protect(data, entropy, DataProtectionScope.CurrentUser));
            return protectedData;
        }

        private void signup_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenPage(MainWindow.pages.signup);
        }
    }
}
