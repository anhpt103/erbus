create or replace PROCEDURE "NHAPMUA_CHITIET" (
    LOAI_CHUNGTU    IN              VARCHAR2,
    DIEUKIEN_NHOM   IN              VARCHAR2,
    MAKHO           IN              VARCHAR2,
    MALOAI          IN              VARCHAR2,
    MANHOM          IN              VARCHAR2,
    MAHANG          IN              VARCHAR2,
    MANHACUNGCAP    IN              VARCHAR2,
    UNITCODE        IN              VARCHAR2,
    MANHANVIEN      IN              VARCHAR2,
    TUNGAY          IN              DATE,
    DENNGAY         IN              DATE,
    MATHUE          IN              VARCHAR2,
    CUR             OUT             SYS_REFCURSOR
) AS

    P_EXPRESSION               VARCHAR(3000);
    QUERY_STR                  VARCHAR(5000);
    P_TABLE_GROUPBY            VARCHAR(5000);
    P_COLUMNS_GROUPBY          VARCHAR(5000);
    P_SELECT_COLUMNS_GROUPBY   VARCHAR(5000);
    P_SELECT                   VARCHAR(2000);
    P_SELECT2                  VARCHAR(2000);
BEGIN
    IF TRIM(MAKHO) IS NOT NULL THEN
        P_EXPRESSION := P_EXPRESSION
                        || ' AND chungtu.MAKHO_NHAP IN (SELECT REGEXP_SUBSTR('''
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
                        || ' AND chungtu.MANHACUNGCAP IN (SELECT REGEXP_SUBSTR('''
                        || MANHACUNGCAP
                        || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                        || MANHACUNGCAP
                        || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
    END IF;

    IF TRIM(MATHUE) IS NOT NULL THEN
        P_EXPRESSION := P_EXPRESSION
                        || ' AND chungtu_chitiet.MATHUE_VAO IN (SELECT REGEXP_SUBSTR('''
                        || MANHACUNGCAP
                        || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                        || MANHACUNGCAP
                        || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
    END IF;

    IF DIEUKIEN_NHOM = 'KHOHANG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'chungtu.MAKHO_NHAP, kho.TENKHO,mathang.MAHANG,mathang.TENHANG,chungtu_chitiet.GIAMUA,mathang_gia.GIABANLE_VAT,chungtu.NGAY_CHUNGTU,nhacungcap.TENNHACUNGCAP'
            ;
            P_SELECT_COLUMNS_GROUPBY := ' NVL(a.MAKHO, ''NULL'') AS  MACHA, NVL(a.TENKHO, ''NULL'') AS  TENCHA,a.MAHANG AS  MACON,a.TENHANG AS  TENCON,a.NGAY_CHUNGTU AS  NGAY_CHUNGTU  '
            ;
            P_TABLE_GROUPBY := ' INNER JOIN KHOHANG kho ON chungtu.MAKHO_NHAP = kho.MAKHO AND kho.UNITCODE = '''
                               || UNITCODE
                               || '''';
            P_SELECT2 := 'nhacungcap.TENNHACUNGCAP AS  TENNHACUNGCAP,chungtu.NGAY_CHUNGTU AS  NGAY_CHUNGTU,chungtu.MAKHO_NHAP AS  MAKHO,kho.TENKHO AS  TENKHO,mathang.MAHANG AS  MAHANG,mathang.TENHANG AS  TENHANG'
            ;
            P_SELECT := ' ';
        END;
    ELSIF DIEUKIEN_NHOM = 'NHOMHANG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.MANHOM,nhomhang.TENNHOM,mathang.MAHANG,mathang.TENHANG,chungtu_chitiet.GIAMUA,mathang_gia.GIABANLE_VAT,chungtu.NGAY_CHUNGTU ,nhacungcap.TENNHACUNGCAP'
            ;
            P_SELECT_COLUMNS_GROUPBY := ' NVL(a.MANHOM, ''NULL'')  AS  MACHA, NVL(A.TENNHOM, ''NULL'') AS  TENCHA,a.MAHANG AS MACON,a.TENHANG AS TENCON,a.NGAY_CHUNGTU AS  NGAY_CHUNGTU '
            ;
            P_TABLE_GROUPBY := ' INNER JOIN NHOMHANG nhomhang on mathang.MANHOM = nhomhang.MANHOM AND nhomhang.UNITCODE = '''
                               || UNITCODE
                               || '''';
            P_SELECT2 := 'nhacungcap.TENNHACUNGCAP AS  TENNHACUNGCAP,chungtu.NGAY_CHUNGTU AS  NGAY_CHUNGTU, mathang.MANHOM AS MANHOM,nhomhang.TENNHOM AS TENNHOM,mathang.MAHANG AS  MAHANG,mathang.TENHANG AS  TENHANG'
            ;
            P_SELECT := ' ';
        END;
    ELSIF DIEUKIEN_NHOM = 'LOAIHANG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.MALOAI,loaihang.TENLOAI,mathang.MAHANG,mathang.TENHANG,chungtu_chitiet.GIAMUA,mathang_gia.GIABANLE_VAT,chungtu.NGAY_CHUNGTU,nhacungcap.TENNHACUNGCAP '
            ;
            P_SELECT_COLUMNS_GROUPBY := ' NVL(a.MALOAI, ''NULL'') AS MACHA, NVL(a.TENLOAI, ''NULL'') AS TENCHA,a.MAHANG AS MACON,a.TENHANG AS TENCON,a.NGAY_CHUNGTU AS  NGAY_CHUNGTU '
            ;
            P_TABLE_GROUPBY := ' INNER JOIN LOAIHANG loaihang ON mathang.MALOAI = loaihang.MALOAI AND loaihang.UNITCODE = '''
                               || UNITCODE
                               || '''';
            P_SELECT2 := 'nhacungcap.TENNHACUNGCAP AS  TENNHACUNGCAP, chungtu.NGAY_CHUNGTU AS  NGAY_CHUNGTU,mathang.MALOAI AS MALOAI,loaihang.TENLOAI AS TENLOAI,mathang.MAHANG AS MAHANG,mathang.TENHANG AS TENHANG'
            ;
            P_SELECT := ' ';
        END;
    ELSIF DIEUKIEN_NHOM = 'NHACUNGCAP' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'chungtu.MANHACUNGCAP,nhacungcap.TENNHACUNGCAP,mathang.MAHANG,mathang.TENHANG,chungtu_chitiet.GIAMUA,mathang_gia.GIABANLE_VAT,chungtu.NGAY_CHUNGTU'
            ;
            P_SELECT_COLUMNS_GROUPBY := ' NVL(a.MANHACUNGCAP, ''NULL'')  AS MACHA, NVL(a.TENNHACUNGCAP, ''NULL'') AS  TENCHA,a.MAHANG AS  MACON,a.TENHANG AS  TENCON,a.NGAY_CHUNGTU AS  NGAY_CHUNGTU '
            ;
            P_TABLE_GROUPBY := '  AND nhacungcap.UNITCODE = '''
                               || UNITCODE
                               || '''';
            P_SELECT2 := 'nhacungcap.TENNHACUNGCAP AS TENNHACUNGCAP, chungtu.NGAY_CHUNGTU AS  NGAY_CHUNGTU ,chungtu.MANHACUNGCAP AS MANHACUNGCAP,mathang.MAHANG AS  MAHANG,mathang.TENHANG AS  TENHANG'
            ;
            P_SELECT := ' ';
        END;
    ELSIF DIEUKIEN_NHOM = 'CHUNGTU' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'chungtu.MA_CHUNGTU,chungtu.DIENGIAI,mathang.MAHANG,mathang.TENHANG,chungtu_chitiet.GIAMUA,mathang_gia.GIABANLE_VAT,chungtu.NGAY_CHUNGTU,nhacungcap.TENNHACUNGCAP'
            ;
            P_SELECT_COLUMNS_GROUPBY := ' NVL(A.MA_CHUNGTU, ''NULL'')  AS  MACHA, NVL(A.DIENGIAI, ''NULL'') AS  TENCHA,a.MAHANG AS  MACON,a.TENHANG AS  TENCON,a.NGAY_CHUNGTU AS  NGAY_CHUNGTU '
            ;
            P_TABLE_GROUPBY := ' AND chungtu.UNITCODE = '''
                               || UNITCODE
                               || ''' ';
            P_SELECT2 := 'nhacungcap.TENNHACUNGCAP AS  TENNHACUNGCAP, chungtu.NGAY_CHUNGTU AS  NGAY_CHUNGTU ,chungtu.MA_CHUNGTU AS  MA_CHUNGTU, chungtu.DIENGIAI,mathang.MAHANG AS  MAHANG,mathang.TENHANG AS  TENHANG'
            ;
            P_SELECT := ' ';
        END;
    ELSIF DIEUKIEN_NHOM = 'MATHUE' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'chungtu_chitiet.MATHUE_VAO, thue.TENTHUE,mathang.MAHANG,mathang.TENHANG,chungtu_chitiet.GIAMUA,mathang_gia.GIABANLE_VAT,chungtu.NGAY_CHUNGTU,nhacungcap.TENNHACUNGCAP'
            ;
            P_SELECT_COLUMNS_GROUPBY := ' NVL(a.MATHUE_VAO, ''NULL'')  AS  MACHA, NVL(a.TENTHUE, ''NULL'') AS  TENCHA,a.MAHANG AS  MACON,a.TENHANG AS  TENCON,a.NGAY_CHUNGTU AS  NGAY_CHUNGTU  '
            ;
            P_TABLE_GROUPBY := ' ';
            P_SELECT2 := 'nhacungcap.TENNHACUNGCAP AS TENNHACUNGCAP,chungtu.NGAY_CHUNGTU AS  NGAY_CHUNGTU,chungtu_chitiet.MATHUE_VAO AS MATHUE_VAO,thue.TENTHUE AS  TENTHUE,mathang.MAHANG AS  MAHANG,mathang.TENHANG AS  TENHANG'
            ;
            P_SELECT := ' ';
        END;
    ELSIF DIEUKIEN_NHOM = 'NGUOIDUNG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'chungtu.I_CREATE_BY, nguoidung.TENNHANVIEN,mathang.MAHANG,mathang.TENHANG,chungtu_chitiet.GIAMUA,mathang_gia.GIABANLE_VAT,chungtu.NGAY_CHUNGTU,nhacungcap.TENNHACUNGCAP'
            ;
            P_SELECT_COLUMNS_GROUPBY := ' NVL(a.I_CREATE_BY, ''NULL'')  AS  MACHA, NVL(a.TENNHANVIEN, ''NULL'') AS  TENCHA,a.MAHANG AS  MACON,a.TENHANG AS  TENCON,a.NGAY_CHUNGTU AS  NGAY_CHUNGTU  '
            ;
            P_TABLE_GROUPBY := ' INNER JOIN NGUOIDUNG nguoidung ON chungtu.I_CREATE_BY = nguoidung.USERNAME AND nguoidung.UNITCODE = '''
                               || UNITCODE
                               || '''';
            P_SELECT2 := 'nhacungcap.TENNHACUNGCAP AS  TENNHACUNGCAP,chungtu.NGAY_CHUNGTU AS  NGAY_CHUNGTU,chungtu.I_CREATE_BY AS I_CREATE_BY,nguoidung.TENNHANVIEN AS TENNHANVIEN,mathang.MAHANG AS  MAHANG,mathang.TENHANG AS TENHANG'
            ;
            P_SELECT := ' ';
        END;
    ELSE
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.MAHANG,mathang.TENHANG,chungtu_chitiet.GIAMUA,mathang_gia.GIABANLE_VAT,chungtu.NGAY_CHUNGTU,nhacungcap.TENNHACUNGCAP'
            ;
            P_SELECT_COLUMNS_GROUPBY := ' NVL(a.MAHANG, ''NULL'')  AS  MACHA, NVL(a.TENHANG, ''NULL'') AS  TENCHA,a.MAHANG AS  MACON,a.TENHANG AS  TENCON,a.NGAY_CHUNGTU AS  NGAY_CHUNGTU '
            ;
            P_TABLE_GROUPBY := ' AND mathang.UNITCODE = '''
                               || UNITCODE
                               || ''' ';
            P_SELECT2 := 'nhacungcap.TENNHACUNGCAP AS  TENNHACUNGCAP, chungtu.NGAY_CHUNGTU AS  NGAY_CHUNGTU,mathang.MAHANG AS  MAHANG,mathang.TENHANG AS  TENHANG'
            ;
            P_SELECT := ' ';
        END;
    END IF;

    QUERY_STR := 'SELECT a.BARCODE,(a.SOLUONG) AS  SOLUONG,(a.DONGIANHAP) AS  DONGIANHAP,a.GIABANLE_VAT AS  GIABAN, (a.TIENHANG) AS  TIENHANG, (a.TIENVAT) AS  TIENVAT, (a.TIEN_CHIETKHAU) AS  TIEN_CHIETKHAU, (a.TIENHANG - a.TIEN_CHIETKHAU + a.TIENVAT) AS  TONGTIEN ,a.TENNHACUNGCAP,'
                 || P_SELECT_COLUMNS_GROUPBY
                 || '
        FROM (
        SELECT '
                 || P_SELECT2
                 || ',
        SUM(chungtu_chitiet.SOLUONG) AS  SOLUONG, 
        SUM(chungtu_chitiet.SOLUONG * ROUND(NVL(chungtu_chitiet.GIAMUA,0),2)) AS  TIENHANG ,
        SUM(ROUND(NVL(chungtu_chitiet.GIAMUA,0),2) * ROUND(NVL(thue.GIATRI /100 ,0 ),2) * ROUND(chungtu_chitiet.SOLUONG,2)) AS  TIENVAT, 
        SUM(ROUND(NVL(chungtu_chitiet.TIEN_GIAMGIA, 0), 2)) AS TIEN_CHIETKHAU,
        ROUND(NVL(chungtu_chitiet.GIAMUA,0),2) AS  DONGIANHAP,
        ROUND(NVL(mathang_gia.GIABANLE_VAT,0),2) AS GIABANLE_VAT,
        mathang.BARCODE AS BARCODE
        FROM CHUNGTU chungtu INNER JOIN CHUNGTU_CHITIET chungtu_chitiet ON chungtu.MA_CHUNGTU = chungtu_chitiet.MA_CHUNGTU 
		INNER JOIN THUE thue ON thue.MATHUE = chungtu_chitiet.MATHUE_VAO
		INNER JOIN MATHANG mathang ON mathang.MAHANG = chungtu_chitiet.MAHANG INNER JOIN MATHANG_GIA mathang_gia 
        ON mathang.MAHANG = mathang_gia.MAHANG INNER JOIN NHACUNGCAP nhacungcap ON mathang.MANHACUNGCAP = nhacungcap.MANHACUNGCAP
        AND mathang.UNITCODE = '''
                 || UNITCODE
                 || '''
'
                 || P_TABLE_GROUPBY
                 || '
WHERE
    chungtu.TRANGTHAI = 10
    AND TO_DATE(chungtu.NGAY_DUYETPHIEU,''DD/MM/YY'') <= TO_DATE('''
                 || DENNGAY
                 || ''',''DD/MM/YY'')
    AND TO_DATE(chungtu.NGAY_DUYETPHIEU,''DD/MM/YY'') >= TO_DATE('''
                 || TUNGAY
                 || ''',''DD/MM/YY'') AND chungtu.UNITCODE = '''
                 || UNITCODE
                 || ''' AND chungtu.LOAI_CHUNGTU ='''
                 || LOAI_CHUNGTU
                 || ''' '
                 || P_EXPRESSION
                 || ' GROUP BY '
                 || P_COLUMNS_GROUPBY
                 || ',mathang.BARCODE
    ORDER BY '
                 || P_COLUMNS_GROUPBY
                 || ' ) a';

--    DBMS_OUTPUT.PUT_LINE(QUERY_STR);
    OPEN CUR FOR QUERY_STR;

EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('<your message>' || SQLERRM);
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('<your message>' || SQLERRM);
END NHAPMUA_CHITIET;