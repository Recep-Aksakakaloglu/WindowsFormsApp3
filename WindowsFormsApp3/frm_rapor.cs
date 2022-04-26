using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace WindowsFormsApp3
{
    public partial class frm_rapor : Form
    {
        public string _idh;
        public frm_rapor(string idh)
        {
            InitializeComponent();
            _idh = idh;
        }
        sqlBaglantisi bgl = new sqlBaglantisi();
        public int year;
        public int month;
        public int day;
        public string _idd;
        private void frm_rapor_Load(object sender, EventArgs e)
        {
            year = DateTime.Now.Year;
            month = DateTime.Now.Month;
            day = DateTime.Now.Day;
            dateTimePicker1.Value = new DateTime(year, month, day);
            //dateTimePicker1 = DateTime.Now.ToString();
            SqlCommand komut = new SqlCommand("Select Hasta_id, Hasta_ad, Hasta_soyad, Hasta_tc from tbl_hasta where Hasta_id = '" + _idh + "'", bgl.baglanti());
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                txthid.Text = dr[0].ToString();
                txthad.Text = dr[1].ToString();
                txthsoyad.Text = dr[2].ToString();
                txthtc.Text = dr[3].ToString();
            }
            bgl.baglanti().Close();

            _idd = AppUser.Doktor_id.ToString();
            SqlCommand komut2 = new SqlCommand("Select Doktor_ad, Doktor_soyad, tbl_brans.Brans_ad, tbl_unvan.Unvan_ad from tbl_doktor inner join tbl_brans on tbl_brans.Brans_id = tbl_doktor.Doktor_brans_id " +
" inner join tbl_unvan on tbl_unvan.Unvan_id = tbl_doktor.Doktor_unvan where tbl_doktor.Doktor_id = '" + _idd + "'", bgl.baglanti());
            SqlDataReader dr2 = komut2.ExecuteReader();
            while (dr2.Read())
            {
                txtdad.Text = dr2[0].ToString();
                txtdsoyad.Text = dr2[1].ToString();
                txtdbrans.Text = dr2[2].ToString();
                txtdunvan.Text = dr2[3].ToString();
            }
            bgl.baglanti().Close();

            SqlCommand komut4 = new SqlCommand("Insert Into tbl_rapor (Hasta_id, Doktor_id, Rapor_Baslangic, Rapor_Bitis, Rapor_aciklama, Rapor_durum) values (@p1,@p2,@p3,@p4,@p5,@p6)", bgl.baglanti());
            komut4.Parameters.AddWithValue("@p1", _idh);
            komut4.Parameters.AddWithValue("@p2", _idd);
            komut4.Parameters.AddWithValue("@p3", dateTimePicker2.Value);
            komut4.Parameters.AddWithValue("@p4", dateTimePicker3.Value);
            komut4.Parameters.AddWithValue("@p5", txtaciklama.Text);
            if (radioButton1.Checked == true)
            {
                komut4.Parameters.AddWithValue("@p6", "False");
            }
            else
            {
                komut4.Parameters.AddWithValue("@p6", "True");
            }

            komut4.ExecuteNonQuery();
            bgl.baglanti().Close();


            SqlCommand komut3 = new SqlCommand("Select Rapor_id from tbl_rapor", bgl.baglanti());
            SqlDataReader dr3 = komut3.ExecuteReader();
            while (dr3.Read())
            {
                txtbno.Text = dr3[0].ToString();
            }
            bgl.baglanti().Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("update tbl_rapor set Rapor_baslangic=@d1, Rapor_bitis=@d2, Rapor_aciklama=@d3, Rapor_durum = @d4 where Rapor_id = '" + txtbno.Text + "'", bgl.baglanti());
            komut.Parameters.AddWithValue("@d1", dateTimePicker2.Value);
            komut.Parameters.AddWithValue("@d2", dateTimePicker3.Value);
            komut.Parameters.AddWithValue("@d3", txtaciklama.Text);
            if (radioButton1.Checked == true)
            {
                komut.Parameters.AddWithValue("@d4", SqlDbType.Bit).Value = true;
            }
            else if (radioButton2.Checked == true)
            {
                komut.Parameters.AddWithValue("@d4", SqlDbType.Bit).Value = false;
            }
            komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Rapor Oluşturuldu");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panel9_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}
