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

        public void InsertMhs(string nim, string nama, string alamat, string jeniskelamin, DateTime tanggallahir, string kodeProdi, byte[] foto)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlTransaction trans = conn.BeginTransaction();
            try
            {
                SqlCommand command = new SqlCommand("sp_InsertMahasiswa", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Transaction = trans; // Kaitkan transaksi
                command.Parameters.AddWithValue("@pNIM", nim);
                command.Parameters.AddWithValue("@pNama", nama);
                command.Parameters.AddWithValue("@pAlamat", alamat);
                command.Parameters.AddWithValue("@pTanggalLahir", tanggallahir);
                command.Parameters.AddWithValue("@pJenisKelamin", jeniskelamin);
                command.Parameters.AddWithValue("@pKodeProdi", kodeProdi);

                // PERBAIKAN: Deklarasi parameter tipe VarBinary secara eksplisit
                SqlParameter fotoParam = new SqlParameter("@pFoto", SqlDbType.VarBinary);
                if (foto != null)
                    fotoParam.Value = foto;
                else
                    fotoParam.Value = DBNull.Value;

                command.Parameters.Add(fotoParam);

                command.ExecuteNonQuery();
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public void UpdateMhs(string nim, string nama, string alamat, string jeniskelamin, DateTime tanggallahir, string kodeProdi, byte[] foto)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlCommand command = new SqlCommand("sp_UpdateMahasiswa", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@pNIM", nim);
            command.Parameters.AddWithValue("@pNama", nama);
            command.Parameters.AddWithValue("@pAlamat", alamat);
            command.Parameters.AddWithValue("@pJenisKelamin", jeniskelamin);
            command.Parameters.AddWithValue("@pTanggalLahir", tanggallahir);
            command.Parameters.AddWithValue("@pKodeProdi", kodeProdi);

            // PERBAIKAN: Deklarasi parameter tipe VarBinary secara eksplisit
            SqlParameter fotoParam = new SqlParameter("@pFoto", SqlDbType.VarBinary);
            if (foto != null)
                fotoParam.Value = foto;
            else
                fotoParam.Value = DBNull.Value;

            command.Parameters.Add(fotoParam);

            command.ExecuteNonQuery();
        }

        public void DeleteMhs(string nim)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlCommand cmd = new SqlCommand("sp_DeleteMahasiswa", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pNIM", nim);
            cmd.ExecuteNonQuery();
        }

        public void resetData()
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            string deleteQuery = "DELETE FROM mahasiswa;";
            SqlCommand cmdDelete = new SqlCommand(deleteQuery, conn);
            cmdDelete.ExecuteNonQuery();

            string insertQuery = @"INSERT INTO mahasiswa SELECT * FROM Mahasiswa_Backup;";
            SqlCommand cmdInsert = new SqlCommand(insertQuery, conn);
            cmdInsert.ExecuteNonQuery();
        }

        public void testInject(string nim)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            string query = "Update mahasiswa set nama = 'HACKED' where NIM = " + nim;
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
        }

        public DataTable GetMhsByNIM(string nim)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlCommand cmd = new SqlCommand("sp_GetMahasiswaByNIM", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pNIM", nim);

            da = new SqlDataAdapter(cmd);
            dtMahasiswa = new DataTable();
            da.Fill(dtMahasiswa);
            return dtMahasiswa;
        }

        public void InsertLog(string message)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlCommand cmd = new SqlCommand("sp_LogMessage", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@psn", message);
            cmd.ExecuteNonQuery();
        }

        public DataTable getProdi()
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlCommand cmd = new SqlCommand("select NamaProdi from ProgramStudi", conn);
            cmd.CommandType = CommandType.Text;
            dtProdi = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dtProdi);
            return dtProdi;
        }

        public DataTable getDataRekap(string prodi, DateTime tanggalMasuk)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlCommand cmd = new SqlCommand("sp_Report", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@inProdi", prodi);
            cmd.Parameters.AddWithValue("@inTglMsuk", tanggalMasuk.Year.ToString());

            da = new SqlDataAdapter(cmd);
            dtMahasiswa = new DataTable();
            da.Fill(dtMahasiswa);
            return dtMahasiswa;
        }

        
    }
}