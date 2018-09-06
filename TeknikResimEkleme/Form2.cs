using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text;


namespace TeknikResimEkleme
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection("Data Source=WIN-OMQO47A64BF\\AYREF;Initial Catalog=HORTUM_2016;Integrated Security=True");
        SqlDataAdapter da;
        DataSet ds1;

        private void btn_Listele_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Columns.Clear();
                string sorgu = "SELECT LTRIM(RTRIM(STK.KOD)) AS 'STOK KODU',LTRIM(RTRIM(STK.AD)) AS 'STOK ADI' FROM STOK00 AS STK(NOLOCK) where  NOT exists(SELECT DISTINCT STKP.EVRAKNO FROM STOK0P AS STKP WHERE STKP.EVRAKNO = STK.KOD) ";
                string sart1 = " AND STK.KOD >= '" + lookUpEdit1.Text + "'";
                string sart2 = " AND STK.KOD <= '" + lookUpEdit2.Text + "'";
                string sart3 = " AND STK.GK_5 = '" + lookUpEdit3.Text + "'";
                if (lookUpEdit1.Text != "[Stok Kartı Seçiniz.]")
                {
                    sorgu = sorgu + sart1;
                }
                if (lookUpEdit2.Text != "[Stok Kartı Seçiniz.]")
                {
                    sorgu = sorgu + sart2;
                }
                if (lookUpEdit3.Text != "[Grup Kodu Seçiniz.]")
                {
                    sorgu = sorgu + sart3;
                }
                sorgu = sorgu + "ORDER BY STK.KOD";
                con.Open();
                da = new SqlDataAdapter(sorgu, con);
                ds1 = new DataSet();
                da.Fill(ds1, "STOK00");
                dataGridView1.DataSource = ds1.Tables[0];
                con.Close();
             

                DataGridViewTextBoxColumn gridtxtbx1 = new DataGridViewTextBoxColumn();
                dataGridView1.Columns.Add(gridtxtbx1);
                gridtxtbx1.HeaderText = "Teknik Resim 1";
                gridtxtbx1.Name = "gridtxtbx1";


                DataGridViewTextBoxColumn gridtxtbx2 = new DataGridViewTextBoxColumn();
                dataGridView1.Columns.Add(gridtxtbx2);
                gridtxtbx2.HeaderText = "Teknik Resim 2";
                gridtxtbx2.Name = "gridtxtbx2";


                DataGridViewTextBoxColumn gridtxtbx3 = new DataGridViewTextBoxColumn();
                dataGridView1.Columns.Add(gridtxtbx3);
                gridtxtbx3.HeaderText = "Teknik Resim 3";
                gridtxtbx3.Name = "gridtxtbx3";
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                da = new SqlDataAdapter("SELECT STK.KOD FROM STOK00 AS STK  WHERE NOT exists (SELECT DISTINCT STKP.EVRAKNO FROM STOK0P AS STKP WHERE STKP.EVRAKNO = STK.KOD ) ORDER BY STK.KOD", con);
                ds1 = new DataSet();
                da.Fill(ds1, "STOK00");
                lookUpEdit1.Properties.DataSource = ds1.Tables[0];
                lookUpEdit2.Properties.DataSource = ds1.Tables[0];

                lookUpEdit1.Properties.DisplayMember = "KOD";
                lookUpEdit1.Properties.ValueMember = "KOD";

                lookUpEdit2.Properties.DisplayMember = "KOD";
                lookUpEdit2.Properties.ValueMember = "KOD";

                da = new SqlDataAdapter("SELECT KOD FROM GECOUST WHERE EVRAKNO='STKGK5'", con);
                ds1 = new DataSet();
                da.Fill(ds1, "GECOUST");
                lookUpEdit3.Properties.DataSource = ds1.Tables[0];
                lookUpEdit3.Properties.DisplayMember = "KOD";
                lookUpEdit3.Properties.ValueMember = "KOD";
                con.Close();
            }
            catch(Exception)
            {
                throw;
            }
        }

        private void btn_temizle_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2) //butonun bulunduğu sütun sırası. Griddeki diğer sütunlar üzerinde yapılan tıklamaları dikkate almasın
            {
                OpenFileDialog dosya = new OpenFileDialog();
                dosya.Filter = "Resim Dosyası |*.jpg;*.nef;*.png| Tüm Dosyalar |*.*";
                dosya.Title = "Dosya gezgini";
                dosya.ShowDialog();
                object DosyaYolu = dosya.FileName;
                dataGridView1.Rows[e.RowIndex].Cells["gridtxtbx1"].Value =  dosya.FileName;
            }
            if (e.ColumnIndex == 3) //butonun bulunduğu sütun sırası. Griddeki diğer sütunlar üzerinde yapılan tıklamaları dikkate almasın
            {
                OpenFileDialog dosya = new OpenFileDialog();
                dosya.Filter = "Resim Dosyası |*.jpg;*.nef;*.png| Tüm Dosyalar |*.*";
                dosya.Title = "Dosya gezgini";
                dosya.ShowDialog();
                object DosyaYolu = dosya.FileName;
                dataGridView1.Rows[e.RowIndex].Cells["gridtxtbx2"].Value = dosya.FileName;
            }
            if (e.ColumnIndex == 4) //butonun bulunduğu sütun sırası. Griddeki diğer sütunlar üzerinde yapılan tıklamaları dikkate almasın
            {
                OpenFileDialog dosya = new OpenFileDialog();
                dosya.Filter = "Resim Dosyası |*.jpg;*.nef;*.png| Tüm Dosyalar |*.*";
                dosya.Title = "Dosya gezgini";
                dosya.ShowDialog();
                object DosyaYolu = dosya.FileName;
                dataGridView1.Rows[e.RowIndex].Cells["gridtxtbx3"].Value = dosya.FileName;
            }
        }

        private void btn_Kayıt_Click(object sender, EventArgs e)
        {
            try
            {
                Type dinamoType = Type.GetTypeFromProgID("D7ebcom1.DocCreator.1");
                dynamic dinamoInstance = Activator.CreateInstance(dinamoType);
                dinamoInstance.SetWorkDirectory(@"C:\d7s\_HORTUM_2017");
                var doc_STOK00 = dinamoInstance.CreateDocumentU("STOK00", "D7KSUPER", "D7KSUPER");

                

                for (int i = 0; i < Convert.ToInt32(dataGridView1.RowCount); i++)
                {
                    string EvrakNo = dataGridView1.Rows[i].Cells[0].Value.ToString();

                    if (dataGridView1.Rows[i].Cells["gridtxtbx1"].Value != null)
                    {
                        doc_STOK00.Load_Voucher_ForUpdate_ForceUnlock(EvrakNo);
                        var RS_STOK0P = doc_STOK00.GetRowSetObject("STOK0P");

                        RS_STOK0P.AddRow();
                        var cnt = RS_STOK0P.GetRowCount();
                        RS_STOK0P.SetCurrentRow(cnt);

                        RS_STOK0P.EVRAKNO = EvrakNo;
                        RS_STOK0P.FULLFILENAME1 = dataGridView1.Rows[i].Cells["gridtxtbx1"].Value.ToString();

                        var sonuc = doc_STOK00.Save_Voucher();
                        doc_STOK00.LoadEmpty_Voucher();
                    }
                    if (dataGridView1.Rows[i].Cells["gridtxtbx2"].Value != null)
                    {
                        doc_STOK00.Load_Voucher_ForUpdate_ForceUnlock(EvrakNo);
                        var RS_STOK0P = doc_STOK00.GetRowSetObject("STOK0P");

                        RS_STOK0P.AddRow();
                        var cnt = RS_STOK0P.GetRowCount();
                        RS_STOK0P.SetCurrentRow(cnt);

                        RS_STOK0P.EVRAKNO = EvrakNo;
                        RS_STOK0P.FULLFILENAME1 = dataGridView1.Rows[i].Cells["gridtxtbx2"].Value.ToString();

                        var sonuc = doc_STOK00.Save_Voucher();
                        doc_STOK00.LoadEmpty_Voucher();
                    }
                    if (dataGridView1.Rows[i].Cells["gridtxtbx3"].Value != null)
                    {
                        doc_STOK00.Load_Voucher_ForUpdate_ForceUnlock(EvrakNo);
                        var RS_STOK0P = doc_STOK00.GetRowSetObject("STOK0P");

                        RS_STOK0P.AddRow();
                        var cnt = RS_STOK0P.GetRowCount();
                        RS_STOK0P.SetCurrentRow(cnt);

                        RS_STOK0P.EVRAKNO = EvrakNo;
                        RS_STOK0P.FULLFILENAME1 = dataGridView1.Rows[i].Cells["gridtxtbx3"].Value.ToString();

                        var sonuc = doc_STOK00.Save_Voucher();
                        doc_STOK00.LoadEmpty_Voucher();
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
