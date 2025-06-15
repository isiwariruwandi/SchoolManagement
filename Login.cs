using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchoolManagement
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            // Verify that all required fields are filled
            if (!verify())
            {
                MessageBox.Show("Fields Can Not Be Empty!", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Stop further execution
            }
            else
            {
                if ((text_user.Text == "Admin") && (text_pass.Text == "123"))
                {
                    DialogResult msgret = MessageBox.Show("Login Successful!", "Information",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    Dashboard dash = new Dashboard();
                    dash.Show();
                }
                else
                {
                    DialogResult msgret = MessageBox.Show("Incorrect Username or Password!", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Verify that all required fields are filled
        private bool verify()
        {
            if ((text_user.Text == "") || (text_pass.Text == ""))
            {
                return false;
            }
            return true;
        }

        private void labelExit_Click(object sender, EventArgs e)
        {
            //execute the command
            DialogResult msgret = MessageBox.Show("Do You Want To Exit?", "Warning",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (msgret == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btn_hide_Click(object sender, EventArgs e)
        {
            if (text_pass.PasswordChar == '\0')
            {
                btn_show.BringToFront();
                text_pass.PasswordChar = '*';
            }
        }

        private void btn_show_Click(object sender, EventArgs e)
        {
            if (text_pass.PasswordChar == '*')
            {
                btn_hide.BringToFront();
                text_pass.PasswordChar = '\0';
            }
        }
    }
}
