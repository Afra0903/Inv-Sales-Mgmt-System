

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Inventory_SalesManagementSystem
{
    public partial class ManageOrders : Form
    {
        public ManageOrders()
        {
            InitializeComponent();
        }

        SqlConnection Con = new SqlConnection(
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\user\Documents\Inventorydb.mdf;Integrated Security=True;Connect Timeout=30");

        DataTable table = new DataTable();
        int num = 0, uprice, totprice, qty, stock, flag = 0, sum = 0;
        string product;

        // Populate Customers 
        void populate()
        {
            try
            {
                Con.Open();
                string query = "SELECT * FROM CustomerTbl";
                SqlDataAdapter da = new SqlDataAdapter(query, Con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                CustomerGV.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customers: " + ex.Message);
            }
            finally
            {
                Con.Close();
            }
        }

        // Populate Products 
        void populateproducts()
        {
            try
            {
                Con.Open();
                string query = "SELECT * FROM ProductTbl";
                SqlDataAdapter da = new SqlDataAdapter(query, Con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                ProductsGV.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message);
            }
            finally
            {
                Con.Close();
            }
        }

        // Fill Category Combo
        void fillcategory()
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM CategoryTbl", Con);
                SqlDataReader rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Columns.Add("CatName", typeof(string));
                dt.Load(rdr);
                SearchCombo.ValueMember = "CatName";
                SearchCombo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message);
            }
            finally
            {
                Con.Close();
            }
        }

        // Update Product Quantity
        void updateproduct()
        {
            try
            {
                int id = Convert.ToInt32(ProductsGV.SelectedRows[0].Cells[0].Value);
                int newQty = stock - Convert.ToInt32(QtyTb.Text);

                if (newQty < 0)
                {
                    MessageBox.Show("❌ Not enough stock!");
                    return;
                }

                Con.Open();
                string query = "UPDATE ProductTbl SET ProdQty = " + newQty + " WHERE ProdId = " + id;
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("✅ Product quantity updated.");
                populateproducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating product: " + ex.Message);
            }
            finally
            {
                Con.Close();
            }
        }

        // Form Load
        private void ManageOrders_Load(object sender, EventArgs e)
        {
            populate();
            populateproducts();
            fillcategory();

            table.Columns.Add("Num", typeof(int));
            table.Columns.Add("Product", typeof(string));
            table.Columns.Add("Quantity", typeof(int));
            table.Columns.Add("UPrice", typeof(int));
            table.Columns.Add("TotalPrice", typeof(int));
            OrderGV.DataSource = table;

            // Test DB connection
            try
            {
                Con.Open();
                MessageBox.Show("✅ Database connected successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Database connection failed: " + ex.Message);
            }
            finally
            {
                Con.Close();
            }
        }

        // Customer Grid Click
        private void CustomerGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (CustomerGV.SelectedRows.Count > 0)
            {
                CustId.Text = CustomerGV.SelectedRows[0].Cells[0].Value.ToString();
                CustName.Text = CustomerGV.SelectedRows[0].Cells[1].Value.ToString();
            }
        }

        //  Product Grid Click 
        private void ProductsGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ProductsGV.SelectedRows.Count > 0)
            {
                product = ProductsGV.SelectedRows[0].Cells[1].Value.ToString();
                stock = Convert.ToInt32(ProductsGV.SelectedRows[0].Cells[2].Value);
                uprice = Convert.ToInt32(ProductsGV.SelectedRows[0].Cells[3].Value);
                flag = 1;
            }
        }

        //  ADD Button
        private void button1_Click(object sender, EventArgs e)
        {
            if (QtyTb.Text == "")
            {
                MessageBox.Show("Enter product quantity.");
                return;
            }
            if (flag == 0)
            {
                MessageBox.Show("Select a product first.");
                return;
            }
            if (Convert.ToInt32(QtyTb.Text) > stock)
            {
                MessageBox.Show("Not enough stock available.");
                return;
            }

            num++;
            qty = Convert.ToInt32(QtyTb.Text);
            totprice = qty * uprice;
            table.Rows.Add(num, product, qty, uprice, totprice);
            OrderGV.DataSource = table;

            sum += totprice;
            TotAmount.Text = sum.ToString();

            updateproduct();
            flag = 0;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void OrderIdTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void orderdate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        // SAVE ORDER Button 
        private void button2_Click(object sender, EventArgs e)
         {
             if (OrderIdTb.Text == "" || CustId.Text == "" || CustName.Text == "" || TotAmount.Text == "")
             {
                 MessageBox.Show("Fill in all required details.");
                 return;
             }

             try
             {
                 Con.Open();
                 string query = "INSERT INTO OrderTbl VALUES (" +
                     OrderIdTb.Text + "," +
                     CustId.Text + ",'" +
                     CustName.Text + "','" +
                     orderdate.Text + "'," +
                     TotAmount.Text + ")";
                 SqlCommand cmd = new SqlCommand(query, Con);
                 cmd.ExecuteNonQuery();
                 MessageBox.Show("✅ Order added successfully!");
             }
             catch (Exception ex)
             {
                 MessageBox.Show("Error saving order: " + ex.Message);
             }
             finally
             {
                 Con.Close();
             }
         } 

       
        // VIEW ORDERS Button 
        private void button3_Click(object sender, EventArgs e)
        {
            ViewOrderscs view = new ViewOrderscs();
            view.Show();
        }

        // HOME Button
        private void button4_Click(object sender, EventArgs e)
        {
            HomeForm home = new HomeForm();
            home.Show();
            this.Hide();
        }

        // Category Search
        private void SearchCombo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Con.Open();
                string query = "SELECT * FROM ProductTbl WHERE ProdCat='" + SearchCombo.SelectedValue.ToString() + "'";
                SqlDataAdapter da = new SqlDataAdapter(query, Con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                ProductsGV.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filtering products: " + ex.Message);
            }
            finally
            {
                Con.Close();
            }
        }

        private void OrderGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void QtyTb_TextChanged(object sender, EventArgs e)
        {

        }


    }
}