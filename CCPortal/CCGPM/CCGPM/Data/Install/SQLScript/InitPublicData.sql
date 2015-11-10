
DELETE FROM GPM_LinkSort;
INSERT INTO GPM_LinkSort(No,Name) VALUES ('01','驰骋产品');
INSERT INTO GPM_LinkSort(No,Name) VALUES ('02','门户网站');
INSERT INTO GPM_LinkSort(No,Name) VALUES ('03','相关链接');
 
DELETE FROM GPM_Link;
INSERT INTO GPM_Link(No,Name,URL,FK_Sort,OpenWay) VALUES ('01','驰骋工作流程管理系统','http://ccflow.org','01',0);
INSERT INTO GPM_Link(No,Name,URL,FK_Sort,OpenWay) VALUES ('02','驰骋办公自动化','http://www.chichengoa.org/','01',0);  /*此处地址被孙战平改动*/
INSERT INTO GPM_Link(No,Name,URL,FK_Sort,OpenWay) VALUES ('03','驰骋单点登录系统','http://ccflow.org','01',0);
INSERT INTO GPM_Link(No,Name,URL,FK_Sort,OpenWay) VALUES ('04','驰骋关系型表单设计器','http://ccflow.org','01',0);

INSERT INTO GPM_Link(No,Name,URL,FK_Sort,OpenWay) VALUES ('10','新浪网sina','http://sina.com.cn','02',0);
INSERT INTO GPM_Link(No,Name,URL,FK_Sort,OpenWay) VALUES ('11','百度baidu','http://baidu.com','02',0);
INSERT INTO GPM_Link(No,Name,URL,FK_Sort,OpenWay) VALUES ('12','谷歌google','http://google.com.cn','02',0);

INSERT INTO GPM_Link(No,Name,URL,FK_Sort,OpenWay) VALUES ('20','国家总局','http://ccflow.org','03',0);
INSERT INTO GPM_Link(No,Name,URL,FK_Sort,OpenWay) VALUES ('21','xxxx省局','http://ccflow.org','03',0);
 

DELETE FROM GPM_Bar;
INSERT INTO GPM_Bar(No,Name,Title,Tag1,Tag2) VALUES ('01','信息块测试','信息块测试1','SELECT No,Name FROM PORT_DEPT','http://ccflow.org');
INSERT INTO GPM_Bar(No,Name,Title,Tag1,Tag2) VALUES ('02','信息块测试','信息块测试2','SELECT No,Name FROM PORT_DEPT','http://ccflow.org');
INSERT INTO GPM_Bar(No,Name,Title,Tag1,Tag2) VALUES ('03','信息块测试','信息块测试3','SELECT No,Name FROM PORT_DEPT','http://ccflow.org');
INSERT INTO GPM_Bar(No,Name,Title,Tag1,Tag2) VALUES ('04','信息块测试','信息块测试4','SELECT No,Name FROM PORT_DEPT','http://ccflow.org');
INSERT INTO GPM_Bar(No,Name,Title,Tag1,Tag2) VALUES ('05','信息块测试','信息块测试5','SELECT No,Name FROM PORT_DEPT','http://ccflow.org');
INSERT INTO GPM_Bar(No,Name,Title,Tag1,Tag2) VALUES ('06','信息块测试','信息块测试6','SELECT No,Name FROM PORT_DEPT','http://ccflow.org');
INSERT INTO GPM_Bar(No,Name,Title,Tag1,Tag2) VALUES ('07','信息块测试','信息块测试7','SELECT No,Name FROM PORT_DEPT','http://ccflow.org');
INSERT INTO GPM_Bar(No,Name,Title,Tag1,Tag2) VALUES ('08','信息块测试','信息块测试8','SELECT No,Name FROM PORT_DEPT','http://ccflow.org');
INSERT INTO GPM_Bar(No,Name,Title,Tag1,Tag2) VALUES ('09','信息块测试','信息块测试9','SELECT No,Name FROM PORT_DEPT','http://ccflow.org');
INSERT INTO GPM_Bar(No,Name,Title,Tag1,Tag2) VALUES ('10','信息块测试','信息块测试10','SELECT No,Name FROM PORT_DEPT','http://ccflow.org');
INSERT INTO GPM_Bar(No,Name,Title,Tag1,Tag2) VALUES ('11','信息块测试','信息块测试11','SELECT No,Name FROM PORT_DEPT','http://ccflow.org');

