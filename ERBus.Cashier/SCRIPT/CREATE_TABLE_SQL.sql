﻿USE ERBUS_CASHIER
GO
CREATE TABLE dbo."NGUOIDUNG" (
  "ID" varchar(50) NOT NULL,
  "USERNAME" varchar(50) NOT NULL,
  "PASSWORD" varchar(50) NOT NULL,
  "MANHANVIEN" varchar(10),
  "TENNHANVIEN" nvarchar(100),
  "SODIENTHOAI" varchar(16),
  "CHUNGMINHTHU" varchar(20),
  "GIOITINH" int,
  "UNITCODE" varchar(10),
  CONSTRAINT "PK_NGUOIDUNG" PRIMARY KEY ("ID")
);
USE ERBUS_CASHIER
GO
CREATE TABLE dbo."KHACHHANG" (
  "ID" varchar(50) NOT NULL,
  "MAKHACHHANG" varchar(50) NOT NULL,
  "TENKHACHHANG" nvarchar(300) NOT NULL,
  "DIACHI" nvarchar(300),
  "DIENTHOAI" varchar(20),
  "CANCUOC_CONGDAN" varchar(20),
  "NGAYSINH" date,
  "NGAYDACBIET" date,
  "MATHE" varchar(20),
  "SODIEM" decimal NOT NULL,
  "TONGTIEN" decimal NOT NULL,
  "DIENGIAI" nvarchar(300),
  "MAHANG" varchar(50) NOT NULL,
  "UNITCODE" varchar(10),
  CONSTRAINT "PK_KHACHHANG" PRIMARY KEY ("ID")
);

USE ERBUS_CASHIER
GO
CREATE TABLE dbo."KHUYENMAI" (
  "ID" varchar(50) NOT NULL,
  "MA_KHUYENMAI" varchar(30) NOT NULL,
  "LOAI_KHUYENMAI" varchar(20) NOT NULL,
  "TUNGAY" date NOT NULL,
  "DENNGAY" date NOT NULL,
  "MAKHO_KHUYENMAI" varchar(50),
  "DIENGIAI" nvarchar(500),
  "TUGIO" varchar(10),
  "DENGIO" varchar(10),
  "UNITCODE" varchar(10),
  CONSTRAINT "PK_KHUYENMAI" PRIMARY KEY ("ID")
);

USE ERBUS_CASHIER
GO
CREATE TABLE dbo."KHUYENMAI_CHITIET" (
  "ID" varchar(50) NOT NULL,
  "MA_KHUYENMAI" varchar(30) NOT NULL,
  "MAHANG" varchar(50) NOT NULL,
  "SOLUONG" decimal NOT NULL,
  "GIATRI_KHUYENMAI" decimal,
  CONSTRAINT "PK_KHUYENMAI_CHITIET" PRIMARY KEY ("ID")
);


CREATE TABLE dbo."THAMSOHETHONG" (
  "ID" varchar(50) NOT NULL,
  "MA_THAMSO" varchar(50) NOT NULL,
  "TEN_THAMSO" nvarchar(200) NOT NULL,
  "GIATRI_SO" int NOT NULL,
  "GIATRI_CHU" varchar(100) NOT NULL,
  CONSTRAINT "PK_THAMSOHETHONG" PRIMARY KEY ("ID")
);

USE ERBUS_CASHIER
GO
CREATE TABLE dbo."MATHANG" (
  "ID" varchar(50) NOT NULL,
  "MAHANG" varchar(50) NOT NULL,
  "TENHANG" nvarchar(300) NOT NULL,
  "MANHACUNGCAP" varchar(50) NOT NULL,
  "MALOAI" varchar(50) NOT NULL,
  "MANHOM" varchar(50) NOT NULL,
  "MATHUE_VAO" varchar(50) NOT NULL,
  "MATHUE_RA" varchar(50) NOT NULL,
  "MADONVITINH" varchar(50),
  "MAKEHANG" varchar(50),
  "MABAOBI" varchar(50),
  "BARCODE" varchar(2000),
  "ITEMCODE" varchar(10),
  "UNITCODE" varchar(10),
  CONSTRAINT "PK_MATHANG" PRIMARY KEY ("ID")
);

