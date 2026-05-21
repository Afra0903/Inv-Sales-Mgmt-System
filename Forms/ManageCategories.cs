

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Inventory_SalesManagementSystem
{
    public partial class ManageCategories : Form
    {
        public ManageCategories()
        {
            InitializeComponent();
            // optional: ensure grid selects full row
            CategoriesGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            CategoriesGV.MultiSelect = false;
        }

        // Use a single connection string (if you prefer centralize it elsewhere, do that)
        private readonly string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\user\Documents\Inventorydb.mdf;Integrated Security=True;Connect Timeout=30";

        void populate()
        {
            try
            {
                using (SqlConnection Con = new SqlConnection(connString))
                {
                    Con.Open();
                    string Myquery = "SELECT * FROM CategoryTbl";
                    SqlDataAdapter da = new SqlDataAdapter(Myquery, Con);
                    var ds = new DataSet();
                    da.Fill(ds);
                    CategoriesGV.DataSource = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Populate failed: " + ex.Message);
            }
        }

        private void ManageCategories_Load(object sender, EventArgs e)
        {
            populate();

            // quick connection check (optional)
            try
            {
                using (SqlConnection Con = new SqlConnection(connString))
                {
                    Con.Open();
                    MessageBox.Show("✅ Database connected successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Connection failed: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e) // Add
        {
            if (string.IsNullOrWhiteSpace(CatIdTb.Text) || string.IsNullOrWhiteSpace(CatNameTb.Text))
            {
                MessageBox.Show("Enter both Category Id and Name.");
                return;
            }

            try
            {
                using (SqlConnection Con = new SqlConnection(connString))
                {
                    Con.Open();
                    // explicitly name columns to avoid issues if table structure changes
                    string sql = "INSERT INTO CategoryTbl (CatId, CatName) VALUES (@id, @name)";
                    using (SqlCommand cmd = new SqlCommand(sql, Con))
                    {
                        cmd.Parameters.AddWithValue("@id", CatIdTb.Text.Trim());
                        cmd.Parameters.AddWithValue("@name", CatNameTb.Text.Trim());
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                            MessageBox.Show("Category Successfully Added");
                        else
                            MessageBox.Show("Insert failed.");
                    }
                }
                populate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Add failed: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e) // Delete
        {
            if (string.IsNullOrWhiteSpace(CatIdTb.Text))
            {
                MessageBox.Show("Enter The Category Id");
                return;
            }

            var confirm = MessageBox.Show("Are you sure you want to delete this category?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                using (SqlConnection Con = new SqlConnection(connString))
                {
                    Con.Open();
                    string myquery = "DELETE FROM CategoryTbl WHERE CatId = @id";
                    using (SqlCommand cmd = new SqlCommand(myquery, Con))
                    {
                        cmd.Parameters.AddWithValue("@id", CatIdTb.Text.Trim());
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                            MessageBox.Show("Category Successfully Deleted");
                        else
                            MessageBox.Show("No category found with that ID.");
                    }
                }
                populate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete failed: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e) // Update
        {
            if (string.IsNullOrWhiteSpace(CatIdTb.Text) || string.IsNullOrWhiteSpace(CatNameTb.Text))
            {
                MessageBox.Show("Enter both Category Id and Name.");
                return;
            }

            try
            {
                using (SqlConnection Con = new SqlConnection(connString))
                {
                    Con.Open();
                    string sql = "UPDATE CategoryTbl SET CatName = @name WHERE CatId = @id";
                    using (SqlCommand cmd = new SqlCommand(sql, Con))
                    {
                        cmd.Parameters.AddWithValue("@name", CatNameTb.Text.Trim());
                        cmd.Parameters.AddWithValue("@id", CatIdTb.Text.Trim());
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                            MessageBox.Show("Category Successfully Updated");
                        else
                            MessageBox.Show("No category found with that ID.");
                    }
                }
                populate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update failed: " + ex.Message);
            }
        }

        // Better to use CellClick rather than CellContentClick
        private void  CategoriesGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    var row = CategoriesGV.Rows[e.RowIndex];
                    CatIdTb.Text = row.Cells["CatId"].Value?.ToString();
                    CatNameTb.Text = row.Cells["CatName"].Value?.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Row select failed: " + ex.Message);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            HomeForm home = new HomeForm();
            home.Show();
            this.Hide();
        }
    }
}





