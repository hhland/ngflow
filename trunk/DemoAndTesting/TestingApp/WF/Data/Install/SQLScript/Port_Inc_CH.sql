DELETE FROM Port_DeptType;
DELETE FROM Port_Dept;
DELETE FROM Port_Station;
DELETE FROM Port_Emp;
DELETE FROM Port_DeptEmpStation;
DELETE FROM Port_DeptEmp;
DELETE FROM Port_StationType;

/*  Create a view compatible ccflow4. Port_EmpStation */;
CREATE VIEW Port_EmpDept AS
  SELECT FK_Emp,FK_Dept FROM Port_DeptEmp;

/* Port_EmpStation */;
CREATE VIEW Port_EmpStation AS
  SELECT FK_Emp,FK_Station FROM Port_DeptEmpStation;

--  Position  ;
DELETE FROM Port_Duty;
INSERT INTO Port_Duty (No,Name) VALUES('01',' Chairman of the board ') ;
INSERT INTO Port_Duty (No,Name) VALUES('02',' General manager ');
INSERT INTO Port_Duty (No,Name) VALUES('03',' Chief ');
INSERT INTO Port_Duty (No,Name) VALUES('04',' Clerks ');
INSERT INTO Port_Duty (No,Name) VALUES('05',' Branch Manager ');

 
-- Port_DeptType ;
DELETE FROM Port_DeptType;
INSERT INTO Port_DeptType (No,Name) VALUES('01',' Group ') ;
INSERT INTO Port_DeptType (No,Name) VALUES('02',' Group Sector ');
INSERT INTO Port_DeptType (No,Name) VALUES('03',' Branch ');
INSERT INTO Port_DeptType (No,Name) VALUES('04',' Branch departments ');
 
-- Port_Dept ;
DELETE FROM Port_Dept;
INSERT INTO Port_Dept (No,Name,ParentNo,FK_DeptType,Leader,IsDir) VALUES('100',' Group Headquarters ','0','01','zhoupeng',0)   ;
INSERT INTO Port_Dept (No,Name,ParentNo,FK_DeptType,Leader,IsDir) VALUES('1001',' Group Marketing ','100','02','zhanghaicheng',1)  ;
INSERT INTO Port_Dept (No,Name,ParentNo,FK_DeptType,Leader,IsDir) VALUES('1002',' Group Develop department ','100','02','qifenglin',1)  ;
INSERT INTO Port_Dept (No,Name,ParentNo,FK_DeptType,Leader,IsDir) VALUES('1003',' Group Services ','100','02','zhanghaicheng',1)  ;
INSERT INTO Port_Dept (No,Name,ParentNo,FK_DeptType,Leader,IsDir) VALUES('1004',' Group Finance Ministry ','100','02','yangyilei',1)  ;
INSERT INTO Port_Dept (No,Name,ParentNo,FK_DeptType,Leader,IsDir) VALUES('1005',' Group Human Resources ','100','02','liping',1) ;
INSERT INTO Port_Dept (No,Name,ParentNo,FK_DeptType,Leader,IsDir) VALUES('1060',' South Branch ','100','03','wangwenying',0) ;
INSERT INTO Port_Dept (No,Name,ParentNo,FK_DeptType,Leader,IsDir) VALUES('1061',   ' Marketing ','1006','04','ranqingxin',1) ;
INSERT INTO Port_Dept (No,Name,ParentNo,FK_DeptType,Leader,IsDir) VALUES('1062',    ' Finance Department ','1006','04','randun',1) ;
INSERT INTO Port_Dept (No,Name,ParentNo,FK_DeptType,Leader,IsDir) VALUES('1063',    ' Sales ','1006','04','randun',1) ;

INSERT INTO Port_Dept (No,Name,ParentNo,FK_DeptType,Leader,IsDir) VALUES('1070',' North Branch ','100','03','lining',0) ;
INSERT INTO Port_Dept (No,Name,ParentNo,FK_DeptType,Leader,IsDir) VALUES('1071',' Marketing ','1070','04','lichao',1) ;
INSERT INTO Port_Dept (No,Name,ParentNo,FK_DeptType,Leader,IsDir) VALUES('1072',' Finance Department ','1070','04','linyangyang',1) ;
INSERT INTO Port_Dept (No,Name,ParentNo,FK_DeptType,Leader,IsDir) VALUES('1073',' Sales ','1070','04','tianyi',1) ;

-- Port_StationType ;
DELETE FROM Port_StationType;
INSERT INTO Port_StationType (No,Name) VALUES('1',' High level ');
INSERT INTO Port_StationType (No,Name) VALUES('2',' Middle ');
INSERT INTO Port_StationType (No,Name) VALUES('3',' Grass roots ');
 
