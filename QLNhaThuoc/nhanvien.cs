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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Data.SqlClient;
namespace QLNhaThuoc
{
    public partial class nhanvien : DevExpress.XtraEditors.XtraForm
    {
        SqlConnection conn = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand();
        DataTable dt = new DataTable();
        string sql, constr;
        private bool userHasSelectedRow = false;

        public nhanvien()
        {
            InitializeComponent();
            

        }
        // Tạo GraphicsPath bo góc
        private GraphicsPath TaoPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        // Bo góc + viền cho control chuẩn WinForms
        private void BoGocVaVien(Control control, int radius, Color borderColor, int borderSize)
        {
            control.Resize += (s, e) =>
            {
                if (control.Width > 0 && control.Height > 0)
                    control.Region = new Region(TaoPath(control.ClientRectangle, radius));
            };

            control.Paint += (s, e) =>
            {
                if (control.Width <= 0 || control.Height <= 0) return;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle rect = control.ClientRectangle;
                rect.Width -= 1;
                rect.Height -= 1;

                using (SolidBrush brush = new SolidBrush(control.BackColor))
                    e.Graphics.FillPath(brush, TaoPath(rect, radius));

                if (borderSize > 0)
                    using (Pen pen = new Pen(borderColor, borderSize))
                        e.Graphics.DrawPath(pen, TaoPath(rect, radius));
            };
        }

