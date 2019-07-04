CREATE OR replace PROCEDURE "GET_MENU" (
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
    DBMS_OUTPUT.PUT_LINE(QUERY_STR);
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
 
 
 