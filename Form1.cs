using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Bai4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private Dictionary<string,CHocSinh> dsHS=new Dictionary<string,CHocSinh>();
        private void HienThi()
        {
            BindingSource bs= new BindingSource();
            bs.DataSource = dsHS.Values;
            dgvHS.DataSource = bs;
        }
        private void clear()
        {
            txtMSHS.Text = "";
            txtHoTen.Text = "";
            dtpNgaySinh.Value = DateTime.Today;
            rbtNam.Checked = false;
            txtDiaChi.Text = "";
        }
        private CHocSinh findHS(string ma)
        {
            try
            {
                return dsHS[ma];
            }catch { return null; }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            CHocSinh n=new CHocSinh();
            n.MSHS=txtMSHS.Text;
            n.HoTen=txtHoTen.Text;
            n.NgaySinh = dtpNgaySinh.Value;
            n.Phai = rbtNam.Checked;
            n.DiaChi=txtDiaChi.Text;
            if(findHS(n.MSHS)==null )
            {
                dsHS.Add(n.MSHS, n);
                HienThi();
                clear();
            }
        }


        private bool LuuFile(string filename)
        {
            try
            {
                FileStream fs = new FileStream(filename, FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, dsHS);
                fs.Close();
                return true;
            }catch(Exception) { return false; }
        }
        private bool MoFile(string filename)
        {
            try
            {
                FileStream fs = new FileStream(filename, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                dsHS=(Dictionary<string,CHocSinh>) bf.Deserialize(fs);
                fs.Close();
                return true;
            }
            catch (Exception) { return false; }
        }

        private void btnMoFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg=new OpenFileDialog();
            dlg.ShowDialog();
            if (dlg.FileName != "")
            {
                if (MoFile(dlg.FileName))
                {
                    HienThi();
                    MessageBox.Show("Mở file thành công");
                }
                else
                    MessageBox.Show("Mở file thất bại");
            }
        }

        private void btnLuuFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg=new SaveFileDialog();
            dlg.ShowDialog();
            if(dlg.FileName != "")
            {
                if (LuuFile(dlg.FileName))
                    MessageBox.Show("Lưu file thành công");
                else
                    MessageBox.Show("Lưu file thất bại");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if(dgvHS.SelectedRows.Count > 0)
            {
                int index = dgvHS.SelectedRows[0].Index;
                string mahs = dgvHS.Rows[index].Cells[0].Value.ToString();
                dsHS.Remove(mahs);
                HienThi();
            }
        }

        private void dgvHS_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            string mahs = dgvHS.Rows[e.RowIndex].Cells[0].Value.ToString();
            CHocSinh hs= dsHS[mahs];
            txtMSHS.Text = hs.MSHS;
            txtHoTen.Text = hs.HoTen;
            dtpNgaySinh.Value= hs.NgaySinh;
            rbtNam.Checked = hs.Phai;
            txtDiaChi.Text = hs.DiaChi;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvHS.SelectedRows.Count > 0)
            {
                int index = dgvHS.SelectedRows[0].Index;
                string mshs = dgvHS.Rows[index].Cells[0].Value.ToString();

                CHocSinh hs = dsHS[mshs];
                hs.HoTen=txtHoTen.Text;
                hs.NgaySinh = dtpNgaySinh.Value;
                hs.Phai=rbtNam.Checked;
                hs.DiaChi = txtDiaChi.Text;
                HienThi();
            }
        }
    }
}
