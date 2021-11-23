using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows;
using System.IO;
namespace Client
{
    class sql_request
    {
        MySqlConnection mysqlConn = new MySqlConnection("server=IP;uid=root;port=3306;pwd=pwd;database=admin;");

        string sql = "";

        public string get_command()
        {
            string id = File.ReadAllText("id_pc.txt");
            string otv = "";
            try
            {
                mysqlConn.Open();
                sql = "select command from commands  where id = '" + id + "'"; //select balance from users  where login = 'test'

                MySqlCommand command = new MySqlCommand(sql, mysqlConn);
                otv = command.ExecuteScalar().ToString();
                mysqlConn.Close();
                return otv;
            }

            catch
            {

                //MessageBox.Show("Ошибка! get balance");

            }

            mysqlConn.Close();
            return otv;
        }

        public bool set_command()
        {
            string id = File.ReadAllText("id_pc.txt");
            string my_command = "";
            try
            {
                mysqlConn.Open();

                sql = "update commands set command = '" + my_command + "' where id = '" + id + "'";
                MySqlCommand command = new MySqlCommand(sql, mysqlConn);
                command = new MySqlCommand(sql, mysqlConn);
                command.ExecuteNonQuery();
                mysqlConn.Close();
                return true;
                

            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
            mysqlConn.Close();
            return false;
        }
        public bool change_password(string login, string password)
        {
           
                try
                {
                    mysqlConn.Open();

                    sql = "update users set password = '" + password + "' where login = '" + login + "'";
                    MySqlCommand command = new MySqlCommand(sql, mysqlConn);
                    command = new MySqlCommand(sql, mysqlConn);
                    command.ExecuteNonQuery();

                mysqlConn.Close();
                return true;

                }
                catch
                {
                    MessageBox.Show("Ошибка!");
                }
            
            mysqlConn.Close();
            return false;
        }

        public bool change_email(string login,string email)
        {
            if (!check_email_existence(email))
            {
                try
                {
                    mysqlConn.Open();

                    sql = "update users set email = '" + email + "' where login = '" + login + "'";
                    MySqlCommand command = new MySqlCommand(sql, mysqlConn);
                    command = new MySqlCommand(sql, mysqlConn);
                    command.ExecuteNonQuery();

                    mysqlConn.Close();
                    return true;
                    //MessageBox.Show("Пароль успешно изменен!");

                }
                catch
                {
                    //MessageBox.Show("Ошибка!");
                }
            }
            else
            {
                //MessageBox.Show("Такой Email уже кто то использует!");
                Errors.Error = "Такой Email уже кто то использует!";
                Message mess = new Message();
                mess.Show();
            }
            

            mysqlConn.Close();
            return false;
        }

        private bool check_email_existence(string email)
        {

            bool exists = false;

            try
            {
                mysqlConn.Open();

                sql = "select count(1) from users where email = '" + email + "'"; //select count(1) from users where login = 'test';

                MySqlCommand command = new MySqlCommand(sql, mysqlConn);

                string otv = command.ExecuteScalar().ToString();

                if (Convert.ToInt32(otv) == 0)
                {
                    exists = false; // пользователя не существует
                }
                else
                {
                    exists = true;
                }

            }

            catch
            {

                MessageBox.Show("Ошибка!");

            }

            mysqlConn.Close();
            return exists;

        }

        private bool check_user_existence(string login)
        {

            bool exists = false;

            try
            {
                mysqlConn.Open();

                sql = "select count(1) from users where login = '" + login + "'"; //select count(1) from users where login = 'test';

                MySqlCommand command = new MySqlCommand(sql, mysqlConn);

                string otv = command.ExecuteScalar().ToString();

                if (Convert.ToInt32(otv) == 0)
                {
                    exists = false; // пользователя не существует
                }
                else
                {
                    exists = true;
                }
                
            }

            catch
            {

                MessageBox.Show("Ошибка!");

            }

            mysqlConn.Close();
            return exists;

        }

        public string get_email(string login)
        {
            string email = "";

            try
            {
                mysqlConn.Open();
                sql = "select email from users  where login = '" + login + "'"; //select balance from users  where login = 'test'

                MySqlCommand command = new MySqlCommand(sql, mysqlConn);

                email = command.ExecuteScalar().ToString();
                mysqlConn.Close();
                return email;
            }

            catch
            {

                //MessageBox.Show("Ошибка! get balance");

            }

            mysqlConn.Close();
            return email;
        }

        public string get_balance(string login)
        {
            string balance = "";

           try
           {
                mysqlConn.Open();
                sql = "select balance from users  where login = '"+login+"'"; //select balance from users  where login = 'test'

                MySqlCommand command = new MySqlCommand(sql, mysqlConn);

                balance = command.ExecuteScalar().ToString();
                mysqlConn.Close();
                return balance;
           }

           catch
           {

               //MessageBox.Show("Ошибка! get balance");

           }

            mysqlConn.Close();
            return balance;
        }

        public string get_logo(string name)
        {
            

            try
            {
                mysqlConn.Open();
                sql = "select path from logo  where name = '" + name + "'"; //select balance from users  where login = 'test'

                MySqlCommand command = new MySqlCommand(sql, mysqlConn);

                name = command.ExecuteScalar().ToString();
                mysqlConn.Close();
                return name;
            }

            catch
            {

                //MessageBox.Show("Ошибка! get balance");

            }

            mysqlConn.Close();
            return name;
        }
        public void set_balance(string login,double balance_now)
        {
            try
            {
              
                mysqlConn.Open();


                MySqlCommand command = new MySqlCommand(sql, mysqlConn);

               
            
                sql = "update users set balance = '" +Convert.ToString(balance_now)+ "' where login = '" + login + "'";
                command = new MySqlCommand(sql, mysqlConn);
                command.ExecuteNonQuery();

            }
            catch
            {
                //MessageBox.Show("Ошбика set blanace!");
            }

            mysqlConn.Close();
        }
        public bool loginIN(string login,string password)
        {

            if (check_user_existence(login))
            {
                try
                {
                    mysqlConn.Open();

                    sql = "select password from users where login = '" + login + "'"; //select count(1) from users where login = 'test';

                    MySqlCommand command = new MySqlCommand(sql, mysqlConn);

                    string otv = command.ExecuteScalar().ToString();
                    
                    if(password == otv)
                    {
                        mysqlConn.Close();
                        return true;
                    }
                }

                catch
                {

                    MessageBox.Show("Ошибка!");

                }

                mysqlConn.Close();
            }
            
            return false;
        }
        public string get_path(string name) {

            string path = "";
            try
            {
                mysqlConn.Open();

                sql = "select path from paths where name = '" + name + "'"; //select count(1) from users where login = 'test';

                MySqlCommand command = new MySqlCommand(sql, mysqlConn);

                path = command.ExecuteScalar().ToString();
            }

            catch
            {

               // MessageBox.Show("Ошибка!");

            }

            mysqlConn.Close();
            return path;
        }

        public int get_price()
        {
            string price = "";
            try
            {
                mysqlConn.Open();

                sql = "select price from prices"; //select count(1) from users where login = 'test';

                MySqlCommand command = new MySqlCommand(sql, mysqlConn);

                price = command.ExecuteScalar().ToString();
            }

            catch
            {

                // MessageBox.Show("Ошибка!");

            }

            mysqlConn.Close();
            return Convert.ToInt32(price);
        }

    }
}
