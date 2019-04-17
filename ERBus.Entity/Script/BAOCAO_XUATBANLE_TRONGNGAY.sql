CREATE OR REPLACE PROCEDURE "BAOCAO_XUATBANLE_TRONGNGAY" (
    TUNGAY     IN         DATE,
    DENNGAY    IN         DATE,
    UNITCODE   IN         NVARCHAR2,
    USERNAME   IN         VARCHAR2,
    CUR        OUT        SYS_REFCURSOR
) IS

    P_CREATE_TABLE                  VARCHAR2(1000);
    P_SQL_CLEAR                     VARCHAR2(2000);
    P_SQL_INSERT_BAN                VARCHAR2(2000);
    P_SQL_INSERT_BANBUON_QUAYHANG   VARCHAR2(2000);
    QUERY_STR                       VARCHAR2(2000);
    P_SQL_INSERT_TRA_LAI            VARCHAR2(2000);
    DK_GROUPBY                      VARCHAR2(2000) := '';
    N_COUNT                         NUMBER(10, 0) := 0;
BEGIN
    P_CREATE_TABLE := 'CREATE GLOBAL TEMPORARY TABLE "ERBUS"."TEMP_XUATBANLE_TRONGNGAY" 
   ("UNITCODE" VARCHAR2(10), 
    "USERNAME" VARCHAR2(20),
	"I_CREATE_BY" VARCHAR2(50), 
	"TENNHANVIEN" NVARCHAR2(200), 
	"TONGBAN" NUMBER(18,2) DEFAULT 0, 
	"TONGTRALAI" NUMBER(18,2) DEFAULT 0, 
	"SAPXEP" NUMBER(10,0) DEFAULT 0
   ) ON COMMIT DELETE ROWS'
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
    P_SQL_CLEAR := 'DELETE FROM TEMP_XUATBANLE_TRONGNGAY';
    P_SQL_INSERT_BAN := 'INSERT INTO TEMP_XUATBANLE_TRONGNGAY a(a.UNITCODE,a.USERNAME,a.I_CREATE_BY,a.TENNHANVIEN,a.TONGBAN,a.SAPXEP)
              (SELECT giaodich.UNITCODE AS UNITCODE,
               '''
                        || USERNAME
                        || ''' AS USERNAME,
                giaodich.I_CREATE_BY AS I_CREATE_BY,
                nguoidung.TENNHANVIEN AS TENNHANVIEN,
                SUM(THANHTIEN) AS TONGBAN,
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
                            GROUP BY giaodich.UNITCODE,giaodich.I_CREATE_BY,nguoidung.TENNHANVIEN)';

    P_SQL_INSERT_BANBUON_QUAYHANG := 'INSERT INTO TEMP_XUATBANLE_TRONGNGAY a(a.UNITCODE,a.USERNAME,a.I_CREATE_BY,a.TENNHANVIEN,a.TONGBAN,a.SAPXEP)
              (SELECT giaodich.UNITCODE AS UNITCODE,
              '''
                                     || USERNAME
                                     || ''' AS USERNAME,
                giaodich.I_CREATE_BY || ''-BB'' AS I_CREATE_BY,
                nguoidung.TENNHANVIEN AS TENNHANVIEN,
                SUM(THANHTIEN) AS TONGBAN,
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
                             GROUP BY giaodich.UNITCODE,giaodich.I_CREATE_BY,nguoidung.TENNHANVIEN)'
                                     ;

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
                                    GROUP BY giaodich.UNITCODE,giaodich.I_CREATE_BY,nguoidung.TENNHANVIEN), 0)'
                            ;

    QUERY_STR := 'SELECT UNITCODE,USERNAME,I_CREATE_BY,TENNHANVIEN,TONGBAN,TONGTRALAI,SAPXEP FROM TEMP_XUATBANLE_TRONGNGAY';
    BEGIN
        EXECUTE IMMEDIATE P_SQL_CLEAR;
        EXECUTE IMMEDIATE P_SQL_INSERT_BAN;
        EXECUTE IMMEDIATE P_SQL_INSERT_BANBUON_QUAYHANG;
        EXECUTE IMMEDIATE P_SQL_INSERT_TRA_LAI;
        OPEN CUR FOR QUERY_STR;

    END;

END BAOCAO_XUATBANLE_TRONGNGAY;