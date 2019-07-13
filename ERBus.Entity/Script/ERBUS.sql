--------------------------------------------------------
--  File created - Saturday-July-13-2019   
--------------------------------------------------------
--------------------------------------------------------
--  DDL for Procedure BANLE_TIMKIEM_BOHANG_MAHANG
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."BANLE_TIMKIEM_BOHANG_MAHANG" 
(
  P_MADONVI IN VARCHAR2 ,
  P_TUKHOA IN VARCHAR2,
  P_SUDUNG_TIMKIEM_ALL IN NUMBER,
  P_DIEUKIENCHON IN NUMBER,
  CURSOR_RESULT OUT SYS_REFCURSOR
) AS 
  QUERY_SELECT VARCHAR2(3000);
  T_LOAITIMKIEM VARCHAR2(20) := '';
  TEXT_IS_NUMBER NUMBER(18,2) := 0;
  IS_CONTAIN_UNITCODE VARCHAR2(2):='';
BEGIN
  SELECT IS_NUMBER(P_TUKHOA) INTO TEXT_IS_NUMBER FROM DUAL;
  BEGIN 
      SELECT * INTO IS_CONTAIN_UNITCODE FROM DUAL WHERE REGEXP_REPLACE(P_TUKHOA, '[^ -~]', '@') LIKE '%@%';
      EXCEPTION WHEN NO_DATA_FOUND
      THEN IS_CONTAIN_UNITCODE := '';
  END;
  IF P_SUDUNG_TIMKIEM_ALL = 1 THEN
      IF LENGTH(P_TUKHOA) = 13 AND TEXT_IS_NUMBER = 1 AND (IS_CONTAIN_UNITCODE IS NULL OR IS_CONTAIN_UNITCODE = '') 
        THEN T_LOAITIMKIEM := 'BARCODE';
      END IF;
      IF LENGTH(P_TUKHOA) = 7 AND SUBSTR(P_TUKHOA,0,2) <> 'BH' AND TEXT_IS_NUMBER = 0 AND (IS_CONTAIN_UNITCODE IS NULL OR IS_CONTAIN_UNITCODE = '') 
        THEN T_LOAITIMKIEM := 'MAHANG';
      END IF;
      IF IS_CONTAIN_UNITCODE = 'X' 
        THEN T_LOAITIMKIEM := 'TENHANG';
      END IF; 
      IF LENGTH(P_TUKHOA) = 4 AND TEXT_IS_NUMBER = 1 AND (IS_CONTAIN_UNITCODE IS NULL OR IS_CONTAIN_UNITCODE = '') 
        THEN T_LOAITIMKIEM := 'MACANDIENTU';
      END IF; 
      IF SUBSTR(P_TUKHOA,0,2) = 'BH' 
        THEN T_LOAITIMKIEM := 'BOHANG';
      END IF;
  ELSE
      IF P_DIEUKIENCHON = 0
        THEN T_LOAITIMKIEM := 'MAHANG';
      END IF;
      IF P_DIEUKIENCHON = 1
        THEN T_LOAITIMKIEM := 'BARCODE';
      END IF;
      IF P_DIEUKIENCHON = 2
        THEN T_LOAITIMKIEM := 'MACANDIENTU';
      END IF; 
      IF P_DIEUKIENCHON = 3
        THEN T_LOAITIMKIEM := 'TENHANG';
      END IF; 
      IF P_DIEUKIENCHON = 4
        THEN T_LOAITIMKIEM := 'BOHANG';
      END IF; 
      IF P_DIEUKIENCHON = 5
        THEN T_LOAITIMKIEM := 'MAHANGTRONGBO';
      END IF;
      IF P_DIEUKIENCHON = 6
        THEN T_LOAITIMKIEM := 'GIABANLE_VAT';
      END IF; 
  END IF;
IF P_SUDUNG_TIMKIEM_ALL = 1 THEN
    IF T_LOAITIMKIEM = 'BARCODE' THEN
        QUERY_SELECT := 'SELECT a.MAHANG,a.MAHANG AS MACON,a.TENHANG,a.MALOAI,a.MANHOM,
                        d.TENDONVITINH AS DONVITINH,a.MANHACUNGCAP,c.TENNHACUNGCAP,
                        b.GIABANLE_VAT,a.ITEMCODE,a.BARCODE
                        FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG
                        INNER JOIN NHACUNGCAP c ON a.MANHACUNGCAP = c.MANHACUNGCAP
                        INNER JOIN DONVITINH d ON a.MADONVITINH = d.MADONVITINH
                        WHERE a.UNITCODE = '''||P_MADONVI||''' AND a.BARCODE LIKE ''%'||P_TUKHOA||'%'' AND ROWNUM < 201';
        ELSIF T_LOAITIMKIEM = 'MAHANG' THEN
        QUERY_SELECT := 'SELECT a.MAHANG,a.MAHANG AS MACON,a.TENHANG,a.MALOAI,a.MANHOM,
                        d.TENDONVITINH AS DONVITINH,a.MANHACUNGCAP,c.TENNHACUNGCAP,
                        b.GIABANLE_VAT,a.ITEMCODE,a.BARCODE
                        FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG
                        INNER JOIN NHACUNGCAP c ON a.MANHACUNGCAP = c.MANHACUNGCAP
                        INNER JOIN DONVITINH d ON a.MADONVITINH = d.MADONVITINH
                        WHERE a.UNITCODE = '''||P_MADONVI||''' AND a.MAHANG LIKE ''%'||P_TUKHOA||'%'' AND ROWNUM < 201';
        ELSIF T_LOAITIMKIEM = 'TENHANG' THEN
        QUERY_SELECT := 'SELECT a.MAHANG,a.MAHANG AS MACON,a.TENHANG,a.MALOAI,a.MANHOM,
                        d.TENDONVITINH AS DONVITINH,a.MANHACUNGCAP,c.TENNHACUNGCAP,
                        b.GIABANLE_VAT,a.ITEMCODE,a.BARCODE
                        FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG
                        INNER JOIN NHACUNGCAP c ON a.MANHACUNGCAP = c.MANHACUNGCAP
                        INNER JOIN DONVITINH d ON a.MADONVITINH = d.MADONVITINH
                        WHERE a.UNITCODE = '''||P_MADONVI||''' AND UPPER(a.TENHANG) LIKE N''%'||UPPER(P_TUKHOA)||'%'' AND ROWNUM < 201';
        ELSIF T_LOAITIMKIEM = 'MACANDIENTU' THEN
        QUERY_SELECT := 'SELECT a.MAHANG,a.MAHANG AS MACON,a.TENHANG,a.MALOAI,a.MANHOM,
                        d.TENDONVITINH AS DONVITINH,a.MANHACUNGCAP,c.TENNHACUNGCAP,
                        b.GIABANLE_VAT,a.ITEMCODE,a.BARCODE
                        FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG
                        INNER JOIN NHACUNGCAP c ON a.MANHACUNGCAP = c.MANHACUNGCAP
                        INNER JOIN DONVITINH d ON a.MADONVITINH = d.MADONVITINH
                        WHERE a.UNITCODE = '''||P_MADONVI||''' AND a.ITEMCODE = '''||P_TUKHOA||''' AND ROWNUM < 201';
        ELSIF T_LOAITIMKIEM = 'BOHANG' THEN
        QUERY_SELECT := 'SELECT
                        A.MABOHANG       AS MAHANG,
                        B.MAHANG         AS MACON,
                        C.TENHANG        AS TENHANG,
                        C.MALOAI,
                        C.MANHOM,
                        ''Bó'' AS DONVITINH,
                        C.MANHACUNGCAP   AS MANHACUNGCAP,
                        E.TENNHACUNGCAP,
                        D.GIABANLE_VAT,
                        C.ITEMCODE,
                        C.BARCODE
                    FROM
                        BOHANG A
                        INNER JOIN BOHANG_CHITIET B ON A.MABOHANG = B.MABOHANG
                        INNER JOIN MATHANG C ON B.MAHANG = C.MAHANG
                        INNER JOIN MATHANG_GIA D ON B.MAHANG = C.MAHANG
                        INNER JOIN NHACUNGCAP E ON C.MANHACUNGCAP = E.MANHACUNGCAP AND c.UNITCODE = '''||P_MADONVI||''' AND a.MABOHANG LIKE ''%'||P_TUKHOA||'%'' AND ROWNUM < 201';
        ELSIF T_LOAITIMKIEM = 'MAHANGTRONGBO' THEN
        QUERY_SELECT := 'SELECT
                        A.MABOHANG       AS MAHANG,
                        B.MAHANG         AS MACON,
                        C.TENHANG        AS TENHANG,
                        C.MALOAI,
                        C.MANHOM,
                        ''Bó'' AS DONVITINH,
                        C.MANHACUNGCAP   AS MANHACUNGCAP,
                        E.TENNHACUNGCAP,
                        D.GIABANLE_VAT,
                        C.ITEMCODE,
                        C.BARCODE
                    FROM
                        BOHANG A
                        INNER JOIN BOHANG_CHITIET B ON A.MABOHANG = B.MABOHANG
                        INNER JOIN MATHANG C ON B.MAHANG = C.MAHANG
                        INNER JOIN MATHANG_GIA D ON B.MAHANG = C.MAHANG
                        INNER JOIN NHACUNGCAP E ON C.MANHACUNGCAP = E.MANHACUNGCAP 
                        AND c.UNITCODE = '''||P_MADONVI||''' AND b.MAHANG LIKE ''%'||P_TUKHOA||'%'' AND ROWNUM < 201';
        ELSIF T_LOAITIMKIEM = 'GIABANLE_VAT' THEN
        QUERY_SELECT := 'SELECT a.MAHANG,a.MAHANG AS MACON,a.TENHANG,a.MALOAI,a.MANHOM,
                        d.TENDONVITINH AS DONVITINH,a.MANHACUNGCAP,c.TENNHACUNGCAP,
                        b.GIABANLE_VAT,a.ITEMCODE,a.BARCODE
                        FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG
                        INNER JOIN NHACUNGCAP c ON a.MANHACUNGCAP = c.MANHACUNGCAP
                        INNER JOIN DONVITINH d ON a.MADONVITINH = d.MADONVITINH
                        WHERE a.UNITCODE = '''||P_MADONVI||''' AND b.GIABANLE_VAT = '||P_TUKHOA||' AND ROWNUM < 201';
        ELSE 
        QUERY_SELECT := 'SELECT a.MAHANG,a.MAHANG AS MACON,a.TENHANG,a.MALOAI,a.MANHOM,
                        d.TENDONVITINH AS DONVITINH,a.MANHACUNGCAP,c.TENNHACUNGCAP,
                        b.GIABANLE_VAT,a.ITEMCODE,a.BARCODE
                        FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG
                        INNER JOIN NHACUNGCAP c ON a.MANHACUNGCAP = c.MANHACUNGCAP
                        INNER JOIN DONVITINH d ON a.MADONVITINH = d.MADONVITINH
                        WHERE a.UNITCODE = '''||P_MADONVI||''' AND a.MAHANG LIKE ''%'||P_TUKHOA||'%'' AND ROWNUM < 201';
    END IF;
ELSE
    IF T_LOAITIMKIEM = 'BARCODE' THEN
        QUERY_SELECT := 'SELECT a.MAHANG,a.MAHANG AS MACON,a.TENHANG,a.MALOAI,a.MANHOM,
                        d.TENDONVITINH AS DONVITINH,a.MANHACUNGCAP,c.TENNHACUNGCAP,
                        b.GIABANLE_VAT,a.ITEMCODE,a.BARCODE
                        FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG
                        INNER JOIN NHACUNGCAP c ON a.MANHACUNGCAP = c.MANHACUNGCAP
                        INNER JOIN DONVITINH d ON a.MADONVITINH = d.MADONVITINH
                        WHERE a.UNITCODE = '''||P_MADONVI||''' AND a.BARCODE LIKE ''%'||P_TUKHOA||'%'' AND ROWNUM < 201';
        ELSIF T_LOAITIMKIEM = 'MAHANG' THEN
        QUERY_SELECT := 'SELECT a.MAHANG,a.MAHANG AS MACON,a.TENHANG,a.MALOAI,a.MANHOM,
                        d.TENDONVITINH AS DONVITINH,a.MANHACUNGCAP,c.TENNHACUNGCAP,
                        b.GIABANLE_VAT,a.ITEMCODE,a.BARCODE
                        FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG
                        INNER JOIN NHACUNGCAP c ON a.MANHACUNGCAP = c.MANHACUNGCAP
                        INNER JOIN DONVITINH d ON a.MADONVITINH = d.MADONVITINH
                        WHERE a.UNITCODE = '''||P_MADONVI||''' AND a.MAHANG LIKE ''%'||P_TUKHOA||'%'' AND ROWNUM < 201';
        ELSIF T_LOAITIMKIEM = 'TENHANG' THEN
        QUERY_SELECT := 'SELECT a.MAHANG,a.MAHANG AS MACON,a.TENHANG,a.MALOAI,a.MANHOM,
                        d.TENDONVITINH AS DONVITINH,a.MANHACUNGCAP,c.TENNHACUNGCAP,
                        b.GIABANLE_VAT,a.ITEMCODE,a.BARCODE
                        FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG
                        INNER JOIN NHACUNGCAP c ON a.MANHACUNGCAP = c.MANHACUNGCAP
                        INNER JOIN DONVITINH d ON a.MADONVITINH = d.MADONVITINH
                        WHERE a.UNITCODE = '''||P_MADONVI||''' AND UPPER(a.TENHANG) LIKE N''%'||UPPER(P_TUKHOA)||'%'' AND ROWNUM < 201';
        ELSIF T_LOAITIMKIEM = 'MACANDIENTU' THEN
        QUERY_SELECT := 'SELECT a.MAHANG,a.MAHANG AS MACON,a.TENHANG,a.MALOAI,a.MANHOM,
                        d.TENDONVITINH AS DONVITINH,a.MANHACUNGCAP,c.TENNHACUNGCAP,
                        b.GIABANLE_VAT,a.ITEMCODE,a.BARCODE
                        FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG
                        INNER JOIN NHACUNGCAP c ON a.MANHACUNGCAP = c.MANHACUNGCAP
                        INNER JOIN DONVITINH d ON a.MADONVITINH = d.MADONVITINH
                        WHERE a.UNITCODE = '''||P_MADONVI||''' AND a.ITEMCODE = '''||P_TUKHOA||''' AND ROWNUM < 201';
        ELSIF T_LOAITIMKIEM = 'BOHANG' THEN
        QUERY_SELECT := 'SELECT
                        A.MABOHANG       AS MAHANG,
                        B.MAHANG         AS MACON,
                        C.TENHANG        AS TENHANG,
                        C.MALOAI,
                        C.MANHOM,
                        ''Bó'' AS DONVITINH,
                        C.MANHACUNGCAP   AS MANHACUNGCAP,
                        E.TENNHACUNGCAP,
                        D.GIABANLE_VAT,
                        C.ITEMCODE,
                        C.BARCODE
                    FROM
                        BOHANG A
                        INNER JOIN BOHANG_CHITIET B ON A.MABOHANG = B.MABOHANG
                        INNER JOIN MATHANG C ON B.MAHANG = C.MAHANG
                        INNER JOIN MATHANG_GIA D ON B.MAHANG = C.MAHANG
                        INNER JOIN NHACUNGCAP E ON C.MANHACUNGCAP = E.MANHACUNGCAP AND c.UNITCODE = '''||P_MADONVI||''' AND a.MABOHANG LIKE ''%'||P_TUKHOA||'%'' AND ROWNUM < 201';
        ELSIF T_LOAITIMKIEM = 'MAHANGTRONGBO' THEN
        QUERY_SELECT := 'SELECT
                        A.MABOHANG       AS MAHANG,
                        B.MAHANG         AS MACON,
                        C.TENHANG        AS TENHANG,
                        C.MALOAI,
                        C.MANHOM,
                        ''Bó'' AS DONVITINH,
                        C.MANHACUNGCAP   AS MANHACUNGCAP,
                        E.TENNHACUNGCAP,
                        D.GIABANLE_VAT,
                        C.ITEMCODE,
                        C.BARCODE
                    FROM
                        BOHANG A
                        INNER JOIN BOHANG_CHITIET B ON A.MABOHANG = B.MABOHANG
                        INNER JOIN MATHANG C ON B.MAHANG = C.MAHANG
                        INNER JOIN MATHANG_GIA D ON B.MAHANG = C.MAHANG
                        INNER JOIN NHACUNGCAP E ON C.MANHACUNGCAP = E.MANHACUNGCAP 
                        AND c.UNITCODE = '''||P_MADONVI||''' AND b.MAHANG LIKE ''%'||P_TUKHOA||'%'' AND ROWNUM < 201';
        ELSIF T_LOAITIMKIEM = 'GIABANLE_VAT' THEN
        QUERY_SELECT := 'SELECT a.MAHANG,a.MAHANG AS MACON,a.TENHANG,a.MALOAI,a.MANHOM,
                        d.TENDONVITINH AS DONVITINH,a.MANHACUNGCAP,c.TENNHACUNGCAP,
                        b.GIABANLE_VAT,a.ITEMCODE,a.BARCODE
                        FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG
                        INNER JOIN NHACUNGCAP c ON a.MANHACUNGCAP = c.MANHACUNGCAP
                        INNER JOIN DONVITINH d ON a.MADONVITINH = d.MADONVITINH
                        WHERE a.UNITCODE = '''||P_MADONVI||''' AND b.GIABANLE_VAT = '||P_TUKHOA||' AND ROWNUM < 201';
        ELSE 
        QUERY_SELECT := 'SELECT a.MAHANG,a.MAHANG AS MACON,a.TENHANG,a.MALOAI,a.MANHOM,
                        d.TENDONVITINH AS DONVITINH,a.MANHACUNGCAP,c.TENNHACUNGCAP,
                        b.GIABANLE_VAT,a.ITEMCODE,a.BARCODE
                        FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG
                        INNER JOIN NHACUNGCAP c ON a.MANHACUNGCAP = c.MANHACUNGCAP
                        INNER JOIN DONVITINH d ON a.MADONVITINH = d.MADONVITINH
                        WHERE a.UNITCODE = '''||P_MADONVI||''' AND a.MAHANG LIKE ''%'||P_TUKHOA||'%'' AND ROWNUM < 201';
    END IF;
END IF;
    --DBMS_OUTPUT.PUT_LINE('QUERY_SELECT:'||QUERY_SELECT);
  BEGIN
  OPEN CURSOR_RESULT FOR QUERY_SELECT;
    EXCEPTION
    WHEN NO_DATA_FOUND THEN
     DBMS_OUTPUT.put_line ('NO_DATA_FOUND');  
       WHEN OTHERS THEN
     DBMS_OUTPUT.put_line (SQLERRM);  
  END;
END BANLE_TIMKIEM_BOHANG_MAHANG;




