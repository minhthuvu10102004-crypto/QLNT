using DevExpress.Drawing;
using DevExpress.Drawing.Printing;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using System;
using System.Data;
using System.Drawing;

#nullable enable

namespace QLNhaThuoc
{
    public class phieunhapkho : XtraReport
    {
        public phieunhapkho(DataTable chitiet, string tenNCC, string maPhieu, DateTime ngayLap,
                          string nguoiLap, decimal tongTien, Image? logo)
        {
            // --- Cấu hình khổ giấy ---
            this.PaperKind = DXPaperKind.A4;
            this.Landscape = true;
            this.Margins = new DXMargins(80, 80, 80, 80);

            // --- Tham số ---
            this.Parameters.AddRange(new Parameter[]
            {
                new Parameter() { Name = "pMaPhieu", Value = maPhieu, Visible = false },
                new Parameter() { Name = "pNgayLap", Value = ngayLap.ToString("dd/MM/yyyy"), Visible = false },               
                new Parameter() { Name = "pNguoiLap", Value = nguoiLap, Visible = false },
                new Parameter() { Name = "pTongTien", Value = tongTien.ToString("N0") + " VNĐ", Visible = false },
                new Parameter() { Name = "pNCC", Value = tenNCC, Visible = false }
            });

            // --- HEADER ---
            ReportHeaderBand header = new ReportHeaderBand() { HeightF = 260 };

            // Logo
            XRPictureBox picLogo = new XRPictureBox()
            {
                ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource(logo ?? Properties.Resources.logo),
                BoundsF = new RectangleF(0, 0, 180, 130),
                Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage
            };

            // Tên nhà thuốc
            XRLabel lblTenNhaThuoc = new XRLabel()
            {
                Text = "NHÀ THUỐC ABC",
                Font = new Font("Arial", 17, FontStyle.Bold),
                BoundsF = new RectangleF(350, 20, 650, 25),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
            };

            // Địa chỉ
            XRLabel lblDiaChi = new XRLabel()
            {
                Text = "Địa chỉ: 123 Giải Phóng, phường Hai Bà Trưng, Hà Nội",
                Font = new Font("Arial", 13),
                BoundsF = new RectangleF(350, 50, 650, 25),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
            };

            // SĐT
            XRLabel lblSDT = new XRLabel()
            {
                Text = "SDT: 0912345678 - 0987654321",
                Font = new Font("Arial", 13),
                BoundsF = new RectangleF(350, 80, 650, 25),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
            };

            // Tiêu đề
            XRLabel lblTieuDe = new XRLabel()
            {
                Text = "PHIẾU NHẬP KHO",
                Font = new Font("Arial", 17, FontStyle.Bold),
                //căn giữa trang
                BoundsF = new RectangleF((this.PageWidth - 100) / 2 - 200, 130, 400, 30),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };

            // Ngày lập
            XRLabel lblNgay = new XRLabel()
            {
                ExpressionBindings = { new ExpressionBinding("BeforePrint", "Text", "'Ngày ' + ?pNgayLap") },
                Font = new Font("Arial", 11, FontStyle.Italic),
                BoundsF = new RectangleF((this.PageWidth - 100) / 2 - 200, 160, 400, 30),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };

            // Số phiếu
            XRLabel lblSo = new XRLabel()
            {
                ExpressionBindings = { new ExpressionBinding("BeforePrint", "Text", "'Số: ' + ?pMaPhieu") },
                Font = new Font("Arial", 11, FontStyle.Italic),
                BoundsF = new RectangleF((this.PageWidth - 100) / 2 - 200, 180, 400, 30),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };

            // Thông tin NCC (HTML format, có giãn dòng)
            XRLabel lblThongTin = new XRLabel()
            {
                AllowMarkupText = true,
                Font = new Font("Arial", 13, FontStyle.Regular),
                BoundsF = new RectangleF(0, 230, this.PageWidth - 100, 60),
                Padding = new DevExpress.XtraPrinting.PaddingInfo(20, 0, 0, 0)
            };
            lblThongTin.AllowMarkupText = true;
            lblThongTin.ExpressionBindings.Add(new ExpressionBinding(
    "BeforePrint",
    "Text",
    "'<b>Họ và tên người giao:...............................................................................................</b><br><br>' + " +
    "'<b>Theo: BB bàn giao hàng hóa ngày ' + ?pNgayLap + ' của ' + ?pNCC + '</b><br><br>' + " +
    "'<b>Nhập tại kho: ABC01 địa điểm 300 Giải Phóng, phường Hai Bà Trưng, Hà Nội</b>'"
));

            header.Controls.AddRange(new XRControl[]
            {
                picLogo, lblTenNhaThuoc, lblDiaChi, lblSDT, lblTieuDe, lblNgay, lblSo, lblThongTin
            });
            this.Bands.Add(header);

            // --- BODY ---
            AddTableAndFooter(chitiet);
        }

