create or replace PROCEDURE "NHAPMUA_TONGHOP" (
    LOAI_CHUNGTU    IN              VARCHAR2,
    DIEUKIEN_NHOM   IN              VARCHAR2,
    MAKHO           IN              VARCHAR2,
    MALOAI          IN              VARCHAR2,
    MANHOM          IN              VARCHAR2,
    MAHANG          IN              VARCHAR2,
    MANHACUNGCAP    IN              VARCHAR2,
    UNITCODE        IN              VARCHAR2,
    MATHUE          IN              VARCHAR2,
    MANHANVIEN      IN              VARCHAR2,
    TUNGAY          IN              DATE,
    DENNGAY         IN              DATE,
    CUR             OUT             SYS_REFCURSOR
) AS

    P_EXPRESSION               VARCHAR(3000);
    QUERY_STR                  VARCHAR(3000);
    P_TABLE_GROUPBY            VARCHAR(2000);
    P_COLUMNS_GROUPBY          VARCHAR(2000);
    P_SELECT_COLUMNS_GROUPBY   VARCHAR(2000);
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
            P_COLUMNS_GROUPBY := 'chungtu.MAKHO_NHAP, kho.TENKHO';
            P_SELECT_COLUMNS_GROUPBY := ' NVL(A.MAKHO, ''NULL'') AS MACHA, NVL(a.TENKHO, ''NULL'') AS TENCHA ';
            P_TABLE_GROUPBY := ' INNER JOIN KHOHANG kho ON chungtu.MAKHO_NHAP = kho.MAKHO AND kho.UNITCODE = '''
                               || UNITCODE
                               || '''';
            P_SELECT2 := 'chungtu.MAKHO_NHAP AS MAKHO,kho.TENKHO AS TENKHO';
            P_SELECT := ' ';
        END;
    ELSIF DIEUKIEN_NHOM = 'NHOMHANG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.MANHOM,nhomhang.TENNHOM';
            P_SELECT_COLUMNS_GROUPBY := ' NVL(A.MANHOM, ''NULL'')  AS MACHA, NVL(A.TENNHOM, ''NULL'') AS TENCHA ';
            P_TABLE_GROUPBY := ' INNER JOIN NHOMHANG nhomhang on mathang.MANHOM = nhomhang.MANHOM AND nhomhang.UNITCODE = '''
                               || UNITCODE
                               || '''';
            P_SELECT2 := 'mathang.MANHOM AS MANHOM,nhomhang.TENNHOM AS TENNHOM';
            P_SELECT := ' ';
        END;
    ELSIF DIEUKIEN_NHOM = 'LOAIHANG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.MALOAI,loaihang.TENLOAI';
            P_SELECT_COLUMNS_GROUPBY := ' NVL(A.MALOAI, ''NULL'')  AS MACHA, NVL(A.TENLOAI, ''NULL'') AS TENCHA ';
            P_TABLE_GROUPBY := ' INNER JOIN LOAIHANG loaihang ON mathang.MALOAI = loaihang.MALOAI AND loaihang.UNITCODE = '''
                               || UNITCODE
                               || '''';
            P_SELECT2 := ' mathang.MALOAI AS MALOAI,loaihang.TENLOAI AS TENLOAI';
            P_SELECT := ' ';
        END;
    ELSIF DIEUKIEN_NHOM = 'NHACUNGCAP' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'chungtu.MANHACUNGCAP , nhacungcap.TENNHACUNGCAP';
            P_SELECT_COLUMNS_GROUPBY := ' NVL(A.MANHACUNGCAP, ''NULL'') AS MACHA, NVL(A.TENNHACUNGCAP, ''NULL'') AS TENCHA ';
            P_TABLE_GROUPBY := ' INNER JOIN NHACUNGCAP nhacungcap on chungtu.MANHACUNGCAP = nhacungcap.MANHACUNGCAP  AND nhacungcap.UNITCODE = '''
                               || UNITCODE
                               || '''';
            P_SELECT2 := ' chungtu.MANHACUNGCAP AS MANHACUNGCAP,nhacungcap.TENNHACUNGCAP AS TENNHACUNGCAP';
            P_SELECT := ' ';
        END;
    ELSIF DIEUKIEN_NHOM = 'NGUOIDUNG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'chungtu.I_CREATE_BY, nguoidung.TENNHANVIEN';
            P_SELECT_COLUMNS_GROUPBY := ' NVL(a.NGUOITAO, ''NULL'')  AS MACHA, NVL(a.TENNGUOITAO, ''NULL'') AS TENCHA ';
            P_TABLE_GROUPBY := ' INNER JOIN NGUOIDUNG nguoidung ON chungtu.I_CREATE_BY = nguoidung.USERNAME AND nguoidung.UNITCODE = '''
                               || UNITCODE
                               || '''';
            P_SELECT2 := ' chungtu.I_CREATE_BY AS NGUOITAO, nguoidung.TENNHANVIEN AS TENNGUOITAO ';
        END;
    ELSIF DIEUKIEN_NHOM = 'CHUNGTU' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'chungtu.MA_CHUNGTU , chungtu.DIENGIAI';
            P_SELECT_COLUMNS_GROUPBY := ' NVL(A.MA_CHUNGTU, ''NULL'')  AS MACHA, NVL(A.DIENGIAI, ''NULL'') AS TENCHA ';
            P_TABLE_GROUPBY := ' AND chungtu.UNITCODE = '''
                               || UNITCODE
                               || '''';
            P_SELECT2 := ' chungtu.MA_CHUNGTU AS MA_CHUNGTU,chungtu.DIENGIAI AS DIENGIAI';
            P_SELECT := ' ';
        END;
    ELSIF DIEUKIEN_NHOM = 'MAHANG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'chungtu_chitiet.MAHANG,mathang.TENHANG,mathang_gia.GIAMUA,mathang_gia.GIABANLE_VAT';
            P_SELECT_COLUMNS_GROUPBY := ' NVL(A.MAHANG, ''NULL'')  AS MACHA, NVL(A.TENHANG, ''NULL'') AS TENCHA,A.MAHANG AS MACON ,A.TENHANG AS TENCON '
            ;
            P_TABLE_GROUPBY := ' AND mathang.UNITCODE = '''
                               || UNITCODE
                               || ''' ';
            P_SELECT2 := ' chungtu_chitiet.MAHANG AS MAHANG,mathang.TENHANG AS TENHANG';
            P_SELECT := ' ';
        END;
    ELSIF DIEUKIEN_NHOM = 'MATHUE' THEN
        BEGIN
            P_COLUMNS_GROUPBY := ' thue.MATHUE ,thue.TENTHUE';
            P_SELECT_COLUMNS_GROUPBY := 'A.MACHA , A.TENCHA';
            P_TABLE_GROUPBY := ' ';
            P_SELECT2 := 'thue.MATHUE AS MACHA , thue.TENTHUE AS TENCHA';
            P_SELECT := '';
        END;
    END IF;

    QUERY_STR := 'SELECT (a.SOLUONG) AS SOLUONG, (a.TIENHANG) AS TIENHANG, (a.TIENVAT) AS TIENVAT, (a.TIEN_CHIETKHAU) AS TIEN_CHIETKHAU, (a.TIENHANG - a.TIEN_CHIETKHAU + a.TIENVAT) AS TONGTIEN ,'
                 || P_SELECT_COLUMNS_GROUPBY
                 || '
		FROM (
		SELECT   '
                 || P_SELECT2
                 || ',SUM(chungtu_chitiet.SOLUONG) AS SOLUONG, 
				  SUM(chungtu_chitiet.SOLUONG * ROUND(NVL(chungtu_chitiet.GIAMUA,0),2)) AS TIENHANG '
                 || P_SELECT
                 || ' ,
				  SUM( ROUND(NVL(chungtu_chitiet.GIAMUA,0),2) * ROUND(NVL(thue.GIATRI / 100,0),2) * ROUND(chungtu_chitiet.SOLUONG,2) ) AS TIENVAT, 
				  SUM(ROUND(NVL(chungtu_chitiet.TIEN_GIAMGIA, 0), 2)) TIEN_CHIETKHAU  '
                 || P_SELECT
                 || ' 
		FROM CHUNGTU chungtu INNER JOIN CHUNGTU_CHITIET chungtu_chitiet ON chungtu.MA_CHUNGTU = chungtu_chitiet.MA_CHUNGTU 
		INNER JOIN THUE thue ON thue.MATHUE = chungtu_chitiet.MATHUE_VAO
		INNER JOIN MATHANG mathang ON mathang.MAHANG = chungtu_chitiet.MAHANG INNER JOIN MATHANG_GIA mathang_gia ON mathang.MAHANG = mathang_gia.MAHANG
        AND mathang.UNITCODE = '''
                 || UNITCODE
                 || '''
		'
                 || P_TABLE_GROUPBY
                 || '
		WHERE chungtu.TRANGTHAI = 10
			AND TO_DATE(chungtu.NGAY_DUYETPHIEU,''DD/MM/YY'') <= TO_DATE('''
                 || DENNGAY
                 || ''',''DD/MM/YY'')
			AND TO_DATE(chungtu.NGAY_DUYETPHIEU,''DD/MM/YY'') >= TO_DATE('''
                 || TUNGAY
                 || ''',''DD/MM/YY'') AND chungtu.UNITCODE = '''
                 || UNITCODE
                 || ''' AND chungtu.LOAI_CHUNGTU = '''
                 || LOAI_CHUNGTU
                 || ''' '
                 || P_EXPRESSION
                 || ' GROUP BY '
                 || P_COLUMNS_GROUPBY
                 || ' ORDER BY '
                 || P_COLUMNS_GROUPBY
                 || ') a';

--    DBMS_OUTPUT.PUT_LINE(QUERY_STR);
    OPEN CUR FOR QUERY_STR;

EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('<your message>' || SQLERRM);
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE(SQLERRM);
END NHAPMUA_TONGHOP;
--------------------------------------------------------
--  DDL for Procedure NHAPMUATONGHOP
--------------------------------------------------------