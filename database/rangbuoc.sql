
SET NOCOUNT ON;
GO

/* ========== 1) FOREIGN KEY ========== */
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_Thuoc_DanhMuc')
ALTER TABLE Thuoc ADD CONSTRAINT FK_Thuoc_DanhMuc
FOREIGN KEY (MaDanhMuc) REFERENCES DanhMucThuoc(MaDanhMuc);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_Thuoc_DonViTinh')
ALTER TABLE Thuoc ADD CONSTRAINT FK_Thuoc_DonViTinh
FOREIGN KEY (MaDonViTinh) REFERENCES DonViTinh(MaDonViTinh);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_Thuoc_NCC')
ALTER TABLE Thuoc ADD CONSTRAINT FK_Thuoc_NCC
FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_TonKho_Thuoc')
ALTER TABLE TonKho ADD CONSTRAINT FK_TonKho_Thuoc
FOREIGN KEY (MaThuoc) REFERENCES Thuoc(MaThuoc);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_HD_KH')
ALTER TABLE HoaDon ADD CONSTRAINT FK_HD_KH
FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_HD_NV')
ALTER TABLE HoaDon ADD CONSTRAINT FK_HD_NV
FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_HD_PTTT')
ALTER TABLE HoaDon ADD CONSTRAINT FK_HD_PTTT
FOREIGN KEY (MaPTTT) REFERENCES PhuongThucTT(MaPTTT);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_CTHD_HD')
ALTER TABLE ChiTietHoaDon ADD CONSTRAINT FK_CTHD_HD
FOREIGN KEY (MaHoaDon) REFERENCES HoaDon(MaHoaDon) ON DELETE CASCADE;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_CTHD_Thuoc')
ALTER TABLE ChiTietHoaDon ADD CONSTRAINT FK_CTHD_Thuoc
FOREIGN KEY (MaThuoc) REFERENCES Thuoc(MaThuoc);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_PN_NCC')
ALTER TABLE PhieuNhap ADD CONSTRAINT FK_PN_NCC
FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_PN_NV')
ALTER TABLE PhieuNhap ADD CONSTRAINT FK_PN_NV
FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_PN_PTTT')
ALTER TABLE PhieuNhap ADD CONSTRAINT FK_PN_PTTT
FOREIGN KEY (MaPTTT) REFERENCES PhuongThucTT(MaPTTT);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_CTPN_PN')
ALTER TABLE ChiTietPhieuNhap ADD CONSTRAINT FK_CTPN_PN
FOREIGN KEY (MaPhieuNhap) REFERENCES PhieuNhap(MaPhieuNhap) ON DELETE CASCADE;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_CTPN_Thuoc')
ALTER TABLE ChiTietPhieuNhap ADD CONSTRAINT FK_CTPN_Thuoc
FOREIGN KEY (MaThuoc) REFERENCES Thuoc(MaThuoc);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_PT_NCC')
ALTER TABLE PhieuTraHang ADD CONSTRAINT FK_PT_NCC
FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_PT_NV')
ALTER TABLE PhieuTraHang ADD CONSTRAINT FK_PT_NV
FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_CTPT_PT')
ALTER TABLE ChiTietPhieuTraHang ADD CONSTRAINT FK_CTPT_PT
FOREIGN KEY (MaPhieuTra) REFERENCES PhieuTraHang(MaPhieuTra) ON DELETE CASCADE;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_CTPT_Thuoc')
ALTER TABLE ChiTietPhieuTraHang ADD CONSTRAINT FK_CTPT_Thuoc
FOREIGN KEY (MaThuoc) REFERENCES Thuoc(MaThuoc);


/* ========== 2) UNIQUE TỐI THIỂU ========== */
IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name='UQ_DMT_Ten')
ALTER TABLE DanhMucThuoc ADD CONSTRAINT UQ_DMT_Ten UNIQUE (TenDanhMuc);

IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name='UQ_DVT_Ten')
ALTER TABLE DonViTinh ADD CONSTRAINT UQ_DVT_Ten UNIQUE (TenDonViTinh);

IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name='UQ_NCC_Ten')
ALTER TABLE NhaCungCap ADD CONSTRAINT UQ_NCC_Ten UNIQUE (TenNCC);

IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name='UQ_CTHD')
ALTER TABLE ChiTietHoaDon ADD CONSTRAINT UQ_CTHD UNIQUE (MaHoaDon, MaThuoc);


