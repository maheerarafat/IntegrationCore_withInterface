
select count(*) from tsdocument where flduid in 
(select t.flddocuid from tsdocumentsexchange t);

delete from tsdocument where flduid in 
(select t.flddocuid from tsdocumentsexchange t);


select * from tsdocumentsexchangehistory;
delete from tsdocumentsexchangehistory;


select * from TSDOCUMENTEXCHANGENOTIFICATION
delete from TSDOCUMENTEXCHANGENOTIFICATION;

-- (status) 0 empty, 1 exported, 2 imported,  3 send,4 failed   , 5 is delivery , 6 rejected, 7 not found, 8 management filed

-- (action)  1 exported,2 imported,3 send,4 failed   ,5 accept, 6 rejected, 7 check status, 8 management filed
delete from tsdocumentsexchange;
commit;


