create or replace PROCEDURE "XUATNHAPTON_CHITIET" (
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