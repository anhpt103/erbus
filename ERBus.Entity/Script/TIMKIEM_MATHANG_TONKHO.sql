﻿create or replace PROCEDURE "TIMKIEM_MATHANG_TONKHO" 
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
  DBMS_OUTPUT.PUT_LINE('QUERY_SELECT:'||QUERY_SELECT);
  BEGIN
  OPEN CURSOR_RESULT FOR QUERY_SELECT;
    EXCEPTION
    WHEN NO_DATA_FOUND THEN
     DBMS_OUTPUT.put_line ('NO_DATA_FOUND');  
       WHEN OTHERS THEN
     DBMS_OUTPUT.put_line (SQLERRM);  
  END;
END TIMKIEM_MATHANG_TONKHO;