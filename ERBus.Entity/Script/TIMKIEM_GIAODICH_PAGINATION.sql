create or replace PROCEDURE "TIMKIEM_GIAODICH_PAGINATION" 
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