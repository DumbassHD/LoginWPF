using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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
using System.Security.Cryptography;

namespace UsingDatabase
{
    /// <summary>
    /// Interaction logic for signup.xaml
    /// </summary>
    public partial class signup : Page
    {
        public MainWindow mainWindow;
        public signup(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
        }

        private void signup_Click(object sender, RoutedEventArgs e)
        {
            if (email_signup.Text.Length > 0) // провіряємо логін
            {
                if (password_signup.Password.Length > 0) // провіряємо пароль
                {
                    if (confpassword_signup.Password.Length > 0) // провіряємо другий пароль
                    {

                        string[] dataLogin = email_signup.Text.Split('@'); // ділимо строку на дві частини
                        if (dataLogin.Length == 2) // провіряємо чи є у нас дві частини
                        {
                            string[] data2Login = dataLogin[1].Split('.'); // ділимо другу частину ще на дві частини
                            if (data2Login.Length == 2)
                            {

                            }
                            else MessageBox.Show("Вкажіть логін в форматі х@x.x");
                        }
                        else MessageBox.Show("Вкажіть логін в форматі х@x.x");

                        if (password_signup.Password.Length >= 6)
                        {
                            bool en = true; // англійска розкладка
                            bool symbol = false; // символ
                            bool number = false; // цифра

                            for (int i = 0; i < password_signup.Password.Length; i++) // перебираємо символи
                            {
                                if (password_signup.Password[i] >= 'А' && password_signup.Password[i] <= 'Я') en = false; // чи російська розкладка
                                if (password_signup.Password[i] >= '0' && password_signup.Password[i] <= '9') number = true; // чи є цифри
                                if (password_signup.Password[i] == '_' || password_signup.Password[i] == '-' || password_signup.Password[i] == '!') symbol = true; // чи є символ
                            }

                            if (!en)
                                MessageBox.Show("Доступна тільки англійська розкладка"); // виводимо повідомлення
                            else if (symbol)
                                MessageBox.Show("Добавте один із наступних символів: _ - !"); // виводимо повідомлення
                            else if (!number)
                                MessageBox.Show("Добавте хотя-б одну цифру"); // виводимо повідомлення
                            if (en && symbol && number) // провіряємо співпадіння
                            {
                                if (password_signup.Password == confpassword_signup.Password) // перевірка на співпадіння паролів
                                {
                                    /*
                                     * byte[] salt = new byte[32];
                                    System.Security.Cryptography.RNGCryptoServiceProvider.Create().GetBytes(salt);
                                    byte[] plainTextBytes = System.Text.UnicodeEncoding.Unicode.GetBytes(password_signup.Password);
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
                                    string password = Convert.ToString(sb);
                                    */
                                    string pass = Protect(password_signup.Password);
                                    DataTable dt_user = mainWindow.Select("INSERT INTO [dbo].[users] VALUES ('" + email_signup.Text + "', '" + pass + "')");
                                    MessageBox.Show("Користувач зареєстрований");
                                }
                                else MessageBox.Show("Паролі не співпадають");
                            }
                        }
                        else MessageBox.Show("Пароль дуже короткий, мінімум 6 символів!");
                        
                    }
                    else MessageBox.Show("Повторіть пароль");
                }
                else MessageBox.Show("Вкажіть пароль");
            }
            else MessageBox.Show("Вкажіть Email");
        }
        
        public static string Protect(string str)
        {
            byte[] entropy = Encoding.ASCII.GetBytes(Assembly.GetExecutingAssembly().FullName);
            byte[] data = Encoding.ASCII.GetBytes(str);
            string protectedData = Convert.ToBase64String(ProtectedData.Protect(data, entropy, DataProtectionScope.CurrentUser));
            return protectedData;
        }
        
        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenPage(MainWindow.pages.login);
        }
    }
}