-- 系统类别.
DELETE FROM GPM_AppSort;
INSERT INTO GPM_AppSort(No,Name,RefMenuNo) VALUES ('01','业务系统','2000');
INSERT INTO GPM_AppSort(No,Name,RefMenuNo) VALUES ('02','办公系统','2001');

-- 系统.
DELETE FROM GPM_App;
INSERT INTO GPM_App(No,Name,AppModel,Url,MyFileName,MyFilePath,MyFileExt,WebPath,FK_AppSort,RefMenuNo) VALUES ('CCOA','驰骋OA',0,'http://www.chichengoa.org/app/login/login.ashx','ccoa.png','Path','GIF','/DataUser/BP.GPM.STem/CCOA.png','01','2002'); /*此处地址被孙战平改动*/
INSERT INTO GPM_App(No,Name,AppModel,Url,MyFileName,MyFilePath,MyFileExt,WebPath,FK_AppSort,RefMenuNo) VALUES ('CCFlow','工作流程',0,'http://localhost:8082/ccflow/WF/Login.aspx','admin.gif','Path','GIF','Img/workflow.png','01','2004');
INSERT INTO GPM_App(No,Name,AppModel,Url,MyFileName,MyFilePath,MyFileExt,WebPath,FK_AppSort,RefMenuNo) VALUES ('CCIM','即时通讯',1,'http://localhost/ccflow/WF/Login.aspx','admin.gif','Path','GIF','Img/im.png','01','2005');
INSERT INTO GPM_App(No,Name,AppModel,Url,MyFileName,MyFilePath,MyFileExt,WebPath,FK_AppSort,RefMenuNo) VALUES ('GGXXW','公共信息网',1,'http://localhost:8083/','db.gif','Path','GIF','Img/common.png','02','2006');
INSERT INTO GPM_App(No,Name,AppModel,Url,MyFileName,MyFilePath,MyFileExt,WebPath,FK_AppSort,RefMenuNo) VALUES ('GPM','权限管理',1,'/GPM/Default.aspx','MyWork.gif','Path','GIF','Img/auth.png','02','2007');
INSERT INTO GPM_App(No,Name,AppModel,Url,MyFileName,MyFilePath,MyFileExt,WebPath,FK_AppSort,RefMenuNo) VALUES ('SSO','单点登陆',1,'/SSO/Default.aspx','RptDir.gif','Path','GIF','Img/sso.png','02','2008');


