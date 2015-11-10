DELETE FROM Port_Dept;
DELETE FROM Port_Station;
DELETE FROM Port_Emp;
DELETE FROM Port_EmpStation;
DELETE FROM Port_EmpDept;

 
-- Port_Dept ;
INSERT INTO Port_Dept (No,Name,ParentNo) VALUES('1',' General Manager Office ','0');
INSERT INTO Port_Dept (No,Name,ParentNo) VALUES('2',' Marketing ','1')  ;
INSERT INTO Port_Dept (No,Name,ParentNo) VALUES('3',' Develop department ','1')  ;
INSERT INTO Port_Dept (No,Name,ParentNo) VALUES('4',' Services ','1')  ;
INSERT INTO Port_Dept (No,Name,ParentNo) VALUES('5',' Finance Department ','1')  ;
INSERT INTO Port_Dept (No,Name,ParentNo) VALUES('6',' Human Resources ','1');

-- Port_Station ;
INSERT INTO Port_Station (No,Name,StaGrade) VALUES('01',' General manager ','1')  ;
INSERT INTO Port_Station (No,Name,StaGrade) VALUES('02',' Marketing Manager ','2')  ;
INSERT INTO Port_Station (No,Name,StaGrade) VALUES('03',' Develop Manager ','2')  ;
INSERT INTO Port_Station (No,Name,StaGrade) VALUES('04',' Customer Service Manager ','2')  ;
INSERT INTO Port_Station (No,Name,StaGrade) VALUES('05',' Finance Manager ','2')  ;
INSERT INTO Port_Station (No,Name,StaGrade) VALUES('06',' Human Resources Manager ','2')  ;
INSERT INTO Port_Station (No,Name,StaGrade) VALUES('07',' Sales Gang ','3')  ;
INSERT INTO Port_Station (No,Name,StaGrade) VALUES('08',' Gang Programmer ','3')  ;
INSERT INTO Port_Station (No,Name,StaGrade) VALUES('09',' Technical Services post ','3')  ;
INSERT INTO Port_Station (No,Name,StaGrade) VALUES('10',' Cashier Gang ','3')  ;
INSERT INTO Port_Station (No,Name,StaGrade) VALUES('11',' Human Resources Assistant post ','3')  ;

-- Port_Emp ;
--  General Manager of the Department of  ;
INSERT INTO Port_Emp (No,Name,Pass,FK_Dept) VALUES('admin','admin','pub','1')  ;
INSERT INTO Port_Emp (No,Name,Pass,FK_Dept) VALUES('zhoupeng',' Zhou Peng ','pub','1')  ;

--  Marketing  ;
INSERT INTO Port_Emp (No,Name,Pass,FK_Dept) VALUES('zhanghaicheng',' Zhang Haicheng ','pub','2')  ;
INSERT INTO Port_Emp (No,Name,Pass,FK_Dept) VALUES('zhangyifan',' Yifan Zhang ','pub','2')  ;
INSERT INTO Port_Emp (No,Name,Pass,FK_Dept) VALUES('zhoushengyu',' Week liter rain ','pub','2')  ;

--  Develop department  ;
INSERT INTO Port_Emp (No,Name,Pass,FK_Dept) VALUES('qifenglin',' Qifeng Lin ','pub','3')  ;
INSERT INTO Port_Emp (No,Name,Pass,FK_Dept) VALUES('zhoutianjiao',' Zhou Jiao ','pub','3')  ;

--  Service Manager  ;
INSERT INTO Port_Emp (No,Name,Pass,FK_Dept) VALUES('guoxiangbin',' Guoxiang Bin ','pub','4')  ;
INSERT INTO Port_Emp (No,Name,Pass,FK_Dept) VALUES('fuhui',' Blessings ','pub','4')  ;

--  Finance Department  ;
INSERT INTO Port_Emp (No,Name,Pass,FK_Dept) VALUES('yangyilei',' According to Yang Lei ','pub','5')  ;
INSERT INTO Port_Emp (No,Name,Pass,FK_Dept) VALUES('guobaogeng',' Guo Baogeng ','pub','5') ;

--  Human Resources  ;
INSERT INTO Port_Emp (No,Name,Pass,FK_Dept) VALUES('liping',' Ping ','pub','6')  ;
INSERT INTO Port_Emp (No,Name,Pass,FK_Dept) VALUES('liyan',' Li Yan ','pub','6')  ;

 
-- Port_EmpDept  Correspondence with the department staff  ;
INSERT INTO Port_EmpDept (FK_Emp,FK_Dept) VALUES('zhoupeng','1') ;

--  Marketing  ;
INSERT INTO Port_EmpDept (FK_Emp,FK_Dept) VALUES('zhanghaicheng','2') ;
INSERT INTO Port_EmpDept (FK_Emp,FK_Dept) VALUES('zhangyifan','2') ;
INSERT INTO Port_EmpDept (FK_Emp,FK_Dept) VALUES('zhoushengyu','2') ;

--  Develop department  ;
INSERT INTO Port_EmpDept (FK_Emp,FK_Dept) VALUES('qifenglin','3') ;
INSERT INTO Port_EmpDept (FK_Emp,FK_Dept) VALUES('zhoutianjiao','3') ;

--  Service Manager  ;
INSERT INTO Port_EmpDept (FK_Emp,FK_Dept) VALUES('guoxiangbin','4') ;
INSERT INTO Port_EmpDept (FK_Emp,FK_Dept) VALUES('fuhui','4') ;

--  Finance Department  ;
INSERT INTO Port_EmpDept (FK_Emp,FK_Dept) VALUES('yangyilei','5') ;
INSERT INTO Port_EmpDept (FK_Emp,FK_Dept) VALUES('guobaogeng','5') ;

--  Human Resources  ;
INSERT INTO Port_EmpDept (FK_Emp,FK_Dept) VALUES('liping','6') ;
INSERT INTO Port_EmpDept (FK_Emp,FK_Dept) VALUES('liyan','6') ;

-- Port_EmpStation  Counterparts and positions  ;
INSERT INTO Port_EmpStation (FK_Emp,FK_Station) VALUES('zhoupeng','01')  ;

--  Marketing ;
INSERT INTO Port_EmpStation (FK_Emp,FK_Station) VALUES('zhanghaicheng','02')  ;
INSERT INTO Port_EmpStation (FK_Emp,FK_Station) VALUES('zhangyifan','07')  ;
INSERT INTO Port_EmpStation (FK_Emp,FK_Station) VALUES('zhoushengyu','07')  ;

--  Develop department  ;
INSERT INTO Port_EmpStation (FK_Emp,FK_Station) VALUES('qifenglin','03')  ;
INSERT INTO Port_EmpStation (FK_Emp,FK_Station) VALUES('zhoutianjiao','08')  ;

-- Services ;
INSERT INTO Port_EmpStation (FK_Emp,FK_Station) VALUES('guoxiangbin','04');
INSERT INTO Port_EmpStation (FK_Emp,FK_Station) VALUES('fuhui','09') ; 


--  Finance Department ;
INSERT INTO Port_EmpStation (FK_Emp,FK_Station) VALUES('yangyilei','05')   ;
INSERT INTO Port_EmpStation (FK_Emp,FK_Station) VALUES('guobaogeng','10')  ;

--  Human resources department ;
INSERT INTO Port_EmpStation (FK_Emp,FK_Station) VALUES('liping','06')  ;
INSERT INTO Port_EmpStation (FK_Emp,FK_Station) VALUES('liyan','11')  ;
 