/* ========== 3) CHECK CƠ BẢN ========== */
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name='CK_Thuoc_GiaBan')
ALTER TABLE Thuoc ADD CONSTRAINT CK_Thuoc_GiaBan CHECK (GiaBan >= 0);

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name='CK_TonKho_KhongAm')
ALTER TABLE TonKho ADD CONSTRAINT CK_TonKho_KhongAm CHECK (TonKho >= 0);

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name='CK_CTHD_SL_DG')
ALTER TABLE ChiTietHoaDon ADD CONSTRAINT CK_CTHD_SL_DG CHECK (SoLuong > 0 AND DonGia >= 0);

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name='CK_CTPN_SL_DG')
ALTER TABLE ChiTietPhieuNhap ADD CONSTRAINT CK_CTPN_SL_DG CHECK (SoLuong > 0 AND DonGiaNhap >= 0);

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name='CK_CTPT_SL_DG')
ALTER TABLE ChiTietPhieuTraHang ADD CONSTRAINT CK_CTPT_SL_DG CHECK (SoLuong > 0 AND DonGia >= 0);


/* ========== 4) DEFAULT  ========== */
IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name='DF_HD_NgayLap')
ALTER TABLE HoaDon ADD CONSTRAINT DF_HD_NgayLap DEFAULT (GETDATE()) FOR NgayLap;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name='DF_PN_NgayNhap')
ALTER TABLE PhieuNhap ADD CONSTRAINT DF_PN_NgayNhap DEFAULT (GETDATE()) FOR NgayNhap;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name='DF_PT_NgayTra')
ALTER TABLE PhieuTraHang ADD CONSTRAINT DF_PT_NgayTra DEFAULT (GETDATE()) FOR NgayTra;


/* ========== 5) SÍNH MÃ HÓA ĐƠN – SEQUENCE + DEFAULT ========== */
IF NOT EXISTS (SELECT * FROM sys.sequences WHERE name='Seq_HoaDon')
    CREATE SEQUENCE Seq_HoaDon START WITH 1 INCREMENT BY 1;
GO
-- Default sinh mã như HD000001 (không dùng trigger, không lỗi ISNULL)
IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE parent_object_id=OBJECT_ID('HoaDon') AND name='DF_HD_MaHoaDon')
ALTER TABLE HoaDon ADD CONSTRAINT DF_HD_MaHoaDon
    DEFAULT ('HD' + RIGHT('000000' + CAST(NEXT VALUE FOR Seq_HoaDon AS VARCHAR(6)), 6))
    FOR MaHoaDon;
GO


/* ========== 6) INDEX  ========== */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_HD_NgayLap')
CREATE INDEX IX_HD_NgayLap ON HoaDon(NgayLap DESC);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_CTHD_HD')
CREATE INDEX IX_CTHD_HD ON ChiTietHoaDon(MaHoaDon);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Thuoc_Ten')
CREATE INDEX IX_Thuoc_Ten ON Thuoc(TenThuoc);


/* ========== 7) VIEW TỔNG TIỀN (thay cho trigger cộng trừ) ========== */
GO
CREATE OR ALTER VIEW vw_HoaDon_Tong AS
SELECT 
    h.MaHoaDon, h.MaKH, h.MaNV, h.NgayLap, h.MaPTTT, h.GhiChu,
    SUM(ct.ThanhTien) AS TongTienTinh
FROM HoaDon h
LEFT JOIN ChiTietHoaDon ct ON ct.MaHoaDon = h.MaHoaDon
GROUP BY h.MaHoaDon, h.MaKH, h.MaNV, h.NgayLap, h.MaPTTT, h.GhiChu;
GO

/* ==========================================
   RÀNG BUỘC CHO QUẢN LÝ NHẬP HÀNG
   ========================================== */
SET NOCOUNT ON;
GO

/* ========== 1) CHECK LOGIC ========== */
/* Chi tiết phiếu nhập: SL > 0, giá ≥ 0, thành tiền = SL*Giá */
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name='CK_CTPN_SL_Gia_TT')
ALTER TABLE ChiTietPhieuNhap
ADD CONSTRAINT CK_CTPN_SL_Gia_TT
CHECK (SoLuong > 0 AND DonGiaNhap >= 0 AND ThanhTien = SoLuong * DonGiaNhap);

/* HSD cho lô nhập (nếu có) */
IF COL_LENGTH('dbo.ChiTietPhieuNhap','HanSuDung') IS NOT NULL
AND NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name='CK_CTPN_HSD')
ALTER TABLE ChiTietPhieuNhap
ADD CONSTRAINT CK_CTPN_HSD CHECK (HanSuDung IS NULL OR HanSuDung >= '2000-01-01');

/* Tổng tiền PN không âm (nếu có cột lưu) */
IF COL_LENGTH('dbo.PhieuNhap','TongTien') IS NOT NULL
AND NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name='CK_PN_TongTien')
ALTER TABLE PhieuNhap
ADD CONSTRAINT CK_PN_TongTien CHECK (TongTien >= 0);

