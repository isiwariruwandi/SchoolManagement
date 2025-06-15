using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchoolManagement
{
    public partial class Score : Form
    {
        public Score()
        {
            InitializeComponent();
        }

        private void Score_Load(object sender, EventArgs e)
        {
            showTable();
            showTable1();

            //connection
            string cs = @"Data Source=DESKTOP-LKG8OU2;
            Initial Catalog=School;Integrated Security=True";
            SqlConnection conn = new SqlConnection(cs);
            conn.Open();

            //command
            string sql = "SELECT SubjectName FROM Subregis";
            SqlCommand com = new SqlCommand(sql, conn);

            //access data
            SqlDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                this.ComboSub.Items.Add(dr.GetValue(0));
            }

            //disconnect
            conn.Close();
        }

        private void dataGridView_stu_Click(object sender, EventArgs e)
        {

        }

        public DataTable getStudentlist()
        {
            try
            {
                // Ensure the connection is properly managed
                string cs = @"Data Source=DESKTOP-LKG8OU2; Initial catalog=School;Integrated Security=True";
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    conn.Open();
                    SqlCommand com = new SqlCommand("SELECT StdID, FirstName, LastName FROM Stdregis", conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(com);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null; // Return null if an error occurs
            }
        }

        public void showTable()
        {
            try
            {
                // Set the DataSource for the DataGridView
                dataGridView_stu.DataSource = getStudentlist();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Display Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public DataTable getScorelist()
        {
            try
            {
                // Ensure the connection is properly managed
                string cs = @"Data Source=DESKTOP-LKG8OU2; Initial catalog=School;Integrated Security=True";
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    conn.Open();
                    SqlCommand com = new SqlCommand("SELECT * FROM Scoreinfo", conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(com);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null; // Return null if an error occurs
            }
        }

        public void showTable1()
        {
            try
            {
                // Set the DataSource for the DataGridView
                dataGridView_scr.DataSource = getScorelist();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Display Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            this.STID.Clear();
            this.ComboSub.SelectedIndex = -1;
            this.ScrTB.Clear();
            this.DescriptionTB.Clear();
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            // Create a connection with SQL Server
            string cs = @"Data Source=DESKTOP-LKG8OU2; Initial catalog=School;Integrated Security=True";
            SqlConnection conn = new SqlConnection(cs);

            try
            {
                conn.Open();

                // Verify that all required fields are filled
                if (!verify())
                {
                    MessageBox.Show("Please fill all required fields!", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Stop further execution
                }

                // Define the SQL statement
                string sql = "INSERT INTO Scoreinfo (StdID, SubjectName, Score, Description) " +
                             "VALUES (@StdID, @SubjectName, @Score, @Description)";
                SqlCommand com = new SqlCommand(sql, conn);

                // Add parameters
                com.Parameters.AddWithValue("@StdID", STID.Text);
                com.Parameters.AddWithValue("@SubjectName", ComboSub.Text);
                com.Parameters.AddWithValue("@Score", ScrTB.Text);
                com.Parameters.AddWithValue("@Description", DescriptionTB.Text);

                // Execute the command
                int ret = com.ExecuteNonQuery();
                MessageBox.Show("New Score Added : " + ret, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                showTable1();
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., SQL errors)
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Always close the connection
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private bool verify()
        {
            if ((STID.Text == "") || (ComboSub.Text == "") || (ScrTB.Text == "") || (DescriptionTB.Text == ""))
            {
                return false;
            }
            return true;
        }

        private void dataGridView_scr_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
