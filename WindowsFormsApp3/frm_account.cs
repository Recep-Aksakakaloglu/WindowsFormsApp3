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
using System.IO;

namespace WindowsFormsApp3
{
    public partial class frm_account : Form
    {
        public string _id;
        public frm_account(string id)
        {
            InitializeComponent();
            _id = id;
        }
        sqlBaglantisi bgl = new sqlBaglantisi();
        //        private void frm_account_Load(object sender, EventArgs e)
        //        {

        //            SqlCommand komut = new SqlCommand("select Doktor_id, Brans_ad, Unvan_ad, Doktor_ad, Doktor_soyad from tbl_doktor inner join tbl_brans on tbl_doktor.Doktor_brans_id = tbl_brans.Brans_id" +
        //" inner join tbl_unvan on tbl_unvan.Unvan_id = tbl_doktor.Doktor_unvan where tbl_doktor.Doktor_id = '" + _id + "'", bgl.baglanti());
        //            komut.Parameters.AddWithValue("@p1", txtdid.Text);
        //            SqlDataReader dr = komut.ExecuteReader();
        //            while (dr.Read())
        //            {
        //                txtdid.Text = dr[0].ToString();
        //                txtdbrans.Text = dr[1].ToString();
        //                txtdunvan.Text = dr[2].ToString();
        //                txtdad.Text = dr[3].ToString();
        //                txtdsoyad.Text = dr[4].ToString();
        //                pictureBox2.Image = Image.FromStream(dr[5]);
        //            }
        //            bgl.baglanti().Close();
        //        }

        private void frm_account_Load(object sender, EventArgs e)
        {

            SqlCommand komut = new SqlCommand("select Doktor_id, Brans_ad, Unvan_ad, Doktor_ad, Doktor_soyad, Doktor_resim from tbl_doktor inner join tbl_brans on tbl_doktor.Doktor_brans_id = tbl_brans.Brans_id" +
" inner join tbl_unvan on tbl_unvan.Unvan_id = tbl_doktor.Doktor_unvan where tbl_doktor.Doktor_id = '" + _id + "'", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", txtdid.Text);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                txtdid.Text = dr[0].ToString();
                txtdbrans.Text = dr[1].ToString();
                txtdunvan.Text = dr[2].ToString();
                txtdad.Text = dr[3].ToString();
                txtdsoyad.Text = dr[4].ToString();
                pictureBox2.ImageLocation = dr[5].ToString();
            }
            bgl.baglanti().Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ((txtsifre3 != txtsifre2) && (txtsifre == txtsifre3) && (txtsifre == txtsifre2))
            {
                MessageBox.Show("Yeni Şifre ve  Yeni Şifre Tekrar Aynı Olmalı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                SqlCommand komut = new SqlCommand("Update tbl_doktor set Doktor_sifre=@p1 Where Doktor_id= @p2", bgl.baglanti());
                komut.Parameters.AddWithValue("@p1", txtsifre3.Text);
                komut.Parameters.AddWithValue("@p2", _id);
                komut.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Bilgileriniz Güncellendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}
