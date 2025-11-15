/* =========================
   0) TVP TYPES
   ========================= */
IF TYPE_ID('dbo.tvp_CTPN') IS NULL
CREATE TYPE dbo.tvp_CTPN AS TABLE
(
    MaThuoc     CHAR(10)       NOT NULL,
    SoLuong     INT            NOT NULL CHECK (SoLuong > 0),
    DonGiaNhap  DECIMAL(18,2)  NOT NULL CHECK (DonGiaNhap >= 0),
    HanSuDung   DATE           NULL
);
GO
IF TYPE_ID('dbo.tvp_CTHD') IS NULL
CREATE TYPE dbo.tvp_CTHD AS TABLE
(
    MaThuoc     CHAR(10)       NOT NULL,
    SoLuong     INT            NOT NULL CHECK (SoLuong > 0),
    DonGia      DECIMAL(18,2)  NULL  -- NULL => lấy Thuoc.GiaBan
);
GO
IF TYPE_ID('dbo.tvp_CTTra') IS NULL
CREATE TYPE dbo.tvp_CTTra AS TABLE
(
    MaThuoc     CHAR(10)       NOT NULL,
    SoLuong     INT            NOT NULL CHECK (SoLuong > 0),
    DonGia      DECIMAL(18,2)  NULL,
    MaLoSX      CHAR(10)       NULL, -- chỉ định lô; NULL => FEFO
    GhiChu      NVARCHAR(200)  NULL
);
GO

/* =========================
   1) SEQUENCES
   ========================= */
IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name='Seq_PhieuNhap')
    CREATE SEQUENCE dbo.Seq_PhieuNhap START WITH 1 INCREMENT BY 1;
IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name='Seq_LoSX')
    CREATE SEQUENCE dbo.Seq_LoSX START WITH 1 INCREMENT BY 1;
IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name='Seq_CTPN')
    CREATE SEQUENCE dbo.Seq_CTPN START WITH 1 INCREMENT BY 1;
IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name='Seq_CTHD')
    CREATE SEQUENCE dbo.Seq_CTHD START WITH 1 INCREMENT BY 1;
IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name='Seq_PhieuTra')
    CREATE SEQUENCE dbo.Seq_PhieuTra START WITH 1 INCREMENT BY 1;
IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name='Seq_CTPT')
    CREATE SEQUENCE dbo.Seq_CTPT START WITH 1 INCREMENT BY 1;
GO

/* =========================
   2) INDEXES
   ========================= */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_HoaDon_NgayLap')
    CREATE INDEX IX_HoaDon_NgayLap ON dbo.HoaDon(NgayLap);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_HoaDon_MaKH')
    CREATE INDEX IX_HoaDon_MaKH ON dbo.HoaDon(MaKH);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_HoaDon_MaNV')
    CREATE INDEX IX_HoaDon_MaNV ON dbo.HoaDon(MaNV);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_PhieuNhap_NgayNhap')
    CREATE INDEX IX_PhieuNhap_NgayNhap ON dbo.PhieuNhap(NgayNhap);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_PhieuNhap_MaNCC')
    CREATE INDEX IX_PhieuNhap_MaNCC ON dbo.PhieuNhap(MaNCC);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_TonKho_Thuoc_HSD')
    CREATE INDEX IX_TonKho_Thuoc_HSD ON dbo.TonKho(MaThuoc, HanSuDung) INCLUDE (TonKho);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_CTHD_MaHoaDon')
    CREATE INDEX IX_CTHD_MaHoaDon ON dbo.ChiTietHoaDon(MaHoaDon);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_CTPN_MaPhieuNhap')
    CREATE INDEX IX_CTPN_MaPhieuNhap ON dbo.ChiTietPhieuNhap(MaPhieuNhap);
GO

/* =========================
   3) VIEWS (mỗi VIEW mở đầu batch)
   ========================= */
CREATE OR ALTER VIEW dbo.v_TonKho_TheoThuoc
AS
SELECT  t.MaThuoc, th.TenThuoc,
        SUM(t.TonKho) AS Ton,
        MIN(t.HanSuDung) AS HanGanNhat
FROM dbo.TonKho t
JOIN dbo.Thuoc th ON th.MaThuoc = t.MaThuoc
GROUP BY t.MaThuoc, th.TenThuoc;
GO

CREATE OR ALTER VIEW dbo.v_HoaDon_List
AS
SELECT  h.MaHoaDon, h.NgayLap, h.TongTien,
        kh.TenKH, nv.TenNV, pttt.TenPTTT,
        STRING_AGG(CONCAT(ct.MaThuoc, N' x', ct.SoLuong), N', ') AS SanPhamRutGon
FROM dbo.HoaDon h
LEFT JOIN dbo.ChiTietHoaDon ct ON ct.MaHoaDon = h.MaHoaDon
LEFT JOIN dbo.KhachHang kh ON kh.MaKH = h.MaKH
LEFT JOIN dbo.NhanVien nv ON nv.MaNV = h.MaNV
LEFT JOIN dbo.PhuongThucTT pttt ON pttt.MaPTTT = h.MaPTTT
GROUP BY h.MaHoaDon, h.NgayLap, h.TongTien, kh.TenKH, nv.TenNV, pttt.TenPTTT;
GO