/
--------------------------------------------------------
--  DDL for Procedure BAOCAO_BANBUON_CHITIET
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."BAOCAO_BANBUON_CHITIET" (
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



/
--------------------------------------------------------
--  DDL for Procedure BAOCAO_BANBUON_TONGHOP
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."BAOCAO_BANBUON_TONGHOP" (
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
    P_SELECT_COLUMNS    VARCHAR(2000);
    P_TABLE_GROUPBY     VARCHAR(2000);
    P_COLUMNS_GROUPBY   VARCHAR(2000);
    P_EXPRESSION        VARCHAR(2000) := '';
    P_CREATE_TABLE      VARCHAR2(1000);
    P_TRUNCATE_TABLE    VARCHAR2(200);
    N_COUNT             NUMBER(10, 0) := 0;
BEGIN
    P_TRUNCATE_TABLE := 'DELETE TEMP_BANBUON_TONGHOP WHERE USERNAME = '''
                        || USERNAME
                        || '''';
    P_CREATE_TABLE := 'CREATE GLOBAL TEMPORARY TABLE "ERBUS"."TEMP_BANBUON_TONGHOP" 
   (
    "MA" VARCHAR2(70),
    "TEN" NVARCHAR2(200), 
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
        TABLE_NAME = 'TEMP_BANBUON_TONGHOP';

    IF N_COUNT = 0 THEN
        EXECUTE IMMEDIATE P_CREATE_TABLE;
    END IF;
    EXECUTE IMMEDIATE P_TRUNCATE_TABLE;
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
            P_COLUMNS_GROUPBY := 'chungtu.MAKHO_XUAT ,khohang.TENKHO, chungtu.UNITCODE,xnt.GIAVON,thue.GIATRI,chungtu_chitiet.GIABANLE,chungtu_chitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := ' chungtu.MAKHO_XUAT AS MA, khohang.TENKHO AS TEN, chungtu.UNITCODE ';
            P_TABLE_GROUPBY := ' INNER JOIN KHOHANG khohang ON chungtu.MAKHO_XUAT = khohang.MAKHO AND chungtu.UNITCODE = khohang.UNITCODE'
            ;
        END;
    ELSIF DIEUKIEN_NHOM = 'NHOMHANG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.MANHOM, nhomhang.TENNHOM, chungtu.UNITCODE, xnt.GIAVON,thue.GIATRI,chungtu_chitiet.GIABANLE,chungtu_chitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := ' mathang.MANHOM AS MA, nhomhang.TENNHOM AS TEN, chungtu.UNITCODE ';
            P_TABLE_GROUPBY := ' INNER JOIN NHOMHANG nhomhang ON mathang.MANHOM = nhomhang.MANHOM AND chungtu.UNITCODE = nhomhang.UNITCODE';
        END;
    ELSIF DIEUKIEN_NHOM = 'LOAIHANG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.MALOAI, loaihang.TENLOAI, chungtu.UNITCODE, xnt.GIAVON,thue.GIATRI,chungtu_chitiet.GIABANLE,chungtu_chitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := 'mathang.MALOAI AS MA, loaihang.TENLOAI AS TEN, chungtu.UNITCODE ';
            P_TABLE_GROUPBY := ' INNER JOIN LOAIHANG loaihang ON mathang.MALOAI = loaihang.MALOAI AND chungtu.UNITCODE = loaihang.UNITCODE'
            ;
        END;
    ELSIF DIEUKIEN_NHOM = 'NHACUNGCAP' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.MANHACUNGCAP, nhacungcap.TENNHACUNGCAP, chungtu.UNITCODE, xnt.GIAVON,thue.GIATRI,chungtu_chitiet.GIABANLE,chungtu_chitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := ' mathang.MANHACUNGCAP AS MA, nhacungcap.TENNHACUNGCAP AS TEN, chungtu.UNITCODE ';
            P_TABLE_GROUPBY := ' INNER JOIN NHACUNGCAP nhacungcap ON mathang.MANHACUNGCAP = nhacungcap.MANHACUNGCAP AND chungtu.UNITCODE = nhacungcap.UNITCODE';
        END;
    ELSIF DIEUKIEN_NHOM = 'KHACHHANG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'chungtu.MAKHACHHANG, khachhang.TENKHACHHANG, chungtu.UNITCODE, xnt.GIAVON,thue.GIATRI,chungtu_chitiet.GIABANLE,chungtu_chitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := ' chungtu.MAKHACHHANG AS MA ,khachhang.TENKHACHHANG AS TEN, chungtu.UNITCODE ';
            P_TABLE_GROUPBY := ' INNER JOIN KHACHHANG khachhang ON chungtu.MAKHACHHANG = khachhang.MAKHACHHANG AND chungtu.UNITCODE = khachhang.UNITCODE';
        END;
    ELSIF DIEUKIEN_NHOM = 'NGUOIDUNG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'chungtu.I_CREATE_BY, nguoidung.TENNHANVIEN, chungtu.UNITCODE, xnt.GIAVON,thue.GIATRI,chungtu_chitiet.GIABANLE,chungtu_chitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := ' chungtu.I_CREATE_BY AS MA, nguoidung.TENNHANVIEN AS TEN, chungtu.UNITCODE ';
            P_TABLE_GROUPBY := ' INNER JOIN NGUOIDUNG nguoidung ON chungtu.I_CREATE_BY = nguoidung.USERNAME AND chungtu.UNITCODE = nguoidung.UNITCODE';
        END;
    ELSIF DIEUKIEN_NHOM = 'GIAODICH' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'chungtu.MA_CHUNGTU, chungtu.DIENGIAI,chungtu.UNITCODE, xnt.GIAVON,thue.GIATRI,chungtu_chitiet.GIABANLE,chungtu_chitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := ' chungtu.MA_CHUNGTU AS MA ,chungtu.DIENGIAI AS TEN, chungtu.UNITCODE';
            P_TABLE_GROUPBY := ' ';
        END;
    ELSE
        BEGIN
            P_COLUMNS_GROUPBY := 'chungtu_chitiet.MAHANG, mathang.TENHANG, chungtu.UNITCODE, xnt.GIAVON,thue.GIATRI,chungtu_chitiet.GIABANLE,chungtu_chitiet.GIABANLE_VAT ';
            P_SELECT_COLUMNS := ' chungtu_chitiet.MAHANG AS MA, mathang.TENHANG AS TEN, chungtu.UNITCODE ';
            P_TABLE_GROUPBY := '  ';
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
        QUERRY_INSERT   VARCHAR2(2500) := '';
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

            QUERRY_INSERT := 'INSERT INTO TEMP_BANBUON_TONGHOP(MA,TEN,UNITCODE,SOLUONG,GIAVON,GIATRI_THUE_RA,
                                                                TIEN_GIAMGIA,GIABANLE,GIABANLE_VAT,USERNAME)
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
        QUERY_STR := 'SELECT MA,TEN,SUM(SOLUONG) AS SOLUONG,SUM(GIAVON) AS GIAVON,SUM(VON) AS VON, SUM(VON_VAT) AS VON_VAT,SUM(TIEN_GIAMGIA) AS TIEN_GIAMGIA,
                      SUM(DOANHTHU) AS DOANHTHU,SUM(TIENTHUE) AS TIENTHUE,SUM(LAIBANLE) AS LAIBANLE,SUM(DOANHTHU + TIENTHUE) AS TONGBAN
        FROM
        (
        SELECT MA,TEN,SOLUONG,
        ROUND(GIAVON, 2) AS GIAVON,
        ROUND(GIAVON * SOLUONG, 2) AS VON,
        ROUND(GIAVON * (1 + (GIATRI_THUE_RA / 100)) * SOLUONG, 2) AS VON_VAT,
        ROUND(TIEN_GIAMGIA, 2) AS TIEN_GIAMGIA,
        ROUND((NVL(SOLUONG,0) * NVL(GIABANLE_VAT,0))/(1 + (GIATRI_THUE_RA / 100)), 2) AS DOANHTHU,
        ROUND(SOLUONG * (GIATRI_THUE_RA / 100) * (GIABANLE_VAT / (1 + (GIATRI_THUE_RA / 100))), 2) AS TIENTHUE,
        ROUND(SOLUONG * (GIABANLE_VAT /(1 + (GIATRI_THUE_RA / 100))) - ROUND(GIAVON * SOLUONG, 2) ,2) AS LAIBANLE,
        ROUND(GIABANLE_VAT, 2) AS GIABANLE_VAT
        FROM TEMP_BANBUON_TONGHOP WHERE USERNAME = '''
                     || USERNAME
                     || '''
        ) GROUP BY MA,TEN ';
        --DBMS_OUTPUT.PUT_LINE(QUERY_STR);
        OPEN CUR FOR QUERY_STR;

    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            NULL;
        WHEN OTHERS THEN
            NULL;
    END;

END BAOCAO_BANBUON_TONGHOP;



/
--------------------------------------------------------
--  DDL for Procedure BAOCAO_XBANLE_CHITIET
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."BAOCAO_XBANLE_CHITIET" (
    DIEUKIEN_NHOM   IN              VARCHAR2,
    MAKHO           IN              VARCHAR2,
    MALOAI          IN              VARCHAR2,
    MANHOM          IN              VARCHAR2,
    MAHANG          IN              VARCHAR2,
    MANHACUNGCAP    IN              VARCHAR2,
    MANHANVIEN      IN              VARCHAR2,
    MA_GIAODICH     IN              VARCHAR2,
    USERNAME        IN              VARCHAR2,
    UNITCODE        IN              VARCHAR2,
    TUNGAY          IN              DATE,
    DENNGAY         IN              DATE,
    CUR             OUT             SYS_REFCURSOR
) IS

    QUERY_STR                      VARCHAR(5000) := '';
    P_SELECT_COLUMNS               VARCHAR(3000) := '';
    P_SELECT_COLUMNS_DATPHONG      VARCHAR(3000) := '';
    P_TABLE_GROUPBY                VARCHAR(3000) := '';
    P_TABLE_GROUPBY_DATPHONG       VARCHAR(3000) := '';
    P_COLUMNS_GROUPBY              VARCHAR(3000) := '';
    P_COLUMNS_GROUPBY_DATPHONG     VARCHAR(3000) := '';
    P_EXPRESSION                   VARCHAR(3000) := '';
    P_EXPRESSION_DATPHONG          VARCHAR(3000) := '';
    P_CREATE_TABLE                 VARCHAR2(1000);
    P_TRUNCATE_TABLE               VARCHAR2(200);
    N_COUNT                        NUMBER(10, 0) := 0;
    P_THOIGIAN_GIOHAT              VARCHAR2(200) := '';
    T_MAHANG_GIOHAT                VARCHAR2(50) := '';
    T_SOPHUT_GIOHAT                NUMBER(18,2) := 0;
BEGIN
   P_TRUNCATE_TABLE := 'DELETE "ERBUS"."TEMP_XBANLE_CHITIET" WHERE USERNAME = '''
                        || USERNAME
                        || '''';
    P_CREATE_TABLE := 'CREATE GLOBAL TEMPORARY TABLE "ERBUS"."TEMP_XBANLE_CHITIET" 
   (
    "MACHA" VARCHAR2(70),
    "TENCHA" NVARCHAR2(200),
    "BARCODE" VARCHAR2(2000), 
    "MA" VARCHAR2(70),
    "TEN" NVARCHAR2(200),
    "NGAY_GIAODICH" DATE,
    "GROUP_CODE" VARCHAR2(70), 
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
        TABLE_NAME = 'TEMP_XBANLE_CHITIET';

    IF N_COUNT = 0 THEN
        EXECUTE IMMEDIATE P_CREATE_TABLE;
    END IF;
    EXECUTE IMMEDIATE P_TRUNCATE_TABLE;
    COMMIT;
    -- LAY THONG TIN GIO HAT NEU CO
    P_THOIGIAN_GIOHAT := 'SELECT MAHANG,SOPHUT FROM CAUHINH_LOAIPHONG WHERE UNITCODE = '''||UNITCODE||''' ';
    EXECUTE IMMEDIATE P_THOIGIAN_GIOHAT INTO T_MAHANG_GIOHAT,T_SOPHUT_GIOHAT;
    -- END LAY THONG TIN GIO HAT
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


    IF TRIM(MA_GIAODICH) IS NOT NULL THEN
        P_EXPRESSION := P_EXPRESSION
                        || ' AND giaodich.MA_GIAODICH IN (SELECT REGEXP_SUBSTR('''
                        || MA_GIAODICH
                        || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                        || MA_GIAODICH
                        || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
    END IF;


    IF DIEUKIEN_NHOM = 'KHOHANG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'giaodich.MAKHO_XUAT, khohang.TENKHO,mathang.BARCODE, giaodich_chitiet.MAHANG, mathang.TENHANG, giaodich.NGAY_GIAODICH, giaodich.MAKHO_XUAT, giaodich.UNITCODE, thue.GIATRI,xnt.GIAVON,giaodich_chitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := 'giaodich.MAKHO_XUAT AS MACHA, khohang.TENKHO AS TENCHA, mathang.BARCODE AS BARCODE, giaodich_chitiet.MAHANG AS MA, mathang.TENHANG AS TEN, giaodich.NGAY_GIAODICH AS NGAY_GIAODICH , giaodich.MAKHO_XUAT AS GROUP_CODE, giaodich.UNITCODE';
            P_TABLE_GROUPBY := ' INNER JOIN KHOHANG khohang ON khohang.MAKHO = giaodich.MAKHO_XUAT AND giaodich.UNITCODE = khohang.UNITCODE ';
            
            P_COLUMNS_GROUPBY_DATPHONG := 'thanhtoan.MAKHO, khohang.TENKHO,mathang.BARCODE, thanhtoanchitiet.MAHANG, mathang.TENHANG, thanhtoan.NGAY_THANHTOAN, thanhtoan.MAKHO, thanhtoan.UNITCODE, thue.GIATRI,xnt.GIAVON,thanhtoanchitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS_DATPHONG := 'thanhtoan.MAKHO AS MACHA, khohang.TENKHO AS TENCHA, mathang.BARCODE AS BARCODE, thanhtoanchitiet.MAHANG AS MA, mathang.TENHANG AS TEN, thanhtoan.NGAY_THANHTOAN AS NGAY_THANHTOAN , thanhtoan.MAKHO AS GROUP_CODE, thanhtoan.UNITCODE';
            P_TABLE_GROUPBY_DATPHONG := ' INNER JOIN KHOHANG khohang ON khohang.MAKHO = thanhtoan.MAKHO AND thanhtoan.UNITCODE = khohang.UNITCODE ';
        END;
    ELSIF DIEUKIEN_NHOM = 'NHOMHANG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.MANHOM, nhomhang.TENNHOM, mathang.BARCODE, giaodich_chitiet.MAHANG, mathang.TENHANG, mathang.MANHOM,giaodich.NGAY_GIAODICH, giaodich.UNITCODE, thue.GIATRI,xnt.GIAVON,giaodich_chitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := 'mathang.MANHOM AS MACHA, nhomhang.TENNHOM AS TENCHA,mathang.BARCODE AS BARCODE, giaodich_chitiet.MAHANG AS MA, mathang.TENHANG AS TEN , giaodich.NGAY_GIAODICH AS NGAY_GIAODICH, mathang.MANHOM AS GROUP_CODE, giaodich.UNITCODE';
            P_TABLE_GROUPBY := ' INNER JOIN NHOMHANG nhomhang ON nhomhang.MANHOM = mathang.MANHOM AND giaodich.UNITCODE = nhomhang.UNITCODE ';
            
            P_COLUMNS_GROUPBY_DATPHONG := 'mathang.MANHOM, nhomhang.TENNHOM, mathang.BARCODE, thanhtoanchitiet.MAHANG, mathang.TENHANG, mathang.MANHOM,thanhtoan.NGAY_THANHTOAN, thanhtoan.UNITCODE, thue.GIATRI,xnt.GIAVON,thanhtoanchitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS_DATPHONG := 'mathang.MANHOM AS MACHA, nhomhang.TENNHOM AS TENCHA,mathang.BARCODE AS BARCODE, thanhtoanchitiet.MAHANG AS MA, mathang.TENHANG AS TEN , thanhtoan.NGAY_THANHTOAN AS NGAY_THANHTOAN, mathang.MANHOM AS GROUP_CODE, thanhtoan.UNITCODE';
            P_TABLE_GROUPBY_DATPHONG := ' INNER JOIN NHOMHANG nhomhang ON nhomhang.MANHOM = mathang.MANHOM AND thanhtoan.UNITCODE = nhomhang.UNITCODE ';
        END;
    ELSIF DIEUKIEN_NHOM = 'LOAIHANG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.MALOAI, loaihang.TENLOAI, mathang.BARCODE, giaodich_chitiet.MAHANG, mathang.TENHANG, mathang.MALOAI,giaodich.NGAY_GIAODICH, giaodich.UNITCODE, thue.GIATRI,xnt.GIAVON,giaodich_chitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := 'mathang.MALOAI AS MACHA, loaihang.TENLOAI AS TENCHA, mathang.BARCODE AS BARCODE, giaodich_chitiet.MAHANG AS MA, mathang.TENHANG AS TEN,giaodich.NGAY_GIAODICH AS NGAY_GIAODICH,mathang.MALOAI AS GROUP_CODE, giaodich.UNITCODE';
            P_TABLE_GROUPBY := ' INNER JOIN LOAIHANG loaihang ON loaihang.MALOAI = mathang.MALOAI AND giaodich.UNITCODE = loaihang.UNITCODE ';
            
            P_COLUMNS_GROUPBY_DATPHONG := 'mathang.MALOAI, loaihang.TENLOAI, mathang.BARCODE, thanhtoanchitiet.MAHANG, mathang.TENHANG, mathang.MALOAI,thanhtoan.NGAY_THANHTOAN, thanhtoan.UNITCODE, thue.GIATRI,xnt.GIAVON,thanhtoanchitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS_DATPHONG := 'mathang.MALOAI AS MACHA, loaihang.TENLOAI AS TENCHA, mathang.BARCODE AS BARCODE, thanhtoanchitiet.MAHANG AS MA, mathang.TENHANG AS TEN,thanhtoan.NGAY_THANHTOAN AS NGAY_THANHTOAN,mathang.MALOAI AS GROUP_CODE, thanhtoan.UNITCODE';
            P_TABLE_GROUPBY_DATPHONG := ' INNER JOIN LOAIHANG loaihang ON loaihang.MALOAI = mathang.MALOAI AND thanhtoan.UNITCODE = loaihang.UNITCODE ';
        END;
    ELSIF DIEUKIEN_NHOM = 'NHACUNGCAP' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.MANHACUNGCAP, nhacungcap.TENNHACUNGCAP, mathang.BARCODE, giaodich_chitiet.MAHANG, mathang.TENHANG, mathang.MANHACUNGCAP,giaodich.NGAY_GIAODICH, giaodich.UNITCODE, thue.GIATRI,xnt.GIAVON,giaodich_chitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := 'mathang.MANHACUNGCAP AS MACHA, nhacungcap.TENNHACUNGCAP AS TENCHA, mathang.BARCODE AS BARCODE, giaodich_chitiet.MAHANG AS MA, mathang.TENHANG AS TEN, giaodich.NGAY_GIAODICH AS NGAY_GIAODICH, mathang.MANHACUNGCAP AS GROUP_CODE, giaodich.UNITCODE';
            P_TABLE_GROUPBY := ' INNER JOIN NHACUNGCAP nhacungcap ON nhacungcap.MANHACUNGCAP = mathang.MANHACUNGCAP AND giaodich.UNITCODE = nhacungcap.UNITCODE ';
            
            P_COLUMNS_GROUPBY_DATPHONG := 'mathang.MANHACUNGCAP, nhacungcap.TENNHACUNGCAP, mathang.BARCODE, thanhtoanchitiet.MAHANG, mathang.TENHANG, mathang.MANHACUNGCAP,thanhtoan.NGAY_THANHTOAN, thanhtoan.UNITCODE, thue.GIATRI,xnt.GIAVON,thanhtoanchitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS_DATPHONG := 'mathang.MANHACUNGCAP AS MACHA, nhacungcap.TENNHACUNGCAP AS TENCHA, mathang.BARCODE AS BARCODE, thanhtoanchitiet.MAHANG AS MA, mathang.TENHANG AS TEN, thanhtoan.NGAY_THANHTOAN AS NGAY_THANHTOAN, mathang.MANHACUNGCAP AS GROUP_CODE, thanhtoan.UNITCODE';
            P_TABLE_GROUPBY_DATPHONG := ' INNER JOIN NHACUNGCAP nhacungcap ON nhacungcap.MANHACUNGCAP = mathang.MANHACUNGCAP AND thanhtoan.UNITCODE = nhacungcap.UNITCODE ';
        END;
    ELSIF DIEUKIEN_NHOM = 'GIAODICH' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.TENHANG, mathang.BARCODE, giaodich_chitiet.MAHANG, giaodich.MA_GIAODICH, giaodich.NGAY_GIAODICH, giaodich.LOAI_GIAODICH, giaodich.UNITCODE, thue.GIATRI,xnt.GIAVON,giaodich_chitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := 'giaodich.MA_GIAODICH AS MACHA, giaodich.LOAI_GIAODICH AS TENCHA, mathang.BARCODE AS BARCODE, giaodich_chitiet.MAHANG AS MA, mathang.TENHANG AS TEN ,giaodich.NGAY_GIAODICH, giaodich.MA_GIAODICH AS GROUP_CODE, giaodich.UNITCODE';
            P_TABLE_GROUPBY := ' ';
            
            P_COLUMNS_GROUPBY_DATPHONG := 'mathang.TENHANG, mathang.BARCODE, thanhtoanchitiet.MAHANG, thanhtoan.MA_DATPHONG, thanhtoan.NGAY_THANHTOAN, thanhtoan.MA_DATPHONG, thanhtoan.UNITCODE, thue.GIATRI, xnt.GIAVON, thanhtoanchitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS_DATPHONG := 'thanhtoan.MA_DATPHONG AS MACHA, thanhtoan.MA_DATPHONG AS TENCHA, mathang.BARCODE AS BARCODE, thanhtoanchitiet.MAHANG AS MA, mathang.TENHANG AS TEN ,thanhtoan.NGAY_THANHTOAN, thanhtoan.MA_DATPHONG AS GROUP_CODE, thanhtoan.UNITCODE';
            P_TABLE_GROUPBY_DATPHONG := ' ';
        END;
    ELSIF DIEUKIEN_NHOM = 'NGUOIDUNG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.BARCODE,nguoidung.TENNHANVIEN, giaodich_chitiet.MAHANG, mathang.TENHANG, giaodich.I_CREATE_BY, giaodich.NGAY_GIAODICH, giaodich.UNITCODE, thue.GIATRI,xnt.GIAVON,giaodich_chitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := 'giaodich.I_CREATE_BY AS MACHA, nguoidung.TENNHANVIEN AS TENCHA, mathang.BARCODE AS BARCODE, giaodich_chitiet.MAHANG AS MA, mathang.TENHANG AS TEN, giaodich.NGAY_GIAODICH AS NGAY_GIAODICH, giaodich.I_CREATE_BY AS GROUP_CODE, giaodich.UNITCODE';
            P_TABLE_GROUPBY := ' INNER JOIN NGUOIDUNG nguoidung ON giaodich.I_CREATE_BY = nguoidung.MANHANVIEN AND giaodich.UNITCODE= nguoidung.UNITCODE ';
            
            P_COLUMNS_GROUPBY_DATPHONG := 'mathang.BARCODE,nguoidung.TENNHANVIEN, thanhtoanchitiet.MAHANG, mathang.TENHANG, thanhtoan.I_CREATE_BY, thanhtoan.NGAY_THANHTOAN, thanhtoan.UNITCODE, thue.GIATRI, xnt.GIAVON, thanhtoanchitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS_DATPHONG := 'thanhtoan.I_CREATE_BY AS MACHA, nguoidung.TENNHANVIEN AS TENCHA, mathang.BARCODE AS BARCODE, thanhtoanchitiet.MAHANG AS MA, mathang.TENHANG AS TEN, thanhtoan.NGAY_THANHTOAN AS NGAY_THANHTOAN, thanhtoan.I_CREATE_BY AS GROUP_CODE, thanhtoan.UNITCODE';
            P_TABLE_GROUPBY_DATPHONG := ' INNER JOIN NGUOIDUNG nguoidung ON thanhtoan.I_CREATE_BY = nguoidung.USERNAME AND thanhtoan.UNITCODE= nguoidung.UNITCODE ';
        END;
    ELSE
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.BARCODE, mathang.MAHANG, mathang.TENHANG, giaodich.NGAY_GIAODICH, giaodich.UNITCODE, thue.GIATRI,xnt.GIAVON,giaodich_chitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := 'mathang.MAHANG AS MACHA, mathang.TENHANG AS TENCHA, mathang.BARCODE AS BARCODE,mathang.MAHANG AS MA, mathang.TENHANG AS TEN , giaodich.NGAY_GIAODICH AS NGAY_GIAODICH,mathang.MAHANG AS GROUP_CODE, giaodich.UNITCODE AS UNITCODE';
            P_TABLE_GROUPBY := ' ';
            
            P_COLUMNS_GROUPBY_DATPHONG := 'mathang.BARCODE, mathang.MAHANG, mathang.TENHANG, thanhtoan.NGAY_THANHTOAN, thanhtoan.UNITCODE, thue.GIATRI,xnt.GIAVON,thanhtoanchitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS_DATPHONG := 'mathang.MAHANG AS MACHA, mathang.TENHANG AS TENCHA, mathang.BARCODE AS BARCODE, mathang.MAHANG AS MA, mathang.TENHANG AS TEN , thanhtoan.NGAY_THANHTOAN AS NGAY_THANHTOAN, mathang.MAHANG AS GROUP_CODE, thanhtoan.UNITCODE AS UNITCODE';
            P_TABLE_GROUPBY_DATPHONG := ' ';
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
        QUERRY_INSERT   VARCHAR2(6000) := '';
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

            QUERRY_INSERT := 'INSERT INTO TEMP_XBANLE_CHITIET(MACHA, TENCHA, BARCODE, MA, TEN, NGAY_GIAODICH, GROUP_CODE, UNITCODE,SOLUONG,GIAVON,GIATRI_THUE_RA,
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
                             
        UNION
            SELECT '
                                 || P_SELECT_COLUMNS_DATPHONG
                                 || '
                                  ,SUM(NVL(thanhtoanchitiet.SOLUONG,0)) AS SOLUONG
                                  ,NVL(xnt.GIAVON, 0) AS GIAVON
                                  ,NVL(thue.GIATRI, 0) AS GIATRI_THUE_RA
                                  ,0 AS TIENTHE_VIP
                                  ,0 AS TIEN_CHIETKHAU
                                  ,0 AS TIEN_KHUYENMAI
                                  ,0 AS TIEN_VOUCHER
                                  ,CASE TO_CHAR(thanhtoanchitiet.MAHANG)
                                    WHEN TO_CHAR('''||T_MAHANG_GIOHAT||''') THEN ROUND((THANHTOANCHITIET.GIABANLE_VAT / '||T_SOPHUT_GIOHAT||'), 2)
                                    ELSE NVL(thanhtoanchitiet.GIABANLE_VAT,0)
                                    END AS GIABANLE_VAT
                                  ,'''
                                 || USERNAME
                                 || ''' AS USERNAME
        FROM THANHTOAN_DATPHONG thanhtoan 
        INNER JOIN THANHTOAN_DATPHONG_CHITIET thanhtoanchitiet ON thanhtoan.MA_DATPHONG = thanhtoanchitiet.MA_DATPHONG 
        INNER JOIN MATHANG mathang ON mathang.MAHANG = thanhtoanchitiet.MAHANG AND thanhtoan.UNITCODE= mathang.UNITCODE
        INNER JOIN '
                                 || TABLE_NAME
                                 || ' xnt ON xnt.MAHANG = thanhtoanchitiet.MAHANG AND xnt.MAKHO = thanhtoan.MAKHO
        INNER JOIN THUE thue ON mathang.MATHUE_RA = thue.MATHUE
        '
                                 || P_TABLE_GROUPBY_DATPHONG
                                 || '
        AND thanhtoan.UNITCODE = '''
                                 || UNITCODE
                                 || '''
            AND TO_DATE(thanhtoan.NGAY_THANHTOAN,''DD/MM/YY'') <= TO_DATE('''
                                 || END_DAY
                                 || ''',''DD/MM/YY'')
            AND TO_DATE(thanhtoan.NGAY_THANHTOAN,''DD/MM/YY'') >= TO_DATE('''
                                 || BEGIN_DAY
                                 || ''',''DD/MM/YY'') 
            '
                                 || P_EXPRESSION_DATPHONG
                                 || '
            GROUP BY '
                                 || P_COLUMNS_GROUPBY_DATPHONG
                                 || ' ,thanhtoanchitiet.MAHANG
        ';
   --DBMS_OUTPUT.PUT_LINE(QUERRY_INSERT);

            EXECUTE IMMEDIATE QUERRY_INSERT;
        END LOOP;
        CLOSE CUR_RESULT;
    END;
  BEGIN
        QUERY_STR := 'SELECT MACHA, TENCHA, BARCODE, MACON, TENCON, NGAY_GIAODICH, GROUP_CODE, 
        SOLUONG,GIAVON,VON,VON_VAT,TIEN_CHIETKHAU,TIEN_KHUYENMAI,TIENTHE_VIP,DOANHTHU,
        TIENTHUE,((DOANHTHU + TIENTHUE) - VON_VAT) AS LAIBANLE,DOANHTHU + TIENTHUE AS TONGBAN,GIABANLE_VAT
        FROM
        (
        SELECT MACHA, TENCHA, BARCODE, MA AS MACON, TEN AS TENCON, NGAY_GIAODICH, GROUP_CODE, SOLUONG, GIAVON,
        ROUND(GIAVON * SOLUONG, 2) AS VON,
        GIAVON * (1 + (GIATRI_THUE_RA / 100)) * SOLUONG AS VON_VAT,
        TIEN_CHIETKHAU,TIEN_KHUYENMAI,TIENTHE_VIP,
        SOLUONG * (GIABANLE_VAT / (1 + GIATRI_THUE_RA / 100)) - (TIEN_CHIETKHAU + TIEN_KHUYENMAI + TIENTHE_VIP) AS DOANHTHU,
        SOLUONG * (GIATRI_THUE_RA / 100) * (GIABANLE_VAT / (1 + (GIATRI_THUE_RA / 100))) AS TIENTHUE,
        GIABANLE_VAT
        FROM TEMP_XBANLE_CHITIET WHERE USERNAME = '''
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

END BAOCAO_XBANLE_CHITIET;

/
--------------------------------------------------------
--  DDL for Procedure BAOCAO_XBANLE_TONGHOP
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."BAOCAO_XBANLE_TONGHOP" (
    DIEUKIEN_NHOM   IN              VARCHAR2,
    MAKHO           IN              VARCHAR2,
    MALOAI          IN              VARCHAR2,
    MANHOM          IN              VARCHAR2,
    MAHANG          IN              VARCHAR2,
    MANHACUNGCAP    IN              VARCHAR2,
    MA_GIAODICH     IN              VARCHAR2,
    UNITCODE        IN              VARCHAR2,
    TUNGAY          IN              DATE,
    DENNGAY         IN              DATE,
    MANHANVIEN      IN              VARCHAR2,
    USERNAME        IN              VARCHAR2,
    CUR             OUT             SYS_REFCURSOR
) AS

    QUERY_STR                      VARCHAR(5000) := '';
    P_SELECT_COLUMNS               VARCHAR(3000) := '';
    P_SELECT_COLUMNS_DATPHONG      VARCHAR(3000) := '';
    P_TABLE_GROUPBY                VARCHAR(3000) := '';
    P_TABLE_GROUPBY_DATPHONG       VARCHAR(3000) := '';
    P_COLUMNS_GROUPBY              VARCHAR(3000) := '';
    P_COLUMNS_GROUPBY_DATPHONG     VARCHAR(3000) := '';
    P_EXPRESSION                   VARCHAR(3000) := '';
    P_EXPRESSION_DATPHONG          VARCHAR(3000) := '';
    P_CREATE_TABLE                 VARCHAR2(1000);
    P_TRUNCATE_TABLE               VARCHAR2(200);
    N_COUNT                        NUMBER(10, 0) := 0;
    P_THOIGIAN_GIOHAT              VARCHAR2(200) := '';
    T_MAHANG_GIOHAT                VARCHAR2(50) := '';
    T_SOPHUT_GIOHAT                NUMBER(18,2) := 0;
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
    -- LAY THONG TIN GIO HAT NEU CO
    P_THOIGIAN_GIOHAT := 'SELECT MAHANG,SOPHUT FROM CAUHINH_LOAIPHONG WHERE UNITCODE = '''||UNITCODE||''' ';
    EXECUTE IMMEDIATE P_THOIGIAN_GIOHAT INTO T_MAHANG_GIOHAT,T_SOPHUT_GIOHAT;
    -- END LAY THONG TIN GIO HAT
    IF TRIM(MAKHO) IS NOT NULL THEN
        P_EXPRESSION := P_EXPRESSION
                        || ' AND giaodich.MAKHO_XUAT IN (SELECT REGEXP_SUBSTR('''
                        || MAKHO
                        || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                        || MAKHO
                        || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
        P_EXPRESSION_DATPHONG := P_EXPRESSION_DATPHONG
                        || ' AND thanhtoan.MAKHO IN (SELECT REGEXP_SUBSTR('''
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
                        
        P_EXPRESSION_DATPHONG := P_EXPRESSION_DATPHONG
                        || ' AND thanhtoan.I_CREATE_BY IN (SELECT REGEXP_SUBSTR('''
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
        P_EXPRESSION_DATPHONG := P_EXPRESSION_DATPHONG
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
        P_EXPRESSION_DATPHONG := P_EXPRESSION_DATPHONG
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
        P_EXPRESSION_DATPHONG := P_EXPRESSION_DATPHONG
                        || ' AND thanhtoanchitiet.MAHANG IN (SELECT REGEXP_SUBSTR('''
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
        P_EXPRESSION_DATPHONG := P_EXPRESSION_DATPHONG
                        || ' AND mathang.MANHACUNGCAP IN (SELECT REGEXP_SUBSTR('''
                        || MANHACUNGCAP
                        || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                        || MANHACUNGCAP
                        || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';                        
    END IF;

    IF TRIM(MA_GIAODICH) IS NOT NULL THEN
        P_EXPRESSION := P_EXPRESSION
                        || ' AND giaodich.MA_GIAODICH IN (SELECT REGEXP_SUBSTR('''
                        || MA_GIAODICH
                        || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                        || MA_GIAODICH
                        || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
        P_EXPRESSION_DATPHONG := P_EXPRESSION_DATPHONG
                        || ' AND thanhtoan.MA_DATPHONG IN (SELECT REGEXP_SUBSTR('''
                        || MA_GIAODICH
                        || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                        || MA_GIAODICH
                        || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
    END IF;

    IF DIEUKIEN_NHOM = 'KHOHANG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'giaodich.MAKHO_XUAT,khohang.TENKHO, giaodich.UNITCODE, thue.GIATRI,xnt.GIAVON,giaodich_chitiet.GIABANLE_VAT';
            P_COLUMNS_GROUPBY_DATPHONG := 'thanhtoan.MAKHO,khohang.TENKHO, thanhtoan.UNITCODE, thue.GIATRI,xnt.GIAVON,thanhtoanchitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := ' giaodich.MAKHO_XUAT AS MA,khohang.TENKHO AS TEN,giaodich.UNITCODE ';
            P_SELECT_COLUMNS_DATPHONG := ' thanhtoan.MAKHO AS MA,khohang.TENKHO AS TEN,thanhtoan.UNITCODE ';
            P_TABLE_GROUPBY := ' INNER JOIN KHOHANG khohang ON giaodich.MAKHO_XUAT = khohang.MAKHO AND khohang.UNITCODE = giaodich.UNITCODE';
            P_TABLE_GROUPBY_DATPHONG := ' INNER JOIN KHOHANG khohang ON thanhtoan.MAKHO = khohang.MAKHO AND khohang.UNITCODE = thanhtoan.UNITCODE';
        END;
    ELSIF DIEUKIEN_NHOM = 'LOAIHANG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.MALOAI, loaihang.TENLOAI, giaodich.UNITCODE, thue.GIATRI,xnt.GIAVON,giaodich_chitiet.GIABANLE_VAT';
            P_COLUMNS_GROUPBY_DATPHONG := 'mathang.MALOAI, loaihang.TENLOAI, thanhtoan.UNITCODE, thue.GIATRI,xnt.GIAVON,thanhtoanchitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := ' mathang.MALOAI AS MA,loaihang.TENLOAI AS TEN, giaodich.UNITCODE ';
            P_SELECT_COLUMNS_DATPHONG := ' mathang.MALOAI AS MA,loaihang.TENLOAI AS TEN, thanhtoan.UNITCODE ';
            P_TABLE_GROUPBY := ' INNER JOIN LOAIHANG loaihang ON mathang.MALOAI = loaihang.MALOAI AND loaihang.UNITCODE = giaodich.UNITCODE';
            P_TABLE_GROUPBY_DATPHONG := ' INNER JOIN LOAIHANG loaihang ON mathang.MALOAI = loaihang.MALOAI AND loaihang.UNITCODE = thanhtoan.UNITCODE';
        END;
    ELSIF DIEUKIEN_NHOM = 'NHOMHANG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.MANHOM, nhomhang.TENNHOM, giaodich.UNITCODE, thue.GIATRI,xnt.GIAVON,giaodich_chitiet.GIABANLE_VAT';
            P_COLUMNS_GROUPBY_DATPHONG := 'mathang.MANHOM, nhomhang.TENNHOM, thanhtoan.UNITCODE, thue.GIATRI,xnt.GIAVON,thanhtoanchitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := ' mathang.MANHOM AS MA,nhomhang.TENNHOM AS TEN, giaodich.UNITCODE ';
            P_SELECT_COLUMNS_DATPHONG := ' mathang.MANHOM AS MA,nhomhang.TENNHOM AS TEN, thanhtoan.UNITCODE ';
            P_TABLE_GROUPBY := ' INNER JOIN NHOMHANG nhomhang ON mathang.MANHOM = nhomhang.MANHOM AND nhomhang.UNITCODE = giaodich.UNITCODE';
            P_TABLE_GROUPBY_DATPHONG := ' INNER JOIN NHOMHANG nhomhang ON mathang.MANHOM = nhomhang.MANHOM AND nhomhang.UNITCODE = thanhtoan.UNITCODE';
        END;
    ELSIF DIEUKIEN_NHOM = 'NHACUNGCAP' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.MANHACUNGCAP, nhacungcap.TENNHACUNGCAP, giaodich.UNITCODE, thue.GIATRI,xnt.GIAVON,giaodich_chitiet.GIABANLE_VAT';
            P_COLUMNS_GROUPBY_DATPHONG := 'mathang.MANHACUNGCAP, nhacungcap.TENNHACUNGCAP, thanhtoan.UNITCODE, thue.GIATRI,xnt.GIAVON,thanhtoanchitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := ' mathang.MANHACUNGCAP AS MA,nhacungcap.TENNHACUNGCAP AS TEN, giaodich.UNITCODE ';
            P_SELECT_COLUMNS_DATPHONG := ' mathang.MANHACUNGCAP AS MA,nhacungcap.TENNHACUNGCAP AS TEN, thanhtoan.UNITCODE ';
            P_TABLE_GROUPBY := '  INNER JOIN NHACUNGCAP nhacungcap ON mathang.MANHACUNGCAP = nhacungcap.MANHACUNGCAP AND nhacungcap.UNITCODE = giaodich.UNITCODE';
            P_TABLE_GROUPBY_DATPHONG := '  INNER JOIN NHACUNGCAP nhacungcap ON mathang.MANHACUNGCAP = nhacungcap.MANHACUNGCAP AND nhacungcap.UNITCODE = thanhtoan.UNITCODE';
        END;
    ELSIF DIEUKIEN_NHOM = 'GIAODICH' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'giaodich.MA_GIAODICH, giaodich.NGAY_GIAODICH, giaodich.UNITCODE, thue.GIATRI,xnt.GIAVON,giaodich_chitiet.GIABANLE_VAT';
            P_COLUMNS_GROUPBY_DATPHONG := 'thanhtoan.MA_DATPHONG, thanhtoan.NGAY_THANHTOAN, thanhtoan.UNITCODE, thue.GIATRI,xnt.GIAVON,thanhtoanchitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := 'giaodich.MA_GIAODICH AS MA, giaodich.NGAY_GIAODICH AS TEN, giaodich.UNITCODE AS UNITCODE ';
            P_SELECT_COLUMNS_DATPHONG := 'thanhtoan.MA_DATPHONG AS MA, thanhtoan.NGAY_THANHTOAN AS TEN, thanhtoan.UNITCODE AS UNITCODE ';
            P_TABLE_GROUPBY := ' ';
            P_TABLE_GROUPBY_DATPHONG := ' ';
        END;
    ELSIF DIEUKIEN_NHOM = 'NGUOIDUNG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'giaodich.I_CREATE_BY, nguoidung.TENNHANVIEN, giaodich.UNITCODE, thue.GIATRI, xnt.GIAVON,giaodich_chitiet.GIABANLE_VAT';
            P_COLUMNS_GROUPBY_DATPHONG := 'thanhtoan.I_CREATE_BY, nguoidung.TENNHANVIEN, thanhtoan.UNITCODE, thue.GIATRI, xnt.GIAVON,thanhtoanchitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := ' giaodich.I_CREATE_BY AS MA, nguoidung.TENNHANVIEN AS TEN, giaodich.UNITCODE ';
            P_SELECT_COLUMNS_DATPHONG := ' thanhtoan.I_CREATE_BY AS MA, nguoidung.TENNHANVIEN AS TEN, thanhtoan.UNITCODE ';
            P_TABLE_GROUPBY := ' INNER JOIN NGUOIDUNG nguoidung ON giaodich.I_CREATE_BY = nguoidung.USERNAME';
            P_TABLE_GROUPBY_DATPHONG := ' INNER JOIN NGUOIDUNG nguoidung ON thanhtoan.I_CREATE_BY = nguoidung.USERNAME';
        END;
    ELSE
        BEGIN
            P_COLUMNS_GROUPBY := 'giaodich_chitiet.MAHANG,mathang.TENHANG,giaodich.UNITCODE, thue.GIATRI,xnt.GIAVON,giaodich_chitiet.GIABANLE_VAT';
            P_COLUMNS_GROUPBY_DATPHONG := 'thanhtoanchitiet.MAHANG,mathang.TENHANG,thanhtoan.UNITCODE, thue.GIATRI,xnt.GIAVON,thanhtoanchitiet.GIABANLE_VAT';
            P_SELECT_COLUMNS := ' giaodich_chitiet.MAHANG AS MA, mathang.TENHANG AS TEN, giaodich.UNITCODE ';
            P_SELECT_COLUMNS_DATPHONG := ' thanhtoanchitiet.MAHANG AS MA, mathang.TENHANG AS TEN, thanhtoan.UNITCODE ';
            P_TABLE_GROUPBY := ' ';
            P_TABLE_GROUPBY_DATPHONG := ' ';
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
        QUERRY_INSERT   VARCHAR2(4000) := '';
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
        UNION
        SELECT '|| P_SELECT_COLUMNS_DATPHONG || '
        ,SUM(NVL(thanhtoanchitiet.SOLUONG,0)) AS SOLUONG
        ,NVL(xnt.GIAVON, 0) AS GIAVON
        ,NVL(thue.GIATRI, 0) AS GIATRI_THUE_RA
        ,0 AS TIENTHE_VIP
        ,0 AS TIEN_CHIETKHAU
        ,0 AS TIEN_KHUYENMAI
        ,0 AS TIEN_VOUCHER
        ,CASE TO_CHAR(thanhtoanchitiet.MAHANG)
        WHEN TO_CHAR('''||T_MAHANG_GIOHAT||''') THEN ROUND((THANHTOANCHITIET.GIABANLE_VAT / '||T_SOPHUT_GIOHAT||'), 2)
        ELSE NVL(thanhtoanchitiet.GIABANLE_VAT,0)
        END AS GIABANLE_VAT,
        '''||USERNAME||''' AS USERNAME
        FROM THANHTOAN_DATPHONG thanhtoan INNER JOIN THANHTOAN_DATPHONG_CHITIET thanhtoanchitiet 
        ON thanhtoan.MA_DATPHONG = thanhtoanchitiet.MA_DATPHONG
        INNER JOIN MATHANG mathang ON mathang.MAHANG = thanhtoanchitiet.MAHANG AND thanhtoan.UNITCODE = mathang.UNITCODE
        INNER JOIN '||TABLE_NAME||' xnt ON xnt.MAHANG = thanhtoanchitiet.MAHANG AND xnt.MAKHO = thanhtoan.MAKHO
        INNER JOIN THUE thue ON mathang.MATHUE_RA = thue.MATHUE AND mathang.UNITCODE = thue.UNITCODE
        '|| P_TABLE_GROUPBY_DATPHONG|| '
        AND thanhtoan.UNITCODE = '''||UNITCODE||'''
        AND TO_DATE(thanhtoan.NGAY_THANHTOAN,''DD/MM/YY'') <= TO_DATE('''
                             || END_DAY
                             || ''',''DD/MM/YY'')
        AND TO_DATE(thanhtoan.NGAY_THANHTOAN,''DD/MM/YY'') >= TO_DATE('''
                             || BEGIN_DAY
                             || ''',''DD/MM/YY'')
        '|| P_EXPRESSION_DATPHONG || ' GROUP BY '|| P_COLUMNS_GROUPBY_DATPHONG|| ' ,thanhtoanchitiet.MAHANG    ';
        --DBMS_OUTPUT.PUT_LINE(QUERRY_INSERT);

            EXECUTE IMMEDIATE QUERRY_INSERT;
        END LOOP;
        CLOSE CUR_RESULT;
    END;

    BEGIN
        QUERY_STR := 'SELECT MA,TEN,SOLUONG,GIAVON,VON,VON_VAT,TIEN_CHIETKHAU,TIEN_KHUYENMAI,
        TIENTHE_VIP,DOANHTHU,TIENTHUE,((DOANHTHU + TIENTHUE) - VON_VAT) AS LAIBANLE,
        DOANHTHU + TIENTHUE AS TONGBAN,GIABANLE_VAT
        FROM
        (
        SELECT MA,TEN,SOLUONG,GIAVON,
        ROUND(GIAVON * SOLUONG, 2) AS VON,
        GIAVON * (1 + (GIATRI_THUE_RA / 100)) * SOLUONG AS VON_VAT,
        TIEN_CHIETKHAU,TIEN_KHUYENMAI,TIENTHE_VIP,
        SOLUONG * (GIABANLE_VAT / (1 + GIATRI_THUE_RA / 100)) - (TIEN_CHIETKHAU + TIEN_KHUYENMAI + TIENTHE_VIP) AS DOANHTHU,
        SOLUONG * (GIATRI_THUE_RA / 100) * (GIABANLE_VAT / (1 + (GIATRI_THUE_RA / 100))) AS TIENTHUE,
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

/
--------------------------------------------------------
--  DDL for Procedure BAOCAO_XUATBANLE_TRONGNGAY
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."BAOCAO_XUATBANLE_TRONGNGAY" (
    TUNGAY     IN         DATE,
    DENNGAY    IN         DATE,
    UNITCODE   IN         NVARCHAR2,
    USERNAME   IN         VARCHAR2,
    CUR        OUT        SYS_REFCURSOR
) IS

    P_CREATE_TABLE                  VARCHAR2(1000);
    P_SQL_CLEAR                     VARCHAR2(2000);
    P_SQL_INSERT_BAN                VARCHAR2(3000);
    P_SQL_INSERT_BANBUON_QUAYHANG   VARCHAR2(2000);
    QUERY_STR                       VARCHAR2(2000);
    P_SQL_INSERT_TRA_LAI            VARCHAR2(2000);
    DK_GROUPBY                      VARCHAR2(2000) := '';
    N_COUNT                         NUMBER(10, 0) := 0;
    P_TRUNCATE_TABLE                VARCHAR2(200) := '';
    P_THOIGIAN_GIOHAT               VARCHAR2(200) := '';
    T_MAHANG_GIOHAT                 VARCHAR2(50) := '';
    T_SOPHUT_GIOHAT                 NUMBER(18,2) := 0;
BEGIN
    P_TRUNCATE_TABLE := 'DELETE "ERBUS"."TEMP_XBANLE_TONGHOP" WHERE USERNAME = '''
                        || USERNAME
                        || '''';
    P_CREATE_TABLE := 'CREATE GLOBAL TEMPORARY TABLE "ERBUS"."TEMP_XUATBANLE_TRONGNGAY" 
   ("UNITCODE" VARCHAR2(10), 
	"I_CREATE_BY" VARCHAR2(50), 
	"TENNHANVIEN" NVARCHAR2(200), 
	"TONGBAN" NUMBER(18,2) DEFAULT 0, 
	"TONGTRALAI" NUMBER(18,2) DEFAULT 0,
    "USERNAME" VARCHAR2(20),
	"SAPXEP" NUMBER(10,0) DEFAULT 0
   ) ON COMMIT PRESERVE ROWS'
    ;
    SELECT
        COUNT(TABLE_NAME)
    INTO N_COUNT
    FROM
        USER_TABLES
    WHERE
        TABLE_NAME = 'TEMP_XUATBANLE_TRONGNGAY';

    IF N_COUNT = 0 THEN
        EXECUTE IMMEDIATE P_CREATE_TABLE;
    END IF;
    EXECUTE IMMEDIATE P_TRUNCATE_TABLE;
    -- LAY THONG TIN GIO HAT NEU CO
    P_THOIGIAN_GIOHAT := 'SELECT MAHANG,SOPHUT FROM CAUHINH_LOAIPHONG WHERE UNITCODE = '''||UNITCODE||''' ';
    EXECUTE IMMEDIATE P_THOIGIAN_GIOHAT INTO T_MAHANG_GIOHAT,T_SOPHUT_GIOHAT;
    -- END LAY THONG TIN GIO HAT
    
    P_SQL_CLEAR := 'DELETE FROM TEMP_XUATBANLE_TRONGNGAY';
    P_SQL_INSERT_BAN := 'INSERT INTO TEMP_XUATBANLE_TRONGNGAY a(a.UNITCODE,a.I_CREATE_BY,a.TENNHANVIEN,a.TONGBAN,a.USERNAME,a.SAPXEP)
              (SELECT giaodich.UNITCODE AS UNITCODE,
                giaodich.I_CREATE_BY AS I_CREATE_BY,
                nguoidung.TENNHANVIEN AS TENNHANVIEN,
                SUM(THANHTIEN) AS TONGBAN,
                '''
                        || USERNAME
                        || ''' AS USERNAME,
                1 AS SAPXEP
                            FROM GIAODICH giaodich INNER JOIN GIAODICH_CHITIET giaodich_chitiet ON giaodich.MA_GIAODICH = giaodich_chitiet.MA_GIAODICH 
                            INNER JOIN NGUOIDUNG nguoidung ON giaodich.I_CREATE_BY = nguoidung.MANHANVIEN
                            WHERE giaodich.UNITCODE = '''
                        || UNITCODE
                        || ''' AND giaodich.LOAI_GIAODICH = ''XBAN_LE'' 
                            AND TO_DATE(giaodich.NGAY_GIAODICH,''DD-MM-YY'') >= TO_DATE('''
                        || TUNGAY
                        || ''',''DD-MM-YY'') 
                            AND TO_DATE(giaodich.NGAY_GIAODICH,''DD-MM-YY'') <= TO_DATE('''
                        || DENNGAY
                        || ''',''DD-MM-YY'')  '
                        || '
                            GROUP BY giaodich.UNITCODE,giaodich.I_CREATE_BY,nguoidung.TENNHANVIEN
                        UNION
                        SELECT UNITCODE,I_CREATE_BY,TENNHANVIEN,SUM(TONGBAN) AS TONGBAN, USERNAME,SAPXEP
                        FROM (
                            SELECT thanhtoan.UNITCODE,thanhtoan.I_CREATE_BY AS I_CREATE_BY,
                            nguoidung.TENNHANVIEN AS TENNHANVIEN,
                            CASE TO_CHAR(THANHTOANCHITIET.MAHANG)
                            WHEN TO_CHAR('''||T_MAHANG_GIOHAT||''') THEN SUM(ROUND((THANHTOANCHITIET.GIABANLE_VAT / '||T_SOPHUT_GIOHAT||') * THANHTOANCHITIET.SOLUONG, 2))
                            ELSE
                                SUM(ROUND(THANHTOANCHITIET.SOLUONG * THANHTOANCHITIET.GIABANLE_VAT, 2))
                            END AS TONGBAN,
                            ''' || USERNAME|| ''' AS USERNAME,
                            2 AS SAPXEP
                            FROM THANHTOAN_DATPHONG thanhtoan INNER JOIN THANHTOAN_DATPHONG_CHITIET thanhtoanchitiet
                            ON thanhtoan.MA_DATPHONG = thanhtoanchitiet.MA_DATPHONG AND thanhtoan.UNITCODE = thanhtoanchitiet.UNITCODE
                            INNER JOIN NGUOIDUNG nguoidung ON thanhtoan.I_CREATE_BY = nguoidung.USERNAME
                                AND thanhtoan.UNITCODE = nguoidung.UNITCODE
                                AND thanhtoan.UNITCODE = '''
                                     || UNITCODE
                                     || '''
                                     AND TO_DATE(thanhtoan.NGAY_THANHTOAN,''DD-MM-YY'') >= TO_DATE('''
                                     || TUNGAY
                                     || ''',''DD-MM-YY'') 
                                     AND TO_DATE(thanhtoan.NGAY_THANHTOAN,''DD-MM-YY'') <= TO_DATE('''
                                     || DENNGAY
                                     || ''',''DD-MM-YY'')  '
                                     || '
                                GROUP BY thanhtoan.UNITCODE,thanhtoan.I_CREATE_BY,nguoidung.TENNHANVIEN,THANHTOANCHITIET.MAHANG 
                                ) GROUP BY UNITCODE,I_CREATE_BY,TENNHANVIEN, USERNAME,SAPXEP )
                                ';

    P_SQL_INSERT_BANBUON_QUAYHANG := 'INSERT INTO TEMP_XUATBANLE_TRONGNGAY a(a.UNITCODE,a.I_CREATE_BY,a.TENNHANVIEN,a.TONGBAN,a.USERNAME,a.SAPXEP)
              (SELECT giaodich.UNITCODE AS UNITCODE,
                giaodich.I_CREATE_BY || ''-BB'' AS I_CREATE_BY,
                nguoidung.TENNHANVIEN AS TENNHANVIEN,
                SUM(THANHTIEN) AS TONGBAN,
                '''
                                     || USERNAME
                                     || ''' AS USERNAME,
                2 AS SAPXEP
                            FROM GIAODICH giaodich INNER JOIN GIAODICH_CHITIET giaodich_chitiet ON giaodich.MA_GIAODICH = giaodich_chitiet.MA_GIAODICH 
                            INNER JOIN NGUOIDUNG nguoidung ON giaodich.I_CREATE_BY = nguoidung.MANHANVIEN
                            WHERE giaodich.UNITCODE = '''
                                     || UNITCODE
                                     || ''' AND giaodich.LOAI_GIAODICH = ''BANBUON_QUAY'' 
                            AND TO_DATE(giaodich.NGAY_GIAODICH,''DD-MM-YY'') >= TO_DATE('''
                                     || TUNGAY
                                     || ''',''DD-MM-YY'') 
                            AND TO_DATE(giaodich.NGAY_GIAODICH,''DD-MM-YY'') <= TO_DATE('''
                                     || DENNGAY
                                     || ''',''DD-MM-YY'')  '
                                     || '
                             GROUP BY giaodich.UNITCODE,giaodich.I_CREATE_BY,nguoidung.TENNHANVIEN) 
                             ';

    P_SQL_INSERT_TRA_LAI := 'UPDATE  TEMP_XUATBANLE_TRONGNGAY a SET TONGTRALAI = NVL(( SELECT SUM(THANHTIEN) AS TONGTRALAI
                                    FROM GIAODICH giaodich INNER JOIN GIAODICH_CHITIET giaodich_chitiet ON giaodich.MA_GIAODICH = giaodich_chitiet.MA_GIAODICH 
                            INNER JOIN NGUOIDUNG nguoidung ON giaodich.I_CREATE_BY = nguoidung.MANHANVIEN
                            WHERE giaodich.UNITCODE = '''
                            || UNITCODE
                            || ''' AND giaodich.LOAI_GIAODICH = ''XBAN_TRALAI'' 
                                    AND TO_DATE(giaodich.NGAY_GIAODICH,''DD-MM-YY'') >= TO_DATE('''
                            || TUNGAY
                            || ''',''DD-MM-YY'') 
                                    AND TO_DATE(giaodich.NGAY_GIAODICH,''DD-MM-YY'') <= TO_DATE('''
                            || DENNGAY
                            || ''',''DD-MM-YY'')
                                    AND giaodich.I_CREATE_BY = a.I_CREATE_BY AND nguoidung.TENNHANVIEN = a.TENNHANVIEN
                                    AND a.USERNAME = '''
                            || USERNAME
                            || '''
                                    GROUP BY giaodich.UNITCODE,giaodich.I_CREATE_BY,nguoidung.TENNHANVIEN), 0)'
                            ;

    QUERY_STR := 'SELECT UNITCODE,USERNAME,I_CREATE_BY,TENNHANVIEN,TONGBAN,TONGTRALAI,SAPXEP FROM TEMP_XUATBANLE_TRONGNGAY WHERE USERNAME = '''
                 || USERNAME
                 || ''' ';
    BEGIN
        EXECUTE IMMEDIATE P_SQL_CLEAR;
        --DBMS_OUTPUT.PUT_LINE(P_SQL_INSERT_BAN);
        EXECUTE IMMEDIATE P_SQL_INSERT_BAN;
        EXECUTE IMMEDIATE P_SQL_INSERT_BANBUON_QUAYHANG;
        EXECUTE IMMEDIATE P_SQL_INSERT_TRA_LAI;
        OPEN CUR FOR QUERY_STR;

    END;

END BAOCAO_XUATBANLE_TRONGNGAY;

/
--------------------------------------------------------
--  DDL for Procedure GET_MENU
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."GET_MENU" (
    P_USERNAME   IN           VARCHAR2,
    P_UNITCODE   IN           VARCHAR2,
    CUR          OUT          SYS_REFCURSOR
) AS
    QUERY_STR   VARCHAR2(1500);
    QUERY_STR_CHECK   VARCHAR2(1500);
    N_COUNT NUMBER(10,0) := 0;
BEGIN
QUERY_STR := 'SELECT MA_MENU,TIEUDE,SAPXEP,MENU_CHA FROM
                (SELECT MA_MENU,TIEUDE,SAPXEP,MENU_CHA FROM (SELECT
                menu.MA_MENU,
                menu.TIEUDE,
                menu.SAPXEP,
                menu.MENU_CHA   
            FROM
                MENU menu
            WHERE
                menu.TRANGTHAI = 10
                AND menu.UNITCODE = '''||P_UNITCODE||'''
                AND menu.MA_MENU IN (
                    SELECT
                        MA_MENU
                    FROM
                        NGUOIDUNG_MENU
                    WHERE
                        USERNAME = '''||P_USERNAME||'''
                        AND UNITCODE = '''||P_UNITCODE||'''
                    UNION ALL
                    SELECT
                        C.MA_MENU
                    FROM
                        NGUOIDUNG_NHOMQUYEN B
                        INNER JOIN NHOMQUYEN_MENU C ON B.MANHOMQUYEN = C.MANHOMQUYEN
                    WHERE
                        B.USERNAME = '''||P_USERNAME||'''
                        AND B.UNITCODE = '''||P_UNITCODE||'''
                        AND C.UNITCODE = '''||P_UNITCODE||'''
                )
            ORDER BY
                menu.SAPXEP
            )
            UNION 
                SELECT
                menu.MA_MENU,
                menu.TIEUDE,
                menu.SAPXEP,
                menu.MENU_CHA FROM MENU menu WHERE menu.MENU_CHA IS NULL AND menu.TRANGTHAI = 10 AND menu.UNITCODE = '''||P_UNITCODE||''') ORDER BY SAPXEP'
                 ;
    QUERY_STR_CHECK := 'SELECT COUNT(*) FROM ('||QUERY_STR||')';
    EXECUTE IMMEDIATE QUERY_STR_CHECK INTO N_COUNT;
    --DBMS_OUTPUT.PUT_LINE(QUERY_STR);
    IF N_COUNT = 0 THEN
        QUERY_STR := 'SELECT
                menu.MA_MENU,
                menu.TIEUDE,
                menu.SAPXEP,
                menu.MENU_CHA FROM MENU menu WHERE menu.MENU_CHA IS NULL AND menu.TRANGTHAI = 10 ORDER BY SAPXEP';
    END IF;
    OPEN CUR FOR QUERY_STR;
  EXCEPTION
   WHEN NO_DATA_FOUND
   THEN
      DBMS_OUTPUT.put_line ('<your message>' || SQLERRM);
   WHEN OTHERS
   THEN
         DBMS_OUTPUT.put_line (QUERY_STR  || SQLERRM);   
END GET_MENU;

/
--------------------------------------------------------
--  DDL for Procedure KHOASO_MULTIPLE_PERIOD
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."KHOASO_MULTIPLE_PERIOD" (P_TUKY IN NUMBER,P_DENKY IN NUMBER)
AS
    T_TABLENAME_KYTRUOC VARCHAR2(50);
    T_TABLENAME_KYNAY VARCHAR2(50);
    PARAM_TUKY NUMBER(18,2);
    PARAM_DENKY NUMBER(18,2);
BEGIN
    IF P_TUKY IS NULL THEN PARAM_TUKY := 1; 
    ELSE PARAM_TUKY := P_TUKY;
    END IF;
    IF P_DENKY IS NULL THEN PARAM_DENKY := PARAM_DENKY; 
    ELSE PARAM_DENKY := P_DENKY;
    END IF;
    FOR KY IN PARAM_TUKY..PARAM_DENKY
    LOOP
       T_TABLENAME_KYTRUOC := 'XNT_2019_KY_'||TO_NUMBER(KY-1)||'';
       T_TABLENAME_KYNAY := 'XNT_2019_KY_'||KY||'';
          BEGIN
            XUATNHAPTON.XNT_KHOASO(T_TABLENAME_KYTRUOC,T_TABLENAME_KYNAY,'01',2019,KY);
			EXECUTE IMMEDIATE 'UPDATE KYKETOAN SET TRANGTHAI = 10 WHERE KYKETOAN = '||KY||'';
            COMMIT;
          END;
    END LOOP;
END KHOASO_MULTIPLE_PERIOD;

/
--------------------------------------------------------
--  DDL for Procedure KIEMTRA_TRUNGBARCODE_EXCEL
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."KIEMTRA_TRUNGBARCODE_EXCEL" 
(
  P_MADONVI IN VARCHAR2 ,
  P_BARCODE IN VARCHAR2,
  CURSOR_RESULT OUT SYS_REFCURSOR
) AS 
  QUERY_SELECT VARCHAR2(1000);
  N_RESULT NUMBER(10,0) := 0;
  BARCODE_EXITS NUMBER(10,0) := 0;
BEGIN
  BEGIN
    IF P_BARCODE IS NOT NULL OR P_BARCODE != '' THEN
        IF INSTR(P_BARCODE,',') > 0 THEN
            BARCODE_EXITS := 0;
            FOR BARCODE_ROW IN (SELECT REGEXP_SUBSTR(''||P_BARCODE||'', '[^,]+', 1, LEVEL) AS BARCODE FROM DUAL CONNECT BY REGEXP_SUBSTR(''||P_BARCODE||'', '[^,]+', 1, LEVEL) IS NOT NULL)
            LOOP
                 QUERY_SELECT := 'SELECT COUNT(a.MAHANG) FROM( SELECT MAHANG,SUBSTR(BARCODE, INSTR(BARCODE, '''||BARCODE_ROW.BARCODE||'''), INSTR(BARCODE, '';'') - 1) AS BARCODE FROM MATHANG WHERE INSTR(BARCODE, '''||BARCODE_ROW.BARCODE||''') > 0 AND UNITCODE = '''||P_MADONVI||''') A WHERE LENGTH(A.BARCODE) = LENGTH('''||BARCODE_ROW.BARCODE||''')';
                 EXECUTE IMMEDIATE QUERY_SELECT INTO BARCODE_EXITS;
                 N_RESULT := N_RESULT + BARCODE_EXITS;
            END LOOP;
        ELSE
            QUERY_SELECT := 'SELECT COUNT(a.MAHANG) FROM( SELECT MAHANG,SUBSTR(BARCODE, INSTR(BARCODE, '''||P_BARCODE||'''), INSTR(BARCODE, '';'') - 1) AS BARCODE FROM MATHANG WHERE INSTR(BARCODE, '''||P_BARCODE||''') > 0 AND UNITCODE = '''||P_MADONVI||''') A WHERE LENGTH(A.BARCODE) = LENGTH('''||P_BARCODE||''')';
            EXECUTE IMMEDIATE QUERY_SELECT INTO N_RESULT;
        END IF;
    END IF;
  END;
  BEGIN
  OPEN CURSOR_RESULT FOR 'SELECT '||NVL(N_RESULT,0)||' AS REFUND_DATA FROM DUAL';
    EXCEPTION
    WHEN NO_DATA_FOUND THEN
     DBMS_OUTPUT.put_line ('NO_DATA_FOUND');  
       WHEN OTHERS THEN
     DBMS_OUTPUT.put_line (SQLERRM);  
  END;
END KIEMTRA_TRUNGBARCODE_EXCEL;




/
--------------------------------------------------------
--  DDL for Procedure MATHANG_TONKHO_PAGINATION
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."MATHANG_TONKHO_PAGINATION" 
(
  P_TABLE_NAME IN VARCHAR2 ,
  P_MAKHO IN VARCHAR2 ,
  P_MADONVI IN VARCHAR2 ,
  P_TUKHOA IN VARCHAR2,
  P_PAGENUMBER IN NUMBER,
  P_PAGESIZE IN NUMBER,
  P_TOTALITEM OUT SYS_REFCURSOR,
  CURSOR_RESULT OUT SYS_REFCURSOR
) AS
  STR_COUNT VARCHAR2(1000);
  STR_QUERY VARCHAR2(3000);
  QUERY_SELECT VARCHAR2(2000);
  T_LOAITIMKIEM VARCHAR2(20) := '';
  TEXT_IS_NUMBER NUMBER(18,2) := 0;
  IS_CONTAIN_UNITCODE VARCHAR2(2):='';
BEGIN
      SELECT IS_NUMBER(P_TUKHOA) INTO TEXT_IS_NUMBER FROM DUAL;
      BEGIN 
          SELECT * INTO IS_CONTAIN_UNITCODE FROM DUAL WHERE REGEXP_REPLACE(P_TUKHOA, '[^ -~]', '@') LIKE '%@%';
          EXCEPTION WHEN NO_DATA_FOUND
          THEN IS_CONTAIN_UNITCODE := '';
      END;
      IF LENGTH(P_TUKHOA) > 5 AND TEXT_IS_NUMBER = 1 AND (IS_CONTAIN_UNITCODE IS NULL OR IS_CONTAIN_UNITCODE = '') 
        THEN T_LOAITIMKIEM := 'BARCODE';
      END IF;
      IF LENGTH(P_TUKHOA) = 7 AND SUBSTR(P_TUKHOA,0,2) <> 'BH' AND TEXT_IS_NUMBER = 0 AND (IS_CONTAIN_UNITCODE IS NULL OR IS_CONTAIN_UNITCODE = '') 
        THEN T_LOAITIMKIEM := 'MAHANG';
      END IF;
      IF IS_NUMBER(SUBSTR(P_TUKHOA,0,1)) = 0 AND IS_NUMBER(SUBSTR(P_TUKHOA,2,LENGTH(P_TUKHOA))) = 1 
        THEN T_LOAITIMKIEM := 'MAHANG';
      END IF;
      IF IS_CONTAIN_UNITCODE = 'X' 
        THEN T_LOAITIMKIEM := 'TENHANG';
      END IF; 
      IF LENGTH(P_TUKHOA) = 4 AND TEXT_IS_NUMBER = 1 AND (IS_CONTAIN_UNITCODE IS NULL OR IS_CONTAIN_UNITCODE = '') 
        THEN T_LOAITIMKIEM := 'ITEMCODE';
      END IF; 
--     DBMS_OUTPUT.PUT_LINE('T_LOAITIMKIEM:'||T_LOAITIMKIEM);
    IF T_LOAITIMKIEM = 'BARCODE' THEN
            QUERY_SELECT := 'SELECT a.ID,a.MAHANG,a.TENHANG,a.MALOAI,a.MANHOM,a.MADONVITINH,a.MANHACUNGCAP,a.MATHUE_VAO,a.MATHUE_RA,a.BARCODE,a.TRANGTHAI,b.TYLE_LAILE,b.TYLE_LAIBUON,b.GIAMUA,b.GIAMUA_VAT,b.GIABANLE,b.GIABANLE_VAT,b.GIABANBUON,b.GIABANBUON_VAT,xnt.GIAVON,xnt.TONCUOIKYSL
                            FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG INNER JOIN '||P_TABLE_NAME||' xnt ON a.MAHANG = xnt.MAHANG AND xnt.MAKHO = '''||P_MAKHO||''' AND a.TRANGTHAI = b.TRANGTHAI AND a.TRANGTHAI = 10 AND a.UNITCODE = '''||P_MADONVI||''' AND UPPER(a.BARCODE) LIKE ''%'||UPPER(P_TUKHOA)||'%'' ';
            ELSIF T_LOAITIMKIEM = 'MAHANG' THEN
                IF IS_NUMBER(SUBSTR(P_TUKHOA,0,1)) = 0 AND IS_NUMBER(SUBSTR(P_TUKHOA,2,LENGTH(P_TUKHOA))) = 1 THEN
                    QUERY_SELECT := 'SELECT a.ID,a.MAHANG,a.TENHANG,a.MALOAI,a.MANHOM,a.MADONVITINH,a.MANHACUNGCAP,a.MATHUE_VAO,a.MATHUE_RA,a.BARCODE,a.TRANGTHAI,b.TYLE_LAILE,b.TYLE_LAIBUON,b.GIAMUA,b.GIAMUA_VAT,b.GIABANLE,b.GIABANLE_VAT,b.GIABANBUON,b.GIABANBUON_VAT,xnt.GIAVON,xnt.TONCUOIKYSL
                            FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG INNER JOIN '||P_TABLE_NAME||' xnt ON a.MAHANG = xnt.MAHANG AND xnt.MAKHO = '''||P_MAKHO||''' AND a.TRANGTHAI = b.TRANGTHAI AND a.TRANGTHAI = 10 AND a.UNITCODE = '''||P_MADONVI||''' AND UPPER(a.MAHANG) LIKE ''%'||UPPER(SUBSTR(P_TUKHOA,0,1))||'%'' AND a.MAHANG LIKE ''%'||UPPER(SUBSTR(P_TUKHOA,2,LENGTH(P_TUKHOA)))||'%'' ';
                ELSE
                    QUERY_SELECT := 'SELECT a.ID,a.MAHANG,a.TENHANG,a.MALOAI,a.MANHOM,a.MADONVITINH,a.MANHACUNGCAP,a.MATHUE_VAO,a.MATHUE_RA,a.BARCODE,a.TRANGTHAI,b.TYLE_LAILE,b.TYLE_LAIBUON,b.GIAMUA,b.GIAMUA_VAT,b.GIABANLE,b.GIABANLE_VAT,b.GIABANBUON,b.GIABANBUON_VAT,xnt.GIAVON,xnt.TONCUOIKYSL
                            FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG INNER JOIN '||P_TABLE_NAME||' xnt ON a.MAHANG = xnt.MAHANG AND xnt.MAKHO = '''||P_MAKHO||''' AND a.TRANGTHAI = b.TRANGTHAI AND a.TRANGTHAI = 10 AND a.UNITCODE = '''||P_MADONVI||''' AND UPPER(a.MAHANG) LIKE ''%'||UPPER(P_TUKHOA)||'%'' ';
                END IF;
            ELSIF T_LOAITIMKIEM = 'TENHANG' THEN
            QUERY_SELECT := 'SELECT a.ID,a.MAHANG,a.TENHANG,a.MALOAI,a.MANHOM,a.MADONVITINH,a.MANHACUNGCAP,a.MATHUE_VAO,a.MATHUE_RA,a.BARCODE,a.TRANGTHAI,b.TYLE_LAILE,b.TYLE_LAIBUON,b.GIAMUA,b.GIAMUA_VAT,b.GIABANLE,b.GIABANLE_VAT,b.GIABANBUON,b.GIABANBUON_VAT,xnt.GIAVON,xnt.TONCUOIKYSL
                            FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG INNER JOIN '||P_TABLE_NAME||' xnt ON a.MAHANG = xnt.MAHANG AND xnt.MAKHO = '''||P_MAKHO||''' AND a.TRANGTHAI = b.TRANGTHAI AND a.TRANGTHAI = 10 AND a.UNITCODE = '''||P_MADONVI||''' AND UPPER(a.TENHANG) LIKE N''%'||UPPER(P_TUKHOA)||'%'' ';
            ELSIF T_LOAITIMKIEM = 'ITEMCODE' THEN
            QUERY_SELECT := 'SELECT a.ID,a.MAHANG,a.TENHANG,a.MALOAI,a.MANHOM,a.MADONVITINH,a.MANHACUNGCAP,a.MATHUE_VAO,a.MATHUE_RA,a.BARCODE,a.TRANGTHAI,b.TYLE_LAILE,b.TYLE_LAIBUON,b.GIAMUA,b.GIAMUA_VAT,b.GIABANLE,b.GIABANLE_VAT,b.GIABANBUON,b.GIABANBUON_VAT,xnt.GIAVON,xnt.TONCUOIKYSL
                            FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG INNER JOIN '||P_TABLE_NAME||' xnt ON a.MAHANG = xnt.MAHANG AND xnt.MAKHO = '''||P_MAKHO||''' AND a.TRANGTHAI = b.TRANGTHAI AND a.TRANGTHAI = 10 AND a.UNITCODE = '''||P_MADONVI||''' AND UPPER(a.ITEMCODE) = '''||UPPER(P_TUKHOA)||''' ';
            ELSE 
            QUERY_SELECT := 'SELECT a.ID,a.MAHANG,a.TENHANG,a.MALOAI,a.MANHOM,a.MADONVITINH,a.MANHACUNGCAP,a.MATHUE_VAO,a.MATHUE_RA,a.BARCODE,a.TRANGTHAI,b.TYLE_LAILE,b.TYLE_LAIBUON,b.GIAMUA,b.GIAMUA_VAT,b.GIABANLE,b.GIABANLE_VAT,b.GIABANBUON,b.GIABANBUON_VAT,xnt.GIAVON,xnt.TONCUOIKYSL
                            FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG INNER JOIN '||P_TABLE_NAME||' xnt ON a.MAHANG = xnt.MAHANG AND xnt.MAKHO = '''||P_MAKHO||''' AND a.TRANGTHAI = b.TRANGTHAI AND a.TRANGTHAI = 10 AND a.UNITCODE = '''||P_MADONVI||''' AND UPPER(a.MAHANG) LIKE ''%'||UPPER(P_TUKHOA)||'%'' ';
    END IF;
    BEGIN
    OPEN P_TOTALITEM FOR 'SELECT COUNT(*) AS TOTAL_ITEM FROM ('||QUERY_SELECT||')';
    EXCEPTION WHEN OTHERS THEN 
    GOTO countinus;
    END;
    <<countinus>>
    STR_QUERY:= 'SELECT * FROM
    (
        SELECT a.*, rownum r__
        FROM
        (
            '||QUERY_SELECT||'
        ) a
        WHERE rownum < (('||P_PAGENUMBER||' * '||P_PAGESIZE||') + 1 )
    )
    WHERE r__ >= ((('||P_PAGENUMBER||'-1) * '||P_PAGESIZE||') + 1)';
            OPEN CURSOR_RESULT FOR STR_QUERY;    
            EXCEPTION WHEN OTHERS THEN COMMIT;
END MATHANG_TONKHO_PAGINATION;



/
--------------------------------------------------------
--  DDL for Procedure NHAPMUA_CHITIET
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."NHAPMUA_CHITIET" (
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



/
--------------------------------------------------------
--  DDL for Procedure NHAPMUA_TONGHOP
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."NHAPMUA_TONGHOP" (
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
    ELSIF DIEUKIEN_NHOM = 'MATHUE' THEN
        BEGIN
            P_COLUMNS_GROUPBY := ' thue.MATHUE ,thue.TENTHUE';
            P_SELECT_COLUMNS_GROUPBY := 'A.MACHA , A.TENCHA';
            P_TABLE_GROUPBY := ' ';
            P_SELECT2 := 'thue.MATHUE AS MACHA , thue.TENTHUE AS TENCHA';
            P_SELECT := '';
        END;
    ELSE
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

    --DBMS_OUTPUT.PUT_LINE(QUERY_STR);
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




/
--------------------------------------------------------
--  DDL for Procedure TIMKIEM_CHUNGTU_PAGINATION
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."TIMKIEM_CHUNGTU_PAGINATION" 
(
  P_MADONVI IN VARCHAR2 ,
  P_TUKHOA IN VARCHAR2,
  P_PAGENUMBER IN NUMBER,
  P_PAGESIZE IN NUMBER,
  P_LOAI_CHUNGTU IN VARCHAR2 ,
  P_TOTALITEM OUT SYS_REFCURSOR,
  CURSOR_RESULT OUT SYS_REFCURSOR
) AS
  STR_COUNT VARCHAR2(1000);
  STR_QUERY VARCHAR2(3000);
  QUERY_SELECT VARCHAR2(2000);
  T_LOAITIMKIEM VARCHAR2(20) := '';
  TEXT_IS_NUMBER NUMBER(18,2) := 0;
  IS_CONTAIN_UNITCODE VARCHAR2(2):='';
BEGIN
      SELECT IS_NUMBER(P_TUKHOA) INTO TEXT_IS_NUMBER FROM DUAL;
      BEGIN 
          SELECT * INTO IS_CONTAIN_UNITCODE FROM DUAL WHERE REGEXP_REPLACE(P_TUKHOA, '[^ -~]', '@') LIKE '%@%';
          EXCEPTION WHEN NO_DATA_FOUND
          THEN IS_CONTAIN_UNITCODE := '';
      END;
      IF LENGTH(P_TUKHOA) > 5 AND TEXT_IS_NUMBER = 1 AND (IS_CONTAIN_UNITCODE IS NULL OR IS_CONTAIN_UNITCODE = '') 
        THEN T_LOAITIMKIEM := 'BARCODE';
      END IF;
      IF LENGTH(P_TUKHOA) = 7 AND SUBSTR(P_TUKHOA,0,2) <> 'BH' AND TEXT_IS_NUMBER = 0 AND (IS_CONTAIN_UNITCODE IS NULL OR IS_CONTAIN_UNITCODE = '') 
        THEN T_LOAITIMKIEM := 'MAHANG';
      END IF;
      IF IS_NUMBER(SUBSTR(P_TUKHOA,0,1)) = 0 AND IS_NUMBER(SUBSTR(P_TUKHOA,2,LENGTH(P_TUKHOA))) = 1 
        THEN T_LOAITIMKIEM := 'MAHANG';
      END IF;
      IF IS_CONTAIN_UNITCODE = 'X' 
        THEN T_LOAITIMKIEM := 'TENHANG';
      END IF; 
      IF LENGTH(P_TUKHOA) = 4 AND INSTR(P_TUKHOA,P_LOAI_CHUNGTU) = 0  AND  TEXT_IS_NUMBER = 1 AND (IS_CONTAIN_UNITCODE IS NULL OR IS_CONTAIN_UNITCODE = '') 
        THEN T_LOAITIMKIEM := 'ITEMCODE';
      END IF;
      IF INSTR(P_TUKHOA,P_LOAI_CHUNGTU) > 0  AND (IS_CONTAIN_UNITCODE IS NULL OR IS_CONTAIN_UNITCODE = '') 
        THEN T_LOAITIMKIEM := 'MA_CHUNGTU';
      END IF;

    IF T_LOAITIMKIEM = 'BARCODE' THEN
            QUERY_SELECT := '   SELECT f.ID,f.MA_CHUNGTU,f.NGAY_CHUNGTU,f.NGAY_DUYETPHIEU,f.MAKHO_NHAP,f.MAKHO_XUAT,f.TRANGTHAI,f.MANHACUNGCAP,f.MAKHACHHANG
                                FROM (
                                SELECT a.MAHANG,a.BARCODE
                                FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG  AND a.TRANGTHAI = b.TRANGTHAI 
                                AND a.TRANGTHAI = 10 AND a.UNITCODE = b.UNITCODE AND a.UNITCODE = '''||P_MADONVI||'''
                                ) e INNER JOIN (SELECT a.ID,a.LOAI_CHUNGTU,a.MA_CHUNGTU,NGAY_CHUNGTU,a.NGAY_DUYETPHIEU,a.MAKHO_NHAP,a.MAKHO_XUAT,a.TRANGTHAI,a.MANHACUNGCAP,a.MAKHACHHANG,b.MAHANG
                                FROM CHUNGTU a INNER JOIN CHUNGTU_CHITIET b ON a.MA_CHUNGTU = b.MA_CHUNGTU AND a.UNITCODE = '''||P_MADONVI||''') f
                                ON e.MAHANG = f.MAHANG WHERE UPPER(e.BARCODE) LIKE ''%'||UPPER(P_TUKHOA)||'%'' AND f.LOAI_CHUNGTU = '''||P_LOAI_CHUNGTU||'''
                                GROUP BY f.ID,f.MA_CHUNGTU,f.NGAY_CHUNGTU,f.NGAY_DUYETPHIEU,f.MAKHO_NHAP,f.MAKHO_XUAT,f.TRANGTHAI,f.MANHACUNGCAP,f.MAKHACHHANG 
                                ORDER BY f.NGAY_CHUNGTU DESC,f.MA_CHUNGTU DESC';
            ELSIF T_LOAITIMKIEM = 'MAHANG' THEN
                QUERY_SELECT := '   SELECT f.ID,f.MA_CHUNGTU,f.NGAY_CHUNGTU,f.NGAY_DUYETPHIEU,f.MAKHO_NHAP,f.MAKHO_XUAT,f.TRANGTHAI,f.MANHACUNGCAP,f.MAKHACHHANG
                                FROM (
                                SELECT a.MAHANG
                                FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG  AND a.TRANGTHAI = b.TRANGTHAI 
                                AND a.TRANGTHAI = 10 AND a.UNITCODE = b.UNITCODE AND a.UNITCODE = '''||P_MADONVI||'''
                                ) e INNER JOIN (SELECT a.ID,a.LOAI_CHUNGTU,a.MA_CHUNGTU,NGAY_CHUNGTU,a.NGAY_DUYETPHIEU,a.MAKHO_NHAP,a.MAKHO_XUAT,a.TRANGTHAI,a.MANHACUNGCAP,a.MAKHACHHANG,b.MAHANG
                                FROM CHUNGTU a INNER JOIN CHUNGTU_CHITIET b ON a.MA_CHUNGTU = b.MA_CHUNGTU AND a.UNITCODE = '''||P_MADONVI||''') f
                                ON e.MAHANG = f.MAHANG WHERE UPPER(e.MAHANG) LIKE ''%'||UPPER(P_TUKHOA)||'%'' AND f.LOAI_CHUNGTU = '''||P_LOAI_CHUNGTU||'''
                                GROUP BY f.ID,f.MA_CHUNGTU,f.NGAY_CHUNGTU,f.NGAY_DUYETPHIEU,f.MAKHO_NHAP,f.MAKHO_XUAT,f.TRANGTHAI,f.MANHACUNGCAP,f.MAKHACHHANG 
                                ORDER BY f.NGAY_CHUNGTU DESC,f.MA_CHUNGTU DESC';
            ELSIF T_LOAITIMKIEM = 'TENHANG' THEN
                QUERY_SELECT := '   SELECT f.ID,f.MA_CHUNGTU,f.NGAY_CHUNGTU,f.NGAY_DUYETPHIEU,f.MAKHO_NHAP,f.MAKHO_XUAT,f.TRANGTHAI,f.MANHACUNGCAP,f.MAKHACHHANG
                                FROM (
                                SELECT a.MAHANG,a.TENHANG
                                FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG  AND a.TRANGTHAI = b.TRANGTHAI 
                                AND a.TRANGTHAI = 10 AND a.UNITCODE = b.UNITCODE AND a.UNITCODE = '''||P_MADONVI||'''
                                ) e INNER JOIN (SELECT a.ID,a.LOAI_CHUNGTU,a.MA_CHUNGTU,NGAY_CHUNGTU,a.NGAY_DUYETPHIEU,a.MAKHO_NHAP,a.MAKHO_XUAT,a.TRANGTHAI,a.MANHACUNGCAP,a.MAKHACHHANG,b.MAHANG
                                FROM CHUNGTU a INNER JOIN CHUNGTU_CHITIET b ON a.MA_CHUNGTU = b.MA_CHUNGTU AND a.UNITCODE = '''||P_MADONVI||''') f
                                ON e.MAHANG = f.MAHANG WHERE UPPER(e.TENHANG) LIKE N''%'||UPPER(P_TUKHOA)||'%'' AND f.LOAI_CHUNGTU = '''||P_LOAI_CHUNGTU||'''
                                GROUP BY f.ID,f.MA_CHUNGTU,f.NGAY_CHUNGTU,f.NGAY_DUYETPHIEU,f.MAKHO_NHAP,f.MAKHO_XUAT,f.TRANGTHAI,f.MANHACUNGCAP,f.MAKHACHHANG 
                                ORDER BY f.NGAY_CHUNGTU DESC,f.MA_CHUNGTU DESC';
            ELSIF T_LOAITIMKIEM = 'ITEMCODE' THEN
                QUERY_SELECT := '   SELECT f.ID,f.MA_CHUNGTU,f.NGAY_CHUNGTU,f.NGAY_DUYETPHIEU,f.MAKHO_NHAP,f.MAKHO_XUAT,f.TRANGTHAI,f.MANHACUNGCAP,f.MAKHACHHANG
                                FROM (
                                SELECT a.MAHANG,a.ITEMCODE
                                FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG  AND a.TRANGTHAI = b.TRANGTHAI 
                                AND a.TRANGTHAI = 10 AND a.UNITCODE = b.UNITCODE AND a.UNITCODE = '''||P_MADONVI||'''
                                ) e INNER JOIN (SELECT a.ID,a.LOAI_CHUNGTU,a.MA_CHUNGTU,NGAY_CHUNGTU,a.NGAY_DUYETPHIEU,a.MAKHO_NHAP,a.MAKHO_XUAT,a.TRANGTHAI,a.MANHACUNGCAP,a.MAKHACHHANG,b.MAHANG
                                FROM CHUNGTU a INNER JOIN CHUNGTU_CHITIET b ON a.MA_CHUNGTU = b.MA_CHUNGTU AND a.UNITCODE = '''||P_MADONVI||''') f
                                ON e.MAHANG = f.MAHANG WHERE UPPER(e.ITEMCODE) = N''%'||UPPER(P_TUKHOA)||'%'' AND f.LOAI_CHUNGTU = '''||P_LOAI_CHUNGTU||'''
                                GROUP BY f.ID,f.MA_CHUNGTU,f.NGAY_CHUNGTU,f.NGAY_DUYETPHIEU,f.MAKHO_NHAP,f.MAKHO_XUAT,f.TRANGTHAI,f.MANHACUNGCAP,f.MAKHACHHANG 
                                ORDER BY f.NGAY_CHUNGTU DESC,f.MA_CHUNGTU DESC';
            ELSIF T_LOAITIMKIEM = 'MA_CHUNGTU' THEN
                QUERY_SELECT := '   SELECT a.ID,a.LOAI_CHUNGTU,a.MA_CHUNGTU,a.NGAY_CHUNGTU,a.NGAY_DUYETPHIEU,a.MAKHO_NHAP,a.MAKHO_XUAT,a.TRANGTHAI,a.MANHACUNGCAP,a.MAKHACHHANG
                                FROM CHUNGTU a INNER JOIN CHUNGTU_CHITIET b ON a.MA_CHUNGTU = b.MA_CHUNGTU AND a.UNITCODE = '''||P_MADONVI||''' AND UPPER(a.MA_CHUNGTU) LIKE N''%'||UPPER(P_TUKHOA)||'%'' AND a.LOAI_CHUNGTU = '''||P_LOAI_CHUNGTU||'''
                                GROUP BY a.ID,a.LOAI_CHUNGTU,a.MA_CHUNGTU,a.NGAY_CHUNGTU,a.NGAY_DUYETPHIEU,a.MAKHO_NHAP,a.MAKHO_XUAT,a.TRANGTHAI,a.MANHACUNGCAP,a.MAKHACHHANG 
                                ORDER BY a.NGAY_CHUNGTU DESC,a.MA_CHUNGTU DESC';
            ELSE 
                QUERY_SELECT := '   SELECT f.ID,f.MA_CHUNGTU,f.NGAY_CHUNGTU,f.NGAY_DUYETPHIEU,f.MAKHO_NHAP,f.MAKHO_XUAT,f.TRANGTHAI,f.MANHACUNGCAP,f.MAKHACHHANG
                                FROM (
                                SELECT a.MAHANG,a.TENHANG
                                FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG  AND a.TRANGTHAI = b.TRANGTHAI 
                                AND a.TRANGTHAI = 10 AND a.UNITCODE = b.UNITCODE AND a.UNITCODE = '''||P_MADONVI||'''
                                ) e INNER JOIN (SELECT a.ID,a.LOAI_CHUNGTU,a.MA_CHUNGTU,NGAY_CHUNGTU,a.NGAY_DUYETPHIEU,a.MAKHO_NHAP,a.MAKHO_XUAT,a.TRANGTHAI,a.MANHACUNGCAP,a.MAKHACHHANG,b.MAHANG
                                FROM CHUNGTU a INNER JOIN CHUNGTU_CHITIET b ON a.MA_CHUNGTU = b.MA_CHUNGTU AND a.UNITCODE = '''||P_MADONVI||''') f
                                ON e.MAHANG = f.MAHANG WHERE UPPER(e.TENHANG) LIKE N''%'||UPPER(P_TUKHOA)||'%'' AND f.LOAI_CHUNGTU = '''||P_LOAI_CHUNGTU||'''
                                GROUP BY f.ID,f.MA_CHUNGTU,f.NGAY_CHUNGTU,f.NGAY_DUYETPHIEU,f.MAKHO_NHAP,f.MAKHO_XUAT,f.TRANGTHAI,f.MANHACUNGCAP,f.MAKHACHHANG 
                                ORDER BY f.NGAY_CHUNGTU DESC,f.MA_CHUNGTU DESC';
    END IF;
    -- DBMS_OUTPUT.PUT_LINE(QUERY_SELECT);
    BEGIN
    OPEN P_TOTALITEM FOR 'SELECT COUNT(*) AS TOTAL_ITEM FROM ('||QUERY_SELECT||')';
    EXCEPTION WHEN OTHERS THEN 
    GOTO countinus;
    END;
    <<countinus>>
    STR_QUERY:= 'SELECT * FROM
    (
        SELECT a.*, rownum r__
        FROM
        (
            '||QUERY_SELECT||'
        ) a
        WHERE rownum < (('||P_PAGENUMBER||' * '||P_PAGESIZE||') + 1 )
    )
    WHERE r__ >= ((('||P_PAGENUMBER||'-1) * '||P_PAGESIZE||') + 1)';
            OPEN CURSOR_RESULT FOR STR_QUERY;    
            EXCEPTION WHEN OTHERS THEN COMMIT;
END TIMKIEM_CHUNGTU_PAGINATION;


/
--------------------------------------------------------
--  DDL for Procedure TIMKIEM_DATPHONG_PAGINATION
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."TIMKIEM_DATPHONG_PAGINATION" 
(
  P_MADONVI IN VARCHAR2 ,
  P_MAPHONG IN VARCHAR2,
  P_PAGENUMBER IN NUMBER,
  P_PAGESIZE IN NUMBER,
  P_TOTALITEM OUT SYS_REFCURSOR,
  CURSOR_RESULT OUT SYS_REFCURSOR
) AS
  STR_QUERY VARCHAR2(3000);
  QUERY_SELECT VARCHAR2(2000);
BEGIN
    QUERY_SELECT := 'SELECT a.ID,a.MA_DATPHONG,a.MAPHONG,a.NGAY_DATPHONG,a.THOIGIAN_DATPHONG,a.TEN_KHACHHANG,a.DIENTHOAI,
    a.CANCUOC_CONGDAN,a.DIENGIAI,a.TRANGTHAI,c.MALOAIPHONG,a.UNITCODE
    FROM DATPHONG a INNER JOIN PHONG b ON a.MAPHONG = b.MAPHONG 
    INNER JOIN LOAIPHONG c ON b.MALOAIPHONG = c.MALOAIPHONG AND a.UNITCODE = b.UNITCODE AND b.UNITCODE = c.UNITCODE 
    WHERE a.TRANGTHAI = 10 
    AND a.MAPHONG = '''||P_MAPHONG||''' AND a.UNITCODE = '''||P_MADONVI||''' 
    AND TO_DATE(A.NGAY_DATPHONG,''DD-MM-YY'') < TO_DATE((SYSDATE + 1),''DD-MM-YY'')';
    BEGIN
    OPEN P_TOTALITEM FOR 'SELECT COUNT(*) AS TOTAL_ITEM FROM ('||QUERY_SELECT||')';
    EXCEPTION WHEN OTHERS THEN 
    GOTO countinus;
    END;
    <<countinus>>
    STR_QUERY:= 'SELECT * FROM
    (
        SELECT a.*, rownum r__
        FROM
        (
            '||QUERY_SELECT||'
        ) a
        WHERE rownum < (('||P_PAGENUMBER||' * '||P_PAGESIZE||') + 1 )
    )
    WHERE r__ >= ((('||P_PAGENUMBER||'-1) * '||P_PAGESIZE||') + 1)';
            OPEN CURSOR_RESULT FOR STR_QUERY;    
            EXCEPTION WHEN OTHERS THEN COMMIT;
END TIMKIEM_DATPHONG_PAGINATION;

/
--------------------------------------------------------
--  DDL for Procedure TIMKIEM_GIAODICHQUAY
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."TIMKIEM_GIAODICHQUAY" 
(
  P_KEYSEARCH IN VARCHAR2 ,
  P_TUNGAY IN OUT DATE,
  P_DENNGAY IN OUT DATE,
  P_DIEUKIENLOC IN OUT NUMBER,
  P_UNITCODE IN VARCHAR2,
  CURSOR_RESULT OUT SYS_REFCURSOR
) AS 
  QUERY_SELECT VARCHAR2(3000);
BEGIN
  IF P_TUNGAY IS NULL OR P_TUNGAY = '' THEN P_TUNGAY := SYSDATE;
  END IF;
  IF P_DENNGAY IS NULL OR P_DENNGAY = '' THEN P_DENNGAY := SYSDATE;
  END IF;
  IF P_DIEUKIENLOC IS NULL OR P_DIEUKIENLOC = '' THEN P_DIEUKIENLOC := '0';
  END IF;
  -- TÌM KIẾM THEO MÃ GIAO DỊCH
  IF P_DIEUKIENLOC = '0' THEN
   QUERY_SELECT := 'SELECT a.MA_GIAODICH,a.LOAI_GIAODICH,a.I_CREATE_DATE,a.I_CREATE_BY,
                    a.NGAY_GIAODICH,a.MA_VOUCHER,a.TIENKHACH_TRA,a.TIEN_TRALAI_KHACH,
                    SUM(b.TIEN_VOUCHER) AS TIEN_VOUCHER,SUM(b.TIENTHE_VIP) AS TIENTHE,
                    SUM(b.THANHTIEN) AS THANHTIEN,a.THOIGIAN_TAO,a.MAKHACHHANG,a.UNITCODE
                    FROM GIAODICH a INNER JOIN GIAODICH_CHITIET b ON a.MA_GIAODICH = b.MA_GIAODICH
                    WHERE a.MA_GIAODICH LIKE ''%'||P_KEYSEARCH||'%'' AND TO_DATE(a.NGAY_GIAODICH,''DD-MM-YY'') >= TO_DATE('''||P_TUNGAY||''',''DD-MM-YY'') 
                    AND TO_DATE(a.NGAY_GIAODICH,''DD-MM-YY'') <= TO_DATE('''||P_DENNGAY||''',''DD-MM-YY'') AND UNITCODE = '''||P_UNITCODE||'''
                    GROUP BY a.MA_GIAODICH,a.LOAI_GIAODICH,a.I_CREATE_DATE,a.I_CREATE_BY,
                    a.NGAY_GIAODICH,a.MA_VOUCHER,a.TIENKHACH_TRA,a.TIEN_TRALAI_KHACH,
                    a.THOIGIAN_TAO,a.MAKHACHHANG,a.UNITCODE
                    ORDER BY a.THOIGIAN_TAO DESC';
  -- TÌM KIẾM THEO SỐ TIỀN
  ELSIF P_DIEUKIENLOC = '1' THEN
   QUERY_SELECT := 'SELECT c.MA_GIAODICH,c.LOAI_GIAODICH,c.I_CREATE_DATE,c.I_CREATE_BY,
                    c.NGAY_GIAODICH,c.MA_VOUCHER,c.TIENKHACH_TRA,c.TIEN_TRALAI_KHACH,
                    c.TIEN_VOUCHER,c.TIENTHE_VIP AS TIENTHE,c.THANHTIEN,c.THOIGIAN_TAO,c.MAKHACHHANG,c.UNITCODE
                    FROM
                    (SELECT a.MA_GIAODICH,a.LOAI_GIAODICH,a.I_CREATE_DATE,a.I_CREATE_BY,
                    a.NGAY_GIAODICH,a.MA_VOUCHER,a.TIENKHACH_TRA,a.TIEN_TRALAI_KHACH,
                    SUM(b.TIEN_VOUCHER) AS TIEN_VOUCHER,SUM(b.TIENTHE_VIP) AS TIENTHE_VIP,
                    SUM(b.THANHTIEN) AS THANHTIEN,a.THOIGIAN_TAO,a.MAKHACHHANG,a.UNITCODE
                    FROM GIAODICH a INNER JOIN GIAODICH_CHITIET b ON a.MA_GIAODICH = b.MA_GIAODICH
                    WHERE TO_DATE(a.NGAY_GIAODICH,''DD-MM-YY'') >= TO_DATE('''||P_TUNGAY||''',''DD-MM-YY'') 
                    AND TO_DATE(a.NGAY_GIAODICH,''DD-MM-YY'') <= TO_DATE('''||P_DENNGAY||''',''DD-MM-YY'') 
                    AND a.UNITCODE = '''||P_UNITCODE||''' 
                    GROUP BY a.MA_GIAODICH,a.LOAI_GIAODICH,a.I_CREATE_DATE,a.I_CREATE_BY,
                    a.NGAY_GIAODICH,a.MA_VOUCHER,a.TIENKHACH_TRA,a.TIEN_TRALAI_KHACH,
                    a.THOIGIAN_TAO,a.MAKHACHHANG,a.UNITCODE
                    ) c WHERE c.THANHTIEN = TO_NUMBER('||P_KEYSEARCH||') ORDER BY c.THOIGIAN_TAO DESC';
   -- TÌM KIẾM THEO THU NGÂN TẠO   
  ELSE
   QUERY_SELECT := 'SELECT c.MA_GIAODICH,c.LOAI_GIAODICH,c.I_CREATE_DATE,c.I_CREATE_BY,
                    c.NGAY_GIAODICH,c.MA_VOUCHER,c.TIENKHACH_TRA,c.TIEN_TRALAI_KHACH,
                    c.TIEN_VOUCHER,c.TIENTHE_VIP AS TIENTHE,c.THANHTIEN,c.THOIGIAN_TAO,c.MAKHACHHANG,c.UNITCODE 
                    FROM (SELECT a.MA_GIAODICH,a.LOAI_GIAODICH,a.I_CREATE_DATE,a.I_CREATE_BY,
                    a.NGAY_GIAODICH,a.MA_VOUCHER,a.TIENKHACH_TRA,a.TIEN_TRALAI_KHACH,
                    SUM(b.TIEN_VOUCHER) AS TIEN_VOUCHER,SUM(b.TIENTHE_VIP) AS TIENTHE_VIP,
                    SUM(b.THANHTIEN) AS THANHTIEN,a.THOIGIAN_TAO,a.MAKHACHHANG,a.UNITCODE
                    FROM GIAODICH a INNER JOIN GIAODICH_CHITIET b ON a.MA_GIAODICH = b.MA_GIAODICH
                    WHERE TO_DATE(a.NGAY_GIAODICH,''DD-MM-YY'') >= TO_DATE('''||P_TUNGAY||''',''DD-MM-YY'') 
                    AND TO_DATE(a.NGAY_GIAODICH,''DD-MM-YY'') <= TO_DATE('''||P_DENNGAY||''',''DD-MM-YY'') AND UNITCODE = '''||P_UNITCODE||'''
                    GROUP BY a.MA_GIAODICH,a.LOAI_GIAODICH,a.I_CREATE_DATE,a.I_CREATE_BY,
                    a.NGAY_GIAODICH,a.MA_VOUCHER,a.TIENKHACH_TRA,a.TIEN_TRALAI_KHACH,
                    a.THOIGIAN_TAO,a.MAKHACHHANG,a.UNITCODE)
                    c INNER JOIN NGUOIDUNG d ON c.I_CREATE_BY = d.MANHANVIEN 
                    WHERE UPPER(d.TENNHANVIEN) LIKE ''%'||UPPER(P_KEYSEARCH)||'%'' ORDER BY c.THOIGIAN_TAO DESC';
  END IF;
    --DBMS_OUTPUT.PUT_LINE('QUERY_SELECT:'||QUERY_SELECT);
  BEGIN
  OPEN CURSOR_RESULT FOR QUERY_SELECT;
    EXCEPTION
    WHEN NO_DATA_FOUND THEN
     DBMS_OUTPUT.put_line ('NO_DATA_FOUND');  
       WHEN OTHERS THEN
     DBMS_OUTPUT.put_line (SQLERRM);  
  END;
END TIMKIEM_GIAODICHQUAY;



/
--------------------------------------------------------
--  DDL for Procedure TIMKIEM_GIAODICH_PAGINATION
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."TIMKIEM_GIAODICH_PAGINATION" 
(
  P_MADONVI IN VARCHAR2 ,
  P_TUKHOA IN VARCHAR2,
  P_PAGENUMBER IN NUMBER,
  P_PAGESIZE IN NUMBER,
  P_TOTALITEM OUT SYS_REFCURSOR,
  CURSOR_RESULT OUT SYS_REFCURSOR
) AS
  STR_COUNT VARCHAR2(1000);
  STR_QUERY VARCHAR2(3000);
  QUERY_SELECT VARCHAR2(2000);
  T_LOAITIMKIEM VARCHAR2(20) := '';
  TEXT_IS_NUMBER NUMBER(18,2) := 0;
  IS_CONTAIN_UNITCODE VARCHAR2(2):='';
BEGIN
      SELECT IS_NUMBER(P_TUKHOA) INTO TEXT_IS_NUMBER FROM DUAL;
      BEGIN 
          SELECT * INTO IS_CONTAIN_UNITCODE FROM DUAL WHERE REGEXP_REPLACE(P_TUKHOA, '[^ -~]', '@') LIKE '%@%';
          EXCEPTION WHEN NO_DATA_FOUND
          THEN IS_CONTAIN_UNITCODE := '';
      END;
      IF TEXT_IS_NUMBER = 0 AND (IS_CONTAIN_UNITCODE IS NULL OR IS_CONTAIN_UNITCODE = '') 
        THEN T_LOAITIMKIEM := 'MA_GIAODICH';
      END IF;
      IF IS_CONTAIN_UNITCODE = 'X' 
        THEN T_LOAITIMKIEM := 'DIENGIAI';
      END IF; 

    IF T_LOAITIMKIEM = 'MA_GIAODICH' THEN
            QUERY_SELECT := ' SELECT
                        GIAODICH.ID,
                        GIAODICH.MA_GIAODICH,
                        GIAODICH.LOAI_GIAODICH,
                        GIAODICH.NGAY_GIAODICH,
                        GIAODICH.MAKHACHHANG,
                        GIAODICH.THOIGIAN_TAO,
                        GIAODICH.TIENKHACH_TRA,
                        GIAODICH.TIEN_TRALAI_KHACH,
                        GIAODICH.MAKHO_XUAT,
                        GIAODICH.MA_VOUCHER,
                        GIAODICH.DIENGIAI,
                        GIAODICH.I_CREATE_BY,
                        SUM(GIAODICH_CHITIET.THANHTIEN) AS THANHTIEN
                    FROM
                        GIAODICH GIAODICH
                        INNER JOIN GIAODICH_CHITIET GIAODICH_CHITIET ON GIAODICH.MA_GIAODICH = GIAODICH_CHITIET.MA_GIAODICH
                        AND GIAODICH.UNITCODE = '''||P_MADONVI||'''
                        AND UPPER(GIAODICH.MA_GIAODICH) LIKE ''%'||UPPER(P_TUKHOA)||'%''
                    GROUP BY
                        GIAODICH.ID,
                        GIAODICH.MA_GIAODICH,
                        GIAODICH.LOAI_GIAODICH,
                        GIAODICH.NGAY_GIAODICH,
                        GIAODICH.MAKHACHHANG,
                        GIAODICH.THOIGIAN_TAO,
                        GIAODICH.TIENKHACH_TRA,
                        GIAODICH.TIEN_TRALAI_KHACH,
                        GIAODICH.MAKHO_XUAT,
                        GIAODICH.MA_VOUCHER,
                        GIAODICH.DIENGIAI,
                        GIAODICH.I_CREATE_BY
                        ORDER BY GIAODICH.NGAY_GIAODICH DESC,GIAODICH.THOIGIAN_TAO ';
            ELSIF T_LOAITIMKIEM = 'DIENGIAI' THEN
                QUERY_SELECT := ' SELECT
                        GIAODICH.ID,
                        GIAODICH.MA_GIAODICH,
                        GIAODICH.LOAI_GIAODICH,
                        GIAODICH.NGAY_GIAODICH,
                        GIAODICH.MAKHACHHANG,
                        GIAODICH.THOIGIAN_TAO,
                        GIAODICH.TIENKHACH_TRA,
                        GIAODICH.TIEN_TRALAI_KHACH,
                        GIAODICH.MAKHO_XUAT,
                        GIAODICH.MA_VOUCHER,
                        GIAODICH.DIENGIAI,
                        GIAODICH.I_CREATE_BY,
                        SUM(GIAODICH_CHITIET.THANHTIEN) AS THANHTIEN
                    FROM
                        GIAODICH GIAODICH
                        INNER JOIN GIAODICH_CHITIET GIAODICH_CHITIET ON GIAODICH.MA_GIAODICH = GIAODICH_CHITIET.MA_GIAODICH
                        AND GIAODICH.UNITCODE = '''||P_MADONVI||'''
                        AND UPPER(GIAODICH.DIENGIAI) LIKE ''%'||UPPER(P_TUKHOA)||'%''
                    GROUP BY
                        GIAODICH.ID,
                        GIAODICH.MA_GIAODICH,
                        GIAODICH.LOAI_GIAODICH,
                        GIAODICH.NGAY_GIAODICH,
                        GIAODICH.MAKHACHHANG,
                        GIAODICH.THOIGIAN_TAO,
                        GIAODICH.TIENKHACH_TRA,
                        GIAODICH.TIEN_TRALAI_KHACH,
                        GIAODICH.MAKHO_XUAT,
                        GIAODICH.MA_VOUCHER,
                        GIAODICH.DIENGIAI,
                        GIAODICH.I_CREATE_BY
                        ORDER BY GIAODICH.NGAY_GIAODICH DESC, GIAODICH.THOIGIAN_TAO';
            ELSE 
                QUERY_SELECT := ' SELECT
                        GIAODICH.ID,
                        GIAODICH.MA_GIAODICH,
                        GIAODICH.LOAI_GIAODICH,
                        GIAODICH.NGAY_GIAODICH,
                        GIAODICH.MAKHACHHANG,
                        GIAODICH.THOIGIAN_TAO,
                        GIAODICH.TIENKHACH_TRA,
                        GIAODICH.TIEN_TRALAI_KHACH,
                        GIAODICH.MAKHO_XUAT,
                        GIAODICH.MA_VOUCHER,
                        GIAODICH.DIENGIAI,
                        GIAODICH.I_CREATE_BY,
                        SUM(GIAODICH_CHITIET.THANHTIEN) AS THANHTIEN
                    FROM
                        GIAODICH GIAODICH
                        INNER JOIN GIAODICH_CHITIET GIAODICH_CHITIET ON GIAODICH.MA_GIAODICH = GIAODICH_CHITIET.MA_GIAODICH
                        AND GIAODICH.UNITCODE = '''||P_MADONVI||'''
                        AND UPPER(GIAODICH.MA_GIAODICH) LIKE ''%'||UPPER(P_TUKHOA)||'%''
                    GROUP BY
                        GIAODICH.ID,
                        GIAODICH.MA_GIAODICH,
                        GIAODICH.LOAI_GIAODICH,
                        GIAODICH.NGAY_GIAODICH,
                        GIAODICH.MAKHACHHANG,
                        GIAODICH.THOIGIAN_TAO,
                        GIAODICH.TIENKHACH_TRA,
                        GIAODICH.TIEN_TRALAI_KHACH,
                        GIAODICH.MAKHO_XUAT,
                        GIAODICH.MA_VOUCHER,
                        GIAODICH.DIENGIAI,
                        GIAODICH.I_CREATE_BY
                        ORDER BY GIAODICH.NGAY_GIAODICH DESC, GIAODICH.THOIGIAN_TAO';
    END IF;
--     DBMS_OUTPUT.PUT_LINE(QUERY_SELECT);
    BEGIN
    OPEN P_TOTALITEM FOR 'SELECT COUNT(*) AS TOTAL_ITEM FROM ('||QUERY_SELECT||')';
    EXCEPTION WHEN OTHERS THEN 
    GOTO countinus;
    END;
    <<countinus>>
    STR_QUERY:= 'SELECT * FROM
    (
        SELECT a.*, rownum r__
        FROM
        (
            '||QUERY_SELECT||'
        ) a
        WHERE rownum < (('||P_PAGENUMBER||' * '||P_PAGESIZE||') + 1 )
    )
    WHERE r__ >= ((('||P_PAGENUMBER||'-1) * '||P_PAGESIZE||') + 1)';
            OPEN CURSOR_RESULT FOR STR_QUERY;    
            EXCEPTION WHEN OTHERS THEN COMMIT;
END TIMKIEM_GIAODICH_PAGINATION;



/
--------------------------------------------------------
--  DDL for Procedure TIMKIEM_KHACHHANG
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."TIMKIEM_KHACHHANG" 
(
  P_KEYSEARCH IN VARCHAR2,
  P_UNITCODE IN VARCHAR2,
  -- 1 là có sử dụng tìm kiếm all
  -- 0 là sử dụng tìm kiếm theo tiêu chí
  P_USE_TIMKIEM_ALL IN NUMBER,
  P_DIEUKIEN_TIMKIEM IN NUMBER,
  CURSOR_RESULT OUT SYS_REFCURSOR
) AS 
  QUERY_SELECT VARCHAR2(3000);
  P_WHERE VARCHAR2(3000);
  TEXT_IS_NUMBER NUMBER(18,2) := 0;
  IS_CONTAIN_UNITCODE VARCHAR2(2):='';
BEGIN
  IF P_KEYSEARCH IS NULL OR P_KEYSEARCH = '' THEN P_WHERE := ' 1 = 1 AND ';
  END IF;
  SELECT IS_NUMBER(P_KEYSEARCH) INTO TEXT_IS_NUMBER FROM DUAL;
  BEGIN 
      SELECT * INTO IS_CONTAIN_UNITCODE FROM DUAL WHERE REGEXP_REPLACE(P_KEYSEARCH, '[^ -~]', '@') LIKE '%@%';
      EXCEPTION WHEN NO_DATA_FOUND
      THEN IS_CONTAIN_UNITCODE := '';
  END;
IF P_USE_TIMKIEM_ALL = 1 THEN
    BEGIN
            -- TÌM KIẾM THEO MÃ KHÁCH HÀNG
          IF SUBSTR(P_KEYSEARCH,0,3) = 'VIP' THEN
          --DBMS_OUTPUT.PUT_LINE('TÌM KIẾM THEO MÃ KHÁCH HÀNG');
           QUERY_SELECT := 'SELECT MAKHACHHANG,TENKHACHHANG,DIACHI,DIENTHOAI,CANCUOC_CONGDAN,NGAYSINH,NGAYDACBIET,MATHE,SODIEM,MAHANG,TONGTIEN,DIENGIAI,TRANGTHAI,I_CREATE_DATE
                            FROM KHACHHANG 
                            WHERE '|| P_WHERE ||' UPPER(MAKHACHHANG) LIKE ''%'||UPPER(P_KEYSEARCH)||'%'' AND UNITCODE = '''||P_UNITCODE||''' ORDER BY MAKHACHHANG';
          -- TÌM KIẾM THEO TÊN KHÁCH HÀNG
          ELSIF IS_CONTAIN_UNITCODE = 'X' THEN
           --DBMS_OUTPUT.PUT_LINE('TÌM KIẾM THEO TÊN KHÁCH HÀNG');
           QUERY_SELECT := 'SELECT MAKHACHHANG,TENKHACHHANG,DIACHI,DIENTHOAI,CANCUOC_CONGDAN,NGAYSINH,NGAYDACBIET,MATHE,SODIEM,MAHANG,TONGTIEN,DIENGIAI,TRANGTHAI,I_CREATE_DATE
                            FROM KHACHHANG 
                            WHERE  '|| P_WHERE ||' UPPER(TENKHACHHANG) LIKE ''%'||UPPER(P_KEYSEARCH)||'%'' AND UNITCODE = '''||P_UNITCODE||''' ORDER BY TENKHACHHANG';
          -- TÌM KIẾM THEO SỐ ĐIỆN THOẠI
          ELSIF TEXT_IS_NUMBER = 1 AND (LENGTH(P_KEYSEARCH) = 11 OR LENGTH(P_KEYSEARCH) = 10)  THEN
          --DBMS_OUTPUT.PUT_LINE('TÌM KIẾM THEO SỐ ĐIỆN THOẠI');
           QUERY_SELECT := 'SELECT MAKHACHHANG,TENKHACHHANG,DIACHI,DIENTHOAI,CANCUOC_CONGDAN,NGAYSINH,NGAYDACBIET,MATHE,SODIEM,MAHANG,TONGTIEN,DIENGIAI,TRANGTHAI,I_CREATE_DATE
                            FROM KHACHHANG 
                            WHERE  '|| P_WHERE ||' UPPER(UNITCODE) = '''||UPPER(P_UNITCODE)||''' AND (UPPER(DIENTHOAI) LIKE ''%'||UPPER(P_KEYSEARCH)||'%'') ORDER BY DIENTHOAI';
          -- TÌM KIẾM THEO SỐ CHỨNG MINH THƯ NHÂN DÂN
          ELSIF TEXT_IS_NUMBER = 1 AND (LENGTH(P_KEYSEARCH) = 9 OR LENGTH(P_KEYSEARCH) = 12)  THEN
          --DBMS_OUTPUT.PUT_LINE('TÌM KIẾM THEO SỐ CHỨNG MINH THƯ NHÂN DÂN');
           QUERY_SELECT := 'SELECT MAKHACHHANG,TENKHACHHANG,DIACHI,DIENTHOAI,CANCUOC_CONGDAN,NGAYSINH,NGAYDACBIET,MATHE,SODIEM,MAHANG,TONGTIEN,DIENGIAI,TRANGTHAI,I_CREATE_DATE 
                            FROM KHACHHANG 
                            WHERE  '|| P_WHERE ||' (UPPER(CANCUOC_CONGDAN) LIKE ''%'||UPPER(P_KEYSEARCH)||'%'' OR UPPER(MATHE) LIKE ''%'||UPPER(P_KEYSEARCH)||'%'' ) AND UNITCODE = '''||P_UNITCODE||''' ORDER BY CANCUOC_CONGDAN';
           -- TÌM KIẾM THEO SỐ ĐIỂM  
          ELSIF TEXT_IS_NUMBER = 1 AND LENGTH(P_KEYSEARCH) < 9  THEN
          --DBMS_OUTPUT.PUT_LINE('TÌM KIẾM THEO SỐ ĐIỂM');
           QUERY_SELECT := 'SELECT MAKHACHHANG,TENKHACHHANG,DIACHI,DIENTHOAI,CANCUOC_CONGDAN,NGAYSINH,NGAYDACBIET,MATHE,SODIEM,MAHANG,TONGTIEN,DIENGIAI,TRANGTHAI,I_CREATE_DATE
                            FROM KHACHHANG 
                            WHERE  '|| P_WHERE ||' UPPER(SODIEM) = '||UPPER(P_KEYSEARCH)||' AND UNITCODE = '''||P_UNITCODE||''' ORDER BY SODIEM';
          ELSE
            --DBMS_OUTPUT.PUT_LINE('TÌM KIẾM MẶC ĐỊNH');
           QUERY_SELECT := 'SELECT MAKHACHHANG,TENKHACHHANG,DIACHI,DIENTHOAI,CANCUOC_CONGDAN,NGAYSINH,NGAYDACBIET,MATHE,SODIEM,MAHANG,TONGTIEN,DIENGIAI,TRANGTHAI,I_CREATE_DATE
                            FROM KHACHHANG 
                            WHERE  '|| P_WHERE ||' ( UPPER(MAKHACHHANG) LIKE ''%'||UPPER(P_KEYSEARCH)||'%'' 
                            OR UPPER(DIENTHOAI) LIKE ''%'||UPPER(P_KEYSEARCH)||'%'' OR UPPER(CANCUOC_CONGDAN) LIKE ''%'||UPPER(P_KEYSEARCH)||'%'' 
                            OR UPPER(TENKHACHHANG) LIKE ''%'||UPPER(P_KEYSEARCH)||'%'' OR UPPER(MATHE) LIKE ''%'||UPPER(P_KEYSEARCH)||'%'' ) 
                            AND UNITCODE = '''||P_UNITCODE||''' ';
        END IF;     
    END;                
ELSE
    BEGIN
            -- TÌM KIẾM THEO MÃ KHÁCH HÀNG
          IF P_DIEUKIEN_TIMKIEM = 0 THEN
          --DBMS_OUTPUT.PUT_LINE('TÌM KIẾM THEO MÃ THẺ');
           QUERY_SELECT := 'SELECT MAKHACHHANG,TENKHACHHANG,DIACHI,DIENTHOAI,CANCUOC_CONGDAN,NGAYSINH,NGAYDACBIET,MATHE,SODIEM,MAHANG,TONGTIEN,DIENGIAI,TRANGTHAI,I_CREATE_DATE
                            FROM KHACHHANG 
                            WHERE '|| P_WHERE ||' UPPER(MATHE) LIKE ''%'||UPPER(P_KEYSEARCH)||'%'' AND UNITCODE = '''||P_UNITCODE||''' ORDER BY MATHE';
          -- TÌM KIẾM THEO TÊN KHÁCH HÀNG
          ELSIF P_DIEUKIEN_TIMKIEM = 1 THEN
           --DBMS_OUTPUT.PUT_LINE('TÌM KIẾM THEO MÃ KHÁCH HÀNG');
           QUERY_SELECT := 'SELECT MAKHACHHANG,TENKHACHHANG,DIACHI,DIENTHOAI,CANCUOC_CONGDAN,NGAYSINH,NGAYDACBIET,MATHE,SODIEM,MAHANG,TONGTIEN,DIENGIAI,TRANGTHAI,I_CREATE_DATE
                            FROM KHACHHANG 
                            WHERE  '|| P_WHERE ||' UPPER(MAKHACHHANG) LIKE ''%'||UPPER(P_KEYSEARCH)||'%'' AND UNITCODE = '''||P_UNITCODE||''' ORDER BY MAKHACHHANG';
          -- TÌM KIẾM THEO TÊN KHÁCH HÀNG
          ELSIF P_DIEUKIEN_TIMKIEM = 2 THEN
          --DBMS_OUTPUT.PUT_LINE('TÌM KIẾM THEO TÊN KHÁCH HÀNG');
           QUERY_SELECT := 'SELECT MAKHACHHANG,TENKHACHHANG,DIACHI,DIENTHOAI,CANCUOC_CONGDAN,NGAYSINH,NGAYDACBIET,MATHE,SODIEM,MAHANG,TONGTIEN,DIENGIAI,TRANGTHAI,I_CREATE_DATE
                            FROM KHACHHANG 
                            WHERE  '|| P_WHERE ||' UNITCODE = '''||P_UNITCODE||''' AND (UPPER(TENKHACHHANG) LIKE N''%'||UPPER(P_KEYSEARCH)||'%'') ';
          -- TÌM KIẾM THEO SỐ ĐIỆN THOẠI
          ELSIF P_DIEUKIEN_TIMKIEM = 3 THEN
          --DBMS_OUTPUT.PUT_LINE('TÌM KIẾM THEO SỐ ĐIỆN THOẠI');
           QUERY_SELECT := 'SELECT MAKHACHHANG,TENKHACHHANG,DIACHI,DIENTHOAI,CANCUOC_CONGDAN,NGAYSINH,NGAYDACBIET,MATHE,SODIEM,MAHANG,TONGTIEN,DIENGIAI,TRANGTHAI,I_CREATE_DATE
                            FROM KHACHHANG 
                            WHERE  '|| P_WHERE ||' UNITCODE = '''||P_UNITCODE||''' AND (UPPER(DIENTHOAI) LIKE ''%'||UPPER(P_KEYSEARCH)||'%'') ORDER BY DIENTHOAI';
          -- TÌM KIẾM THEO SỐ CHỨNG MINH THƯ NHÂN DÂN
          ELSIF P_DIEUKIEN_TIMKIEM = 4 THEN
          --DBMS_OUTPUT.PUT_LINE('TÌM KIẾM THEO SỐ CHỨNG MINH THƯ NHÂN DÂN');
           QUERY_SELECT := 'SELECT MAKHACHHANG,TENKHACHHANG,DIACHI,DIENTHOAI,CANCUOC_CONGDAN,NGAYSINH,NGAYDACBIET,MATHE,SODIEM,MAHANG,TONGTIEN,DIENGIAI,TRANGTHAI,I_CREATE_DATE
                            FROM KHACHHANG 
                            WHERE  '|| P_WHERE ||' UPPER(CANCUOC_CONGDAN) LIKE ''%'||UPPER(P_KEYSEARCH)||'%'' AND UNITCODE = '''||P_UNITCODE||''' ORDER BY CANCUOC_CONGDAN';
           -- TÌM KIẾM THEO SỐ ĐIỂM 
          ELSIF P_DIEUKIEN_TIMKIEM = 5 THEN
          --DBMS_OUTPUT.PUT_LINE('TÌM KIẾM THEO SỐ ĐIỂM');
           QUERY_SELECT := 'SELECT MAKHACHHANG,TENKHACHHANG,DIACHI,DIENTHOAI,CANCUOC_CONGDAN,NGAYSINH,NGAYDACBIET,MATHE,SODIEM,MAHANG,TONGTIEN,DIENGIAI,TRANGTHAI,I_CREATE_DATE
                            FROM KHACHHANG 
                            WHERE  '|| P_WHERE ||' UPPER(SODIEM) = '||UPPER(P_KEYSEARCH)||' AND UNITCODE = '''||P_UNITCODE||''' ORDER BY SODIEM';
          ELSE
            --DBMS_OUTPUT.PUT_LINE('TÌM KIẾM MẶC ĐỊNH');
           QUERY_SELECT := 'SELECT MAKHACHHANG,TENKHACHHANG,DIACHI,DIENTHOAI,CANCUOC_CONGDAN,NGAYSINH,NGAYDACBIET,MATHE,SODIEM,MAHANG,TONGTIEN,DIENGIAI,TRANGTHAI,I_CREATE_DATE
                            FROM KHACHHANG 
                            WHERE  '|| P_WHERE ||' ( UPPER(MAKHACHHANG) LIKE ''%'||UPPER(P_KEYSEARCH)||'%'' OR UPPER(DIENTHOAI) LIKE ''%'||UPPER(P_KEYSEARCH)||'%'' 
                            OR UPPER(CANCUOC_CONGDAN) LIKE ''%'||UPPER(P_KEYSEARCH)||'%'' OR UPPER(TENKHACHHANG) LIKE ''%'||UPPER(P_KEYSEARCH)||'%'' OR UPPER(MATHE) LIKE ''%'||UPPER(P_KEYSEARCH)||'%'' ) 
                            AND UNITCODE = '''||P_UNITCODE||''' ';
        END IF;
    END;
END IF;
    --DBMS_OUTPUT.PUT_LINE('QUERY_SELECT:'||QUERY_SELECT);
  BEGIN
  OPEN CURSOR_RESULT FOR QUERY_SELECT;
    EXCEPTION
    WHEN NO_DATA_FOUND THEN
     DBMS_OUTPUT.put_line ('NO_DATA_FOUND');  
       WHEN OTHERS THEN
     DBMS_OUTPUT.put_line (SQLERRM);  
  END;
END TIMKIEM_KHACHHANG;




/
--------------------------------------------------------
--  DDL for Procedure TIMKIEM_KHOHANG_PAGINATION
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."TIMKIEM_KHOHANG_PAGINATION" 
(
  P_MADONVI IN VARCHAR2 ,
  P_TUKHOA IN VARCHAR2,
  P_PAGENUMBER IN NUMBER,
  P_PAGESIZE IN NUMBER,
  P_TOTALITEM OUT SYS_REFCURSOR,
  CURSOR_RESULT OUT SYS_REFCURSOR
) AS
  STR_COUNT VARCHAR2(1000);
  STR_QUERY VARCHAR2(3000);
  QUERY_SELECT VARCHAR2(2000);
  T_LOAITIMKIEM VARCHAR2(20) := '';
  TEXT_IS_NUMBER NUMBER(18,2) := 0;
  IS_CONTAIN_UNITCODE VARCHAR2(2):='';
BEGIN
      SELECT IS_NUMBER(P_TUKHOA) INTO TEXT_IS_NUMBER FROM DUAL;
      BEGIN 
          SELECT * INTO IS_CONTAIN_UNITCODE FROM DUAL WHERE REGEXP_REPLACE(P_TUKHOA, '[^ -~]', '@') LIKE '%@%';
          EXCEPTION WHEN NO_DATA_FOUND
          THEN IS_CONTAIN_UNITCODE := '';
      END;
      IF IS_CONTAIN_UNITCODE = 'X' 
        THEN T_LOAITIMKIEM := 'TENKHO';
      END IF; 
      IF TEXT_IS_NUMBER = 1 AND (IS_CONTAIN_UNITCODE IS NULL OR IS_CONTAIN_UNITCODE = '') 
        THEN T_LOAITIMKIEM := 'MAKHO';
      END IF;
       IF INSTR(P_TUKHOA,'KH') > 0  AND (IS_CONTAIN_UNITCODE IS NULL OR IS_CONTAIN_UNITCODE = '') 
        THEN T_LOAITIMKIEM := 'MAKHO';
      END IF;
--     DBMS_OUTPUT.PUT_LINE('T_LOAITIMKIEM:'||T_LOAITIMKIEM);
            IF  T_LOAITIMKIEM = 'TENKHO' THEN
            QUERY_SELECT := 'SELECT a.ID,a.MAKHO,a.TENKHO,a.DIENGIAI,a.TRANGTHAI,a.UNITCODE FROM KHOHANG a WHERE a.TRANGTHAI = 10 AND a.UNITCODE = '''||P_MADONVI||''' AND UPPER(a.TENKHO) LIKE N''%'||UPPER(P_TUKHOA)||'%'' ORDER BY a.TENKHO';
            ELSIF T_LOAITIMKIEM = 'MAKHO' THEN
            QUERY_SELECT := 'SELECT a.ID,a.MAKHO,a.TENKHO,a.DIENGIAI,a.TRANGTHAI,a.UNITCODE FROM KHOHANG a WHERE a.TRANGTHAI = 10 AND a.UNITCODE = '''||P_MADONVI||''' AND UPPER(a.MAKHO) LIKE ''%'||UPPER(P_TUKHOA)||'%'' ORDER BY a.MAKHO';
            ELSE 
            QUERY_SELECT := 'SELECT a.ID,a.MAKHO,a.TENKHO,a.DIENGIAI,a.TRANGTHAI,a.UNITCODE FROM KHOHANG a WHERE a.TRANGTHAI = 10 AND a.UNITCODE = '''||P_MADONVI||''' AND UPPER(a.TENKHO) LIKE N''%'||UPPER(P_TUKHOA)||'%'' ORDER BY a.TENKHO';
    END IF;
    BEGIN
    OPEN P_TOTALITEM FOR 'SELECT COUNT(*) AS TOTAL_ITEM FROM ('||QUERY_SELECT||')';
    EXCEPTION WHEN OTHERS THEN 
    GOTO countinus;
    END;
    <<countinus>>
    STR_QUERY:= 'SELECT * FROM
    (
        SELECT a.*, rownum r__
        FROM
        (
            '||QUERY_SELECT||'
        ) a
        WHERE rownum < (('||P_PAGENUMBER||' * '||P_PAGESIZE||') + 1 )
    )
    WHERE r__ >= ((('||P_PAGENUMBER||'-1) * '||P_PAGESIZE||') + 1)';
            OPEN CURSOR_RESULT FOR STR_QUERY;    
            EXCEPTION WHEN OTHERS THEN COMMIT;
END TIMKIEM_KHOHANG_PAGINATION;




/
--------------------------------------------------------
--  DDL for Procedure TIMKIEM_KHUYENMAI_PAGINATION
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."TIMKIEM_KHUYENMAI_PAGINATION" 
(
  P_MADONVI IN VARCHAR2 ,
  P_TUKHOA IN VARCHAR2,
  P_PAGENUMBER IN NUMBER,
  P_PAGESIZE IN NUMBER,
  P_LOAI_KHUYENMAI IN VARCHAR2 ,
  P_TOTALITEM OUT SYS_REFCURSOR,
  CURSOR_RESULT OUT SYS_REFCURSOR
) AS
  STR_COUNT VARCHAR2(1000);
  STR_QUERY VARCHAR2(3000);
  QUERY_SELECT VARCHAR2(2000);
  T_LOAITIMKIEM VARCHAR2(20) := '';
  TEXT_IS_NUMBER NUMBER(18,2) := 0;
  IS_CONTAIN_UNITCODE VARCHAR2(2):='';
BEGIN
      SELECT IS_NUMBER(P_TUKHOA) INTO TEXT_IS_NUMBER FROM DUAL;
      BEGIN 
          SELECT * INTO IS_CONTAIN_UNITCODE FROM DUAL WHERE REGEXP_REPLACE(P_TUKHOA, '[^ -~]', '@') LIKE '%@%';
          EXCEPTION WHEN NO_DATA_FOUND
          THEN IS_CONTAIN_UNITCODE := '';
      END;
      IF IS_CONTAIN_UNITCODE = 'X' 
        THEN T_LOAITIMKIEM := 'DIENGIAI';
      END IF; 
      IF TEXT_IS_NUMBER = 0 AND (IS_CONTAIN_UNITCODE IS NULL OR IS_CONTAIN_UNITCODE = '')
        THEN T_LOAITIMKIEM := 'DIENGIAI';
      END IF;
      IF INSTR(P_TUKHOA,P_LOAI_KHUYENMAI) = 0  AND  TEXT_IS_NUMBER = 1 AND (IS_CONTAIN_UNITCODE IS NULL OR IS_CONTAIN_UNITCODE = '') 
        THEN T_LOAITIMKIEM := 'MAKHUYENMAI';
      END IF;
--     DBMS_OUTPUT.PUT_LINE('T_LOAITIMKIEM:'||T_LOAITIMKIEM);
            IF  T_LOAITIMKIEM = 'MAKHUYENMAI' THEN
            QUERY_SELECT := 'SELECT a.ID,a.MA_KHUYENMAI,a.TUNGAY,a.DENNGAY,a.TUGIO,a.DENGIO,a.MAKHO_KHUYENMAI,a.DIENGIAI,a.TRANGTHAI,a.UNITCODE,a.I_CREATE_DATE,a.THOIGIAN_TAO FROM KHUYENMAI a WHERE  a.LOAI_KHUYENMAI = '''||P_LOAI_KHUYENMAI||''' AND a.UNITCODE = '''||P_MADONVI||''' AND UPPER(a.MA_KHUYENMAI) LIKE ''%'||UPPER(P_TUKHOA)||'%'' ORDER BY a.MA_KHUYENMAI';
            ELSIF T_LOAITIMKIEM = 'DIENGIAI' THEN
            QUERY_SELECT := 'SELECT a.ID,a.MA_KHUYENMAI,a.TUNGAY,a.DENNGAY,a.TUGIO,a.DENGIO,a.MAKHO_KHUYENMAI,a.DIENGIAI,a.TRANGTHAI,a.UNITCODE,a.I_CREATE_DATE,a.THOIGIAN_TAO FROM KHUYENMAI a WHERE  a.LOAI_KHUYENMAI = '''||P_LOAI_KHUYENMAI||''' AND a.UNITCODE = '''||P_MADONVI||''' AND UPPER(a.DIENGIAI) LIKE N''%'||UPPER(P_TUKHOA)||'%'' ORDER BY a.MA_KHUYENMAI';
            ELSE 
            QUERY_SELECT := 'SELECT a.ID,a.MA_KHUYENMAI,a.TUNGAY,a.DENNGAY,a.TUGIO,a.DENGIO,a.MAKHO_KHUYENMAI,a.DIENGIAI,a.TRANGTHAI,a.UNITCODE,a.I_CREATE_DATE,a.THOIGIAN_TAO FROM KHUYENMAI a WHERE  a.LOAI_KHUYENMAI = '''||P_LOAI_KHUYENMAI||''' AND a.UNITCODE = '''||P_MADONVI||''' ORDER BY a.MA_KHUYENMAI';
    END IF;
    --DBMS_OUTPUT.PUT_LINE('QUERY_SELECT:'||QUERY_SELECT);
    BEGIN
    OPEN P_TOTALITEM FOR 'SELECT COUNT(*) AS TOTAL_ITEM FROM ('||QUERY_SELECT||')';
    EXCEPTION WHEN OTHERS THEN 
    GOTO countinus;
    END;
    <<countinus>>
    STR_QUERY:= 'SELECT * FROM
    (
        SELECT a.*, rownum r__
        FROM
        (
            '||QUERY_SELECT||'
        ) a
        WHERE rownum < (('||P_PAGENUMBER||' * '||P_PAGESIZE||') + 1 )
    )
    WHERE r__ >= ((('||P_PAGENUMBER||'-1) * '||P_PAGESIZE||') + 1)';
            OPEN CURSOR_RESULT FOR STR_QUERY;    
            EXCEPTION WHEN OTHERS THEN COMMIT;
END TIMKIEM_KHUYENMAI_PAGINATION;




/
--------------------------------------------------------
--  DDL for Procedure TIMKIEM_LOAIHANG_PAGINATION
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."TIMKIEM_LOAIHANG_PAGINATION" 
(
  P_MADONVI IN VARCHAR2 ,
  P_TUKHOA IN VARCHAR2,
  P_PAGENUMBER IN NUMBER,
  P_PAGESIZE IN NUMBER,
  P_TOTALITEM OUT SYS_REFCURSOR,
  CURSOR_RESULT OUT SYS_REFCURSOR
) AS
  STR_COUNT VARCHAR2(1000);
  STR_QUERY VARCHAR2(3000);
  QUERY_SELECT VARCHAR2(2000);
  T_LOAITIMKIEM VARCHAR2(20) := '';
  TEXT_IS_NUMBER NUMBER(18,2) := 0;
  IS_CONTAIN_UNITCODE VARCHAR2(2):='';
BEGIN
      SELECT IS_NUMBER(P_TUKHOA) INTO TEXT_IS_NUMBER FROM DUAL;
      BEGIN 
          SELECT * INTO IS_CONTAIN_UNITCODE FROM DUAL WHERE REGEXP_REPLACE(P_TUKHOA, '[^ -~]', '@') LIKE '%@%';
          EXCEPTION WHEN NO_DATA_FOUND
          THEN IS_CONTAIN_UNITCODE := '';
      END;
      IF IS_CONTAIN_UNITCODE = 'X' 
        THEN T_LOAITIMKIEM := 'TENLOAI';
      END IF; 
      IF TEXT_IS_NUMBER = 0 AND LENGTH(P_TUKHOA) = 1 
        THEN T_LOAITIMKIEM := 'MALOAI';
      END IF;
--     DBMS_OUTPUT.PUT_LINE('T_LOAITIMKIEM:'||T_LOAITIMKIEM);
            IF  T_LOAITIMKIEM = 'TENLOAI' THEN
            QUERY_SELECT := 'SELECT a.ID,a.MALOAI,a.TENLOAI,a.TRANGTHAI,a.UNITCODE FROM LOAIHANG a WHERE a.TRANGTHAI = 10 AND a.UNITCODE LIKE '''||P_MADONVI||'%'' AND UPPER(a.TENLOAI) LIKE N''%'||UPPER(P_TUKHOA)||'%'' ORDER BY a.TENLOAI';
            ELSIF T_LOAITIMKIEM = 'MALOAI' THEN
            QUERY_SELECT := 'SELECT a.ID,a.MALOAI,a.TENLOAI,a.TRANGTHAI,a.UNITCODE FROM LOAIHANG a WHERE a.TRANGTHAI = 10 AND a.UNITCODE LIKE '''||P_MADONVI||'%'' AND UPPER(a.MALOAI) LIKE ''%'||UPPER(P_TUKHOA)||'%'' ORDER BY a.MALOAI';
            ELSE 
            QUERY_SELECT := 'SELECT a.ID,a.MALOAI,a.TENLOAI,a.TRANGTHAI,a.UNITCODE FROM LOAIHANG a WHERE a.TRANGTHAI = 10 AND a.UNITCODE LIKE '''||P_MADONVI||'%'' AND UPPER(a.TENLOAI) LIKE N''%'||UPPER(P_TUKHOA)||'%'' ORDER BY a.TENLOAI';
    END IF;
    BEGIN
    OPEN P_TOTALITEM FOR 'SELECT COUNT(*) AS TOTAL_ITEM FROM ('||QUERY_SELECT||')';
    EXCEPTION WHEN OTHERS THEN 
    GOTO countinus;
    END;
    <<countinus>>
    STR_QUERY:= 'SELECT * FROM
    (
        SELECT a.*, rownum r__
        FROM
        (
            '||QUERY_SELECT||'
        ) a
        WHERE rownum < (('||P_PAGENUMBER||' * '||P_PAGESIZE||') + 1 )
    )
    WHERE r__ >= ((('||P_PAGENUMBER||'-1) * '||P_PAGESIZE||') + 1)';
            OPEN CURSOR_RESULT FOR STR_QUERY;    
            EXCEPTION WHEN OTHERS THEN COMMIT;
END TIMKIEM_LOAIHANG_PAGINATION;

/
--------------------------------------------------------
--  DDL for Procedure TIMKIEM_MATHANG
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."TIMKIEM_MATHANG" 
(
  P_MADONVI IN VARCHAR2 ,
  P_TUKHOA IN VARCHAR2,
  CURSOR_RESULT OUT SYS_REFCURSOR
) AS 
  QUERY_SELECT VARCHAR2(3000);
  T_LOAITIMKIEM VARCHAR2(20) := '';
  TEXT_IS_NUMBER NUMBER(18,2) := 0;
  IS_CONTAIN_UNITCODE VARCHAR2(2):='';
BEGIN
  SELECT IS_NUMBER(P_TUKHOA) INTO TEXT_IS_NUMBER FROM DUAL;
  BEGIN 
      SELECT * INTO IS_CONTAIN_UNITCODE FROM DUAL WHERE REGEXP_REPLACE(P_TUKHOA, '[^ -~]', '@') LIKE '%@%';
      EXCEPTION WHEN NO_DATA_FOUND
      THEN IS_CONTAIN_UNITCODE := '';
  END;
      IF LENGTH(P_TUKHOA) > 5 AND TEXT_IS_NUMBER = 1 AND (IS_CONTAIN_UNITCODE IS NULL OR IS_CONTAIN_UNITCODE = '') 
        THEN T_LOAITIMKIEM := 'BARCODE';
      END IF;
      IF LENGTH(P_TUKHOA) = 7 AND SUBSTR(P_TUKHOA,0,2) <> 'BH' AND TEXT_IS_NUMBER = 0 AND (IS_CONTAIN_UNITCODE IS NULL OR IS_CONTAIN_UNITCODE = '') 
        THEN T_LOAITIMKIEM := 'MAHANG';
      END IF;
      IF IS_NUMBER(SUBSTR(P_TUKHOA,0,1)) = 0 AND IS_NUMBER(SUBSTR(P_TUKHOA,2,LENGTH(P_TUKHOA))) = 1 
        THEN T_LOAITIMKIEM := 'MAHANG';
      END IF;
      IF IS_CONTAIN_UNITCODE = 'X' 
        THEN T_LOAITIMKIEM := 'TENHANG';
      END IF; 
      IF LENGTH(P_TUKHOA) = 4 AND TEXT_IS_NUMBER = 1 AND (IS_CONTAIN_UNITCODE IS NULL OR IS_CONTAIN_UNITCODE = '') 
        THEN T_LOAITIMKIEM := 'ITEMCODE';
      END IF; 
--     DBMS_OUTPUT.PUT_LINE('T_LOAITIMKIEM:'||T_LOAITIMKIEM);
    IF T_LOAITIMKIEM = 'BARCODE' THEN
            QUERY_SELECT := 'SELECT a.ID,a.MAHANG,a.TENHANG,a.MALOAI,a.MANHOM,a.MADONVITINH,a.MANHACUNGCAP,a.MATHUE_VAO,a.MATHUE_RA,a.BARCODE,a.TRANGTHAI,b.TYLE_LAILE,b.TYLE_LAIBUON,b.GIAMUA,b.GIAMUA_VAT,b.GIABANLE,b.GIABANLE_VAT,b.GIABANBUON,b.GIABANBUON_VAT
                            FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG AND a.TRANGTHAI = b.TRANGTHAI AND a.TRANGTHAI = 10 AND a.UNITCODE = '''||P_MADONVI||''' AND UPPER(a.BARCODE) LIKE ''%'||UPPER(P_TUKHOA)||'%'' ';
            ELSIF T_LOAITIMKIEM = 'MAHANG' THEN
                IF IS_NUMBER(SUBSTR(P_TUKHOA,0,1)) = 0 AND IS_NUMBER(SUBSTR(P_TUKHOA,2,LENGTH(P_TUKHOA))) = 1 THEN
                    QUERY_SELECT := 'SELECT a.ID,a.MAHANG,a.TENHANG,a.MALOAI,a.MANHOM,a.MADONVITINH,a.MANHACUNGCAP,a.MATHUE_VAO,a.MATHUE_RA,a.BARCODE,a.TRANGTHAI,b.TYLE_LAILE,b.TYLE_LAIBUON,b.GIAMUA,b.GIAMUA_VAT,b.GIABANLE,b.GIABANLE_VAT,b.GIABANBUON,b.GIABANBUON_VAT
                            FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG AND a.TRANGTHAI = b.TRANGTHAI AND a.TRANGTHAI = 10 AND a.UNITCODE = '''||P_MADONVI||''' AND UPPER(a.MAHANG) LIKE ''%'||UPPER(SUBSTR(P_TUKHOA,0,1))||'%'' AND a.MAHANG LIKE ''%'||UPPER(SUBSTR(P_TUKHOA,2,LENGTH(P_TUKHOA)))||'%'' ';
                ELSE
                    QUERY_SELECT := 'SELECT a.ID,a.MAHANG,a.TENHANG,a.MALOAI,a.MANHOM,a.MADONVITINH,a.MANHACUNGCAP,a.MATHUE_VAO,a.MATHUE_RA,a.BARCODE,a.TRANGTHAI,b.TYLE_LAILE,b.TYLE_LAIBUON,b.GIAMUA,b.GIAMUA_VAT,b.GIABANLE,b.GIABANLE_VAT,b.GIABANBUON,b.GIABANBUON_VAT
                            FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG AND a.TRANGTHAI = b.TRANGTHAI AND a.TRANGTHAI = 10 AND a.UNITCODE = '''||P_MADONVI||''' AND UPPER(a.MAHANG) LIKE ''%'||UPPER(P_TUKHOA)||'%'' ';
                END IF;
            ELSIF T_LOAITIMKIEM = 'TENHANG' THEN
            QUERY_SELECT := 'SELECT a.ID,a.MAHANG,a.TENHANG,a.MALOAI,a.MANHOM,a.MADONVITINH,a.MANHACUNGCAP,a.MATHUE_VAO,a.MATHUE_RA,a.BARCODE,a.TRANGTHAI,b.TYLE_LAILE,b.TYLE_LAIBUON,b.GIAMUA,b.GIAMUA_VAT,b.GIABANLE,b.GIABANLE_VAT,b.GIABANBUON,b.GIABANBUON_VAT
                            FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG AND a.TRANGTHAI = b.TRANGTHAI AND a.TRANGTHAI = 10 AND a.UNITCODE = '''||P_MADONVI||''' AND UPPER(a.TENHANG) LIKE N''%'||UPPER(P_TUKHOA)||'%'' ';
            ELSIF T_LOAITIMKIEM = 'ITEMCODE' THEN
            QUERY_SELECT := 'SELECT a.ID,a.MAHANG,a.TENHANG,a.MALOAI,a.MANHOM,a.MADONVITINH,a.MANHACUNGCAP,a.MATHUE_VAO,a.MATHUE_RA,a.BARCODE,a.TRANGTHAI,b.TYLE_LAILE,b.TYLE_LAIBUON,b.GIAMUA,b.GIAMUA_VAT,b.GIABANLE,b.GIABANLE_VAT,b.GIABANBUON,b.GIABANBUON_VAT
                            FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG AND a.TRANGTHAI = b.TRANGTHAI AND a.TRANGTHAI = 10 AND a.UNITCODE = '''||P_MADONVI||''' AND UPPER(a.ITEMCODE) = '''||UPPER(P_TUKHOA)||''' ';
            ELSE 
            QUERY_SELECT := 'SELECT a.ID,a.MAHANG,a.TENHANG,a.MALOAI,a.MANHOM,a.MADONVITINH,a.MANHACUNGCAP,a.MATHUE_VAO,a.MATHUE_RA,a.BARCODE,a.TRANGTHAI,b.TYLE_LAILE,b.TYLE_LAIBUON,b.GIAMUA,b.GIAMUA_VAT,b.GIABANLE,b.GIABANLE_VAT,b.GIABANBUON,b.GIABANBUON_VAT
                            FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG AND a.TRANGTHAI = b.TRANGTHAI AND a.TRANGTHAI = 10 AND a.UNITCODE = '''||P_MADONVI||''' AND UPPER(a.MAHANG) LIKE ''%'||UPPER(P_TUKHOA)||'%'' ';
    END IF;
--    DBMS_OUTPUT.PUT_LINE('QUERY_SELECT:'||QUERY_SELECT);
  BEGIN
  OPEN CURSOR_RESULT FOR QUERY_SELECT;
    EXCEPTION
    WHEN NO_DATA_FOUND THEN
     DBMS_OUTPUT.put_line ('NO_DATA_FOUND');  
       WHEN OTHERS THEN
     DBMS_OUTPUT.put_line (SQLERRM);  
  END;
END TIMKIEM_MATHANG;





/
--------------------------------------------------------
--  DDL for Procedure TIMKIEM_MATHANG_PAGINATION
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."TIMKIEM_MATHANG_PAGINATION" 
(
  P_MADONVI IN VARCHAR2 ,
  P_TUKHOA IN VARCHAR2,
  P_PAGENUMBER IN NUMBER,
  P_PAGESIZE IN NUMBER,
  P_TOTALITEM OUT SYS_REFCURSOR,
  CURSOR_RESULT OUT SYS_REFCURSOR
) AS
  STR_COUNT VARCHAR2(1000);
  STR_QUERY VARCHAR2(3000);
  QUERY_SELECT VARCHAR2(2000);
  T_LOAITIMKIEM VARCHAR2(20) := '';
  TEXT_IS_NUMBER NUMBER(18,2) := 0;
  IS_CONTAIN_UNITCODE VARCHAR2(2):='';
BEGIN
      SELECT IS_NUMBER(P_TUKHOA) INTO TEXT_IS_NUMBER FROM DUAL;
      BEGIN 
          SELECT * INTO IS_CONTAIN_UNITCODE FROM DUAL WHERE REGEXP_REPLACE(P_TUKHOA, '[^ -~]', '@') LIKE '%@%';
          EXCEPTION WHEN NO_DATA_FOUND
          THEN IS_CONTAIN_UNITCODE := '';
      END;
      IF LENGTH(P_TUKHOA) > 5 AND TEXT_IS_NUMBER = 1 AND (IS_CONTAIN_UNITCODE IS NULL OR IS_CONTAIN_UNITCODE = '') 
        THEN T_LOAITIMKIEM := 'BARCODE';
      END IF;
      IF LENGTH(P_TUKHOA) = 7 AND SUBSTR(P_TUKHOA,0,2) <> 'BH' AND TEXT_IS_NUMBER = 0 AND (IS_CONTAIN_UNITCODE IS NULL OR IS_CONTAIN_UNITCODE = '') 
        THEN T_LOAITIMKIEM := 'MAHANG';
      END IF;
      IF IS_NUMBER(SUBSTR(P_TUKHOA,0,1)) = 0 AND IS_NUMBER(SUBSTR(P_TUKHOA,2,LENGTH(P_TUKHOA))) = 1 
        THEN T_LOAITIMKIEM := 'MAHANG';
      END IF;
      IF IS_CONTAIN_UNITCODE = 'X' 
        THEN T_LOAITIMKIEM := 'TENHANG';
      END IF; 
      IF LENGTH(P_TUKHOA) = 4 AND TEXT_IS_NUMBER = 1 AND (IS_CONTAIN_UNITCODE IS NULL OR IS_CONTAIN_UNITCODE = '') 
        THEN T_LOAITIMKIEM := 'ITEMCODE';
      END IF; 
--     DBMS_OUTPUT.PUT_LINE('T_LOAITIMKIEM:'||T_LOAITIMKIEM);
    IF T_LOAITIMKIEM = 'BARCODE' THEN
            QUERY_SELECT := 'SELECT a.ID,a.MAHANG,a.TENHANG,a.MALOAI,a.MANHOM,a.MADONVITINH,a.MANHACUNGCAP,a.MATHUE_VAO,a.MATHUE_RA,a.BARCODE,a.TRANGTHAI,b.TYLE_LAILE,b.TYLE_LAIBUON,b.GIAMUA,b.GIAMUA_VAT,b.GIABANLE,b.GIABANLE_VAT,b.GIABANBUON,b.GIABANBUON_VAT
                            FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG AND a.TRANGTHAI = b.TRANGTHAI AND a.TRANGTHAI = 10 AND a.UNITCODE LIKE '''||P_MADONVI||'%'' AND UPPER(a.BARCODE) LIKE ''%'||UPPER(P_TUKHOA)||'%'' ';
            ELSIF T_LOAITIMKIEM = 'MAHANG' THEN
                IF IS_NUMBER(SUBSTR(P_TUKHOA,0,1)) = 0 AND IS_NUMBER(SUBSTR(P_TUKHOA,2,LENGTH(P_TUKHOA))) = 1 THEN
                    QUERY_SELECT := 'SELECT a.ID,a.MAHANG,a.TENHANG,a.MALOAI,a.MANHOM,a.MADONVITINH,a.MANHACUNGCAP,a.MATHUE_VAO,a.MATHUE_RA,a.BARCODE,a.TRANGTHAI,b.TYLE_LAILE,b.TYLE_LAIBUON,b.GIAMUA,b.GIAMUA_VAT,b.GIABANLE,b.GIABANLE_VAT,b.GIABANBUON,b.GIABANBUON_VAT
                            FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG AND a.TRANGTHAI = b.TRANGTHAI AND a.TRANGTHAI = 10 AND a.UNITCODE LIKE '''||P_MADONVI||'%'' AND UPPER(a.MAHANG) LIKE ''%'||UPPER(SUBSTR(P_TUKHOA,0,1))||'%'' AND a.MAHANG LIKE ''%'||UPPER(SUBSTR(P_TUKHOA,2,LENGTH(P_TUKHOA)))||'%'' ';
                ELSE
                    QUERY_SELECT := 'SELECT a.ID,a.MAHANG,a.TENHANG,a.MALOAI,a.MANHOM,a.MADONVITINH,a.MANHACUNGCAP,a.MATHUE_VAO,a.MATHUE_RA,a.BARCODE,a.TRANGTHAI,b.TYLE_LAILE,b.TYLE_LAIBUON,b.GIAMUA,b.GIAMUA_VAT,b.GIABANLE,b.GIABANLE_VAT,b.GIABANBUON,b.GIABANBUON_VAT
                            FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG AND a.TRANGTHAI = b.TRANGTHAI AND a.TRANGTHAI = 10 AND a.UNITCODE LIKE '''||P_MADONVI||'%'' AND UPPER(a.MAHANG) LIKE ''%'||UPPER(P_TUKHOA)||'%'' ';
                END IF;
            ELSIF T_LOAITIMKIEM = 'TENHANG' THEN
            QUERY_SELECT := 'SELECT a.ID,a.MAHANG,a.TENHANG,a.MALOAI,a.MANHOM,a.MADONVITINH,a.MANHACUNGCAP,a.MATHUE_VAO,a.MATHUE_RA,a.BARCODE,a.TRANGTHAI,b.TYLE_LAILE,b.TYLE_LAIBUON,b.GIAMUA,b.GIAMUA_VAT,b.GIABANLE,b.GIABANLE_VAT,b.GIABANBUON,b.GIABANBUON_VAT
                            FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG AND a.TRANGTHAI = b.TRANGTHAI AND a.TRANGTHAI = 10 AND a.UNITCODE LIKE '''||P_MADONVI||'%'' AND UPPER(a.TENHANG) LIKE N''%'||UPPER(P_TUKHOA)||'%'' ';
            ELSIF T_LOAITIMKIEM = 'ITEMCODE' THEN
            QUERY_SELECT := 'SELECT a.ID,a.MAHANG,a.TENHANG,a.MALOAI,a.MANHOM,a.MADONVITINH,a.MANHACUNGCAP,a.MATHUE_VAO,a.MATHUE_RA,a.BARCODE,a.TRANGTHAI,b.TYLE_LAILE,b.TYLE_LAIBUON,b.GIAMUA,b.GIAMUA_VAT,b.GIABANLE,b.GIABANLE_VAT,b.GIABANBUON,b.GIABANBUON_VAT
                            FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG AND a.TRANGTHAI = b.TRANGTHAI AND a.TRANGTHAI = 10 AND a.UNITCODE LIKE '''||P_MADONVI||'%'' AND UPPER(a.ITEMCODE) = '''||UPPER(P_TUKHOA)||''' ';
            ELSE 
            QUERY_SELECT := 'SELECT a.ID,a.MAHANG,a.TENHANG,a.MALOAI,a.MANHOM,a.MADONVITINH,a.MANHACUNGCAP,a.MATHUE_VAO,a.MATHUE_RA,a.BARCODE,a.TRANGTHAI,b.TYLE_LAILE,b.TYLE_LAIBUON,b.GIAMUA,b.GIAMUA_VAT,b.GIABANLE,b.GIABANLE_VAT,b.GIABANBUON,b.GIABANBUON_VAT
                            FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG AND a.TRANGTHAI = b.TRANGTHAI AND a.TRANGTHAI = 10 AND a.UNITCODE LIKE '''||P_MADONVI||'%'' AND UPPER(a.MAHANG) LIKE ''%'||UPPER(P_TUKHOA)||'%'' ';
    END IF;
    BEGIN
    OPEN P_TOTALITEM FOR 'SELECT COUNT(*) AS TOTAL_ITEM FROM ('||QUERY_SELECT||')';
    EXCEPTION WHEN OTHERS THEN 
    GOTO countinus;
    END;
    <<countinus>>
    STR_QUERY:= 'SELECT * FROM
    (
        SELECT a.*, rownum r__
        FROM
        (
            '||QUERY_SELECT||'
        ) a
        WHERE rownum < (('||P_PAGENUMBER||' * '||P_PAGESIZE||') + 1 )
    )
    WHERE r__ >= ((('||P_PAGENUMBER||'-1) * '||P_PAGESIZE||') + 1)';
            OPEN CURSOR_RESULT FOR STR_QUERY;    
            EXCEPTION WHEN OTHERS THEN COMMIT;
END TIMKIEM_MATHANG_PAGINATION;

/
--------------------------------------------------------
--  DDL for Procedure TIMKIEM_MATHANG_TONKHO
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."TIMKIEM_MATHANG_TONKHO" 
(
  P_TABLE_NAME IN VARCHAR2 ,
  P_MAKHO IN VARCHAR2 ,
  P_MADONVI IN VARCHAR2 ,
  P_TUKHOA IN VARCHAR2,
  CURSOR_RESULT OUT SYS_REFCURSOR
) AS 
  QUERY_SELECT VARCHAR2(3000);
  T_LOAITIMKIEM VARCHAR2(20) := '';
  TEXT_IS_NUMBER NUMBER(18,2) := 0;
  IS_CONTAIN_UNITCODE VARCHAR2(2):='';
BEGIN
  SELECT IS_NUMBER(P_TUKHOA) INTO TEXT_IS_NUMBER FROM DUAL;
  BEGIN 
      SELECT * INTO IS_CONTAIN_UNITCODE FROM DUAL WHERE REGEXP_REPLACE(P_TUKHOA, '[^ -~]', '@') LIKE '%@%';
      EXCEPTION WHEN NO_DATA_FOUND
      THEN IS_CONTAIN_UNITCODE := '';
  END;
      IF LENGTH(P_TUKHOA) > 5 AND TEXT_IS_NUMBER = 1 AND (IS_CONTAIN_UNITCODE IS NULL OR IS_CONTAIN_UNITCODE = '') 
        THEN T_LOAITIMKIEM := 'BARCODE';
      END IF;
      IF LENGTH(P_TUKHOA) = 7 AND SUBSTR(P_TUKHOA,0,2) <> 'BH' AND TEXT_IS_NUMBER = 0 AND (IS_CONTAIN_UNITCODE IS NULL OR IS_CONTAIN_UNITCODE = '') 
        THEN T_LOAITIMKIEM := 'MAHANG';
      END IF;
      IF IS_NUMBER(SUBSTR(P_TUKHOA,0,1)) = 0 AND IS_NUMBER(SUBSTR(P_TUKHOA,2,LENGTH(P_TUKHOA))) = 1 
        THEN T_LOAITIMKIEM := 'MAHANG';
      END IF;
      IF IS_CONTAIN_UNITCODE = 'X' 
        THEN T_LOAITIMKIEM := 'TENHANG';
      END IF; 
      IF LENGTH(P_TUKHOA) = 4 AND TEXT_IS_NUMBER = 1 AND (IS_CONTAIN_UNITCODE IS NULL OR IS_CONTAIN_UNITCODE = '') 
        THEN T_LOAITIMKIEM := 'ITEMCODE';
      END IF; 
--     DBMS_OUTPUT.PUT_LINE('T_LOAITIMKIEM:'||T_LOAITIMKIEM);
    IF T_LOAITIMKIEM = 'BARCODE' THEN
            QUERY_SELECT := 'SELECT a.ID,a.MAHANG,a.TENHANG,a.MALOAI,a.MANHOM,a.MADONVITINH,a.MANHACUNGCAP,a.MATHUE_VAO,a.MATHUE_RA,a.BARCODE,a.TRANGTHAI,b.TYLE_LAILE,b.TYLE_LAIBUON,b.GIAMUA,b.GIAMUA_VAT,b.GIABANLE,b.GIABANLE_VAT,b.GIABANBUON,b.GIABANBUON_VAT,xnt.GIAVON, xnt.TONCUOIKYSL
                            FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG INNER JOIN '||P_TABLE_NAME||' xnt ON a.MAHANG = xnt.MAHANG AND xnt.MAKHO = '''||P_MAKHO||''' AND a.TRANGTHAI = b.TRANGTHAI AND a.TRANGTHAI = 10 AND a.UNITCODE = '''||P_MADONVI||''' AND UPPER(a.BARCODE) LIKE ''%'||UPPER(P_TUKHOA)||'%'' ';
            ELSIF T_LOAITIMKIEM = 'MAHANG' THEN
                IF IS_NUMBER(SUBSTR(P_TUKHOA,0,1)) = 0 AND IS_NUMBER(SUBSTR(P_TUKHOA,2,LENGTH(P_TUKHOA))) = 1 THEN
                    QUERY_SELECT := 'SELECT a.ID,a.MAHANG,a.TENHANG,a.MALOAI,a.MANHOM,a.MADONVITINH,a.MANHACUNGCAP,a.MATHUE_VAO,a.MATHUE_RA,a.BARCODE,a.TRANGTHAI,b.TYLE_LAILE,b.TYLE_LAIBUON,b.GIAMUA,b.GIAMUA_VAT,b.GIABANLE,b.GIABANLE_VAT,b.GIABANBUON,b.GIABANBUON_VAT,xnt.GIAVON, xnt.TONCUOIKYSL
                            FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG INNER JOIN '||P_TABLE_NAME||' xnt ON a.MAHANG = xnt.MAHANG AND xnt.MAKHO = '''||P_MAKHO||'''  AND a.TRANGTHAI = b.TRANGTHAI AND a.TRANGTHAI = 10 AND a.UNITCODE = '''||P_MADONVI||''' AND UPPER(a.MAHANG) LIKE ''%'||UPPER(SUBSTR(P_TUKHOA,0,1))||'%'' AND a.MAHANG LIKE ''%'||UPPER(SUBSTR(P_TUKHOA,2,LENGTH(P_TUKHOA)))||'%'' ';
                ELSE
                    QUERY_SELECT := 'SELECT a.ID,a.MAHANG,a.TENHANG,a.MALOAI,a.MANHOM,a.MADONVITINH,a.MANHACUNGCAP,a.MATHUE_VAO,a.MATHUE_RA,a.BARCODE,a.TRANGTHAI,b.TYLE_LAILE,b.TYLE_LAIBUON,b.GIAMUA,b.GIAMUA_VAT,b.GIABANLE,b.GIABANLE_VAT,b.GIABANBUON,b.GIABANBUON_VAT,xnt.GIAVON, xnt.TONCUOIKYSL
                            FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG INNER JOIN '||P_TABLE_NAME||' xnt ON a.MAHANG = xnt.MAHANG AND xnt.MAKHO = '''||P_MAKHO||'''  AND a.TRANGTHAI = b.TRANGTHAI AND a.TRANGTHAI = 10 AND a.UNITCODE = '''||P_MADONVI||''' AND UPPER(a.MAHANG) LIKE ''%'||UPPER(P_TUKHOA)||'%'' ';
                END IF;
            ELSIF T_LOAITIMKIEM = 'TENHANG' THEN
            QUERY_SELECT := 'SELECT a.ID,a.MAHANG,a.TENHANG,a.MALOAI,a.MANHOM,a.MADONVITINH,a.MANHACUNGCAP,a.MATHUE_VAO,a.MATHUE_RA,a.BARCODE,a.TRANGTHAI,b.TYLE_LAILE,b.TYLE_LAIBUON,b.GIAMUA,b.GIAMUA_VAT,b.GIABANLE,b.GIABANLE_VAT,b.GIABANBUON,b.GIABANBUON_VAT,xnt.GIAVON, xnt.TONCUOIKYSL
                            FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG INNER JOIN '||P_TABLE_NAME||' xnt ON a.MAHANG = xnt.MAHANG AND xnt.MAKHO = '''||P_MAKHO||'''  AND a.TRANGTHAI = b.TRANGTHAI AND a.TRANGTHAI = 10 AND a.UNITCODE = '''||P_MADONVI||''' AND UPPER(a.TENHANG) LIKE N''%'||UPPER(P_TUKHOA)||'%'' ';
            ELSIF T_LOAITIMKIEM = 'ITEMCODE' THEN
            QUERY_SELECT := 'SELECT a.ID,a.MAHANG,a.TENHANG,a.MALOAI,a.MANHOM,a.MADONVITINH,a.MANHACUNGCAP,a.MATHUE_VAO,a.MATHUE_RA,a.BARCODE,a.TRANGTHAI,b.TYLE_LAILE,b.TYLE_LAIBUON,b.GIAMUA,b.GIAMUA_VAT,b.GIABANLE,b.GIABANLE_VAT,b.GIABANBUON,b.GIABANBUON_VAT,xnt.GIAVON, xnt.TONCUOIKYSL
                            FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG INNER JOIN '||P_TABLE_NAME||' xnt ON a.MAHANG = xnt.MAHANG AND xnt.MAKHO = '''||P_MAKHO||'''  AND a.TRANGTHAI = b.TRANGTHAI AND a.TRANGTHAI = 10 AND a.UNITCODE = '''||P_MADONVI||''' AND UPPER(a.ITEMCODE) = '''||UPPER(P_TUKHOA)||''' ';
            ELSE 
            QUERY_SELECT := 'SELECT a.ID,a.MAHANG,a.TENHANG,a.MALOAI,a.MANHOM,a.MADONVITINH,a.MANHACUNGCAP,a.MATHUE_VAO,a.MATHUE_RA,a.BARCODE,a.TRANGTHAI,b.TYLE_LAILE,b.TYLE_LAIBUON,b.GIAMUA,b.GIAMUA_VAT,b.GIABANLE,b.GIABANLE_VAT,b.GIABANBUON,b.GIABANBUON_VAT,xnt.GIAVON, xnt.TONCUOIKYSL
                            FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG INNER JOIN '||P_TABLE_NAME||' xnt ON a.MAHANG = xnt.MAHANG AND xnt.MAKHO = '''||P_MAKHO||'''  AND a.TRANGTHAI = b.TRANGTHAI AND a.TRANGTHAI = 10 AND a.UNITCODE = '''||P_MADONVI||''' AND UPPER(a.MAHANG) LIKE ''%'||UPPER(P_TUKHOA)||'%'' ';
    END IF;
  --DBMS_OUTPUT.PUT_LINE('QUERY_SELECT:'||QUERY_SELECT);
  BEGIN
  OPEN CURSOR_RESULT FOR QUERY_SELECT;
    EXCEPTION
    WHEN NO_DATA_FOUND THEN
     DBMS_OUTPUT.put_line ('NO_DATA_FOUND');  
       WHEN OTHERS THEN
     DBMS_OUTPUT.put_line (SQLERRM);  
  END;
END TIMKIEM_MATHANG_TONKHO;



/
--------------------------------------------------------
--  DDL for Procedure TIMKIEM_NHACUNGCAP_PAGINATION
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."TIMKIEM_NHACUNGCAP_PAGINATION" 
(
  P_MADONVI IN VARCHAR2 ,
  P_TUKHOA IN VARCHAR2,
  P_PAGENUMBER IN NUMBER,
  P_PAGESIZE IN NUMBER,
  P_TOTALITEM OUT SYS_REFCURSOR,
  CURSOR_RESULT OUT SYS_REFCURSOR
) AS
  STR_COUNT VARCHAR2(1000);
  STR_QUERY VARCHAR2(3000);
  QUERY_SELECT VARCHAR2(2000);
  T_LOAITIMKIEM VARCHAR2(20) := '';
  TEXT_IS_NUMBER NUMBER(18,2) := 0;
  IS_CONTAIN_UNITCODE VARCHAR2(2):='';
BEGIN
      SELECT IS_NUMBER(P_TUKHOA) INTO TEXT_IS_NUMBER FROM DUAL;
      BEGIN 
          SELECT * INTO IS_CONTAIN_UNITCODE FROM DUAL WHERE REGEXP_REPLACE(P_TUKHOA, '[^ -~]', '@') LIKE '%@%';
          EXCEPTION WHEN NO_DATA_FOUND
          THEN IS_CONTAIN_UNITCODE := '';
      END;
      IF IS_CONTAIN_UNITCODE = 'X' 
        THEN T_LOAITIMKIEM := 'TENNHACUNGCAP';
      END IF; 
      IF TEXT_IS_NUMBER = 1
        THEN T_LOAITIMKIEM := 'SODIENTHOAI';
      END IF;
      IF TEXT_IS_NUMBER = 0 AND INSTR(P_TUKHOA,'PP') > 0
        THEN T_LOAITIMKIEM := 'MANHACUNGCAP';
      END IF;
--     DBMS_OUTPUT.PUT_LINE('T_LOAITIMKIEM:'||T_LOAITIMKIEM);
            IF  T_LOAITIMKIEM = 'TENNHACUNGCAP' THEN
            QUERY_SELECT := 'SELECT a.ID,a.MANHACUNGCAP,a.TENNHACUNGCAP,a.DIACHI,a.MASOTHUE,a.DIENTHOAI,a.DIENGIAI,a.TRANGTHAI,a.UNITCODE FROM NHACUNGCAP a WHERE a.TRANGTHAI = 10 AND a.UNITCODE = '''||P_MADONVI||''' AND (UPPER(a.TENNHACUNGCAP) LIKE N''%'||UPPER(P_TUKHOA)||'%'' OR UPPER(a.DIACHI) LIKE N''%'||UPPER(P_TUKHOA)||'%'') ORDER BY a.TENNHACUNGCAP';
            ELSIF T_LOAITIMKIEM = 'MANHACUNGCAP' THEN
            QUERY_SELECT := 'SELECT a.ID,a.MANHACUNGCAP,a.TENNHACUNGCAP,a.DIACHI,a.MASOTHUE,a.DIENTHOAI,a.DIENGIAI,a.TRANGTHAI,a.UNITCODE FROM NHACUNGCAP a WHERE a.TRANGTHAI = 10 AND a.UNITCODE = '''||P_MADONVI||''' AND UPPER(a.MANHACUNGCAP) LIKE ''%'||UPPER(P_TUKHOA)||'%'' ORDER BY a.MANHACUNGCAP';
            ELSIF T_LOAITIMKIEM = 'SODIENTHOAI' THEN
            QUERY_SELECT := 'SELECT a.ID,a.MANHACUNGCAP,a.TENNHACUNGCAP,a.DIACHI,a.MASOTHUE,a.DIENTHOAI,a.DIENGIAI,a.TRANGTHAI,a.UNITCODE FROM NHACUNGCAP a WHERE a.TRANGTHAI = 10 AND a.UNITCODE = '''||P_MADONVI||''' AND (UPPER(a.DIENTHOAI) LIKE ''%'||UPPER(P_TUKHOA)||'%'' OR UPPER(a.MASOTHUE) LIKE ''%'||UPPER(P_TUKHOA)||'%'') ORDER BY a.DIENTHOAI,a.MASOTHUE';
            ELSE 
            QUERY_SELECT := 'SELECT a.ID,a.MANHACUNGCAP,a.TENNHACUNGCAP,a.DIACHI,a.MASOTHUE,a.DIENTHOAI,a.DIENGIAI,a.TRANGTHAI,a.UNITCODE FROM NHACUNGCAP a WHERE a.TRANGTHAI = 10 AND a.UNITCODE = '''||P_MADONVI||''' AND (UPPER(a.TENNHACUNGCAP) LIKE N''%'||UPPER(P_TUKHOA)||'%'' OR UPPER(a.DIACHI) LIKE N''%'||UPPER(P_TUKHOA)||'%'') ORDER BY a.TENNHACUNGCAP';
    END IF;
    IF P_TUKHOA IS NULL OR P_TUKHOA = ''  THEN QUERY_SELECT := 'SELECT a.ID,a.MANHACUNGCAP,a.TENNHACUNGCAP,a.DIACHI,a.MASOTHUE,a.DIENTHOAI,a.DIENGIAI,a.TRANGTHAI,a.UNITCODE FROM NHACUNGCAP a WHERE a.TRANGTHAI = 10 AND a.UNITCODE = '''||P_MADONVI||''' ORDER BY a.TENNHACUNGCAP';
    END IF;
    --DBMS_OUTPUT.PUT_LINE('QUERY_SELECT:'||QUERY_SELECT);
    BEGIN
    OPEN P_TOTALITEM FOR 'SELECT COUNT(*) AS TOTAL_ITEM FROM ('||QUERY_SELECT||')';
    EXCEPTION WHEN OTHERS THEN 
    GOTO countinus;
    END;
    <<countinus>>
    STR_QUERY:= 'SELECT * FROM
    (
        SELECT a.*, rownum r__
        FROM
        (
            '||QUERY_SELECT||'
        ) a
        WHERE rownum < (('||P_PAGENUMBER||' * '||P_PAGESIZE||') + 1 )
    )
    WHERE r__ >= ((('||P_PAGENUMBER||'-1) * '||P_PAGESIZE||') + 1)';
            OPEN CURSOR_RESULT FOR STR_QUERY;    
            EXCEPTION WHEN OTHERS THEN COMMIT;
END TIMKIEM_NHACUNGCAP_PAGINATION;


/
--------------------------------------------------------
--  DDL for Procedure TIMKIEM_NHOMHANG_PAGINATION
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."TIMKIEM_NHOMHANG_PAGINATION" 
(
  P_MADONVI IN VARCHAR2 ,
  P_TUKHOA IN VARCHAR2,
  P_PAGENUMBER IN NUMBER,
  P_PAGESIZE IN NUMBER,
  P_TOTALITEM OUT SYS_REFCURSOR,
  CURSOR_RESULT OUT SYS_REFCURSOR
) AS
  STR_COUNT VARCHAR2(1000);
  STR_QUERY VARCHAR2(3000);
  QUERY_SELECT VARCHAR2(2000);
  T_LOAITIMKIEM VARCHAR2(20) := '';
  TEXT_IS_NUMBER NUMBER(18,2) := 0;
  IS_CONTAIN_UNITCODE VARCHAR2(2):='';
BEGIN
      SELECT IS_NUMBER(P_TUKHOA) INTO TEXT_IS_NUMBER FROM DUAL;
      BEGIN 
          SELECT * INTO IS_CONTAIN_UNITCODE FROM DUAL WHERE REGEXP_REPLACE(P_TUKHOA, '[^ -~]', '@') LIKE '%@%';
          EXCEPTION WHEN NO_DATA_FOUND
          THEN IS_CONTAIN_UNITCODE := '';
      END;
      IF IS_CONTAIN_UNITCODE = 'X' 
        THEN T_LOAITIMKIEM := 'TENNHOM';
      END IF; 
      IF TEXT_IS_NUMBER = 0 AND LENGTH(P_TUKHOA) = 1 
        THEN T_LOAITIMKIEM := 'MALOAI';
      END IF;
      IF TEXT_IS_NUMBER = 0 AND INSTR(P_TUKHOA,'N') > 0
        THEN T_LOAITIMKIEM := 'MANHOM';
      END IF;
--     DBMS_OUTPUT.PUT_LINE('T_LOAITIMKIEM:'||T_LOAITIMKIEM);
            IF  T_LOAITIMKIEM = 'TENLOAI' THEN
            QUERY_SELECT := 'SELECT a.ID,a.MANHOM,a.TENNHOM,a.MALOAI,b.TENLOAI,a.TRANGTHAI,a.UNITCODE FROM NHOMHANG a INNER JOIN LOAIHANG b ON a.MALOAI = b.MALOAI WHERE a.TRANGTHAI = 10 AND a.UNITCODE LIKE '''||P_MADONVI||'%'' AND UPPER(a.TENNHOM) LIKE N''%'||UPPER(P_TUKHOA)||'%'' ORDER BY a.TENNHOM';
            ELSIF T_LOAITIMKIEM = 'MANHOM' THEN
            QUERY_SELECT := 'SELECT a.ID,a.MANHOM,a.TENNHOM,a.MALOAI,b.TENLOAI,a.TRANGTHAI,a.UNITCODE FROM NHOMHANG a INNER JOIN LOAIHANG b ON a.MALOAI = b.MALOAI WHERE a.TRANGTHAI = 10 AND a.UNITCODE LIKE '''||P_MADONVI||'%'' AND UPPER(a.MANHOM) LIKE ''%'||UPPER(P_TUKHOA)||'%'' ORDER BY a.MANHOM';
            ELSIF T_LOAITIMKIEM = 'MALOAI' THEN
            QUERY_SELECT := 'SELECT a.ID,a.MANHOM,a.TENNHOM,a.MALOAI,b.TENLOAI,a.TRANGTHAI,a.UNITCODE FROM NHOMHANG a INNER JOIN LOAIHANG b ON a.MALOAI = b.MALOAI WHERE a.TRANGTHAI = 10 AND a.UNITCODE LIKE '''||P_MADONVI||'%'' AND UPPER(a.MALOAI) LIKE ''%'||UPPER(P_TUKHOA)||'%'' ORDER BY a.MALOAI';
            ELSE 
            QUERY_SELECT := 'SELECT a.ID,a.MANHOM,a.TENNHOM,a.MALOAI,b.TENLOAI,a.TRANGTHAI,a.UNITCODE FROM NHOMHANG a INNER JOIN LOAIHANG b ON a.MALOAI = b.MALOAI WHERE a.TRANGTHAI = 10 AND a.UNITCODE LIKE '''||P_MADONVI||'%'' AND UPPER(a.TENNHOM) LIKE N''%'||UPPER(P_TUKHOA)||'%'' ORDER BY a.TENNHOM';
    END IF;
    BEGIN
    OPEN P_TOTALITEM FOR 'SELECT COUNT(*) AS TOTAL_ITEM FROM ('||QUERY_SELECT||')';
    EXCEPTION WHEN OTHERS THEN 
    GOTO countinus;
    END;
    <<countinus>>
    STR_QUERY:= 'SELECT * FROM
    (
        SELECT a.*, rownum r__
        FROM
        (
            '||QUERY_SELECT||'
        ) a
        WHERE rownum < (('||P_PAGENUMBER||' * '||P_PAGESIZE||') + 1 )
    )
    WHERE r__ >= ((('||P_PAGENUMBER||'-1) * '||P_PAGESIZE||') + 1)';
            OPEN CURSOR_RESULT FOR STR_QUERY;    
            EXCEPTION WHEN OTHERS THEN COMMIT;
END TIMKIEM_NHOMHANG_PAGINATION;

/
--------------------------------------------------------
--  DDL for Procedure UPDATE_GIA_SAUDUYET_NHAPMUA
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."UPDATE_GIA_SAUDUYET_NHAPMUA" 
(
  P_MADONVI IN VARCHAR2 ,
  P_ID IN VARCHAR2,
  P_RESULT_UPDATE OUT SYS_REFCURSOR
) AS
  STR_QUERY VARCHAR2(500);
  CURSOR_VOUCHER SYS_REFCURSOR;
  MAHANG VARCHAR2(20) := '';
  GIAMUA NUMBER(18,2) := 0;
  GIAMUA_VAT NUMBER(18,2) := 0;
  N_UPDATE NUMBER(10,0) := 0;
BEGIN
    OPEN CURSOR_VOUCHER FOR 'SELECT b.MAHANG,b.GIAMUA,b.GIAMUA_VAT FROM CHUNGTU a INNER JOIN CHUNGTU_CHITIET b ON a.MA_CHUNGTU = b.MA_CHUNGTU AND a.ID = '''||P_ID||''' AND a.UNITCODE = '''||P_MADONVI||''' ';
    LOOP
       FETCH CURSOR_VOUCHER INTO MAHANG,GIAMUA,GIAMUA_VAT;
       EXIT WHEN CURSOR_VOUCHER%NOTFOUND;
       -- neu gia mua = 0 thi gan lai gia mua bang 1
       IF (GIAMUA = 0 OR GIAMUA IS NULL) THEN GIAMUA := 1; END IF;
       STR_QUERY := 'UPDATE MATHANG_GIA SET GIAMUA = '||GIAMUA||', GIAMUA_VAT = '||GIAMUA_VAT||', TYLE_LAILE = ROUND(100 * ((100 * (GIABANLE - '||GIAMUA||')) / '||GIAMUA||')) / 100 , TYLE_LAIBUON = ROUND(100 * ((100 * (GIABANBUON - '||GIAMUA||')) / '||GIAMUA||')) / 100 WHERE MAHANG = '''||MAHANG||''' AND UNITCODE = '''||P_MADONVI||''' ';
       EXECUTE IMMEDIATE STR_QUERY;
       N_UPDATE := N_UPDATE + 1;
    END LOOP;
    OPEN P_RESULT_UPDATE FOR 'SELECT '||N_UPDATE||' AS RESULT_UPDATE FROM DUAL' ;        
        EXCEPTION
           WHEN NO_DATA_FOUND
           THEN
              DBMS_OUTPUT.put_line ('<your message>' || SQLERRM);
           WHEN OTHERS
           THEN
              DBMS_OUTPUT.put_line ('ERROR'  || SQLERRM);   
END UPDATE_GIA_SAUDUYET_NHAPMUA;




/
--------------------------------------------------------
--  DDL for Procedure XUATBANLE_TONGHOP
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."XUATBANLE_TONGHOP" (TUNGAY IN DATE, DENNGAY IN DATE,DIEUKIEN_NHOM IN VARCHAR2,MAKHO IN VARCHAR2,MALOAI IN VARCHAR2,MANHOM IN VARCHAR2,MANHACUNGCAP IN VARCHAR2,MAHANG IN VARCHAR2,UNITCODE IN VARCHAR2,USERNAME IN VARCHAR2, CUR_OUT OUT SYS_REFCURSOR) AS
    QUERY_STR VARCHAR(2000);
    QUERY_SELECT VARCHAR(500);
    QUERY_WHERE_IN VARCHAR(500) := '';
    QUERY_GROUPBY VARCHAR(500) := '';
    QUERY_ORDERBY VARCHAR(200) := '';
    TABLE_JOIN VARCHAR(500) := '';
BEGIN
    IF MAKHO IS NOT NULL OR MAKHO != '' THEN
        QUERY_WHERE_IN := QUERY_WHERE_IN || ' AND gd.MAKHO_XUAT IN (SELECT REGEXP_SUBSTR('''||MAKHO||''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''||MAKHO||''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
    END IF;
    IF MALOAI IS NOT NULL OR MALOAI != '' THEN
        QUERY_WHERE_IN := QUERY_WHERE_IN || ' AND loaihang.MALOAI IN (SELECT REGEXP_SUBSTR('''||MALOAI||''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''||MALOAI||''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
    END IF;
    IF MANHOM IS NOT NULL OR MANHOM != '' THEN
        QUERY_WHERE_IN := QUERY_WHERE_IN || ' AND nhomhang.MANHOM IN (SELECT REGEXP_SUBSTR('''||MANHOM||''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''||MANHOM||''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
    END IF;
    IF MANHACUNGCAP IS NOT NULL OR MANHACUNGCAP != '' THEN
        QUERY_WHERE_IN := QUERY_WHERE_IN || ' AND nhacungcap.MANHACUNGCAP IN (SELECT REGEXP_SUBSTR('''||MANHACUNGCAP||''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''||MANHACUNGCAP||''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
    END IF;
    IF MAHANG IS NOT NULL OR MAHANG != '' THEN
        QUERY_WHERE_IN := QUERY_WHERE_IN || ' AND gdct.MAHANG IN (SELECT REGEXP_SUBSTR('''||MAHANG||''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''||MAHANG||''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
    END IF;

    IF DIEUKIEN_NHOM = 'KHOHANG' THEN
        QUERY_SELECT := QUERY_SELECT || ' gd.MAKHO_XUAT AS MA,khohang.TENKHO AS TEN ';
        TABLE_JOIN := ' INNER JOIN KHOHANG khohang ON gd.MAKHO_XUAT = khohang.MAKHO ';
        QUERY_GROUPBY := QUERY_GROUPBY || ' GROUP BY gd.MAKHO_XUAT,khohang.TENKHO' ;
        QUERY_ORDERBY := ' ORDER BY gd.MAKHO_XUAT';
    ELSIF DIEUKIEN_NHOM = 'LOAIHANG' THEN
        QUERY_SELECT := QUERY_SELECT || ' mathang.MALOAI AS MA,loaihang.TENLOAI AS TEN ';
        TABLE_JOIN := ' INNER JOIN MATHANG mathang ON gdct.MAHANG = mathang.MAHANG INNER JOIN LOAIHANG loaihang ON mathang.MALOAI = loaihang.MALOAI ';
        QUERY_GROUPBY := QUERY_GROUPBY || ' GROUP BY mathang.MALOAI,loaihang.TENLOAI' ;
        QUERY_ORDERBY := ' ORDER BY mathang.MALOAI';
    ELSIF DIEUKIEN_NHOM = 'NHOMHANG' THEN
        QUERY_SELECT := QUERY_SELECT || ' mathang.MANHOM AS MA,nhomhang.TENNHOM AS TEN ';
        TABLE_JOIN := ' INNER JOIN MATHANG mathang ON gdct.MAHANG = mathang.MAHANG INNER JOIN NHOMHANG nhomhang ON mathang.MANHOM = nhomhang.MANHOM ';
        QUERY_GROUPBY := QUERY_GROUPBY || ' GROUP BY mathang.MANHOM,nhomhang.TENNHOM' ;
        QUERY_ORDERBY := ' ORDER BY mathang.MANHOM';
    ELSIF DIEUKIEN_NHOM = 'NHACUNGCAP' THEN
        QUERY_SELECT := QUERY_SELECT || ' mathang.MANHACUNGCAP AS MA,nhacungcap.TENNHACUNGCAP AS TEN ';
        TABLE_JOIN := ' INNER JOIN MATHANG mathang ON gdct.MAHANG = mathang.MAHANG INNER JOIN NHACUNGCAP nhacungcap ON mathang.MANHACUNGCAP = nhacungcap.MANHACUNGCAP ';
        QUERY_GROUPBY := QUERY_GROUPBY || ' GROUP BY mathang.MANHACUNGCAP,nhacungcap.TENNHACUNGCAP' ;
        QUERY_ORDERBY := ' ORDER BY mathang.MANHACUNGCAP';
    ELSIF DIEUKIEN_NHOM = 'MATHANG' THEN
        QUERY_SELECT := QUERY_SELECT || ' gdct.MAHANG AS MA,mathang.TENHANG AS TEN ';
        TABLE_JOIN := ' INNER JOIN MATHANG mathang ON gdct.MAHANG = mathang.MAHANG ';
        QUERY_GROUPBY := QUERY_GROUPBY || ' GROUP BY gdct.MAHANG,mathang.TENHANG ' ;
        QUERY_ORDERBY := ' ORDER BY gdct.MAHANG';
    ELSE
        QUERY_GROUPBY := QUERY_GROUPBY || ' 1 AND 1 ';
    END IF;

    QUERY_STR := 'SELECT '||QUERY_SELECT||' ,ROUND(SUM(gdct.SOLUONG), 2) AS SOLUONG, ROUND(SUM(gdct.THANHTIEN), 2) AS GIATRI 
    FROM GIAODICH gd INNER JOIN GIAODICH_CHITIET gdct ON gd.MA_GIAODICH = gdct.MA_GIAODICH '||TABLE_JOIN||' '||QUERY_WHERE_IN||'
    WHERE gd.UNITCODE = '''||UNITCODE||''' 
    AND TO_DATE(NGAY_GIAODICH,''DD-MM-YY'') >= TO_DATE('''||TUNGAY||''',''DD-MM-YY'')
    AND TO_DATE(NGAY_GIAODICH,''DD-MM-YY'') <= TO_DATE('''||DENNGAY||''',''DD-MM-YY'') 
    '||QUERY_GROUPBY || QUERY_ORDERBY ||'';
    BEGIN
    --DBMS_OUTPUT.put_line (QUERY_STR);  
    OPEN CUR_OUT FOR QUERY_STR;
    EXCEPTION
                WHEN NO_DATA_FOUND THEN
                 NULL;
                   WHEN OTHERS THEN
          NULL;
    END;
END XUATBANLE_TONGHOP;



/
--------------------------------------------------------
--  DDL for Procedure XUATNHAPTON_CHITIET
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."XUATNHAPTON_CHITIET" (
    DIEUKIEN_NHOM   IN              VARCHAR2,
    MAKHO           IN              VARCHAR2,
    MALOAI          IN              VARCHAR2,
    MANHOM          IN              VARCHAR2,
    MAHANG          IN              VARCHAR2,
    MANHACUNGCAP    IN              VARCHAR2,
    UNITCODE        IN              VARCHAR2,
    MANHANVIEN      IN              VARCHAR2,
    USERNAME        IN              VARCHAR2,
    TUNGAY          IN              DATE,
    DENNGAY         IN              DATE,
    CUR             OUT             SYS_REFCURSOR
) AS

    QUERY_STR           VARCHAR(4000);
    KY_KETTHUC          NUMBER(10, 0);
    NAM_KETTHUC         NUMBER(10, 0);
    P_SELECT_COLUMNS    VARCHAR(1000);
    P_TABLE_GROUPBY     VARCHAR(1000);
    P_COLUMNS_GROUPBY   VARCHAR(1000);
    P_SQL_INSERT        VARCHAR(5000);
    P_TRUNCATE_TABLE    VARCHAR2(200);
    P_NOTNULL           VARCHAR(1000);
    N                   NUMBER := 0;
    N_COUNT             NUMBER(10, 0) := 0;
    CREATE_TABLE        VARCHAR(2000);
BEGIN
    P_TRUNCATE_TABLE := 'DELETE TEMP_XUATNHAPTON_NGAY WHERE USERNAME = '''
                        || USERNAME
                        || ''' AND UNITCODE = '''
                        || UNITCODE
                        || '''';
    CREATE_TABLE := ' CREATE GLOBAL TEMPORARY TABLE "ERBUS"."TEMP_XUATNHAPTON_NGAY" 
   (
    "UNITCODE" VARCHAR2(10), 
	"MAKHO" VARCHAR2(50), 
	"MAHANG" VARCHAR2(50),
    "USERNAME" VARCHAR2(50), 
	"GIAVON" NUMBER(18,2) DEFAULT 0, 
	"TONDAUKYSL" NUMBER(18,2) DEFAULT 0, 
	"TONDAUKYGT" NUMBER(18,2) DEFAULT 0, 
	"NHAPSL" NUMBER(18,2) DEFAULT 0, 
	"NHAPGT" NUMBER(18,2) DEFAULT 0, 
	"XUATSL" NUMBER(18,2) DEFAULT 0, 
	"XUATGT" NUMBER(18,2) DEFAULT 0, 
	"TONCUOIKYSL" NUMBER(18,2) DEFAULT 0, 
	"TONCUOIKYGT" NUMBER(18,2) DEFAULT 0
   ) ON COMMIT PRESERVE ROWS'
    ;
    EXECUTE IMMEDIATE 'SELECT COUNT(*) FROM USER_TABLES WHERE TABLE_NAME = ''TEMP_XUATNHAPTON_NGAY'' '
    INTO N_COUNT;
    IF N_COUNT = 0 THEN
        EXECUTE IMMEDIATE CREATE_TABLE;
    END IF;
    EXECUTE IMMEDIATE P_TRUNCATE_TABLE;
    EXECUTE IMMEDIATE 'DELETE TEMP_XUATNHAPTON_NGAY WHERE USERNAME = '''
                      || USERNAME
                      || ''' AND UNITCODE = '''
                      || UNITCODE
                      || '''';
    BEGIN
        FOR CUR_PERIOD IN (
            SELECT
                TO_DATE(TUNGAY, 'dd/MM/yyyy') + LEVEL - 1 AS TODAY
            FROM
                DUAL
            CONNECT BY
                LEVEL <= TO_DATE(DENNGAY, 'dd/MM/yyyy') - TO_DATE(TUNGAY, 'dd/MM/yyyy') + 1
        ) LOOP
            BEGIN
                BEGIN
                    SELECT
                        KYKETOAN,
                        NAM
                    INTO
                        KY_KETTHUC,
                        NAM_KETTHUC
                    FROM
                        KYKETOAN
                    WHERE
                        TO_DATE(DENNGAY, 'dd/MM/yyyy') = TO_DATE(CUR_PERIOD.TODAY, 'dd/MM/yyyy')
                        AND UNITCODE = ''
                                       || UNITCODE
                                       || ''
                        AND TRANGTHAI = 10
                        AND ROWNUM = 1;

                END;

                BEGIN
                    N := N + 1;
                    IF ( N = 1 ) THEN
                        BEGIN
                            P_SQL_INSERT := 'INSERT INTO TEMP_XUATNHAPTON_NGAY
                         (UNITCODE, MAKHO, MAHANG, GIAVON, TONDAUKYSL, TONDAUKYGT, NHAPSL, NHAPGT, XUATSL, XUATGT,USERNAME)
                         SELECT xnt.UNITCODE, xnt.MAKHO, xnt.MAHANG, xnt.GIAVON, xnt.TONDAUKYSL, xnt.TONDAUKYGT AS TONDAUKYGT, xnt.NHAPSL, xnt.NHAPGT AS NHAPGT, xnt.XUATSL, xnt.XUATGT AS XUATGT ,'''
                                            || USERNAME
                                            || ''' AS USERNAME
                         FROM XNT_'
                                            || NAM_KETTHUC
                                            || '_KY_'
                                            || KY_KETTHUC
                                            || ' xnt INNER JOIN MATHANG mathang ON xnt.MAHANG = mathang.MAHANG  AND xnt.UNITCODE = mathang.UNITCODE
                         AND xnt.UNITCODE = '''
                                            || UNITCODE
                                            || '''
                         ';

                            IF TRIM(MAKHO) IS NOT NULL THEN
                                P_SQL_INSERT := P_SQL_INSERT
                                                || ' AND xnt.MAKHO IN (SELECT REGEXP_SUBSTR('''
                                                || MAKHO
                                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                                || MAKHO
                                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';

                            END IF;

                            IF TRIM(MALOAI) IS NOT NULL THEN
                                P_SQL_INSERT := P_SQL_INSERT
                                                || ' AND mathang.MALOAI IN (SELECT REGEXP_SUBSTR('''
                                                || MALOAI
                                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                                || MALOAI
                                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
                            END IF;

                            IF TRIM(MANHOM) IS NOT NULL THEN
                                P_SQL_INSERT := P_SQL_INSERT
                                                || ' AND mathang.MANHOM IN (SELECT REGEXP_SUBSTR('''
                                                || MANHOM
                                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                                || MANHOM
                                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
                            END IF;

                            IF TRIM(MAHANG) IS NOT NULL THEN
                                P_SQL_INSERT := P_SQL_INSERT
                                                || ' AND xnt.MAHANG IN (SELECT REGEXP_SUBSTR('''
                                                || MAHANG
                                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                                || MAHANG
                                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
                            END IF;

                            IF TRIM(MANHACUNGCAP) IS NOT NULL THEN
                                P_SQL_INSERT := P_SQL_INSERT
                                                || ' AND mathang.MANHACUNGCAP IN (SELECT REGEXP_SUBSTR('''
                                                || MANHACUNGCAP
                                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                                || MANHACUNGCAP
                                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
                            END IF;

                            EXECUTE IMMEDIATE P_SQL_INSERT;
                        END;

                    ELSE
                        BEGIN
                            P_SQL_INSERT := ' INSERT INTO TEMP_XUATNHAPTON_NGAY
                         (UNITCODE, MAKHO, MAHANG, GIAVON,NHAPSL, NHAPGT, XUATSL, XUATGT,USERNAME)
                         SELECT xnt.UNITCODE, xnt.MAKHO, xnt.MAHANG, xnt.GIAVON, xnt.NHAPSL, xnt.NHAPGT, xnt.XUATSL, xnt.XUATGT,'''
                                            || USERNAME
                                            || ''' AS USERNAME
                         FROM XNT_'
                                            || NAM_KETTHUC
                                            || '_KY_'
                                            || KY_KETTHUC
                                            || ' xnt INNER JOIN MATHANG mathang ON xnt.MAHANG = mathang.MAHANG AND xnt.UNITCODE = mathang.UNITCODE
                         AND xnt.UNITCODE = '''
                                            || UNITCODE
                                            || '''  ';
                         --

                            IF TRIM(MAKHO) IS NOT NULL THEN
                                P_SQL_INSERT := P_SQL_INSERT
                                                || ' AND xnt.MAKHO IN (SELECT REGEXP_SUBSTR('''
                                                || MAKHO
                                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                                || MAKHO
                                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';

                            END IF;

                            IF TRIM(MALOAI) IS NOT NULL THEN
                                P_SQL_INSERT := P_SQL_INSERT
                                                || ' AND mathang.MALOAI IN (SELECT REGEXP_SUBSTR('''
                                                || MALOAI
                                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                                || MALOAI
                                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
                            END IF;

                            IF TRIM(MANHOM) IS NOT NULL THEN
                                P_SQL_INSERT := P_SQL_INSERT
                                                || ' AND mathang.MANHOM IN (SELECT REGEXP_SUBSTR('''
                                                || MANHOM
                                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                                || MANHOM
                                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
                            END IF;

                            IF TRIM(MAHANG) IS NOT NULL THEN
                                P_SQL_INSERT := P_SQL_INSERT
                                                || ' AND xnt.MAHANG IN (SELECT REGEXP_SUBSTR('''
                                                || MAHANG
                                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                                || MAHANG
                                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
                            END IF;

                            IF TRIM(MANHACUNGCAP) IS NOT NULL THEN
                                P_SQL_INSERT := P_SQL_INSERT
                                                || ' AND mathang.MANHACUNGCAP IN (SELECT REGEXP_SUBSTR('''
                                                || MANHACUNGCAP
                                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                                || MANHACUNGCAP
                                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
                            END IF;

                        --DBMS_OUTPUT.PUT_LINE('P_SQL_INSERT: '||P_SQL_INSERT);

                            EXECUTE IMMEDIATE P_SQL_INSERT;
                        END;
                    END IF;

                END;

            END;
        END LOOP;
        -- TINH TON CUOI KY

        BEGIN
            P_SQL_INSERT := 'INSERT INTO TEMP_XUATNHAPTON_NGAY
                     (UNITCODE, MAKHO, MAHANG, TONCUOIKYSL, TONCUOIKYGT, USERNAME)
                     SELECT xnt.UNITCODE, xnt.MAKHO, xnt.MAHANG, xnt.TONCUOIKYSL, xnt.TONCUOIKYGT AS TONCUOIKYGT,'''
                            || USERNAME
                            || ''' AS USERNAME
                     FROM XNT_'
                            || NAM_KETTHUC
                            || '_KY_'
                            || KY_KETTHUC
                            || ' xnt INNER JOIN MATHANG mathang ON xnt.MAHANG = mathang.MAHANG AND xnt.UNITCODE = mathang.UNITCODE
                    AND xnt.UNITCODE = '''
                            || UNITCODE
                            || '''
                     ';
                     --

            IF TRIM(MAKHO) IS NOT NULL THEN
                P_SQL_INSERT := P_SQL_INSERT
                                || ' AND xnt.MAKHO IN (SELECT REGEXP_SUBSTR('''
                                || MAKHO
                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                || MAKHO
                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';

            END IF;

            IF TRIM(MALOAI) IS NOT NULL THEN
                P_SQL_INSERT := P_SQL_INSERT
                                || ' AND mathang.MALOAI IN (SELECT REGEXP_SUBSTR('''
                                || MALOAI
                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                || MALOAI
                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
            END IF;

            IF TRIM(MANHOM) IS NOT NULL THEN
                P_SQL_INSERT := P_SQL_INSERT
                                || ' AND mathang.MANHOM IN (SELECT REGEXP_SUBSTR('''
                                || MANHOM
                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                || MANHOM
                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
            END IF;

            IF TRIM(MAHANG) IS NOT NULL THEN
                P_SQL_INSERT := P_SQL_INSERT
                                || ' AND xnt.MAHANG IN (SELECT REGEXP_SUBSTR('''
                                || MAHANG
                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                || MAHANG
                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
            END IF;

            IF TRIM(MANHACUNGCAP) IS NOT NULL THEN
                P_SQL_INSERT := P_SQL_INSERT
                                || ' AND mathang.MANHACUNGCAP IN (SELECT REGEXP_SUBSTR('''
                                || MANHACUNGCAP
                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                || MANHACUNGCAP
                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
            END IF;

            EXECUTE IMMEDIATE P_SQL_INSERT;
        EXCEPTION
            WHEN NO_DATA_FOUND THEN
                RAISE;
            WHEN OTHERS THEN
                RAISE;
        END;

        IF DIEUKIEN_NHOM = 'KHOHANG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'temp.MAKHO, khohang.TENKHO,mathang.BARCODE, temp.MAHANG, mathang.TENHANG, temp.UNITCODE';
            P_SELECT_COLUMNS := 'temp.MAKHO AS MACHA, khohang.TENKHO AS TENCHA, mathang.BARCODE AS BARCODE, temp.MAHANG AS MA, mathang.TENHANG AS TEN, temp.UNITCODE';
            P_TABLE_GROUPBY := ' INNER JOIN KHOHANG khohang ON khohang.MAKHO = temp.MAKHO AND temp.UNITCODE = khohang.UNITCODE ';
        END;
    ELSIF DIEUKIEN_NHOM = 'NHOMHANG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.MANHOM, nhomhang.TENNHOM, mathang.BARCODE, temp.MAHANG, mathang.TENHANG, mathang.MANHOM, temp.UNITCODE';
            P_SELECT_COLUMNS := 'mathang.MANHOM AS MACHA, nhomhang.TENNHOM AS TENCHA,mathang.BARCODE AS BARCODE, temp.MAHANG AS MA, mathang.TENHANG AS TEN , temp.UNITCODE';
            P_TABLE_GROUPBY := ' INNER JOIN NHOMHANG nhomhang ON nhomhang.MANHOM = mathang.MANHOM AND temp.UNITCODE = nhomhang.UNITCODE ';
        END;
    ELSIF DIEUKIEN_NHOM = 'LOAIHANG' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.MALOAI, loaihang.TENLOAI, mathang.BARCODE, temp.MAHANG, mathang.TENHANG, mathang.MALOAI, temp.UNITCODE';
            P_SELECT_COLUMNS := 'mathang.MALOAI AS MACHA, loaihang.TENLOAI AS TENCHA, mathang.BARCODE AS BARCODE, temp.MAHANG AS MA, mathang.TENHANG AS TEN, temp.UNITCODE';
            P_TABLE_GROUPBY := ' INNER JOIN LOAIHANG loaihang ON loaihang.MALOAI = mathang.MALOAI AND temp.UNITCODE = loaihang.UNITCODE ';
        END;
    ELSIF DIEUKIEN_NHOM = 'NHACUNGCAP' THEN
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.MANHACUNGCAP, nhacungcap.TENNHACUNGCAP, mathang.BARCODE, temp.MAHANG, mathang.TENHANG, mathang.MANHACUNGCAP, temp.UNITCODE';
            P_SELECT_COLUMNS := 'mathang.MANHACUNGCAP AS MACHA, nhacungcap.TENNHACUNGCAP AS TENCHA, mathang.BARCODE AS BARCODE, temp.MAHANG AS MA, mathang.TENHANG AS TEN, temp.UNITCODE';
            P_TABLE_GROUPBY := ' INNER JOIN NHACUNGCAP nhacungcap ON nhacungcap.MANHACUNGCAP = mathang.MANHACUNGCAP AND temp.UNITCODE = nhacungcap.UNITCODE ';
        END;
    ELSE
        BEGIN
            P_COLUMNS_GROUPBY := 'mathang.BARCODE, temp.MAHANG, mathang.TENHANG, temp.UNITCODE';
            P_SELECT_COLUMNS := 'temp.MAHANG AS MACHA, mathang.TENHANG AS TENCHA, mathang.BARCODE AS BARCODE,temp.MAHANG AS MA, mathang.TENHANG AS TEN, temp.UNITCODE AS UNITCODE';
            P_TABLE_GROUPBY := ' ';
        END;
    END IF;
        P_NOTNULL := ' TONCUOIKYGT <> 0 ';
        QUERY_STR := 'SELECT MACHA,TENCHA,BARCODE,MA,TEN,UNITCODE,TONDAUKYSL,TONDAUKYGT,NHAPSL,NHAPGT,XUATSL,XUATGT,TONCUOIKYSL,TONCUOIKYGT FROM (
                    SELECT '
                     || P_SELECT_COLUMNS
                     || ', 
                                        SUM(temp.TONDAUKYSL)  AS TONDAUKYSL,
                                        SUM(temp.TONDAUKYGT) AS TONDAUKYGT,
                                        SUM(temp.NHAPSL) AS NHAPSL, 
                                        SUM(temp.NHAPGT) AS NHAPGT,
                                        SUM(temp.XUATSL) AS XUATSL, 
                                        SUM(temp.XUATGT) AS XUATGT,
                                        SUM(temp.TONCUOIKYSL) AS TONCUOIKYSL, 
                                        SUM(temp.TONCUOIKYGT) AS TONCUOIKYGT
                                        FROM TEMP_XUATNHAPTON_NGAY temp
                                        INNER JOIN MATHANG mathang ON temp.MAHANG = mathang.MAHANG AND temp.UNITCODE = mathang.UNITCODE AND temp.USERNAME = '''
                     || USERNAME
                     || '''  '
                     || P_TABLE_GROUPBY
                     || ' GROUP BY '
                     || P_COLUMNS_GROUPBY
                     || ' ORDER BY '
                     || P_COLUMNS_GROUPBY
                     || '
                     ) WHERE '||P_NOTNULL||'
                    ';

    END;
    BEGIN
        OPEN CUR FOR QUERY_STR;

    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            DBMS_OUTPUT.PUT_LINE('<your message>' || SQLERRM);
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE(QUERY_STR || SQLERRM);
    END;

END XUATNHAPTON_CHITIET;



/
--------------------------------------------------------
--  DDL for Procedure XUATNHAPTON_TONGHOP
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."XUATNHAPTON_TONGHOP" (
    DIEUKIEN_NHOM   IN              VARCHAR2,
    MAKHO           IN              VARCHAR2,
    MALOAI          IN              VARCHAR2,
    MANHOM          IN              VARCHAR2,
    MAHANG          IN              VARCHAR2,
    MANHACUNGCAP    IN              VARCHAR2,
    UNITCODE        IN              VARCHAR2,
    MANHANVIEN      IN              VARCHAR2,
    USERNAME        IN              VARCHAR2,
    TUNGAY          IN              DATE,
    DENNGAY         IN              DATE,
    CUR             OUT             SYS_REFCURSOR
) AS

    QUERY_STR           VARCHAR(4000);
    KY_KETTHUC          NUMBER(10, 0);
    NAM_KETTHUC         NUMBER(10, 0);
    P_SELECT_COLUMNS    VARCHAR(1000);
    P_TABLE_GROUPBY     VARCHAR(1000);
    P_COLUMNS_GROUPBY   VARCHAR(1000);
    P_SQL_INSERT        VARCHAR(5000);
    P_TRUNCATE_TABLE    VARCHAR2(200);
    P_NOTNULL           VARCHAR(1000);
    N                   NUMBER := 0;
    N_COUNT             NUMBER(10, 0) := 0;
    CREATE_TABLE        VARCHAR(2000);
BEGIN
    P_TRUNCATE_TABLE := 'DELETE TEMP_XUATNHAPTON_NGAY WHERE USERNAME = '''
                        || USERNAME
                        || ''' AND UNITCODE = '''
                        || UNITCODE
                        || '''';
    CREATE_TABLE := ' CREATE GLOBAL TEMPORARY TABLE "ERBUS"."TEMP_XUATNHAPTON_NGAY" 
   (
    "UNITCODE" VARCHAR2(10), 
	"MAKHO" VARCHAR2(50), 
	"MAHANG" VARCHAR2(50),
    "USERNAME" VARCHAR2(50), 
	"GIAVON" NUMBER(18,2) DEFAULT 0, 
	"TONDAUKYSL" NUMBER(18,2) DEFAULT 0, 
	"TONDAUKYGT" NUMBER(18,2) DEFAULT 0, 
	"NHAPSL" NUMBER(18,2) DEFAULT 0, 
	"NHAPGT" NUMBER(18,2) DEFAULT 0, 
	"XUATSL" NUMBER(18,2) DEFAULT 0, 
	"XUATGT" NUMBER(18,2) DEFAULT 0, 
	"TONCUOIKYSL" NUMBER(18,2) DEFAULT 0, 
	"TONCUOIKYGT" NUMBER(18,2) DEFAULT 0
   ) ON COMMIT PRESERVE ROWS'
    ;
    EXECUTE IMMEDIATE 'SELECT COUNT(*) FROM USER_TABLES WHERE TABLE_NAME = ''TEMP_XUATNHAPTON_NGAY'' '
    INTO N_COUNT;
    IF N_COUNT = 0 THEN
        EXECUTE IMMEDIATE CREATE_TABLE;
    END IF;
    EXECUTE IMMEDIATE P_TRUNCATE_TABLE;
    EXECUTE IMMEDIATE 'DELETE TEMP_XUATNHAPTON_NGAY WHERE USERNAME = '''
                      || USERNAME
                      || ''' AND UNITCODE = '''
                      || UNITCODE
                      || '''';
    BEGIN
        FOR CUR_PERIOD IN (
            SELECT
                TO_DATE(TUNGAY, 'dd/MM/yyyy') + LEVEL - 1 AS TODAY
            FROM
                DUAL
            CONNECT BY
                LEVEL <= TO_DATE(DENNGAY, 'dd/MM/yyyy') - TO_DATE(TUNGAY, 'dd/MM/yyyy') + 1
        ) LOOP
            BEGIN
                BEGIN
                    SELECT
                        KYKETOAN,
                        NAM
                    INTO
                        KY_KETTHUC,
                        NAM_KETTHUC
                    FROM
                        KYKETOAN
                    WHERE
                        TO_DATE(DENNGAY, 'dd/MM/yyyy') = TO_DATE(CUR_PERIOD.TODAY, 'dd/MM/yyyy')
                        AND UNITCODE = ''
                                       || UNITCODE
                                       || ''
                        AND TRANGTHAI = 10
                        AND ROWNUM = 1;

                END;

                BEGIN
                    N := N + 1;
                    IF ( N = 1 ) THEN
                        BEGIN
                            P_SQL_INSERT := 'INSERT INTO TEMP_XUATNHAPTON_NGAY
                         (UNITCODE, MAKHO, MAHANG, GIAVON, TONDAUKYSL, TONDAUKYGT, NHAPSL, NHAPGT, XUATSL, XUATGT,USERNAME)
                         SELECT xnt.UNITCODE, xnt.MAKHO, xnt.MAHANG, xnt.GIAVON, xnt.TONDAUKYSL, xnt.TONDAUKYGT AS TONDAUKYGT, xnt.NHAPSL, xnt.NHAPGT AS NHAPGT, xnt.XUATSL, xnt.XUATGT AS XUATGT ,'''
                                            || USERNAME
                                            || ''' AS USERNAME
                         FROM XNT_'
                                            || NAM_KETTHUC
                                            || '_KY_'
                                            || KY_KETTHUC
                                            || ' xnt INNER JOIN MATHANG mathang ON xnt.MAHANG = mathang.MAHANG  AND xnt.UNITCODE = mathang.UNITCODE
                         AND xnt.UNITCODE = '''
                                            || UNITCODE
                                            || '''
                         ';

                            IF TRIM(MAKHO) IS NOT NULL THEN
                                P_SQL_INSERT := P_SQL_INSERT
                                                || ' AND xnt.MAKHO IN (SELECT REGEXP_SUBSTR('''
                                                || MAKHO
                                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                                || MAKHO
                                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';

                            END IF;

                            IF TRIM(MALOAI) IS NOT NULL THEN
                                P_SQL_INSERT := P_SQL_INSERT
                                                || ' AND mathang.MALOAI IN (SELECT REGEXP_SUBSTR('''
                                                || MALOAI
                                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                                || MALOAI
                                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
                            END IF;

                            IF TRIM(MANHOM) IS NOT NULL THEN
                                P_SQL_INSERT := P_SQL_INSERT
                                                || ' AND mathang.MANHOM IN (SELECT REGEXP_SUBSTR('''
                                                || MANHOM
                                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                                || MANHOM
                                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
                            END IF;

                            IF TRIM(MAHANG) IS NOT NULL THEN
                                P_SQL_INSERT := P_SQL_INSERT
                                                || ' AND xnt.MAHANG IN (SELECT REGEXP_SUBSTR('''
                                                || MAHANG
                                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                                || MAHANG
                                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
                            END IF;

                            IF TRIM(MANHACUNGCAP) IS NOT NULL THEN
                                P_SQL_INSERT := P_SQL_INSERT
                                                || ' AND mathang.MANHACUNGCAP IN (SELECT REGEXP_SUBSTR('''
                                                || MANHACUNGCAP
                                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                                || MANHACUNGCAP
                                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
                            END IF;

                            EXECUTE IMMEDIATE P_SQL_INSERT;
                        END;

                    ELSE
                        BEGIN
                            P_SQL_INSERT := ' INSERT INTO TEMP_XUATNHAPTON_NGAY
                         (UNITCODE, MAKHO, MAHANG, GIAVON,NHAPSL, NHAPGT, XUATSL, XUATGT,USERNAME)
                         SELECT xnt.UNITCODE, xnt.MAKHO, xnt.MAHANG, xnt.GIAVON, xnt.NHAPSL, xnt.NHAPGT, xnt.XUATSL, xnt.XUATGT,'''
                                            || USERNAME
                                            || ''' AS USERNAME
                         FROM XNT_'
                                            || NAM_KETTHUC
                                            || '_KY_'
                                            || KY_KETTHUC
                                            || ' xnt INNER JOIN MATHANG mathang ON xnt.MAHANG = mathang.MAHANG AND xnt.UNITCODE = mathang.UNITCODE
                         AND xnt.UNITCODE = '''
                                            || UNITCODE
                                            || '''  ';
                         --

                            IF TRIM(MAKHO) IS NOT NULL THEN
                                P_SQL_INSERT := P_SQL_INSERT
                                                || ' AND xnt.MAKHO IN (SELECT REGEXP_SUBSTR('''
                                                || MAKHO
                                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                                || MAKHO
                                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';

                            END IF;

                            IF TRIM(MALOAI) IS NOT NULL THEN
                                P_SQL_INSERT := P_SQL_INSERT
                                                || ' AND mathang.MALOAI IN (SELECT REGEXP_SUBSTR('''
                                                || MALOAI
                                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                                || MALOAI
                                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
                            END IF;

                            IF TRIM(MANHOM) IS NOT NULL THEN
                                P_SQL_INSERT := P_SQL_INSERT
                                                || ' AND mathang.MANHOM IN (SELECT REGEXP_SUBSTR('''
                                                || MANHOM
                                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                                || MANHOM
                                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
                            END IF;

                            IF TRIM(MAHANG) IS NOT NULL THEN
                                P_SQL_INSERT := P_SQL_INSERT
                                                || ' AND xnt.MAHANG IN (SELECT REGEXP_SUBSTR('''
                                                || MAHANG
                                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                                || MAHANG
                                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
                            END IF;

                            IF TRIM(MANHACUNGCAP) IS NOT NULL THEN
                                P_SQL_INSERT := P_SQL_INSERT
                                                || ' AND mathang.MANHACUNGCAP IN (SELECT REGEXP_SUBSTR('''
                                                || MANHACUNGCAP
                                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                                || MANHACUNGCAP
                                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
                            END IF;

                        --DBMS_OUTPUT.PUT_LINE('P_SQL_INSERT: '||P_SQL_INSERT);

                            EXECUTE IMMEDIATE P_SQL_INSERT;
                        END;
                    END IF;

                END;

            END;
        END LOOP;
        -- TINH TON CUOI KY

        BEGIN
            P_SQL_INSERT := 'INSERT INTO TEMP_XUATNHAPTON_NGAY
                     (UNITCODE, MAKHO, MAHANG, TONCUOIKYSL, TONCUOIKYGT, USERNAME)
                     SELECT xnt.UNITCODE, xnt.MAKHO, xnt.MAHANG, xnt.TONCUOIKYSL, xnt.TONCUOIKYGT AS TONCUOIKYGT,'''
                            || USERNAME
                            || ''' AS USERNAME
                     FROM XNT_'
                            || NAM_KETTHUC
                            || '_KY_'
                            || KY_KETTHUC
                            || ' xnt INNER JOIN MATHANG mathang ON xnt.MAHANG = mathang.MAHANG AND xnt.UNITCODE = mathang.UNITCODE
                    AND xnt.UNITCODE = '''
                            || UNITCODE
                            || '''
                     ';
                     --

            IF TRIM(MAKHO) IS NOT NULL THEN
                P_SQL_INSERT := P_SQL_INSERT
                                || ' AND xnt.MAKHO IN (SELECT REGEXP_SUBSTR('''
                                || MAKHO
                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                || MAKHO
                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';

            END IF;

            IF TRIM(MALOAI) IS NOT NULL THEN
                P_SQL_INSERT := P_SQL_INSERT
                                || ' AND mathang.MALOAI IN (SELECT REGEXP_SUBSTR('''
                                || MALOAI
                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                || MALOAI
                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
            END IF;

            IF TRIM(MANHOM) IS NOT NULL THEN
                P_SQL_INSERT := P_SQL_INSERT
                                || ' AND mathang.MANHOM IN (SELECT REGEXP_SUBSTR('''
                                || MANHOM
                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                || MANHOM
                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
            END IF;

            IF TRIM(MAHANG) IS NOT NULL THEN
                P_SQL_INSERT := P_SQL_INSERT
                                || ' AND xnt.MAHANG IN (SELECT REGEXP_SUBSTR('''
                                || MAHANG
                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                || MAHANG
                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
            END IF;

            IF TRIM(MANHACUNGCAP) IS NOT NULL THEN
                P_SQL_INSERT := P_SQL_INSERT
                                || ' AND mathang.MANHACUNGCAP IN (SELECT REGEXP_SUBSTR('''
                                || MANHACUNGCAP
                                || ''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''
                                || MANHACUNGCAP
                                || ''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
            END IF;

            EXECUTE IMMEDIATE P_SQL_INSERT;
        EXCEPTION
            WHEN NO_DATA_FOUND THEN
                RAISE;
            WHEN OTHERS THEN
                RAISE;
        END;

        IF DIEUKIEN_NHOM = 'KHOHANG' THEN
            BEGIN
                P_COLUMNS_GROUPBY := 'temp.MAKHO,khohang.TENKHO, temp.UNITCODE';
                P_SELECT_COLUMNS := ' temp.MAKHO AS MA,khohang.TENKHO AS TEN,temp.UNITCODE ';
                P_TABLE_GROUPBY := ' INNER JOIN KHOHANG khohang ON temp.MAKHO = khohang.MAKHO AND temp.UNITCODE = khohang.UNITCODE';
                P_NOTNULL := ' AND temp.MAKHO IS NOT NULL';
            END;
        ELSIF DIEUKIEN_NHOM = 'LOAIHANG' THEN
            BEGIN
                P_COLUMNS_GROUPBY := 'mathang.MALOAI, loaihang.TENLOAI, temp.UNITCODE';
                P_SELECT_COLUMNS := ' mathang.MALOAI AS MA,loaihang.TENLOAI AS TEN, temp.UNITCODE ';
                P_TABLE_GROUPBY := ' INNER JOIN LOAIHANG loaihang ON mathang.MALOAI = loaihang.MALOAI AND temp.UNITCODE = loaihang.UNITCODE';
                P_NOTNULL := ' AND mathang.MALOAI IS NOT NULL';
            END;
        ELSIF DIEUKIEN_NHOM = 'NHOMHANG' THEN
            BEGIN
                P_COLUMNS_GROUPBY := 'mathang.MANHOM, nhomhang.TENNHOM, temp.UNITCODE';
                P_SELECT_COLUMNS := ' mathang.MANHOM AS MA,nhomhang.TENNHOM AS TEN, temp.UNITCODE ';
                P_TABLE_GROUPBY := ' INNER JOIN NHOMHANG nhomhang ON mathang.MANHOM = nhomhang.MANHOM AND temp.UNITCODE = nhomhang.UNITCODE';
                P_NOTNULL := ' AND mathang.MANHOM IS NOT NULL';
            END;
        ELSIF DIEUKIEN_NHOM = 'NHACUNGCAP' THEN
            BEGIN
                P_COLUMNS_GROUPBY := 'mathang.MANHACUNGCAP, nhacungcap.TENNHACUNGCAP, temp.UNITCODE';
                P_SELECT_COLUMNS := ' mathang.MANHACUNGCAP AS MA,nhacungcap.TENNHACUNGCAP AS TEN, temp.UNITCODE ';
                P_TABLE_GROUPBY := '  INNER JOIN NHACUNGCAP nhacungcap ON mathang.MANHACUNGCAP = nhacungcap.MANHACUNGCAP AND temp.UNITCODE = nhacungcap.UNITCODE';
                P_NOTNULL := ' AND mathang.MANHACUNGCAP IS NOT NULL';
            END;
        ELSE
            BEGIN
                P_COLUMNS_GROUPBY := 'temp.MAHANG,mathang.TENHANG,temp.UNITCODE';
                P_SELECT_COLUMNS := ' temp.MAHANG AS MA, mathang.TENHANG AS TEN, temp.UNITCODE ';
                P_TABLE_GROUPBY := ' ';
                P_NOTNULL := ' AND temp.MAHANG IS NOT NULL';
            END;
        END IF;

        QUERY_STR := 'SELECT '
                     || P_SELECT_COLUMNS
                     || ', 
                                        SUM(temp.TONDAUKYSL)  AS TONDAUKYSL,
                                        SUM(temp.TONDAUKYGT) AS TONDAUKYGT,
                                        SUM(temp.NHAPSL) AS NHAPSL, 
                                        SUM(temp.NHAPGT) AS NHAPGT,
                                        SUM(temp.XUATSL) AS XUATSL, 
                                        SUM(temp.XUATGT) AS XUATGT,
                                        SUM(temp.TONCUOIKYSL) AS TONCUOIKYSL, 
                                        SUM(temp.TONCUOIKYGT) AS TONCUOIKYGT
                                        FROM TEMP_XUATNHAPTON_NGAY temp
                                        INNER JOIN MATHANG mathang ON temp.MAHANG = mathang.MAHANG AND temp.UNITCODE = mathang.UNITCODE AND temp.USERNAME = '''
                     || USERNAME
                     || '''  '
                     || P_TABLE_GROUPBY
                     || '  '
                     || P_NOTNULL
                     || ' GROUP BY '
                     || P_COLUMNS_GROUPBY
                     || ' ORDER BY '
                     || P_COLUMNS_GROUPBY
                     || '
                    ';

    END;

    BEGIN
        OPEN CUR FOR QUERY_STR;

    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            DBMS_OUTPUT.PUT_LINE('<your message>' || SQLERRM);
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE(QUERY_STR || SQLERRM);
    END;

END XUATNHAPTON_TONGHOP;



/
--------------------------------------------------------
--  DDL for Procedure XUATNHAPTON_TONKHO_CHITIET
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."XUATNHAPTON_TONKHO_CHITIET" (TABLE_NAME IN VARCHAR2,DIEUKIEN_NHOM IN VARCHAR2,MAKHO IN VARCHAR2,MALOAI IN VARCHAR2,MANHOM IN VARCHAR2,MANHACUNGCAP IN VARCHAR2,MAHANG IN VARCHAR2,UNITCODE IN VARCHAR2,USERNAME IN VARCHAR2, CUR_OUT OUT SYS_REFCURSOR) AS
    QUERY_STR VARCHAR(2000);
    QUERY_SELECT VARCHAR(500);
    QUERY_WHERE_IN VARCHAR(500) := '';
    QUERY_GROUPBY VARCHAR(500) := '';
    QUERY_ORDERBY VARCHAR(200) := '';
    TABLE_JOIN VARCHAR(500) := '';
BEGIN
    IF MAKHO IS NOT NULL OR MAKHO != '' THEN
        QUERY_WHERE_IN := QUERY_WHERE_IN || ' AND xnt.MAKHO IN (SELECT REGEXP_SUBSTR('''||MAKHO||''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''||MAKHO||''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
    END IF;
    IF MALOAI IS NOT NULL OR MALOAI != '' THEN
        QUERY_WHERE_IN := QUERY_WHERE_IN || ' AND loaihang.MALOAI IN (SELECT REGEXP_SUBSTR('''||MALOAI||''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''||MALOAI||''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
    END IF;
    IF MANHOM IS NOT NULL OR MANHOM != '' THEN
        QUERY_WHERE_IN := QUERY_WHERE_IN || ' AND nhomhang.MANHOM IN (SELECT REGEXP_SUBSTR('''||MANHOM||''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''||MANHOM||''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
    END IF;
    IF MANHACUNGCAP IS NOT NULL OR MANHACUNGCAP != '' THEN
        QUERY_WHERE_IN := QUERY_WHERE_IN || ' AND mathang.MANHACUNGCAP IN (SELECT REGEXP_SUBSTR('''||MANHACUNGCAP||''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''||MANHACUNGCAP||''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
    END IF;
    IF MAHANG IS NOT NULL OR MAHANG != '' THEN
        QUERY_WHERE_IN := QUERY_WHERE_IN || ' AND xnt.MAHANG IN (SELECT REGEXP_SUBSTR('''||MAHANG||''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''||MAHANG||''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
    END IF;

    IF DIEUKIEN_NHOM = 'KHOHANG' THEN
        QUERY_SELECT := QUERY_SELECT || ' xnt.MAKHO AS MA,khohang.TENKHO AS TEN, xnt.MAHANG AS MACON, mathang.TENHANG AS TENCON, xnt.GIAVON, xnt.TONCUOIKYSL, xnt.TONCUOIKYGT, mathang_gia.GIAMUA, mathang_gia.GIABANLE   ';
        TABLE_JOIN := ' INNER JOIN KHOHANG khohang ON xnt.MAKHO = khohang.MAKHO INNER JOIN MATHANG mathang ON xnt.MAHANG = mathang.MAHANG INNER JOIN MATHANG_GIA mathang_gia ON mathang.MAHANG = mathang_gia.MAHANG 
        INNER JOIN LOAIHANG loaihang ON mathang.MALOAI = loaihang.MALOAI INNER JOIN NHACUNGCAP nhacungcap ON mathang.MANHACUNGCAP = nhacungcap.MANHACUNGCAP INNER JOIN NHOMHANG nhomhang ON mathang.MANHOM = nhomhang.MANHOM ';
        QUERY_GROUPBY := QUERY_GROUPBY || ' GROUP BY xnt.MAKHO,khohang.TENKHO, xnt.MAHANG, mathang.TENHANG ,xnt.GIAVON, xnt.TONCUOIKYSL, xnt.TONCUOIKYGT, mathang_gia.GIAMUA, mathang_gia.GIABANLE' ;
        QUERY_ORDERBY := ' ORDER BY xnt.MAKHO,xnt.MAHANG';
    ELSIF DIEUKIEN_NHOM = 'LOAIHANG' THEN
        QUERY_SELECT := QUERY_SELECT || ' mathang.MALOAI AS MA,loaihang.TENLOAI AS TEN, xnt.MAHANG AS MACON, mathang.TENHANG AS TENCON, xnt.GIAVON, xnt.TONCUOIKYSL, xnt.TONCUOIKYGT, mathang_gia.GIAMUA, mathang_gia.GIABANLE   ';
        TABLE_JOIN := ' INNER JOIN MATHANG mathang ON xnt.MAHANG = mathang.MAHANG INNER JOIN LOAIHANG loaihang ON mathang.MALOAI = loaihang.MALOAI INNER JOIN MATHANG_GIA mathang_gia ON mathang.MAHANG = mathang_gia.MAHANG
        INNER JOIN KHOHANG khohang ON xnt.MAKHO = khohang.MAKHO INNER JOIN NHACUNGCAP nhacungcap ON mathang.MANHACUNGCAP = nhacungcap.MANHACUNGCAP INNER JOIN NHOMHANG nhomhang ON mathang.MANHOM = nhomhang.MANHOM  ';
        QUERY_GROUPBY := QUERY_GROUPBY || ' GROUP BY mathang.MALOAI,loaihang.TENLOAI, xnt.MAHANG, mathang.TENHANG ,xnt.GIAVON, xnt.TONCUOIKYSL, xnt.TONCUOIKYGT, mathang_gia.GIAMUA, mathang_gia.GIABANLE' ;
        QUERY_ORDERBY := ' ORDER BY mathang.MALOAI,xnt.MAHANG';
    ELSIF DIEUKIEN_NHOM = 'NHOMHANG' THEN
        QUERY_SELECT := QUERY_SELECT || ' mathang.MANHOM AS MA,nhomhang.TENNHOM AS TEN, xnt.MAHANG AS MACON, mathang.TENHANG AS TENCON, xnt.GIAVON, xnt.TONCUOIKYSL, xnt.TONCUOIKYGT, mathang_gia.GIAMUA, mathang_gia.GIABANLE ';
        TABLE_JOIN := ' INNER JOIN MATHANG mathang ON xnt.MAHANG = mathang.MAHANG INNER JOIN NHOMHANG nhomhang ON mathang.MANHOM = nhomhang.MANHOM INNER JOIN MATHANG_GIA mathang_gia ON mathang.MAHANG = mathang_gia.MAHANG
        INNER JOIN KHOHANG khohang ON xnt.MAKHO = khohang.MAKHO INNER JOIN NHACUNGCAP nhacungcap ON mathang.MANHACUNGCAP = nhacungcap.MANHACUNGCAP INNER JOIN LOAIHANG loaihang ON mathang.MALOAI = loaihang.MALOAI ';
        QUERY_GROUPBY := QUERY_GROUPBY || ' GROUP BY mathang.MANHOM,nhomhang.TENNHOM, xnt.MAHANG, mathang.TENHANG, xnt.GIAVON, xnt.TONCUOIKYSL, xnt.TONCUOIKYGT, mathang_gia.GIAMUA, mathang_gia.GIABANLE' ;
        QUERY_ORDERBY := ' ORDER BY mathang.MANHOM,xnt.MAHANG';
    ELSIF DIEUKIEN_NHOM = 'NHACUNGCAP' THEN
        QUERY_SELECT := QUERY_SELECT || ' mathang.MANHACUNGCAP AS MA,nhacungcap.TENNHACUNGCAP AS TEN, xnt.MAHANG AS MACON, mathang.TENHANG AS TENCON, xnt.GIAVON, xnt.TONCUOIKYSL, xnt.TONCUOIKYGT, mathang_gia.GIAMUA, mathang_gia.GIABANLE ';
        TABLE_JOIN := ' INNER JOIN MATHANG mathang ON xnt.MAHANG = mathang.MAHANG INNER JOIN NHACUNGCAP nhacungcap ON mathang.MANHACUNGCAP = nhacungcap.MANHACUNGCAP INNER JOIN MATHANG_GIA mathang_gia ON mathang.MAHANG = mathang_gia.MAHANG
        INNER JOIN KHOHANG khohang ON xnt.MAKHO = khohang.MAKHO INNER JOIN LOAIHANG loaihang ON mathang.MALOAI = loaihang.MALOAI INNER JOIN NHOMHANG nhomhang ON mathang.MANHOM = nhomhang.MANHOM ';
        QUERY_GROUPBY := QUERY_GROUPBY || ' GROUP BY mathang.MANHACUNGCAP,nhacungcap.TENNHACUNGCAP, xnt.MAHANG, mathang.TENHANG ,xnt.GIAVON, xnt.TONCUOIKYSL, xnt.TONCUOIKYGT, mathang_gia.GIAMUA, mathang_gia.GIABANLE' ;
        QUERY_ORDERBY := ' ORDER BY mathang.MANHACUNGCAP,xnt.MAHANG';
    ELSIF DIEUKIEN_NHOM = 'MATHANG' THEN
        QUERY_SELECT := QUERY_SELECT || ' xnt.MAHANG AS MA,mathang.TENHANG AS TEN, xnt.MAKHO AS MACON, khohang.TENKHO AS TENCON, xnt.GIAVON, xnt.TONCUOIKYSL, xnt.TONCUOIKYGT, mathang_gia.GIAMUA, mathang_gia.GIABANLE ';
        TABLE_JOIN := ' INNER JOIN MATHANG mathang ON xnt.MAHANG = mathang.MAHANG INNER JOIN MATHANG_GIA mathang_gia ON mathang.MAHANG = mathang_gia.MAHANG INNER JOIN KHOHANG khohang ON xnt.MAKHO = khohang.MAKHO';
        QUERY_GROUPBY := QUERY_GROUPBY || ' GROUP BY xnt.MAHANG, mathang.TENHANG, xnt.MAKHO, khohang.TENKHO, xnt.GIAVON, xnt.TONCUOIKYSL, xnt.TONCUOIKYGT, mathang_gia.GIAMUA, mathang_gia.GIABANLE ' ;
        QUERY_ORDERBY := ' ORDER BY xnt.MAHANG,xnt.MAKHO';
    ELSE
        QUERY_GROUPBY := QUERY_GROUPBY || ' 1 AND 1 ';
    END IF;

    QUERY_STR := 'SELECT '||QUERY_SELECT||' 
    FROM '||TABLE_NAME||' xnt '||TABLE_JOIN||' '||QUERY_WHERE_IN||'
    WHERE xnt.UNITCODE = '''||UNITCODE||''' '||QUERY_GROUPBY || QUERY_ORDERBY ||'';
    BEGIN
    --DBMS_OUTPUT.put_line (QUERY_STR);  
    OPEN CUR_OUT FOR QUERY_STR;
    EXCEPTION
                WHEN NO_DATA_FOUND THEN
                 NULL;
                   WHEN OTHERS THEN
          NULL;
    END;
END XUATNHAPTON_TONKHO_CHITIET;


/
--------------------------------------------------------
--  DDL for Procedure XUATNHAPTON_TONKHO_TONGHOP
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ERBUS"."XUATNHAPTON_TONKHO_TONGHOP" (TABLE_NAME IN VARCHAR2,DIEUKIEN_NHOM IN VARCHAR2,MAKHO IN VARCHAR2,MALOAI IN VARCHAR2,MANHOM IN VARCHAR2,MANHACUNGCAP IN VARCHAR2,MAHANG IN VARCHAR2,UNITCODE IN VARCHAR2,USERNAME IN VARCHAR2, CUR_OUT OUT SYS_REFCURSOR) AS
    QUERY_STR VARCHAR(2000);
    QUERY_SELECT VARCHAR(500);
    QUERY_WHERE_IN VARCHAR(500) := '';
    QUERY_GROUPBY VARCHAR(500) := '';
    QUERY_ORDERBY VARCHAR(200) := '';
    TABLE_JOIN VARCHAR(500) := '';
BEGIN
    IF MAKHO IS NOT NULL OR MAKHO != '' THEN
        QUERY_WHERE_IN := QUERY_WHERE_IN || ' AND xnt.MAKHO IN (SELECT REGEXP_SUBSTR('''||MAKHO||''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''||MAKHO||''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
    END IF;
    IF MALOAI IS NOT NULL OR MALOAI != '' THEN
        QUERY_WHERE_IN := QUERY_WHERE_IN || ' AND loaihang.MALOAI IN (SELECT REGEXP_SUBSTR('''||MALOAI||''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''||MALOAI||''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
    END IF;
    IF MANHOM IS NOT NULL OR MANHOM != '' THEN
        QUERY_WHERE_IN := QUERY_WHERE_IN || ' AND nhomhang.MANHOM IN (SELECT REGEXP_SUBSTR('''||MANHOM||''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''||MANHOM||''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
    END IF;
    IF MANHACUNGCAP IS NOT NULL OR MANHACUNGCAP != '' THEN
        QUERY_WHERE_IN := QUERY_WHERE_IN || ' AND mathang.MANHACUNGCAP IN (SELECT REGEXP_SUBSTR('''||MANHACUNGCAP||''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''||MANHACUNGCAP||''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
    END IF;
    IF MAHANG IS NOT NULL OR MAHANG != '' THEN
        QUERY_WHERE_IN := QUERY_WHERE_IN || ' AND xnt.MAHANG IN (SELECT REGEXP_SUBSTR('''||MAHANG||''',''[^,]+'',1,LEVEL) FROM DUAL CONNECT BY REGEXP_SUBSTR('''||MAHANG||''',''[^,]+'' ,1,LEVEL) IS NOT NULL)';
    END IF;

    IF DIEUKIEN_NHOM = 'KHOHANG' THEN
        QUERY_SELECT := QUERY_SELECT || ' xnt.MAKHO AS MA,khohang.TENKHO AS TEN ';
        TABLE_JOIN := ' INNER JOIN KHOHANG khohang ON xnt.MAKHO = khohang.MAKHO ';
        QUERY_GROUPBY := QUERY_GROUPBY || ' GROUP BY xnt.MAKHO,khohang.TENKHO' ;
        QUERY_ORDERBY := ' ORDER BY xnt.MAKHO';
    ELSIF DIEUKIEN_NHOM = 'LOAIHANG' THEN
        QUERY_SELECT := QUERY_SELECT || ' mathang.MALOAI AS MA,loaihang.TENLOAI AS TEN ';
        TABLE_JOIN := ' INNER JOIN MATHANG mathang ON xnt.MAHANG = mathang.MAHANG INNER JOIN LOAIHANG loaihang ON mathang.MALOAI = loaihang.MALOAI ';
        QUERY_GROUPBY := QUERY_GROUPBY || ' GROUP BY mathang.MALOAI,loaihang.TENLOAI' ;
        QUERY_ORDERBY := ' ORDER BY mathang.MALOAI';
    ELSIF DIEUKIEN_NHOM = 'NHOMHANG' THEN
        QUERY_SELECT := QUERY_SELECT || ' mathang.MANHOM AS MA,nhomhang.TENNHOM AS TEN ';
        TABLE_JOIN := ' INNER JOIN MATHANG mathang ON xnt.MAHANG = mathang.MAHANG INNER JOIN NHOMHANG nhomhang ON mathang.MANHOM = nhomhang.MANHOM ';
        QUERY_GROUPBY := QUERY_GROUPBY || ' GROUP BY nhomhang.MANHOM,nhomhang.TENNHOM' ;
        QUERY_ORDERBY := ' ORDER BY mathang.MANHOM';
    ELSIF DIEUKIEN_NHOM = 'NHACUNGCAP' THEN
        QUERY_SELECT := QUERY_SELECT || ' mathang.MANHACUNGCAP AS MA,nhacungcap.TENNHACUNGCAP AS TEN ';
        TABLE_JOIN := ' INNER JOIN MATHANG mathang ON xnt.MAHANG = mathang.MAHANG INNER JOIN NHACUNGCAP nhacungcap ON mathang.MANHACUNGCAP = nhacungcap.MANHACUNGCAP ';
        QUERY_GROUPBY := QUERY_GROUPBY || ' GROUP BY mathang.MANHACUNGCAP,nhacungcap.TENNHACUNGCAP' ;
        QUERY_ORDERBY := ' ORDER BY mathang.MANHACUNGCAP';
    ELSIF DIEUKIEN_NHOM = 'MATHANG' THEN
        QUERY_SELECT := QUERY_SELECT || ' xnt.MAHANG AS MA,mathang.TENHANG AS TEN ';
        TABLE_JOIN := ' INNER JOIN MATHANG mathang ON xnt.MAHANG = mathang.MAHANG ';
        QUERY_GROUPBY := QUERY_GROUPBY || ' GROUP BY xnt.MAHANG,mathang.TENHANG ' ;
        QUERY_ORDERBY := ' ORDER BY xnt.MAHANG';
    ELSE
        QUERY_GROUPBY := QUERY_GROUPBY || ' 1 AND 1 ';
    END IF;

    QUERY_STR := 'SELECT '||QUERY_SELECT||' ,ROUND(SUM(xnt.TONCUOIKYSL), 2) AS SOLUONG, ROUND(SUM(xnt.TONCUOIKYGT), 2) AS GIATRI 
    FROM '||TABLE_NAME||' xnt '||TABLE_JOIN||' '||QUERY_WHERE_IN||'
    WHERE xnt.UNITCODE = '''||UNITCODE||''' '||QUERY_GROUPBY || QUERY_ORDERBY ||'';
    BEGIN
    --DBMS_OUTPUT.put_line (QUERY_STR);  
    OPEN CUR_OUT FOR QUERY_STR;
    EXCEPTION
                WHEN NO_DATA_FOUND THEN
                 NULL;
                   WHEN OTHERS THEN
          NULL;
    END;
END XUATNHAPTON_TONKHO_TONGHOP;

/
--------------------------------------------------------
--  DDL for Package XUATNHAPTON
--------------------------------------------------------

  CREATE OR REPLACE PACKAGE "ERBUS"."XUATNHAPTON" AS 
PROCEDURE XNT_CREATE_TABLE_TONKY (
        P_TABLENAME_KYTRUOC   IN                    VARCHAR2,
        P_TABLENAME           IN                    VARCHAR2,
        P_UNITCODE            IN                    VARCHAR2,
        P_NAM                 IN                    NUMBER,
        P_KY                  IN                    NUMBER
    );
PROCEDURE XNT_TANG_TONKY (
        P_TABLENAME    IN             VARCHAR2,
        P_UNITCODE     IN             VARCHAR2,
        P_NAM          IN             NUMBER,
        P_KY           IN             NUMBER,
        P_MA_NHAPXUAT  IN            VARCHAR2,
        P_TUNGAY       DATE,
        P_DENNGAY      DATE
    );
PROCEDURE XNT_GIAM_TONKY (
        P_TABLENAME    IN             VARCHAR2,
        P_UNITCODE      IN            VARCHAR2,
        P_NAM          IN             NUMBER,
        P_KY           IN             NUMBER,
        P_MA_NHAPXUAT   IN            VARCHAR2,
        P_TUNGAY       DATE,
        P_DENNGAY      DATE
    );
    
PROCEDURE XNT_GIAM_TONKY_DATPHONG (
        P_TABLENAME    IN             VARCHAR2,
        P_UNITCODE      IN            VARCHAR2,
        P_NAM          IN             NUMBER,
        P_KY           IN             NUMBER,
        P_MA_NHAPXUAT   IN            VARCHAR2,
        P_TUNGAY       DATE,
        P_DENNGAY      DATE
    );
    
PROCEDURE XNT_KHOASO (
        P_TABLENAME_KYTRUOC   IN                    VARCHAR2,
        P_TABLENAME           IN                    VARCHAR2,
        P_UNITCODE            IN                    VARCHAR2,
        P_NAM                 IN                    NUMBER,
        P_KY                  IN                    NUMBER
    );
PROCEDURE XNT_TANG_PHIEU (
        P_TABLENAME   IN            VARCHAR2,
        P_NAM         IN            NUMBER,
        P_KY          IN            NUMBER,
        P_ID          IN            VARCHAR2
    );
PROCEDURE XNT_GIAM_PHIEU (
        P_TABLENAME   IN            VARCHAR2,
        P_NAM         IN            NUMBER,
        P_KY          IN            NUMBER,
        P_ID          IN            VARCHAR2
    );
PROCEDURE XNT_THANHTOAN_DATPHONG (
    P_TABLENAME   IN            VARCHAR2,
    P_NAM         IN            NUMBER,
    P_KY          IN            NUMBER,
    P_ID          IN            VARCHAR2
);
END XUATNHAPTON;

/
--------------------------------------------------------
--  DDL for Package Body XUATNHAPTON
--------------------------------------------------------

  CREATE OR REPLACE PACKAGE BODY "ERBUS"."XUATNHAPTON" AS
    PROCEDURE XNT_CREATE_TABLE_TONKY (
        P_TABLENAME_KYTRUOC   IN                    VARCHAR2,
        P_TABLENAME           IN                    VARCHAR2,
        P_UNITCODE            IN                    VARCHAR2,
        P_NAM                 IN                    NUMBER,
        P_KY                  IN                    NUMBER
    ) IS

        P_CREATE_TABLE   VARCHAR2(2000);
        P_SQL_INSERT     VARCHAR2(1000);
        P_SQL_DELETE     VARCHAR2(200);
        N_COUNT          NUMBER(10, 0) := 0;
    BEGIN
        P_CREATE_TABLE := 'CREATE TABLE '
                 || P_TABLENAME
                 || '(
    "UNITCODE" NVARCHAR2(10), 
    "NAM" NUMBER(10,0),
    "KY" NUMBER(10,0),
    "MAKHO" NVARCHAR2(50),
    "MAHANG" NVARCHAR2(50),
    "GIAVON" NUMBER(18,2) DEFAULT 0,
    "TONDAUKYSL" NUMBER(18,2) DEFAULT 0,
    "TONDAUKYGT" NUMBER(18,2) DEFAULT 0,
    "NHAPSL" NUMBER(18,2) DEFAULT 0,
    "NHAPGT" NUMBER(18,2) DEFAULT 0,
    "XUATSL" NUMBER(18,2) DEFAULT 0,
    "XUATGT" NUMBER(18,2) DEFAULT 0,
    "TONCUOIKYSL" NUMBER(18,2) DEFAULT 0, 
    "TONCUOIKYGT" NUMBER(18,2) DEFAULT 0
    ) SEGMENT CREATION IMMEDIATE PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 NOCOMPRESS LOGGING STORAGE
    (INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645 PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)';
--    TỒN CUỐI KỲ CỦA KỲ TRƯỚC ĐẨY LÊN TỒN ĐẦU KỲ CỦA KỲ SAU
        P_SQL_INSERT := 'INSERT INTO '
                        || P_TABLENAME
                        || ' a
                        (
                         a.UNITCODE, a.NAM, a.KY, a.MAKHO, a.MAHANG, a.GIAVON, 
                         a.TONDAUKYSL, a.TONDAUKYGT,
                         a.TONCUOIKYSL, a.TONCUOIKYGT
                        )
                        SELECT b.UNITCODE, '
                        || P_NAM
                        || ', '
                        || P_KY
                        || ', b.MAKHO, b.MAHANG, b.GIAVON, 
                         b.TONCUOIKYSL ,b.TONCUOIKYGT, 
                         b.TONCUOIKYSL ,b.TONCUOIKYGT
                        FROM '
                        || P_TABLENAME_KYTRUOC
                        || ' b WHERE b.UNITCODE = '''
                        || P_UNITCODE
                        || '''';

        IF TRIM(P_TABLENAME_KYTRUOC) IS NULL THEN
            P_SQL_INSERT := 'DECLARE CURSOR CUR_KHO IS SELECT MAKHO FROM KHOHANG WHERE UNITCODE='''
                            || P_UNITCODE
                            || ''';
        BEGIN FOR ROW_KHO IN CUR_KHO LOOP
         BEGIN 
            INSERT INTO '
                            || P_TABLENAME
                            || '(UNITCODE,NAM, KY, MAKHO, MAHANG,GIAVON,TONDAUKYSL,TONDAUKYGT,NHAPSL,NHAPGT,XUATSL,XUATGT,TONCUOIKYSL,TONCUOIKYGT) 
             SELECT '''
                            || P_UNITCODE
                            || ''' AS UNITCODE,'
                            || P_NAM
                            || ' AS NAM,'
                            || P_KY
                            || ' AS KY,
                            ROW_KHO.MAKHO AS MAKHO,
                            MAHANG, 0 AS GIAVON, 0 AS TONDAUKYSL, 0 AS TONDAUKYGT, 0 AS NHAPSL, 0 AS NHAPGT,
                            0 AS XUATSL, 0 AS XUATGT, 0 AS TONCUOIKYSL, 0 AS TONCUOIKYGT
                            FROM MATHANG WHERE UNITCODE = '''||P_UNITCODE||''';
          END;
         END LOOP;
        END;';
        END IF;
       --DBMS_OUTPUT.PUT_LINE(P_SQL_INSERT);  
        BEGIN
            SELECT COUNT(*) INTO N_COUNT
            FROM USER_TABLES
            WHERE TABLE_NAME = UPPER(P_TABLENAME);
        EXCEPTION
            WHEN OTHERS THEN
                N_COUNT := 0;
        END;

        IF ( N_COUNT IS NULL OR N_COUNT <= 0 ) THEN
            BEGIN
                EXECUTE IMMEDIATE P_CREATE_TABLE;
            END;
        END IF;

        BEGIN
            P_SQL_DELETE := 'DELETE FROM '
                            || P_TABLENAME
                            || ' WHERE UNITCODE='''
                            || P_UNITCODE
                            || '''';
            EXECUTE IMMEDIATE P_SQL_DELETE;
            EXECUTE IMMEDIATE P_SQL_INSERT;
            COMMIT;
        END;

    END XNT_CREATE_TABLE_TONKY;

    PROCEDURE XNT_TANG_TONKY (
        P_TABLENAME    IN             VARCHAR2,
        P_UNITCODE     IN             VARCHAR2,
        P_NAM          IN             NUMBER,
        P_KY           IN             NUMBER,
        P_MA_NHAPXUAT  IN            VARCHAR2,
        P_TUNGAY       DATE,
        P_DENNGAY      DATE
    ) IS
        N_SQL_INSERT    VARCHAR(5000);
        STR_IF_REFUND   VARCHAR(3000);
    BEGIN
        IF P_MA_NHAPXUAT = 'XBAN_TRALAI' THEN
            BEGIN
                    STR_IF_REFUND := 'SELECT B.MAHANG, B.MAKHO_NHAP,B.SOLUONG, 
                CASE  
                WHEN xnt.GIAVON IS NULL OR xnt.GIAVON = 0 THEN ROUND(NVL(gia.GIAMUA,0) * B.SOLUONG, 2) 
                ELSE ROUND(NVL(xnt.GIAVON,0) * B.SOLUONG, 2) 
                END AS NHAPGT, B.UNITCODE 
                FROM
                (SELECT b.MAHANG, a.MAKHO_NHAP, ROUND(SUM(b.SOLUONG), 2) AS SOLUONG, ROUND(SUM(b.GIAMUA * b.SOLUONG), 2) AS NHAPGT, a.UNITCODE FROM
                CHUNGTU a INNER JOIN CHUNGTU_CHITIET b ON a.MA_CHUNGTU = b.MA_CHUNGTU 
                WHERE a.LOAI_CHUNGTU = '''|| P_MA_NHAPXUAT|| '''
                 AND UNITCODE = '''|| P_UNITCODE|| '''
                 AND TO_DATE(a.NGAY_DUYETPHIEU,''DD-MM-YY'') <= TO_DATE('''|| P_DENNGAY|| ''',''DD-MM-YY'')
                 AND TO_DATE(a.NGAY_DUYETPHIEU,''DD-MM-YY'') >= TO_DATE('''|| P_TUNGAY || ''',''DD-MM-YY'')
                GROUP BY b.MAHANG,a.MAKHO_NHAP,a.UNITCODE) B
                INNER JOIN '|| P_TABLENAME || ' xnt  ON  B.MAHANG = xnt.MAHANG AND B.MAKHO_NHAP = xnt.MAKHO 
                INNER JOIN MATHANG_GIA gia ON B.MAHANG = gia.MAHANG AND B.UNITCODE = gia.UNITCODE AND B.UNITCODE = xnt.UNITCODE AND gia.UNITCODE = '''|| P_UNITCODE ||'''
                UNION ALL
                SELECT B.MAHANG, B.MAKHO_NHAP, B.SOLUONG, 
                CASE WHEN xnt.GIAVON IS NULL OR xnt.GIAVON = 0 THEN NVL(gia.GIABANLE, 0)*B.SOLUONG 
                ELSE ROUND(xnt.GIAVON * B.SOLUONG , 2) END AS NHAPGT,B.UNITCODE FROM 
                (SELECT b.MAHANG, a.MAKHO_XUAT AS MAKHO_NHAP, a.UNITCODE, ROUND(SUM(b.SOLUONG), 2) AS SOLUONG, SUM(ROUND(ROUND(b.GIABANLE_VAT/(1 + c.GIATRI/100),2) * b.SOLUONG, 2)) AS XUATGT                     
                FROM GIAODICH a INNER JOIN GIAODICH_CHITIET b ON a.MA_GIAODICH = b.MA_GIAODICH INNER JOIN THUE c ON b.MATHUE_RA = c.MATHUE
                WHERE a.LOAI_GIAODICH = '''|| P_MA_NHAPXUAT|| '''  
                AND a.UNITCODE = '''|| P_UNITCODE|| ''' 
                AND TO_DATE(a.NGAY_GIAODICH,''DD-MM-YY'') <= TO_DATE('''|| P_DENNGAY || ''',''DD-MM-YY'')
                AND TO_DATE(a.NGAY_GIAODICH,''DD-MM-YY'') >= TO_DATE('''|| P_TUNGAY || ''',''DD-MM-YY'') 
                GROUP BY b.MAHANG,a.MAKHO_XUAT,a.UNITCODE) B
                INNER JOIN '|| P_TABLENAME || ' xnt ON B.MAHANG = xnt.MAHANG AND B.MAKHO_NHAP = xnt.MAKHO 
                AND B.UNITCODE = xnt.UNITCODE 
                INNER JOIN MATHANG_GIA gia ON B.MAHANG = gia.MAHANG AND B.UNITCODE = gia.UNITCODE;
                ';
            END;
        ELSE
                    STR_IF_REFUND := 'SELECT b.MAHANG,a.MAKHO_NHAP,ROUND(SUM(b.SOLUONG), 2) AS SOLUONG, ROUND(SUM(b.SOLUONG*b.GIAMUA), 2) AS NHAPGT,a.UNITCODE
                        FROM CHUNGTU a INNER JOIN CHUNGTU_CHITIET b ON a.MA_CHUNGTU = b.MA_CHUNGTU
                        WHERE UNITCODE = '''|| P_UNITCODE || ''' 
                        AND a.LOAI_CHUNGTU = '''|| P_MA_NHAPXUAT|| '''
                        AND TO_DATE(a.NGAY_DUYETPHIEU,''DD-MM-YY'') <= TO_DATE('''|| P_DENNGAY|| ''',''DD-MM-YY'')
                        AND TO_DATE(a.NGAY_DUYETPHIEU,''DD-MM-YY'') >= TO_DATE('''|| P_TUNGAY || ''',''DD-MM-YY'')
                        GROUP BY b.MAHANG,a.MAKHO_NHAP,a.UNITCODE;';
        END IF;
        --DBMS_OUTPUT.PUT_LINE('STR_IF_REFUND: ' || STR_IF_REFUND);
        N_SQL_INSERT := 'DECLARE 
        N_COUNT NUMBER(10,0) :=0; 
        P_TONDAUKYSL NUMBER(18,2) :=0; 
        P_TONDAUKYGT NUMBER(18,2) :=0;
        T_NHAPSL NUMBER(18,2) :=0;

    CURSOR NHAP_XUAT_VATTU IS '|| STR_IF_REFUND|| '
    BEGIN FOR ROW_VATTU IN NHAP_XUAT_VATTU LOOP
    N_COUNT :=0;
      BEGIN 
        SELECT COUNT(*) INTO N_COUNT FROM '|| P_TABLENAME|| ' WHERE 
        MAHANG = ROW_VATTU.MAHANG AND MAKHO = ROW_VATTU.MAKHO_NHAP AND UNITCODE = ROW_VATTU.UNITCODE;      
        EXCEPTION WHEN OTHERS THEN N_COUNT:=0;
      END;
    BEGIN
      IF(N_COUNT=0) THEN 
        BEGIN 
      INSERT INTO '|| P_TABLENAME|| ' (UNITCODE, NAM, KY, MAKHO, MAHANG,GIAVON,TONDAUKYSL,TONDAUKYGT,NHAPSL,NHAPGT,XUATSL,XUATGT,TONCUOIKYSL,TONCUOIKYGT) 
      SELECT ROW_VATTU.UNITCODE AS UNITCODE,'
                        || P_NAM
                        || ' AS NAM,'
                        || P_KY
                        || ' AS KY,ROW_VATTU.MAKHO_NHAP AS MAKHO,ROW_VATTU.MAHANG,
                        0 AS GIAVON, 0 AS TONDAUKYSL, 0 AS TONDAUKYGT,
                        0 AS NHAPSL, 0 AS NHAPGT, 0 AS XUATSL, 0 AS XUATGT,
                        0 AS TONCUOIKYSL, 0 AS TONCUOIKYGT
                        FROM MATHANG 
                        WHERE UNITCODE = ROW_VATTU.UNITCODE AND MAHANG = ROW_VATTU.MAHANG;
                COMMIT;
        END;
       END IF;
     END;
    BEGIN 
     UPDATE '|| P_TABLENAME|| ' SET
                  NHAPSL = NHAPSL + ROW_VATTU.SOLUONG, 
                  NHAPGT = NHAPGT + ROW_VATTU.NHAPGT, 
                  TONCUOIKYSL = TONCUOIKYSL + ROW_VATTU.SOLUONG, 
                  TONCUOIKYGT = TONCUOIKYGT + ROW_VATTU.NHAPGT,
                  GIAVON = CASE (TONCUOIKYSL + ROW_VATTU.SOLUONG) WHEN 0 THEN NVL((SELECT GIAMUA FROM MATHANG_GIA WHERE MAHANG = ROW_VATTU.MAHANG AND UNITCODE = ROW_VATTU.UNITCODE), 0) ELSE ( ABS(NVL(TONCUOIKYGT, 0))+ ABS(NVL(ROW_VATTU.NHAPGT, 0)) )/( ABS(NVL(TONCUOIKYSL, 0)) + ABS(NVL(ROW_VATTU.SOLUONG, 0)) ) END
                  WHERE UNITCODE = ROW_VATTU.UNITCODE AND MAHANG = ROW_VATTU.MAHANG AND MAKHO = ROW_VATTU.MAKHO_NHAP;  
                  COMMIT;
    END;
    END LOOP; 
    END; ';
    --DBMS_OUTPUT.PUT_LINE('N_SQL_INSERT: ' || N_SQL_INSERT);
    EXECUTE IMMEDIATE N_SQL_INSERT;
    EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('ERROR:' || N_SQL_INSERT);
    END XNT_TANG_TONKY;
---------------------------------------------------

    PROCEDURE XNT_GIAM_TONKY (
        P_TABLENAME    IN             VARCHAR2,
        P_UNITCODE     IN             VARCHAR2,
        P_NAM          IN             NUMBER,
        P_KY           IN             NUMBER,
        P_MA_NHAPXUAT  IN             VARCHAR2,
        P_TUNGAY       DATE,
        P_DENNGAY      DATE
    ) IS
        N_SQL_INSERT   VARCHAR(5000);
    BEGIN
        N_SQL_INSERT := '
    DECLARE 
    N_COUNT NUMBER :=0; 
    P_TONDAUKYSL NUMBER :=0; 
    P_TONDAUKYGT NUMBER := 0; 
    GIAVON_KHOASO NUMBER := 0; 
    TONDAUKY_KHOASO NUMBER := 0; 
    GIAMUA NUMBER := 0;
    CURSOR NHAP_XUAT_VATTU IS SELECT B.MAHANG, B.MAKHO_XUAT ,B.UNITCODE , B.SOLUONG,
	 CASE  WHEN xnt.GIAVON = 0 OR xnt.GIAVON IS NULL THEN ROUND(NVL(gia.GIAMUA, 0) * B.SOLUONG, 2) 
	 ELSE ROUND(xnt.GIAVON * B.SOLUONG, 2) END AS XUATGT
     FROM
    (SELECT b.MAHANG, a.MAKHO_XUAT, a.UNITCODE, ROUND(SUM(b.SOLUONG),2) AS SOLUONG, 
    ROUND(SUM(b.GIAMUA * b.SOLUONG), 2) AS XUATGT 
    FROM CHUNGTU a INNER JOIN CHUNGTU_CHITIET b ON a.MA_CHUNGTU = b.MA_CHUNGTU 
    WHERE a.LOAI_CHUNGTU = '''|| P_MA_NHAPXUAT|| '''  
    AND a.UNITCODE = '''|| P_UNITCODE|| ''' 
    AND TO_DATE(a.NGAY_DUYETPHIEU,''DD-MM-YY'') <= TO_DATE('''|| P_DENNGAY || ''',''DD-MM-YY'')
    AND TO_DATE(a.NGAY_DUYETPHIEU,''DD-MM-YY'') >= TO_DATE('''|| P_TUNGAY || ''',''DD-MM-YY'') 
    GROUP BY b.MAHANG,a.MAKHO_XUAT,a.UNITCODE) B
    INNER JOIN '|| P_TABLENAME || ' xnt ON B.MAHANG = xnt.MAHANG AND B.MAKHO_XUAT = xnt.MAKHO 
    AND B.UNITCODE = xnt.UNITCODE 
	INNER JOIN MATHANG_GIA gia ON B.MAHANG = gia.MAHANG AND B.UNITCODE = gia.UNITCODE
    UNION ALL
    SELECT B.MAHANG, B.MAKHO_XUAT, B.UNITCODE, B.SOLUONG, 
	CASE WHEN xnt.GIAVON IS NULL OR xnt.GIAVON = 0 THEN NVL(gia.GIABANLE, 0)*B.SOLUONG 
	ELSE ROUND(xnt.GIAVON * B.SOLUONG , 2) END AS XUATGT FROM 
    (SELECT b.MAHANG, a.MAKHO_XUAT, a.UNITCODE, ROUND(SUM(b.SOLUONG), 2) AS SOLUONG, SUM(ROUND(ROUND(b.GIABANLE_VAT/(1 + c.GIATRI/100),2) * b.SOLUONG, 2)) AS XUATGT                     
    FROM GIAODICH a INNER JOIN GIAODICH_CHITIET b ON a.MA_GIAODICH = b.MA_GIAODICH INNER JOIN THUE c ON b.MATHUE_RA = c.MATHUE
    WHERE a.LOAI_GIAODICH = '''|| P_MA_NHAPXUAT|| '''  
    AND a.UNITCODE = '''|| P_UNITCODE|| ''' 
    AND TO_DATE(a.NGAY_GIAODICH,''DD-MM-YY'') <= TO_DATE('''|| P_DENNGAY || ''',''DD-MM-YY'')
    AND TO_DATE(a.NGAY_GIAODICH,''DD-MM-YY'') >= TO_DATE('''|| P_TUNGAY || ''',''DD-MM-YY'') 
    GROUP BY b.MAHANG,a.MAKHO_XUAT,a.UNITCODE) B
    INNER JOIN '|| P_TABLENAME || ' xnt ON B.MAHANG = xnt.MAHANG AND B.MAKHO_XUAT = xnt.MAKHO 
    AND B.UNITCODE = xnt.UNITCODE 
	INNER JOIN MATHANG_GIA gia ON B.MAHANG = gia.MAHANG AND B.UNITCODE = gia.UNITCODE;

  BEGIN FOR ROW_VATTU IN NHAP_XUAT_VATTU LOOP
    N_COUNT :=0;
      BEGIN 
        SELECT COUNT(*) INTO N_COUNT FROM '|| P_TABLENAME|| ' WHERE 
        MAHANG = ROW_VATTU.MAHANG AND MAKHO = ROW_VATTU.MAKHO_XUAT AND UNITCODE = ROW_VATTU.UNITCODE;      
        EXCEPTION WHEN OTHERS THEN N_COUNT := 0;
      END;
    BEGIN
      IF(N_COUNT=0) THEN
        BEGIN 
      INSERT INTO '|| P_TABLENAME|| ' (UNITCODE,NAM,KY,MAKHO,MAHANG, GIAVON,TONDAUKYSL,TONDAUKYGT,NHAPSL,NHAPGT,XUATSL,XUATGT,TONCUOIKYSL,TONCUOIKYGT)
      SELECT ROW_VATTU.UNITCODE AS UNITCODE,'
                        || P_NAM
                        || ' AS NAM,'
                        || P_KY
                        || ' AS KY,ROW_VATTU.MAKHO_XUAT AS MAKHO,
                        ROW_VATTU.MAHANG AS MAHANG,0 AS GIAVON, 0 AS TONDAUKYSL, 0 AS TONDAUKYGT,
                        0 AS NHAPSL, 0 AS NHAPGT, 0 AS XUATSL, 0 AS XUATGT,
                        0 AS TONCUOIKYSL, 0 AS TONCUOIKYGT
                        FROM MATHANG
                        WHERE UNITCODE = ROW_VATTU.UNITCODE AND MAHANG = ROW_VATTU.MAHANG;
                        COMMIT;
        END;
       END IF;
     END;

    BEGIN   
      UPDATE '|| P_TABLENAME|| ' SET XUATSL = NVL(XUATSL,0) + NVL(ROW_VATTU.SOLUONG,0), 
          XUATGT = NVL(XUATGT,0) + NVL(ROW_VATTU.XUATGT,0), 
          TONCUOIKYSL = NVL(TONCUOIKYSL,0) - NVL(ROW_VATTU.SOLUONG,0) ,
          TONCUOIKYGT = NVL(TONCUOIKYGT,0) - NVL(ROW_VATTU.XUATGT,0)
          WHERE UNITCODE = ROW_VATTU.UNITCODE AND MAHANG = ROW_VATTU.MAHANG AND MAKHO = ROW_VATTU.MAKHO_XUAT;     
          COMMIT;
    END;
    END LOOP; 
    END;';
    --DBMS_OUTPUT.PUT_LINE('GIAMTONKY' || N_SQL_INSERT);        
        EXECUTE IMMEDIATE N_SQL_INSERT;
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
    END XNT_GIAM_TONKY;
    
    
    PROCEDURE XNT_GIAM_TONKY_DATPHONG (
        P_TABLENAME    IN             VARCHAR2,
        P_UNITCODE     IN             VARCHAR2,
        P_NAM          IN             NUMBER,
        P_KY           IN             NUMBER,
        P_MA_NHAPXUAT  IN             VARCHAR2,
        P_TUNGAY       DATE,
        P_DENNGAY      DATE
    ) IS
        N_SQL_INSERT   VARCHAR(5000);
    BEGIN
        N_SQL_INSERT := '
    DECLARE 
    N_COUNT NUMBER :=0; 
    P_TONDAUKYSL NUMBER :=0; 
    P_TONDAUKYGT NUMBER := 0; 
    GIAVON_KHOASO NUMBER := 0; 
    TONDAUKY_KHOASO NUMBER := 0; 
    GIAMUA NUMBER := 0;
    CURSOR CUR_THANHTOAN IS SELECT B.MAHANG, B.MAKHO ,B.UNITCODE , B.SOLUONG,
	 CASE  WHEN xnt.GIAVON = 0 OR xnt.GIAVON IS NULL THEN ROUND(NVL(gia.GIAMUA, 0) * B.SOLUONG, 2) 
	 ELSE ROUND(xnt.GIAVON * B.SOLUONG, 2) END AS XUATGT
     FROM
    (
    SELECT thanhtoan.MAHANG,thanhtoan.MAKHO,thanhtoan.UNITCODE,thanhtoan.SOLUONG,thanhtoan.XUATGT FROM
        (
        SELECT b.MAHANG, a.MAKHO, a.UNITCODE, ROUND(SUM(b.SOLUONG)) AS SOLUONG, ROUND(SUM(b.GIABANLE_VAT * b.SOLUONG),2) AS XUATGT,loaiphong.MALOAIPHONG 
        FROM THANHTOAN_DATPHONG a INNER JOIN THANHTOAN_DATPHONG_CHITIET b ON a.MA_DATPHONG = b.MA_DATPHONG
        INNER JOIN PHONG phong ON a.MAPHONG = phong.MAPHONG INNER JOIN LOAIPHONG loaiphong ON PHONG.MALOAIPHONG = LOAIPHONG.MALOAIPHONG
        AND a.UNITCODE = b.UNITCODE AND phong.UNITCODE = loaiphong.UNITCODE
        WHERE TO_DATE(a.NGAY_THANHTOAN,''DD-MM-YY'') <= TO_DATE('''|| P_DENNGAY || ''',''DD-MM-YY'')
        AND TO_DATE(a.NGAY_THANHTOAN,''DD-MM-YY'') >= TO_DATE('''|| P_TUNGAY || ''',''DD-MM-YY'')
        AND a.UNITCODE = '''|| P_UNITCODE|| '''
        GROUP BY b.MAHANG, a.MAKHO, a.UNITCODE,loaiphong.MALOAIPHONG
        ) thanhtoan 
        WHERE thanhtoan.MAHANG NOT IN (SELECT cauhinh.MAHANG FROM CAUHINH_LOAIPHONG cauhinh WHERE cauhinh.MALOAIPHONG = thanhtoan.MALOAIPHONG AND thanhtoan.UNITCODE=cauhinh.UNITCODE)
        AND thanhtoan.MAHANG NOT IN (SELECT cauhinh.MAHANG_DICHVU FROM CAUHINH_LOAIPHONG cauhinh WHERE cauhinh.MALOAIPHONG = thanhtoan.MALOAIPHONG AND thanhtoan.UNITCODE=cauhinh.UNITCODE)
    ) B
    INNER JOIN '|| P_TABLENAME || ' xnt ON B.MAHANG = xnt.MAHANG AND B.MAKHO = xnt.MAKHO 
    AND B.UNITCODE = xnt.UNITCODE 
	INNER JOIN MATHANG_GIA gia ON B.MAHANG = gia.MAHANG AND B.UNITCODE = gia.UNITCODE ;

  BEGIN FOR ROW_VATTU IN CUR_THANHTOAN LOOP
    N_COUNT :=0;
      BEGIN 
        SELECT COUNT(*) INTO N_COUNT FROM '|| P_TABLENAME|| ' WHERE 
        MAHANG = ROW_VATTU.MAHANG AND MAKHO = ROW_VATTU.MAKHO AND UNITCODE = ROW_VATTU.UNITCODE;      
        EXCEPTION WHEN OTHERS THEN N_COUNT := 0;
      END;
    BEGIN
      IF(N_COUNT=0) THEN
        BEGIN 
      INSERT INTO '|| P_TABLENAME|| ' (UNITCODE,NAM,KY,MAKHO,MAHANG, GIAVON,TONDAUKYSL,TONDAUKYGT,NHAPSL,NHAPGT,XUATSL,XUATGT,TONCUOIKYSL,TONCUOIKYGT)
      SELECT ROW_VATTU.UNITCODE AS UNITCODE,'
                        || P_NAM
                        || ' AS NAM,'
                        || P_KY
                        || ' AS KY,ROW_VATTU.MAKHO AS MAKHO,
                        ROW_VATTU.MAHANG AS MAHANG,0 AS GIAVON, 0 AS TONDAUKYSL, 0 AS TONDAUKYGT,
                        0 AS NHAPSL, 0 AS NHAPGT, 0 AS XUATSL, 0 AS XUATGT,
                        0 AS TONCUOIKYSL, 0 AS TONCUOIKYGT
                        FROM MATHANG
                        WHERE UNITCODE = ROW_VATTU.UNITCODE AND MAHANG = ROW_VATTU.MAHANG;
                        COMMIT;
        END;
       END IF;
     END;

    BEGIN   
      UPDATE '|| P_TABLENAME|| ' SET XUATSL = NVL(XUATSL,0) + NVL(ROW_VATTU.SOLUONG,0), 
          XUATGT = NVL(XUATGT,0) + NVL(ROW_VATTU.XUATGT,0), 
          TONCUOIKYSL = NVL(TONCUOIKYSL,0) - NVL(ROW_VATTU.SOLUONG,0) ,
          TONCUOIKYGT = NVL(TONCUOIKYGT,0) - NVL(ROW_VATTU.XUATGT,0)
          WHERE UNITCODE = ROW_VATTU.UNITCODE AND MAHANG = ROW_VATTU.MAHANG AND MAKHO = ROW_VATTU.MAKHO;     
          COMMIT;
    END;
    END LOOP; 
    END;';
    --DBMS_OUTPUT.PUT_LINE('GIAMTONKY_DATPHONG' || N_SQL_INSERT);        
        EXECUTE IMMEDIATE N_SQL_INSERT;
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
    END XNT_GIAM_TONKY_DATPHONG;

    PROCEDURE XNT_GIAM_PHIEU (
        P_TABLENAME   IN            VARCHAR2,
        P_NAM         IN            NUMBER,
        P_KY          IN            NUMBER,
        P_ID          IN            VARCHAR2
    ) IS
        N_SQL_INSERT   VARCHAR(5000);
    BEGIN
        N_SQL_INSERT := 'DECLARE 
    N_COUNT NUMBER(10,0) := 0; 
    P_TONDAUKYSL NUMBER(18,2) := 0; 
    P_TONDAUKYGT NUMBER(18,2) := 0;
    CURSOR NHAP_XUAT_VATTU IS SELECT B.MAHANG, B.MAKHO_XUAT, B.UNITCODE, B.SOLUONG, 
	CASE WHEN xnt.GIAVON IS NULL OR xnt.GIAVON = 0 THEN NVL(gia.GIAMUA, 0)*B.SOLUONG 
	ELSE ROUND(xnt.GIAVON * B.SOLUONG , 2) END AS XUATGT FROM 
    (SELECT b.MAHANG, a.MAKHO_XUAT, a.UNITCODE, ROUND(SUM(b.SOLUONG)) AS SOLUONG, ROUND(SUM(b.GIAMUA * b.SOLUONG),2) AS XUATGT 
    FROM CHUNGTU a INNER JOIN CHUNGTU_CHITIET b ON a.MA_CHUNGTU = b.MA_CHUNGTU
    WHERE a.ID = '''|| P_ID || ''' GROUP BY b.MAHANG, a.MAKHO_XUAT, a.UNITCODE)  B 
    INNER JOIN '|| P_TABLENAME|| ' xnt  ON  B.MAHANG = xnt.MAHANG AND B.MAKHO_XUAT = xnt.MAKHO AND B.UNITCODE = xnt.UNITCODE 
   	INNER JOIN MATHANG_GIA gia ON B.MAHANG = gia.MAHANG AND B.UNITCODE = gia.UNITCODE
    UNION ALL
    SELECT B.MAHANG, B.MAKHO_XUAT, B.UNITCODE, B.SOLUONG, 
	CASE WHEN xnt.GIAVON IS NULL OR xnt.GIAVON = 0 THEN NVL(gia.GIABANLE, 0)*B.SOLUONG 
	ELSE ROUND(xnt.GIAVON * B.SOLUONG , 2) END AS XUATGT FROM 
    (SELECT b.MAHANG, a.MAKHO_XUAT, a.UNITCODE, ROUND(SUM(b.SOLUONG), 2) AS SOLUONG, SUM(ROUND(ROUND(b.GIABANLE_VAT/(1 + c.GIATRI/100),2) * b.SOLUONG, 2)) AS XUATGT                     
    FROM GIAODICH a INNER JOIN GIAODICH_CHITIET b ON a.MA_GIAODICH = b.MA_GIAODICH INNER JOIN THUE c ON b.MATHUE_RA = c.MATHUE
    WHERE a.ID = '''|| P_ID || ''' GROUP BY b.MAHANG, a.MAKHO_XUAT, a.UNITCODE)  B 
    INNER JOIN '|| P_TABLENAME|| ' xnt  ON  B.MAHANG = xnt.MAHANG AND B.MAKHO_XUAT = xnt.MAKHO AND B.UNITCODE = xnt.UNITCODE 
   	INNER JOIN MATHANG_GIA gia ON B.MAHANG = gia.MAHANG AND B.UNITCODE = gia.UNITCODE;
  BEGIN FOR ROW_VATTU IN NHAP_XUAT_VATTU LOOP
    N_COUNT :=0;
      BEGIN 
        SELECT COUNT(*) INTO N_COUNT FROM '|| P_TABLENAME || ' WHERE 
        MAHANG = ROW_VATTU.MAHANG AND MAKHO = ROW_VATTU.MAKHO_XUAT AND UNITCODE = ROW_VATTU.UNITCODE;      
        EXCEPTION WHEN OTHERS THEN N_COUNT:=0;
      END;
    BEGIN
      IF(N_COUNT=0) THEN 
        BEGIN 
      INSERT INTO '|| P_TABLENAME || ' (UNITCODE, NAM, KY, MAKHO, MAHANG,GIAVON,TONDAUKYSL,TONDAUKYGT,NHAPSL,NHAPGT,XUATSL,XUATGT,TONCUOIKYSL,TONCUOIKYGT) 
      SELECT ROW_VATTU.UNITCODE AS UNITCODE,'
                        || P_NAM
                        || ' AS NAM,'
                        || P_KY
                        || ' AS KY,ROW_VATTU.MAKHO_XUAT AS MAKHO,ROW_VATTU.MAHANG,
                        0 AS GIAVON, 0 AS TONDAUKYSL, 0 AS TONDAUKYGT,
                        0 AS NHAPSL, 0 AS NHAPGT, 0 AS XUATSL, 0 AS XUATGT,
                        0 AS TONCUOIKYSL, 0 AS TONCUOIKYGT
                        FROM MATHANG 
                        WHERE UNITCODE = ROW_VATTU.UNITCODE AND MAHANG = ROW_VATTU.MAHANG;
        END;
       END IF;
     END;

    BEGIN     
      UPDATE '|| P_TABLENAME|| ' SET
      XUATSL=NVL(XUATSL,0)+NVL(ROW_VATTU.SOLUONG,0), 
      XUATGT=NVL(XUATGT,0)+NVL(ROW_VATTU.XUATGT,0), 
      TONCUOIKYSL=NVL(TONCUOIKYSL,0)-NVL(ROW_VATTU.SOLUONG,0),
      TONCUOIKYGT=NVL(TONCUOIKYGT,0)-NVL(ROW_VATTU.XUATGT,0)
      WHERE UNITCODE = ROW_VATTU.UNITCODE AND MAHANG = ROW_VATTU.MAHANG AND MAKHO = ROW_VATTU.MAKHO_XUAT;      
    END;

    END LOOP; 
    END;';
        --DBMS_OUTPUT.PUT_LINE(N_SQL_INSERT);
        EXECUTE IMMEDIATE N_SQL_INSERT;
    EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE(SQLERRM);
    END XNT_GIAM_PHIEU;

    PROCEDURE XNT_TANG_PHIEU (
        P_TABLENAME   IN            VARCHAR2,
        P_NAM         IN            NUMBER,
        P_KY          IN            NUMBER,
        P_ID          IN            VARCHAR2
    ) IS
        N_SQL_INSERT    VARCHAR(5000);
        N_LOAICHUNGTU   VARCHAR(100);
        STR_IF_REFUND   VARCHAR(2000);
    BEGIN
    SELECT a.LOAI_CHUNGTU INTO N_LOAICHUNGTU FROM CHUNGTU a WHERE a.ID = P_ID AND ROWNUM = 1;
        IF N_LOAICHUNGTU = '2' THEN
            BEGIN
                STR_IF_REFUND := 'SELECT B.MAHANG, B.MAKHO_NHAP, B.UNITCODE, B.SOLUONG, 
                CASE WHEN P.TONCUOIKYSL = 0 THEN ROUND(NVL(gia.GIAMUA, 0)*B.SOLUONG,2) 
                WHEN (xnt.TONDAUKYSL + xnt.NHAPSL) IS NULL THEN ROUND(NVL(gia.GIAMUA, 0)*B.SOLUONG,2) 
                ELSE ABS(B.SOLUONG * ((xnt.TONDAUKYGT + ((.NHAPGT)/(((.TONDAUKYSL + ((.NHAPSL))) END AS NHAPGT FROM 
                (SELECT MAVATTU, MAKHONHAP, UNITCODE, SUM(SOLUONG) AS SOLUONG, SUM(DONGIA*SOLUONG) AS NHAPGT 
                FROM CHUNGTU a INNER JOIN CHUNGTU_CHITIET b ON a.MA_CHUNGTU = b.MA_CHUNGTU WHERE  
                a.ID = '''|| P_ID ||''' GROUP BY b.MAHANG, a.MAKHO_NHAP, a.UNITCODE)  B 
                LEFT JOIN '|| P_TABLENAME|| ' xnt ON  B.MAHANG = xnt.MAHANG AND B.MAKHO_NHAP = xnt.MAKHO AND B.UNITCODE = xnt.UNITCODE 
                LEFT JOIN MATHANG_GIA gia ON B.MAHANG = gia.MAHANG AND B.UNITCODE = gia.UNITCODE ;';
            END;
        ELSE
            STR_IF_REFUND := 'SELECT b.MAHANG,a.MAKHO_NHAP,a.UNITCODE,ROUND(SUM(b.SOLUONG),2) AS SOLUONG,
            ROUND(SUM(b.SOLUONG * b.GIAMUA),2) AS NHAPGT 
            FROM CHUNGTU a INNER JOIN CHUNGTU_CHITIET b ON a.MA_CHUNGTU = b.MA_CHUNGTU
            WHERE a.ID = '''|| P_ID || '''     
            GROUP BY b.MAHANG,a.MAKHO_NHAP,a.UNITCODE;';
        END IF;
        N_SQL_INSERT := 'DECLARE 
    N_COUNT NUMBER(10,0) := 0; 
    P_TONDAUKYSL NUMBER(18,2) := 0; 
    P_TONDAUKYGT NUMBER(18,2) := 0;
    CURSOR NHAP_XUAT_VATTU IS '|| STR_IF_REFUND|| '
    BEGIN FOR ROW_VATTU IN NHAP_XUAT_VATTU LOOP
    N_COUNT := 0;
      BEGIN 
        SELECT COUNT(*) INTO N_COUNT FROM '|| P_TABLENAME || ' WHERE 
        MAHANG = ROW_VATTU.MAHANG AND MAKHO = ROW_VATTU.MAKHO_NHAP AND UNITCODE = ROW_VATTU.UNITCODE;      
        EXCEPTION WHEN OTHERS THEN N_COUNT:=0;
      END;
    BEGIN
      IF(N_COUNT=0) THEN 
        BEGIN 
      INSERT INTO '|| P_TABLENAME|| ' (UNITCODE, NAM, KY, MAKHO, MAHANG,GIAVON,TONDAUKYSL,TONDAUKYGT,NHAPSL,NHAPGT,XUATSL,XUATGT,TONCUOIKYSL,TONCUOIKYGT) 
      SELECT ROW_VATTU.UNITCODE AS UNITCODE,'
                        || P_NAM
                        || ' AS NAM,'
                        || P_KY
                        || ' AS KY,ROW_VATTU.MAKHO_NHAP AS MAKHO,ROW_VATTU.MAHANG,
                        0 AS GIAVON, 0 AS TONDAUKYSL, 0 AS TONDAUKYGT,
                        0 AS NHAPSL, 0 AS NHAPGT, 0 AS XUATSL, 0 AS XUATGT,
                        0 AS TONCUOIKYSL, 0 AS TONCUOIKYGT
                        FROM MATHANG 
                        WHERE UNITCODE = ROW_VATTU.UNITCODE AND MAHANG = ROW_VATTU.MAHANG;
        END;
       END IF;
     END;

    BEGIN     
      UPDATE '|| P_TABLENAME || ' SET
      NHAPSL = NHAPSL + ROW_VATTU.SOLUONG, 
	  NHAPGT = NHAPGT + ROW_VATTU.NHAPGT, 
	  TONCUOIKYSL = TONCUOIKYSL + ROW_VATTU.SOLUONG, 
	  TONCUOIKYGT = TONCUOIKYGT + ROW_VATTU.NHAPGT,
	  GIAVON = 
    CASE (TONCUOIKYSL + ROW_VATTU.SOLUONG) 
    WHEN 0 THEN 
    NVL((SELECT GIAMUA FROM MATHANG_GIA 
    WHERE MAHANG = ROW_VATTU.MAHANG AND UNITCODE =ROW_VATTU.UNITCODE), 0) 
	ELSE ABS((NVL(TONCUOIKYGT, 0) + NVL(ROW_VATTU.NHAPGT, 0))/(NVL(TONCUOIKYSL, 0)+NVL(ROW_VATTU.SOLUONG, 0))) END
      WHERE UNITCODE = ROW_VATTU.UNITCODE AND MAHANG = ROW_VATTU.MAHANG AND MAKHO = ROW_VATTU.MAKHO_NHAP; 
    END;
    END LOOP; 
    END;';
        --DBMS_OUTPUT.PUT_LINE(N_SQL_INSERT);
        EXECUTE IMMEDIATE N_SQL_INSERT;
    EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE(SQLERRM);
    END XNT_TANG_PHIEU;


    PROCEDURE XNT_THANHTOAN_DATPHONG (
        P_TABLENAME   IN            VARCHAR2,
        P_NAM         IN            NUMBER,
        P_KY          IN            NUMBER,
        P_ID          IN            VARCHAR2
    ) IS
        N_SQL_INSERT   VARCHAR(5000);
    BEGIN
        N_SQL_INSERT := 'DECLARE 
    N_COUNT NUMBER(10,0) := 0; 
    P_TONDAUKYSL NUMBER(18,2) := 0; 
    P_TONDAUKYGT NUMBER(18,2) := 0;
    CURSOR CUR_THANHTOAN IS 
    SELECT B.MAHANG, B.MAKHO, B.UNITCODE, B.SOLUONG, 
	CASE WHEN xnt.GIAVON IS NULL OR xnt.GIAVON = 0 THEN NVL(gia.GIAMUA, 0)*B.SOLUONG 
	ELSE ROUND(xnt.GIAVON * B.SOLUONG , 2) END AS XUATGT FROM 
    (
        SELECT thanhtoan.MAHANG,thanhtoan.MAKHO,thanhtoan.UNITCODE,thanhtoan.SOLUONG,thanhtoan.XUATGT FROM
        (
        SELECT b.MAHANG, a.MAKHO, a.UNITCODE, ROUND(SUM(b.SOLUONG)) AS SOLUONG, ROUND(SUM(b.GIABANLE_VAT * b.SOLUONG),2) AS XUATGT,loaiphong.MALOAIPHONG 
        FROM THANHTOAN_DATPHONG a INNER JOIN THANHTOAN_DATPHONG_CHITIET b ON a.MA_DATPHONG = b.MA_DATPHONG
        INNER JOIN PHONG phong ON a.MAPHONG = phong.MAPHONG INNER JOIN LOAIPHONG loaiphong ON PHONG.MALOAIPHONG = LOAIPHONG.MALOAIPHONG
        AND a.UNITCODE = b.UNITCODE AND phong.UNITCODE = loaiphong.UNITCODE
        WHERE a.ID = '''|| P_ID || ''' GROUP BY b.MAHANG, a.MAKHO, a.UNITCODE,loaiphong.MALOAIPHONG
        ) thanhtoan 
        WHERE thanhtoan.MAHANG NOT IN (SELECT cauhinh.MAHANG FROM CAUHINH_LOAIPHONG cauhinh WHERE cauhinh.MALOAIPHONG = thanhtoan.MALOAIPHONG AND thanhtoan.UNITCODE=cauhinh.UNITCODE)
        AND thanhtoan.MAHANG NOT IN (SELECT cauhinh.MAHANG_DICHVU FROM CAUHINH_LOAIPHONG cauhinh WHERE cauhinh.MALOAIPHONG = thanhtoan.MALOAIPHONG AND thanhtoan.UNITCODE=cauhinh.UNITCODE)
    )  B 
    INNER JOIN '|| P_TABLENAME|| ' xnt ON  B.MAHANG = xnt.MAHANG AND B.MAKHO = xnt.MAKHO AND B.UNITCODE = xnt.UNITCODE 
   	INNER JOIN MATHANG_GIA gia ON B.MAHANG = gia.MAHANG AND B.UNITCODE = gia.UNITCODE;
  BEGIN FOR ROW_VATTU IN CUR_THANHTOAN LOOP
    N_COUNT :=0;
      BEGIN 
        SELECT COUNT(*) INTO N_COUNT FROM '|| P_TABLENAME || ' WHERE 
        MAHANG = ROW_VATTU.MAHANG AND MAKHO = ROW_VATTU.MAKHO AND UNITCODE = ROW_VATTU.UNITCODE;      
        EXCEPTION WHEN OTHERS THEN N_COUNT:=0;
      END;
    BEGIN
      IF(N_COUNT=0) THEN 
        BEGIN 
      INSERT INTO '|| P_TABLENAME || ' (UNITCODE, NAM, KY, MAKHO, MAHANG,GIAVON,TONDAUKYSL,TONDAUKYGT,NHAPSL,NHAPGT,XUATSL,XUATGT,TONCUOIKYSL,TONCUOIKYGT) 
      SELECT ROW_VATTU.UNITCODE AS UNITCODE,'
                        || P_NAM
                        || ' AS NAM,'
                        || P_KY
                        || ' AS KY,ROW_VATTU.MAKHO AS MAKHO,ROW_VATTU.MAHANG,
                        0 AS GIAVON, 0 AS TONDAUKYSL, 0 AS TONDAUKYGT,
                        0 AS NHAPSL, 0 AS NHAPGT, 0 AS XUATSL, 0 AS XUATGT,
                        0 AS TONCUOIKYSL, 0 AS TONCUOIKYGT
                        FROM MATHANG 
                        WHERE UNITCODE = ROW_VATTU.UNITCODE AND MAHANG = ROW_VATTU.MAHANG;
        END;
       END IF;
     END;

    BEGIN     
      UPDATE '|| P_TABLENAME|| ' SET
      XUATSL=NVL(XUATSL,0)+NVL(ROW_VATTU.SOLUONG,0), 
      XUATGT=NVL(XUATGT,0)+NVL(ROW_VATTU.XUATGT,0), 
      TONCUOIKYSL=NVL(TONCUOIKYSL,0)-NVL(ROW_VATTU.SOLUONG,0),
      TONCUOIKYGT=NVL(TONCUOIKYGT,0)-NVL(ROW_VATTU.XUATGT,0)
      WHERE UNITCODE = ROW_VATTU.UNITCODE AND MAHANG = ROW_VATTU.MAHANG AND MAKHO = ROW_VATTU.MAKHO;      
    END;

    END LOOP; 
    END;';
        --DBMS_OUTPUT.PUT_LINE(N_SQL_INSERT);
        EXECUTE IMMEDIATE N_SQL_INSERT;
    EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE(SQLERRM);
    END XNT_THANHTOAN_DATPHONG;
    
    
    PROCEDURE XNT_KHOASO (
        P_TABLENAME_KYTRUOC   IN                    VARCHAR2,
        P_TABLENAME           IN                    VARCHAR2,
        P_UNITCODE            IN                    VARCHAR2,
        P_NAM                 IN                    NUMBER,
        P_KY                  IN                    NUMBER
    ) IS
        P_TUNGAY           DATE;
        P_TUNGAY_KYTRUOC   DATE;
        P_DENNGAY          DATE;
        N_COUNT            NUMBER;
    BEGIN
      SELECT TO_DATE(TUNGAY,'DD-MM-YY') AS TUNGAY,TO_DATE(DENNGAY,'DD-MM-YY') AS DENNGAY INTO P_TUNGAY,P_DENNGAY FROM KYKETOAN WHERE KYKETOAN = P_KY AND NAM = P_NAM AND UNITCODE = P_UNITCODE;
        BEGIN
            XNT_CREATE_TABLE_TONKY(P_TABLENAME_KYTRUOC, P_TABLENAME, P_UNITCODE, P_NAM, P_KY);
            XNT_TANG_TONKY(P_TABLENAME, P_UNITCODE, P_NAM, P_KY, 'NMUA', P_TUNGAY, P_DENNGAY);
            XNT_TANG_TONKY(P_TABLENAME, P_UNITCODE, P_NAM, P_KY, 'XBAN_TRALAI', P_TUNGAY, P_DENNGAY);
            XNT_GIAM_TONKY(P_TABLENAME, P_UNITCODE, P_NAM, P_KY, 'XBAN_LE', P_TUNGAY, P_DENNGAY);
            XNT_GIAM_TONKY(P_TABLENAME, P_UNITCODE, P_NAM, P_KY, 'XBAN', P_TUNGAY, P_DENNGAY);
            XNT_GIAM_TONKY_DATPHONG(P_TABLENAME, P_UNITCODE, P_NAM, P_KY, 'XBAN', P_TUNGAY, P_DENNGAY);
        END;
    END XNT_KHOASO;
END XUATNHAPTON;

/
--------------------------------------------------------
--  DDL for Function IS_NUMBER
--------------------------------------------------------

  CREATE OR REPLACE FUNCTION "ERBUS"."IS_NUMBER" (p_string IN VARCHAR2)
   RETURN INT
IS
   v_new_num NUMBER;
BEGIN
   v_new_num := TO_NUMBER(p_string);
   RETURN 1;
EXCEPTION
WHEN VALUE_ERROR THEN
   RETURN 0;
END "IS_NUMBER";




/