        // Căn giữa FlowLayoutPanel
        private void CenterFlowLayoutPanel()
        {
            // căn giữa theo chiều dọc
            flpnv.Top = (pnlnv.ClientSize.Height - flpnv.Height) / 2;
        }
        private void CenterControlsVerticallyInFlowLayoutPanel(FlowLayoutPanel flp)
        {
            foreach (Control ctrl in flp.Controls)
            {
                // Nếu control có chiều cao nhỏ hơn FlowLayoutPanel thì căn giữa nó
                int topMargin = Math.Max(0, (flp.ClientSize.Height - ctrl.Height) / 2);

                // Giữ nguyên margin trái–phải, chỉnh lại top–bottom
                ctrl.Margin = new Padding(ctrl.Margin.Left, topMargin, ctrl.Margin.Right, topMargin);
            }
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


        private void nhanvien_Load(object sender, EventArgs e)
        {
            //gọi dữ liệu
            constr = "Data Source=MINHTHUVU\\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False";
            conn.ConnectionString = constr;
            conn.Open();
            sql = "SELECT * FROM NhanVien";
            da = new SqlDataAdapter(sql, conn);
            da.Fill(dt);
            gridControlnv.DataSource = dt;
            gridControlnv.Refresh();
            this.gridViewnv.MouseUp += GridViewnv_MouseUp;


            //
            pnlnv.Controls.Add(flpnv);
            pnlnv.BackColor = Color.White;
            txttimkiem.Properties.AutoHeight = false;
            txttimkiem.Height = btnloctheo.Height; // Đặt chiều cao của txttimkiem bằng btnloc
            btnloc.Height = btnloctheo.Height;
            pnlnv.Resize += (s, ev) =>
            {
                CenterFlowLayoutPanel();
                CenterControlsVerticallyInFlowLayoutPanel(flpnv);
            };

            flpnv.Resize += (s, ev) =>
            {
                CenterControlsVerticallyInFlowLayoutPanel(flpnv);
            };

            // Căn ngay lúc khởi tạo
            CenterFlowLayoutPanel();
            CenterControlsVerticallyInFlowLayoutPanel(flpnv);
            //
            // nút thêm mới
            btnthem.FlatStyle = FlatStyle.Flat;
            btnthem.FlatAppearance.BorderSize = 0;
            btnthem.Margin = new Padding(10);
            //màu mặc định
            btnthem.BackColor = Color.FromArgb(66, 144, 242);
            btnthem.ForeColor = Color.White;
            //màu khi di chuột
            btnthem.MouseEnter += (s, e) =>
            {
                btnthem.BackColor = Color.FromArgb(118, 173, 243);
            };

            btnthem.MouseLeave += (s, e) =>
            {
                btnthem.BackColor = Color.FromArgb(66, 144, 242);
            };
            //màu khi nhấn
            btnthem.MouseDown += (s, e) =>
            {
                btnthem.BackColor = Color.FromArgb(66,144,242);
            };
            btnthem.MouseUp += (s, e) =>
            {
                btnthem.BackColor = Color.FromArgb(118, 173, 243);
            };
            

            //nút sửa
            btnsua.FlatStyle = FlatStyle.Flat;
            btnsua.FlatAppearance.BorderSize = 0;
            btnsua.Margin = new Padding(10);
            //màu mặc định
            btnsua.BackColor = Color.FromArgb(66, 144, 242);
            btnsua.ForeColor = Color.White;
            //màu khi di chuột
            btnsua.MouseEnter += (s, e) =>
            {
                btnsua.BackColor = Color.FromArgb(118, 173, 243);
            };

            btnsua.MouseLeave += (s, e) =>
            {
                btnsua.BackColor = Color.FromArgb(66, 144, 242);
            };
            //màu khi nhấn
            btnsua.MouseDown += (s, e) =>
            {
                btnsua.BackColor = Color.FromArgb(40, 116, 240);
            };
            btnsua.MouseUp += (s, e) =>
            {
                btnsua.BackColor = Color.FromArgb(118, 173, 243);
            };

            //nút xóa
            btnxoa.FlatStyle = FlatStyle.Flat;
            btnxoa.FlatAppearance.BorderSize = 0;
            btnxoa.Margin = new Padding(10);
            //màu mặc định
            btnxoa.BackColor = Color.FromArgb(66, 144, 242);
            btnxoa.ForeColor = Color.White;
            //màu khi di chuột
            btnxoa.MouseEnter += (s, e) =>
            {
                btnxoa.BackColor = Color.FromArgb(118, 173, 243);
            };
            btnxoa.MouseLeave += (s, e) =>
            {
                btnxoa.BackColor = Color.FromArgb(66, 144, 242);
            };
            //màu khi nhấn
            btnxoa.MouseDown += (s, e) =>
            {
                btnxoa.BackColor = Color.FromArgb(40, 116, 240);
            };
            btnxoa.MouseUp += (s, e) =>
            {
                btnxoa.BackColor = Color.FromArgb(118, 173, 243);
            };

            //
            txttimkiem.Properties.NullValuePrompt = "Nhập mã nhân viên và nhấn enter để tìm kiếm";
            txttimkiem.Properties.NullValuePromptShowForEmptyValue = true;
            txttimkiem.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            txttimkiem.BackColor = Color.White;
            BoGocVaVien(txttimkiem, 12, Color.DarkGray, 1);
            //
            btnloc.LookAndFeel.UseDefaultLookAndFeel = false;
            btnloc.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            btnloc.Text = "Lọc";
            btnloc.AllowFocus = false;

            btnloc.ImageOptions.SvgImage = Properties.Resources.filter;
            btnloc.ImageOptions.SvgImageSize = new Size(12, 12);
            btnloc.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            btnloc.ImageOptions.ImageToTextIndent = 10;

            btnloc.Appearance.BackColor = Color.White;
            btnloc.Appearance.ForeColor = Color.Black;
            btnloc.Appearance.Options.UseBackColor = true;
            btnloc.Appearance.Options.UseForeColor = true;
            btnloc.Margin = new Padding(5, 10, 1300, 0);
            //set hiệu ứng chuột
            //màu khi di chuột
            btnloc.MouseEnter += (s, e) =>
            {
                btnloc.Appearance.BackColor = Color.FromArgb(240, 240, 240);

            };
            btnloc.MouseLeave += (s, e) =>
            {
                btnloc.Appearance.BackColor = Color.White;
            };
            //màu khi nhấn
            btnloc.MouseDown += (s, e) =>
            {
                btnloc.Appearance.BackColor = Color.FromArgb(220, 220, 220);
            };
            btnloc.MouseUp += (s, e) =>
            {
                btnloc.Appearance.BackColor = Color.FromArgb(240, 240, 240);
            };
            btnloctheo.AllowFocus = false;
            btnloctheo.ImageOptions.SvgImage = Properties.Resources.settings;
            btnloctheo.ImageOptions.SvgImageSize = new Size(12, 12);
            btnloctheo.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.None;
            btnloctheo.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            btnloctheo.Text = "";
            //màu khi di chuột
            btnloctheo.MouseEnter += (s, e) =>
            {
                btnloctheo.Appearance.BackColor = Color.FromArgb(240, 240, 240);
            };
            btnloctheo.MouseLeave += (s, e) =>
            {
                btnloctheo.Appearance.BackColor = Color.White;
            };
            //màu khi nhấn
            btnloctheo.MouseDown += (s, e) =>
            {
                btnloctheo.Appearance.BackColor = Color.FromArgb(220, 220, 220);
            };
            btnloctheo.MouseUp += (s, e) =>
            {
                btnloctheo.Appearance.BackColor = Color.FromArgb(240, 240, 240);
            };
            //
            btntailai.AllowFocus = false;
            btntailai.ImageOptions.SvgImage = Properties.Resources.refresh;
            btntailai.ImageOptions.SvgImageSize = new Size(12, 12);
            btntailai.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.None;
            btntailai.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            btntailai.Text = "";
            //màu khi di chuột
            btntailai.MouseEnter += (s, e) =>
            {
                btntailai.Appearance.BackColor = Color.FromArgb(229, 238, 238);
            };
            btntailai.MouseLeave += (s, e) =>
            {
                btntailai.Appearance.BackColor = Color.White;
            };
            //màu khi nhấn
            btntailai.MouseDown += (s, e) =>
            {
                btntailai.Appearance.BackColor = Color.FromArgb(229, 238, 238);
            };
            btntailai.MouseUp += (s, e) =>
            {
                btntailai.Appearance.BackColor = Color.FromArgb(240, 240, 240);
            };
            //bo góc + viền
            BoGocVaVien(btnloc, 12, Color.DarkGray, 1);
            btnloctheo.LookAndFeel.UseDefaultLookAndFeel = false;
            btnloctheo.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            BoGocVaVien(btnloctheo, 12, Color.DarkGray, 1);
            btntailai.LookAndFeel.UseDefaultLookAndFeel = false;
            btntailai.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            BoGocVaVien(btntailai, 12, Color.DarkGray, 1);
            //
            //Cấu hình chọn dòng bằng checkbox-- -
            gridViewnv.OptionsSelection.MultiSelect = true;
            gridViewnv.OptionsSelection.MultiSelectMode =
                DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;

            // Hiển thị ô vuông ở header để chọn tất cả
            gridViewnv.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DevExpress.Utils.DefaultBoolean.True;

            // Ẩn cột indicator (số thứ tự dòng) để checkbox ra ngoài cùng
            gridViewnv.OptionsView.ShowIndicator = false;

            // --- Cấu hình giao diện ---
            gridViewnv.OptionsView.ShowGroupPanel = false;
            gridViewnv.OptionsView.EnableAppearanceEvenRow = true;
            gridViewnv.Appearance.HeaderPanel.Font = new Font("Arial", 10, FontStyle.Bold);
            gridViewnv.Appearance.Row.Font = new Font("Arial", 10);
            gridViewnv.RowHeight = 28;
            gridViewnv.OptionsView.ShowIndicator = false;
            
            // 
            gridViewnv.OptionsBehavior.Editable = false;
            gridViewnv.OptionsBehavior.ReadOnly = true;
            gridViewnv.OptionsSelection.EnableAppearanceFocusedCell = false;

            gridViewnv.Columns["MaNV"].Caption = "Mã nhân viên";
            gridViewnv.Columns["TenNV"].Caption = "Tên nhân viên";
            gridViewnv.Columns["GioiTinh"].Caption = "Giới tính";
            gridViewnv.Columns["NgaySinh"].Caption = "Ngày sinh";
            gridViewnv.Columns["SDT"].Caption = "Số điện thoại";
            gridViewnv.Columns["DiaChi"].Caption = "Địa chỉ";
           
            gridViewnv.Appearance.Row.Font = new Font("Arial", 10);
            gridViewnv.Appearance.HeaderPanel.Font = new Font("Arial", 10, FontStyle.Bold);
            BuildPopupContent();
            
        }
        private void BuildPopupContent()
        {
            // Xóa nội dung cũ (an toàn để gọi lại)
            popuploctheo.Controls.Clear();

            // Title
            var lblTitle = new LabelControl
            {
                Text = "Tìm kiếm theo",
                AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None,
                Location = new Point((popuploctheo.Width - 210) / 2, 10)
            };
            lblTitle.Appearance.Font = new Font("Arial", 12, FontStyle.Bold);
            lblTitle.Width = 250;
            popuploctheo.Controls.Add(lblTitle);
            //
            string[] options =
            {

             "Mã nhân viên",
             "Tên nhân viên",
             "Khu vực"
            };

            int y = 45;
            foreach (var text in options)
            {
                CheckEdit chk = new CheckEdit
                {
                    Text = text,
                    Location = new Point(12, y),
                    Width = 220
                };
                chk.Properties.Appearance.Font = new Font("Arial", 8, FontStyle.Regular);
                chk.Properties.Appearance.Options.UseFont = true;

                // Tick mặc định
                if (text == "Mã nhân viên")
                    chk.Checked = true;

                // Khi thay đổi trạng thái CheckEdit thì gọi lọc ngay
                chk.CheckedChanged += (s, e) =>
                {
                    // Bỏ tick ở tất cả các CheckEdit khác để chỉ chọn 1 tiêu chí
                    foreach (var control in popuploctheo.Controls.OfType<CheckEdit>())
                    {
                        if (control != chk)
                            control.Checked = false;
                    }                   
                };

                popuploctheo.Controls.Add(chk);
                y += 30;
            }
            // Đường kẻ nhẹ ngăn cách
            Panel line = new Panel
            {
                BackColor = Color.FromArgb(230, 230, 230),
                Height = 1,
                Width = 240,
                Location = new Point(10, y - 6)
            };
            popuploctheo.Controls.Add(line);

            
            popuploctheo.Size = new Size(270, y + 20);
        }
        

        private void btnloctheo_Click(object sender, EventArgs e)
        {
            int offsetY = 80; 
            int offsetX = 6;  

            pceloctheo.Location = new Point(btnloctheo.Left + offsetX, btnloctheo.Bottom + offsetY);
            pceloctheo.ShowPopup();
        }

        private void btnloc_Click(object sender, EventArgs e)
        {

            string keyword = txttimkiem.Text.Trim();
            List<string> filters = new List<string>();

            foreach (Control control in popuploctheo.Controls)
            {
                if (control is CheckEdit chk && chk.Checked)
                {
                    switch (chk.Text)
                    {
                        case "Mã nhân viên":
                            filters.Add($"[MaNV] LIKE '%{keyword}%'");
                            break;

                        case "Tên nhân viên":
                            filters.Add($"[TenNV] LIKE '%{keyword}%'");
                            break;
                        case "Khu vực":
                            filters.Add($"[DiaChi] LIKE '%{keyword}%'");
                            break;
                    }
                }
            }
            // Nếu có ít nhất một tiêu chí được chọn → tạo chuỗi lọc OR
            if (filters.Count > 0)
                gridViewnv.ActiveFilterString = string.Join(" OR ", filters);
            else
                gridViewnv.ActiveFilterString = string.Empty; // Bỏ lọc nếu không chọn gì
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=MINHTHUVU\\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False";

            string maMoi = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // Lấy mã NCC lớn nhất hiện có
                string query = "SELECT TOP 1 MaNV FROM NhanVien ORDER BY MaNV DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                object result = cmd.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                {
                    maMoi = "NV01";
                }
                else
                {
                    string maCu = result.ToString(); 
                    int so = int.Parse(maCu.Substring(3)) + 1; 
                    maMoi = "NV" + so.ToString("D3"); 
                }
            }

