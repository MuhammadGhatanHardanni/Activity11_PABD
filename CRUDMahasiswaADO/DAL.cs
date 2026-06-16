using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient; // Wajib ditambahkan untuk koneksi SQL Server

namespace CRUDMahasiswaADO
{
    internal class DAL
    {
        // Connection string sudah disesuaikan dengan server kamu
        static string connectionString = "Data Source=LAPTOP-VL5SDNPR\\GHATANHARDANNI; Initial Catalog=DB_AkademikADO; User ID=sa; Password=12345678;";

        public string GetConnectionString()
        {
            return connectionString;
        }

        SqlConnection conn = new SqlConnection(connectionString);
        SqlDataAdapter da;
        DataTable dtMahasiswa;
        DataTable dtProdi;

        public int CountMhs()
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlCommand cmd = new SqlCommand("sp_CountMahasiswa", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter outputParam = new SqlParameter("@pCount", SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outputParam);

            cmd.ExecuteNonQuery();
            return Convert.ToInt32(outputParam.Value);
        }

        public DataTable GetMhs()
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlCommand cmd = new SqlCommand("sp_GetMahasiswa", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            da = new SqlDataAdapter(cmd);
            dtMahasiswa = new DataTable();
            da.Fill(dtMahasiswa);
            return dtMahasiswa;
        }

        
    }
}