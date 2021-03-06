﻿create or replace PROCEDURE "BAOCAO_BANBUON_CHITIET" (
    DIEUKIEN_NHOM   IN              VARCHAR2,
    MANHANVIEN      IN              VARCHAR2,
    MAKHO           IN              VARCHAR2,
    MALOAI          IN              VARCHAR2,
    MANHOM          IN              VARCHAR2,
    MAHANG          IN              VARCHAR2,
    MANHACUNGCAP    IN              VARCHAR2,
    MAKHACHHANG     IN              VARCHAR2,
    UNITCODE        IN              VARCHAR2,
    TUNGAY          IN              DATE,
    DENNGAY         IN              DATE,
    USERNAME        IN              VARCHAR2,
    CUR             OUT             SYS_REFCURSOR
) AS

    QUERY_STR           VARCHAR(5000);
    P_SELECT_COLUMNS    VARCHAR(3000);
    P_TABLE_GROUPBY     VARCHAR(3000);
    P_COLUMNS_GROUPBY   VARCHAR(3000);
    P_EXPRESSION        VARCHAR(3000) := '';
    P_CREATE_TABLE      VARCHAR2(1000);
    P_TRUNCATE_TABLE    VARCHAR2(200);
    N_COUNT             NUMBER(10, 0) := 0;
