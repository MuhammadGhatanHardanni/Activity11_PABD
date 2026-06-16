using System;
using System.Data;
using System.Windows.Forms;

namespace CRUDMahasiswaADO
{
    public partial class FormReport : Form
    {
        // 1. Properti penampung parameter filter
        string prodi;
        DateTime tglmasuk;

        // 2. Instansiasi class DAL agar rapi dan terpusat
        DAL dbLogic = new DAL();

        public FormReport(string Prodi, DateTime TglMasuk)
        {
            InitializeComponent();

            prodi = Prodi;
            tglmasuk = TglMasuk;

            try
            {
                // 3. Panggil method getDataRekap dari DAL.cs
                DataTable dtMahasiswa = dbLogic.getDataRekap(prodi, tglmasuk);

                // 4. Instansiasi desain Report (.rpt)
                // Pastikan "ReportMahasiswa" sesuai dengan nama file .rpt yang kamu buat
                ReportMahasiswa report = new ReportMahasiswa();
                report.SetDataSource(dtMahasiswa);

                // 5. Tampilkan ke Viewer
                crystalReportViewer1.ReportSource = report;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal Load data: " + ex.Message, "Error Laporan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}   