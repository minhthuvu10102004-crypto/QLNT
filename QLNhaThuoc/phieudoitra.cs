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
    public class phieudoitra : XtraReport
    {
        public phieudoitra(DataTable chitiet, string maPhieu, DateTime ngayLap,
                           string nhaCC, string diaChi, string soPhieuNhap,
                           string nguoiLap, decimal tongTien, Image? logo)
        {
            // --- Cấu hình khổ giấy ---
            this.PaperKind = DXPaperKind.A4;
            this.Margins = new DXMargins(50, 50, 50, 50);

            // --- Tham số ---
            this.Parameters.AddRange(new Parameter[]
            {
                new Parameter() { Name = "pMaPhieu", Value = maPhieu, Visible = false },
                new Parameter() { Name = "pNgayLap", Value = ngayLap.ToString("dd/MM/yyyy"), Visible = false },
                new Parameter() { Name = "pNCC", Value = nhaCC, Visible = false },
                new Parameter() { Name = "pDiaChi", Value = diaChi, Visible = false },
                new Parameter() { Name = "pSoPhieuNhap", Value = soPhieuNhap, Visible = false },
                new Parameter() { Name = "pNguoiLap", Value = nguoiLap, Visible = false },
                new Parameter() { Name = "pTongTien", Value = tongTien.ToString("N0") + " VNĐ", Visible = false },
                
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
                Font = new Font("Arial", 16, FontStyle.Bold),
                BoundsF = new RectangleF(350, 20, 350, 25),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
            };

            // Địa chỉ
            XRLabel lblDiaChi = new XRLabel()
            {
                Text = "Địa chỉ: 123 Giải Phóng, phường Hai Bà Trưng, Hà Nội",
                Font = new Font("Arial", 12),
                BoundsF = new RectangleF(350, 50, 350, 20),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
            };

            // SĐT
            XRLabel lblSDT = new XRLabel()
            {
                Text = "SDT: 0912345678 - 0987654321",
                Font = new Font("Arial", 12),
                BoundsF = new RectangleF(350, 70, 350, 20),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
            };

            // Tiêu đề
            XRLabel lblTieuDe = new XRLabel()
            {
                Text = "PHIẾU XUẤT THUỐC TRẢ LẠI NHÀ CUNG CẤP",
                Font = new Font("Arial", 16, FontStyle.Bold),
                BoundsF = new RectangleF(0, 150, this.PageWidth - 100, 30),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };

            // Ngày lập
            XRLabel lblNgay = new XRLabel()
            {
                ExpressionBindings = { new ExpressionBinding("BeforePrint", "Text", "'Ngày ' + ?pNgayLap") },
                Font = new Font("Arial", 10, FontStyle.Italic),
                BoundsF = new RectangleF(0, 180, this.PageWidth - 100, 20),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };

            // Số phiếu
            XRLabel lblSo = new XRLabel()
            {
                ExpressionBindings = { new ExpressionBinding("BeforePrint", "Text", "'Số: ' + ?pMaPhieu") },
                Font = new Font("Arial", 10, FontStyle.Italic),
                BoundsF = new RectangleF(0, 200, this.PageWidth - 100, 20),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };

            // Thông tin NCC (HTML format, có giãn dòng)
            XRLabel lblThongTin = new XRLabel()
            {
                AllowMarkupText = true,
                Font = new Font("Arial", 12),
                BoundsF = new RectangleF(0, 230, this.PageWidth - 100, 60),
                Padding = new DevExpress.XtraPrinting.PaddingInfo(20, 0, 0, 0)
            };
            lblThongTin.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text",
                "'<b>Nhà cung cấp:</b> ' + ?pNCC + '<br><br>' +" +
                "'<b>Địa chỉ:</b> ' + ?pDiaChi + '<br><br>' +" +
                "'<b>Số phiếu nhập liên quan:</b> ' + ?pSoPhieuNhap"));

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
            float[] colWidths = { 40, 80, 186, 80, 60, 80, 100, 100 };
            float totalWidth = 0;
            foreach (float w in colWidths) totalWidth += w;

            // Sửa lại tableLeft để căn giữa trong vùng in, cộng thêm Margin.Left
            float tableLeft = 0;


            // --- Header bảng ---
            PageHeaderBand pageHeader = new PageHeaderBand() { HeightF = 35 };
            XRTable tableHeader = new XRTable()
            {
                BoundsF = new RectangleF(tableLeft, 10, totalWidth, 35),  // Y=10 để bảng thấp hơn chút
                Borders = DevExpress.XtraPrinting.BorderSide.All,
                Font = new Font("Arial", 10, FontStyle.Bold),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                BackColor = Color.LightGray
            };
            XRTableRow rowHeader = new XRTableRow();
            string[] headers = { "STT", "Mã thuốc", "Tên thuốc", "Lý do", "ĐVT", "Số lượng", "Đơn giá", "Thành tiền" };
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
                Font = new Font("Arial", 10),
               
            };
            XRTableRow rowDetail = new XRTableRow();
            string[] bindings = { "[STT]", "[MaThuoc]", "[TenThuoc]", "[LyDoTra]", "[TenDonViTinh]", "[SoLuong]", "[DonGia]", "[ThanhTien]" };

            for (int i = 0; i < bindings.Length; i++)
            {
                XRTableCell cell = CreateCell(bindings[i], colWidths[i]);

                // 
                if (bindings[i] == "[DonGia]" || bindings[i] == "[ThanhTien]")
                {
                    cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                    cell.TextFormatString = "{0:n0}"; 
                }
                //nếu cột tên thuốc nhiều thì căn trái và xuống dòng
                else if (bindings[i] == "[TenThuoc]" || bindings[i] == "[TenDonViTinh]")
                {
                    cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
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
                else
                {
                    cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                }    

                rowDetail.Cells.Add(cell);
            }

            tableDetail.Rows.Add(rowDetail);
            detail.Controls.Add(tableDetail);
            this.Bands.Add(detail);


            // --- FOOTER (Tổng tiền khít tuyệt đối) ---
            ReportFooterBand footer = new ReportFooterBand() { HeightF = 160 };

            XRTable tblTongTien = new XRTable()
            {
                BoundsF = new RectangleF(tableLeft, 0, totalWidth, 30),
                Borders = DevExpress.XtraPrinting.BorderSide.All,
                Font = new Font("Arial", 10, FontStyle.Bold),
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

            // --- Khu vực chữ ký ---
            float yChuKy = 40; // Cố định cách trên của band
            float heightTop = 20;   // chiều cao dòng chức danh
            float heightBottom = 20; // chiều cao dòng (Ký, họ tên)
            float yChuKy2 = yChuKy + heightTop; // đặt dòng ký phía dưới

            // ======= DÒNG TRÊN =======
            XRLabel lblNguoiLap = new XRLabel()
            {
                Text = "Người lập phiếu",
                Font = new Font("Arial", 10),
                BoundsF = new RectangleF(5, yChuKy, 249, heightTop),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };

            XRLabel lblNguoiGiao = new XRLabel()
            {
                Text = "Người giao hàng",
                Font = new Font("Arial", 10),
                BoundsF = new RectangleF(254, yChuKy, 249, heightTop),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };

            XRLabel lblNCC = new XRLabel()
            {
                Text = "Nhà cung cấp",
                Font = new Font("Arial", 10),
                BoundsF = new RectangleF(503, yChuKy, 249, heightTop),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };

            // ======= DÒNG DƯỚI: (Ký, họ tên) =======
            XRLabel kyNguoiLap = new XRLabel()
            {
                Text = "(Ký, họ tên)",
                Font = new Font("Arial", 9, FontStyle.Italic),
                BoundsF = new RectangleF(5, yChuKy2, 249, heightBottom),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };

            XRLabel kyNguoiGiao = new XRLabel()
            {
                Text = "(Ký, họ tên)",
                Font = new Font("Arial", 9, FontStyle.Italic),
                BoundsF = new RectangleF(254, yChuKy2, 249, heightBottom),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };

            XRLabel kyNCC = new XRLabel()
            {
                Text = "(Ký, họ tên)",
                Font = new Font("Arial", 9, FontStyle.Italic),
                BoundsF = new RectangleF(503, yChuKy2, 249, heightBottom),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };

            // ======= THÊM VÀO FOOTER =======
            footer.Controls.AddRange(new XRControl[]
            {
            lblNguoiLap, lblNguoiGiao, lblNCC,
            kyNguoiLap, kyNguoiGiao, kyNCC
            });

            //
            XRLabel date = new XRLabel()
            {
                ExpressionBindings =
                {
                new ExpressionBinding("BeforePrint", "Text",
                "'Ngày ' + GetDay(?pNgayLap) + ' tháng ' + GetMonth(?pNgayLap) + ' năm ' + GetYear(?pNgayLap)")
                },
                Font = new Font("Arial", 10, FontStyle.Italic),
                BoundsF = new RectangleF(503, 42, 249, 20),  // Cố định
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };
            footer.Controls.Add(date);
            this.DataSource = chitiet;
            this.Bands.Add(footer);
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