BEGIN
    P_TRUNCATE_TABLE := 'DELETE TEMP_BANBUON_CHITIET WHERE USERNAME = '''
                        || USERNAME
                        || '''';
    P_CREATE_TABLE := 'CREATE GLOBAL TEMPORARY TABLE "ERBUS"."TEMP_BANBUON_CHITIET" 
   (
    "MACHA" VARCHAR2(70),
    "TENCHA" NVARCHAR2(200),
    "BARCODE" VARCHAR2(2000), 
    "MA" VARCHAR2(70),
    "TEN" NVARCHAR2(200),
    "NGAY_DUYETPHIEU" DATE,
    "GROUP_CODE" VARCHAR2(70),
    "UNITCODE" VARCHAR2(10),
	"SOLUONG" NUMBER(18,2) DEFAULT 0, 
	"GIAVON" NUMBER(18,2) DEFAULT 0, 
	"GIATRI_THUE_RA" NUMBER(18,2) DEFAULT 0, 
	"TIEN_GIAMGIA" NUMBER(18,2) DEFAULT 0,
    "GIABANLE" NUMBER(18,2) DEFAULT 0, 
	"GIABANLE_VAT" NUMBER(18,2) DEFAULT 0, 
	"USERNAME" VARCHAR2(20)
   ) ON COMMIT PRESERVE ROWS'
    ;
    SELECT
        COUNT(TABLE_NAME)
    INTO N_COUNT
    FROM
        USER_TABLES
    WHERE
        TABLE_NAME = 'TEMP_BANBUON_CHITIET';

    IF N_COUNT = 0 THEN
        EXECUTE IMMEDIATE P_CREATE_TABLE;
    END IF;
    EXECUTE IMMEDIATE P_TRUNCATE_TABLE;
    IF TRIM(MAKHO) IS NOT NULL THEN
        P_EXPRESSION := P_EXPRESSION
                        || ' AND chungtu.MAKHO_xuat IN (SELECT REGEXP_SUBSTR('''
                        || MAKHO
                        || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                        || MAKHO
                        || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
    END IF;

    IF TRIM(MANHANVIEN) IS NOT NULL THEN
        P_EXPRESSION := P_EXPRESSION
                        || ' AND chungtu.I_CREATE_BY IN (SELECT REGEXP_SUBSTR('''
                        || MANHANVIEN
                        || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                        || MANHANVIEN
                        || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
    END IF;

    IF TRIM(MALOAI) IS NOT NULL THEN
        P_EXPRESSION := P_EXPRESSION
                        || ' AND mathang.MALOAI IN (SELECT REGEXP_SUBSTR('''
                        || MALOAI
                        || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                        || MALOAI
                        || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
    END IF;

    IF TRIM(MANHOM) IS NOT NULL THEN
        P_EXPRESSION := P_EXPRESSION
                        || ' AND mathang.MANHOM IN (SELECT REGEXP_SUBSTR('''
                        || MANHOM
                        || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                        || MANHOM
                        || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
    END IF;

    IF TRIM(MAHANG) IS NOT NULL THEN
        P_EXPRESSION := P_EXPRESSION
                        || ' AND chungtu_chitiet.MAHANG IN (SELECT REGEXP_SUBSTR('''
                        || MAHANG
                        || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                        || MAHANG
                        || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
    END IF;

    IF TRIM(MANHACUNGCAP) IS NOT NULL THEN
        P_EXPRESSION := P_EXPRESSION
                        || ' AND mathang.MANHACUNGCAP IN (SELECT REGEXP_SUBSTR('''
                        || MANHACUNGCAP
                        || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                        || MANHACUNGCAP
                        || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
    END IF;

    IF TRIM(MAKHACHHANG) IS NOT NULL THEN
        P_EXPRESSION := P_EXPRESSION
                        || ' AND chungtu.MAKHACHHANG IN (SELECT REGEXP_SUBSTR('''
                        || MAKHACHHANG
                        || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                        || MAKHACHHANG
                        || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
    END IF;

    IF DIEUKIEN_NHOM = 'KHOHANG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'chungtu.MAKHO_XUAT, khohang.TENKHO, mathang.BARCODE, chungtu_chitiet.MAHANG, mathang.TENHANG, chungtu.NGAY_DUYETPHIEU, chungtu.MAKHO_XUAT, chungtu.UNITCODE, thue.GIATRI,xnt.GIAVON,thue.GIATRI,chungtu_chitiet.GIABANLE,chungtu_chitiet.GIABANLE_VAT'
            ;
            P_SELECT_COLUMNS := 'chungtu.MAKHO_XUAT AS MACHA, khohang.TENKHO AS TENCHA, mathang.BARCODE AS BARCODE, chungtu_chitiet.MAHANG AS MA, mathang.TENHANG AS TEN, chungtu.NGAY_DUYETPHIEU AS NGAY_DUYETPHIEU , chungtu.MAKHO_XUAT AS GROUP_CODE, chungtu.UNITCODE'
            ;
            P_TABLE_GROUPBY := ' INNER JOIN KHOHANG khohang ON khohang.MAKHO = chungtu.MAKHO_XUAT AND chungtu.UNITCODE = khohang.UNITCODE '
            ;
        END;
    ELSIF DIEUKIEN_NHOM = 'NHOMHANG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.MANHOM, nhomhang.TENNHOM, mathang.BARCODE, chungtu_chitiet.MAHANG, mathang.TENHANG, mathang.MANHOM,chungtu.NGAY_DUYETPHIEU, chungtu.UNITCODE, thue.GIATRI,xnt.GIAVON,thue.GIATRI,chungtu_chitiet.GIABANLE,chungtu_chitiet.GIABANLE_VAT'
            ;
            P_SELECT_COLUMNS := 'mathang.MANHOM AS MACHA, nhomhang.TENNHOM AS TENCHA,mathang.BARCODE AS BARCODE, chungtu_chitiet.MAHANG AS MA, mathang.TENHANG AS TEN , chungtu.NGAY_DUYETPHIEU AS NGAY_DUYETPHIEU, mathang.MANHOM AS GROUP_CODE, chungtu.UNITCODE'
            ;
            P_TABLE_GROUPBY := ' INNER JOIN NHOMHANG nhomhang ON nhomhang.MANHOM = mathang.MANHOM AND chungtu.UNITCODE = nhomhang.UNITCODE '
            ;
        END;
    ELSIF DIEUKIEN_NHOM = 'LOAIHANG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.MALOAI, loaihang.TENLOAI, mathang.BARCODE, chungtu_chitiet.MAHANG, mathang.TENHANG, mathang.MALOAI,chungtu.NGAY_DUYETPHIEU, chungtu.UNITCODE, thue.GIATRI,xnt.GIAVON,thue.GIATRI,chungtu_chitiet.GIABANLE,chungtu_chitiet.GIABANLE_VAT'
            ;
            P_SELECT_COLUMNS := 'mathang.MALOAI AS MACHA, loaihang.TENLOAI AS TENCHA, mathang.BARCODE AS BARCODE, chungtu_chitiet.MAHANG AS MA, mathang.TENHANG AS TEN,chungtu.NGAY_DUYETPHIEU AS NGAY_DUYETPHIEU,mathang.MALOAI AS GROUP_CODE, chungtu.UNITCODE'
            ;
            P_TABLE_GROUPBY := ' INNER JOIN LOAIHANG loaihang ON loaihang.MALOAI = mathang.MALOAI AND chungtu.UNITCODE = loaihang.UNITCODE '
            ;
        END;
    ELSIF DIEUKIEN_NHOM = 'NHACUNGCAP' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.MANHACUNGCAP, nhacungcap.TENNHACUNGCAP, mathang.BARCODE, chungtu_chitiet.MAHANG, mathang.TENHANG, mathang.MANHACUNGCAP,chungtu.NGAY_DUYETPHIEU, chungtu.UNITCODE, thue.GIATRI,xnt.GIAVON,thue.GIATRI,chungtu_chitiet.GIABANLE,chungtu_chitiet.GIABANLE_VAT'
            ;
            P_SELECT_COLUMNS := 'mathang.MANHACUNGCAP AS MACHA, nhacungcap.TENNHACUNGCAP AS TENCHA, mathang.BARCODE AS BARCODE, chungtu_chitiet.MAHANG AS MA, mathang.TENHANG AS TEN, chungtu.NGAY_DUYETPHIEU AS NGAY_DUYETPHIEU, mathang.MANHACUNGCAP AS GROUP_CODE, chungtu.UNITCODE'
            ;
            P_TABLE_GROUPBY := ' INNER JOIN NHACUNGCAP nhacungcap ON nhacungcap.MANHACUNGCAP = mathang.MANHACUNGCAP AND chungtu.UNITCODE = nhacungcap.UNITCODE '
            ;
        END;
    ELSIF DIEUKIEN_NHOM = 'KHACHHANG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'chungtu.MAKHACHHANG, khachhang.TENKHACHHANG, mathang.BARCODE, chungtu_chitiet.MAHANG, mathang.TENHANG, chungtu.MAKHACHHANG,chungtu.NGAY_DUYETPHIEU, chungtu.UNITCODE, thue.GIATRI,xnt.GIAVON,thue.GIATRI,chungtu_chitiet.GIABANLE,chungtu_chitiet.GIABANLE_VAT'
            ;
            P_SELECT_COLUMNS := 'chungtu.MAKHACHHANG AS MACHA, khachhang.TENKHACHHANG AS TENCHA, mathang.BARCODE AS BARCODE, chungtu_chitiet.MAHANG AS MA, mathang.TENHANG AS TEN, chungtu.NGAY_DUYETPHIEU AS NGAY_DUYETPHIEU, chungtu.MAKHACHHANG AS GROUP_CODE, chungtu.UNITCODE'
            ;
            P_TABLE_GROUPBY := ' INNER JOIN KHACHHANG khachhang ON khachhang.MAKHACHHANG = chungtu.MAKHACHHANG AND chungtu.UNITCODE = khachhang.UNITCODE '
            ;
        END;
    ELSIF DIEUKIEN_NHOM = 'GIAODICH' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.TENHANG, mathang.BARCODE, chungtu_chitiet.MAHANG, chungtu.MA_chungtu, chungtu.NGAY_DUYETPHIEU, chungtu.LOAI_chungtu, chungtu.UNITCODE, thue.GIATRI,xnt.GIAVON,thue.GIATRI,chungtu_chitiet.GIABANLE,chungtu_chitiet.GIABANLE_VAT'
            ;
            P_SELECT_COLUMNS := 'chungtu.MA_chungtu AS MACHA, chungtu.LOAI_chungtu AS TENCHA, mathang.BARCODE AS BARCODE, chungtu_chitiet.MAHANG AS MA, mathang.TENHANG AS TEN ,chungtu.NGAY_DUYETPHIEU, chungtu.MA_chungtu AS GROUP_CODE, chungtu.UNITCODE'
            ;
            P_TABLE_GROUPBY := ' ';
        END;
    ELSIF DIEUKIEN_NHOM = 'NGUOIDUNG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.BARCODE,nguoidung.TENNHANVIEN, chungtu_chitiet.MAHANG, mathang.TENHANG, chungtu.I_CREATE_BY, chungtu.NGAY_DUYETPHIEU, chungtu.UNITCODE, thue.GIATRI,xnt.GIAVON,thue.GIATRI,chungtu_chitiet.GIABANLE,chungtu_chitiet.GIABANLE_VAT'
            ;
            P_SELECT_COLUMNS := 'chungtu.I_CREATE_BY AS MACHA, nguoidung.TENNHANVIEN AS TENCHA, mathang.BARCODE AS BARCODE, chungtu_chitiet.MAHANG AS MA, mathang.TENHANG AS TEN, chungtu.NGAY_DUYETPHIEU AS NGAY_DUYETPHIEU, chungtu.I_CREATE_BY AS GROUP_CODE, chungtu.UNITCODE'
            ;
            P_TABLE_GROUPBY := ' INNER JOIN NGUOIDUNG nguoidung ON chungtu.I_CREATE_BY = nguoidung.USERNAME AND chungtu.UNITCODE= nguoidung.UNITCODE '
            ;
        END;
    ELSE
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.BARCODE, mathang.MAHANG, mathang.TENHANG, chungtu.NGAY_DUYETPHIEU, chungtu.UNITCODE, thue.GIATRI,xnt.GIAVON,thue.GIATRI,chungtu_chitiet.GIABANLE,chungtu_chitiet.GIABANLE_VAT'
            ;
            P_SELECT_COLUMNS := 'mathang.MAHANG AS MACHA, mathang.TENHANG AS TENCHA, mathang.BARCODE AS BARCODE,mathang.MAHANG AS MA, mathang.TENHANG AS TEN , chungtu.NGAY_DUYETPHIEU AS NGAY_DUYETPHIEU,mathang.MAHANG AS GROUP_CODE, chungtu.UNITCODE AS UNITCODE'
            ;
            P_TABLE_GROUPBY := ' ';
        END;
    END IF;

    DECLARE
        TABLE_NAME      VARCHAR2(20);
        QUERRY          VARCHAR2(200);
        CUR_RESULT      SYS_REFCURSOR;
        BEGIN_DAY       DATE;
        END_DAY         DATE;
        KYKETOAN        NUMBER(10, 0);
        NAM             NUMBER(10, 0);
        QUERRY_INSERT   VARCHAR2(3000) := '';
    BEGIN
        QUERRY := 'SELECT TUNGAY, DENNGAY, KYKETOAN, NAM FROM KYKETOAN WHERE KYKETOAN.TUNGAY >= '''
                  || TUNGAY
                  || ''' AND KYKETOAN.DENNGAY <= '''
                  || DENNGAY
                  || ''' ORDER BY KYKETOAN.TUNGAY,KYKETOAN.DENNGAY';
        OPEN CUR_RESULT FOR QUERRY;

        LOOP
            FETCH CUR_RESULT INTO
                BEGIN_DAY,
                END_DAY,
                KYKETOAN,
                NAM;
            EXIT WHEN CUR_RESULT%NOTFOUND;
            SELECT
                'XNT_'
                || NAM
                || '_KY_'
                || KYKETOAN
            INTO TABLE_NAME
            FROM
                DUAL;

            QUERRY_INSERT := 'INSERT INTO TEMP_BANBUON_CHITIET(MACHA, TENCHA, BARCODE, MA,TEN, NGAY_DUYETPHIEU, GROUP_CODE, UNITCODE,SOLUONG,GIAVON,GIATRI_THUE_RA, TIEN_GIAMGIA,GIABANLE,GIABANLE_VAT, USERNAME)
            SELECT '
                             || P_SELECT_COLUMNS
                             || '

                            ,ROUND(SUM(NVL(chungtu_chitiet.SOLUONG,0)), 2) AS SOLUONG
                            ,NVL(xnt.GIAVON, 0) AS GIAVON
                            ,NVL(thue.GIATRI, 0) AS GIATRI_THUE_RA
                            ,ROUND(SUM(NVL(chungtu_chitiet.TIEN_GIAMGIA,0)), 2) AS TIEN_GIAMGIA
                            ,NVL(chungtu_chitiet.GIABANLE,0) AS GIABANLE
                            ,NVL(chungtu_chitiet.GIABANLE_VAT,0) AS GIABANLE_VAT
                              ,'''
                             || USERNAME
                             || ''' AS USERNAME
    FROM CHUNGTU chungtu
    INNER JOIN CHUNGTU_CHITIET chungtu_chitiet ON chungtu.MA_CHUNGTU = chungtu_chitiet.MA_CHUNGTU AND chungtu.LOAI_CHUNGTU = ''XBAN''
    INNER JOIN MATHANG mathang ON mathang.MAHANG = chungtu_chitiet.MAHANG 
    INNER JOIN MATHANG_GIA mathang_gia ON mathang.MAHANG = mathang_gia.MAHANG
    INNER JOIN THUE thue ON chungtu_chitiet.MATHUE_RA = thue.MATHUE
    INNER JOIN '
                             || TABLE_NAME
                             || ' xnt ON xnt.MAHANG = chungtu_chitiet.MAHANG AND xnt.MAKHO = chungtu.MAKHO_XUAT
    '
                             || P_TABLE_GROUPBY
                             || '
    AND mathang.UNITCODE = chungtu.UNITCODE
    WHERE
    chungtu.TRANGTHAI = 10 AND chungtu.LOAI_CHUNGTU = ''XBAN''
    AND chungtu.UNITCODE = '''
                             || UNITCODE
                             || '''
    AND TO_DATE(chungtu.NGAY_DUYETPHIEU,''DD/MM/YY'') <= TO_DATE('''
                             || END_DAY
                             || ''',''DD/MM/YY'')
    AND TO_DATE(chungtu.NGAY_DUYETPHIEU,''DD/MM/YY'') >= TO_DATE('''
                             || BEGIN_DAY
                             || ''',''DD/MM/YY'')
	'
                             || P_EXPRESSION
                             || '
	GROUP BY '
                             || P_COLUMNS_GROUPBY;

            --DBMS_OUTPUT.PUT_LINE(QUERRY_INSERT);

            EXECUTE IMMEDIATE QUERRY_INSERT;
        END LOOP;

        CLOSE CUR_RESULT;
    END;

    BEGIN
        QUERY_STR := 'SELECT MACHA, TENCHA, BARCODE, MACON, TENCON, NGAY_DUYETPHIEU, GROUP_CODE ,SOLUONG,GIAVON,VON,VON_VAT,TIEN_GIAMGIA,DOANHTHU,TIENTHUE,LAIBANLE,DOANHTHU + TIENTHUE AS TONGBAN,GIABANLE_VAT
        FROM
        (
        SELECT MACHA, TENCHA, BARCODE, MA AS MACON, TEN AS TENCON, NGAY_DUYETPHIEU, GROUP_CODE, SOLUONG,
        ROUND(GIAVON, 2) AS GIAVON,
        ROUND(GIAVON * SOLUONG, 2) AS VON,
        ROUND(GIAVON * (1 + (GIATRI_THUE_RA / 100)) * SOLUONG, 2) AS VON_VAT,
        ROUND(TIEN_GIAMGIA, 2) AS TIEN_GIAMGIA,
        ROUND((NVL(SOLUONG,0) * NVL(GIABANLE_VAT,0))/(1 + (GIATRI_THUE_RA / 100)), 2) AS DOANHTHU,
        ROUND(SOLUONG * (GIATRI_THUE_RA / 100) * (GIABANLE_VAT / (1 + (GIATRI_THUE_RA / 100))), 2) AS TIENTHUE,
        ROUND(SOLUONG * (GIABANLE_VAT /(1 + (GIATRI_THUE_RA / 100))) - ROUND(GIAVON * SOLUONG, 2) ,2) AS LAIBANLE,
        ROUND(GIABANLE_VAT, 2) AS GIABANLE_VAT
        FROM TEMP_BANBUON_CHITIET WHERE USERNAME = '''
                     || USERNAME
                     || '''
        )';
        --DBMS_OUTPUT.PUT_LINE(QUERY_STR);
        OPEN CUR FOR QUERY_STR;

    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            NULL;
        WHEN OTHERS THEN
            NULL;
    END;

END BAOCAO_BANBUON_CHITIET;