-- Port_Station ;
DELETE FROM Port_Station;
INSERT INTO Port_Station (No,Name,FK_StationType) VALUES('01',' General manager ','1') ;
INSERT INTO Port_Station (No,Name,FK_StationType) VALUES('02',' Marketing Manager ','2');
INSERT INTO Port_Station (No,Name,FK_StationType) VALUES('03',' Develop Manager ','2')  ;
INSERT INTO Port_Station (No,Name,FK_StationType) VALUES('04',' Customer Service Manager ','2')  ;
INSERT INTO Port_Station (No,Name,FK_StationType) VALUES('05',' Finance Manager ','2')  ;
INSERT INTO Port_Station (No,Name,FK_StationType) VALUES('06',' Human Resources Manager ','2')  ;
INSERT INTO Port_Station (No,Name,FK_StationType) VALUES('07',' Sales Gang ','3')  ;
INSERT INTO Port_Station (No,Name,FK_StationType) VALUES('08',' Gang Programmer ','3')  ;
INSERT INTO Port_Station (No,Name,FK_StationType) VALUES('09',' Technical Services post ','3')  ;
INSERT INTO Port_Station (No,Name,FK_StationType) VALUES('10',' Cashier Gang ','3')  ;
INSERT INTO Port_Station (No,Name,FK_StationType) VALUES('11',' Human Resources Assistant post ','3')  ;


-- Port_Emp ;
--  General Manager of the Department of  ;
DELETE FROM Port_Emp;
INSERT INTO Port_Emp (No,Name,Pass,FK_Dept,FK_Duty,Leader,Tel,Email,NumOfDept) VALUES('admin','admin','pub','100','01','admin','0531-82374939','zhoupeng@ccflow.org',1)  ;
INSERT INTO Port_Emp (No,Name,Pass,FK_Dept,FK_Duty,Leader,Tel,Email,NumOfDept) VALUES('zhoupeng',' Zhou Peng ','pub','100','02','admin','0531-82374939','zhoupeng@ccflow.org',1)  ;

--  Marketing  ;
INSERT INTO Port_Emp (No,Name,Pass,FK_Dept,FK_Duty,Leader,Tel,Email,NumOfDept) VALUES('zhanghaicheng',' Zhang Haicheng ','pub','1001','03','zhoupeng','0531-82374939','zhanghaicheng@ccflow.org',1)  ;
INSERT INTO Port_Emp (No,Name,Pass,FK_Dept,FK_Duty,Leader,Tel,Email,NumOfDept) VALUES('zhangyifan',' Yifan Zhang ','pub','1001','04','zhanghaicheng','0531-82374939','zhangyifan@ccflow.org',1)  ;
INSERT INTO Port_Emp (No,Name,Pass,FK_Dept,FK_Duty,Leader,Tel,Email,NumOfDept) VALUES('zhoushengyu',' Week liter rain ','pub','1001','04','zhanghaicheng','0531-82374939','zhoushengyu@ccflow.org',1)  ;

--  Develop department  ;
INSERT INTO Port_Emp (No,Name,Pass,FK_Dept,FK_Duty,Leader,Tel,Email,NumOfDept) VALUES('qifenglin',' Qifeng Lin ','pub','1002','03','zhoupeng','0531-82374939','qifenglin@ccflow.org',1)  ;
INSERT INTO Port_Emp (No,Name,Pass,FK_Dept,FK_Duty,Leader,Tel,Email,NumOfDept) VALUES('zhoutianjiao',' Zhou Jiao ','pub','1002','04','qifenglin','0531-82374939','zhoutianjiao@ccflow.org',1)  ;

--  Service Manager  ;
INSERT INTO Port_Emp (No,Name,Pass,FK_Dept,FK_Duty,Leader,Tel,Email,NumOfDept) VALUES('guoxiangbin',' Guoxiang Bin ','pub','1003','03','zhoupeng','0531-82374939','guoxiangbin@ccflow.org',1)  ;
INSERT INTO Port_Emp (No,Name,Pass,FK_Dept,FK_Duty,Leader,Tel,Email,NumOfDept) VALUES('fuhui',' Blessings ','pub','1003','04','guoxiangbin','0531-82374939','fuhui@ccflow.org',1)  ;

--  Finance Department  ;
INSERT INTO Port_Emp (No,Name,Pass,FK_Dept,FK_Duty,Leader,Tel,Email,NumOfDept) VALUES('yangyilei',' According to Yang Lei ','pub','1004','03','zhoupeng','0531-82374939','yangyilei@ccflow.org',1)  ;
INSERT INTO Port_Emp (No,Name,Pass,FK_Dept,FK_Duty,Leader,Tel,Email,NumOfDept) VALUES('guobaogeng',' Guo Baogeng ','pub','1004','04','yangyilei','0531-82374939','guobaogeng@ccflow.org',1) ;