CREATE OR ALTER VIEW dbo.v_PhieuNhap_List
AS
SELECT  pn.MaPhieuNhap, pn.NgayNhap, pn.TongTien,
        ncc.TenNCC, nv.TenNV, pttt.TenPTTT
FROM dbo.PhieuNhap pn
JOIN dbo.NhaCungCap ncc ON ncc.MaNCC = pn.MaNCC
JOIN dbo.NhanVien nv ON nv.MaNV = pn.MaNV
LEFT JOIN dbo.PhuongThucTT pttt ON pttt.MaPTTT = pn.MaPTTT;
GO

CREATE OR ALTER VIEW dbo.v_BaoCaoNhap
AS
SELECT CAST(pn.NgayNhap AS date) AS Ngay,
       pn.MaPhieuNhap, pn.MaNCC, ncc.TenNCC,
       SUM(ct.ThanhTien) AS GiaTriNhap
FROM dbo.PhieuNhap pn
JOIN dbo.ChiTietPhieuNhap ct ON ct.MaPhieuNhap = pn.MaPhieuNhap
JOIN dbo.NhaCungCap ncc ON ncc.MaNCC = pn.MaNCC
GROUP BY CAST(pn.NgayNhap AS date), pn.MaPhieuNhap, pn.MaNCC, ncc.TenNCC;
GO

CREATE OR ALTER VIEW dbo.v_BaoCaoDoiTra
AS
SELECT CAST(pt.NgayTra AS date) AS Ngay,
       pt.MaPhieuTra, pt.MaNCC, ncc.TenNCC,
       SUM(ct.ThanhTien) AS GiaTriTra,
       pt.LyDoTra, pt.TrangThai
FROM dbo.PhieuTraHang pt
JOIN dbo.ChiTietPhieuTraHang ct ON ct.MaPhieuTra = pt.MaPhieuTra
JOIN dbo.NhaCungCap ncc ON ncc.MaNCC = pt.MaNCC
GROUP BY CAST(pt.NgayTra AS date), pt.MaPhieuTra, pt.MaNCC, ncc.TenNCC, pt.LyDoTra, pt.TrangThai;
GO

/* =========================
   4) PROC – NHẬP KHO
   ========================= */


/* =========================
   5) PROC – XUẤT KHO (FEFO)
   ========================= */
CREATE OR ALTER PROCEDURE dbo.usp_XuatKho_TaoHoaDon
    @MaKH      CHAR(10) = NULL,
    @MaNV      CHAR(10),
    @MaPTTT    CHAR(10) = NULL,
    @GhiChu    NVARCHAR(255) = NULL,
    @ChiTiet   dbo.tvp_CTHD READONLY
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM @ChiTiet)
            THROW 50002, N'Chi tiết hóa đơn trống.', 1;

        BEGIN TRAN;

        DECLARE @t TABLE(MaHoaDon CHAR(10));
        INSERT INTO dbo.HoaDon (MaKH, MaNV, NgayLap, MaPTTT, GhiChu, TongTien)
        OUTPUT inserted.MaHoaDon INTO @t(MaHoaDon)
        VALUES (@MaKH, @MaNV, SYSDATETIME(), @MaPTTT, @GhiChu, 0);

        DECLARE @MaHoaDon CHAR(10) = (SELECT TOP 1 MaHoaDon FROM @t);

        DECLARE @MaThuoc CHAR(10), @SLReq INT, @DonGia DECIMAL(18,2);

        DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
            SELECT c.MaThuoc, c.SoLuong, COALESCE(c.DonGia, t.GiaBan)
            FROM @ChiTiet c
            JOIN dbo.Thuoc t ON t.MaThuoc = c.MaThuoc;
        OPEN cur;
        FETCH NEXT FROM cur INTO @MaThuoc, @SLReq, @DonGia;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            DECLARE @ConCan INT = @SLReq;

            DECLARE c2 CURSOR LOCAL FAST_FORWARD FOR
                SELECT MaLoSX, TonKho
                FROM dbo.TonKho
                WHERE MaThuoc=@MaThuoc AND TonKho>0
                ORDER BY CASE WHEN HanSuDung IS NULL THEN 1 ELSE 0 END, HanSuDung;
            DECLARE @MaLo CHAR(10), @SLCo INT;
            OPEN c2;
            FETCH NEXT FROM c2 INTO @MaLo, @SLCo;

            WHILE @@FETCH_STATUS = 0 AND @ConCan > 0
            BEGIN
                DECLARE @Lay INT = IIF(@SLCo >= @ConCan, @ConCan, @SLCo);
                IF @Lay > 0
                BEGIN
                    DECLARE @seqCT BIGINT = NEXT VALUE FOR dbo.Seq_CTHD;
                    DECLARE @MaCTHD CHAR(10) = CONCAT('CB', RIGHT(CONCAT(REPLICATE('0',6), CAST(@seqCT AS VARCHAR(10))), 6));

                    INSERT INTO dbo.ChiTietHoaDon
                        (MaCTHD, MaHoaDon, MaThuoc, SoLuong, DonGia, ThanhTien, TrangThai)
                    VALUES (@MaCTHD, @MaHoaDon, @MaThuoc, @Lay, @DonGia, @Lay*@DonGia, N'Bán');

                    UPDATE dbo.TonKho
                        SET TonKho = TonKho - @Lay
                    WHERE MaThuoc=@MaThuoc AND MaLoSX=@MaLo;

                    SET @ConCan -= @Lay;
                END
                FETCH NEXT FROM c2 INTO @MaLo, @SLCo;
            END
            CLOSE c2; DEALLOCATE c2;

            IF @ConCan > 0
            BEGIN
                DECLARE @msg NVARCHAR(200) = CONCAT(N'Tồn kho không đủ cho thuốc ', @MaThuoc, N'.');
                THROW 50003, @msg, 1;
            END

            FETCH NEXT FROM cur INTO @MaThuoc, @SLReq, @DonGia;
        END
        CLOSE cur; DEALLOCATE cur;

        UPDATE h
           SET TongTien = x.Tong
        FROM dbo.HoaDon h
        CROSS APPLY (SELECT SUM(ThanhTien) AS Tong
                     FROM dbo.ChiTietHoaDon ct
                     WHERE ct.MaHoaDon = h.MaHoaDon) x
        WHERE h.MaHoaDon = @MaHoaDon;

        COMMIT;

        SELECT MaHoaDon=@MaHoaDon,
               TongTien=(SELECT SUM(ThanhTien) FROM dbo.ChiTietHoaDon WHERE MaHoaDon=@MaHoaDon);
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT>0 ROLLBACK;
        THROW;
    END CATCH
