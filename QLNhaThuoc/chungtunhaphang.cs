using DevExpress.Xpo.DB.Helpers;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraMap;
using DevExpress.XtraReports.UI;
using DevExpress.XtraVerticalGrid;
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
using static QLNhaThuoc.FormWarehouse;
namespace QLNhaThuoc
{
    public partial class chungtunhaphang : DevExpress.XtraEditors.XtraForm
    {
        private const decimal TILE_CHENH = 10; // chênh lệch 10%
        SqlConnection conn = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand();
        DataTable dt = new DataTable();
        private DataTable dsThuoc = new DataTable();
        private string _maMoi;
        public enum Mode { ThemMoi, ChinhSua }
        public Mode FormMode { get; set; }
        public string MaPhieuNhap { get; set; }
        public chungtunhaphang(string maMoi)
        {
            InitializeComponent();
            _maMoi = maMoi;
        }
        public chungtunhaphang(Mode mode, string maPhieuNhap = "")
        {
            InitializeComponent();
            FormMode = mode;
            if (mode == Mode.ThemMoi)
                _maMoi = maPhieuNhap;
            else
                MaPhieuNhap = maPhieuNhap;

        }
        public chungtunhaphang()
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

        private void chitietnhaphang_Load(object sender, EventArgs e)
        {
            // Load danh sách cố định
            LoadNhanVien();
            LoadNCC();
            LoadPTTT();
            //
            lueNCC.EditValueChanged += lookUpEditNCC_EditValueChanged;

            // Nếu đã có sẵn NCC khi mở form (VD: truyền từ form trước)
            if (lueNCC.EditValue != null)
            {
                LoadVaGanDanhSachThuoc(lueNCC.EditValue.ToString());
            }
            // Cấu hình hiển thị chung cho GridView
            gridViewnhaplieu.OptionsView.ShowIndicator = false;
            gridViewnhaplieu.OptionsSelection.MultiSelect = true;
            gridViewnhaplieu.OptionsSelection.MultiSelectMode =
                DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            gridViewnhaplieu.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DevExpress.Utils.DefaultBoolean.True;
            gridViewnhaplieu.OptionsView.ShowGroupPanel = false;
            gridViewnhaplieu.OptionsView.EnableAppearanceEvenRow = true;
            gridViewnhaplieu.Appearance.HeaderPanel.Font = new Font("Arial", 10, FontStyle.Bold);
            gridViewnhaplieu.Appearance.Row.Font = new Font("Arial", 10);
            gridViewnhaplieu.RowHeight = 28;
            gridViewnhaplieu.OptionsView.ColumnAutoWidth = true;
            gridViewnhaplieu.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridViewnhaplieu.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridViewnhaplieu.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            // đổi lề của caltongtien
            caltongtien.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            gridViewnhaplieu.CellValueChanged += gridViewnhaplieu_CellValueChanged;
            gridViewnhaplieu.RowCountChanged += gridViewnhaplieu_RowCountChanged;

            // ----------- PHÂN NHÁNH MODE -----------
            if (FormMode == Mode.ThemMoi)
            {
                lblheader.Text = "THÊM MỚI CHỨNG TỪ NHẬP HÀNG";
                txtspn.Text = _maMoi;

                // Tạo mới DataTable rỗng
                DataTable dt = new DataTable();

                dt.Columns.Add("MaThuoc", typeof(string));
                dt.Columns.Add("TenThuoc", typeof(string));
                dt.Columns.Add("SoLuong", typeof(decimal));
                dt.Columns.Add("DonGiaNhap", typeof(decimal));
                dt.Columns.Add("NgaySanXuat", typeof(DateTime));
                dt.Columns.Add("HanSuDung", typeof(DateTime));
                dt.Columns.Add("ThanhTien", typeof(decimal));

                // Gán nguồn dữ liệu
                gridControlnhaplieu.DataSource = dt;
                gridViewnhaplieu.PopulateColumns();
                gridViewnhaplieu.Columns["MaThuoc"].Caption = "Mã thuốc";
                gridViewnhaplieu.Columns["TenThuoc"].Caption = "Tên thuốc";
                gridViewnhaplieu.Columns["SoLuong"].Caption = "Số lượng";
                gridViewnhaplieu.Columns["DonGiaNhap"].Caption = "Đơn giá nhập";
                gridViewnhaplieu.Columns["NgaySanXuat"].Caption = "Ngày sản xuất";
                gridViewnhaplieu.Columns["HanSuDung"].Caption = "Hạn sử dụng";
                gridViewnhaplieu.Columns["ThanhTien"].Caption = "Thành tiền";
                //
                date.ReadOnly = false;
                lueNCC.ReadOnly = false;
            }
            else // ----- CHỈNH SỬA -----
            {
                lblheader.Text = "CHỈNH SỬA CHỨNG TỪ NHẬP HÀNG";

                LoadData();           // Thông tin phiếu
                LoadChiTietPhieu();   // Dòng chi tiết thuốc
                gridViewnhaplieu.OptionsBehavior.Editable = true;   // Cho phép sửa chi tiết
               
            }

            CaiDatMauChoCacNut();
            gridViewnhaplieu.DataSourceChanged += (s, ev) =>
            {
                if (lueNCC.EditValue != null)
                {
                    string maNCC = lueNCC.EditValue.ToString();
                    LoadVaGanDanhSachThuoc(maNCC);
                    AddDateEditToGridView();
                }
            };
        }
        private void gridViewnhaplieu_RowCountChanged(object sender, EventArgs e)
        {
            TinhTongTien();
        }