/* Chi tiết phiếu trả: SL > 0, giá ≥ 0, thành tiền = SL*Giá */
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name='CK_CTPT_SL_Gia_TT')
ALTER TABLE ChiTietPhieuTraHang
ADD CONSTRAINT CK_CTPT_SL_Gia_TT
CHECK (SoLuong > 0 AND DonGia >= 0 AND ThanhTien = SoLuong * DonGia);

/* Tổng tiền trả không âm (nếu có cột lưu) */
IF COL_LENGTH('dbo.PhieuTraHang','TongTien') IS NOT NULL
AND NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name='CK_PT_TongTien')
ALTER TABLE PhieuTraHang
ADD CONSTRAINT CK_PT_TongTien CHECK (TongTien >= 0);


/* ========== 2) UNIQUE – chống trùng hàng trong 1 phiếu ========== */
IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name='UQ_CTPN_Phieu_Thuoc')
ALTER TABLE ChiTietPhieuNhap
ADD CONSTRAINT UQ_CTPN_Phieu_Thuoc UNIQUE (MaPhieuNhap, MaThuoc);

IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name='UQ_CTPT_Phieu_Thuoc')
ALTER TABLE ChiTietPhieuTraHang
ADD CONSTRAINT UQ_CTPT_Phieu_Thuoc UNIQUE (MaPhieuTra, MaThuoc);


/* ========== 3) DEFAULT – tự điền cho form tạo/sửa ========== */
IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name='DF_PN_NgayNhap')
ALTER TABLE PhieuNhap ADD CONSTRAINT DF_PN_NgayNhap DEFAULT (GETDATE()) FOR NgayNhap;

IF COL_LENGTH('dbo.PhieuNhap','TrangThai') IS NOT NULL
AND NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name='DF_PN_TrangThai')
ALTER TABLE PhieuNhap ADD CONSTRAINT DF_PN_TrangThai DEFAULT (N'Nháp') FOR TrangThai;

IF COL_LENGTH('dbo.PhieuTraHang','NgayTra') IS NOT NULL
AND NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name='DF_PT_NgayTra')
ALTER TABLE PhieuTraHang ADD CONSTRAINT DF_PT_NgayTra DEFAULT (GETDATE()) FOR NgayTra;

IF COL_LENGTH('dbo.PhieuTraHang','TrangThai') IS NOT NULL
AND NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name='DF_PT_TrangThai')
ALTER TABLE PhieuTraHang ADD CONSTRAINT DF_PT_TrangThai DEFAULT (N'Nháp') FOR TrangThai;

/* Trạng thái hợp lệ (nếu có cột) – phục vụ “Sửa/Hủy phiếu” */
IF COL_LENGTH('dbo.PhieuNhap','TrangThai') IS NOT NULL
AND NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name='CK_PN_TrangThai')
ALTER TABLE PhieuNhap
ADD CONSTRAINT CK_PN_TrangThai CHECK (TrangThai IN (N'Nháp', N'Đã xác nhận', N'Đã hủy'));

IF COL_LENGTH('dbo.PhieuTraHang','TrangThai') IS NOT NULL
AND NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name='CK_PT_TrangThai')
ALTER TABLE PhieuTraHang
ADD CONSTRAINT CK_PT_TrangThai CHECK (TrangThai IN (N'Nháp', N'Đã xác nhận', N'Đã hủy'));


/* ========== 4) INDEX – lọc theo ngày, mã phiếu, NCC ========== */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_PN_NgayNhap')
CREATE INDEX IX_PN_NgayNhap ON PhieuNhap(NgayNhap DESC);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_PN_MaNCC_Ngay')
CREATE INDEX IX_PN_MaNCC_Ngay ON PhieuNhap(MaNCC, NgayNhap DESC);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_CTPN_Phieu')
CREATE INDEX IX_CTPN_Phieu ON ChiTietPhieuNhap(MaPhieuNhap);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_PT_NgayTra')
CREATE INDEX IX_PT_NgayTra ON PhieuTraHang(NgayTra DESC);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_PT_MaNCC_Ngay')
CREATE INDEX IX_PT_MaNCC_Ngay ON PhieuTraHang(MaNCC, NgayTra DESC);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_CTPT_Phieu')
CREATE INDEX IX_CTPT_Phieu ON ChiTietPhieuTraHang(MaPhieuTra);


