if object_id('"IS_NUMBER"', 'fn') is not null
  drop function "IS_NUMBER";
go
create FUNCTION "IS_NUMBER" (@p_string VARCHAR(500))
   RETURNS INT
AS
BEGIN
   DECLARE @v_new_num INT;
   SET @v_new_num = ISNUMERIC(@p_string);
   RETURN @v_new_num;
END ;
/

if object_id('"BANLE_TIMKIEM_BOHANG_MAHANG"', 'P') is not null
  drop procedure "BANLE_TIMKIEM_BOHANG_MAHANG";
go

create PROCEDURE "BANLE_TIMKIEM_BOHANG_MAHANG" 
(
  @P_MADONVI VARCHAR(10) ,
  @P_TUKHOA VARCHAR(200),
  @P_SUDUNG_TIMKIEM_ALL INT,
  @P_DIEUKIENCHON INT
) AS
 BEGIN 
  DECLARE @QUERY_SELECT VARCHAR(3000);
  DECLARE @T_LOAITIMKIEM VARCHAR(20) = '';
  DECLARE @TEXT_IS_NUMBER DECIMAL(18,2) = 0;
  DECLARE @IS_CONTAIN_UNITCODE VARCHAR(2)= '';

SET NOCOUNT ON;
  SELECT @TEXT_IS_NUMBER = ISNUMERIC(@P_TUKHOA);
  BEGIN
  IF CAST(@P_TUKHOA AS VARCHAR(MAX)) <> @P_TUKHOA 
	BEGIN SET @IS_CONTAIN_UNITCODE = 'X';
  END
  ELSE
	BEGIN SET @IS_CONTAIN_UNITCODE = '';
  END;
  IF @P_SUDUNG_TIMKIEM_ALL = 1 BEGIN
      IF LEN(@P_TUKHOA) = 13 AND @TEXT_IS_NUMBER = 1 AND (@IS_CONTAIN_UNITCODE IS NULL OR @IS_CONTAIN_UNITCODE = '') 
        BEGIN SET @T_LOAITIMKIEM = 'BARCODE';
      END 
      IF LEN(@P_TUKHOA) = 7 AND SUBSTRING(@P_TUKHOA,0,2) <> 'BH' AND @TEXT_IS_NUMBER = 0 AND (@IS_CONTAIN_UNITCODE IS NULL OR @IS_CONTAIN_UNITCODE = '') 
        BEGIN SET @T_LOAITIMKIEM = 'MAHANG';
      END 
      IF @IS_CONTAIN_UNITCODE = 'X' 
        BEGIN SET @T_LOAITIMKIEM = 'TENHANG';
      END  
      IF LEN(@P_TUKHOA) = 4 AND @TEXT_IS_NUMBER = 1 AND (@IS_CONTAIN_UNITCODE IS NULL OR @IS_CONTAIN_UNITCODE = '') 
        BEGIN SET @T_LOAITIMKIEM = 'MACANDIENTU';
      END  
      IF SUBSTRING(@P_TUKHOA,0,2) = 'BH' 
        BEGIN SET @T_LOAITIMKIEM = 'BOHANG';
      END 
  END
  ELSE BEGIN
      IF @P_DIEUKIENCHON = 0
        BEGIN SET @T_LOAITIMKIEM = 'MAHANG';
      END 
      IF @P_DIEUKIENCHON = 1
        BEGIN SET @T_LOAITIMKIEM = 'BARCODE';
      END 
      IF @P_DIEUKIENCHON = 2
        BEGIN SET @T_LOAITIMKIEM = 'MACANDIENTU';
      END  
      IF @P_DIEUKIENCHON = 3
        BEGIN SET @T_LOAITIMKIEM = 'TENHANG';
      END  
      IF @P_DIEUKIENCHON = 4
        BEGIN SET @T_LOAITIMKIEM = 'BOHANG';
      END  
      IF @P_DIEUKIENCHON = 5
        BEGIN SET @T_LOAITIMKIEM = 'MAHANGTRONGBO';
      END 
      IF @P_DIEUKIENCHON = 6
        BEGIN SET @T_LOAITIMKIEM = 'GIABANLE_VAT';
      END  
  END 
