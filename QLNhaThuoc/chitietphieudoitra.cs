using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using static QLNhaThuoc.FormWarehouse;


namespace QLNhaThuoc
{
    public partial class chitietphieudoitra : DevExpress.XtraEditors.XtraForm
    {

        SqlConnection conn = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand();
        DataTable dt = new DataTable();
        private DataTable dsThuoc = new DataTable();
        private string _maMoi;
        public enum Mode { ThemMoi, ChinhSua }
        public Mode FormMode { get; set; }
        public string MaPhieuTra { get; set; }
        public chitietphieudoitra(string maMoi)
        {
            InitializeComponent();
            _maMoi = maMoi;
        }
        public chitietphieudoitra(Mode mode, string maPhieuTra = "")
        {
            InitializeComponent();
            FormMode = mode;
            if (mode == Mode.ThemMoi)
                _maMoi = maPhieuTra;
            else
                MaPhieuTra = maPhieuTra;

        }
        public chitietphieudoitra()
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

        private void chitietphieudoitra_Load(object sender, EventArgs e)
        {
            // Load danh sách cố định
            LoadNhanVien();
            LoadNCC();
            //
            luetenncc.EditValueChanged += Luetenncc_EditValueChanged;

            // Nếu đã có sẵn NCC khi mở form (VD: truyền từ form trước)
            if (luetenncc.EditValue != null)
            {
                LoadVaGanDanhSachThuoc(luetenncc.EditValue.ToString());
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
            gridViewnhaplieu.CellValueChanged += gridViewnhaplieu_CellValueChanged;
            gridViewnhaplieu.RowCountChanged += gridViewnhaplieu_RowCountChanged;

            // ----------- PHÂN NHÁNH MODE -----------
            if (FormMode == Mode.ThemMoi)
            {
                lblheader.Text = "THÊM MỚI PHIẾU TRẢ HÀNG" + " " + _maMoi;
                lblmaphieu.Text = _maMoi;

                // Tạo mới DataTable rỗng
                DataTable dt = new DataTable();
                dt.Columns.Add("MaThuoc", typeof(string));
                dt.Columns.Add("TenThuoc", typeof(string));
                dt.Columns.Add("SoLuong", typeof(decimal));
                dt.Columns.Add("DonGia", typeof(decimal));
                dt.Columns.Add("ThanhTien", typeof(decimal));

                // Gán nguồn dữ liệu
                gridControlnhaplieu.DataSource = dt;
                gridViewnhaplieu.PopulateColumns();

                gridViewnhaplieu.Columns["MaThuoc"].Caption = "Mã thuốc";
                gridViewnhaplieu.Columns["TenThuoc"].Caption = "Tên thuốc";
                gridViewnhaplieu.Columns["SoLuong"].Caption = "Số lượng";
                gridViewnhaplieu.Columns["DonGia"].Caption = "Đơn giá";
                gridViewnhaplieu.Columns["ThanhTien"].Caption = "Thành tiền";
                //
                date.ReadOnly = false;
                luetenncc.ReadOnly = false;
            }
            else // ----- CHỈNH SỬA -----
            {
                lblheader.Text = "CHỈNH SỬA PHIẾU TRẢ HÀNG" + " " + MaPhieuTra;

                LoadData();           // Thông tin phiếu
                LoadChiTietPhieu();   // Dòng chi tiết thuốc
                gridViewnhaplieu.OptionsBehavior.Editable = true;
            }

            CaiDatMauChoCacNut();
            gridViewnhaplieu.DataSourceChanged += (s, ev) =>
            {
                if (luetenncc.EditValue != null)
                {
                    string maNCC = luetenncc.EditValue.ToString();
                    LoadVaGanDanhSachThuoc(maNCC);
                }
            };
        }
        private void gridViewnhaplieu_RowCountChanged(object sender, EventArgs e)
        {
            TinhTongTien();
        }

        private void Luetenncc_EditValueChanged(object sender, EventArgs e)
        {
            if (luetenncc.EditValue != null)
            {
                LoadVaGanDanhSachThuoc(luetenncc.EditValue.ToString());
            }
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

        
        private void LoadVaGanDanhSachThuoc(string maNCC)
        {
            string connectionString = @"Data Source=MINHTHUVU\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False";
            string query = "SELECT MaThuoc, TenThuoc, GiaBan AS DonGia FROM Thuoc WHERE MaNCC = @MaNCC";

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

            if (repoMaThuoc.Columns["DonGia"] != null)
                repoMaThuoc.Columns["DonGia"].Visible = false;
            if (repoMaThuoc.Columns["TenThuoc"] != null)
                repoMaThuoc.Columns["TenThuoc"].Caption = "Tên thuốc";

            // ⚙️ Gọi sau khi grid đã có DataSource
            this.BeginInvoke(new Action(() =>
            {
                if (gridViewnhaplieu.Columns["MaThuoc"] == null)
                    gridViewnhaplieu.PopulateColumns();

                gridControlnhaplieu.RepositoryItems.Add(repoMaThuoc);
                if (gridViewnhaplieu.Columns["MaThuoc"] != null)
                    gridViewnhaplieu.Columns["MaThuoc"].ColumnEdit = repoMaThuoc;
            }));
        }


        private void gridViewnhaplieu_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;

            // Khi chọn mã thuốc
            if (e.Column.FieldName == "MaThuoc")
            {
                string maThuoc = e.Value?.ToString();
                if (string.IsNullOrEmpty(maThuoc)) return;

                DataRow[] rows = dsThuoc.Select($"MaThuoc = '{maThuoc}'");
                if (rows.Length > 0)
                {
                    view.SetRowCellValue(e.RowHandle, "TenThuoc", rows[0]["TenThuoc"].ToString());

                    if (decimal.TryParse(rows[0]["DonGia"].ToString(), out decimal donGia))
                        view.SetRowCellValue(e.RowHandle, "DonGia", donGia);
                }
            }

            // Khi nhập số lượng hoặc đơn giá => tính thành tiền
            if (e.Column.FieldName == "SoLuong" || e.Column.FieldName == "DonGia")
            {
                object valSoLuong = view.GetRowCellValue(e.RowHandle, "SoLuong");
                object valDonGia = view.GetRowCellValue(e.RowHandle, "DonGia");

                decimal soLuong = 0;
                decimal donGia = 0;

                if (valSoLuong != null && valSoLuong != DBNull.Value)
                    decimal.TryParse(valSoLuong.ToString(), out soLuong);

                if (valDonGia != null && valDonGia != DBNull.Value)
                    decimal.TryParse(valDonGia.ToString(), out donGia);

                view.SetRowCellValue(e.RowHandle, "ThanhTien", soLuong * donGia);
            }

            // Cập nhật tổng tiền
            TinhTongTien();
        }



        private void LoadChiTietPhieu()
        {
            string constr = @"Data Source=MINHTHUVU\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False";
            string sql = @"
        SELECT 
            ctp.MaThuoc, 
            t.TenThuoc, 
            ctp.SoLuong, 
            ctp.DonGia, 
            ctp.ThanhTien
        FROM ChiTietPhieuTraHang ctp
        INNER JOIN Thuoc t ON ctp.MaThuoc = t.MaThuoc
        WHERE ctp.MaPhieuTra = @MaPhieuTra";

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(constr))
            using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
            {
                da.SelectCommand.Parameters.AddWithValue("@MaPhieuTra", lblmaphieu.Text);
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
            if (gridViewnhaplieu.Columns["DonGia"] != null)
                gridViewnhaplieu.Columns["DonGia"].Caption = "Đơn giá";
            if (gridViewnhaplieu.Columns["ThanhTien"] != null)
                gridViewnhaplieu.Columns["ThanhTien"].Caption = "Thành tiền";

            // 🔹 Quan trọng: gọi LoadVaGanDanhSachThuoc SAU khi DataSource đã có cột MaThuoc
            this.BeginInvoke(new Action(() =>
            {
                if (luetenncc.EditValue != null)
                {
                    LoadVaGanDanhSachThuoc(luetenncc.EditValue.ToString());

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
                    string sql = "SELECT * FROM PhieuTraHang p JOIN NhaCungCap n ON p.MaNCC = n.MaNCC JOIN NhanVien nv ON p.MaNV = nv.MaNV WHERE MaPhieuTra = @MaPhieuTra";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaPhieuTra", MaPhieuTra);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblmaphieu.Text = reader["MaPhieuTra"].ToString();
                                date.Text = reader["NgayTra"].ToString();
                                luetenncc.EditValue = reader["MaNCC"].ToString();
                                luenv.EditValue = reader["MaNV"].ToString();
                                caltongtien.Text = reader["TongTien"].ToString();
                                txtlydo.Text = reader["LyDoTra"].ToString();

                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy phiếu trả hàng với mã phiếu đã cho.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnhuy_Click(object sender, EventArgs e)
        {
            ((doitra)this.Owner).gridViewphieutrahang.ClearSelection();
            this.Close();
        }

        private void btnprint_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    // Lấy dữ liệu chi tiết từ GridControl
                    DataTable chitiet = gridControlnhaplieu.DataSource as DataTable;

                    // Nếu có dữ liệu thì thêm cột và gán lý do
                    if (chitiet != null)
                    {
                        string lyDo = txtlydo.Text.Trim();

                        // Thêm cột nếu chưa có
                        if (!chitiet.Columns.Contains("LyDoTra"))
                            chitiet.Columns.Add("LyDoTra", typeof(string));

                        // Gán lý do cho tất cả dòng
                        foreach (DataRow row in chitiet.Rows)
                            row["LyDoTra"] = lyDo;
                    }
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


                    // Lấy thông tin NCC từ LookUpEdit
                    string nhaCC = luetenncc.Text; // tên nhà cung cấp
                    string soPhieuNhap = "";     
                    string maNCC = luetenncc.EditValue?.ToString();
                    string diaChi = "";

                    if (!string.IsNullOrEmpty(maNCC))
                    {
                        using (SqlConnection conn = new SqlConnection("Data Source=MINHTHUVU\\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False"))
                        {
                            conn.Open();
                            string sql = "SELECT DiaChi FROM NhaCungCap WHERE MaNCC = @MaNCC";

                            using (SqlCommand cmd = new SqlCommand(sql, conn))
                            {
                                cmd.Parameters.AddWithValue("@MaNCC", maNCC);
                                object result = cmd.ExecuteScalar();
                                if (result != null)
                                    diaChi = result.ToString();
                            }
                        }
                    }




                    // Lấy các thông tin còn lại
                    string maPhieu = lblmaphieu.Text;
                    DateTime ngayLap = date.DateTime;
                    string nguoiLap = luenv.Text;
                    decimal tongTien = Convert.ToDecimal(caltongtien.Text);
                    

                    // Lấy logo từ Resources
                    Image logo = Properties.Resources.logo; // đổi tên nếu khác

                    // Tạo report (phiếu trả, không có hình thức thanh toán)
                    phieudoitra rpt = new phieudoitra(
                        chitiet,
                        maPhieu,
                        ngayLap,
                        nhaCC,
                        diaChi,
                        soPhieuNhap,
                        nguoiLap,
                        tongTien,
                        logo
                      

                    );

                    // Hiển thị xem trước
                    ReportPrintTool printTool = new ReportPrintTool(rpt);
                    printTool.ShowPreviewDialog();
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
                string sql = "SELECT MaNCC, TenNCC FROM NhaCungCap";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                luetenncc.Properties.DataSource = dt;
                luetenncc.Properties.DisplayMember = "TenNCC";
                luetenncc.Properties.ValueMember = "MaNCC";
            }
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

        private void btnsave_Click(object sender, EventArgs e)
        {
            string maPhieuTra = lblmaphieu.Text?.Trim();
            if (string.IsNullOrEmpty(maPhieuTra))
            {
                MessageBox.Show("Mã phiếu không hợp lệ. Hãy tạo phiếu mới trước khi lưu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maNCC = luetenncc.EditValue?.ToString();
            string maNV = luenv.EditValue?.ToString();
            DateTime ngayTra = date.DateTime;
            string lyDoTra = txtlydo.Text.Trim();

            if (string.IsNullOrEmpty(maNV) || string.IsNullOrEmpty(maNCC))
            {
                MessageBox.Show("Vui lòng chọn đầy đủ thông tin nhân viên và nhà cung cấp.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal tongTien = 0;
            string connectionString = @"Data Source=MINHTHUVU\MINHTHU;Initial Catalog=QLBH_NhaThuoc;Integrated Security=True;Encrypt=False";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();

                try
                {
                    // Cập nhật hoặc thêm phiếu trả hàng
                    string sqlPhieu = @"
                IF EXISTS (SELECT 1 FROM PhieuTraHang WHERE MaPhieuTra=@MaPhieuTra)
                   UPDATE PhieuTraHang
                   SET MaNV=@MaNV, MaNCC=@MaNCC, LyDoTra=@LyDoTra, NgayTra=@NgayTra
                   WHERE MaPhieuTra=@MaPhieuTra
                ELSE
                   INSERT INTO PhieuTraHang(MaPhieuTra, MaNV, MaNCC, LyDoTra, NgayTra, TongTien)
                   VALUES(@MaPhieuTra, @MaNV, @MaNCC, @LyDoTra, @NgayTra, 0)";

                    using (SqlCommand cmd = new SqlCommand(sqlPhieu, conn, tran))
                    {
                        cmd.Parameters.AddWithValue("@MaPhieuTra", maPhieuTra);
                        cmd.Parameters.AddWithValue("@MaNV", maNV);
                        cmd.Parameters.AddWithValue("@MaNCC", maNCC);
                        cmd.Parameters.AddWithValue("@LyDoTra", lyDoTra);
                        cmd.Parameters.AddWithValue("@NgayTra", ngayTra);
                        cmd.ExecuteNonQuery();
                    }

                    // Cập nhật hoặc thêm chi tiết phiếu trả
                    for (int i = 0; i < gridViewnhaplieu.RowCount; i++)
                    {
                        string maThuoc = gridViewnhaplieu.GetRowCellValue(i, "MaThuoc")?.ToString();
                        if (string.IsNullOrWhiteSpace(maThuoc)) continue;

                        int soLuong = Convert.ToInt32(gridViewnhaplieu.GetRowCellValue(i, "SoLuong") ?? 0);
                        decimal donGia = Convert.ToDecimal(gridViewnhaplieu.GetRowCellValue(i, "DonGia") ?? 0);
                        if (soLuong <= 0 || donGia <= 0)
                        {
                            MessageBox.Show($"Dòng {i + 1}: Số lượng và đơn giá phải lớn hơn 0.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            tran.Rollback();
                            return;
                        }

                        decimal thanhTien = soLuong * donGia;
                        tongTien += thanhTien;

                        string maCTTra = gridViewnhaplieu.GetRowCellValue(i, "MaCTTra")?.ToString();

                        // Sinh mã chi tiết mới nếu chưa có
                        if (string.IsNullOrEmpty(maCTTra))
                        {
                            string sqlMaxCT = @"SELECT TOP 1 MaCTTra 
                                        FROM ChiTietPhieuTraHang 
                                        WHERE MaCTTra LIKE @MaPhieuTra + '-%' 
                                        ORDER BY MaCTTra DESC";
                            using (SqlCommand cmdMax = new SqlCommand(sqlMaxCT, conn, tran))
                            {
                                cmdMax.Parameters.AddWithValue("@MaPhieuTra", maPhieuTra);
                                object result = cmdMax.ExecuteScalar();
                                int nextNum = 1;
                                if (result != null)
                                {
                                    string lastMa = result.ToString();
                                    nextNum = int.Parse(lastMa.Split('-')[1]) + 1;
                                }
                                maCTTra = $"{maPhieuTra}-{nextNum:D2}";
                            }
                        }

                        // cập nhật hoặc thêm chi tiết
                        string sqlCT = @"
                        IF EXISTS (SELECT 1 FROM ChiTietPhieuTraHang WHERE MaPhieuTra=@MaPhieuTra AND MaThuoc=@MaThuoc)
                           UPDATE ChiTietPhieuTraHang
                           SET SoLuong=@SoLuong, DonGia=@DonGia, ThanhTien=@ThanhTien
                           WHERE MaPhieuTra=@MaPhieuTra AND MaThuoc=@MaThuoc
                        ELSE
                           INSERT INTO ChiTietPhieuTraHang(MaCTTra, MaPhieuTra, MaThuoc, SoLuong, DonGia, ThanhTien)
                           VALUES(@MaCTTra, @MaPhieuTra, @MaThuoc, @SoLuong, @DonGia, @ThanhTien)";

                        using (SqlCommand cmdCT = new SqlCommand(sqlCT, conn, tran))
                        {
                            cmdCT.Parameters.AddWithValue("@MaCTTra", maCTTra);
                            cmdCT.Parameters.AddWithValue("@MaPhieuTra", maPhieuTra);
                            cmdCT.Parameters.AddWithValue("@MaThuoc", maThuoc);
                            cmdCT.Parameters.AddWithValue("@SoLuong", soLuong);
                            cmdCT.Parameters.AddWithValue("@DonGia", donGia);
                            cmdCT.Parameters.AddWithValue("@ThanhTien", thanhTien);
                            cmdCT.ExecuteNonQuery();
                        }

                        // Cập nhật lại grid
                        gridViewnhaplieu.SetRowCellValue(i, "MaCTTra", maCTTra);
                        gridViewnhaplieu.SetRowCellValue(i, "ThanhTien", thanhTien);
                    }

                    // cậ nhật vào phieu trả tổng tiền
                    string sqlUpdateTong = "UPDATE PhieuTraHang SET TongTien=@TongTien WHERE MaPhieuTra=@MaPhieuTra";
                    using (SqlCommand cmdTong = new SqlCommand(sqlUpdateTong, conn, tran))
                    {
                        cmdTong.Parameters.AddWithValue("@MaPhieuTra", maPhieuTra);
                        cmdTong.Parameters.AddWithValue("@TongTien", tongTien);
                        cmdTong.ExecuteNonQuery();
                    }

                    tran.Commit();
                    caltongtien.EditValue = tongTien;
                    MessageBox.Show("Lưu phiếu trả hàng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    try { tran.Rollback(); } catch { }
                    MessageBox.Show("Lỗi khi lưu phiếu trả hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }


}






