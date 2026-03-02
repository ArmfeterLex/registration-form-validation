using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinFormsApp5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string login = textBox2.Text;
            string password = textBox3.Text;
            string email = textBox4.Text;

            string namePattern = @"^[a-zA-ZА-Яа-яЁё\s]+$"; 
            string loginPattern = @"^[a-zA-Z0-9]+$";
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$"; 
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            if (!Regex.IsMatch(name, namePattern))
            {
                MessageBox.Show("Имя должно содержать только кириллицу.");
                return;
            }

            if (!Regex.IsMatch(login, loginPattern))
            {
                MessageBox.Show("Логин должен содержать только латинские буквы и цифры.");
                return;
            }

            if (!Regex.IsMatch(password, passwordPattern))
            {
                MessageBox.Show("Пароль должен содержать минимум 8 символов, хотя бы одну строчную и одну заглавную букву, и одну цифру.");
                return;
            }

            if (!Regex.IsMatch(email, emailPattern))
            {
                MessageBox.Show("Некорректный email адрес.");
                return;
            }

            if (IsEmailExists(email))
            {
                MessageBox.Show("Аккаунт с таким email уже зарегистрирован.");
                return;
            }

            try
            {
                using (StreamWriter writer = new StreamWriter("log.txt", true)) 
                {
                    writer.WriteLine($"{name};{login};{password};{email}");
                }
                MessageBox.Show("Регистрация успешна!");
                this.Close(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка записи в файл: {ex.Message}");
            }
        }

        private bool IsEmailExists(string email)
        {
            if (!File.Exists("log.txt")) return false;

            using (StreamReader reader = new StreamReader("log.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(';');
                    if (parts.Length >= 4 && parts[3] == email) return true;
                }
            }
            return false;
        }
    }
}