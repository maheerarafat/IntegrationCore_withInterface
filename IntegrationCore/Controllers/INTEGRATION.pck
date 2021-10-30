CREATE OR REPLACE PACKAGE INTEGRATION iS

  --type VacationList is table of intuservacation%rowtype;

  Function GetLastUpdateDate(ServiceName varchar2) return date;
  Function GetRank(RankID varchar2,lang varchar2) return varchar2;
  PROCEDURE UpdateDepartments(XList IN XMLTYPE, ServiceName varchar);
  PROCEDURE UpdateUsers(XList IN XMLTYPE, ServiceName varchar);
END INTEGRATION;
/
CREATE OR REPLACE PACKAGE BODY INTEGRATION IS

  --===============================================================================
  PROCEDURE UpdateDepartments(XList IN XMLTYPE, ServiceName varchar) IS
    VacCount         number;
    TarasolClassType number;
    ParentExisits    number;
    status           number;
  BEGIN
    FOR i IN (SELECT XMLTYPE.EXTRACT(VALUE(a), '/Content/DepartmentID/text()')
                     .getstringval() AS DepartmentID,
                     XMLTYPE.EXTRACT(VALUE(a), '/Content/TransactionType/text()')
                     .getstringval() AS TransactionType,
                     XMLTYPE.EXTRACT(VALUE(a), '/Content/ChangedColumn/text()')
                     .getstringval() AS ChangedColumn,
                     XMLTYPE.EXTRACT(VALUE(a), '/Content/TransactionDate/text()')
                     .getstringval() AS TransactionDate,
                     XMLTYPE.EXTRACT(VALUE(a), '/Content/SerialNumber/text()')
                     .getstringval() AS SerialNumber,
                     XMLTYPE.EXTRACT(VALUE(a), '/Content/OldValue/text()')
                     .getstringval() AS OldValue,
                     XMLTYPE.EXTRACT(VALUE(a), '/Content/NewValue/text()')
                     .getstringval() AS NewValue
                FROM TABLE
                     
                     (XMLSEQUENCE(XList.EXTRACT('/GetListOfUpdatedDepartmentsResponse/Content'))) a) LOOP
      VacCount         := 0;
      TarasolClassType := -1;
      status           := 0;
    
      select count(*)
        into VacCount
        from intupdates
       where FLDServiceName = ServiceName
         and intupdates.FLDTRANSACTIONID = i.SerialNumber
         and intupdates.FLDTRANSACTIONDATE =
             to_date(replace(i.TransactionDate, 'T', ' '),
                     'YYYY-MM-DD HH24:mi:ss')
         and intupdates.fldstatus = '1';
    
      if VacCount = 0 then
        select count(*)
          into VacCount
          from intupdates
         where fldtransactionid = i.SerialNumber
           and FLDTRANSACTIONDATE =
               to_date(replace(i.TransactionDate, 'T', ' '),
                       'YYYY-MM-DD HH24:mi:ss');
        if VacCount = 0 then
          insert into intupdates
            (FLDServiceName,
             FLDObjectID,
             FLDChangedColumn,
             FLDTRANSACTIONID,
             FLDTRANSACTIONDATE,
             fldtransactiontype,
             fldOldValue,
             fldNewValue,
             fldstatus)
          VALUES
            (servicename,
             i.DepartmentID,
             i.ChangedColumn,
             i.SerialNumber,
             to_date(replace(i.TransactionDate, 'T', ' '),
                     'YYYY-MM-DD HH24:mi:ss'),
             i.TransactionType,
             i.OldValue,
             i.NewValue,
             '0')
          
          ;
        end if;
      
        if i.ChangedColumn = 'DEPT_NAME_A' then
        
          update tsdepartment d
             set fldlname = i.newvalue
           where d.fldcode = i.DepartmentID;
          status := 1;
        
        elsif i.ChangedColumn = 'DEPT_NAME_E' then
        
          update tsdepartment d
             set fldename = i.newvalue
           where d.fldcode = i.DepartmentID;
          status := 1;
        
        elsif i.ChangedColumn = 'DEPT_ID_PARENT' then
        
          select count(flduid)
            into ParentExisits
            from tsdepartment
           where fldcode = i.newvalue;
          if ParentExisits > 0 then
            update tsdepartment d
               set d.fldparentid = i.newvalue
             where d.fldcode = i.DepartmentID;
          
            if TarasolClassType in (1, 2) then
              update tsdepartment d
                 set d.fldmainparentid = d.flduid, d.fldismain = 1
               where d.fldcode = i.DepartmentID;
            elsif TarasolClassType in (3, 4) then
              update tsdepartment d
                 set d.fldismain       = 0,
                     d.fldmainparentid =
                     (select flduid
                        from (select flduid, rownum
                                from tsdepartment
                               where fldtype in (1, 2)
                                 and rownum = 1
                               start with fldcode = i.DepartmentID
                              connect by flduid = prior fldparentid
                               order by fldtype desc))
               where d.fldcode = i.DepartmentID;
            
            end if;
            status := 1;
          end if;
        
        elsif i.ChangedColumn = 'DEPT_CLASS_ID' then
          TarasolClassType := case
                                when i.newvalue in (6, 5) then
                                 4
                                when i.newvalue = 4 then
                                 3
                                when i.newvalue in (2, 3, 9) then
                                 2
                                else
                                 1
                              end;
        
          update tsdepartment d
             set fldtype = TarasolClassType
           where d.fldcode = i.DepartmentID;
        
          if TarasolClassType in (1, 2) then
            update tsdepartment d
               set d.fldmainparentid = d.flduid, d.fldismain = 1
             where d.fldcode = i.DepartmentID;
          elsif TarasolClassType in (3, 4) then
            update tsdepartment d
               set d.fldismain       = 0,
                   d.fldmainparentid =
                   (select flduid
                      from (select flduid, rownum
                              from tsdepartment
                             where fldtype in (1, 2)
                               and rownum = 1
                             start with fldcode = i.DepartmentID
                            connect by flduid = prior fldparentid
                             order by fldtype desc))
             where d.fldcode = i.DepartmentID;
          
          end if;
          status := 1;
          --DEPT_STATUS , DEPTSECTOR_ID , WRKPLACE_ID
        end if;
      
        update intupdates d
           set fldstatus = status
         where d.fldtransactionid = i.SerialNumber
           and d.FLDTRANSACTIONDATE =
               to_date(replace(i.TransactionDate, 'T', ' '),
                       'YYYY-MM-DD HH24:mi:ss');
      
      end if;
      commit;
    END LOOP;
  END;
  --==================================================================================
  PROCEDURE UpdateUsers(XList IN XMLTYPE, ServiceName varchar) IS
    VacCount         number;
    TarasolClassType number;
  
    status number;
  BEGIN
    FOR i IN (SELECT XMLTYPE.EXTRACT(VALUE(a), '/Content/EmployeeID/text()')
                     .getstringval() AS EmployeeID,
                     XMLTYPE.EXTRACT(VALUE(a), '/Content/TransactionType/text()')
                     .getstringval() AS TransactionType,
                     XMLTYPE.EXTRACT(VALUE(a), '/Content/ChangedColumn/text()')
                     .getstringval() AS ChangedColumn,
                     XMLTYPE.EXTRACT(VALUE(a), '/Content/TransactionDate/text()')
                     .getstringval() AS TransactionDate,
                     XMLTYPE.EXTRACT(VALUE(a), '/Content/SerialNumber/text()')
                     .getstringval() AS SerialNumber,
                     XMLTYPE.EXTRACT(VALUE(a), '/Content/OldValue/text()')
                     .getstringval() AS OldValue,
                     XMLTYPE.EXTRACT(VALUE(a), '/Content/NewValue/text()')
                     .getstringval() AS NewValue
                FROM TABLE
                     
                     (XMLSEQUENCE(XList.EXTRACT('/GetListOfUpdatedEmployeesResponse/Content'))) a) LOOP
      VacCount         := 0;
      TarasolClassType := -1;
      status           := 0;
    
      select count(*)
        into VacCount
        from intupdates
       where FLDServiceName = ServiceName
         and intupdates.FLDTRANSACTIONID = i.SerialNumber
         and intupdates.FLDTRANSACTIONDATE =
             to_date(replace(i.TransactionDate, 'T', ' '),
                     'YYYY-MM-DD HH24:mi:ss')
         and intupdates.fldstatus = '1';
    
      if VacCount = 0 then
        select count(*)
          into VacCount
          from intupdates
         where fldtransactionid = i.SerialNumber
           and FLDTRANSACTIONDATE =
               to_date(replace(i.TransactionDate, 'T', ' '),
                       'YYYY-MM-DD HH24:mi:ss');
        if VacCount = 0 then
          insert into intupdates
            (FLDServiceName,
             FLDObjectID,
             FLDChangedColumn,
             FLDTRANSACTIONID,
             FLDTRANSACTIONDATE,
             fldtransactiontype,
             fldOldValue,
             fldNewValue,
             fldstatus)
          VALUES
            (servicename,
             i.EmployeeID,
             i.ChangedColumn,
             i.SerialNumber,
             to_date(replace(i.TransactionDate, 'T', ' '),
                     'YYYY-MM-DD HH24:mi:ss'),
             i.TransactionType,
             i.OldValue,
             i.NewValue,
             '0');
        end if;
      
        if i.ChangedColumn = 'EMP_NAME_A' then
        
          update tsusers u
             set fldlocalfirstname = i.newvalue
           where u.fldnickname = i.employeeid;
          status := 1;
        
        elsif i.ChangedColumn = 'EMP_NAME_E' then
        
          update tsusers u
             set fldfirstname = i.newvalue
           where u.fldnickname = i.employeeid;
          status := 1;
        elsif i.ChangedColumn = 'Rank_ID' then
        
          update tsusers u
             set u.fldtitle = GetRank(i.newvalue,'E'),u.fldlocaltitle = GetRank(i.newvalue,'A')
           where u.fldnickname = i.employeeid;
          status := 1;
        elsif i.ChangedColumn = 'EMP_STATUS' then
        
          update tsusers u
             set u.fldblocked = case when i.newvalue = 'N' then '1' else '0' end
           where u.fldnickname = i.employeeid;
          status := 1;
        end if;
      
        update intupdates d
           set fldstatus = status
         where d.fldtransactionid = i.SerialNumber
           and d.FLDTRANSACTIONDATE =
               to_date(replace(i.TransactionDate, 'T', ' '),
                       'YYYY-MM-DD HH24:mi:ss');
      end if;
    
      commit;
    END LOOP;
  END;
  --==================================================================================
  Function GetLastUpdateDate(ServiceName varchar2) return date is
    lastDate date;
  begin
    select max(FLDTRANSACTIONDATE)
      into lastDate
      from intupdates
     where fldservicename = ServiceName
       and fldstatus = '1';
    return lastDate;
  end;
--==================================================================================
  Function GetRank(RankID varchar2,lang varchar2) return varchar2 is
    
  begin
    
    return '';
  end;
END INTEGRATION;
/