        private void CaiDatMauChoCacNut()
        {
            List<Button> buttons = new List<Button> { btnsave, btnprint, btnhuy };
            foreach (var btn in buttons)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Margin = new Padding(10);
                btn.BackColor = Color.FromArgb(66, 144, 242);
                btn.ForeColor = Color.White;

                btn.MouseEnter += (s, e) => { btn.BackColor = Color.FromArgb(118, 173, 243); };
                btn.MouseLeave += (s, e) => { btn.BackColor = Color.FromArgb(66, 144, 242); };
                btn.MouseDown += (s, e) => { btn.BackColor = Color.FromArgb(40, 116, 240); };
                btn.MouseUp += (s, e) => { btn.BackColor = Color.FromArgb(118, 173, 243); };
            }
        }
        private void lookUpEditNCC_EditValueChanged(object sender, EventArgs e)
        {
            if (lueNCC.EditValue != null)
            {
                LoadVaGanDanhSachThuoc(lueNCC.EditValue.ToString());
            }
            DataRowView row = lueNCC.GetSelectedDataRow() as DataRowView;
            if (row != null)
            {
                txttenncc.Text = row["TenNCC"].ToString();
                txtdc.Text = row["DiaChi"].ToString();
            }
            else
            {
                txttenncc.Text = string.Empty;
                txtdc.Text = string.Empty;
            }
        }
        private void LoadChiTietPhieu()
        {
            string constr = @"Data Source=MINHTHUVU\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False";
            string sql = @"
        SELECT 
            ctp.MaThuoc, 
            t.TenThuoc, 
            ctp.SoLuong, 
            ctp.DonGiaNhap,
            ctp.NgaySanXuat,
            ctp.HanSuDung,
            ctp.ThanhTien
        FROM ChiTietPhieuNhap ctp
        INNER JOIN Thuoc t ON ctp.MaThuoc = t.MaThuoc
        WHERE ctp.MaPhieuNhap = @MaPhieuNhap";
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(constr))
            using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
            {
                da.SelectCommand.Parameters.AddWithValue("@MaPhieuNhap", txtspn.Text);
                da.Fill(dt);
            }