        private void AddTableAndFooter(DataTable chitiet)
        {
            // --- Chiều rộng tổng thể (tính đúng tổng cột) ---
            float[] colWidths = { 40, 125, 223, 130, 130, 80, 80, 100, 100 };
            float totalWidth = 0;
            foreach (float w in colWidths) totalWidth += w;

            float tableLeft = 0; // căn giữa trong vùng in (có thể cộng Margin.Left nếu muốn)

            // --- Header bảng ---
            PageHeaderBand pageHeader = new PageHeaderBand() { HeightF = 35 };
            XRTable tableHeader = new XRTable()
            {
                BoundsF = new RectangleF(tableLeft, 10, totalWidth, 35),
                Borders = DevExpress.XtraPrinting.BorderSide.All,
                Font = new Font("Arial", 12, FontStyle.Bold),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                BackColor = Color.LightGray
            };
            XRTableRow rowHeader = new XRTableRow();
            string[] headers = { "STT", "Tên danh mục", "Tên thuốc", "NSX", "HSD", "ĐVT", "Số lượng", "Đơn giá nhập", "Thành tiền" };
            for (int i = 0; i < headers.Length; i++)
                rowHeader.Cells.Add(new XRTableCell() { Text = headers[i], WidthF = colWidths[i] });
            tableHeader.Rows.Add(rowHeader);
            pageHeader.Controls.Add(tableHeader);
            this.Bands.Add(pageHeader);

            // --- Dòng chi tiết ---
            DetailBand detail = new DetailBand() { HeightF = 25 };
            XRTable tableDetail = new XRTable()
            {
                BoundsF = new RectangleF(tableLeft, 0, totalWidth, 25),
                Borders = DevExpress.XtraPrinting.BorderSide.All,
                Font = new Font("Arial", 12)
            };
            XRTableRow rowDetail = new XRTableRow();
            string[] bindings = { "[STT]", "[TenDanhMuc]", "[TenThuoc]", "[NgaySanXuat]", "[HanSuDung]", "[TenDonViTinh]", "[SoLuong]", "[DonGiaNhap]", "[ThanhTien]" };

            for (int i = 0; i < bindings.Length; i++)
            {
                XRTableCell cell = CreateCell(bindings[i], colWidths[i]);

                if (bindings[i] == "[DonGiaNhap]" || bindings[i] == "[ThanhTien]")
                {
                    cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                    cell.TextFormatString = "{0:n0}";
                }
                else if (bindings[i] == "[TenThuoc]" || bindings[i] == "[TenDonViTinh]")
                {
                    cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                    cell.WordWrap = true;
                }
                else if (bindings[i] == "[STT]")
                {
                    cell = CreateCell("", colWidths[i]);
                    cell.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "sumRecordNumber()"));
                    cell.Summary = new XRSummary()
                    {
                        Func = SummaryFunc.RecordNumber,
                        Running = SummaryRunning.Report
                    };
                    cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                }
                else if (bindings[i] == "[NgaySanXuat]" || bindings[i] == "[HanSuDung]")
                {
                    cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    cell.TextFormatString = "{0:dd/MM/yyyy}";
                }
                else
                {
                    cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                }

                rowDetail.Cells.Add(cell);
            }

            tableDetail.Rows.Add(rowDetail);
            detail.Controls.Add(tableDetail);
            this.Bands.Add(detail);

            // --- FOOTER ---
            ReportFooterBand footer = new ReportFooterBand() { HeightF = 300 }; // tăng cao để chứa label

            // Bảng tổng tiền
            XRTable tblTongTien = new XRTable()
            {
                BoundsF = new RectangleF(tableLeft, 0, totalWidth, 30),
                Borders = DevExpress.XtraPrinting.BorderSide.All,
                Font = new Font("Arial", 12, FontStyle.Bold),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
            };