IF @P_SUDUNG_TIMKIEM_ALL = 1 BEGIN
    IF @T_LOAITIMKIEM = 'BARCODE' BEGIN
        SET @QUERY_SELECT = 'SELECT a.MAHANG,a.MAHANG AS MACON,a.TENHANG,a.MALOAI,a.MANHOM,
                        d.TENDONVITINH AS DONVITINH,a.MANHACUNGCAP,c.TENNHACUNGCAP,
                        b.GIABANLE_VAT,a.ITEMCODE,a.BARCODE
                        FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG
                        INNER JOIN NHACUNGCAP c ON a.MANHACUNGCAP = c.MANHACUNGCAP
                        INNER JOIN DONVITINH d ON a.MADONVITINH = d.MADONVITINH
                        WHERE a.UNITCODE = '''+ISNULL(@P_MADONVI, '')+''' AND a.BARCODE LIKE ''%'+ISNULL(@P_TUKHOA, '')+'%'' ';
        END
        ELSE IF @T_LOAITIMKIEM = 'MAHANG' BEGIN
        SET @QUERY_SELECT = 'SELECT a.MAHANG,a.MAHANG AS MACON,a.TENHANG,a.MALOAI,a.MANHOM,
                        d.TENDONVITINH AS DONVITINH,a.MANHACUNGCAP,c.TENNHACUNGCAP,
                        b.GIABANLE_VAT,a.ITEMCODE,a.BARCODE
                        FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG
                        INNER JOIN NHACUNGCAP c ON a.MANHACUNGCAP = c.MANHACUNGCAP
                        INNER JOIN DONVITINH d ON a.MADONVITINH = d.MADONVITINH
                        WHERE a.UNITCODE = '''+ISNULL(@P_MADONVI, '')+''' AND a.MAHANG LIKE ''%'+ISNULL(@P_TUKHOA, '')+'%'' ';
        END
        ELSE IF @T_LOAITIMKIEM = 'TENHANG' BEGIN
        SET @QUERY_SELECT = 'SELECT a.MAHANG,a.MAHANG AS MACON,a.TENHANG,a.MALOAI,a.MANHOM,
                        d.TENDONVITINH AS DONVITINH,a.MANHACUNGCAP,c.TENNHACUNGCAP,
                        b.GIABANLE_VAT,a.ITEMCODE,a.BARCODE
                        FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG
                        INNER JOIN NHACUNGCAP c ON a.MANHACUNGCAP = c.MANHACUNGCAP
                        INNER JOIN DONVITINH d ON a.MADONVITINH = d.MADONVITINH
                        WHERE a.UNITCODE = '''+ISNULL(@P_MADONVI, '')+''' AND UPPER(a.TENHANG) LIKE N''%'+ISNULL(UPPER(@P_TUKHOA), '')+'%'' ';
        END
        ELSE IF @T_LOAITIMKIEM = 'MACANDIENTU' BEGIN
        SET @QUERY_SELECT = 'SELECT a.MAHANG,a.MAHANG AS MACON,a.TENHANG,a.MALOAI,a.MANHOM,
                        d.TENDONVITINH AS DONVITINH,a.MANHACUNGCAP,c.TENNHACUNGCAP,
                        b.GIABANLE_VAT,a.ITEMCODE,a.BARCODE
                        FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG
                        INNER JOIN NHACUNGCAP c ON a.MANHACUNGCAP = c.MANHACUNGCAP
                        INNER JOIN DONVITINH d ON a.MADONVITINH = d.MADONVITINH
                        WHERE a.UNITCODE = '''+ISNULL(@P_MADONVI, '')+''' AND a.ITEMCODE = '''+ISNULL(@P_TUKHOA, '')+''' ';
        END
        ELSE IF @T_LOAITIMKIEM = 'BOHANG' BEGIN
        SET @QUERY_SELECT = 'SELECT
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
                        INNER JOIN NHACUNGCAP E ON C.MANHACUNGCAP = E.MANHACUNGCAP AND c.UNITCODE = '''+ISNULL(@P_MADONVI, '')+''' AND a.MABOHANG LIKE ''%'+ISNULL(@P_TUKHOA, '')+'%'' ';
        END
        ELSE IF @T_LOAITIMKIEM = 'MAHANGTRONGBO' BEGIN
        SET @QUERY_SELECT = 'SELECT
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
                        AND c.UNITCODE = '''+ISNULL(@P_MADONVI, '')+''' AND b.MAHANG LIKE ''%'+ISNULL(@P_TUKHOA, '')+'%'' ';
        END
        ELSE IF @T_LOAITIMKIEM = 'GIABANLE_VAT' BEGIN
        SET @QUERY_SELECT = 'SELECT a.MAHANG,a.MAHANG AS MACON,a.TENHANG,a.MALOAI,a.MANHOM,
                        d.TENDONVITINH AS DONVITINH,a.MANHACUNGCAP,c.TENNHACUNGCAP,
                        b.GIABANLE_VAT,a.ITEMCODE,a.BARCODE
                        FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG
                        INNER JOIN NHACUNGCAP c ON a.MANHACUNGCAP = c.MANHACUNGCAP
                        INNER JOIN DONVITINH d ON a.MADONVITINH = d.MADONVITINH
                        WHERE a.UNITCODE = '''+ISNULL(@P_MADONVI, '')+''' AND b.GIABANLE_VAT = '+ISNULL(@P_TUKHOA, '')+' ';
        END
        ELSE BEGIN 
        SET @QUERY_SELECT = 'SELECT a.MAHANG,a.MAHANG AS MACON,a.TENHANG,a.MALOAI,a.MANHOM,
                        d.TENDONVITINH AS DONVITINH,a.MANHACUNGCAP,c.TENNHACUNGCAP,
                        b.GIABANLE_VAT,a.ITEMCODE,a.BARCODE
                        FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG
                        INNER JOIN NHACUNGCAP c ON a.MANHACUNGCAP = c.MANHACUNGCAP
                        INNER JOIN DONVITINH d ON a.MADONVITINH = d.MADONVITINH
                        WHERE a.UNITCODE = '''+ISNULL(@P_MADONVI, '')+''' AND a.MAHANG LIKE ''%'+ISNULL(@P_TUKHOA, '')+'%'' ';
    END 