            // Gán nguồn dữ liệu
            gridControlnhaplieu.DataSource = dt;
            gridViewnhaplieu.PopulateColumns();
            gridViewnhaplieu.RefreshData();

            // Đặt caption cho các cột
            if (gridViewnhaplieu.Columns["MaThuoc"] != null)
                gridViewnhaplieu.Columns["MaThuoc"].Caption = "Mã thuốc";
            if (gridViewnhaplieu.Columns["TenThuoc"] != null)
                gridViewnhaplieu.Columns["TenThuoc"].Caption = "Tên thuốc";
            if (gridViewnhaplieu.Columns["SoLuong"] != null)
                gridViewnhaplieu.Columns["SoLuong"].Caption = "Số lượng";
            if (gridViewnhaplieu.Columns["DonGiaNhap"] != null)
                gridViewnhaplieu.Columns["DonGiaNhap"].Caption = "Đơn giá nhập";
            if (gridViewnhaplieu.Columns["NgaySanXuat"] != null)
                gridViewnhaplieu.Columns["NgaySanXuat"].Caption = "Ngày sản xuất";
            if (gridViewnhaplieu.Columns["HanSuDung"] != null)
                gridViewnhaplieu.Columns["HanSuDung"].Caption = "Hạn sử dụng";
            if (gridViewnhaplieu.Columns["ThanhTien"] != null)
                gridViewnhaplieu.Columns["ThanhTien"].Caption = "Thành tiền";

