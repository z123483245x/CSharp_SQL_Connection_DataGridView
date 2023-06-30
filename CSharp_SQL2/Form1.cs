using System;
using System.Drawing;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace CSharp_SQL2
{
    public partial class Form1 : Form
    {
        
        DataGridView dgv1;
        private Label lblNo;
        TextBox tbNo = new TextBox();
        private Button btnSearch;
        public Form1()
        {

            InitializeComponent();
            this.Size = new Size(400,500);
            this.StartPosition = FormStartPosition.CenterScreen;

            lblNo = new Label();
            lblNo.Text = "工單號碼 : ";
            lblNo.Location = new Point(30, 28);
            lblNo.AutoSize = true;
            lblNo.Font = new Font(TextBox.DefaultFont.FontFamily, 11);
            this.Controls.Add(lblNo);

            tbNo.Size = new Size(120, 00);
            tbNo.Location = new Point(110, 25);
            tbNo.Text = "請輸入工單號碼";
            this.Controls.Add(tbNo);

            btnSearch = new Button();
            btnSearch.Text = "查詢";
            btnSearch.Size = new Size(70, 26);
            btnSearch.Location = new Point(280, 23);
            btnSearch.Click += btnSearch_Click;
            this.Controls.Add(btnSearch);

            dgv1 = new DataGridView();
            dgv1.Size = new Size(330, 330);
            dgv1.Location = new Point(27, 90);
            dgv1.BorderStyle = BorderStyle.FixedSingle;
            dgv1.BackColor = Color.Gray;
            dgv1.RowHeadersVisible = false;
            dgv1.AutoGenerateColumns = true;
            this.Controls.Add(dgv1);


        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            string connectionString = "DATA SOURCE=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.0.0.1)(PORT=1521)))(CONNECT_DATA=(SID = MIS)));PERSIST SECURITY INFO=True;USER ID=MIS;PASSWORD=OracleMis;";
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sqlQuery = "SELECT mse0044.f002 as 生產條碼 from MISTEST.mse0017 INNER JOIN MISTEST.mse0044 ON MISTEST.mse0017.f005 LIKE MISTEST.mse0044.f001 where mse0017.f003 LIKE :WorkOrderNumber and  Rownum <= 20 ";
                    OracleCommand command = new OracleCommand(sqlQuery, connection);
                    command.Parameters.Add(new OracleParameter(":WorkOrderNumber", OracleDbType.Varchar2)).Value = tbNo.Text;
                    OracleDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        dgv1.DataSource = dataTable;
                        reader.Close();
                        dgv1.Columns["生產條碼"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    }
                    else
                    {
                        MessageBox.Show("目前查詢的工單不存在");
                    }
                    reader.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("連線或查詢過程中發生錯誤 : " + ex.Message);
                }
            }
        }

    }
}
