using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace ModernUI
{
    /// <summary>
    /// Interaction logic for Signup.xaml
    /// </summary>
    public partial class Signup : Window
    {


        public Signup()
        {
            InitializeComponent();
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtbox_Password.Password) && txtbox_Password.Password.Length > 0)
                textPassword.Visibility = Visibility.Collapsed;
            else
                textPassword.Visibility = Visibility.Visible;
        }

        private void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtbox_Password.Focus();
        }

        private void textEmail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtbox_Username.Focus();
        }

        private void txtEmail_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtbox_Username.Text) && txtbox_Username.Text.Length > 0)
                textEmail.Visibility = Visibility.Collapsed;
            else
                textEmail.Visibility = Visibility.Visible;
        }


        private void textFirstname_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtbox_Firstname.Focus();
        }

        private void txtFirstname_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtbox_Firstname.Text) && txtbox_Firstname.Text.Length > 0)
                textFirstname.Visibility = Visibility.Collapsed;
            else
                textFirstname.Visibility = Visibility.Visible;
        }

        private void textLastname_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtbox_Lastname.Focus();
        }

        private void txtLastname_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtbox_Lastname.Text) && txtbox_Lastname.Text.Length > 0)
                textLastname.Visibility = Visibility.Collapsed;
            else
                textLastname.Visibility = Visibility.Visible;
        }

        private void button_Signup_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "SERVER=localhost;DATABASE=mydb;UID=root;PASSWORD=admin;";
            if (txtbox_Firstname.Text != "" && txtbox_Lastname.Text != "" && txtbox_Username.Text != "" && txtbox_Password.Password != "")
            {
                if (txtbox_Username.Text.Length > 8 && txtbox_Password.Password.Length > 8)
                {
                    try
                    {

                        MySqlConnection connection = new MySqlConnection(connectionString);

                        string query = "INSERT INTO `mydb`.`users` (`firstname`, `lastname`, `username`, `password`) VALUES('" + txtbox_Firstname.Text + "','" + txtbox_Lastname.Text + "','" + txtbox_Username.Text + "','" + txtbox_Password.Password + "');";

                        MySqlCommand command = new MySqlCommand(query, connection);
                        connection.Open();

                        if (command.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show("Registered Successfully");
                            txtbox_Firstname.Clear();
                            txtbox_Lastname.Clear();
                            txtbox_Username.Clear();
                            txtbox_Password.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Something is wrong! Please check the input carefully");
                        }
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Username or Password must be atleast 8 characters above!");
                }
            }
            else
            {
                MessageBox.Show("Inputs cannot be empty!");
            }

        }

        private void button_Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            this.Close();
            main.Show();
        }

    }
}
