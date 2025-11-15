

==
CREATE OR ALTER PROCEDURE dbo.usp_TraNCC_CapNhatKho
    @MaPhieuTra VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRAN;

        DECLARE @MaThuoc CHAR(10), 
                @SLReq INT, 
                @DonGia DECIMAL(18,2), 
                @MaLo CHAR(10);

        -- Cursor lấy chi tiết phiếu
        DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
            SELECT ct.MaThuoc, ct.SoLuong, ct.DonGia, tk.MaLoSX
            FROM dbo.ChiTietPhieuTraHang ct
            LEFT JOIN dbo.TonKho tk ON ct.MaThuoc = tk.MaThuoc
            WHERE ct.MaPhieuTra = @MaPhieuTra;

        OPEN cur;
        FETCH NEXT FROM cur INTO @MaThuoc, @SLReq, @DonGia, @MaLo;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            IF @MaLo IS NOT NULL
            BEGIN
                DECLARE @SLCo INT;
                SELECT @SLCo = TonKho 
                FROM dbo.TonKho 
                WHERE MaThuoc = @MaThuoc AND MaLoSX = @MaLo;

                IF @SLCo IS NULL OR @SLCo < @SLReq
                BEGIN
                    -- Nếu lô không đủ, chuyển sang phân bổ tự động
                    SET @MaLo = NULL;
                END
                ELSE
                BEGIN
                    UPDATE dbo.TonKho 
                    SET TonKho = TonKho - @SLReq
                    WHERE MaThuoc = @MaThuoc AND MaLoSX = @MaLo;
                    FETCH NEXT FROM cur INTO @MaThuoc, @SLReq, @DonGia, @MaLo;
                    CONTINUE;
                END
            END

            -- Nếu @MaLo = NULL hoặc lô không đủ, phân bổ tự động
            DECLARE @ConCan INT = @SLReq;
            DECLARE c2 CURSOR LOCAL FAST_FORWARD FOR
                SELECT MaLoSX, TonKho
                FROM dbo.TonKho
                WHERE MaThuoc = @MaThuoc AND TonKho > 0
                ORDER BY CASE WHEN HanSuDung IS NULL THEN 1 ELSE 0 END, HanSuDung;

            DECLARE @Lo CHAR(10), @Co INT;
            OPEN c2;
            FETCH NEXT FROM c2 INTO @Lo, @Co;

            WHILE @@FETCH_STATUS = 0 AND @ConCan > 0
            BEGIN
                DECLARE @Lay INT = IIF(@Co >= @ConCan, @ConCan, @Co);

                IF @Lay > 0
                BEGIN
                    UPDATE dbo.TonKho 
                    SET TonKho = TonKho - @Lay
                    WHERE MaThuoc = @MaThuoc AND MaLoSX = @Lo;

                    SET @ConCan -= @Lay;
                END;

                FETCH NEXT FROM c2 INTO @Lo, @Co;
            END;

            CLOSE c2; DEALLOCATE c2;

            IF @ConCan > 0
            BEGIN
                DECLARE @msg2 NVARCHAR(200) = CONCAT(N'Tồn kho không đủ để trả: ', @MaThuoc);
                THROW 50006, @msg2, 1;
            END;

            FETCH NEXT FROM cur INTO @MaThuoc, @SLReq, @DonGia, @MaLo;
        END;

        CLOSE cur; DEALLOCATE cur;

        -- Cập nhật trạng thái phiếu trả là 'Đã xác nhận'
        UPDATE dbo.PhieuTraHang
        SET TrangThai = N'Đã xác nhận'
        WHERE RTRIM(MaPhieuTra) = RTRIM(@MaPhieuTra);

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN;

        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrNum INT = ERROR_NUMBER();

        THROW @ErrNum, @ErrMsg, 1;
    END CATCH
END;
GO


CREATE OR ALTER PROCEDURE dbo.usp_CapNhat_TonKho
    @MaPhieuNhap CHAR(10),           -- Thêm tham số phiếu nhập
    @ChiTiet dbo.tvp_CTPN READONLY
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM @ChiTiet)
            THROW 50001, N'Chi tiết trống.', 1;

        BEGIN TRAN;

        DECLARE @MaThuoc CHAR(10), @SL INT, @DG DECIMAL(18,2), @HSD DATE;

        DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
            SELECT MaThuoc, SoLuong, DonGiaNhap, HanSuDung FROM @ChiTiet;
        OPEN cur;
        FETCH NEXT FROM cur INTO @MaThuoc, @SL, @DG, @HSD;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Tạo mã lô mới cho tồn kho
            DECLARE @seqLo BIGINT = NEXT VALUE FOR dbo.Seq_LoSX;
            DECLARE @MaLoSX CHAR(10) = CONCAT('LO', RIGHT(CONCAT(REPLICATE('0',6), CAST(@seqLo AS VARCHAR(10))), 6));

            -- Cập nhật bảng Tồn Kho
            INSERT INTO dbo.TonKho (MaLoSX, MaThuoc, TonKho, HanSuDung)
            VALUES (@MaLoSX, @MaThuoc, @SL, @HSD);          

            FETCH NEXT FROM cur INTO @MaThuoc, @SL, @DG, @HSD;
        END

        CLOSE cur; DEALLOCATE cur;

        -- **Cập nhật trạng thái phiếu nhập là 'Đã xác nhận'**
        UPDATE dbo.PhieuNhap
        SET TrangThai = N'Đã xác nhận'
        WHERE RTRIM(MaPhieuNhap) = RTRIM(@MaPhieuNhap);

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT>0 ROLLBACK;
        THROW;
    END CATCH
END
GO
