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
using System.Data;

namespace ModernUI
{
    /// <summary>
    /// Interaction logic for StaffHomeWindow.xaml
    /// </summary>
    public partial class StaffHomeWindow : Window
    {
        public StaffHomeWindow()
        {
            InitializeComponent();
            userData.Name = MainWindow.mainConnectionClass.staffFirstName + " " + MainWindow.mainConnectionClass.staffLastName;
            dataGrid();
        }

        public static class userData
        {
            public static string ID  { get; set; }
            public static string Name { get; set; }
            public static string ReportedID { get; set; }
            public static string ReportedName { get; set; }
            public static string TicketID { get; set; }
        }

        /// for buttons. ticket problem and steps to resolve
        /// 
        private void text_TicketProblem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtbox_TicketProblem.Focus();
        }

        private void txtbox_TicketProblem_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (!string.IsNullOrEmpty(txtbox_TicketProblem.Text) && txtbox_TicketProblem.Text.Length > 0)
                text_TicketProblem.Visibility = Visibility.Collapsed;
            else
                text_TicketProblem.Visibility = Visibility.Visible;
        }

        private void text_TicketSolution_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtbox_TicketSolution.Focus();
        }

        private void txtbox_TicketSolution_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (!string.IsNullOrEmpty(txtbox_TicketSolution.Text) && txtbox_TicketSolution.Text.Length > 0)
                text_TicketSolution.Visibility = Visibility.Collapsed;
            else
                text_TicketSolution.Visibility = Visibility.Visible;
        }

        /// for buttons. ticket problem and steps to resolve
        void dataGrid()
        {

            string connectionString = "SERVER=localhost;DATABASE=mydb;UID=root;PASSWORD=admin;";

            MySqlConnection connection = new MySqlConnection(connectionString);

            string query = "SELECT number As 'Ticket Number', issue_title As 'Ticket Category', problem As 'Ticket Problem', status As 'Ticket Status', date_created As 'Date Created' FROM tickets WHERE assigned_to=@staffName";

            MySqlCommand cmd = new MySqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@staffName", userData.Name);

            connection.Open();


            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            connection.Close();

            dtGrid.DataContext = dt;
        }

        private void button_ViewTickets_Click(object sender, RoutedEventArgs e)
        {
            grid_ViewTickets.Visibility = Visibility.Visible;

            titletext_Create.Visibility = Visibility.Collapsed;
            titletext_View.Visibility = Visibility.Visible;

            button_WorkTicket.FontWeight = FontWeights.SemiBold;

            grid_CreateTicket.Visibility = Visibility.Collapsed;

            dataGrid();
        }

        private void button_WorkTicket_Click(object sender, RoutedEventArgs e)
        {

        }

        void workGrid()
        {
            grid_ViewTickets.Visibility = Visibility.Collapsed;
            grid_CreateTicket.Visibility = Visibility.Visible;

            titletext_Create.Visibility = Visibility.Visible;
            titletext_View.Visibility = Visibility.Collapsed;
        }

        private void dtGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid row = (DataGrid)sender;
            DataRowView row_select = row.SelectedItem as DataRowView;

            if (row_select != null)
            {
                workGrid();

                string connectionString = "SERVER=localhost;DATABASE=mydb;UID=root;PASSWORD=admin;";

                MySqlConnection connection = new MySqlConnection(connectionString);

                MySqlCommand cmd = new MySqlCommand("select * from tickets WHERE number = @ticketID", connection);

                cmd.Parameters.AddWithValue("@ticketID", row_select["Ticket Number"].ToString());
                connection.Open();

                try
                {
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            combobox_TicketCategory.Text = row_select["Ticket Category"].ToString();

                            txtbox_TicketProblem.Text = row_select["Ticket Problem"].ToString();
                            txtbox_TicketDetails.Text = reader.GetString("details").ToString();

                            txtbox_ticketID.Text = reader.GetInt32("number").ToString();
                            txtbox_AssignedTo.Text = reader.GetString("assigned_to");

                            userData.ReportedID = reader.GetString("reported_by_id");
                            userData.ReportedName = reader.GetString("reported_by_name");
                            userData.TicketID = reader.GetString("id");
                            combobox_TicketStatus.Text = "RESOLVED";

                        }
                        connection.Close();
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                
            }
        }

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
        }

        private void button_SignOut_Click(object sender, RoutedEventArgs e)
        {
            userData.ID = "";
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void button_Submit_Click(object sender, RoutedEventArgs e)
        {
            if (combobox_TicketCategory.Text != "Ticket Category"
               && txtbox_TicketSolution.Text != "" && txtbox_TicketSolution.Text.Length > 20)
            {
                try
                {
                    string connectionString = "SERVER=localhost;DATABASE=mydb;UID=root;PASSWORD=admin;";
                    DateTime aDate = DateTime.Now;
                    MySqlConnection conn = new MySqlConnection(connectionString);

                    string query = "INSERT INTO mydb.resolved(user_id, number, issue_title, problem, details, solution, reported_by_id, reported_by_name, resolved_by_id, resolved_by_name, assigned_to, status, date_created, date_updated)" +
                        " VALUES('" + int.Parse(userData.ID) + "','" + txtbox_ticketID.Text + "','" + combobox_TicketCategory.Text + "','" + txtbox_TicketProblem.Text + "','" + txtbox_TicketDetails.Text 
                        + "','" + txtbox_TicketSolution.Text + "','" + userData.ReportedID + "','" + userData.ReportedName + "','" + userData.ID + "','" + userData.Name + "','" + userData.Name + "','" + combobox_TicketStatus.Text + "','" + aDate.ToString("yyyy-MM-dd HH:mm:ss") + "','" + aDate.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                    MySqlCommand command = new MySqlCommand(query, conn);
                    conn.Open();

                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Ticket Submitted Successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        conn.Close();
                        delete();

                        dataGrid();
                        grid_ViewTickets.Visibility = Visibility.Visible;
                        grid_CreateTicket.Visibility = Visibility.Collapsed;

                        titletext_View.Visibility = Visibility.Visible;
                        titletext_Create.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        MessageBox.Show("Something went wrong! Please check the input carefully", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("Fill in approriate input", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void delete()
        {
            string connectionString = "SERVER=localhost;DATABASE=mydb;UID=root;PASSWORD=admin;";

            MySqlConnection conn = new MySqlConnection(connectionString);

            string query = "DELETE FROM tickets WHERE id = '"+ userData.TicketID +"'";
            MySqlCommand command = new MySqlCommand(query, conn);
           
            conn.Open();


            if (command.ExecuteNonQuery() == 1)
            {
                conn.Close();
            }
        }
    }
}