            XRTableRow rowTong = new XRTableRow();
            XRTableCell cellTong = new XRTableCell()
            {
                Text = "Tổng tiền",
                WidthF = totalWidth - 100,
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 10, 0, 0)
            };
            XRTableCell cellGiaTri = new XRTableCell()
            {
                WidthF = 100,
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
            };
            cellGiaTri.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "?pTongTien"));
            rowTong.Cells.AddRange(new XRTableCell[] { cellTong, cellGiaTri });
            tblTongTien.Rows.Add(rowTong);
            footer.Controls.Add(tblTongTien);

            // 
            XRLabel lblTienChu = new XRLabel()
            {
                Font = new Font("Arial", 12, FontStyle.Italic),
                BoundsF = new RectangleF(tableLeft, 35, totalWidth, 25),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
            };
            lblTienChu.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "'Tổng tiền bằng chữ: ' + ?pTongTienChu"));
            footer.Controls.Add(lblTienChu);

            // 
            float yChuKy = 80;
            float heightTop = 30;      
            float heightBottom = 20;  

            // 
            XRLabel lblNguoiLap = new XRLabel()
            {
                Text = "Người lập phiếu",
                Font = new Font("Arial", 12),
                BoundsF = new RectangleF(5, yChuKy, 249, heightTop),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };

            XRLabel lblNguoiGiao = new XRLabel()
            {
                Text = "Người giao hàng",
                Font = new Font("Arial", 12),
                BoundsF = new RectangleF(254, yChuKy, 249, heightTop),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };

            XRLabel lblNCC = new XRLabel()
            {
                Text = "Thủ kho",
                Font = new Font("Arial", 12),
                BoundsF = new RectangleF(503, yChuKy, 249, heightTop),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };

            XRLabel lblKeToan = new XRLabel()
            {
                Text = "Kế toán trưởng",
                Font = new Font("Arial", 12),
                BoundsF = new RectangleF(752, yChuKy, 249, heightTop),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };

            // ----- DÒNG DƯỚI (ký, họ tên) -----
            float yChuKy2 = yChuKy + heightTop - 5; // Dịch xuống một chút cho đẹp

            XRLabel kyNguoiLap = new XRLabel()
            {
                Text = "(Ký, họ tên)",
                Font = new Font("Arial", 10, FontStyle.Italic),
                BoundsF = new RectangleF(5, yChuKy2, 249, heightBottom),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };

            XRLabel kyNguoiGiao = new XRLabel()
            {
                Text = "(Ký, họ tên)",
                Font = new Font("Arial", 10, FontStyle.Italic),
                BoundsF = new RectangleF(254, yChuKy2, 249, heightBottom),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };

            XRLabel kyNCC = new XRLabel()
            {
                Text = "(Ký, họ tên)",
                Font = new Font("Arial", 10, FontStyle.Italic),
                BoundsF = new RectangleF(503, yChuKy2, 249, heightBottom),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };

            XRLabel kyKeToan = new XRLabel()
            {
                Text = "(Ký, họ tên)",
                Font = new Font("Arial", 10, FontStyle.Italic),
                BoundsF = new RectangleF(752, yChuKy2, 249, heightBottom),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };

            // thêm vào footer
            footer.Controls.AddRange(new XRControl[] {
            lblNguoiLap, lblNguoiGiao, lblNCC, lblKeToan,
            kyNguoiLap, kyNguoiGiao, kyNCC, kyKeToan
            });


            // Ngày tháng
            XRLabel date = new XRLabel()
            {
                ExpressionBindings =
                {
                new ExpressionBinding("BeforePrint", "Text",
                "'Ngày ' + GetDay(?pNgayLap) + ' tháng ' + GetMonth(?pNgayLap) + ' năm ' + GetYear(?pNgayLap)")
                },
                Font = new Font("Arial", 12, FontStyle.Italic),
                BoundsF = new RectangleF(600, 65, 550, 20),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };
            footer.Controls.Add(date);

            this.Bands.Add(footer);

            this.DataSource = chitiet;
        }


        private XRTableCell CreateCell(string binding, float width)
        {
            XRTableCell cell = new XRTableCell()
            {
                WidthF = width,
                Borders = DevExpress.XtraPrinting.BorderSide.All,
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };
            cell.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", binding));
            return cell;
        }
    }
}