--  Human Resources  ;
INSERT INTO Port_Emp (No,Name,Pass,FK_Dept,FK_Duty,Leader,Tel,Email,NumOfDept) VALUES('liping',' Ping ','pub','1005','03','zhoupeng','0531-82374939','liping@ccflow.org',1)  ;
INSERT INTO Port_Emp (No,Name,Pass,FK_Dept,FK_Duty,Leader,Tel,Email,NumOfDept) VALUES('liyan',' Li Yan ','pub','1005','04','liping','0531-82374939','liyan@ccflow.org',1)  ;


---====  Increase corresponding departments and positions .
DELETE FROM Port_DeptDuty;
INSERT INTO Port_DeptDuty (FK_Dept,FK_Duty) VALUES ('100', '01');
INSERT INTO Port_DeptDuty (FK_Dept,FK_Duty) VALUES ('100', '02');
   --  Marketing 
INSERT INTO Port_DeptDuty (FK_Dept,FK_Duty) VALUES ('1001','03');  
INSERT INTO Port_DeptDuty (FK_Dept,FK_Duty) VALUES ('1001','04');  
   --  Develop department 
INSERT INTO Port_DeptDuty (FK_Dept,FK_Duty) VALUES ('1002','03');  
INSERT INTO Port_DeptDuty (FK_Dept,FK_Duty) VALUES ('1002','04'); 
   --  Customer Service Department 
INSERT INTO Port_DeptDuty (FK_Dept,FK_Duty) VALUES ('1003','03');  
INSERT INTO Port_DeptDuty (FK_Dept,FK_Duty) VALUES ('1003','04');  
   --  Finance Department 
INSERT INTO Port_DeptDuty (FK_Dept,FK_Duty) VALUES ('1004','03');  
INSERT INTO Port_DeptDuty (FK_Dept,FK_Duty) VALUES ('1004','04');  
   --  Human Resources 
INSERT INTO Port_DeptDuty (FK_Dept,FK_Duty) VALUES ('1005','03');  
INSERT INTO Port_DeptDuty (FK_Dept,FK_Duty) VALUES ('1005','04');


--====  Departments and the corresponding increase in jobs .
delete FROM Port_DeptStation;

INSERT INTO Port_DeptStation (FK_Dept,FK_Station) VALUES ('100', '01');
   --  Marketing 
INSERT INTO Port_DeptStation (FK_Dept,FK_Station) VALUES ('1001','02');  
INSERT INTO Port_DeptStation (FK_Dept,FK_Station) VALUES ('1001','07');  
   --  Develop department 
INSERT INTO Port_DeptStation (FK_Dept,FK_Station) VALUES ('1002','03');  
INSERT INTO Port_DeptStation (FK_Dept,FK_Station) VALUES ('1002','08'); 
   --  Customer Service Department 
INSERT INTO Port_DeptStation (FK_Dept,FK_Station) VALUES ('1003','04');  
INSERT INTO Port_DeptStation (FK_Dept,FK_Station) VALUES ('1003','09');  
   --  Finance Department 
INSERT INTO Port_DeptStation (FK_Dept,FK_Station) VALUES ('1004','05');  
INSERT INTO Port_DeptStation (FK_Dept,FK_Station) VALUES ('1004','10');  
   --  Human Resources 
INSERT INTO Port_DeptStation (FK_Dept,FK_Station) VALUES ('1005','06');  
INSERT INTO Port_DeptStation (FK_Dept,FK_Station) VALUES ('1005','11');
  
 
-- Port_DeptEmp  Correspondence with the department staff  ;
DELETE FROM Port_DeptEmp;
INSERT INTO Port_DeptEmp (MyPK,FK_Emp,FK_Dept,FK_Duty,DutyLevel,Leader) VALUES('100_zhoupeng','zhoupeng','100','02',10,'zhoupeng') ;

--  Marketing  ;
INSERT INTO Port_DeptEmp (MyPK,FK_Emp,FK_Dept,FK_Duty,DutyLevel,Leader) VALUES('1001_zhanghaicheng','zhanghaicheng','1001','03',20,'zhoupeng') ;
INSERT INTO Port_DeptEmp (MyPK,FK_Emp,FK_Dept,FK_Duty,DutyLevel,Leader) VALUES('1001_zhangyifan','zhangyifan','1001','04',20,'zhanghaicheng') ;
INSERT INTO Port_DeptEmp (MyPK,FK_Emp,FK_Dept,FK_Duty,DutyLevel,Leader) VALUES('1001_zhoushengyu','zhoushengyu','1001','04',20,'zhanghaicheng') ;

