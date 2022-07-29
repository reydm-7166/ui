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
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        ///////////////////////////////////////////////
        // THIS IS THE PARENT CONNECTION CLASS.
        // I SEPERATED IT TO AVOID REPITITION
        ///////////////////////////////////////////////
        public static class mainConnectionClass
        {
            public static string staffID = "";
            public static string staffFirstName = "";
            public static string staffLastName = "";
            public static string role = "";
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
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

        private void button_Signin_Click(object sender, RoutedEventArgs e)
        {
            ///////////////////////////////////////////////
            // THIS CHECKS THE FIELD OF USERNAME AND
            // PASSWORD IF NOT EMPTY. 
            ///////////////////////////////////////////////

            string connectionString = "SERVER=localhost;DATABASE=mydb;UID=root;PASSWORD=admin;";
            MySqlConnection conn = new MySqlConnection(connectionString);

            string query = "SELECT * FROM users WHERE username=@username && password=@password";

            MySqlCommand command = new MySqlCommand(query, conn);

                command.Parameters.AddWithValue("@username", txtbox_Username.Text);
                command.Parameters.AddWithValue("@password", txtbox_Password.Password);

                conn.Open();
                if (!string.IsNullOrEmpty(txtbox_Username.Text) && !string.IsNullOrEmpty(txtbox_Password.Password))
                {

                    ///////////////////////////////////////////////
                    // This initializes the connection to database. 
                    // I inherited the mainconnection class to child class and used it
                    // 
                    try
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    if(reader[5].ToString() != "")
                                    {
                                        HomeWindow HomeWindow = new HomeWindow();
                                        txtbox_Username.Clear();
                                        txtbox_Password.Clear();

                                        StaffHomeWindow.userData.ID = reader[0].ToString();
                                        mainConnectionClass.staffID = reader[0].ToString();
                                        mainConnectionClass.staffFirstName = reader[1].ToString();
                                        mainConnectionClass.staffLastName = reader[2].ToString();
                                        mainConnectionClass.role = reader[5].ToString();


                                        HomeWindow.txtbox_ReportedByStaffID.Text = mainConnectionClass.staffID;
                                        HomeWindow.txtbox_ReportedByStaffName.Text = mainConnectionClass.staffFirstName + " " + mainConnectionClass.staffLastName;
                                        HomeWindow.text_LoggedInAs.Text += "Supervisor";
                                        HomeWindow.text_LoggedInAs.Text += ", " + mainConnectionClass.staffFirstName;

                                        HomeWindow.txtbox_Role.Text = mainConnectionClass.role == "1" ? "Supervisor" : "Staff User";

                                        /// Creates the homestaffwindow object and shows it on else block
                                        StaffHomeWindow staffHome = new StaffHomeWindow();
                                        if (mainConnectionClass.role == "1")
                                        {
                                            HomeWindow.Show();
                                        }
                                        else
                                        {
                                            staffHome.text_LoggedInAs.Text += "Support Staff";
                                            staffHome.text_LoggedInAs.Text += ", " + mainConnectionClass.staffFirstName;

                                            staffHome.Show();
                                        }
                                        this.Close();
                                    }
                                    else
                                    {
                                    MessageBox.Show("CONTACT ADMIN TO ASSIGN ROLE");
                                }
                                }
                                conn.Close();
                            }
                            else
                            {
                                conn.Close();
                                MessageBox.Show("Wrong Credentials");
                            }
                            reader.Close();
                        }

                    }
                    catch (Exception)
                    {
                        
                    }
                }
        }


        private void txtEmail_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtbox_Username.Text) && txtbox_Username.Text.Length > 0)
                textEmail.Visibility = Visibility.Collapsed;
            else
                textEmail.Visibility = Visibility.Visible;
        }

        private void textEmail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtbox_Username.Focus();
        }

        private void button_SignUp_Click(object sender, RoutedEventArgs e)
        {
            Signup signup = new Signup();
            this.Close();
            signup.Show();

        }

    }

}