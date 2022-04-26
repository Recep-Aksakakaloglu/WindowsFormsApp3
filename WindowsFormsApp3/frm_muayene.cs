using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApp3
{
    public partial class frm_muayene : Form
    {
        sqlBaglantisi bgl = new sqlBaglantisi();
        public string _idh;
        public frm_muayene(string idh)
        {
            _idh = idh;
            InitializeComponent();
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            this.Hide();
        }
        public string _idm;
        private void frm_muayene_Load(object sender, EventArgs e)
        {

            SqlCommand komut = new SqlCommand("Select * from tbl_hasta where Hasta_id = '" + _idh + "'", bgl.baglanti());
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

            btnRaporYaz.Enabled = false;
            btnReceteYaz.Enabled = false;
            btnTahlilIste.Enabled = false;
        }

        public string _idd;
        
        private void button1_Click(object sender, EventArgs e)
        {
            _idd = AppUser.Doktor_id.ToString();
            SqlCommand komut = new SqlCommand("Insert Into tbl_muayene (Muayene_tani, Muayene_sikayet, Hasta_id, Doktor_id) values (@d1,@d2,@d3,@d4)", bgl.baglanti());
            komut.Parameters.AddWithValue("@d1", rcthtani.Text);
            komut.Parameters.AddWithValue("@d2", rcthsikayet.Text);
            komut.Parameters.AddWithValue("@d3", txthid.Text);
            komut.Parameters.AddWithValue("@d4", _idd);
            komut.ExecuteNonQuery();
            bgl.baglanti().Close();

            SqlCommand komut2 = new SqlCommand("Select top(1) Muayene_tani, Muayene_sikayet from tbl_muayene inner join  tbl_doktor on tbl_doktor.Doktor_id = tbl_muayene.Doktor_id where tbl_muayene.Doktor_id = ' " + _idd + " ' order by tbl_muayene.Muayene_id desc", bgl.baglanti());
            SqlDataReader dr2 = komut2.ExecuteReader();
            while (dr2.Read())
            {
                rcthtani.Text = dr2[0].ToString();
                rcthsikayet.Text = dr2[1].ToString();
            }
            bgl.baglanti().Close();

            btnRaporYaz.Enabled = true;
            btnReceteYaz.Enabled = true;
            btnTahlilIste.Enabled = true;
        }

        private void btnReceteYaz_Click(object sender, EventArgs e)
        {
            frm_recete fr = new frm_recete(txthid.Text);
            fr.Show();
            //SqlCommand komut = new SqlCommand("Insert Into tbl_recete (Hasta_id) values (@p1)");
            //komut.Parameters.AddWithValue("@p1", txthid.Text);
            //komut.ExecuteNonQuery();
            //bgl.baglanti().Close();
        }

        private void btnTahlilIste_Click(object sender, EventArgs e)
        {
            frm_tahlil fr = new frm_tahlil(txthid.Text);
            fr.Show();
        }

        private void btnRaporYaz_Click(object sender, EventArgs e)
        {
            frm_rapor fr = new frm_rapor(txthid.Text);
            fr.Show();
        }
    }
}
