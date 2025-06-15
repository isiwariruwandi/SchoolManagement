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
using System.IO;

namespace SchoolManagement
{
    public partial class ManageStudents : Form
    {
        public ManageStudents()
        {
            InitializeComponent();
        }

        private void btnupload_Click(object sender, EventArgs e)
        {
            //browse from computer
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Select Photo(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";

            if (opf.ShowDialog() == DialogResult.OK)
                Picbox.Image = Image.FromFile(opf.FileName);
        }

        private void ManageStudents_Load(object sender, EventArgs e)
        {
            showTable();
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
                    SqlCommand com = new SqlCommand("SELECT * FROM Stdregis", conn);
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
                dataGridView1.DataSource = getStudentlist();

                // Check if the DataGridView contains the image column
                if (dataGridView1.Columns["Photo"] != null)
                {
                    DataGridViewImageColumn imageColumn = (DataGridViewImageColumn)dataGridView1.Columns["Photo"];
                    imageColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Display Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //display data to textboxes
        private void dataGridView1_Click(object sender, EventArgs e)
        {
            IDTB.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            FnameTB.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            LnameTB.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            BdayDateTime.Value = (DateTime)dataGridView1.CurrentRow.Cells[3].Value;
            PhoneTB.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString(); 
            if (dataGridView1.CurrentRow.Cells[5].Value.ToString() == "Male")
                radiomale.Checked = true;
            else
                radiofemale.Checked = true;

            AddressTB.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            byte[] img = (byte[])dataGridView1.CurrentRow.Cells[7].Value;
            MemoryStream ms = new MemoryStream(img);
            Picbox.Image = Image.FromStream(ms);
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            this.IDTB.Clear();
            this.FnameTB.Clear();
            this.LnameTB.Clear();
            this.BdayDateTime.Value = DateTime.Now; // Reset to today's date
            this.PhoneTB.Clear();
            this.radiomale.Checked = false;
            this.radiofemale.Checked = false;
            this.AddressTB.Clear();
            this.Picbox.Image = null; // Clear the image
        }

        // Method to fetch student list from the database
        public DataTable searchStudent(string searchdata)
        {
            try
            {
                // Ensure the connection is properly managed
                string cs = @"Data Source=DESKTOP-LKG8OU2; Initial catalog=School;Integrated Security=True";
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    conn.Open();
                    SqlCommand com = new SqlCommand("SELECT * FROM Stdregis WHERE CONCAT(StdID,FirstName,LastName,Address) LIKE '%"+searchdata+"%'", conn);
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
                dataGridView1.DataSource = searchStudent(txtsearch.Text);

                // Check if the DataGridView contains the image column
                if (dataGridView1.Columns["Photo"] != null)
                {
                    DataGridViewImageColumn imageColumn = (DataGridViewImageColumn)dataGridView1.Columns["Photo"];
                    imageColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;
                }
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

                // Check student age
                int born_year = BdayDateTime.Value.Year;
                int this_year = DateTime.Now.Year;
                if (this_year - born_year < 5 || this_year - born_year > 25)
                {
                    MessageBox.Show("The student age must be between 5 and 25", "Invalid Birthdate", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Stop further execution
                }

                

                // Define the SQL statement
                string sql = "UPDATE Stdregis SET FirstName=@FirstName, LastName=@LastName, Birthday=@Birthday, Phone=@Phone, Gender=@Gender, Address=@Address, Photo=@Photo WHERE StdID=@StdID ";
                SqlCommand com = new SqlCommand(sql, conn);

                // Add parameters
                com.Parameters.AddWithValue("@StdID", IDTB.Text);
                com.Parameters.AddWithValue("@FirstName", FnameTB.Text);
                com.Parameters.AddWithValue("@LastName", LnameTB.Text);
                com.Parameters.AddWithValue("@Birthday", BdayDateTime.Value);
                com.Parameters.AddWithValue("@Phone", PhoneTB.Text);
                com.Parameters.AddWithValue("@Gender", radiomale.Checked ? "Male" : "Female");
                com.Parameters.AddWithValue("@Address", AddressTB.Text);

                // Convert the image to a byte array and add it as a parameter
                byte[] imageBytes = ImageToByteArray(Picbox.Image);
                com.Parameters.Add("@Photo", SqlDbType.VarBinary).Value = imageBytes;

                // Execute the command
                int ret = com.ExecuteNonQuery();
                MessageBox.Show("Student Details Updated : " + ret, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        // Function to convert an image to a byte array
        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png); // Use a suitable format
                return ms.ToArray();
            }
        }

        // Function to verify that all required fields are filled
        private bool verify()
        {
            if ((FnameTB.Text == "") || (LnameTB.Text == "") || (PhoneTB.Text == "") || (AddressTB.Text == "")
                    || (Picbox.Image == null))
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
            string sql = "DELETE FROM Stdregis WHERE StdID=@StdID;";
            SqlCommand com = new SqlCommand(sql, conn);
            com.Parameters.AddWithValue("@StdID", IDTB.Text);
            com.Parameters.AddWithValue("@FirstName", FnameTB.Text);
            com.Parameters.AddWithValue("@LastName", LnameTB.Text);
            com.Parameters.AddWithValue("@Birthday", BdayDateTime.Value);
            com.Parameters.AddWithValue("@Phone", PhoneTB.Text);
            com.Parameters.AddWithValue("@Gender", radiomale.Checked ? "Male" : "Female");
            com.Parameters.AddWithValue("@Address", AddressTB.Text);

            // Convert the image to a byte array and add it as a parameter
            byte[] imageBytes = ImageToByteArray(Picbox.Image);
            com.Parameters.Add("@Photo", SqlDbType.VarBinary).Value = imageBytes;

            //execute the command
            DialogResult msgret = MessageBox.Show("Are you sure to delete this record?", "Warning",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (msgret == DialogResult.Yes)
            {
                //execute the command
                int ret = com.ExecuteNonQuery();
                MessageBox.Show("Student records deleted:" + ret, "Information");
            }


            //disconnect
            conn.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
