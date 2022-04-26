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
    public partial class frm_tahlil : Form
    {
        public string _idh;
        public frm_tahlil(string idh)
        {
            _idh = idh;
            InitializeComponent();
        }
        sqlBaglantisi bgl = new sqlBaglantisi();

        private void frm_tahlil_Load(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("Select * from tbl_hasta where Hasta_id = ' " + _idh + " '", bgl.baglanti());
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                var age = (DateTime.Today - DateTime.Parse(dr[3].ToString()));
                txthid.Text = dr[0].ToString();
                txthad.Text = dr[1].ToString();
                txthsoyad.Text = dr[2].ToString();
                txthyas.Text = ((int)(age.TotalDays / 365)).ToString();
                txthcinsiyet.Text = dr[4].ToString();
                txthkan.Text = dr[5].ToString();
                txthtc.Text = dr[6].ToString();
            }
            bgl.baglanti().Close();

            SqlCommand komut2 = new SqlCommand("Select Tahlil_turu From tbl_tahlil", bgl.baglanti());
            SqlDataReader dr2 = komut2.ExecuteReader();
            while (dr2.Read())
            {
                cmbtahlil.Items.Add(dr2[0]);
            }
            bgl.baglanti().Close();

            SqlCommand komut3 = new SqlCommand("select top(1) Muayene_sikayet from tbl_muayene where Hasta_id = ' "+  _idh +" 'order by Muayene_id desc", bgl.baglanti());
            SqlDataReader dr3 = komut3.ExecuteReader();
            while (dr3.Read())
            {
                richTextBox1.Text = dr3[0].ToString();
            }
            bgl.baglanti().Close();

            if (rcthdetay.Text.Equals(string.Empty))
            {
                rcthdetay.Text = "Tahlil Açıklaması";
                rcthdetay.ForeColor = Color.Gray;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panel1_MouseDown_1(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        public string _idt;
        private void cmbtahlil_SelectedValueChanged(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("Select Tahlil_id from tbl_tahlil where Tahlil_turu = '" + cmbtahlil.Text + "'", bgl.baglanti());
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                _idt = dr[0].ToString();
            }
            bgl.baglanti().Close();
        }

        public string _idm;
        
        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand komut2 = new SqlCommand("Select Muayene_id from tbl_muayene", bgl.baglanti());
            SqlDataReader dr2 = komut2.ExecuteReader();
            while (dr2.Read())
            {
                _idm = dr2[0].ToString();
            }
            bgl.baglanti().Close();

            SqlCommand komut = new SqlCommand("insert into tbl_muayene_tahlil (Muayene_id, Tahlil_id, Tahlil_Tarih, Tahlil_detay) values (@d1,@d2,@d3,@d4)", bgl.baglanti());
            komut.Parameters.AddWithValue("@d1", _idm);
            komut.Parameters.AddWithValue("@d2", _idt);
            komut.Parameters.AddWithValue("@d3", DateTime.Now);
            komut.Parameters.AddWithValue("@d4", rcthdetay.Text);
            komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Tahlil İsteği Gönderildi");
        }

        private void rcthdetay_MouseClick(object sender, MouseEventArgs e)
        {
            rcthdetay.Text = " ";
        }
    }
}
