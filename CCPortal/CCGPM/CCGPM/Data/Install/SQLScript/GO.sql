/** 以下代码由孙战平 2013.07.19 */
--GO--
CREATE VIEW V_GPM_EmpGroup
/* 人员与权限组对应 */
AS
SELECT FK_Group,FK_Emp FROM GPM_GroupEmp
UNION
SELECT a.FK_Group,B.FK_Emp FROM GPM_GroupStation a, Port_DeptEmpStation b 
WHERE a.FK_Station=b.FK_Station

--GO-- 
CREATE VIEW V_GPM_EmpGroupMenu
/* 人员-权限组-菜单对应 */
AS
SELECT a.FK_Group,a.FK_Emp,b.FK_Menu,b.IsChecked FROM V_GPM_EmpGroup a, GPM_GroupMenu b 
WHERE a.FK_Group=b.FK_Group


--GO--
CREATE VIEW V_GPM_EmpMenu
AS
/* 人员与菜单对应 */
SELECT a.FK_Emp+'_'+a.FK_Menu as MyPK, a.FK_Emp, b.No as FK_Menu,a.IsChecked, b.* FROM 
   GPM_UserMenu a,GPM_Menu b WHERE a.FK_Menu=b.No AND B.IsEnable=1
UNION
SELECT a.FK_Emp+'_'+a.FK_Menu as MyPK, a.FK_Emp, b.No as FK_Menu,a.IsChecked, b.* FROM 
  V_GPM_EmpGroupMenu a,GPM_Menu b  WHERE a.FK_Menu=b.No AND B.IsEnable=1

--GO--
CREATE VIEW V_GPM_EmpMenu_GPM
AS
select distinct c.* from (
/* 人员与菜单对应 */
SELECT a.FK_Emp+'_'+a.FK_Menu as MyPK, a.FK_Emp,a.IsChecked, b.No as FK_Menu, b.* FROM 
   GPM_UserMenu a,GPM_Menu b WHERE a.FK_Menu=b.No AND B.IsEnable=1
UNION
SELECT a.FK_Emp+'_'+a.FK_Menu as MyPK, a.FK_Emp,a.IsChecked, b.No as FK_Menu, b.* FROM 
  V_GPM_EmpGroupMenu a,GPM_Menu b  WHERE a.FK_Menu=b.No AND B.IsEnable=1
) c

--GO--
Create FUNCTION [dbo].[f_FixNumLen] 
(
	@str nvarchar(200),
	@len int
)
RETURNS nvarchar(200)
AS
BEGIN
    if(@len>len(@str))
	   set @str = replicate('0',@len-len(@str))+@str;
	return @str;
END

--GO--
Create PROCEDURE [dbo].[sp_get_tree_table]
 (@table_name nvarchar(200), @id nvarchar(50), @name nvarchar(50), @parent_id nvarchar(50) , @orderBy nvarchar(50), @startParentId nvarchar(20),@is_top bit, @FixLenFlag int)
AS
    declare @v_affect int
    declare @v_level int
    declare @sql nvarchar(500)
begin
    create table #temp (id nvarchar(20),name nvarchar(50),parent_id nvarchar(20),level int,treespace nvarchar(500),orderid int);
    declare @strTable varchar(200);
    set @table_name=LTRIM(@table_name);
    if(charindex('from ',lower(@table_name))=1)
        set @strTable='select * '+@table_name;
    else
    Begin
		if(charindex('select ',lower(@table_name))<>1 and charindex(' where ',lower(@table_name))>1)
		   set @strTable='select * from '+@table_name;
		else
		   set @strTable=@table_name;
    End
    if(charindex('select ',lower(@strTable))=1)
      set @strTable='('+@strTable+')'    
    if(charindex('(select ',lower(@strTable))=1)
      set @sql='insert into #temp(id,name,parent_id,orderid) select '+@id+','+@name+','+@parent_id +','+@orderBy+' from ' + @strTable +' as A';
    else
      set @sql='insert into #temp(id,name,parent_id,orderid) select '+@id+','+@name+','+@parent_id +','+@orderBy+' from ' + @strTable +'';
    exec sp_executesql @sql;
    set @v_level=1;
    set @v_affect=1;
    while(@v_affect>0)
    Begin
        if @v_level=1
           if @startParentId is null
              update #temp 
              set level=@v_level,treespace=dbo.f_FixNumLen(Cast(orderid as varchar),@FixLenFlag) +'-'+dbo.f_FixNumLen(Cast(id as varchar),@FixLenFlag) 
              where parent_id is null;
           else
			   update #temp 
			   set level=@v_level,treespace=dbo.f_FixNumLen(Cast(orderid as varchar),@FixLenFlag) +'-'+dbo.f_FixNumLen(Cast(id as varchar),@FixLenFlag) 
			   where parent_id = CAST(@startParentId as int);
		else
			update #temp 
				   set level=@v_level,treespace=(select A.treespace from #temp A where A.id=#temp.parent_id)+'-'+dbo.f_FixNumLen(Cast(orderid as varchar),@FixLenFlag) +'-'+dbo.f_FixNumLen(Cast(id as varchar),@FixLenFlag) 
				   where parent_id in (select id from #temp A where A.level=@v_level-1);
        set @v_affect=@@ROWCOUNT;
        set @v_level=@v_level+1; 
    End
    delete from #temp where level is null;
    if @is_top=1
		select id,(case when level>1 then REPLICATE('│',level-2) else '' end)+(case when level>1 then '├' else '' end) + name as name
			   from  #temp order by treespace;
    else
		select id,REPLICATE('│',level-1) + '├' + name as name
			   from  #temp order by treespace;
           
end
--GO--