--  Develop department  ;
INSERT INTO Port_DeptEmp (MyPK,FK_Emp,FK_Dept,FK_Duty,DutyLevel,Leader) VALUES('1002_qifenglin','qifenglin','1002','03',20,'zhoupeng') ;
INSERT INTO Port_DeptEmp (MyPK,FK_Emp,FK_Dept,FK_Duty,DutyLevel,Leader) VALUES('1002_zhoutianjiao','zhoutianjiao','1002','04',20,'qifenglin') ;

--  Service Manager  ;
INSERT INTO Port_DeptEmp (MyPK,FK_Emp,FK_Dept,FK_Duty,DutyLevel,Leader) VALUES('1003_guoxiangbin','guoxiangbin','1003','03',20,'zhoupeng') ;
INSERT INTO Port_DeptEmp (MyPK,FK_Emp,FK_Dept,FK_Duty,DutyLevel,Leader) VALUES('1003_fuhui','fuhui','1003','04',20,'guoxiangbin') ;

--  Finance Department  ;
INSERT INTO Port_DeptEmp (MyPK,FK_Emp,FK_Dept,FK_Duty,DutyLevel,Leader) VALUES('1004_yangyilei','yangyilei','1004','03',20,'zhoupeng') ;
INSERT INTO Port_DeptEmp (MyPK,FK_Emp,FK_Dept,FK_Duty,DutyLevel,Leader) VALUES('1004_guobaogeng','guobaogeng','1004','04',20,'yangyilei') ;

--  Human Resources  ;
INSERT INTO Port_DeptEmp (MyPK,FK_Emp,FK_Dept,FK_Duty,DutyLevel,Leader) VALUES('1005_liping','liping','1005','03',20,'zhoupeng') ;
INSERT INTO Port_DeptEmp (MyPK,FK_Emp,FK_Dept,FK_Duty,DutyLevel,Leader) VALUES('1005_liyan','liyan','1005','04',20,'liping') ;

-- Port_DeptEmpStation  Counterparts and positions  ;
INSERT INTO Port_DeptEmpStation (MyPK,FK_Dept,FK_Emp,FK_Station) VALUES('100_zhoupeng_01','100','zhoupeng','01')  ;

--  Marketing ; 
INSERT INTO Port_DeptEmpStation (MyPK,FK_Dept,FK_Emp,FK_Station) VALUES('1001_zhanghaicheng_01','1001','zhanghaicheng','02')  ;
INSERT INTO Port_DeptEmpStation (MyPK,FK_Dept,FK_Emp,FK_Station) VALUES('1001_zhangyifan_01','1001','zhangyifan','07')  ;
INSERT INTO Port_DeptEmpStation (MyPK,FK_Dept,FK_Emp,FK_Station) VALUES('1001_zhoushengyu_01','1001','zhoushengyu','07')  ;

--  Develop department  ;
INSERT INTO Port_DeptEmpStation (MyPK,FK_Dept,FK_Emp,FK_Station) VALUES('1002_qifenglin_01','1002','qifenglin','03')  ;
INSERT INTO Port_DeptEmpStation (MyPK,FK_Dept,FK_Emp,FK_Station) VALUES('1002_zhoutianjiao_01','1002','zhoutianjiao','08')  ;

--   Services ;
INSERT INTO Port_DeptEmpStation (MyPK,FK_Dept,FK_Emp,FK_Station) VALUES('1003_guoxiangbin_01','1003','guoxiangbin','04');
INSERT INTO Port_DeptEmpStation (MyPK,FK_Dept,FK_Emp,FK_Station) VALUES('1003_fuhui_01','fuhui','1003','09') ; 


--  Finance Department ;
INSERT INTO Port_DeptEmpStation (MyPK,FK_Dept,FK_Emp,FK_Station) VALUES('1004_yangyilei_01','1004','yangyilei','05')   ;
INSERT INTO Port_DeptEmpStation (MyPK,FK_Dept,FK_Emp,FK_Station) VALUES('1004_guobaogeng_01','1004','guobaogeng','10')  ;

--  Human resources department ;
INSERT INTO Port_DeptEmpStation (MyPK,FK_Dept,FK_Emp,FK_Station) VALUES('1005_liping_01','1005','liping','06')  ;
INSERT INTO Port_DeptEmpStation (MyPK,FK_Dept,FK_Emp,FK_Station) VALUES('1005_liyan_01','1005','liyan','11')  ;

 