/* ========== 5) VIEW – tổng tiền cho màn danh sách/chi tiết ========== */
GO
CREATE OR ALTER VIEW vw_PhieuNhap_Tong AS
SELECT 
    p.MaPhieuNhap, p.MaNCC, p.MaNV, p.NgayNhap, p.MaPTTT, p.TrangThai,
    SUM(ct.ThanhTien) AS TongTienTinh
FROM PhieuNhap p
LEFT JOIN ChiTietPhieuNhap ct ON ct.MaPhieuNhap = p.MaPhieuNhap
GROUP BY p.MaPhieuNhap, p.MaNCC, p.MaNV, p.NgayNhap, p.MaPTTT, p.TrangThai;
GO

GO
CREATE OR ALTER VIEW vw_PhieuTraHang_Tong AS
SELECT 
    t.MaPhieuTra, t.MaNCC, t.MaNV, t.NgayTra, t.TrangThai,
    SUM(ct.ThanhTien) AS TongTienTinh
FROM PhieuTraHang t
LEFT JOIN ChiTietPhieuTraHang ct ON ct.MaPhieuTra = t.MaPhieuTra
GROUP BY t.MaPhieuTra, t.MaNCC, t.MaNV, t.NgayTra, t.TrangThai;
GO

/* ==========================================
   RÀNG BUỘC – QUẢN LÝ KHO THUỐC 
   ========================================== */
SET NOCOUNT ON;
GO

/* 1) UNIQUE – chống trùng tên thuốc theo nhà cung cấp (phù hợp pop-up Thêm thuốc) */
IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name='UQ_Thuoc_Ten_NCC')
ALTER TABLE Thuoc
ADD CONSTRAINT UQ_Thuoc_Ten_NCC UNIQUE (TenThuoc, MaNCC);

/* 2) CHECK/DEFAULT cho bảng TonKho (quản lô, hạn dùng, số lượng) */
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name='CK_TonKho_SoLuong_NonNegative')
ALTER TABLE TonKho
ADD CONSTRAINT CK_TonKho_SoLuong_NonNegative CHECK (TonKho >= 0);

IF COL_LENGTH('dbo.TonKho','HanSuDung') IS NOT NULL
AND NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name='CK_TonKho_HSD_Valid')
ALTER TABLE TonKho
ADD CONSTRAINT CK_TonKho_HSD_Valid CHECK (HanSuDung IS NULL OR HanSuDung >= '2000-01-01');

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name='DF_TonKho_SoLuong')
ALTER TABLE TonKho
ADD CONSTRAINT DF_TonKho_SoLuong DEFAULT(0) FOR TonKho;

/* 3) UNIQUE phụ cho lô – đảm bảo MaLoSX gắn đúng thuốc (phục vụ chỉnh sửa nhanh theo lô) */
IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name='UQ_TonKho_MaThuoc_MaLoSX')
ALTER TABLE TonKho
ADD CONSTRAINT UQ_TonKho_MaThuoc_MaLoSX UNIQUE (MaThuoc, MaLoSX);

/* 4) INDEX – tối ưu ô tìm kiếm & bộ lọc của bảng danh sách kho */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Thuoc_Ten_HoatChat')
CREATE INDEX IX_Thuoc_Ten_HoatChat ON Thuoc(TenThuoc, HoatChat);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Thuoc_MaNCC')
CREATE INDEX IX_Thuoc_MaNCC ON Thuoc(MaNCC);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_TonKho_MaThuoc')
CREATE INDEX IX_TonKho_MaThuoc ON TonKho(MaThuoc);

IF COL_LENGTH('dbo.TonKho','HanSuDung') IS NOT NULL
AND NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_TonKho_HanSuDung')
CREATE INDEX IX_TonKho_HanSuDung ON TonKho(HanSuDung);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_TonKho_MaThuoc_HSD')
CREATE INDEX IX_TonKho_MaThuoc_HSD ON TonKho(MaThuoc, HanSuDung);

/* 5) VIEW – dữ liệu hiển thị cho “Danh sách các thuốc” (kèm thông tin lô) */
GO
CREATE OR ALTER VIEW vw_Kho_DanhSachThuoc AS
SELECT
    t.MaThuoc,
    t.TenThuoc,
    t.HoatChat,
    t.DongGoi,
    dv.TenDonViTinh,
    ncc.TenNCC,
    t.GiaBan,
    k.MaLoSX,
    k.HanSuDung,
    k.TonKho AS SoLuongTon
FROM Thuoc t
LEFT JOIN TonKho k      ON k.MaThuoc = t.MaThuoc
LEFT JOIN DonViTinh dv  ON dv.MaDonViTinh = t.MaDonViTinh
LEFT JOIN NhaCungCap ncc ON ncc.MaNCC = t.MaNCC;
GO