USE ERBUS_CASHIER
GO
CREATE TABLE dbo."MATHANG_GIA" (
  "ID" varchar(50) NOT NULL,
  "MAHANG" varchar(50) NOT NULL,
  "GIAMUA" decimal NOT NULL,
  "GIAMUA_VAT" decimal NOT NULL,
  "GIABANLE" decimal NOT NULL,
  "GIABANLE_VAT" decimal NOT NULL,
  "GIABANBUON" decimal NOT NULL,
  "GIABANBUON_VAT" decimal NOT NULL,
  "TYLE_LAILE" decimal NOT NULL,
  "TYLE_LAIBUON" decimal NOT NULL,
  "GIAVON" decimal NOT NULL,
  "UNITCODE" varchar(10),
  CONSTRAINT "PK_MATHANG_GIA" PRIMARY KEY ("ID")
);

USE ERBUS_CASHIER
GO
CREATE TABLE dbo."CUAHANG" (
  "ID" varchar(50) NOT NULL,
  "MA_CUAHANG" varchar(50) NOT NULL,
  "TEN_CUAHANG" nvarchar(200) NOT NULL,
  "SODIENTHOAI" varchar(16),
  "DIACHI" nvarchar(200),
  "SODIENTHOAI" varchar(16),
  "UNITCODE" varchar(10),
  CONSTRAINT "PK_CUAHANG" PRIMARY KEY ("ID")
);

USE ERBUS_CASHIER
GO
CREATE TABLE dbo."DONVITINH" (
  "ID" varchar(50) NOT NULL,
  "MADONVITINH" varchar(50) NOT NULL,
  "TENDONVITINH" nvarchar(300) NOT NULL,
  CONSTRAINT "PK_DONVITINH" PRIMARY KEY ("ID")
);

USE ERBUS_CASHIER
GO
CREATE TABLE dbo."THUE" (
  "ID" varchar(50) NOT NULL,
  "MATHUE" varchar(50) NOT NULL,
  "TENTHUE" nvarchar(200) NOT NULL,
  "GIATRI" decimal NOT NULL,
  CONSTRAINT "PK_THUE" PRIMARY KEY ("ID")
);

USE ERBUS_CASHIER
GO
CREATE TABLE dbo."HANGKHACHHANG" (
    "ID" varchar(50) NOT NULL, 
	"MAHANG" varchar(50) NOT NULL, 
	"TENHANG" nvarchar(100) NOT NULL,
	"SOTIEN_LENHANG" decimal NOT NULL, 
	"TYLE_SINHNHAT" decimal NOT NULL, 
	"TYLE_DACBIET" decimal NOT NULL, 
	"QUYDOITIEN_THANH_DIEM" decimal NOT NULL, 
	"QUYDOIDIEM_THANH_TIEN" decimal NOT NULL, 
	"HANG_KHOIDAU" int NOT NULL, 
	 CONSTRAINT "PK_HANGKHACHHANG" PRIMARY KEY ("ID")
);

USE ERBUS_CASHIER
GO
CREATE TABLE dbo."GIAODICH" 
   ("ID" varchar(50) NOT NULL , 
	"MA_GIAODICH" varchar(70) NOT NULL , 
	"LOAI_GIAODICH" varchar(10) NOT NULL , 
	"NGAY_GIAODICH" date NOT NULL , 
	"MAKHACHHANG" varchar(50) NOT NULL , 
	"THOIGIAN_TAO" varchar(12) NOT NULL , 
	"TIENKHACH_TRA" decimal NOT NULL , 
	"TIEN_TRALAI_KHACH" decimal NOT NULL , 
	"MAKHO_XUAT" varchar(50) NOT NULL , 
	"MA_VOUCHER" varchar(50), 
	"DIENGIAI" nvarchar(300), 
	"I_CREATE_DATE" date, 
	"I_CREATE_BY" varchar(25), 
	"I_UPDATE_DATE" date, 
	"I_UPDATE_BY" varchar(25), 
	"I_STATE" varchar(1), 
	"UNITCODE" varchar(10), 
	 CONSTRAINT "PK_GIAODICH" PRIMARY KEY ("ID")
);

