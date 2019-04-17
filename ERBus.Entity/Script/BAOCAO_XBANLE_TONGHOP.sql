create or replace PROCEDURE "BAOCAO_XBANLE_TONGHOP" (
    DIEUKIEN_NHOM   IN              VARCHAR2,
    MAKHO           IN              VARCHAR2,
    MALOAI          IN              VARCHAR2,
    MANHOM          IN              VARCHAR2,
    MAHANG          IN              VARCHAR2,
    MANHACUNGCAP    IN              VARCHAR2,
    UNITCODE        IN              VARCHAR2,
    TUNGAY          IN              DATE,
    DENNGAY         IN              DATE,
    MANHANVIEN      IN              VARCHAR2,
    USERNAME        IN              VARCHAR2,
    CUR             OUT             SYS_REFCURSOR
) AS

    QUERY_STR                      VARCHAR(5000) := '';
    P_SELECT_COLUMNS    VARCHAR(3000) := '';
    P_TABLE_GROUPBY                VARCHAR(3000) := '';
    P_COLUMNS_GROUPBY              VARCHAR(3000) := '';
    P_EXPRESSION                   VARCHAR(3000) := '';
    P_CREATE_TABLE                 VARCHAR2(1000);
    P_TRUNCATE_TABLE               VARCHAR2(200);
    N_COUNT                        NUMBER(10, 0) := 0;
