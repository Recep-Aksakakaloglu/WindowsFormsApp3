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
using System.Collections;
using System.Runtime.InteropServices;

namespace WindowsFormsApp3
{
    public partial class frm_recete : Form
    {
        public string _idh;
        public string _idr;
        public string _idi;
        public frm_recete(string idh)
        {
            _idh = idh;
            InitializeComponent();
        }
        sqlBaglantisi bgl = new sqlBaglantisi();
        private void frm_recete_Load(object sender, EventArgs e)
        {
            comboBox1.Text = "İlaç Seçiniz";

            SqlCommand komut3 = new SqlCommand("Insert Into tbl_recete (Hasta_id) values (@p1)", bgl.baglanti());
            komut3.Parameters.AddWithValue("@p1", _idh);
            komut3.ExecuteNonQuery();
            bgl.baglanti().Close();

            SqlCommand komut4 = new SqlCommand("Select Recete_id from tbl_recete", bgl.baglanti());
            SqlDataReader dr4 = komut4.ExecuteReader();
            while (dr4.Read())
            {
                _idr = dr4[0].ToString();
            }
            bgl.baglanti().Close();

            SqlCommand komut2 = new SqlCommand("Select Ilac_ad From tbl_ilac", bgl.baglanti());
            SqlDataReader dr2 = komut2.ExecuteReader();
            while (dr2.Read())
            {
                comboBox1.Items.Add(dr2[0]);
            }
            bgl.baglanti().Close();

            SqlCommand komut = new SqlCommand("Select Muayene_tani from tbl_muayene where Hasta_id = '" + _idh + "'", bgl.baglanti());
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                richsonuc.Text = dr[0].ToString();
            }
            bgl.baglanti().Close();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            comboBox1.ForeColor = Color.Black;
            SqlCommand komut = new SqlCommand("Select Ilac_id from tbl_ilac where Ilac_ad = '" + comboBox1.Text + "'", bgl.baglanti());
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                _idi = dr[0].ToString();
            }
            bgl.baglanti().Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            recete_id.Recete_id = _idr;
            SqlCommand komutkaydet = new SqlCommand("insert into tbl_recete_ilac(Recete_id, Ilac_id, Ilac_ad) values (@r1,@r2,@r3)", bgl.baglanti());
            komutkaydet.Parameters.AddWithValue("@r1", recete_id.Recete_id);
            komutkaydet.Parameters.AddWithValue("@r2", _idi);
            komutkaydet.Parameters.AddWithValue("@r3", comboBox1.Text);
            komutkaydet.ExecuteNonQuery();
            bgl.baglanti().Close();

            listrecete.Items.Clear();

            SqlCommand komut2 = new SqlCommand("Select Ilac_ad from tbl_recete_ilac where Recete_id = '" + recete_id.Recete_id + "'", bgl.baglanti());
            SqlDataReader dr2 = komut2.ExecuteReader();
            while (dr2.Read())
            {
                listrecete.Items.Add(dr2[0].ToString());
            }
            bgl.baglanti().Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
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

        public string _iaa;
        private void listrecete_Click(object sender, EventArgs e)
        {
            _iaa = listrecete.SelectedItem.ToString();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("Delete from tbl_recete_ilac where Recete_id = @recete_adi AND Ilac_ad = @ilac_adi", bgl.baglanti());
            komut.Parameters.AddWithValue("@recete_adi", _idr);
            komut.Parameters.AddWithValue("@ilac_adi", _iaa);
            int s = komut.ExecuteNonQuery();
            bgl.baglanti().Close();

            listrecete.Items.Clear();

            SqlCommand komut2 = new SqlCommand("Select Ilac_ad from tbl_recete_ilac where Recete_id = '" + recete_id.Recete_id + "'", bgl.baglanti());
            SqlDataReader dr2 = komut2.ExecuteReader();
            while (dr2.Read())
            {
                listrecete.Items.Add(dr2[0].ToString());
            }
            bgl.baglanti().Close();
        }
    }
}
