using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FASTFOOD
{
    /// <summary>
    /// Interaction logic for Product_Selection.xaml
    /// </summary>
    public partial class Product_Selection : Window
    {
        public String serviceID;
        public Product_Selection(String serviceID)
        {
            InitializeComponent();

            this.serviceID = serviceID;

            List<Products> items = new List<Products>();

            SqlConnection con = null;
            try
            {
                String query = "SELECT * FROM Products"; 
                SqlConnectionConfiguration connectionConfig = new SqlConnectionConfiguration();
                using (con = connectionConfig.GetSqlConnection())
                {
                    con.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        byte[] imageBytes = (byte[])row["Image"]; 
                        Image image = byteArrayToImage(imageBytes);

                        items.Add(new Products()
                        {
                            Name = row["name"].ToString(),
                            Description = row["description"].ToString(),
                            Image = image
                        });
                    }

                    ProductsListBox.ItemsSource = items;
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message.ToString());
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        private Image byteArrayToImage(byte[] buffer)
        {
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = stream;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();

                Image img = new Image();
                img.Source = image;

                return img;
            }
        }

        public class Products
        {
            public string Name { get; set; }

            public string Description { get; set; }

            public Image Image { get; set; }
        }
        private void ProductsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ProductsListBox.SelectedItem != null)
            {
                DataRowView selected = (DataRowView)ProductsListBox.SelectedItem;

                string query;
                SqlCommand cmd;
                SqlConnection con = null;

              
                try
                {
                    SqlConnectionConfiguration connectionConfig = new SqlConnectionConfiguration();
                    using (con = connectionConfig.GetSqlConnection())
                    {
                        con.Open();
                        query = "INSERT INTO Consumptions (serviceID, productID) values (@serviceID, @productID);";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@productID", selected["id"]);
                        cmd.Parameters.AddWithValue("@serviceID", this.serviceID);

                        using (System.Data.IDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                MessageBox.Show("Error adding.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show("There was an error adding the product: \n" + exc.Message.ToString() + ".", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    if (con != null)
                    {
                        con.Close();
                    }
                }

                this.Close();
            }
        }


    }
}