BEGIN
    P_TRUNCATE_TABLE := 'DELETE TEMP_XBANLE_TONGHOP WHERE USERNAME = '''
                        || USERNAME
                        || '''';
    P_CREATE_TABLE := 'CREATE GLOBAL TEMPORARY TABLE "ERBUS"."TEMP_XBANLE_TONGHOP" 
   (
    "MA" VARCHAR2(70),
    "TEN" NVARCHAR2(200), 
    "UNITCODE" VARCHAR2(10),
	"SOLUONG" NUMBER(18,2) DEFAULT 0, 
	"GIAVON" NUMBER(18,2) DEFAULT 0, 
	"GIATRI_THUE_RA" NUMBER(18,2) DEFAULT 0, 
	"TIENTHE_VIP" NUMBER(18,2) DEFAULT 0, 
	"TIEN_CHIETKHAU" NUMBER(18,2) DEFAULT 0, 
    "TIEN_KHUYENMAI" NUMBER(18,2) DEFAULT 0, 
	"TIEN_VOUCHER" NUMBER(18,2) DEFAULT 0, 
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
        TABLE_NAME = 'TEMP_XBANLE_TONGHOP';

    IF N_COUNT = 0 THEN
        EXECUTE IMMEDIATE P_CREATE_TABLE;
    END IF;
    EXECUTE IMMEDIATE P_TRUNCATE_TABLE;
    COMMIT;
    IF TRIM(MAKHO) IS NOT NULL THEN
        P_EXPRESSION := P_EXPRESSION
                        || ' AND giaodich.MAKHO_xuat IN (SELECT REGEXP_SUBSTR('''
                        || MAKHO
                        || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                        || MAKHO
                        || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
    END IF;

    IF TRIM(MANHANVIEN) IS NOT NULL THEN
        P_EXPRESSION := P_EXPRESSION
                        || ' AND giaodich.I_CREATE_BY IN (SELECT REGEXP_SUBSTR('''
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
                        || ' AND giaodich_chitiet.MAHANG IN (SELECT REGEXP_SUBSTR('''
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

    IF DIEUKIEN_NHOM = 'KHOHANG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'giaodich.MAKHO_XUAT,khohang.TENKHO, giaodich.UNITCODE, thue.GIATRI,xnt.GIAVON,giaodich_chitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := ' giaodich.MAKHO_XUAT AS MA,khohang.TENKHO AS TEN,giaodich.UNITCODE ';
            P_TABLE_GROUPBY := ' INNER JOIN KHOHANG khohang ON giaodich.MAKHO_XUAT = khohang.MAKHO';
        END;
    ELSIF DIEUKIEN_NHOM = 'LOAIHANG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.MALOAI, loaihang.TENLOAI, giaodich.UNITCODE, thue.GIATRI,xnt.GIAVON,giaodich_chitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := ' mathang.MALOAI AS MA,loaihang.TENLOAI AS TEN, giaodich.UNITCODE ';
            P_TABLE_GROUPBY := ' INNER JOIN LOAIHANG loaihang ON mathang.MALOAI = loaihang.MALOAI';
        END;
    ELSIF DIEUKIEN_NHOM = 'NHOMHANG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.MANHOM, nhomhang.TENNHOM, giaodich.UNITCODE, thue.GIATRI,xnt.GIAVON,giaodich_chitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := ' mathang.MANHOM AS MA,nhomhang.TENNHOM AS TEN, giaodich.UNITCODE ';
            P_TABLE_GROUPBY := ' INNER JOIN NHOMHANG nhomhang ON mathang.MANHOM = nhomhang.MANHOM ';
        END;
    ELSIF DIEUKIEN_NHOM = 'NHACUNGCAP' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.MANHACUNGCAP, nhacungcap.TENNHACUNGCAP, giaodich.UNITCODE, thue.GIATRI,xnt.GIAVON,giaodich_chitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := ' mathang.MANHACUNGCAP AS MA,nhacungcap.TENNHACUNGCAP AS TEN, giaodich.UNITCODE ';
            P_TABLE_GROUPBY := '  INNER JOIN NHACUNGCAP nhacungcap ON mathang.MANHACUNGCAP = nhacungcap.MANHACUNGCAP';
        END;
    ELSIF DIEUKIEN_NHOM = 'GIAODICH' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'giaodich.MA_GIAODICH, giaodich.NGAY_GIAODICH, giaodich.UNITCODE, thue.GIATRI,xnt.GIAVON,giaodich_chitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := 'giaodich.MA_GIAODICH AS MA, giaodich.NGAY_GIAODICH AS TEN, giaodich.UNITCODE AS UNITCODE '
            ;
            P_TABLE_GROUPBY := ' ';
        END;
    ELSIF DIEUKIEN_NHOM = 'NGUOIDUNG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'giaodich.I_CREATE_BY, nguoidung.TENNHANVIEN, giaodich.UNITCODE, thue.GIATRI, xnt.GIAVON,giaodich_chitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := ' giaodich.I_CREATE_BY AS MA, nguoidung.TENNHANVIEN AS TEN, giaodich.UNITCODE ';
            P_TABLE_GROUPBY := ' INNER JOIN NGUOIDUNG nguoidung ON giaodich.I_CREATE_BY = nguoidung.MANHANVIEN';
        END;
    ELSE
        BEGIN
            P_COLUMNS_GROUPBY := 'giaodich_chitiet.MAHANG,mathang.TENHANG,giaodich.UNITCODE, thue.GIATRI,xnt.GIAVON,giaodich_chitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := ' giaodich_chitiet.MAHANG AS MA, mathang.TENHANG AS TEN, giaodich.UNITCODE ';
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
        QUERRY_INSERT   VARCHAR2(2000) := '';
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

            QUERRY_INSERT := 'INSERT INTO TEMP_XBANLE_TONGHOP(MA,TEN,UNITCODE,SOLUONG,GIAVON,GIATRI_THUE_RA,
        TIENTHE_VIP,TIEN_CHIETKHAU,TIEN_KHUYENMAI,TIEN_VOUCHER,GIABANLE_VAT,USERNAME)
            SELECT '
                             || P_SELECT_COLUMNS
                             || '
                              ,SUM(NVL(giaodich_chitiet.SOLUONG,0)) AS SOLUONG
                              ,NVL(xnt.GIAVON, 0) AS GIAVON
                              ,NVL(thue.GIATRI, 0) AS GIATRI_THUE_RA
                              ,SUM(NVL(giaodich_chitiet.TIENTHE_VIP, 0)) AS TIENTHE_VIP
                              ,SUM(NVL(giaodich_chitiet.TIEN_CHIETKHAU, 0)) AS TIEN_CHIETKHAU
                              ,SUM(NVL(giaodich_chitiet.TIEN_KHUYENMAI, 0)) AS TIEN_KHUYENMAI
                              ,SUM(NVL(giaodich_chitiet.TIEN_VOUCHER, 0)) AS TIEN_VOUCHER
                              ,NVL(giaodich_chitiet.GIABANLE_VAT,0) AS GIABANLE_VAT
                              ,'''
                             || USERNAME
                             || ''' AS USERNAME
    FROM GIAODICH giaodich 
    INNER JOIN GIAODICH_CHITIET giaodich_chitiet ON giaodich.MA_GIAODICH = giaodich_chitiet.MA_GIAODICH 
    INNER JOIN MATHANG mathang ON mathang.MAHANG = giaodich_chitiet.MAHANG AND giaodich.UNITCODE= mathang.UNITCODE
    INNER JOIN '
                             || TABLE_NAME
                             || ' xnt ON xnt.MAHANG = giaodich_chitiet.MAHANG AND xnt.MAKHO = giaodich.MAKHO_XUAT
    INNER JOIN THUE thue ON giaodich_chitiet.MATHUE_RA = thue.MATHUE
    '
                             || P_TABLE_GROUPBY
                             || '
    WHERE giaodich.LOAI_GIAODICH = ''XBAN_LE''
        AND giaodich.UNITCODE = '''
                             || UNITCODE
                             || '''
        AND TO_DATE(giaodich.NGAY_GIAODICH,''DD/MM/YY'') <= TO_DATE('''
                             || END_DAY
                             || ''',''DD/MM/YY'')
        AND TO_DATE(giaodich.NGAY_GIAODICH,''DD/MM/YY'') >= TO_DATE('''
                             || BEGIN_DAY
                             || ''',''DD/MM/YY'') 
        '
                             || P_EXPRESSION
                             || '
        GROUP BY '
                             || P_COLUMNS_GROUPBY
                             || '
        ';
