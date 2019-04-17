namespace ERBus.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitializerDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ERBUS.BAOBI",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MABAOBI = c.String(nullable: false, maxLength: 50),
                        TENBAOBI = c.String(nullable: false, maxLength: 300),
                        SOLUONG = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TRANGTHAI = c.Decimal(precision: 10, scale: 0),
                        I_CREATE_DATE = c.DateTime(),
                        I_CREATE_BY = c.String(maxLength: 25),
                        I_UPDATE_DATE = c.DateTime(),
                        I_UPDATE_BY = c.String(maxLength: 25),
                        I_STATE = c.String(maxLength: 1),
                        UNITCODE = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.BOHANG_CHITIET",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MABOHANG = c.String(nullable: false, maxLength: 50),
                        MAHANG = c.String(nullable: false, maxLength: 50),
                        SOLUONG = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CHIETKHAU = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TONGTIEN = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DIENGIAI = c.String(maxLength: 300),
                        TRANGTHAI = c.Decimal(precision: 10, scale: 0),
                        I_CREATE_DATE = c.DateTime(),
                        I_CREATE_BY = c.String(maxLength: 25),
                        I_UPDATE_DATE = c.DateTime(),
                        I_UPDATE_BY = c.String(maxLength: 25),
                        I_STATE = c.String(maxLength: 1),
                        UNITCODE = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.BOHANG",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MABOHANG = c.String(nullable: false, maxLength: 50),
                        TENBOHANG = c.String(nullable: false, maxLength: 300),
                        DIENGIAI = c.String(maxLength: 300),
                        TRANGTHAI = c.Decimal(precision: 10, scale: 0),
                        I_CREATE_DATE = c.DateTime(),
                        I_CREATE_BY = c.String(maxLength: 25),
                        I_UPDATE_DATE = c.DateTime(),
                        I_UPDATE_BY = c.String(maxLength: 25),
                        I_STATE = c.String(maxLength: 1),
                        UNITCODE = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.CAPMA",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        LOAIMA = c.String(maxLength: 10),
                        NHOMMA = c.String(maxLength: 10),
                        GIATRI = c.String(maxLength: 8),
                        UNITCODE = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.CHUNGTU_CHITIET",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MA_CHUNGTU = c.String(nullable: false, maxLength: 30),
                        MAHANG = c.String(nullable: false, maxLength: 50),
                        MATHUE_VAO = c.String(nullable: false, maxLength: 50),
                        MATHUE_RA = c.String(nullable: false, maxLength: 50),
                        SOLUONG = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GIAMUA = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GIAMUA_VAT = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TIEN_GIAMGIA = c.Decimal(precision: 18, scale: 2),
                        THANHTIEN = c.Decimal(nullable: false, precision: 18, scale: 2),
                        INDEX = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.CHUNGTU",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MA_CHUNGTU = c.String(nullable: false, maxLength: 30),
                        LOAI_CHUNGTU = c.String(nullable: false, maxLength: 10),
                        NGAY_CHUNGTU = c.DateTime(nullable: false),
                        MANHACUNGCAP = c.String(nullable: false, maxLength: 50),
                        MAKHACHHANG = c.String(nullable: false, maxLength: 50),
                        NGAY_DUYETPHIEU = c.DateTime(),
                        THOIGIAN_TAO = c.String(nullable: false, maxLength: 12),
                        THOIGIAN_DUYET = c.String(maxLength: 12),
                        NGAY_DIEUDONG = c.DateTime(),
                        MAKHO_NHAP = c.String(maxLength: 50),
                        MAKHO_XUAT = c.String(maxLength: 50),
                        MADONVI_NHAP = c.String(maxLength: 10),
                        MADONVI_XUAT = c.String(maxLength: 10),
                        TIEN_CHIETKHAU = c.Decimal(precision: 18, scale: 2),
                        MATHUE_TOANDON = c.String(maxLength: 50),
                        LENH_DIEUDONG = c.String(maxLength: 50),
                        MA_LYDO = c.String(maxLength: 10),
                        DIENGIAI = c.String(maxLength: 500),
                        TRANGTHAI = c.Decimal(precision: 10, scale: 0),
                        I_CREATE_DATE = c.DateTime(),
                        I_CREATE_BY = c.String(maxLength: 25),
                        I_UPDATE_DATE = c.DateTime(),
                        I_UPDATE_BY = c.String(maxLength: 25),
                        I_STATE = c.String(maxLength: 1),
                        UNITCODE = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.CUAHANG",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MA_CUAHANG = c.String(nullable: false, maxLength: 50),
                        TEN_CUAHANG = c.String(nullable: false, maxLength: 200),
                        SODIENTHOAI = c.String(maxLength: 16),
                        DIACHI = c.String(maxLength: 200),
                        MA_CUAHANG_CHA = c.String(maxLength: 50),
                        I_CREATE_DATE = c.DateTime(),
                        I_CREATE_BY = c.String(maxLength: 25),
                        I_UPDATE_DATE = c.DateTime(),
                        I_UPDATE_BY = c.String(maxLength: 25),
                        I_STATE = c.String(maxLength: 1),
                        UNITCODE = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.DONVITINH",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MADONVITINH = c.String(nullable: false, maxLength: 50),
                        TENDONVITINH = c.String(nullable: false, maxLength: 300),
                        TRANGTHAI = c.Decimal(precision: 10, scale: 0),
                        I_CREATE_DATE = c.DateTime(),
                        I_CREATE_BY = c.String(maxLength: 25),
                        I_UPDATE_DATE = c.DateTime(),
                        I_UPDATE_BY = c.String(maxLength: 25),
                        I_STATE = c.String(maxLength: 1),
                        UNITCODE = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.GIAODICH_CHITIET",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MA_GIAODICH = c.String(nullable: false, maxLength: 70),
                        MAHANG = c.String(nullable: false, maxLength: 50),
                        MATHUE_RA = c.String(nullable: false, maxLength: 50),
                        SOLUONG = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GIABANLE_VAT = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MA_KHUYENMAI = c.String(maxLength: 30),
                        TYLE_KHUYENMAI = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TIEN_KHUYENMAI = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TYLE_CHIETKHAU = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TIEN_CHIETKHAU = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TIENTHE_VIP = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TIEN_VOUCHER = c.Decimal(nullable: false, precision: 18, scale: 2),
                        THANHTIEN = c.Decimal(nullable: false, precision: 18, scale: 2),
                        INDEX = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.GIAODICH",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MA_GIAODICH = c.String(nullable: false, maxLength: 70),
                        LOAI_GIAODICH = c.String(nullable: false, maxLength: 10),
                        NGAY_GIAODICH = c.DateTime(nullable: false),
                        MAKHACHHANG = c.String(nullable: false, maxLength: 50),
                        THOIGIAN_TAO = c.String(nullable: false, maxLength: 12),
                        TIENKHACH_TRA = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TIEN_TRALAI_KHACH = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MAKHO_XUAT = c.String(nullable: false, maxLength: 50),
                        MA_VOUCHER = c.String(maxLength: 50),
                        DIENGIAI = c.String(maxLength: 300),
                        I_CREATE_DATE = c.DateTime(),
                        I_CREATE_BY = c.String(maxLength: 25),
                        I_UPDATE_DATE = c.DateTime(),
                        I_UPDATE_BY = c.String(maxLength: 25),
                        I_STATE = c.String(maxLength: 1),
                        UNITCODE = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.HANGKHACHHANG",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MAHANG = c.String(nullable: false, maxLength: 50),
                        TENHANG = c.String(nullable: false, maxLength: 100),
                        TYLE_SINHNHAT = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TYLE_DACBIET = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QUYDOITIEN_THANH_DIEM = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QUYDOIDIEM_THANH_TIEN = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HANG_KHOIDAU = c.Decimal(precision: 10, scale: 0),
                        SOTIEN_LENHANG = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TRANGTHAI = c.Decimal(precision: 10, scale: 0),
                        I_CREATE_DATE = c.DateTime(),
                        I_CREATE_BY = c.String(maxLength: 25),
                        I_UPDATE_DATE = c.DateTime(),
                        I_UPDATE_BY = c.String(maxLength: 25),
                        I_STATE = c.String(maxLength: 1),
                        UNITCODE = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.KEHANG",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MAKEHANG = c.String(nullable: false, maxLength: 50),
                        TENKEHANG = c.String(nullable: false, maxLength: 300),
                        TRANGTHAI = c.Decimal(precision: 10, scale: 0),
                        I_CREATE_DATE = c.DateTime(),
                        I_CREATE_BY = c.String(maxLength: 25),
                        I_UPDATE_DATE = c.DateTime(),
                        I_UPDATE_BY = c.String(maxLength: 25),
                        I_STATE = c.String(maxLength: 1),
                        UNITCODE = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.KHACHHANG",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MAKHACHHANG = c.String(nullable: false, maxLength: 50),
                        TENKHACHHANG = c.String(nullable: false, maxLength: 300),
                        DIACHI = c.String(maxLength: 300),
                        DIENTHOAI = c.String(maxLength: 20),
                        CANCUOC_CONGDAN = c.String(maxLength: 20),
                        NGAYSINH = c.DateTime(),
                        NGAYDACBIET = c.DateTime(),
                        MATHE = c.String(maxLength: 20),
                        SODIEM = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TONGTIEN = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DIENGIAI = c.String(maxLength: 300),
                        TRANGTHAI = c.Decimal(precision: 10, scale: 0),
                        I_CREATE_DATE = c.DateTime(),
                        I_CREATE_BY = c.String(maxLength: 25),
                        I_UPDATE_DATE = c.DateTime(),
                        I_UPDATE_BY = c.String(maxLength: 25),
                        I_STATE = c.String(maxLength: 1),
                        UNITCODE = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.KHOHANG",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MAKHO = c.String(nullable: false, maxLength: 50),
                        TENKHO = c.String(nullable: false, maxLength: 150),
                        DIENGIAI = c.String(maxLength: 300),
                        TRANGTHAI = c.Decimal(precision: 10, scale: 0),
                        I_CREATE_DATE = c.DateTime(),
                        I_CREATE_BY = c.String(maxLength: 25),
                        I_UPDATE_DATE = c.DateTime(),
                        I_UPDATE_BY = c.String(maxLength: 25),
                        I_STATE = c.String(maxLength: 1),
                        UNITCODE = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.KHUYENMAI_CHITIET",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MA_KHUYENMAI = c.String(nullable: false, maxLength: 30),
                        MAHANG = c.String(nullable: false, maxLength: 50),
                        SOLUONG = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GIATRI_KHUYENMAI = c.Decimal(precision: 18, scale: 2),
                        INDEX = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.KHUYENMAI",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MA_KHUYENMAI = c.String(nullable: false, maxLength: 30),
                        LOAI_KHUYENMAI = c.String(nullable: false, maxLength: 20),
                        TUNGAY = c.DateTime(nullable: false),
                        DENNGAY = c.DateTime(nullable: false),
                        TUGIO = c.String(maxLength: 10),
                        DENGIO = c.String(maxLength: 10),
                        THOIGIAN_TAO = c.String(nullable: false, maxLength: 12),
                        THOIGIAN_DUYET = c.String(maxLength: 12),
                        MAKHO_KHUYENMAI = c.String(maxLength: 50),
                        DIENGIAI = c.String(maxLength: 500),
                        TRANGTHAI = c.Decimal(precision: 10, scale: 0),
                        I_CREATE_DATE = c.DateTime(),
                        I_CREATE_BY = c.String(maxLength: 25),
                        I_UPDATE_DATE = c.DateTime(),
                        I_UPDATE_BY = c.String(maxLength: 25),
                        I_STATE = c.String(maxLength: 1),
                        UNITCODE = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.KYKETOAN",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        KYKETOAN = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TENKY = c.String(maxLength: 150),
                        TUNGAY = c.DateTime(nullable: false),
                        DENNGAY = c.DateTime(nullable: false),
                        NAM = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TRANGTHAI = c.Decimal(precision: 10, scale: 0),
                        I_CREATE_DATE = c.DateTime(),
                        I_CREATE_BY = c.String(maxLength: 25),
                        I_UPDATE_DATE = c.DateTime(),
                        I_UPDATE_BY = c.String(maxLength: 25),
                        I_STATE = c.String(maxLength: 1),
                        UNITCODE = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.LOAIHANG",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MALOAI = c.String(nullable: false, maxLength: 50),
                        TENLOAI = c.String(nullable: false, maxLength: 300),
                        TRANGTHAI = c.Decimal(precision: 10, scale: 0),
                        I_CREATE_DATE = c.DateTime(),
                        I_CREATE_BY = c.String(maxLength: 25),
                        I_UPDATE_DATE = c.DateTime(),
                        I_UPDATE_BY = c.String(maxLength: 25),
                        I_STATE = c.String(maxLength: 1),
                        UNITCODE = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.MATHANG_GIA",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MAHANG = c.String(nullable: false, maxLength: 50),
                        GIAMUA = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GIAMUA_VAT = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GIABANLE = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GIABANLE_VAT = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GIABANBUON = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GIABANBUON_VAT = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TYLE_LAILE = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TYLE_LAIBUON = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TRANGTHAI = c.Decimal(precision: 10, scale: 0),
                        I_CREATE_DATE = c.DateTime(),
                        I_CREATE_BY = c.String(maxLength: 25),
                        I_UPDATE_DATE = c.DateTime(),
                        I_UPDATE_BY = c.String(maxLength: 25),
                        I_STATE = c.String(maxLength: 1),
                        UNITCODE = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.MATHANG",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MAHANG = c.String(nullable: false, maxLength: 50),
                        TENHANG = c.String(nullable: false, maxLength: 300),
                        MANHACUNGCAP = c.String(nullable: false, maxLength: 50),
                        MALOAI = c.String(nullable: false, maxLength: 50),
                        MANHOM = c.String(nullable: false, maxLength: 50),
                        MATHUE_VAO = c.String(nullable: false, maxLength: 50),
                        MATHUE_RA = c.String(nullable: false, maxLength: 50),
                        MADONVITINH = c.String(maxLength: 50),
                        MAKEHANG = c.String(maxLength: 50),
                        MABAOBI = c.String(maxLength: 50),
                        BARCODE = c.String(maxLength: 2000),
                        ITEMCODE = c.String(maxLength: 10),
                        DUONGDAN = c.String(maxLength: 300),
                        AVATAR = c.Binary(),
                        IMAGE = c.String(maxLength: 2000),
                        TRANGTHAI = c.Decimal(precision: 10, scale: 0),
                        I_CREATE_DATE = c.DateTime(),
                        I_CREATE_BY = c.String(maxLength: 25),
                        I_UPDATE_DATE = c.DateTime(),
                        I_UPDATE_BY = c.String(maxLength: 25),
                        I_STATE = c.String(maxLength: 1),
                        UNITCODE = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.MENU",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MENU_CHA = c.String(maxLength: 50),
                        MA_MENU = c.String(maxLength: 50),
                        TIEUDE = c.String(maxLength: 200),
                        SAPXEP = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TRANGTHAI = c.Decimal(precision: 10, scale: 0),
                        ICON = c.String(maxLength: 50),
                        I_CREATE_DATE = c.DateTime(),
                        I_CREATE_BY = c.String(maxLength: 25),
                        I_UPDATE_DATE = c.DateTime(),
                        I_UPDATE_BY = c.String(maxLength: 25),
                        I_STATE = c.String(maxLength: 1),
                        UNITCODE = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.NGUOIDUNG_MENU",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MA_MENU = c.String(nullable: false, maxLength: 50),
                        USERNAME = c.String(nullable: false, maxLength: 50),
                        XEM = c.Decimal(nullable: false, precision: 1, scale: 0),
                        THEM = c.Decimal(nullable: false, precision: 1, scale: 0),
                        SUA = c.Decimal(nullable: false, precision: 1, scale: 0),
                        XOA = c.Decimal(nullable: false, precision: 1, scale: 0),
                        DUYET = c.Decimal(nullable: false, precision: 1, scale: 0),
                        GIAMUA = c.Decimal(nullable: false, precision: 1, scale: 0),
                        GIABAN = c.Decimal(nullable: false, precision: 1, scale: 0),
                        GIAVON = c.Decimal(nullable: false, precision: 1, scale: 0),
                        TYLELAI = c.Decimal(nullable: false, precision: 1, scale: 0),
                        BANCHIETKHAU = c.Decimal(nullable: false, precision: 1, scale: 0),
                        BANBUON = c.Decimal(nullable: false, precision: 1, scale: 0),
                        BANTRALAI = c.Decimal(nullable: false, precision: 1, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.NGUOIDUNG_NHOMQUYEN",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        USERNAME = c.String(nullable: false, maxLength: 50),
                        MANHOMQUYEN = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.NGUOIDUNG",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        USERNAME = c.String(nullable: false, maxLength: 50),
                        PASSWORD = c.String(nullable: false, maxLength: 50),
                        MANHANVIEN = c.String(maxLength: 10),
                        TENNHANVIEN = c.String(maxLength: 100),
                        SODIENTHOAI = c.String(maxLength: 16),
                        CHUNGMINHTHU = c.String(maxLength: 20),
                        GIOITINH = c.Decimal(precision: 10, scale: 0),
                        VAITRO = c.Decimal(precision: 10, scale: 0),
                        TRANGTHAI = c.Decimal(precision: 10, scale: 0),
                        PARENT_UNITCODE = c.String(maxLength: 10),
                        I_CREATE_DATE = c.DateTime(),
                        I_CREATE_BY = c.String(maxLength: 25),
                        I_UPDATE_DATE = c.DateTime(),
                        I_UPDATE_BY = c.String(maxLength: 25),
                        I_STATE = c.String(maxLength: 1),
                        UNITCODE = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.NHACUNGCAP",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MANHACUNGCAP = c.String(nullable: false, maxLength: 50),
                        TENNHACUNGCAP = c.String(nullable: false, maxLength: 300),
                        DIACHI = c.String(maxLength: 300),
                        MASOTHUE = c.String(maxLength: 80),
                        DIENTHOAI = c.String(maxLength: 20),
                        DIENGIAI = c.String(maxLength: 300),
                        TRANGTHAI = c.Decimal(precision: 10, scale: 0),
                        I_CREATE_DATE = c.DateTime(),
                        I_CREATE_BY = c.String(maxLength: 25),
                        I_UPDATE_DATE = c.DateTime(),
                        I_UPDATE_BY = c.String(maxLength: 25),
                        I_STATE = c.String(maxLength: 1),
                        UNITCODE = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.NHOMHANG",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MANHOM = c.String(nullable: false, maxLength: 50),
                        TENNHOM = c.String(nullable: false, maxLength: 300),
                        MALOAI = c.String(nullable: false, maxLength: 50),
                        TRANGTHAI = c.Decimal(precision: 10, scale: 0),
                        I_CREATE_DATE = c.DateTime(),
                        I_CREATE_BY = c.String(maxLength: 25),
                        I_UPDATE_DATE = c.DateTime(),
                        I_UPDATE_BY = c.String(maxLength: 25),
                        I_STATE = c.String(maxLength: 1),
                        UNITCODE = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.NHOMQUYEN_MENU",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MA_MENU = c.String(nullable: false, maxLength: 50),
                        MANHOMQUYEN = c.String(nullable: false, maxLength: 10),
                        XEM = c.Decimal(nullable: false, precision: 1, scale: 0),
                        THEM = c.Decimal(nullable: false, precision: 1, scale: 0),
                        SUA = c.Decimal(nullable: false, precision: 1, scale: 0),
                        XOA = c.Decimal(nullable: false, precision: 1, scale: 0),
                        DUYET = c.Decimal(nullable: false, precision: 1, scale: 0),
                        GIAMUA = c.Decimal(nullable: false, precision: 1, scale: 0),
                        GIABAN = c.Decimal(nullable: false, precision: 1, scale: 0),
                        GIAVON = c.Decimal(nullable: false, precision: 1, scale: 0),
                        TYLELAI = c.Decimal(nullable: false, precision: 1, scale: 0),
                        BANCHIETKHAU = c.Decimal(nullable: false, precision: 1, scale: 0),
                        BANBUON = c.Decimal(nullable: false, precision: 1, scale: 0),
                        BANTRALAI = c.Decimal(nullable: false, precision: 1, scale: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.NHOMQUYEN",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MANHOMQUYEN = c.String(nullable: false, maxLength: 10),
                        TENNHOMQUYEN = c.String(nullable: false, maxLength: 200),
                        DIENGIAI = c.String(maxLength: 300),
                        I_CREATE_DATE = c.DateTime(),
                        I_CREATE_BY = c.String(maxLength: 25),
                        I_UPDATE_DATE = c.DateTime(),
                        I_UPDATE_BY = c.String(maxLength: 25),
                        I_STATE = c.String(maxLength: 1),
                        UNITCODE = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.THAMSOHETHONG",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MA_THAMSO = c.String(nullable: false, maxLength: 50),
                        TEN_THAMSO = c.String(nullable: false, maxLength: 200),
                        GIATRI_SO = c.Decimal(nullable: false, precision: 10, scale: 0),
                        GIATRI_CHU = c.String(maxLength: 100),
                        ISDIABLED_GIATRI_CHU = c.Decimal(nullable: false, precision: 1, scale: 0),
                        PLACEHOLDER = c.String(maxLength: 150),
                        TRANGTHAI = c.Decimal(precision: 10, scale: 0),
                        I_CREATE_DATE = c.DateTime(),
                        I_CREATE_BY = c.String(maxLength: 25),
                        I_UPDATE_DATE = c.DateTime(),
                        I_UPDATE_BY = c.String(maxLength: 25),
                        I_STATE = c.String(maxLength: 1),
                        UNITCODE = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "ERBUS.THUE",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        MATHUE = c.String(nullable: false, maxLength: 50),
                        TENTHUE = c.String(nullable: false, maxLength: 300),
                        GIATRI = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TRANGTHAI = c.Decimal(precision: 10, scale: 0),
                        I_CREATE_DATE = c.DateTime(),
                        I_CREATE_BY = c.String(maxLength: 25),
                        I_UPDATE_DATE = c.DateTime(),
                        I_UPDATE_BY = c.String(maxLength: 25),
                        I_STATE = c.String(maxLength: 1),
                        UNITCODE = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("ERBUS.THUE");
            DropTable("ERBUS.THAMSOHETHONG");
            DropTable("ERBUS.NHOMQUYEN");
            DropTable("ERBUS.NHOMQUYEN_MENU");
            DropTable("ERBUS.NHOMHANG");
            DropTable("ERBUS.NHACUNGCAP");
            DropTable("ERBUS.NGUOIDUNG");
            DropTable("ERBUS.NGUOIDUNG_NHOMQUYEN");
            DropTable("ERBUS.NGUOIDUNG_MENU");
            DropTable("ERBUS.MENU");
            DropTable("ERBUS.MATHANG");
            DropTable("ERBUS.MATHANG_GIA");
            DropTable("ERBUS.LOAIHANG");
            DropTable("ERBUS.KYKETOAN");
            DropTable("ERBUS.KHUYENMAI");
            DropTable("ERBUS.KHUYENMAI_CHITIET");
            DropTable("ERBUS.KHOHANG");
            DropTable("ERBUS.KHACHHANG");
            DropTable("ERBUS.KEHANG");
            DropTable("ERBUS.HANGKHACHHANG");
            DropTable("ERBUS.GIAODICH");
            DropTable("ERBUS.GIAODICH_CHITIET");
            DropTable("ERBUS.DONVITINH");
            DropTable("ERBUS.CUAHANG");
            DropTable("ERBUS.CHUNGTU");
            DropTable("ERBUS.CHUNGTU_CHITIET");
            DropTable("ERBUS.CAPMA");
            DropTable("ERBUS.BOHANG");
            DropTable("ERBUS.BOHANG_CHITIET");
            DropTable("ERBUS.BAOBI");
        }
    }
}
