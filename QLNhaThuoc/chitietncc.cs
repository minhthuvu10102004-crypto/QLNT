using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QLNhaThuoc
{
   
    public partial class chitietncc : DevExpress.XtraEditors.XtraForm

    {
        SqlConnection conn = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand();
        DataTable dt = new DataTable();
        string sql, constr;
        private string _maMoi;
        public enum Mode { ThemMoi, ChinhSua }

        public Mode FormMode { get; set; }
        public string MaNCC { get; set; }
        public chitietncc(string maMoi)
        {
            InitializeComponent();
            _maMoi = maMoi;
        }
        public chitietncc(Mode mode, string maNCC = "")
        {
            InitializeComponent();
            FormMode = mode;
            MaNCC = maNCC;
           
        }
        public chitietncc()
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
        private void chitietncc_Load(object sender, EventArgs e)
        {

            if (FormMode == Mode.ThemMoi)
            {
                lblTitle.Text = "THÊM MỚI NHÀ CUNG CẤP";
            }
            else
            {
                lblTitle.Text = "CHỈNH SỬA NHÀ CUNG CẤP";
                LoadData(); // nạp dữ liệu của NCC cần sửa
            }
            lblTitle.Left = (this.ClientSize.Width - lblTitle.Width) / 2;


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
                btnluu.BackColor = Color.FromArgb(40, 116, 240);
            };
            btnluu.MouseUp += (s, e) =>
            {
                btnluu.BackColor = Color.FromArgb(118, 173, 243);
            };
            //
            txtmancc.Text = _maMoi;
            txtmancc.ReadOnly = true;
            txtmancc.Properties.AllowFocused = false;


        }
        private void LoadData()
        {
            string connectionString = @"Data Source=MINHTHUVU\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM NhaCungCap WHERE MaNCC = @MaNCC";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNCC", MaNCC);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtmancc.Text = reader["MaNCC"].ToString();
                                txtten.Text = reader["TenNCC"].ToString();
                                txtdiachi.Text = reader["DiaChi"].ToString();
                                txtsdt.Text = reader["SDT"].ToString();
                                txtemail.Text = reader["Email"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy nhà cung cấp với Mã NCC đã cho.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            txtmancc.Enabled = false;
        }
        private void btnhuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnluu_Click(object sender, EventArgs e)
        {
            // 1. Lấy dữ liệu từ các TextBox
            string maNCC = txtmancc.Text.Trim();
            string tenNCC = txtten.Text.Trim();
            string diaChi = txtdiachi.Text.Trim();
            string sdt = txtsdt.Text.Trim();
            string email = txtemail.Text.Trim();
            // 2. Kiểm tra dữ liệu hợp lệ (có thể thêm các kiểm tra khác nếu cần)
            if (string.IsNullOrEmpty(maNCC) || string.IsNullOrEmpty(tenNCC))
            {
                MessageBox.Show("Mã nhà cung cấp và Tên nhà cung cấp không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 3. Kết nối CSDL
            string connectionString = @"Data Source=MINHTHUVU\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                
                    conn.Open();

                    if (FormMode == Mode.ThemMoi)
                    {
                        string sql = "INSERT INTO NhaCungCap (MaNCC, TenNCC, DiaChi, SDT, Email) VALUES (@MaNCC, @TenNCC, @DiaChi, @SDT, @Email)";
                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@MaNCC", maNCC);
                            cmd.Parameters.AddWithValue("@TenNCC", tenNCC);
                            cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                            cmd.Parameters.AddWithValue("@SDT", sdt);
                            cmd.Parameters.AddWithValue("@Email", email);
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Thêm mới nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else // Chỉnh sửa
                    {
                        string sql = "UPDATE NhaCungCap SET TenNCC = @TenNCC, DiaChi = @DiaChi, SDT = @SDT, Email = @Email WHERE MaNCC = @MaNCC";
                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@MaNCC", maNCC);
                            cmd.Parameters.AddWithValue("@TenNCC", tenNCC);
                            cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                            cmd.Parameters.AddWithValue("@SDT", sdt);
                            cmd.Parameters.AddWithValue("@Email", email);
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Cập nhật thông tin nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            this.Close();

        }
    }
}


    
