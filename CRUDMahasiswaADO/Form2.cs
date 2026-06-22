using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net; // WAJIB: Untuk DNS
using System.Net.Sockets; // WAJIB: Untuk membaca IP Address
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUDMahasiswaADO
{
    public partial class Form2 : Form
    {
        // 1. FUNGSI UNTUK MENGAMBIL IP ADDRESS OTOMATIS (Sesuai Modul)
        public static string GetLocalIPAddress()
        {
            string localIP = string.Empty;
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        localIP = ip.ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getting local IP address: " + ex.Message);
            }
            return localIP;
        }

        // 2. CONNECTION STRING DIUPDATE MENGGUNAKAN FUNGSI DI ATAS
        static string connectionString = $"Data Source=LAPTOP-VL5SDNPR\\GHATANHARDANNI;Initial Catalog=DB_AkademikADO; User ID=sa;Password=12345678";

        SqlConnection conn;
        SqlDataAdapter da;
        DataTable dtMahasiswa;
        DataTable dtProdi;

        public Form2()
        {
            InitializeComponent();
            // 3. Inisialisasi koneksi diletakkan di sini agar IP terbaca saat form dimuat
            conn = new SqlConnection(connectionString);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Mengatur format kalender menjadi hanya Tahun
            dtpTanggalMasuk.Format = DateTimePickerFormat.Custom;
            dtpTanggalMasuk.CustomFormat = "yyyy";
            dtpTanggalMasuk.ShowUpDown = true;
            dtpTanggalMasuk.MinDate = new DateTime(2000, 1, 1);
            dtpTanggalMasuk.MaxDate = DateTime.Now;

            // Mencegah user mengetik manual di combobox
            cmbProdi.DropDownStyle = ComboBoxStyle.DropDownList;

            // Mematikan tombol cetak sebelum data di-load
            btnCetak.Enabled = false;

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                // Mengambil data nama prodi untuk dimasukkan ke combobox
                SqlCommand cmd = new SqlCommand("select namaprodi from programstudi", conn);
                cmd.CommandType = CommandType.Text;

                dtProdi = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dtProdi);

                cmbProdi.DataSource = dtProdi;
                cmbProdi.DisplayMember = "namaprodi";
                cmbProdi.ValueMember = "namaprodi";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal Load data: " + ex.Message);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand("sp_Report", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Memasukkan parameter dari UI (Combobox dan DateTimePicker) ke Stored Procedure
                cmd.Parameters.Add("@inProdi", SqlDbType.VarChar, 50).Value = cmbProdi.SelectedValue;
                cmd.Parameters.Add("@inTgLMsuk", SqlDbType.VarChar, 4).Value = dtpTanggalMasuk.Value.Year.ToString();

                da = new SqlDataAdapter(cmd);
                dtMahasiswa = new DataTable();
                da.Fill(dtMahasiswa);

                // Menampilkan hasil dari DataTable ke DataGridView
                dataGridView1.DataSource = dtMahasiswa;

                // Logika mengaktifkan tombol cetak jika data ada (lebih dari 0 baris)
                if (dtMahasiswa.Rows.Count > 0)
                {
                    btnCetak.Enabled = true;
                }
                else
                {
                    btnCetak.Enabled = false;
                    MessageBox.Show("Data tidak ditemukan");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal Load data: " + ex.Message);
            }
        }

        private void btnCetak_Click(object sender, EventArgs e)
        {
            try
            {
                // Memanggil FormReport dan mengirim parameter nama Prodi dan Tahun
                FormReport frmReport = new FormReport(cmbProdi.Text, dtpTanggalMasuk.Value);
                frmReport.Show();
            }
            catch (Exception ex)
            {
                // PERUBAHAN: Membongkar error asli (Inner Exception) di Form2
                string errorAsli = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                MessageBox.Show("Error Asli dari Form2:\n\n" + errorAsli, "Detail Error Laporan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void label1_Click(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dtpTanggalMasuk_ValueChanged(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void cmbProdi_SelectedIndexChanged(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
    }
}