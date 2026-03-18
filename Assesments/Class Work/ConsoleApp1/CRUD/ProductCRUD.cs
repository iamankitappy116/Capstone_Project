using ConsoleApp1.Models;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace WiproTraining
{
    internal class ProductCRUD
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);

        public string AddProduct(Product p)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("insert into Products values (@Name, @CategId)", con);

                cmd.Parameters.AddWithValue("@Name", p.Name);
                cmd.Parameters.AddWithValue("@CategId", p.CategId);

                cmd.ExecuteNonQuery();
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                con.Close();
            }
        }
        public Product UpdateProduct(Product p)
        {
            SqlCommand cmd = new SqlCommand("update Products set Name=@name, CategId=@categId where ProductId = @id", con);

            try
            {
                con.Open();
                cmd.Parameters.AddWithValue("@name", p.Name);
                cmd.Parameters.AddWithValue("@categId", p.CategId);
                cmd.Parameters.AddWithValue("@id", p.ProductId);
                cmd.ExecuteNonQuery();
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        public string Delete(int id)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("delete from Products where ProductId=@id", con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                con.Close();
            }
        } //Ctrl + M + O

        public List<Product> GetProducts()
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from products", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                List<Product> products = new List<Product>();
                foreach (DataRow dr in dt.Rows)
                {
                    Product p = new Product()
                    {
                        ProductId = int.Parse(dr["ProductId"].ToString()),
                        Name = dr["Name"].ToString(),
                        CategId = int.Parse(dr["CategId"].ToString())
                    };
                    products.Add(p);
                }
                return products;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

    }
}
