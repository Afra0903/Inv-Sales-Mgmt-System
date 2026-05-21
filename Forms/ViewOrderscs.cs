using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Inventory_SalesManagementSystem
{
    public partial class ViewOrderscs : Form
    {
        public ViewOrderscs()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\user\Documents\Inventorydb.mdf;Integrated Security=True;Connect Timeout=30");
        void populateproducts()
        {
            try
            {
                Con.Open();
                string Myquery = "select * from OrderTbl";
                SqlDataAdapter da = new SqlDataAdapter(Myquery, Con);
                SqlCommandBuilder builder = new SqlCommandBuilder(da);
                var ds = new DataSet();
                da.Fill(ds);
                OrderGV.DataSource = ds.Tables[0];
                Con.Close();
            }
            catch
            {

            }
        }

        private void OrderGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void ViewOrderscs_Load(object sender, EventArgs e)
        {
            populateproducts();
            try
            {
                Con.Open();
                MessageBox.Show("✅ Database connected successfully!");
                Con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Connection failed: " + ex.Message);
            }

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("Order Summary", new Font("Century", 25, FontStyle.Bold), Brushes.Red, new Point(230, 30));

            e.Graphics.DrawString("Order ID: " + OrderGV.SelectedRows[0].Cells[0].Value.ToString(),
                new Font("Century", 20, FontStyle.Regular), Brushes.Black, new Point(80, 100));

            e.Graphics.DrawString("Customer ID: " + OrderGV.SelectedRows[0].Cells[1].Value.ToString(),
                new Font("Century", 20, FontStyle.Regular), Brushes.Black, new Point(80, 133));

            e.Graphics.DrawString("Customer Name: " + OrderGV.SelectedRows[0].Cells[2].Value.ToString(),
                new Font("Century", 20, FontStyle.Regular), Brushes.Black, new Point(80, 166));

            e.Graphics.DrawString("Order Date: " + OrderGV.SelectedRows[0].Cells[3].Value.ToString(),
                new Font("Century", 20, FontStyle.Regular), Brushes.Black, new Point(80, 199));

            e.Graphics.DrawString("Order Amount: " + OrderGV.SelectedRows[0].Cells[4].Value.ToString(),
                new Font("Century", 20, FontStyle.Regular), Brushes.Black, new Point(80, 232));

            e.Graphics.DrawString("Afrae2410867", new Font("Century", 25, FontStyle.Bold), Brushes.Red, new Point(230, 350));
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}