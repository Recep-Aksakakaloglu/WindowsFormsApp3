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
    public partial class frm_gecmis_tahliller : Form
    {
        public string _idh;
        public frm_gecmis_tahliller(string idh)
        {
            _idh = idh;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        sqlBaglantisi bgl = new sqlBaglantisi();
        private void frm_gecmis_tahliller_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select tbl_muayene_tahlil.Tahlil_tarih, tbl_tahlil.Tahlil_turu, tbl_muayene_tahlil.Tahlil_detay,  tbl_muayene.Muayene_sikayet,tbl_muayene.Muayene_tani " +
            "from tbl_muayene_tahlil inner join tbl_tahlil on tbl_tahlil.Tahlil_id = tbl_muayene_tahlil.Tahlil_id inner " + "join tbl_muayene on tbl_muayene.Muayene_id = tbl_muayene_tahlil.Muayene_id where tbl_muayene.Hasta_id = '" + _idh + "'", bgl.baglanti());
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            bgl.baglanti().Close();
        }
    }
}