--    DBMS_OUTPUT.PUT_LINE(QUERRY_INSERT);

            EXECUTE IMMEDIATE QUERRY_INSERT;
        END LOOP;
        CLOSE CUR_RESULT;
    END;

    BEGIN
        QUERY_STR := 'SELECT MA,TEN,SOLUONG,GIAVON,VON,VON_VAT,TIEN_CHIETKHAU,TIEN_KHUYENMAI,TIENTHE_VIP,DOANHTHU,TIENTHUE,LAIBANLE,DOANHTHU + TIENTHUE AS TONGBAN,GIABANLE_VAT
        FROM
        (
        SELECT MA,TEN,SOLUONG,GIAVON,
        ROUND(GIAVON * SOLUONG, 2) AS VON,
        GIAVON * (1 + (GIATRI_THUE_RA / 100)) * SOLUONG AS VON_VAT,
        TIEN_CHIETKHAU,TIEN_KHUYENMAI,TIENTHE_VIP,
        SOLUONG * (GIABANLE_VAT / (1 + GIATRI_THUE_RA / 100)) - (TIEN_CHIETKHAU + TIEN_KHUYENMAI + TIENTHE_VIP) AS DOANHTHU,
        SOLUONG * (GIATRI_THUE_RA / 100) * (GIABANLE_VAT / (1 + (GIATRI_THUE_RA / 100))) AS TIENTHUE,
        SOLUONG * (GIABANLE_VAT /(1 + (GIATRI_THUE_RA / 100))) - ROUND(GIAVON * SOLUONG, 2) AS LAIBANLE,
        GIABANLE_VAT
        FROM TEMP_XBANLE_TONGHOP WHERE USERNAME = '''
                         || USERNAME
                         || '''
        )';
--   -DBMS_OUTPUT.PUT_LINE(QUERY_STR);

 OPEN CUR FOR QUERY_STR;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            NULL;
        WHEN OTHERS THEN
            NULL;
    END;

END BAOCAO_XBANLE_TONGHOP;