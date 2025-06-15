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
    public partial class ManageScore : Form
    {
        public ManageScore()
        {
            InitializeComponent();
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {
                // Set the DataSource for the DataGridView
                dataGridView1.DataSource = searchScore(txtsearch.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Display Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Method to fetch score list from the database
        public DataTable searchScore(string searchdata)
        {
            try
            {
                // Ensure the connection is properly managed
                string cs = @"Data Source=DESKTOP-LKG8OU2; Initial catalog=School;Integrated Security=True";
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    conn.Open();
                    SqlCommand com = new SqlCommand("SELECT * FROM Scoreinfo WHERE CONCAT(StdID, SubjectName, Score, Description) LIKE '%" + searchdata + "%'", conn);
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

        private void ManageScore_Load(object sender, EventArgs e)
        {
            showTable();

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

        public void showTable()
        {
            try
            {
                // Set the DataSource for the DataGridView
                dataGridView1.DataSource = getScorelist();
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

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            STID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            ComboSub.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            ScrTB.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            DescriptionTB.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            this.STID.Clear();
            this.ComboSub.SelectedIndex = -1;
            this.ScrTB.Clear();
            this.DescriptionTB.Clear();
        }

        private void btnupdate_Click(object sender, EventArgs e)
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
                string sql = "UPDATE Scoreinfo SET Score=@Score, Description=@Description WHERE StdID=@StdID AND SubjectName=@SubjectName";
                SqlCommand com = new SqlCommand(sql, conn);

                // Add parameters
                com.Parameters.AddWithValue("@StdID", STID.Text);
                com.Parameters.AddWithValue("@SubjectName", ComboSub.Text);
                com.Parameters.AddWithValue("@Score", ScrTB.Text);
                com.Parameters.AddWithValue("@Description", DescriptionTB.Text);

                // Execute the command
                int ret = com.ExecuteNonQuery();
                MessageBox.Show("Score Details Updated : " + ret, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                showTable();
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

        private void btndelete_Click(object sender, EventArgs e)
        {
            // Create a Connection with SQL Server
            string cs = @"Data Source=DESKTOP-LKG8OU2;
                 Initial catalog=School;Integrated Security=True";
            SqlConnection conn = new SqlConnection(cs);
            conn.Open();

            //Define a command with SQL Statment
            string sql = "DELETE FROM Scoreinfo WHERE StdID=@StdID;";
            SqlCommand com = new SqlCommand(sql, conn);
            com.Parameters.AddWithValue("@StdID", STID.Text);
            com.Parameters.AddWithValue("@SubjectName", ComboSub.Text);
            com.Parameters.AddWithValue("@Score", ScrTB.Text);
            com.Parameters.AddWithValue("@Description", DescriptionTB.Text);


            //execute the command
            DialogResult msgret = MessageBox.Show("Are you sure to delete this record?", "Warning",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (msgret == DialogResult.Yes)
            {
                //execute the command
                int ret = com.ExecuteNonQuery();
                MessageBox.Show("Score records deleted:" + ret, "Information");
            }


            //disconnect
            conn.Close();
        }
    }
}
