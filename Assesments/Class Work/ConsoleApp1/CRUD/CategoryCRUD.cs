using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApp1.CRUD
{
    internal class CategoryCRUD
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);
        
        public void AddCategory(Category c)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Categories (Id,Name) VALUES (@Id,@Name)", con);
            cmd.Parameters.AddWithValue("@Id", c.Id);
            cmd.Parameters.AddWithValue("@Name", c.Name);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public string UpdateCategory(Category c)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Categories SET [Name] = @Name WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", c.Id);
                cmd.Parameters.AddWithValue("@Name", c.Name);
                cmd.ExecuteNonQuery();
                return "Success";
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                return "Failed";
            }
            finally
            {
                con.Close();
            }

        }
        public string DeleteCategory(int id)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("delete from Categories where id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
                return "Success";
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                return "Failed";
            }
            finally
            {
                con.Close();
            }

        }

        public List<Category> CategoriesList()
        {
            SqlDataAdapter da = new SqlDataAdapter("select *from Categories", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Category> list = new List<Category>();
            foreach(DataRow dr in dt.Rows)
            {
                Category c = new Category();
                c.Id = Convert.ToInt32(dr["Id"]);
                c.Name = dr["Name"].ToString();
                list.Add(c);
            }
            return list;
         
        }

         


    }

}
