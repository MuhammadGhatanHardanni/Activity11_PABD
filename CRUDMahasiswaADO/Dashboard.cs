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
        
    }
}