            // gọi LoadVaGanDanhSachThuoc SAU khi DataSource đã có cột MaThuoc
            this.BeginInvoke(new Action(() =>
            {
                if (lueNCC.EditValue != null)
                {
                    LoadVaGanDanhSachThuoc(lueNCC.EditValue.ToString());

                    if (gridViewnhaplieu.Columns["MaThuoc"] != null)
                    {
                        var repo = gridControlnhaplieu.RepositoryItems
                            .OfType<RepositoryItemLookUpEdit>()
                            .FirstOrDefault();

                        if (repo != null)
                            gridViewnhaplieu.Columns["MaThuoc"].ColumnEdit = repo;
                    }

                }
            }));
        }
        private void LoadData()
        {
            string connectionString = @"Data Source=MINHTHUVU\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM PhieuNhap p JOIN NhaCungCap n ON p.MaNCC = n.MaNCC JOIN NhanVien nv ON p.MaNV = nv.MaNV Join PhuongThucTT t On p.MaPTTT = t.MaPTTT WHERE MaPhieuNhap = @MaPhieuNhap";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaPhieuNhap", MaPhieuNhap);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtspn.Text = reader["MaPhieuNhap"].ToString();
                                date.Text = reader["NgayNhap"].ToString();
                                lueNCC.EditValue = reader["MaNCC"].ToString();
                                luenv.EditValue = reader["MaNV"].ToString();
                                luepttt.EditValue = reader["MaPTTT"].ToString();
                                caltongtien.Text = reader["TongTien"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy phiếu nhập hàng với mã phiếu đã cho.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        }

        private void LoadVaGanDanhSachThuoc(string maNCC)
        {
            string connectionString = @"Data Source=MINHTHUVU\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False";
            string query = "SELECT MaThuoc, TenThuoc, GiaBan FROM Thuoc WHERE MaNCC = @MaNCC";

            dsThuoc.Clear();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
            {
                da.SelectCommand.Parameters.AddWithValue("@MaNCC", maNCC);
                da.Fill(dsThuoc);
            }

            if (dsThuoc.Rows.Count == 0)
                return;

            RepositoryItemLookUpEdit repoMaThuoc = new RepositoryItemLookUpEdit
            {
                DataSource = dsThuoc,
                DisplayMember = "MaThuoc",
                ValueMember = "MaThuoc",
                NullText = "",
                TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard,
                ImmediatePopup = true,
                PopupWidth = 300
            };
            repoMaThuoc.PopulateColumns();

            if (repoMaThuoc.Columns["TenThuoc"] != null)
                repoMaThuoc.Columns["TenThuoc"].Caption = "Tên thuốc";
            // 
            this.BeginInvoke(new Action(() =>
            {
                if (gridViewnhaplieu.Columns["MaThuoc"] == null)
                    gridViewnhaplieu.PopulateColumns();

                gridControlnhaplieu.RepositoryItems.Add(repoMaThuoc);
                if (gridViewnhaplieu.Columns["MaThuoc"] != null)
                    gridViewnhaplieu.Columns["MaThuoc"].ColumnEdit = repoMaThuoc;
            }));
        }
        //thêm dateedit vào cột ngày sản xuất và hạn sử dụng
        private void AddDateEditToGridView()
        {
            RepositoryItemDateEdit dateEdit = new RepositoryItemDateEdit
            {
                CalendarTimeProperties = { DisplayFormat = { FormatString = "dd/MM/yyyy", FormatType = DevExpress.Utils.FormatType.DateTime } },
                DisplayFormat = { FormatString = "dd/MM/yyyy", FormatType = DevExpress.Utils.FormatType.DateTime },
                EditFormat = { FormatString = "dd/MM/yyyy", FormatType = DevExpress.Utils.FormatType.DateTime },
                Mask = { EditMask = "dd/MM/yyyy" }
            };
            gridControlnhaplieu.RepositoryItems.Add(dateEdit);
            if (gridViewnhaplieu.Columns["NgaySanXuat"] != null)
                gridViewnhaplieu.Columns["NgaySanXuat"].ColumnEdit = dateEdit;
            if (gridViewnhaplieu.Columns["HanSuDung"] != null)
                gridViewnhaplieu.Columns["HanSuDung"].ColumnEdit = dateEdit;
        }
        private void gridViewnhaplieu_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.Column.FieldName == "MaThuoc")
            {
                string maThuoc = e.Value?.ToString();
                if (string.IsNullOrEmpty(maThuoc)) return;

                DataRow[] rows = dsThuoc.Select($"MaThuoc = '{maThuoc}'");
                if (rows.Length > 0)
                {
                    view.SetRowCellValue(e.RowHandle, "TenThuoc", rows[0]["TenThuoc"].ToString());

                    if (decimal.TryParse(rows[0]["GiaBan"].ToString(), out decimal giaBan))
                    {
                        decimal giaNhap = giaBan * (1 - TILE_CHENH / 100);
                        view.SetRowCellValue(e.RowHandle, "DonGiaNhap", giaNhap);
                    }
                }
            }

            if (e.Column.FieldName == "SoLuong" || e.Column.FieldName == "DonGiaNhap")
            {
                object valSoLuong = view.GetRowCellValue(e.RowHandle, "SoLuong");
                object valDonGia = view.GetRowCellValue(e.RowHandle, "DonGiaNhap");

                decimal soLuong = 0, donGia = 0;
                decimal.TryParse(valSoLuong?.ToString(), out soLuong);
                decimal.TryParse(valDonGia?.ToString(), out donGia);

                view.SetRowCellValue(e.RowHandle, "ThanhTien", soLuong * donGia);
            }

            TinhTongTien();
        }

        private void TinhTongTien()
        {
            decimal tongTien = 0;

            for (int i = 0; i < gridViewnhaplieu.RowCount; i++)
            {
                var row = gridViewnhaplieu.GetDataRow(i);
                if (row != null && row["ThanhTien"] != DBNull.Value)
                {
                    tongTien += Convert.ToDecimal(row["ThanhTien"]);
                }
            }

            caltongtien.EditValue = tongTien;
        }
        void LoadNhanVien()
        {
            // Nạp danh sách nhân viên (nên gọi ở Form_Load)
            string connectionString = @"Data Source=MINHTHUVU\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT MaNV, TenNV FROM NhanVien";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                luenv.Properties.DataSource = dt;
                luenv.Properties.DisplayMember = "TenNV";
                luenv.Properties.ValueMember = "MaNV";
            }
        }
        void LoadNCC()
        {

            string connectionString = @"Data Source=MINHTHUVU\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT MaNCC, TenNCC, DiaChi FROM NhaCungCap";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                lueNCC.Properties.DataSource = dt;
                lueNCC.Properties.DisplayMember = "MaNCC";
                lueNCC.Properties.ValueMember = "MaNCC";
            }

        }
        void LoadPTTT()
        {
            // Nạp danh sách phương thức thanh toán (nên gọi ở Form_Load)
            string connectionString = @"Data Source=MINHTHUVU\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT MaPTTT, TenPTTT FROM PhuongThucTT";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                luepttt.Properties.DataSource = dt;
                luepttt.Properties.DisplayMember = "TenPTTT";
                luepttt.Properties.ValueMember = "MaPTTT";
            }
        }
        private void btnhuy_Click(object sender, EventArgs e)
        {
            ((nhaphang)this.Owner).gridViewnhaphang.ClearSelection();
            this.Close();
            //bỏ chọn trong gridview

        }

        private void btnprint_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    // Lấy dữ liệu chi tiết từ GridControl
                    DataTable chitiet = gridControlnhaplieu.DataSource as DataTable;


                    // gán đơn vị tính vào bảng
                    if (chitiet != null)
                    {
                        // Thêm cột nếu chưa có
                        if (!chitiet.Columns.Contains("TenDonViTinh"))
                            chitiet.Columns.Add("TenDonViTinh", typeof(string));
                        using (SqlConnection conn = new SqlConnection("Data Source=MINHTHUVU\\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False"))
                        {
                            conn.Open();
                            foreach (DataRow row in chitiet.Rows)
                            {
                                string maThuoc = row["MaThuoc"].ToString();
                                string sql = "SELECT d.TenDonViTinh FROM Thuoc t INNER JOIN DonViTinh d ON t.MaDonViTinh = d.MaDonViTinh WHERE MaThuoc = @MaThuoc";
                                using (SqlCommand cmd = new SqlCommand(sql, conn))
                                {
                                    cmd.Parameters.AddWithValue("@MaThuoc", maThuoc);
                                    object result = cmd.ExecuteScalar();
                                    if (result != null)
                                        row["TenDonViTinh"] = result.ToString();
                                }
                            }
                        }
                    }
                    if (!chitiet.Columns.Contains("TenNCC"))
                        chitiet.Columns.Add("TenNCC", typeof(string));
                    using (SqlConnection conn = new SqlConnection("Data Source=MINHTHUVU\\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False"))
                    {
                        conn.Open();
                        foreach (DataRow row in chitiet.Rows)
                        {
                            string maThuoc = row["MaThuoc"].ToString();
                            string sql = "SELECT n.TenNCC FROM Thuoc t INNER JOIN NhaCungCap n ON t.MaNCC = n.MaNCC WHERE MaThuoc = @MaThuoc";
                            using (SqlCommand cmd = new SqlCommand(sql, conn))
                            {
                                cmd.Parameters.AddWithValue("@MaThuoc", maThuoc);
                                object result = cmd.ExecuteScalar();
                                if (result != null)
                                    row["TenNCC"] = result.ToString();
                            }
                        }
                    }
                    if (!chitiet.Columns.Contains("TenDanhMuc"))
                        chitiet.Columns.Add("TenDanhMuc", typeof(string));
                    using (SqlConnection conn = new SqlConnection("Data Source=MINHTHUVU\\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False"))
                    {
                        conn.Open();
                        foreach (DataRow row in chitiet.Rows)
                        {
                            string maThuoc = row["MaThuoc"].ToString();
                            string sql = "SELECT dm.TenDanhMuc FROM Thuoc t INNER JOIN DanhMucThuoc dm ON t.MaDanhMuc = dm.MaDanhMuc WHERE MaThuoc = @MaThuoc";
                            using (SqlCommand cmd = new SqlCommand(sql, conn))
                            {
                                cmd.Parameters.AddWithValue("@MaThuoc", maThuoc);
                                object result = cmd.ExecuteScalar();
                                if (result != null)
                                    row["TenDanhMuc"] = result.ToString();
                            }
                        }
                        // 1. Lấy tổng tiền từ caltongtien
                        decimal tongTien = 0;
                        if (caltongtien.EditValue != null)
                            decimal.TryParse(caltongtien.EditValue.ToString(), out tongTien);

                        // 2. Chuyển số sang chữ
                        string tongTienChu = SoThanhChu(tongTien); // hàm bạn tự viết bên dưới


                        // Lấy các thông tin còn lại
                        string tenNCC = txttenncc.Text;
                        string maPhieu = txtspn.Text;
                        DateTime ngayLap = date.DateTime;
                        string nguoiLap = luenv.Text;                        
                        // Lấy logo từ Resources
                        Image logo = Properties.Resources.logo; // đổi tên nếu khác

                        // Tạo report 
                        phieunhapkho rpt = new phieunhapkho(
                            chitiet,
                            tenNCC,
                            maPhieu,
                            ngayLap,
                            nguoiLap,
                            tongTien,
                            logo


                        );
                        // Thêm tham số Tổng tiền bằng chữ
                        rpt.Parameters.Add(new DevExpress.XtraReports.Parameters.Parameter()
                        {
                            Name = "pTongTienChu",
                            Value = tongTienChu,
                            Visible = false
                        });

                        // Hiển thị xem trước
                        ReportPrintTool printTool = new ReportPrintTool(rpt);
                        printTool.ShowPreviewDialog();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể in phiếu. Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi in phiếu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            string maPhieuNhap = txtspn.Text?.Trim();
            if (string.IsNullOrEmpty(maPhieuNhap))
            {
                MessageBox.Show("Mã phiếu không hợp lệ. Hãy tạo phiếu mới trước khi lưu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maNCC = lueNCC.EditValue?.ToString();
            string maNV = luenv.EditValue?.ToString();
            DateTime ngayNhap = date.DateTime;
            string maPTTT = luepttt.EditValue?.ToString();

            if (string.IsNullOrEmpty(maNV) || string.IsNullOrEmpty(maNCC))
            {
                MessageBox.Show("Vui lòng chọn đầy đủ thông tin nhân viên và nhà cung cấp.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = @"Data Source=MINHTHUVU\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();

                try
                {
                    // ---------------------------------------------
                    // 1️⃣ Lưu / cập nhật thông tin phiếu nhập
                    // ---------------------------------------------
                    string sqlPhieu = @"
                IF EXISTS (SELECT 1 FROM PhieuNhap WHERE MaPhieuNhap=@MaPhieuNhap)
                    UPDATE PhieuNhap
                    SET MaNV=@MaNV, MaNCC=@MaNCC, MaPTTT=@MaPTTT, NgayNhap=@NgayNhap, TongTien=@TongTien
                    WHERE MaPhieuNhap=@MaPhieuNhap
                ELSE
                    INSERT INTO PhieuNhap(MaPhieuNhap, MaNCC, MaNV, NgayNhap, TongTien, MaPTTT)
                    VALUES(@MaPhieuNhap, @MaNCC, @MaNV, @NgayNhap, @TongTien, @MaPTTT)";

                    decimal tongTien = 0;

                    using (SqlCommand cmd = new SqlCommand(sqlPhieu, conn, tran))
                    {
                        cmd.Parameters.AddWithValue("@MaPhieuNhap", maPhieuNhap);
                        cmd.Parameters.AddWithValue("@MaNV", maNV);
                        cmd.Parameters.AddWithValue("@MaNCC", maNCC);
                        cmd.Parameters.AddWithValue("@MaPTTT", maPTTT);
                        cmd.Parameters.AddWithValue("@NgayNhap", ngayNhap);
                        cmd.Parameters.AddWithValue("@TongTien", tongTien); // cập nhật sau
                        cmd.ExecuteNonQuery();
                    }

                    // ---------------------------------------------
                    // 2️⃣ Lưu chi tiết phiếu nhập
                    // ---------------------------------------------
                    for (int i = 0; i < gridViewnhaplieu.RowCount; i++)
                    {
                        string maThuoc = gridViewnhaplieu.GetRowCellValue(i, "MaThuoc")?.ToString();
                        if (string.IsNullOrWhiteSpace(maThuoc))
                            continue;

                        int soLuong = Convert.ToInt32(gridViewnhaplieu.GetRowCellValue(i, "SoLuong") ?? 0);
                        decimal donGia = Convert.ToDecimal(gridViewnhaplieu.GetRowCellValue(i, "DonGiaNhap") ?? 0);
                        if (soLuong <= 0 || donGia <= 0) continue;

                        object nsxObj = gridViewnhaplieu.GetRowCellValue(i, "NgaySanXuat");
                        object hsdObj = gridViewnhaplieu.GetRowCellValue(i, "HanSuDung");
                        DateTime? ngaySX = nsxObj != null && nsxObj != DBNull.Value ? Convert.ToDateTime(nsxObj) : (DateTime?)null;
                        DateTime? hanSD = hsdObj != null && hsdObj != DBNull.Value ? Convert.ToDateTime(hsdObj) : (DateTime?)null;

                        decimal thanhTien = soLuong * donGia;
                        tongTien += thanhTien;

                        string maCTPN = gridViewnhaplieu.GetRowCellValue(i, "MaCTPN")?.ToString();

                        // Nếu chưa có mã chi tiết -> sinh mới
                        if (string.IsNullOrEmpty(maCTPN))
                        {
                            string sqlMaxCT = @"SELECT TOP 1 MaCTPN 
                                        FROM ChiTietPhieuNhap 
                                        WHERE MaCTPN LIKE @MaPhieuNhap + '-%' 
                                        ORDER BY MaCTPN DESC";
                            using (SqlCommand cmdMax = new SqlCommand(sqlMaxCT, conn, tran))
                            {
                                cmdMax.Parameters.AddWithValue("@MaPhieuNhap", maPhieuNhap);
                                object result = cmdMax.ExecuteScalar();
                                int nextNum = 1;
                                if (result != null)
                                {
                                    string lastMa = result.ToString();
                                    nextNum = int.Parse(lastMa.Split('-')[1]) + 1;
                                }
                                maCTPN = $"{maPhieuNhap}-{nextNum:D2}";
                            }
                        }

                        // Thêm hoặc cập nhật dòng chi tiết
                        string sqlCT = @"
                   IF EXISTS (SELECT 1 FROM ChiTietPhieuNhap WHERE MaPhieuNhap=@MaPhieuNhap AND MaThuoc=@MaThuoc)
                   UPDATE ChiTietPhieuNhap
                   SET SoLuong = @SoLuong,
                   DonGiaNhap = @DonGiaNhap,
                   NgaySanXuat = @NgaySanXuat,
                   HanSuDung = @HanSuDung,
                   ThanhTien = @ThanhTien
                   WHERE MaPhieuNhap=@MaPhieuNhap AND MaThuoc=@MaThuoc
                ELSE
                   INSERT INTO ChiTietPhieuNhap(MaCTPN, MaPhieuNhap, MaThuoc, SoLuong, DonGiaNhap, NgaySanXuat, HanSuDung, ThanhTien)
                   VALUES(@MaCTPN, @MaPhieuNhap, @MaThuoc, @SoLuong, @DonGiaNhap, @NgaySanXuat, @HanSuDung, @ThanhTien)";


                        using (SqlCommand cmdCT = new SqlCommand(sqlCT, conn, tran))
                        {
                            cmdCT.Parameters.AddWithValue("@MaCTPN", maCTPN);
                            cmdCT.Parameters.AddWithValue("@MaPhieuNhap", maPhieuNhap);
                            cmdCT.Parameters.AddWithValue("@MaThuoc", maThuoc);
                            cmdCT.Parameters.AddWithValue("@SoLuong", soLuong);
                            cmdCT.Parameters.AddWithValue("@DonGiaNhap", donGia);
                            cmdCT.Parameters.AddWithValue("@NgaySanXuat", (object?)ngaySX ?? DBNull.Value);
                            cmdCT.Parameters.AddWithValue("@HanSuDung", (object?)hanSD ?? DBNull.Value);
                            cmdCT.Parameters.AddWithValue("@ThanhTien", thanhTien);
                            cmdCT.ExecuteNonQuery();
                        }

                        // Cập nhật lại Grid
                        gridViewnhaplieu.SetRowCellValue(i, "MaCTPN", maCTPN);
                        gridViewnhaplieu.SetRowCellValue(i, "ThanhTien", thanhTien);
                    }

                    // ---------------------------------------------
                    // 3️⃣ Cập nhật tổng tiền vào Phiếu Nhập
                    // ---------------------------------------------
                    string sqlUpdateTong = "UPDATE PhieuNhap SET TongTien=@TongTien WHERE MaPhieuNhap=@MaPhieuNhap";
                    using (SqlCommand cmdTong = new SqlCommand(sqlUpdateTong, conn, tran))
                    {
                        cmdTong.Parameters.AddWithValue("@MaPhieuNhap", maPhieuNhap);
                        cmdTong.Parameters.AddWithValue("@TongTien", tongTien);
                        cmdTong.ExecuteNonQuery();
                    }

                    tran.Commit();
                    caltongtien.EditValue = tongTien;
                    MessageBox.Show("Lưu phiếu nhập hàng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show("Lỗi khi lưu phiếu nhập hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private string SoThanhChu(decimal so)
        {
            if (so == 0) return "Không đồng";

            string[] chuSo = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] hang = { "", "nghìn", "triệu", "tỷ" };

            long n = (long)Math.Floor(so);
            string s = n.ToString();
            int len = s.Length;
            int hangIndex = 0;
            string ketQua = "";

            while (len > 0)
            {
                int tam = len - 3;
                if (tam < 0) tam = 0;
                string block = s.Substring(tam, len - tam);
                int number = int.Parse(block);

                if (number != 0)
                {
                    string blockText = DocBaChuSo(number, chuSo);
                    ketQua = blockText + " " + hang[hangIndex] + " " + ketQua;
                }

                hangIndex++;
                len -= 3;
            }

            ketQua = ketQua.Trim();
            // Viết hoa chữ cái đầu
            if (ketQua.Length > 0)
                ketQua = char.ToUpper(ketQua[0]) + ketQua.Substring(1) + " đồng";

            return ketQua;
        }

        // Hàm đọc 3 chữ số
        private string DocBaChuSo(int so, string[] chuSo)
        {
            int tram = so / 100;
            int chuc = (so % 100) / 10;
            int donVi = so % 10;
            string ketQua = "";

            if (tram > 0)
                ketQua += chuSo[tram] + " trăm";

            if (chuc > 1)
                ketQua += " " + chuSo[chuc] + " mươi";
            else if (chuc == 1)
                ketQua += " mười";
            else if (chuc == 0 && tram > 0 && donVi > 0)
                ketQua += " linh";

            if (donVi > 0)
            {
                if (chuc >= 1)
                {
                    if (donVi == 1) ketQua += " mốt";
                    else if (donVi == 5) ketQua += " lăm";
                    else ketQua += " " + chuSo[donVi];
                }
                else
                    ketQua += " " + chuSo[donVi];
            }

            return ketQua.Trim();
        }


    }
}