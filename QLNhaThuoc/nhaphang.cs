using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;


namespace QLNhaThuoc
{
    public partial class nhaphang : XtraForm
    {
        SqlConnection conn = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand();
        private bool isProcessingChange = false;
        private DataTable dt;
        private bool isHandling = false;
        string sql, constr;
        SimpleButton btnApply;
        SimpleButton btnCancel;
        SimpleButton btnMacDinh;
        SimpleButton btnApDung;
        private Timer timer;
        private bool userHasSelectedRow = false;

        public nhaphang()
        {
            InitializeComponent();
            this.Load += nhaphang_Load;
            this.btntailai.Click += btntailai_Click;

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
            flpnhaphang.Top = (pnlnhaphang.ClientSize.Height - flpnhaphang.Height) / 2;
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



        private void nhaphang_Load(object sender, EventArgs e)
        {
            string constr = "Data Source=MINHTHUVU\\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False";
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                string sql = "SELECT p.MaPhieuNhap, c.TenNCC, n.TenNV, p.NgayNhap, p.TongTien, t.TenPTTT, p.TrangThai FROM PhieuNhap p JOIN NhaCungCap c ON p.MaNCC = c.MaNCC JOIN NhanVien n ON p.MaNV = n.MaNV JOIN PhuongThucTT t ON p.MaPTTT = t.MaPTTT";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                dt = new DataTable();
                da.Fill(dt);
                gridControlnhaphang.DataSource = dt;
            }
            this.gridViewnhaphang.MouseUp += GridViewnhaphang_MouseUp;
            
            txttimkiem.Properties.AutoHeight = false;

            pnlnhaphang.Resize += (s, ev) =>
            {
                CenterFlowLayoutPanel();
                CenterControlsVerticallyInFlowLayoutPanel(flpnhaphang);
            };

            flpnhaphang.Resize += (s, ev) =>
            {
                CenterControlsVerticallyInFlowLayoutPanel(flpnhaphang);
            };

            // Căn ngay lúc khởi tạo
            CenterFlowLayoutPanel();
            CenterControlsVerticallyInFlowLayoutPanel(flpnhaphang);
            CenterControlsVerticallyInFlowLayoutPanel(flpnhaphang);
            lblthoigian.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            lblthoigian.Width = 1250;
            btntailai.Margin = new Padding(0, 9, 5, 0);
            btncaidat.Margin = new Padding(5, 9, 5, 0);
            btnloc.Margin = new Padding(5, 10, 20, 0);
            btnloc.Margin = new Padding(5, 10, 20, 0);

            //nút thêm mới
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
                btnthem.BackColor = Color.FromArgb(40, 116, 240);
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

            // độ rộng txttimkiem
            txttimkiem.Height = btnloc.Height;



            // === TextEdit Tìm Kiếm ===
            txttimkiem.Properties.NullValuePrompt = "Nhập số phiếu và nhấn enter để tìm kiếm";
            txttimkiem.Properties.NullValuePromptShowForEmptyValue = true;
            txttimkiem.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            txttimkiem.BackColor = Color.White;
            BoGocVaVien(txttimkiem, 12, Color.DarkGray, 1);

            // === Panel nền ===
            pnlnhaphang.Appearance.BackColor = Color.White;
            pnlnhaphang.Appearance.Options.UseBackColor = true;

            // === Button Lọc ===
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

            //btn tải lại
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

            //nút cài đặt
            btncaidat.AllowFocus = false;
            btncaidat.ImageOptions.SvgImage = Properties.Resources.settings;
            btncaidat.ImageOptions.SvgImageSize = new Size(12, 12);
            btncaidat.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.None;
            btncaidat.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            btncaidat.Text = "";
            //màu khi di chuột
            btncaidat.MouseEnter += (s, e) =>
            {
                btncaidat.Appearance.BackColor = Color.FromArgb(229, 238, 238);
            };
            btncaidat.MouseLeave += (s, e) =>
            {
                btncaidat.Appearance.BackColor = Color.White;
            };
            //màu khi nhấn
            btncaidat.MouseDown += (s, e) =>
            {
                btncaidat.Appearance.BackColor = Color.FromArgb(229, 238, 238);
            };
            btncaidat.MouseUp += (s, e) =>
            {
                btncaidat.Appearance.BackColor = Color.FromArgb(240, 240, 240);
            };
            lblthoigian.Height = btnloc.Height;

            //bo góc + viền
            BoGocVaVien(btnloc, 12, Color.DarkGray, 1);
            btntailai.LookAndFeel.UseDefaultLookAndFeel = false;
            btntailai.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            BoGocVaVien(btntailai, 12, Color.DarkGray, 1);

            btncaidat.LookAndFeel.UseDefaultLookAndFeel = false;
            btncaidat.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            BoGocVaVien(btncaidat, 12, Color.DarkGray, 1);


            //Cấu hình chọn dòng bằng checkbox-- -
            gridViewnhaphang.OptionsSelection.MultiSelect = true;
            gridViewnhaphang.OptionsSelection.MultiSelectMode =
                DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;

            // Hiển thị ô vuông ở header để chọn tất cả
            gridViewnhaphang.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DevExpress.Utils.DefaultBoolean.True;

            // Ẩn cột indicator (số thứ tự dòng) để checkbox ra ngoài cùng
            gridViewnhaphang.OptionsView.ShowIndicator = false;

            // --- Cấu hình giao diện ---
            gridViewnhaphang.OptionsView.ShowGroupPanel = false;
            gridViewnhaphang.OptionsView.EnableAppearanceEvenRow = true;
            gridViewnhaphang.Appearance.HeaderPanel.Font = new Font("Arial", 10, FontStyle.Bold);
            gridViewnhaphang.Appearance.Row.Font = new Font("Arial", 10);
            gridViewnhaphang.RowHeight = 28;
            gridViewnhaphang.OptionsView.ShowIndicator = false;
            //
            gridViewnhaphang.OptionsBehavior.Editable = true; // Cho phép chỉnh
            gridViewnhaphang.OptionsBehavior.ReadOnly = false;
            // Chỉ cột TrạngThái cho phép sửa
            foreach (DevExpress.XtraGrid.Columns.GridColumn col in gridViewnhaphang.Columns)
            {
                if (col.FieldName != "TrangThai")
                    col.OptionsColumn.AllowEdit = false;
            }
            gridViewnhaphang.Columns["TrangThai"].OptionsColumn.AllowEdit = true;

            gridViewnhaphang.OptionsSelection.EnableAppearanceFocusedCell = false;
           
            gridViewnhaphang.Columns["MaPhieuNhap"].Caption = "Mã phiếu nhập";
            gridViewnhaphang.Columns["TenNCC"].Caption = "Tên nhà cung cấp";
            gridViewnhaphang.Columns["TenNV"].Caption = "Tên nhân viên";
            gridViewnhaphang.Columns["NgayNhap"].Caption = "Ngày nhập";
            gridViewnhaphang.Columns["TongTien"].Caption = "Tổng tiền";
            gridViewnhaphang.Columns["TenPTTT"].Caption = "Phương thức thanh toán";
            gridViewnhaphang.Columns["TongTien"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            gridViewnhaphang.Columns["TrangThai"].Caption = "Trạng thái";
            gridViewnhaphang.Columns["TrangThai"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            // Tạo combo box cho cột Trạng thái
            var repoTrangThai = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            repoTrangThai.Items.Add("Nháp");
            repoTrangThai.Items.Add("Duyệt");
            //thay đổi font combo box
            repoTrangThai.Appearance.Font = new Font("Arial", 10);
            gridControlnhaphang.RepositoryItems.Add(repoTrangThai);
            gridViewnhaphang.Columns["TrangThai"].ColumnEdit = repoTrangThai;
            repoTrangThai.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;


            gridViewnhaphang.Appearance.Row.Font = new Font("Arial", 10);
            gridViewnhaphang.Appearance.HeaderPanel.Font = new Font("Arial", 10, FontStyle.Bold);
            //popuploctheo

            TaoPopupLocTheo();           
            popuplocchitiet.BackColor = Color.White;
            //
            this.gridViewnhaphang.CellValueChanged += gridViewnhaplieu_CellValueChanged;
            this.gridViewnhaphang.ShowingEditor += gridViewnhaphang_ShowingEditor;
            this.gridViewnhaphang.RowCellStyle += gridViewnhaplieu_RowCellStyle;

        }
        private void gridViewnhaplieu_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (isProcessingChange) return;
            GridView view = sender as GridView;

            if (e.Column.FieldName == "TrangThai")
            {
                string newStatus = e.Value?.ToString();
                if (string.IsNullOrEmpty(newStatus)) return;

                string maPhieuNhap = view.GetRowCellValue(e.RowHandle, "MaPhieuNhap")?.ToString();
                if (string.IsNullOrEmpty(maPhieuNhap)) return;

                if (newStatus == "Duyệt")
                {
                    view.PostEditor();
                    view.UpdateCurrentRow();

                    DialogResult result = MessageBox.Show(
                        "Bạn có chắc chắn muốn duyệt phiếu này không?",
                        "Xác nhận duyệt",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    isProcessingChange = true;

                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            string constr = @"Data Source=MINHTHUVU\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False";
                            using (SqlConnection conn = new SqlConnection(constr))
                            {
                                conn.Open();

                                // Gọi thủ tục cập nhật tồn kho
                                using (SqlCommand cmd = new SqlCommand("usp_NhapKho_TaoPhieu", conn))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@MaPhieuNhap", maPhieuNhap);
                                    cmd.ExecuteNonQuery();
                                }

                                // Cập nhật trạng thái phiếu trong DB
                                using (SqlCommand cmdUpdate = new SqlCommand("UPDATE PhieuNhap SET TrangThai = N'Đã xác nhận' WHERE MaPhieuNhap = @MaPhieuNhap", conn))
                                {
                                    cmdUpdate.Parameters.AddWithValue("@MaPhieuNhap", maPhieuNhap);
                                    cmdUpdate.ExecuteNonQuery();
                                }
                            }

                            view.SetRowCellValue(e.RowHandle, "TrangThai", "Đã xác nhận");
                            MessageBox.Show("Đã duyệt phiếu và cập nhật tồn kho thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi khi cập nhật tồn kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        // Nếu không duyệt, đặt lại trạng thái là Nháp
                        view.SetRowCellValue(e.RowHandle, "TrangThai", "Nháp");
                    }

                    isProcessingChange = false;
                }
            }
        }
        private void gridViewnhaphang_ShowingEditor(object sender, CancelEventArgs e)
        {
            GridView view = sender as GridView;
            if (view.FocusedColumn.FieldName == "TrangThai")
            {
                string trangThai = view.GetFocusedRowCellValue("TrangThai")?.ToString();
                if (trangThai == "Đã xác nhận")
                {
                    e.Cancel = true;
                }
            }
        }
        private void gridViewnhaplieu_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Column.FieldName == "TrangThai")
            {
                string value = view.GetRowCellValue(e.RowHandle, "TrangThai")?.ToString();
                if (value == "Đã xác nhận")
                {
                    e.Appearance.ForeColor = Color.FromArgb(40, 167, 69);
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                }
            }
        }
        private void btntailai_Click(object sender, EventArgs e)
        {
            gridViewnhaphang.ActiveFilterString = "";  // Xóa điều kiện lọc
            gridViewnhaphang.ClearColumnsFilter();     // Xóa filter ở từng cột (nếu có)
            //xóa text ở ô tìm kiếm
            // Xóa nội dung trong ô tìm kiếm
            txttimkiem.Text = string.Empty;
            // Đặt lại DataSource ban đầu
            txttimkiem.Properties.NullValuePrompt = "Nhập mã phiếu và nhấn enter để tìm kiếm";
            LoadPhieuNhapHangData();
        }

