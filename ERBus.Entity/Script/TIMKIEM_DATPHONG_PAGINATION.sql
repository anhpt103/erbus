﻿create or replace PROCEDURE "TIMKIEM_DATPHONG_PAGINATION" 
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