DELETE FROM GPM_Menu;
-- root;
INSERT INTO GPM_Menu(FK_App,No,ParentNo,TreeNo,Name,MenuType,Url,IsDir,IsEnable) VALUES ('UnitFullName','1000','0','01','济南驰骋信息技术有限公司',0,'',1,1);
INSERT INTO GPM_Menu(FK_App,No,ParentNo,TreeNo,Name,MenuType,Url,IsDir,IsEnable) VALUES ('AppSort','2000','1000','0101','业务系统',1,'',1,1);
INSERT INTO GPM_Menu(FK_App,No,ParentNo,TreeNo,Name,MenuType,Url,IsDir,IsEnable) VALUES ('AppSort','2001','1000','0102','办公系统',1,'',1,1);
INSERT INTO GPM_Menu(FK_App,No,ParentNo,TreeNo,Name,MenuType,Url,IsDir,IsEnable) VALUES ('CCOA','2002','2000','0103','驰骋OA',2,'',1,1);
INSERT INTO GPM_Menu(FK_App,No,ParentNo,TreeNo,Name,MenuType,Url,IsDir,IsEnable) VALUES ('Tester','2003','2001','0104','Tester',2,'',1,1);
INSERT INTO GPM_Menu(FK_App,No,ParentNo,TreeNo,Name,MenuType,Url,IsDir,IsEnable) VALUES ('CCFlow','2004','2000','0105','工作流程',2,'',1,1);
INSERT INTO GPM_Menu(FK_App,No,ParentNo,TreeNo,Name,MenuType,Url,IsDir,IsEnable) VALUES ('CCIM','2005','2000','0106','即时通讯',2,'',1,1);
INSERT INTO GPM_Menu(FK_App,No,ParentNo,TreeNo,Name,MenuType,Url,IsDir,IsEnable) VALUES ('GGXXW','2006','2000','0107','公共信息网',2,'',1,1);
INSERT INTO GPM_Menu(FK_App,No,ParentNo,TreeNo,Name,MenuType,Url,IsDir,IsEnable) VALUES ('GPM','2007','2000','0108','权限管理',2,'',1,1);
INSERT INTO GPM_Menu(FK_App,No,ParentNo,TreeNo,Name,MenuType,Url,IsDir,IsEnable) VALUES ('SSO','2008','2000','0109','单点登陆',2,'',1,1);
-- CCOA 菜单;
/*此处菜单被孙战平改动 at 2013.07.18*/;
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1010','我的工作','2002','0101','1','0','3','CCOA','','1','','','','','','','','','0','0','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1011','我的计划','1010','010101','0','2','4','CCOA','/App/PrivPlan/MyPlan.aspx','1','','','','','files.gif','','gif','//DataUser/BP.GPM.Menu/1011.gif','16','16','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1012','公告通知','2002','0102','1','0','3','CCOA','','1','','','','','','','','','0','0','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1013','公告管理','1012','010201','1','0','4','CCOA','/App/Notice/NoticeList.aspx','1','','','','','framework.gif','','gif','//DataUser/BP.GPM.Menu/1013.gif','16','16','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1014','添加公告','1012','10202','0','0','4','CCOA','/App/Notice/NewNotice.aspx','1','','','','','new3.gif','','gif','//DataUser/BP.GPM.Menu/1014.gif','16','16','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1015','我的公告','1012','10203','0','0','4','CCOA','/App/Notice/MyNoticeList.aspx','1','','','','','data3.gif','','gif','//DataUser/BP.GPM.Menu/1015.gif','16','16','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('2011','公告类型','1012','10204','0','1','3','CCOA','/Comm/Search.aspx?EnsName=BP.OA.NoticeCategorys','1','','','','','data3.png','','png','//DataUser/BP.GPM.Menu/1015.png','16','16','0');

-- 邮件管理
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1016','邮件管理','2002','0103','1','0','3','CCOA','','1','','','','','','','','','0','0','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1017','写邮件','1016','010301','0','0','4','CCOA','/App/Message/SendMsg.aspx','1','','','','','add.png','','png','//DataUser/BP.GPM.Menu/1017.png','16','16','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1018','收件箱','1016','10302','0','0','4','CCOA','/app/message/InBox.aspx','1','','','','','calendar1.gif','','gif','//DataUser/BP.GPM.Menu/1018.gif','16','16','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1019','发件箱','1016','10303','0','0','4','CCOA','/app/Message/SendBox.aspx','1','','','','','aremove.png','','png','//DataUser/BP.GPM.Menu/1019.png','16','16','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1020','新闻管理','2002','0104','1','0','3','CCOA','','1','','','','','','','','','0','0','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1021','添加新闻','1020','010401','0','0','4','CCOA','/app/News/NewsAdd.aspx','1','','','','','pen.gif','','gif','//DataUser/BP.GPM.Menu/1021.gif','16','16','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1022','新闻管理','1020','10402','0','0','4','CCOA','/app/News/NewsList.aspx','1','','','','','ico_news.gif','','gif','//DataUser/BP.GPM.Menu/1022.gif','16','16','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1024','新闻栏目','1020','10404','0','0','4','CCOA','/Comm/Search.aspx?EnsName=BP.OA.Article.ArticleCatagorys','1','','','','','sharedir.gif','','gif','//DataUser/BP.GPM.Menu/1024.gif','16','16','0');

