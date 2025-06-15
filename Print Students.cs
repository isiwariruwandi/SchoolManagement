using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SchoolManagement
{
    public partial class PrintStudents : Form
    {
        public PrintStudents()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void PrintStudents_Load(object sender, EventArgs e)
        {
            this.crystalReportViewer1.ReportSource = @"C:\Users\USER\Documents\Visual Studio 2022\SchoolManagement\SchoolManagement\CrystalReport1.rpt";
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            //connection
            string cs = @"Data Source=DESKTOP-LKG8OU2;
            Initial Catalog=School;Integrated Security=True";
            SqlConnection conn = new SqlConnection(cs);
            conn.Open();

            //Command
            string sql = "SELECT * FROM Stdregis WHERE StdID=@StdID";
            SqlCommand com = new SqlCommand(sql, conn);
            com.Parameters.AddWithValue("@StdID", this.txtsearch.Text);

            //Access Data
            SqlDataAdapter dap = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            dap.Fill(ds);

            //Bind Data with Crystal Report
            CrystalReport1 rpt1 = new CrystalReport1();
            rpt1.Load(@"C:\Users\USER\Documents\Visual Studio 2022\SchoolManagement\SchoolManagement\CrystalReport1.rpt");
            rpt1.SetDataSource(ds.Tables[0]);

            this.crystalReportViewer1.ReportSource = rpt1;

            //Disconnect
            conn.Close();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
