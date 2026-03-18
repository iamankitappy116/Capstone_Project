using System.Data;
using ADOExample.Models;
using Microsoft.Data.SqlClient;

namespace ADOExample.DAL
{
    public class PatientCRUD
    {
        private readonly string? _conStr;

        public PatientCRUD(IConfiguration config)
        {
            _conStr = config.GetConnectionString("DefaultConnection");
        }

        public void AddPatient(Patient p)
        {
            using (SqlConnection con = new SqlConnection(_conStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("Insert into Patient(Name, Age) Values(@Name, @Age)", con);
                cmd.Parameters.AddWithValue("@Name", p.Name);
                cmd.Parameters.AddWithValue("@Age", p.Age);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Patient> GetPatients()
        {
            using (SqlConnection con = new SqlConnection(_conStr))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM Patient", con);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<Patient> patients = new List<Patient>();

                foreach (DataRow dr in dt.Rows)
                {
                    Patient p = new Patient()
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Name = dr["Name"].ToString(),
                        Age = Convert.ToInt32(dr["Age"])
                    };

                    patients.Add(p);
                }

                return patients;
            }
        }
        public Patient GetPatientById(int Id)
        {
            using (SqlConnection con = new SqlConnection(_conStr))
            {
                con.Open();

                string query = "SELECT * FROM Patient WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", Id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Patient
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Age = Convert.ToInt32(reader["Age"])
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void UpdatePatient(Patient p)
        {
            using (SqlConnection con = new SqlConnection(_conStr))
            {
                con.Open();
                string query = "UPDATE Patient SET Name = @Name, Age = @Age WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Name", p.Name);
                    cmd.Parameters.AddWithValue("@Age", p.Age);
                    cmd.Parameters.AddWithValue("@Id", p.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeletePatient(int Id)
        {
            using (SqlConnection con = new SqlConnection(_conStr))
            {
                con.Open();
                string query = "DELETE FROM Patient WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