END
GO

/* =========================
   6) PROC – TRẢ/ĐỔI NCC
   ========================= */


/* =========================
   7) PROC – Xem tồn
   ========================= */
CREATE OR ALTER PROCEDURE dbo.usp_TonKho_Xem
    @MaThuoc CHAR(10) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT t.MaThuoc, th.TenThuoc, t.MaLoSX, t.TonKho, t.HanSuDung
    FROM dbo.TonKho t
    JOIN dbo.Thuoc th ON th.MaThuoc = t.MaThuoc
    WHERE (@MaThuoc IS NULL OR t.MaThuoc = @MaThuoc)
    ORDER BY t.MaThuoc, CASE WHEN t.HanSuDung IS NULL THEN 1 ELSE 0 END, t.HanSuDung;
END
GO

/* =========================
   8) PROC – Hủy phiếu nhập (demo)
   ========================= */
CREATE OR ALTER PROCEDURE dbo.usp_PhieuNhap_Huy
    @MaPhieuNhap CHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRAN;

        IF NOT EXISTS (SELECT 1 FROM dbo.PhieuNhap WHERE MaPhieuNhap=@MaPhieuNhap)
            THROW 50007, N'Không tìm thấy phiếu nhập.', 1;

        DECLARE @MaThuoc CHAR(10), @SL INT, @DG DECIMAL(18,2), @HSD DATE;

        DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
            SELECT MaThuoc, SoLuong, DonGiaNhap, HanSuDung
            FROM dbo.ChiTietPhieuNhap
            WHERE MaPhieuNhap=@MaPhieuNhap;

        OPEN cur;
        FETCH NEXT FROM cur INTO @MaThuoc, @SL, @DG, @HSD;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            DECLARE @MaLo CHAR(10);
            SELECT TOP 1 @MaLo = tk.MaLoSX
            FROM dbo.TonKho tk
            WHERE tk.MaThuoc=@MaThuoc
              AND ((tk.HanSuDung = @HSD) OR (tk.HanSuDung IS NULL AND @HSD IS NULL))
            ORDER BY tk.MaLoSX DESC;

            IF @MaLo IS NOT NULL
            BEGIN
                UPDATE dbo.TonKho SET TonKho = TonKho - @SL WHERE MaLoSX=@MaLo AND MaThuoc=@MaThuoc;
                IF EXISTS (SELECT 1 FROM dbo.TonKho WHERE MaLoSX=@MaLo AND MaThuoc=@MaThuoc AND TonKho < 0)
                    THROW 50008, N'Không thể hủy: tồn kho âm. Vui lòng tạo phiếu điều chỉnh.', 1;
            END

            FETCH NEXT FROM cur INTO @MaThuoc, @SL, @DG, @HSD;
        END
        CLOSE cur; DEALLOCATE cur;

        UPDATE dbo.PhieuNhap SET TrangThai=N'Đã hủy' WHERE MaPhieuNhap=@MaPhieuNhap;

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT>0 ROLLBACK;
        THROW;
    END CATCH
END
GO
