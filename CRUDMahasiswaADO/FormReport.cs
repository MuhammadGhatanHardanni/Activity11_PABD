using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient; // Tambahan wajib untuk SQL Server
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUDMahasiswaADO
{
    public partial class FormReport : Form
    {
        // Deklarasi variabel SQL
        static string connectionString = "Data Source=LAPTOP-VL5SDNPR\\GHATANHARDANNI;Initial Catalog=DB_AkademikADO; User ID=sa;Password=12345678";
        SqlConnection conn = new SqlConnection(connectionString);
        SqlDataAdapter da;
        DataTable dtMahasiswa;

        // Properti penampung parameter filter
        string prodi { get; set; }
        DateTime tglmasuk { get; set; }

        // Ubah Constructor agar menerima parameter Prodi dan Tanggal
        public FormReport(string Prodi, DateTime TglMasuk)
        {
            InitializeComponent();

            prodi = Prodi;
            tglmasuk = TglMasuk;

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                // Mengeksekusi Stored Procedure
                SqlCommand cmd = new SqlCommand("sp_Report", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Menambahkan parameter sesuai tipe data di prosedur
                cmd.Parameters.AddWithValue("@inProdi", prodi);
                cmd.Parameters.AddWithValue("@inTglMsuk", tglmasuk.Year.ToString()); // Ambil tahunnya saja

                da = new SqlDataAdapter(cmd);
                dtMahasiswa = new DataTable();
                da.Fill(dtMahasiswa);
                conn.Close();

                // Instansiasi file desain .rpt Anda
                // Pastikan "ReportMahasiswa" sesuai dengan nama file .rpt yang Anda buat
                ReportMahasiswa report = new ReportMahasiswa();
                report.SetDataSource(dtMahasiswa);

                // Tampilkan ke CrystalReportViewer
                crystalReportViewer1.ReportSource = report;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal Load data: " + ex.Message);
            }
        }
    }
}