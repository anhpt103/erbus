﻿create or replace PROCEDURE "TIMKIEM_NHACUNGCAP_PAGINATION" 
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
    DBMS_OUTPUT.PUT_LINE('QUERY_SELECT:'||QUERY_SELECT);
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