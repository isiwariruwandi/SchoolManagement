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
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            customizedesign();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            studentCount();
        }

        private void studentCount()
        {
            //display the values
            lbl_totalstd.Text = "Total Students : " + totalStudent();
            lbl_malestd.Text = "Male : " + maleStudent();
            lbl_femalestd.Text = "Female : " + femaleStudent();
        }

        private void customizedesign()
        {
            panel_std.Visible = false;
            panel_sub.Visible = false;
            panel_scr.Visible = false;
        }

        private void hideSubmenu()
        {
            if (panel_std.Visible == true)
                panel_std.Visible = false;
            if (panel_sub.Visible == true)
                panel_sub.Visible = false;
            if (panel_scr.Visible == true)
                panel_scr.Visible = false;
        }

        private void showSubmenu(Panel submenu)
        {
            if(submenu.Visible == false)
            {
                hideSubmenu();
                submenu.Visible = true;
            }
            else
            {
                submenu.Visible = false;
            }
        }

        private void btn_std_Click(object sender, EventArgs e)
        {
            showSubmenu(panel_std);
        }

        private void btn_stdregister_Click(object sender, EventArgs e)
        {
            Registration reg = new Registration();
            reg.Show();
            hideSubmenu();
        }

        private void btn_stdmanage_Click(object sender, EventArgs e)
        {
            ManageStudents ms = new ManageStudents();
            ms.Show();
            hideSubmenu();
        }

        private void btn_stdprint_Click(object sender, EventArgs e)
        {
            PrintStudents ps = new PrintStudents();
            ps.Show();
            hideSubmenu();
        }

        private void btn_subnew_Click(object sender, EventArgs e)
        {
            NewSubject ns = new NewSubject();
            ns.Show();
            hideSubmenu();
        }

        private void btn_submanage_Click(object sender, EventArgs e)
        {
            ManageSubjects mns = new ManageSubjects();
            mns.Show();
            hideSubmenu();
        }

        private void btn_subprint_Click(object sender, EventArgs e)
        {
            PrintSubjects pns = new PrintSubjects();
            pns.Show();
            hideSubmenu();
        }

        private void btn_scrnew_Click(object sender, EventArgs e)
        {
            Score sc = new Score();
            sc.Show();
            hideSubmenu();
        }

        private void btn_scrmanage_Click(object sender, EventArgs e)
        {
            ManageScore msc = new ManageScore();
            msc.Show();
            hideSubmenu();
        }

        private void btn_scrprint_Click(object sender, EventArgs e)
        {
            PrintScore psc = new PrintScore();
            psc.Show();
            hideSubmenu();
        }

        private void btn_sub_Click(object sender, EventArgs e)
        {
            showSubmenu(panel_sub);
        }

        private void btn_scr_Click(object sender, EventArgs e)
        {
            showSubmenu(panel_scr);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btn_dash_Click(object sender, EventArgs e)
        {
            studentCount();
        }

        //function to execute count 
        public string exeCount(string query)
        {
            string cs = @"Data Source=DESKTOP-LKG8OU2; Initial catalog=School;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                SqlCommand com = new SqlCommand(query, conn);
                string count = com.ExecuteScalar().ToString();
                conn.Close();
                return count;
            }
        }

        //get total student count
        public string totalStudent()
        {
            return exeCount("SELECT COUNT (*) FROM Stdregis");
        }
        
        //male students count
        public string maleStudent()
        {
            return exeCount("SELECT COUNT (*) FROM Stdregis WHERE Gender='Male'");
        }

        //female students count
        public string femaleStudent()
        {
            return exeCount("SELECT COUNT (*) FROM Stdregis WHERE Gender='Female'");
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            //execute the command
            DialogResult msgret = MessageBox.Show("Do You Want To Exit?", "Warning",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (msgret == DialogResult.Yes)
            {
                this.Hide();
                Login logf = new Login();
                logf.Show();
            }
            
        }

        private void Dashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
