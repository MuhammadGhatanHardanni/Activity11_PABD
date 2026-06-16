using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting; // Tambahan wajib untuk Chart

namespace CRUDMahasiswaADO
{
    public partial class Dashboard : Form
    {
        // Mendeklarasikan object class DAL dan variabel pendukung
        DAL dbLogic = new DAL();
        bool isInitializing = true;
        DataTable dt;
        int button = 0;

        public Dashboard()
        {
            InitializeComponent();

            // Mengatur format DateTimePicker menjadi Tahun saja
            dtpTanggalMasuk.MinDate = new DateTime(2000, 1, 1);
            dtpTanggalMasuk.Format = DateTimePickerFormat.Custom;
            dtpTanggalMasuk.CustomFormat = "yyyy";
            dtpTanggalMasuk.ShowUpDown = true;
            dtpTanggalMasuk.MaxDate = DateTime.Now;

            // Mengisi item ComboBox untuk Tipe Grafik
            cmbTipe.DropDownStyle = ComboBoxStyle.DropDownList;
            var items = new List<KeyValuePair<string, SeriesChartType>>
            {
                new KeyValuePair<string, SeriesChartType>("Kolom", SeriesChartType.Column),
                new KeyValuePair<string, SeriesChartType>("Pie", SeriesChartType.Pie)
            };

            isInitializing = true;
            cmbTipe.DataSource = items;
            cmbTipe.DisplayMember = "Key";
            cmbTipe.ValueMember = "Value";
            cmbTipe.SelectedIndex = 0;
            isInitializing = false;

            // Panggil method untuk menampilkan grafik pertama kali saat Dashboard dibuka
            loadDataChart();
        }

        // Method utama untuk memuat dan menggambar grafik
        public void loadDataChart()
        {
            chartProdi.Series.Clear();
            chartProdi.Titles.Clear();
            chartProdi.Legends.Clear();
            chartProdi.ChartAreas.Clear();

            ChartArea ca = new ChartArea("MainArea");
            ca.AxisX.Title = "Program Studi";
            ca.AxisY.Title = "Jumlah Mahasiswa";
            ca.AxisX.LabelStyle.Angle = -45;
            ca.BackColor = Color.Transparent;
            chartProdi.ChartAreas.Add(ca);

            try
            {
                // Menentukan data mana yang diambil dari DAL berdasarkan status button
                if (button == 1)
                {
                    dt = dbLogic.getDataChartByTahun(dtpTanggalMasuk.Value);
                }
                else
                {
                    dt = dbLogic.getAllDataChart();
                }

                SeriesChartType tipe = (SeriesChartType)cmbTipe.SelectedValue;

                if (tipe == SeriesChartType.Column)
                {
                    Series s = new Series("Mahasiswa");
                    s.ChartType = SeriesChartType.Column;
                    foreach (DataRow row in dt.Rows)
                    {
                        string prodi = row["NamaProdi"].ToString();
                        int jumlah = Convert.ToInt32(row["JmlhMhs"]);
                        s.Points.AddXY(prodi, jumlah);
                    }
                    chartProdi.Series.Add(s);
                }
                else
                {
                    Series s = new Series("Jumlah Mahasiswa");
                    s.ChartType = tipe;
                    s.IsValueShownAsLabel = true;
                    s.Label = "#VAL";
                    s.LegendText = "#VALX";
                    foreach (DataRow row in dt.Rows)
                    {
                        string prodi = row["NamaProdi"].ToString();
                        int jumlah = Convert.ToInt32(row["JmlhMhs"]);
                        s.Points.AddXY(prodi, jumlah);
                    }
                    chartProdi.Series.Add(s);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load data: " + ex.Message);
            }

            Title title = new Title("Jumlah Mahasiswa per Program Studi", Docking.Top, new Font("Arial", 14, FontStyle.Bold), Color.DarkBlue);
            chartProdi.Titles.Add(title);
            Legend legend = new Legend("MainLegend");
            legend.Docking = Docking.Right;
            chartProdi.Legends.Add(legend);
        }

        private void dtpTanggalMasuk_ValueChanged(object sender, EventArgs e)
        {
            // Kosongkan saja jika tidak ada aksi otomatis yang diinginkan saat tanggal berubah
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            button = 1;
            loadDataChart();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            button = 0;
            loadDataChart();
        }

        private void cmbTipe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInitializing) return;

            if (button == 1)
            {
                loadDataChart();
            }
            else
            {
                loadDataChart();
            }
        }

        private void chartProdi_Click(object sender, EventArgs e)
        {
            // Kosongkan
        }

        private void btnDataMhs_Click(object sender, EventArgs e)
        {
            // CATATAN: Pastikan 'Form1' diganti dengan nama Form Mahasiswa kamu yang sebenarnya.
            // Misalnya jika form kamu bernama 'frmMahasiswa', maka ubah menjadi:
            // frmMahasiswa frm1 = new frmMahasiswa();
            Form1 frm1 = new Form1();
            frm1.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}