        private void btncaidat_Click(object sender, EventArgs e)
        {
            DXPopupMenu menu = new DXPopupMenu();

            foreach (DevExpress.XtraGrid.Columns.GridColumn col in gridViewnhaphang.Columns)
            {
                DXMenuCheckItem item = new DXMenuCheckItem(col.Caption, col.Visible, null, (o, args) =>
                {
                    col.Visible = !col.Visible;
                });

                menu.Items.Add(item);
            }
            if (menu.Items.Count == 0)
            {
                XtraMessageBox.Show("Không có cột nào để hiển thị!");
                return;
            }
            // Hiển thị menu ngay dưới nút
            menu.ShowPopup(btncaidat, new Point(0, btncaidat.Height));

        }
        private void txttimkiem_TextChanged(object sender, EventArgs e)
        {
            if (dt == null) return;

            string keyword = txttimkiem.Text.Trim().Replace("'", "''");
            DataView dv = dt.DefaultView;

            if (string.IsNullOrEmpty(keyword))
                dv.RowFilter = "";
            else
                dv.RowFilter = $"TenNCC LIKE '%{keyword}%' OR TenNV LIKE '%{keyword}%' OR MaPhieuNhap LIKE '%{keyword}%'";

            gridControlnhaphang.DataSource = dv;
        }
        private void btnApDung_Click(object sender, EventArgs e)
        {
            // Lấy các giá trị từ popup lọc
            DateEdit dateTu = popuplocchitiet.Controls.OfType<DateEdit>().FirstOrDefault(c => c.Location == new Point(100, 55));
            DateEdit dateDen = popuplocchitiet.Controls.OfType<DateEdit>().FirstOrDefault(c => c.Location == new Point(408, 55));
            ComboBoxEdit cboNCC = popuplocchitiet.Controls.OfType<ComboBoxEdit>().FirstOrDefault(c => c.Location == new Point(150, 95));
            TextEdit txtSoPhieu = popuplocchitiet.Controls.OfType<TextEdit>().FirstOrDefault(c => c.Location == new Point(150, 135));
            ComboBoxEdit cboNV = popuplocchitiet.Controls.OfType<ComboBoxEdit>().FirstOrDefault(c => c.Location == new Point(150, 175));
            ComboBoxEdit cboTT = popuplocchitiet.Controls.OfType<ComboBoxEdit>().FirstOrDefault(c => c.Location == new Point(150, 215));
            // Xây dựng chuỗi điều kiện lọc
            string filter = "";
            if (dateTu.EditValue != null)
            {
                DateTime fromDate = dateTu.DateTime.Date;
                filter += $"NgayNhap >= #{fromDate:MM/dd/yyyy}# AND ";
            }
            if (dateDen.EditValue != null)
            {
                DateTime toDate = dateDen.DateTime.Date;
                filter += $"NgayNhap <= #{toDate:MM/dd/yyyy}# AND ";
            }
            if (cboNCC.EditValue != null)
            {
                filter += $"TenNCC = '{cboNCC.EditValue}' AND ";
            }
            if (!string.IsNullOrEmpty(txtSoPhieu.Text))
            {
                filter += $"MaPhieuNhap LIKE '%{txtSoPhieu.Text}%' AND ";
            }
            if (cboNV.EditValue != null)
            {
                filter += $"TenNV = '{cboNV.EditValue}' AND ";
            }
            if (cboTT.EditValue != null && cboTT.EditValue.ToString() != "Tất cả")
            {
                filter += $"TrangThai = '{cboTT.EditValue}' AND ";
            }
            // Loại bỏ " AND " thừa ở cuối chuỗi
            if (filter.EndsWith(" AND "))
            {
                filter = filter.Substring(0, filter.Length - 5);
            }
            // Áp dụng bộ lọc
            gridViewnhaphang.ActiveFilterString = filter;
            //
            // hiển thị thời gian lọc
            if (dateTu.EditValue != null && dateDen.EditValue != null)
            {
                lblthoigian.Text = $"Từ {dateTu.DateTime:dd/MM/yyyy} đến {dateDen.DateTime:dd/MM/yyyy}";
            }
            else if (dateTu.EditValue != null)
            {
                lblthoigian.Text = $"Từ {dateTu.DateTime:dd/MM/yyyy}";
            }
            else if (dateDen.EditValue != null)
            {
                lblthoigian.Text = $"Đến {dateDen.DateTime:dd/MM/yyyy}";
            }
            else
            {
                lblthoigian.Text = "";
            }
        }

