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
    public partial class PrintScore : Form
    {
        public PrintScore()
        {
            InitializeComponent();
        }

        private void PrintScore_Load(object sender, EventArgs e)
        {
            this.crystalReportViewer1.ReportSource = @"C:\Users\USER\Documents\Visual Studio 2022\SchoolManagement\SchoolManagement\CrystalReport3.rpt";
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            //connection
            string cs = @"Data Source=DESKTOP-LKG8OU2;
            Initial Catalog=School;Integrated Security=True";
            SqlConnection conn = new SqlConnection(cs);
            conn.Open();

            //Command
            string sql = "SELECT * FROM Scoreinfo WHERE StdID=@StdID";
            SqlCommand com = new SqlCommand(sql, conn);
            com.Parameters.AddWithValue("@StdID", this.txtsearch.Text);

            //Access Data
            SqlDataAdapter dap = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            dap.Fill(ds);

            //Bind Data with Crystal Report
            CrystalReport3 rpt3 = new CrystalReport3();
            rpt3.Load(@"C:\Users\USER\Documents\Visual Studio 2022\SchoolManagement\SchoolManagement\CrystalReport3.rpt");
            rpt3.SetDataSource(ds.Tables[0]);

            this.crystalReportViewer1.ReportSource = rpt3;

            //Disconnect
            conn.Close();
        }
    }
}
