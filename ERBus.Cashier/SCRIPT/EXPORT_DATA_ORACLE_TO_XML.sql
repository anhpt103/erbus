create or replace PROCEDURE EXPORT_DATA_ORACLE_TO_XML
(
   STRQUERY NVARCHAR2,
   STRROOT NVARCHAR2,
   V_RESULT OUT CLOB
  )
AS
BEGIN
      DECLARE
        qryCtx DBMS_XMLGEN.ctxHandle;
     BEGIN 
         qryCtx := DBMS_XMLGEN.newContext(STRQUERY);
          -- Set the row header to be EMPLOYEE
          DBMS_XMLGEN.setRowTag(qryCtx, STRROOT);
          -- Get the result
          V_RESULT := DBMS_XMLGEN.getXML(qryCtx);
          --Close context
          DBMS_XMLGEN.closeContext(qryCtx);
        END;
END EXPORT_DATA_ORACLE_TO_XML;