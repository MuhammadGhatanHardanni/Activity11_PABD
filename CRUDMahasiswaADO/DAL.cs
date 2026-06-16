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

        
    }
}