-- 流程管理
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1025','流程管理','2002','0105','1','0','3','CCOA','','1','','','','','','','','','0','0','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1026','发起','1025','010501','1','0','3','CCOA','/WF/Start.aspx','1','','','','','data1.gif','','gif','//DataUser/BP.GPM.Menu/1026.gif','16','16','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1027','待办','1025','10502','0','0','3','CCOA','/WF/EmpWorks.aspx','1','','','','','search1.png','','png','//DataUser/BP.GPM.Menu/1027.png','16','16','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1028','抄送','1025','10503','0','0','3','CCOA','/WF/CC.aspx','1','','','','','data1.gif','','gif','//DataUser/BP.GPM.Menu/1028.gif','16','16','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1029','挂起','1025','10504','0','0','3','CCOA','/WF/HungupList.aspx','1','','','','','data5.gif','','gif','//DataUser/BP.GPM.Menu/1029.gif','16','16','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1030','在途','1025','10505','0','0','3','CCOA','/WF/Runing.aspx','1','','','','','data4.gif','','gif','//DataUser/BP.GPM.Menu/1030.gif','16','16','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1031','查询','1025','10506','0','0','3','CCOA','/WF/FlowSearch.aspx','1','','','','','ico_search.gif','','gif','//DataUser/BP.GPM.Menu/1031.gif','16','16','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1032','关键字查询','1025','10507','0','0','3','CCOA','/WF/KeySearch.aspx','1','','','','','cache.gif','','gif','//DataUser/BP.GPM.Menu/1032.gif','16','16','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1033','取消审批','1025','10508','0','0','3','CCOA','/WF/GetTask.aspx','1','','','','','color.gif','','gif','//DataUser/BP.GPM.Menu/1033.gif','16','16','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1034','成员','1025','10509','0','0','3','CCOA','/WF/Emps.aspx','1','','','','','data3.gif','','gif','//DataUser/BP.GPM.Menu/1034.gif','16','16','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1036','设置','1025','10511','0','0','3','CCOA','/WF/Tools.aspx','1','','','','','ico_html.gif','','gif','//DataUser/BP.GPM.Menu/1036.gif','16','16','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1037','我的日程','1010','010102','0','1','4','CCOA','/app/Calendar/MyCalendar.aspx','1','','','','','calendar.png','','png','//DataUser/BP.GPM.Menu/1037.png','16','16','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1038','创建流程','1025','10512','0','0','3','CCOA','/WF/Admin/XAP/Designer.aspx','1','','','','','com.gif','','gif','//DataUser/BP.GPM.Menu/1038.gif','16','16','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1039','草稿箱','1016','10304','0','0','4','CCOA','/app/Message/DraftBox.aspx','1','','','','','Rpt.gif','','gif','//DataUser/BP.GPM.Menu/1039.gif','16','16','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('1040','垃圾箱','1016','10305','0','0','4','CCOA','/App/Message/RecycleBox.aspx','1','','','','','admin.gif','','gif','//DataUser/BP.GPM.Menu/1040.gif','17','19','0');
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) Values('2010','发送短消息','1010','10103','0','3','3','CCOA','/Main/Sys/SendShortMsg.aspx','1','','','','','new1.gif','','gif','//DataUser/BP.GPM.Menu/2010.gif','16','16','0');

--配置办公用品管理
DELETE GPM_Menu WHERE No LIKE '21%';

--目录.
INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize) 
Values('2101','办公用品管理','2002','0105','1','0','3','CCOA','','1','','','','','','','','','0','0','0');

INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize)
 Values('2111','类别维护','2101','10103','0','3','3','CCOA','/Comm/Ens.aspx?EnsName=BP.DS.DSSorts','1','','','','','new1.gif','','gif','//DataUser/BP.GPM.Menu/2010.gif','16','16','0');

INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize)
 Values('2112','库存台帐','2101','10103','0','3','3','CCOA','/Comm/Search.aspx?EnsName=BP.DS.DSMains','1','','','','','new1.gif','','gif','//DataUser/BP.GPM.Menu/2010.gif','16','16','0');

INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize)
 Values('2113','采购台帐','2101','10103','0','3','3','CCOA','/Comm/Search.aspx?EnsName=BP.DS.DSBuys','1','','','','','new1.gif','','gif','//DataUser/BP.GPM.Menu/2010.gif','16','16','0');

INSERT INTO GPM_Menu (No,Name,ParentNo,TreeNo,IsDir,Idx,MenuType,FK_App,Url,IsEnable,Flag,Tag1,Tag2,Tag3,MyFileName,MyFilePath,MyFileExt,WebPath,MyFileH,MyFileW,MyFileSize)
 Values('2114','领用台帐','2101','10103','0','3','3','CCOA','/Comm/Search.aspx?EnsName=BP.DS.DSTakes','1','','','','','new1.gif','','gif','//DataUser/BP.GPM.Menu/2010.gif','16','16','0');

 


DELETE FROM GPM_UserMenu;
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1000','0');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1010','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1011','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1012','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1013','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1014','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1015','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1016','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1017','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1018','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1019','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1020','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1021','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1022','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1024','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1025','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1026','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1027','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1028','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1029','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1030','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1031','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1032','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1033','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1034','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1035','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1036','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1037','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1038','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1039','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','1040','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','2000','0');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','2002','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','2010','1');
INSERT INTO GPM_UserMenu (FK_Emp,FK_Menu,IsChecked) Values('admin','2011','1');
 

DELETE FROM GPM_InfoPush;
INSERT INTO GPM_InfoPush(No,Name,Url,GetSQL,WebPath) VALUES ('Email','新邮件','http://mail.google.com','SELECT COUNT(*) FROM PORT_EMP','Img/email.png');
INSERT INTO GPM_InfoPush(No,Name,Url,GetSQL,WebPath) VALUES ('Msg','系统消息','http://mail.google.com','SELECT COUNT(*) FROM PORT_EMP','Img/msg.png');
INSERT INTO GPM_InfoPush(No,Name,Url,GetSQL,WebPath) VALUES ('MyWork','待办工作','http://localhost/ccflow/WF/EmpWorks.aspx','SELECT COUNT(*) FROM PORT_EMP','/DataUser/BP.GPM.InfoPush/MyWork.gif');
INSERT INTO GPM_InfoPush(No,Name,Url,GetSQL,WebPath) VALUES ('News','未读新闻','http://sina.com.cn','SELECT COUNT(*) FROM PORT_EMP','/DataUser/BP.GPM.InfoPush/News.gif');
INSERT INTO GPM_InfoPush(No,Name,Url,GetSQL,WebPath) VALUES ('Notice','未读公告','http://sina.com.cn','SELECT COUNT(*) FROM PORT_EMP','/DataUser/BP.GPM.InfoPush/Notice.gif');
INSERT INTO GPM_InfoPush(No,Name,Url,GetSQL,WebPath) VALUES ('Ontheway','在途工作','http://sina.com.cn','SELECT COUNT(*) FROM PORT_EMP','/DataUser/BP.GPM.InfoPush/Ontheway.gif');

DELETE FROM SYS_GloVar;
INSERT INTO SYS_GloVar(No,Name,Val,GroupKey,Note) VALUES ('ColsOfApp','SSO界面应用系统列数','2','APP','SSO界面应用系统列数。');
INSERT INTO SYS_GloVar(No,Name,Val,GroupKey,Note) VALUES ('ColsOfSSO','SSO界面信息列数','3','SSO','SSO界面信息列数,单点登陆界面中的列数。');
INSERT INTO SYS_GloVar(No,Name,Val,GroupKey,Note) VALUES ('UnitFullName','单位全称','济南驰骋信息技术有限公司','Glo','');
INSERT INTO SYS_GloVar(No,Name,Val,GroupKey,Note) VALUES ('UnitSimpleName','单位简称','驰骋软件','Glo','');
