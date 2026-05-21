
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Inventory_SalesManagementSystem
{
    public partial class ManageProducts : Form
    {
        public ManageProducts()
        {
            InitializeComponent();
        }

        SqlConnection Con = new SqlConnection(
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\user\Documents\Inventorydb.mdf;Integrated Security=True;Connect Timeout=30"
        );

        // ✅ Fill Category ComboBox
        void fillcategory()
        {
            try
            {
                Con.Open();
                string query = "SELECT CatName FROM CategoryTbl";
                SqlCommand cmd = new SqlCommand(query, Con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                CatCombo.DataSource = dt;
                CatCombo.DisplayMember = "CatName";
                CatCombo.ValueMember = "CatName";
                SearchCombo.DataSource = dt.Copy();
                SearchCombo.DisplayMember = "CatName";
                SearchCombo.ValueMember = "CatName";
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

        // ✅ Populate Products Grid
        void populate()
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

        // ✅ Filter by category
        void filterbycategory()
        {
            try
            {
                if (SearchCombo.SelectedValue == null)
                {
                    MessageBox.Show("Select a category first!");
                    return;
                }

                Con.Open();
                string query = "SELECT * FROM ProductTbl WHERE ProdCat = @Cat";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@Cat", SearchCombo.SelectedValue.ToString());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                ProductsGV.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filtering: " + ex.Message);
            }
            finally
            {
                Con.Close();
            }
        }

        // ✅ ADD Product
        private void button1_Click(object sender, EventArgs e)
        {
            if (ProdIdTb.Text == "" || ProdNameTb.Text == "" || QtyTb.Text == "" || PriceTb.Text == "")
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            try
            {
                Con.Open();
                string query = "INSERT INTO ProductTbl (ProdId, ProdName, ProdQty, ProdPrice, ProdDesc, ProdCat) " +
                               "VALUES (@Id, @Name, @Qty, @Price, @Desc, @Cat)";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@Id", ProdIdTb.Text);
                cmd.Parameters.AddWithValue("@Name", ProdNameTb.Text);
                cmd.Parameters.AddWithValue("@Qty", QtyTb.Text);
                cmd.Parameters.AddWithValue("@Price", PriceTb.Text);
                cmd.Parameters.AddWithValue("@Desc", DescriptionTb.Text);
                cmd.Parameters.AddWithValue("@Cat", CatCombo.SelectedValue.ToString());
                cmd.ExecuteNonQuery();
                MessageBox.Show("✅ Product added successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding product: " + ex.Message);
            }
            finally
            {
                Con.Close();
                populate();
            }
        }

        // ✅ UPDATE Product
        private void button2_Click(object sender, EventArgs e)
        {
            if (ProdIdTb.Text == "")
            {
                MessageBox.Show("Select a product to update!");
                return;
            }

            try
            {
                Con.Open();
                string query = "UPDATE ProductTbl SET " +
                               "ProdName=@Name, ProdQty=@Qty, ProdPrice=@Price, ProdDesc=@Desc, ProdCat=@Cat " +
                               "WHERE ProdId=@Id";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@Name", ProdNameTb.Text);
                cmd.Parameters.AddWithValue("@Qty", QtyTb.Text);
                cmd.Parameters.AddWithValue("@Price", PriceTb.Text);
                cmd.Parameters.AddWithValue("@Desc", DescriptionTb.Text);
                cmd.Parameters.AddWithValue("@Cat", CatCombo.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@Id", ProdIdTb.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("✅ Product updated successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating product: " + ex.Message);
            }
            finally
            {
                Con.Close();
                populate();
            }
        }

        // ✅ DELETE Product
        private void button3_Click(object sender, EventArgs e)
        {
            if (ProdIdTb.Text == "")
            {
                MessageBox.Show("Enter Product ID to delete!");
                return;
            }

            try
            {
                Con.Open();
                string query = "DELETE FROM ProductTbl WHERE ProdId=@Id";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@Id", ProdIdTb.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("🗑️ Product deleted successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting product: " + ex.Message);
            }
            finally
            {
                Con.Close();
                populate();
            }
        }

        // ✅ GRID CLICK EVENT
        private void ProductsGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = ProductsGV.Rows[e.RowIndex];
                ProdIdTb.Text = row.Cells["ProdId"].Value.ToString();
                ProdNameTb.Text = row.Cells["ProdName"].Value.ToString();
                QtyTb.Text = row.Cells["ProdQty"].Value.ToString();
                PriceTb.Text = row.Cells["ProdPrice"].Value.ToString();
                DescriptionTb.Text = row.Cells["ProdDesc"].Value.ToString();
                CatCombo.SelectedValue = row.Cells["ProdCat"].Value.ToString();
            }
        }

        // ✅ SEARCH / FILTER BUTTON
        private void button5_Click(object sender, EventArgs e)
        {
            filterbycategory();
        }

        // ✅ REFRESH BUTTON
        private void button6_Click(object sender, EventArgs e)
        {
            populate();
        }

        // ✅ On Form Load
        private void ManageProducts_Load(object sender, EventArgs e)
        {
            fillcategory();
            populate();
        }

        // ✅ Go Back Home
        private void button4_Click(object sender, EventArgs e)
        {
            HomeForm home = new HomeForm();
            home.Show();
            this.Hide();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void CatCombo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
