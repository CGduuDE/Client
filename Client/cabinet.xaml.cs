using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для cabinet.xaml
    /// </summary>
    public partial class cabinet : Window
    {
        sql_request sql = new sql_request();
        //cabinet cab = new cabinet();
        public cabinet()
        {
            InitializeComponent();
            New_Email.Text = sql.get_email(File.ReadAllText("config.txt"));
        }

       

        private void Button_Change_Password(object sender, RoutedEventArgs e)
        {
            if (New_Password.Password.ToString() == "")
            {
                Errors.Error = "Пароль не может быть пыстым!";
                Message mess = new Message();
                mess.Show();
                Hide();
            }
            else if (sql.change_password(File.ReadAllText("config.txt"), New_Password.Password.ToString()))
            {
                Errors.Error = "Пароль успешно изменен!";
                Message mess = new Message();
                mess.Show();
                Hide();
                //MessageBox.Show("Пароль успешно изменен!");
            }
            
        }

        private void Button_Change_Email(object sender, RoutedEventArgs e)
        {
            if (New_Email.Text == "")
            {
                Errors.Error = "Email не может быть пыстым!";
                Message mess = new Message();
                mess.Show();
                Hide();
            }
            else if(sql.change_email(File.ReadAllText("config.txt"), New_Email.Text))
            {
                Errors.Error = "Email успешно изменен!";
                Message mess = new Message();
                mess.Show();
                Hide();
                //MessageBox.Show("Email успешно изменен!");
            }
        }
    }
}