USE ERBUS_CASHIER
GO
CREATE TABLE dbo."GIAODICH_CHITIET" 
   ("ID" varchar(50) NOT NULL , 
	"MA_GIAODICH" varchar(70) NOT NULL , 
	"MAHANG" varchar(50) NOT NULL , 
	"MATHUE_RA" varchar(50) NOT NULL , 
	"SOLUONG" decimal NOT NULL , 
	"GIABANLE_VAT" decimal NOT NULL , 
	"MA_KHUYENMAI" varchar(30), 
	"TYLE_KHUYENMAI" decimal NOT NULL , 
	"TIEN_KHUYENMAI" decimal NOT NULL , 
	"TYLE_CHIETKHAU" decimal NOT NULL , 
	"TIEN_CHIETKHAU" decimal NOT NULL , 
	"TIENTHE_VIP" decimal NOT NULL , 
	"TIEN_VOUCHER" decimal NOT NULL , 
	"THANHTIEN" decimal NOT NULL , 
	"SAPXEP" int, 
	 CONSTRAINT "PK_GIAODICH_CHITIET" PRIMARY KEY ("ID")
);

CREATE TABLE dbo."LICHSU_TANGHANG" 
(
	"ID" varchar(50) NOT NULL, 
	"MAKHACHHANG" varchar(50) NOT NULL, 
	"MAHANG_CU" varchar(50) NOT NULL, 
	"MAHANG_MOI" varchar(50) NOT NULL, 
	"NGAY_LENHANG" DATE NOT NULL, 
	"THOIGIAN_LENHANG" varchar(50), 
	"MA_GIAODICH_LENHANG" varchar(70), 
	CONSTRAINT "PK_LICHSU_TANGHANG" PRIMARY KEY ("ID")
);

CREATE TABLE dbo."KYKETOAN" 
   ("ID" varchar(50), 
	"KYKETOAN" int, 
	"TENKY" nvarchar(150), 
	"TUNGAY" DATE, 
	"DENNGAY" DATE, 
	"NAM" int, 
	"TRANGTHAI" int, 
	"UNITCODE" varchar(10), 
	 CONSTRAINT "PK_KYKETOAN" PRIMARY KEY ("ID")
);


CREATE TABLE dbo."NHACUNGCAP" 
	("ID" VARCHAR(50) NOT NULL, 
	"MANHACUNGCAP" VARCHAR(50) NOT NULL, 
	"TENNHACUNGCAP" NVARCHAR(300) NOT NULL, 
	"DIACHI" NVARCHAR(300), 
	"MASOTHUE" VARCHAR(80), 
	"DIENTHOAI" VARCHAR(20), 
	"DIENGIAI" NVARCHAR(300), 
	"TRANGTHAI" INT, 
	"UNITCODE" VARCHAR(10), 
	CONSTRAINT "PK_NHACUNGCAP" PRIMARY KEY ("ID")
) ;
 
CREATE TABLE dbo."LOAIHANG" (
  "ID" varchar(50) NOT NULL,
  "MALOAI" varchar(50) NOT NULL,
  "TENLOAI" NVARCHAR(300) NOT NULL,
  "UNITCODE" varchar(10),
  CONSTRAINT "PK_LOAIHANG" PRIMARY KEY ("ID")
);


CREATE TABLE dbo."NHOMHANG" (
  "ID" varchar(50) NOT NULL,
  "MALOAI" varchar(50) NOT NULL,
  "MANHOM" varchar(50) NOT NULL,
  "TENNHOM" NVARCHAR(300) NOT NULL,
  "UNITCODE" varchar(10),
  CONSTRAINT "PK_NHOMHANG" PRIMARY KEY ("ID")
);