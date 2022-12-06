create table INTJobEsb (
       FLDid number(10) not null, 
       FLDname nvarchar2(250) not null,
       FLDDescription nvarchar2(750) not null,
       FLDtype int not null,
       FLDdateStart date not null,
       FLDdateEnd date not null,
       FLDduration float not null      
);


alter table INTJobEsb
add (constraint INTJobEsb_pk primary key(FLDid))

create sequence INTJobEsb_sequence;

create or replace trigger INTJobEsb_on_insert
before insert on INTJobEsb
for each row
  begin 
    select INTJobEsb_sequence.nextval
    into : new.FLDid
    from dual;
   end;
   
   
commit;


alter table INTJobEsb add FLDBODYREQUEST nvarchar2(750);
alter table INTJobEsb add FLDSENDSCHUDLE number(10) null;
alter table INTJobEsb add FLDNUMBERTIMESENDSCHUDLE number(10) null;
alter table INTJobEsb add FLDTYPESCHUDLE number(10) null;

------

create table INTHistoryUpdateHR (
       FLDid number(10) not null, 
       FLDnametTable nvarchar2(250) not null,
       FLDDescription nvarchar2(750) null,
       FLDtype int null,
       FLDLastUpdateDate date not null
);


alter table INTHistoryUpdateHR
add (constraint INTHistoryUpdateHR_pk primary key(FLDid));

create sequence INTHistoryUpdateHR_sequence;

create or replace trigger INTHistoryUpdateHR_on_insert
before insert on INTHistoryUpdateHR
for each row
  begin 
    select INTHistoryUpdateHR_sequence.nextval
    into : new.FLDid
    from dual;
   end;
   
/  
commit;


-----

alter table intlookuptable add securityKey nvarchar2(40);
-- enum 1= form official ICA, 2=from HR Manually   
alter table intlookuptable add flag nvarchar2(10);








