CREATE DATABASE QLBH_NhaThuoc
USE QLBH_NhaThuoc

CREATE TABLE KhachHang (
    MaKH CHAR(10) PRIMARY KEY,
    TenKH NVARCHAR(100),
    SDT VARCHAR(15),
    Email VARCHAR(100),
    DiaChi NVARCHAR(200),
    DoanhThu DECIMAL(18,2) );

CREATE TABLE NhanVien (
    MaNV CHAR(10) PRIMARY KEY,
    TenNV NVARCHAR(100),
    GioiTinh NVARCHAR(10),
    NgaySinh DATE,
    SDT VARCHAR(15),
    DiaChi NVARCHAR(200) );

CREATE TABLE DanhMucThuoc (
    MaDanhMuc CHAR(10) PRIMARY KEY,
    TenDanhMuc NVARCHAR(100) );

CREATE TABLE DonViTinh (
    MaDonViTinh CHAR(10) PRIMARY KEY,
    TenDonViTinh NVARCHAR(50) );

CREATE TABLE NhaCungCap (
    MaNCC CHAR(10) PRIMARY KEY,
    TenNCC NVARCHAR(150),
    DiaChi NVARCHAR(200),
    SDT VARCHAR(15),
    Email VARCHAR(100) );

CREATE TABLE PhuongThucTT (
    MaPTTT CHAR(10) PRIMARY KEY,
    TenPTTT NVARCHAR(50) );



CREATE TABLE Thuoc (
    MaThuoc CHAR(10) PRIMARY KEY,
    MaDanhMuc CHAR(10),
    MaDonViTinh CHAR(10),
    MaNCC CHAR(10),
    TenThuoc NVARCHAR(150),
    DongGoi NVARCHAR(150),
    HoatChat NVARCHAR(150),
    GiaBan DECIMAL(18,2),
    FOREIGN KEY (MaDanhMuc) REFERENCES DanhMucThuoc(MaDanhMuc),
    FOREIGN KEY (MaDonViTinh) REFERENCES DonViTinh(MaDonViTinh),
    FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC) );

CREATE TABLE TonKho (
    MaLoSX CHAR(10) PRIMARY KEY,
    MaThuoc CHAR(10),
    TonKho INT,
    HanSuDung DATE,
    FOREIGN KEY (MaThuoc) REFERENCES Thuoc(MaThuoc) );

CREATE TABLE HoaDon (
    MaHoaDon CHAR(10) PRIMARY KEY,
    MaKH CHAR(10),
    MaNV CHAR(10),
    NgayLap DATETIME,
    TongTien DECIMAL(18,2),
    MaPTTT CHAR(10),
    GhiChu NVARCHAR(255),
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV),
    FOREIGN KEY (MaPTTT) REFERENCES PhuongThucTT(MaPTTT) );

CREATE TABLE ChiTietHoaDon (
    MaCTHD CHAR(10) PRIMARY KEY,
    MaHoaDon CHAR(10),
    MaThuoc CHAR(10),
    SoLuong INT,
    DonGia DECIMAL(18,2),
    ThanhTien DECIMAL(18,2),
    TrangThai NVARCHAR(10),
    FOREIGN KEY (MaHoaDon) REFERENCES HoaDon(MaHoaDon),
    FOREIGN KEY (MaThuoc) REFERENCES Thuoc(MaThuoc) );

CREATE TABLE PhieuNhap (
    MaPhieuNhap CHAR(10) PRIMARY KEY,
    MaNCC CHAR(10),
    MaNV CHAR(10),
    NgayNhap DATETIME,
    TongTien DECIMAL(18,2),
    MaPTTT CHAR(10),
    TrangThai NVARCHAR(10),
    FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC),
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV),
    FOREIGN KEY (MaPTTT) REFERENCES PhuongThucTT(MaPTTT) );

CREATE TABLE ChiTietPhieuNhap (
    MaCTPN CHAR(10) PRIMARY KEY,
    MaPhieuNhap CHAR(10),
    MaThuoc CHAR(10),
    SoLuong INT,
    DonGiaNhap DECIMAL(18,2),
    NgaySanXuat DATE,
    HanSuDung DATE,
    ThanhTien DECIMAL(18,2),
    FOREIGN KEY (MaPhieuNhap) REFERENCES PhieuNhap(MaPhieuNhap),
    FOREIGN KEY (MaThuoc) REFERENCES Thuoc(MaThuoc) );

CREATE TABLE PhieuTraHang (
    MaPhieuTra CHAR(10) PRIMARY KEY,
    MaNCC CHAR(10),
    MaNV CHAR(10),
    NgayTra DATETIME,
    LyDoTra NVARCHAR(200),
    TrangThai NVARCHAR(10),
    TongTien DECIMAL(18,2),
    FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC),
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV) );

CREATE TABLE ChiTietPhieuTraHang (
    MaCTTra CHAR(10) PRIMARY KEY,
    MaPhieuTra CHAR(10),
    MaThuoc CHAR(10),
    SoLuong INT,
    DonGia DECIMAL(18,2),
    ThanhTien DECIMAL(18,2),
    FOREIGN KEY (MaPhieuTra) REFERENCES PhieuTraHang(MaPhieuTra),
    FOREIGN KEY (MaThuoc) REFERENCES Thuoc(MaThuoc) );
