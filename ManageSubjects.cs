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
    public partial class ManageSubjects : Form
    {
        public ManageSubjects()
        {
            InitializeComponent();
        }

        private void ManageSubjects_Load(object sender, EventArgs e)
        {
            showTable();
        }

        public DataTable getSubjectlist()
        {
            try
            {
                // Ensure the connection is properly managed
                string cs = @"Data Source=DESKTOP-LKG8OU2; Initial catalog=School;Integrated Security=True";
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    conn.Open();
                    SqlCommand com = new SqlCommand("SELECT * FROM Subregis", conn);
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
                dataGridView1.DataSource = getSubjectlist();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Display Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        //display data to text boxes
        private void dataGridView1_Click(object sender, EventArgs e)
        {
            SNTB.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            SnameTB.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            HoursTB.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            DescriptionTB.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            this.SNTB.Clear();
            this.SnameTB.Clear();
            this.HoursTB.Clear();
            this.DescriptionTB.Clear();
        }

        // Method to fetch subject list from the database
        public DataTable searchSubject(string searchdata)
        {
            try
            {
                // Ensure the connection is properly managed
                string cs = @"Data Source=DESKTOP-LKG8OU2; Initial catalog=School;Integrated Security=True";
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    conn.Open();
                    SqlCommand com = new SqlCommand("SELECT * FROM Subregis WHERE CONCAT(SubjectID, SubjectName, Hours, Description) LIKE '%" + searchdata + "%'", conn);
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

        private void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {
                // Set the DataSource for the DataGridView
                dataGridView1.DataSource = searchSubject(txtsearch.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Display Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                string sql = "UPDATE Subregis SET SubjectName=@SubjectName, Hours=@Hours, Description=@Description WHERE SubjectID=@SubjectID ";
                SqlCommand com = new SqlCommand(sql, conn);

                // Add parameters
                com.Parameters.AddWithValue("@SubjectID", SNTB.Text);
                com.Parameters.AddWithValue("@SubjectName", SnameTB.Text);
                com.Parameters.AddWithValue("@Hours", HoursTB.Text);
                com.Parameters.AddWithValue("@Description", DescriptionTB.Text);

                // Execute the command
                int ret = com.ExecuteNonQuery();
                MessageBox.Show("Subject Details Updated : " + ret, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if ((SNTB.Text == "") || (SnameTB.Text == "") || (HoursTB.Text == "") || (DescriptionTB.Text == ""))
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
            string sql = "DELETE FROM Subregis WHERE SubjectID=@SubjectID;";
            SqlCommand com = new SqlCommand(sql, conn);
            com.Parameters.AddWithValue("@SubjectID", SNTB.Text);
            com.Parameters.AddWithValue("@SubjectName", SnameTB.Text);
            com.Parameters.AddWithValue("@Hours", HoursTB.Text);
            com.Parameters.AddWithValue("@Description", DescriptionTB.Text);


            //execute the command
            DialogResult msgret = MessageBox.Show("Are you sure to delete this record?", "Warning",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (msgret == DialogResult.Yes)
            {
                //execute the command
                int ret = com.ExecuteNonQuery();
                MessageBox.Show("Subject records deleted:" + ret, "Information");
            }


            //disconnect
            conn.Close();
        }
    }
}
