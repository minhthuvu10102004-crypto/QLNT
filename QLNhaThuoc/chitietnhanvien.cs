using DevExpress.XtraCharts.Design;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace QLNhaThuoc
{
    public partial class chitietnhanvien : DevExpress.XtraEditors.XtraForm
    {
        SqlConnection conn = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand();
        DataTable dt = new DataTable();
        string sql, constr;
        string _maMoi;
        public enum Mode { ThemMoi, ChinhSua }

        public Mode FormMode { get; set; }
        public string MaNV { get; set; }
        public chitietnhanvien(Mode mode, string maNV = "")
        {
            InitializeComponent();
            FormMode = mode;
            MaNV = maNV;
        }
        public chitietnhanvien(string maMoi)
        {
            InitializeComponent();
            _maMoi = maMoi;
        }
        public chitietnhanvien()
        {
            InitializeComponent();
        }
        private void BtnCustom_Paint(object sender, PaintEventArgs e)
        {

            Button btn = sender as Button;
            int radius = 20; // bán kính bo góc
            Rectangle rect = new Rectangle(0, 0, btn.Width, btn.Height);

            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseAllFigures();

            btn.Region = new Region(path);

            // Vẽ chữ chính giữa (tùy chọn, thường Button tự canh giữa)
            TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, rect, btn.ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
        private void chitietnhanvien_Load(object sender, EventArgs e)
        {
            if (FormMode == Mode.ThemMoi)
            {
                lblTitle.Text = "THÊM MỚI NHÂN VIÊN";
            }
            else
            {
                lblTitle.Text = "CHỈNH SỬA THÔNG TIN NHÂN VIÊN";
                LoadData(); // nạp dữ liệu của NCC cần sửa
            }
            lblTitle.Left = (this.ClientSize.Width - lblTitle.Width) / 2;
            //cbogender
            cbogender.Properties.Items.Add("Nam");
            cbogender.Properties.Items.Add("Nữ");


            //nút thêm mới
            btnhuy.FlatStyle = FlatStyle.Flat;
            btnhuy.FlatAppearance.BorderSize = 0;
            btnhuy.Margin = new Padding(10);
            //màu mặc định
            btnhuy.BackColor = Color.FromArgb(66, 144, 242);
            btnhuy.ForeColor = Color.White;
            //màu khi di chuột
            btnhuy.MouseEnter += (s, e) =>
            {
                btnhuy.BackColor = Color.FromArgb(118, 173, 243);
            };

            btnhuy.MouseLeave += (s, e) =>
            {
                btnhuy.BackColor = Color.FromArgb(66, 144, 242);
            };
            //màu khi nhấn
            btnhuy.MouseDown += (s, e) =>
            {
                btnhuy.BackColor = Color.FromArgb(40, 116, 240);
            };
            btnhuy.MouseUp += (s, e) =>
            {
                btnhuy.BackColor = Color.FromArgb(118, 173, 243);
            };

            //nút lưu
            btnluu.FlatStyle = FlatStyle.Flat;
            btnluu.FlatAppearance.BorderSize = 0;
            btnluu.Margin = new Padding(10);
            //màu mặc định
            btnluu.BackColor = Color.FromArgb(66, 144, 242);
            btnluu.ForeColor = Color.White;
            //màu khi di chuột
            btnluu.MouseEnter += (s, e) =>
            {
                btnluu.BackColor = Color.FromArgb(118, 173, 243);

            };
            btnluu.MouseLeave += (s, e) =>
            {
                btnluu.BackColor = Color.FromArgb(66, 144, 242);

            };
            //màu khi nhấn
            btnluu.MouseDown += (s, e) =>
            {
                btnluu.BackColor = Color.FromArgb(118, 173, 243);

            };
            date.Properties.Mask.EditMask = "dd/MM/yyyy";
            date.Properties.Mask.UseMaskAsDisplayFormat = true;
            date.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            date.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            date.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            date.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            //
            txtmanv.Text = _maMoi;
            txtmanv.ReadOnly = true;
            txtmanv.Properties.AllowFocused= false;


        }
        private void LoadData()
        {
            string connectionString = @"Data Source=MINHTHUVU\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM NhanVien WHERE MaNV = @MaNV";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNV", MaNV);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtmanv.Text = reader["MaNV"].ToString();
                                txtten.Text = reader["TenNV"].ToString();
                                txtdiachi.Text = reader["DiaChi"].ToString();
                                txtsdt.Text = reader["SDT"].ToString();
                                date.Text = reader["NgaySinh"].ToString();
                                cbogender.Text = reader["GioiTinh"].ToString();
                            }


                            else
                            {
                                MessageBox.Show("Không tìm thấy nhân viên với Mã NV đã cho.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            txtmanv.Enabled = false;
        }
        private void btnhuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnluu_Click(object sender, EventArgs e)
        {
            // 1. Lấy dữ liệu từ các TextBox
            string maNV = txtmanv.Text.Trim();
            string tenNV = txtten.Text.Trim();
            string ngaySinh = date.Text.Trim();
            string gioiTinh = cbogender.Text.Trim();
            string diaChi = txtdiachi.Text.Trim();
            string sdt = txtsdt.Text.Trim();
           
            // 2. Kiểm tra dữ liệu hợp lệ (có thể thêm các kiểm tra khác nếu cần)
            if (string.IsNullOrEmpty(maNV) || string.IsNullOrEmpty(tenNV))
            {
                MessageBox.Show("Mã nhân viên và Tên nhân viên không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 3. Kết nối CSDL
            string connectionString = @"Data Source=MINHTHUVU\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                conn.Open();

                if (FormMode == Mode.ThemMoi)
                {
                    string sql = "INSERT INTO NhanVien (MaNV, TenNV, GioiTinh, NgaySinh, SDT, DiaChi,) VALUES (@MaNV, @TenNV, @GioiTinh, @NgaySinh, @SDT, @DiaChi)";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNV", maNV);
                        cmd.Parameters.AddWithValue("@TenNV", tenNV);
                        cmd.Parameters.AddWithValue("@GioiTinh", gioiTinh);
                        cmd.Parameters.AddWithValue("@NgaySinh", ngaySinh);                      
                        cmd.Parameters.AddWithValue("@SDT", sdt);
                        cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Thêm mới nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else // Chỉnh sửa
                {
                    string sql = "UPDATE NhanVien SET TenNV = @TenNV, GioiTinh = @GioiTinh, NgaySinh = @NgaySinh, SDT = @SDT, DiaChi = @DiaChi WHERE MaNV = @MaNV";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNV", maNV);
                        cmd.Parameters.AddWithValue("@TenNV", tenNV);
                        cmd.Parameters.AddWithValue("@GioiTinh", gioiTinh);
                        cmd.Parameters.AddWithValue("@NgaySinh", ngaySinh);
                        cmd.Parameters.AddWithValue("@SDT", sdt);
                        cmd.Parameters.AddWithValue("@DiaChi", diaChi);                                            
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Cập nhật thông tin nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            this.Close();
        }
    }
}