        private void btnloc_Click(object sender, EventArgs e)
        {
            int offsetY = 80; // 👈 chỉnh giá trị này để dịch popup xuống bao nhiêu pixel tùy ý (10–30 là đẹp)
            int offsetX = 20;  // nếu muốn dịch ngang thì đổi giá trị này

            pcelocchitiet.Location = new Point(btnloc.Left + offsetX, btnloc.Bottom + offsetY);
            pcelocchitiet.ShowPopup();

        }
        private void TaoPopupLocTheo()
        {
            if (popuplocchitiet == null)
            {
                popuplocchitiet = new PopupContainerControl()
                {
                    Size = new Size(500, 350),
                    BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple,
                    BackColor = Color.White
                };
            }
            else
            {
                popuplocchitiet.Controls.Clear();
            }

            // === Title ===
            var lbltitle = new LabelControl
            {
                Text = "Lọc theo",
                AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None,
                Location = new Point(0, 10),
                Width = popuplocchitiet.Width,
                Appearance = { Font = new Font("Arial", 12, FontStyle.Bold), TextOptions = { HAlignment = DevExpress.Utils.HorzAlignment.Center } }
            };
            popuplocchitiet.Controls.Add(lbltitle);

            // === Dòng 1: Kỳ, Từ ngày, Đến ngày ===

            LabelControl lblTuNgay = new LabelControl() { Text = "Từ ngày:", Location = new Point(17, 60) };
            DateEdit dateTu = new DateEdit() { Location = new Point(100, 55), Width = 120 };

            LabelControl lblDenNgay = new LabelControl() { Text = "Đến ngày:", Location = new Point(310, 60) };
            DateEdit dateDen = new DateEdit() { Location = new Point(408, 55), Width = 120 };

            // === Các dòng dưới canh thẳng cột ===
            int labelX = 20;
            int inputX = 150;
            int widthInput = 380;

            // Dòng 2: Nhà cung cấp
            LabelControl lblNCC = new LabelControl() { Text = "Nhà cung cấp:", Location = new Point(labelX, 100) };
            ComboBoxEdit cboNCC = new ComboBoxEdit() { Location = new Point(inputX, 95), Width = widthInput };
            // Thêm dữ liệu thật
            string constr = "Data Source=MINHTHUVU\\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False";
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                string sql = "SELECT TenNCC FROM NhaCungCap";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cboNCC.Properties.Items.Add(reader["TenNCC"].ToString());
                }
            }
            // Dòng 3: Số hóa đơn
            LabelControl lblSoPhieu = new LabelControl() { Text = "Số phiếu:", Location = new Point(labelX, 140) };
            TextEdit txtSoPhieu = new TextEdit() { Location = new Point(inputX, 135), Width = widthInput };

            // Dòng 4: Nhân viên nhập
            LabelControl lblNV = new LabelControl() { Text = "Nhân viên nhập:", Location = new Point(labelX, 180) };
            ComboBoxEdit cboNV = new ComboBoxEdit() { Location = new Point(inputX, 175), Width = widthInput };
            // Thêm dữ liệu thật

            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                string sql = "SELECT TenNV FROM NhanVien";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cboNV.Properties.Items.Add(reader["TenNV"].ToString());
                }
            }


            // Dòng 5: Trạng thái phiếu
            LabelControl lblTT = new LabelControl() { Text = "Trạng thái phiếu:", Location = new Point(labelX, 220) };
            ComboBoxEdit cboTT = new ComboBoxEdit() { Location = new Point(inputX, 215), Width = widthInput };
            cboTT.Properties.Items.AddRange(new[] { "Tất cả", "Đã xác nhận", "Nháp" });

            // === Nút bấm ===
            SimpleButton btnMacDinh = new SimpleButton()
            {
                Text = "Bộ lọc mặc định",
                Location = new Point(300, 280),
                Width = 120
            };
            btnMacDinh.Click += btnMacDinh_Click;
            btnMacDinh.LookAndFeel.UseDefaultLookAndFeel = false;
            btnMacDinh.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            BoGocVaVien(btnMacDinh, 12, Color.DarkGray, 1);
            //màu khi di chuột
            btnMacDinh.MouseEnter += (s, e) =>
            {
                btnMacDinh.Appearance.BackColor = Color.FromArgb(229, 238, 238);
            };
            btnMacDinh.MouseLeave += (s, e) =>
            {
                btnMacDinh.Appearance.BackColor = Color.White;
            };
            //màu khi nhấn
            btnMacDinh.MouseDown += (s, e) =>
            {
                btnMacDinh.Appearance.BackColor = Color.FromArgb(229, 238, 238);
            };
            btnMacDinh.MouseUp += (s, e) =>
            {
                btnMacDinh.Appearance.BackColor = Color.FromArgb(240, 240, 240);
            };
            SimpleButton btnApDung = new SimpleButton()
            {
                Text = "Áp dụng",
                Location = new Point(450, 280),
                Width = 80,

            };
            btnApDung.Click += btnApDung_Click;
            btnApDung.LookAndFeel.UseDefaultLookAndFeel = false;
            btnApDung.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            BoGocVaVien(btnApDung, 12, Color.DarkGray, 1);
            //màu khi di chuột
            btnApDung.MouseEnter += (s, e) =>
            {
                btnApDung.Appearance.BackColor = Color.FromArgb(229, 238, 238);
            };
            btnApDung.MouseLeave += (s, e) =>
            {
                btnApDung.Appearance.BackColor = Color.White;
            };
            //màu khi nhấn
            btnApDung.MouseDown += (s, e) =>
            {
                btnApDung.Appearance.BackColor = Color.FromArgb(229, 238, 238);
            };
            btnApDung.MouseUp += (s, e) =>
            {
                btnApDung.Appearance.BackColor = Color.FromArgb(240, 240, 240);
            };


            // === Thêm tất cả control vào popup ===
            popuplocchitiet.Controls.AddRange(new Control[]
            {
        lbltitle,
        lblTuNgay, dateTu, lblDenNgay, dateDen,
        lblNCC, cboNCC,
        lblSoPhieu, txtSoPhieu,
        lblNV, cboNV,
        lblTT, cboTT,
        btnMacDinh, btnApDung
            });
        }
        private void btnMacDinh_Click(object sender, EventArgs e)
        {
            // Đặt lại tất cả điều kiện lọc về mặc định
            gridViewnhaphang.ActiveFilterString = "";
            lblthoigian.Text = "";
            //xoá các giá trị đã chọn trong popup
            foreach (Control ctrl in popuplocchitiet.Controls)
            {
                if (ctrl is DateEdit dateEdit)
                {
                    dateEdit.EditValue = null;
                }
                else if (ctrl is ComboBoxEdit comboBox)
                {
                    comboBox.EditValue = null;
                }
                else if (ctrl is TextEdit textEdit)
                {
                    textEdit.Text = "";
                }
            }
        }
        private void btnthem_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=MINHTHUVU\\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False";

            string maMoi = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // Lấy mã NCC lớn nhất hiện có
                string query = "SELECT TOP 1 MaPhieuNhap FROM PhieuNhap ORDER BY MaPhieuNhap DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                object result = cmd.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                {
                    maMoi = "PN001";
                }
                else
                {
                    string maCu = result.ToString();
                    int so = int.Parse(maCu.Substring(3)) + 1;
                    maMoi = "PN" + so.ToString("D3");
                }
                // Gọi form chi tiết và truyền mã mới
                chungtunhaphang frm = new chungtunhaphang(chungtunhaphang.Mode.ThemMoi, maMoi);
                frm.ShowDialog();
                //
                LoadPhieuNhapHangData();
            }

        }
        private void LoadPhieuNhapHangData()
        {
            string connectionString = "Data Source=MINHTHUVU\\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT p.MaPhieuNhap, c.TenNCC, n.TenNV, p.NgayNhap, p.TongTien, t.TenPTTT, p.TrangThai FROM PhieuNhap p JOIN NhaCungCap c ON p.MaNCC = c.MaNCC JOIN NhanVien n ON p.MaNV = n.MaNV JOIN PhuongThucTT t ON p.MaPTTT = t.MaPTTT";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                dt.Clear();
                adapter.Fill(dt);
                gridControlnhaphang.DataSource = dt;
            }
        }
        private void gridViewnhaphang_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            var view = sender as DevExpress.XtraGrid.Views.Grid.GridView;

            // Nếu dòng này được chọn (tick checkbox)
            if (view.IsRowSelected(e.RowHandle))
            {
                e.Appearance.BackColor = Color.FromArgb(207, 226, 239);
                e.Appearance.ForeColor = Color.Black;
                e.HighPriority = true;
            }
        }
        private void GridViewnhaphang_MouseUp(object sender, MouseEventArgs e)
        {
            var view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            if (view == null) return;

            var hit = view.CalcHitInfo(e.Location);
            if (hit.InRow || hit.InRowCell)
            {
                userHasSelectedRow = true;
            }
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem người dùng đã click chọn dòng chưa
            if (!userHasSelectedRow)
            {
                MessageBox.Show("Vui lòng chọn một phiếu để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int[] selectedRows = gridViewnhaphang.GetSelectedRows();
            int rowHandle = -1;

            // Nếu có dòng được tick (checkbox)
            if (selectedRows != null && selectedRows.Length > 0)
            {
                if (selectedRows.Length > 1)
                {
                    MessageBox.Show("Chỉ được chọn 1 phiếu để chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                rowHandle = selectedRows[0];
            }
            else
            {
                // Nếu không tick mà chỉ click vào dòng
                rowHandle = gridViewnhaphang.FocusedRowHandle;
            }

            if (rowHandle < 0)
            {
                MessageBox.Show("Không tìm thấy dòng hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maPhieu = gridViewnhaphang.GetRowCellValue(rowHandle, "MaPhieuNhap")?.ToString();
            if (string.IsNullOrEmpty(maPhieu))
            {
                MessageBox.Show("Không thể lấy thông tin phiếu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 
           
            using (chungtunhaphang form = new chungtunhaphang(chungtunhaphang.Mode.ChinhSua, maPhieu))
            {
                form.Owner = this;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Reload dữ liệu sau khi form con đóng
                    LoadPhieuNhapHangData();
                }
            }

            // Reset lại biến khi reload
            userHasSelectedRow = false;
        }


        private void btnxoa_Click(object sender, EventArgs e)
        {
            if (!userHasSelectedRow)
            {
                MessageBox.Show("Vui lòng chọn một phiếu để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int[] selectedRows = gridViewnhaphang.GetSelectedRows();
            int rowHandle = -1;

            // Nếu tick nhiều dòng
            if (selectedRows != null && selectedRows.Length > 1)
            {
                MessageBox.Show("Chỉ được xóa 1 phiếu một lần!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedRows != null && selectedRows.Length == 1)
            {
                rowHandle = selectedRows[0];
            }
            else
            {
                rowHandle = gridViewnhaphang.FocusedRowHandle;
            }

            if (rowHandle < 0)
            {
                MessageBox.Show("Không tìm thấy dòng hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string maPhieu = gridViewnhaphang.GetRowCellValue(rowHandle, "MaPhieuNhap")?.ToString();
            if (string.IsNullOrEmpty(maPhieu))
            {
                MessageBox.Show("Không thể lấy thông tin phiếu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Xác nhận xóa
            DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa phiếu nhập hàng [{maPhieu}] không?",
                                                  "Xác nhận xóa",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    string connectionString = @"Data Source=MINHTHUVU\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False";
                    string query = "DELETE FROM PhieuNhap WHERE MaPhieuNhap = @MaPhieuNhap";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaPhieuNhap", maPhieu);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Xóa phiếu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reload dữ liệu
                    LoadPhieuNhapHangData();
                    userHasSelectedRow = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa phiếu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

}
    