            // Gọi form chi tiết và truyền mã mới
            chitietnhanvien frm = new chitietnhanvien(maMoi);
            frm.ShowDialog();


            // Sau khi đóng form chi tiết thì refresh lại danh sách
            LoadNhanVienData();

        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem người dùng đã click chọn dòng chưa
            if (!userHasSelectedRow)
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int[] selectedRows = gridViewnv.GetSelectedRows();
            int rowHandle = -1;

            // Nếu có dòng được tick (checkbox)
            if (selectedRows != null && selectedRows.Length > 0)
            {
                if (selectedRows.Length > 1)
                {
                    MessageBox.Show("Chỉ được chọn 1 nhân viên để chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                rowHandle = selectedRows[0];
            }
            else
            {
                // Nếu không tick mà chỉ click vào dòng
                rowHandle = gridViewnv.FocusedRowHandle;
            }

            if (rowHandle < 0)
            {
                MessageBox.Show("Không tìm thấy dòng hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maNV = gridViewnv.GetRowCellValue(rowHandle, "MaNV")?.ToString();
            if (string.IsNullOrEmpty(maNV))
            {
                MessageBox.Show("Không thể lấy thông tin nhân viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Mở form chi tiết
            chitietnhanvien form = new chitietnhanvien(chitietnhanvien.Mode.ChinhSua, maNV);
            form.ShowDialog();

            // Reset lại biến khi reload
            userHasSelectedRow = false;
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            if (!userHasSelectedRow)
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int[] selectedRows = gridViewnv.GetSelectedRows();
            int rowHandle = -1;

            // Nếu tick nhiều dòng
            if (selectedRows != null && selectedRows.Length > 1)
            {
                MessageBox.Show("Chỉ được xóa 1 nhân viên một lần!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedRows != null && selectedRows.Length == 1)
            {
                rowHandle = selectedRows[0];
            }
            else
            {
                rowHandle = gridViewnv.FocusedRowHandle;
            }

            if (rowHandle < 0)
            {
                MessageBox.Show("Không tìm thấy dòng hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string maNV = gridViewnv.GetRowCellValue(rowHandle, "MaNV")?.ToString();
            if (string.IsNullOrEmpty(maNV))
            {
                MessageBox.Show("Không thể lấy thông tin nhân viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Xác nhận xóa
            DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa nhân viên [{maNV}] không?",
                                                  "Xác nhận xóa",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    string connectionString = @"Data Source=MINHTHUVU\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False";
                    string query = "DELETE FROM NhanVien WHERE MaNV = @MaNV";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNV", maNV);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Xóa nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reload dữ liệu
                    LoadNhanVienData();
                    userHasSelectedRow = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void LoadNhanVienData()
        {
            string connectionString = @"Data Source=MINHTHUVU\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False";
            string query = "SELECT * FROM NhanVien";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                gridControlnv.DataSource = dt;
                

            }
        }

        private void btntailai_Click(object sender, EventArgs e)
        {
            // Xóa điều kiện lọc trong GridView
            gridViewnv.ActiveFilterString = "";
            gridViewnv.ClearColumnsFilter();

            // Xóa nội dung trong ô tìm kiếm
            txttimkiem.Text = string.Empty;

            // Đặt lại gợi ý mặc định (nếu cần)
            txttimkiem.Properties.NullValuePrompt = "Nhập mã nhân viên và nhấn enter để tìm kiếm";
            LoadNhanVienData();
        }
        private void gridViewnv_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            var view = sender as DevExpress.XtraGrid.Views.Grid.GridView;

            // Nếu dòng này được chọn (tick checkbox)
            if (view.IsRowSelected(e.RowHandle))
            {
                e.Appearance.BackColor = Color.FromArgb(118, 173, 243); 
                e.Appearance.ForeColor = Color.Black;
                e.HighPriority = true; 
            }
        }
        private void GridViewnv_MouseUp(object sender, MouseEventArgs e)
        {
            var view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            if (view == null) return;

            var hit = view.CalcHitInfo(e.Location);
            if (hit.InRow || hit.InRowCell)
            {
                userHasSelectedRow = true; // người dùng đã click chọn dòng
            }
        }




    }
}

