using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QLNhaThuoc
{
    public partial class frmHoaDonBH : Form
    {
        // thay bằng connection string thật
        private string _maHoaDon;
        private string connectionString = @"ata Source=MINHTHUVU\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False";

        public frmHoaDonBH(string maHoaDon)
        {
            InitializeComponent();
            _maHoaDon = maHoaDon;
        }

        private void frmHoaDonBH_Load(object sender, EventArgs e)
        {
            tsslMaHD.Text = "Mã hóa đơn: " + _maHoaDon;
            HienThongTinHoaDon();
            HienChiTietHoaDon();
        }

        private void HienThongTinHoaDon()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT hd.MaHoaDon, hd.NgayLap, hd.TongTien, hd.PTTT,
                           kh.TenKH, kh.SDT, kh.DiaChi, nv.TenNV
                    FROM HoaDon hd
                    INNER JOIN KhachHang kh ON hd.MaKH = kh.MaKH
	        INNER JOIN NhanVien nv ON hd.MaNV = nv.MaNV	
                    WHERE hd.MaHoaDon = @maHD";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@maHD", _maHoaDon);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    lblTenKH.Text = reader["TenKH"].ToString();
                    lblSDT.Text = reader["SDT"].ToString();
                    lbldiachi.Text = reader["DiaChi"].ToString();
                    lblPTTT.Text = reader["PTTT"].ToString();
                    lblTongTien.Text = reader["TongTien"].ToString() + " VNĐ";

                    // check tên của status strip 
                    //lblMaHD = tsslMaHD 
                    //toolStripStatusLabel4 = tsslNgayLap
                    //label10 = tblTong
                    //label12 = lblDiaChi
                    //label5 = lblTenKH
                    //label7 = lblSDT
                    //label8 = lblPTTT

                    tsslMaHD.Text = _maHoaDon;
                    tsslNgayLap.Text = Convert.ToDateTime(reader["NgayLap"]).ToString("dd/MM/yyyy HH:mm");
                    tsslNhanVien.Text = reader["TenNV"].ToString();

                }
                reader.Close();
            }
        }

        private void HienChiTietHoaDon()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT t.TenThuoc, ct.SoLuong, ct.DonGia, (ct.SoLuong * ct.DonGia) AS ThanhTien
                    FROM ChiTietHoaDon ct
                    INNER JOIN Thuoc t ON ct.MaThuoc = t.MaThuoc
                    WHERE ct.MaHoaDon = @maHD";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@maHD", _maHoaDon);

                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvChiTietHD.DataSource = dt;
            }
        }
    }
}