END
ELSE BEGIN
    IF @T_LOAITIMKIEM = 'BARCODE' BEGIN
        SET @QUERY_SELECT = 'SELECT TOP 200 a.MAHANG,a.MAHANG AS MACON,a.TENHANG,a.MALOAI,a.MANHOM,
                        d.TENDONVITINH AS DONVITINH,a.MANHACUNGCAP,c.TENNHACUNGCAP,
                        b.GIABANLE_VAT,a.ITEMCODE,a.BARCODE
                        FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG
                        INNER JOIN NHACUNGCAP c ON a.MANHACUNGCAP = c.MANHACUNGCAP
                        INNER JOIN DONVITINH d ON a.MADONVITINH = d.MADONVITINH
                        WHERE a.UNITCODE = '''+ISNULL(@P_MADONVI, '')+''' AND a.BARCODE LIKE ''%'+ISNULL(@P_TUKHOA, '')+'%'' ';
        END
        ELSE IF @T_LOAITIMKIEM = 'MAHANG' BEGIN
        SET @QUERY_SELECT = 'SELECT TOP 200 a.MAHANG,a.MAHANG AS MACON,a.TENHANG,a.MALOAI,a.MANHOM,
                        d.TENDONVITINH AS DONVITINH,a.MANHACUNGCAP,c.TENNHACUNGCAP,
                        b.GIABANLE_VAT,a.ITEMCODE,a.BARCODE
                        FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG
                        INNER JOIN NHACUNGCAP c ON a.MANHACUNGCAP = c.MANHACUNGCAP
                        INNER JOIN DONVITINH d ON a.MADONVITINH = d.MADONVITINH
                        WHERE a.UNITCODE = '''+ISNULL(@P_MADONVI, '')+''' AND a.MAHANG LIKE ''%'+ISNULL(@P_TUKHOA, '')+'%'' ';
        END
        ELSE IF @T_LOAITIMKIEM = 'TENHANG' BEGIN
        SET @QUERY_SELECT = 'SELECT TOP 200 a.MAHANG,a.MAHANG AS MACON,a.TENHANG,a.MALOAI,a.MANHOM,
                        d.TENDONVITINH AS DONVITINH,a.MANHACUNGCAP,c.TENNHACUNGCAP,
                        b.GIABANLE_VAT,a.ITEMCODE,a.BARCODE
                        FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG
                        INNER JOIN NHACUNGCAP c ON a.MANHACUNGCAP = c.MANHACUNGCAP
                        INNER JOIN DONVITINH d ON a.MADONVITINH = d.MADONVITINH
                        WHERE a.UNITCODE = '''+ISNULL(@P_MADONVI, '')+''' AND UPPER(a.TENHANG) LIKE N''%'+ISNULL(UPPER(@P_TUKHOA), '')+'%'' ';
        END
        ELSE IF @T_LOAITIMKIEM = 'MACANDIENTU' BEGIN
        SET @QUERY_SELECT = 'SELECT TOP 200 a.MAHANG,a.MAHANG AS MACON,a.TENHANG,a.MALOAI,a.MANHOM,
                        d.TENDONVITINH AS DONVITINH,a.MANHACUNGCAP,c.TENNHACUNGCAP,
                        b.GIABANLE_VAT,a.ITEMCODE,a.BARCODE
                        FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG
                        INNER JOIN NHACUNGCAP c ON a.MANHACUNGCAP = c.MANHACUNGCAP
                        INNER JOIN DONVITINH d ON a.MADONVITINH = d.MADONVITINH
                        WHERE a.UNITCODE = '''+ISNULL(@P_MADONVI, '')+''' AND a.ITEMCODE = '''+ISNULL(@P_TUKHOA, '')+''' ';
        END
        ELSE IF @T_LOAITIMKIEM = 'BOHANG' BEGIN
        SET @QUERY_SELECT = 'SELECT TOP 200
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
                        INNER JOIN NHACUNGCAP E ON C.MANHACUNGCAP = E.MANHACUNGCAP AND c.UNITCODE = '''+ISNULL(@P_MADONVI, '')+''' AND a.MABOHANG LIKE ''%'+ISNULL(@P_TUKHOA, '')+'%'' ';
        END
        ELSE IF @T_LOAITIMKIEM = 'MAHANGTRONGBO' BEGIN
        SET @QUERY_SELECT = 'SELECT TOP 200
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
                        AND c.UNITCODE = '''+ISNULL(@P_MADONVI, '')+''' AND b.MAHANG LIKE ''%'+ISNULL(@P_TUKHOA, '')+'%'' ';
        END
        ELSE IF @T_LOAITIMKIEM = 'GIABANLE_VAT' BEGIN
        SET @QUERY_SELECT = 'SELECT TOP 200 a.MAHANG,a.MAHANG AS MACON,a.TENHANG,a.MALOAI,a.MANHOM,
                        d.TENDONVITINH AS DONVITINH,a.MANHACUNGCAP,c.TENNHACUNGCAP,
                        b.GIABANLE_VAT,a.ITEMCODE,a.BARCODE
                        FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG
                        INNER JOIN NHACUNGCAP c ON a.MANHACUNGCAP = c.MANHACUNGCAP
                        INNER JOIN DONVITINH d ON a.MADONVITINH = d.MADONVITINH
                        WHERE a.UNITCODE = '''+ISNULL(@P_MADONVI, '')+''' AND b.GIABANLE_VAT = '+ISNULL(@P_TUKHOA, '')+' ';
        END
        ELSE BEGIN 
        SET @QUERY_SELECT = 'SELECT TOP 200 a.MAHANG,a.MAHANG AS MACON,a.TENHANG,a.MALOAI,a.MANHOM,
                        d.TENDONVITINH AS DONVITINH,a.MANHACUNGCAP,c.TENNHACUNGCAP,
                        b.GIABANLE_VAT,a.ITEMCODE,a.BARCODE
                        FROM MATHANG a INNER JOIN MATHANG_GIA b ON a.MAHANG = b.MAHANG
                        INNER JOIN NHACUNGCAP c ON a.MANHACUNGCAP = c.MANHACUNGCAP
                        INNER JOIN DONVITINH d ON a.MADONVITINH = d.MADONVITINH
                        WHERE a.UNITCODE = '''+ISNULL(@P_MADONVI, '')+''' AND a.MAHANG LIKE ''%'+ISNULL(@P_TUKHOA, '')+'%'' ';
    END 
END 
    --PRINT 'QUERY_SELECT:'+ISNULL(@QUERY_SELECT, '');
  BEGIN
  EXECUTE (@QUERY_SELECT);
  END;
END;
END;