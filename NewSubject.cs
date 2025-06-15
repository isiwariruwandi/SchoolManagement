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
    public partial class NewSubject : Form
    {
        public NewSubject()
        {
            InitializeComponent();
        }

        private void label8_Click(object sender, EventArgs e)
        {

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
                string sql = "INSERT INTO Subregis (SubjectID, SubjectName, Hours, Description) " +
                             "VALUES (@SubjectID, @SubjectName, @Hours, @Description)";
                SqlCommand com = new SqlCommand(sql, conn);

                // Add parameters
                com.Parameters.AddWithValue("@SubjectID", SNTB.Text);
                com.Parameters.AddWithValue("@SubjectName", SnameTB.Text);
                com.Parameters.AddWithValue("@Hours", HoursTB.Text);
                com.Parameters.AddWithValue("@Description", DescriptionTB.Text);

                // Execute the command
                int ret = com.ExecuteNonQuery();
                MessageBox.Show("New Subject Added : " + ret, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnclear_Click(object sender, EventArgs e)
        {
            this.SNTB.Clear();
            this.SnameTB.Clear();
            this.HoursTB.Clear();
            this.DescriptionTB.Clear();
        }

        private void NewSubject_Load(object sender, EventArgs e)
        {
            showTable();
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
    }
}
