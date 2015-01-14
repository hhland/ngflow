CREATE TABLE [UserRecentContact](
	[UserID] [varchar](50) NOT NULL,
	[ContactID] [varchar](50) NULL,
	[ContactType] [int] NULL,
	[ContactTime] [varchar](50) NULL,
	[OrderID] [int] NULL
) ON [PRIMARY]
GO
CREATE TABLE [SMS](
	[msgID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[smsMsgContent] [text] NOT NULL,
	[sendID] [nchar](20) NOT NULL,
	[datetime] [datetime] NOT NULL,
 CONSTRAINT [PK_SMS] PRIMARY KEY CLUSTERED 
(
	[msgID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [sendCount](
	[MsgID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[smsMsgContent] [text] NOT NULL,
	[saveDate] [datetime] NOT NULL,
 CONSTRAINT [PK_sendCount] PRIMARY KEY CLUSTERED 
(
	[MsgID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [RecordMsgUser](
	[ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[MsgId] [numeric](18, 0) NOT NULL,
	[ReceiveID] [nchar](20) NOT NULL,
	[isSendSuccess] [bit] NOT NULL,
	[isSave] [bit] NOT NULL,
 CONSTRAINT [PK_GroupRecordMsg] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE function [Get_StrArrayStrOfIndex]
(
 @str varchar(8000),  -- To split a string 
 @split varchar(10),  -- Delimiter 
 @index int -- Take the first of several elements 
)
returns varchar(1024)
as
begin
 declare @location int
 declare @start int
 declare @next int
 declare @seed int

 set @str=ltrim(rtrim(@str))
 set @start=1
 set @next=1
 set @seed=len(@split)
 
 set @location=charindex(@split,@str)
 while @location<>0 and @index>@next
 begin
   set @start=@location+@seed
   set @location=charindex(@split,@str,@start)
   set @next=@next+1
 end
 if @location =0 select @location =len(@str)+1 
-- Here there are two cases :1, Delimiter string does not exist  2, Delimiter string exists ,
-- Jump while CYCLING ,@location为0, It defaults to the string behind a delimiter .
 
 return substring(@str,@start,@location-@start)
end
GO
CREATE function [dbo].[Get_StrArrayLength]
(
  @str varchar(8000),  -- To split a string 
  @split varchar(10)  -- Delimiter 
)
returns int
as
begin
 declare @location int
 declare @start int
 declare @length int

 set @str=ltrim(rtrim(@str))
 set @location=charindex(@split,@str)
 set @length=1
 while @location<>0
 begin
   set @start=@location+1
   set @location=charindex(@split,@str,@start)
   set @length=@length+1
 end
 return @length
end
GO
CREATE TABLE [CustomGroup](
	[GroupID] [int] IDENTITY(1,1) NOT NULL,
	[GroupName] [varchar](50) NULL,
	[Notice] [nvarchar](200) NULL,
	[users] [ntext] NULL,
	[UserID] [varchar](50) NULL,
	[CreateDateTime] [datetime] NULL,
 CONSTRAINT [PK_CustomGroup] PRIMARY KEY CLUSTERED 
(
	[GroupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE FUNCTION [GetParentDeptID](@DeptID varchar(1000))
RETURNS int
AS
BEGIN
DECLARE @ParentDeptID int
-- Incoming department number is character （ Can be converted to an integer ）, Coding rules aabbccdd

IF LEN(@DeptID)=2
  SET @ParentDeptID=0  -- Top level 
ELSE
  BEGIN
    SET @ParentDeptID=CAST(LEFT(@DeptID,LEN(@DeptID)-2) as int)
  END

RETURN @ParentDeptID
END
GO
CREATE TABLE [RecordMsg](
	[MsgID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[sendID] [nchar](20) NULL,
	[msgDateTime] [datetime] NULL,
	[msgContent] [text] NULL,
	[ImageInfo] [ntext] NULL,
	[fontName] [nchar](30) NULL,
	[fontSize] [float] NULL,
	[fontBold] [bit] NULL,
	[fontItalic] [bit] NULL,
	[fontStrikeout] [bit] NULL,
	[fontUnderline] [bit] NULL,
	[fontColor] [int] NULL,
	[InfoClass] [int] NULL,
	[SMSInfo] [tinyint] NULL,
	[GroupID] [int] NULL,
	[SendUserID] [nchar](20) NULL,
 CONSTRAINT [PK_RecordMsg] PRIMARY KEY CLUSTERED 
(
	[MsgID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE FUNCTION [MD5] 
(
	 --  Source string 
    @src nvarchar(max),
    --  Encryption type (16/32)
    @type int = 32

)
RETURNS nvarchar(max)
WITH EXECUTE AS CALLER
AS
BEGIN
    --  Store md5 Encrypted string (ox)
    DECLARE @smd5 varchar(34)

    --  Encrypted string 
    SELECT @smd5 =sys.fn_VarBinToHexStr(hashbytes('MD5', @src));

    IF @type=16
        SELECT @smd5 = SUBSTRING(@smd5,11,16)   --16位
    ELSE
        SELECT @smd5 = SUBSTRING(@smd5,3,32)    --32位

    --  Returns the encrypted string 
    RETURN @smd5
END
GO
CREATE TABLE [IMUser](
	[UserID] [varchar](50) NOT NULL,
	[orderID] [int] NULL,
	[headPicIdx] [int] NULL,
	[onlineState] [tinyint] NULL,
	[lastIP] [char](30) NULL,
	[lastPort] [int] NULL,
	[lastNetClass] [tinyint] NULL,
	[lastDate] [datetime] NULL,
	[onlineDateLength] [int] NULL,
	[CreateGroupMax] [tinyint] NULL,
	[isSendSMS] [tinyint] NULL,
	[isEditUserData] [tinyint] NULL,
	[isAdmin] [bit] NULL,
	[Remark] [nvarchar](200) NULL,
	[isAbandan] [bit] NULL,
	[DepsVersion] [char](32) NULL,
	[UsersVersion] [char](32) NULL,
	[DepsCount] [int] NULL,
	[UsersCount] [int] NULL,
	[headPicFile] [nvarchar](256) NULL,
	[SID] [varchar](20) NULL,
	[Sex] [varchar](50) NULL,
	[Tel] [varchar](50) NULL,
	[Mobile] [varchar](50) NULL,
	[Fax] [varchar](50) NULL,
	[EMail] [varchar](50) NULL,
	[Address] [varchar](200) NULL,
	[PostCode] [varchar](50) NULL
) ON [PRIMARY]
GO
CREATE TABLE [IMDept](
	[DeptID] [int] NOT NULL,
	[orderID] [int] NULL,
	[Phone] [varchar](50) NULL,
	[Fax] [varchar](50) NULL,
	[isAbandan] [bit] NULL
) ON [PRIMARY]
GO
CREATE PROCEDURE [GroupMsgRecordInsertUsers] 
	-- Add the parameters for the stored procedure here
	@MsgId int,
	@ReceiveID varchar(8000) -- Messages to multiple recipients separated by semicolons 
    --@isSave bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.---1
	SET NOCOUNT ON;
	declare @next int  
	set @next=1
	while @next<dbo.Get_StrArrayLength(@ReceiveID,';')
		begin
			insert into RecordMsgUser (MsgId,ReceiveID)	values(@MsgId ,dbo.Get_StrArrayStrOfIndex(@ReceiveID,';',@next) )
			set @next=@next+1
		end

END
GO
CREATE PROC [GetOrgInfo]
  @userID varchar(50)
AS
BEGIN
SET NOCOUNT ON;

SELECT TOP 1 * FROM IMUser

END
GO
CREATE PROCEDURE [GetAllUsersSigns] 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT UserID,Remark from IMUser where UserID<>'admin'

END
GO
CREATE PROCEDURE [CustomGroupGetInfoFromGroupID] 
	-- Add the parameters for the stored procedure here
	@GroupID int  
AS
BEGIN
	 
	SET NOCOUNT ON;
    SELECT * from customGroup where GroupID=@GroupID
END
GO
CREATE PROCEDURE [CustomGroupGetIDs] 
	-- Add the parameters for the stored procedure here
	--@userID varchar(20)
	@userID nvarchar(20)

AS
BEGIN
	SET NOCOUNT ON;
    SELECT GroupID,GroupName from customGroup where userID=@userID or users like '%' + @userID + ';%'
END
GO
CREATE PROCEDURE [CustomGroupCreateOrUpdate] 
	-- Add the parameters for the stored procedure here
    @GroupID int,
    @GroupName nvarchar(20), 
    @Notice nchar(100),
    @Users text,
	@userID nvarchar(20) 
AS
BEGIN

	declare @createMax int -- The maximum number that identifies the user group promised to create charge 
	declare @createCount int -- Identifies the number of groups the user has created 
    declare @returnGroupID int-- Creating a successful group identification number , Unsuccessful for 0;
        

    set @createMax=1 -- Assuming the maximum number of user-created current 
    set @createCount=0 -- Assuming the current number of user groups that have been created 
    set @returnGroupID=0-- Suppose you create without success 
    
	SET NOCOUNT ON;

   if @GroupID=0 -- Indicates that the user is to create a new group 
   begin
    -- Gets the number of users have created groups 
    select @createCount=count(*) from CustomGroup where UserID=@userID
    -- Creating user groups get maximum permissible charge 
    --select @createMax=CreateGroupMax from SystemUsers where UserID=@userID
    --select @createMax=CreateGroupMax from Users where UserID=@userID
    select @createMax=CreateGroupMax from IMUser where UserID=@userID

    
    if @createCount<@createMax -- If you have not reached the maximum , Create a group , Create a group number and returns 
      begin
	    insert into CustomGroup(GroupName,Notice,users,UserID) values( @GroupName,@Notice,@Users,@userID)-- Will create a group of information into tables 
        set @returnGroupID=@@IDENTITY    -- Returns have been created or updated group number 
      end
   end

   if @GroupID<>0 -- Indicates that the user is to create a group to update their information 
    begin
      select @createCount=count(*) from CustomGroup where GroupID=@GroupID and UserID=@userID -- To update the group determine whether the current user created 
      if @createCount=0 -- If it is not the current user group created , No right to update information 
        begin
           set @returnGroupID=0
        end
      if @createCount=1 -- If it is a group created by the current user , Update information 
        begin
	       update CustomGroup set GroupName=@GroupName,Notice=@Notice,Users=@Users where GroupID=@GroupID
           set @returnGroupID=@GroupID -- Set to return the group ID
        end
    end

    return    @returnGroupID    -- Returns have been created or updated group number 
     
    SET   NOCOUNT   OFF   
END
GO
CREATE VIEW [V_Emp]
AS
SELECT [No] as UserID,[Name] as UserName,Pass,CAST(FK_Dept as int) as DeptID FROM Port_Emp
GO
CREATE VIEW [V_Dept]
AS
--SELECT CAST([No] as int) as DeptID,[Name] as DeptName,dbo.GetParentDeptID(No) as ParentDeptID 
--FROM Port_Dept
SELECT CAST([No] as int) as DeptID,[Name] as DeptName,CAST(ParentNo as int) as ParentDeptID FROM Port_Dept
GO
CREATE PROCEDURE [UserUpdateSign] 
	@userID nvarchar(20),-- User ID 
    @Sign varchar(200) -- Signature 
AS
BEGIN
	SET NOCOUNT ON;

	update IMUser set Remark=@Sign where userID=@userID
END
GO
CREATE PROCEDURE [UserUpdateOnlineState] 
	-- Add the parameters for the stored procedure here
	--@userID varchar(20),-- User ID
	@userID nvarchar(20),-- User ID
	@State tinyint-- 0< User status value <255
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
   
   IF @State=0-- If the user is offline , Is calculated and saved their online time 
    begin 
      --update systemUsers set onlineState=@State,onlineDateLength=(select isnull(onlineDateLength,0)+datediff(ss,lastDate,getdate()) from systemUsers where UserID=@userID) where UserID=@userID 
      update IMUser set onlineState=@State,onlineDateLength=(select isnull(onlineDateLength,0)+datediff(ss,lastDate,getdate()) from IMUser where UserID=@userID) where UserID=@userID 
    end
   else
   begin-- Otherwise, only the user online status update 
	  --update systemUsers set onlineState=@State where UserID=@userID
	  update IMUser set onlineState=@State where UserID=@userID
   end
END
GO
CREATE PROCEDURE [UserUpdateOnlineInfo] 
	-- Add the parameters for the stored procedure here
	--@userID varchar(20),-- User ID 
	@userID nvarchar(20),-- User ID 
	--@IP varchar(15) ,-- User IP
	@IP nvarchar(15) ,-- User IP
    @Port int,-- Port 
    @SID varchar(20), --SID
    @NetClass tinyint-- Network Type 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--update systemUsers set lastIp=@IP,lastPort=@Port,lastNetClass=@NetClass,lastDate=GetDate(),
	--onlineState=1 where userID=@userID
	update IMUser set lastIp=@IP,lastPort=@Port,lastNetClass=@NetClass,lastDate=GetDate(),[SID]=@SID,onlineState=1 where userID=@userID
END
GO
CREATE PROCEDURE [RecordMsgInsert] 
	-- Add the parameters for the stored procedure here
	--@userID varchar(20),
	@userID nvarchar(20),
    @ReceiveID varchar(20),
	@msgContent ntext,
	@ImageInfo ntext,
	@fontName nchar(30),
	@fontSize float,
	@fontBold bit,
	@fontItalic bit,
	@fontStrikeout bit,
	@fontUnderline bit,
	@fontColor int,
	@InfoClass int,
    @SMSInfo tinyint,
    @GroupID int 
AS
BEGIN
	SET NOCOUNT ON;

	insert into RecordMsg (sendID,
							msgContent,
							ImageInfo,
							fontName,
							fontSize,
							fontBold,
							fontItalic,
							fontStrikeout,
							fontUnderline,
							fontColor,
							InfoClass,
                            SMSInfo,
                            GroupID 
)
	values(@userID,
			@msgContent,
			@ImageInfo,
			@fontName,
			@fontSize,
			@fontBold,
			@fontItalic,
			@fontStrikeout,
			@fontUnderline ,
			@fontColor,
			@InfoClass,
            @SMSInfo,
            @GroupID
          )

     if (@ReceiveID<>'')-- If the recipient ID Is not empty , Description is not repeated news , Will be added to the user record RecordMsgUser表
     begin
       insert into RecordMsgUser (MsgId,ReceiveID ) values(@@IDENTITY ,@ReceiveID )
     end
    SET   NOCOUNT   OFF   

    return  @@IDENTITY     
END
GO
CREATE PROCEDURE [RecordMsgGetFromId] 
	-- Add the parameters for the stored procedure here
	@ID numeric (18,0)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * from RecordMsg where MsgId= @ID
END
GO
CREATE PROCEDURE [RecordMsgGet] 
	-- Add the parameters for the stored procedure here
	@userID nvarchar(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT  * from RecordMsg,RecordMsgUser where RecordMsgUser.isSendSuccess=0 and  RecordMsgUser.ReceiveID=@userID and RecordMsg.MsgId=RecordMsgUser.MsgId
END
GO
CREATE PROCEDURE [RecordMsgDelFromID] 
	-- Add the parameters for the stored procedure here
    @ID numeric (18,0),
	@userID nvarchar(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
--    update RecordMsgUser set isSendSuccess=1 where ReceiveID=@userID and MsgId=@ID
--     Remove RecordMsgUser Table of user information 
    delete  from RecordMsgUser where ReceiveID=@userID and MsgId=@ID 
--     Get the current number of records whether the message is 0
    DECLARE @Total INT 
    select  @Total=count(*) from RecordMsgUser where MsgId=@ID
    if (@Total=0)
       begin
	       delete from RecordMsg where MsgId=@ID -- Delete offline messages 
       end
END
GO
CREATE PROCEDURE [UpdateRecentContact] 
	-- Add the parameters for the stored procedure here
	--@userID varchar(20),-- User ID
	@userID nvarchar(20),-- User ID
	@receiveID nvarchar(20)--  Recent contacts ID
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
   SET NOCOUNT ON;
   
   DECLARE @existUserID int;
   DECLARE @cTime varchar(50);

   SET @cTime= CONVERT(varchar(4),year(getdate()))+'-'+CONVERT(varchar(4),month(getdate()))+'-'+CONVERT(varchar(2),day(getdate()))+' '+CONVERT(varchar(2),datepart(hh,getdate

()))+':'+CONVERT(varchar(2),datepart(mi,getdate()))+':'+CONVERT(varchar(2),datepart(ss,getdate()))+'.'+CONVERT(varchar(10),datepart(ms,getdate()))

   SELECT @existUserID=COUNT(*) FROM UserRecentContact WHERE UserID=@userID AND ContactID=@receiveID 
   
   IF @existUserID=0-- Not on the list of the user , Then add records 
    begin 
      INSERT INTO UserRecentContact(UserID,ContactID,ContactType,ContactTime,OrderID) VALUES(@userID,@receiveID,0,@cTime,0)
    end
   else -- Already in the list , The update time 
   begin
	  update UserRecentContact set ContactTime=@cTime where UserID=@userID AND ContactID=@receiveID
   end
END
GO
CREATE PROCEDURE [SYSTEMRecordMsgGet] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT  * from RecordMsg,RecordMsgUser where RecordMsgUser.isSendSuccess=0 and  RecordMsg.SendID='SYSTEM' and RecordMsg.MsgId=RecordMsgUser.MsgId
END
GO
CREATE TABLE [smsUser](
	[ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[msgID] [numeric](18, 0) NOT NULL,
	[receiveID] [nchar](20) NOT NULL,
	[sendFailedCount] [tinyint] NULL,
	[isSendSuccess] [bit] NULL,
	[lastDate] [datetime] NULL,
 CONSTRAINT [PK_smsUser] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
-- Get only the first 20个
CREATE PROCEDURE [UserGetRecentContactList] 
   @userId varchar(20)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT TOP 20 UserID,ContactID,ContactType,ContactTime,OrderID from UserRecentContact  where UserID=@userId ORDER BY CONVERT(datetime,ContactTime) DESC

END
GO
CREATE PROCEDURE  [UserGetUsersSignsByUserID] 
	@userID nvarchar(20)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Remark from IMUser where userID=@userID

END
GO
CREATE PROCEDURE [UserGetUserDataFromUserID] 
	-- Add the parameters for the stored procedure here
	--@userID varchar(20)
	@userID nvarchar(20)

	 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    --2011.6.7 Modification 
	--SELECT * from systemUsers where userID=@userID
    -- Because just take the version number and user name information unrelated departments , So we will do so 
	SELECT a.*,b.orderID,b.Remark,b.SID from V_Emp a,IMUser b where a.UserID=b.UserID AND a.userID=@userID

END
GO
CREATE PROCEDURE    [userIsPassword] 
	--@userID varchar(20), 
	@userID nvarchar(20), 
	--@password varchar(32)
	@password nvarchar(32)
AS
BEGIN
 	SET NOCOUNT ON;
    declare @count int
    -- Insert statements for procedure here
	--SELECT @count=count(*) from systemUsers where userID=@userID and password=@password
	SELECT @count=count(*) from V_Emp where userID=@userID and pass=@password
    return  @count
END
GO
CREATE PROCEDURE [userIsExist] 
	-- Add the parameters for the stored procedure here
	--@userID varchar(20)
	@userID nvarchar(20)
AS
BEGIN
	SET NOCOUNT ON;
    declare @userCount tinyint
    set @userCount=0
	--SELECT @userCount=count(userID) from systemUsers where UserID=@userID
	SELECT @userCount=count(userID) from V_Emp where UserID=@userID
    return @userCount
END
GO
CREATE PROCEDURE [userGetrData] 
	-- Add the parameters for the stored procedure here
	--@userId varchar(20)
	@userId nvarchar(20)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--SELECT * from SystemUsers where userID=@userId
	SELECT a.UserID,a.UserName,a.UserName as [Name],
      b.Tel as ophone,b.Mobile,b.Fax,b.EMail,b.Tel as PHS 
    FROM V_Emp a,IMUser b WHERE a.UserID=b.UserID AND a.UserID=@userId

END
GO
CREATE PROCEDURE [UserGetPageUsersBaseInfo] 
	--@UserID varchar(20),
	@UserID nvarchar(20),
	@PageSize int,
	@PageIndex int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
 
	CREATE TABLE #PageIndex -- Create a temporary table 
	(
		IndexId int IDENTITY (1, 1) NOT NULL,
		--UserID varchar(20)-- For saving user ID Set 
		UserID nvarchar(20)-- For saving user ID Set 
	)

    --INSERT INTO #PageIndex(UserID) select userID from systemUsers 
    --where DepId<>1 order by orderID,userID asc 
    INSERT INTO #PageIndex(UserID) select a.userID from V_Emp a,IMUser b 
      where a.UserID=b.UserID order by b.orderID,a.userID asc 
    
    --select userID,userName,DepId,orderID from systemUsers 
    --WHERE userID in(select userID from #PageIndex where IndexId BETWEEN @PageSize*(@PageIndex-1)+1 AND @PageSize*(@PageIndex))
    select a.userID,a.userName,a.DeptID as DepId,b.orderID 
      from V_Emp a,IMUser b WHERE a.UserID=b.UserID AND a.userID in(select userID from #PageIndex 
      where IndexId BETWEEN @PageSize*(@PageIndex-1)+1 AND @PageSize*(@PageIndex))

END
GO
CREATE PROCEDURE [userGetBaseData] 
	-- Add the parameters for the stored procedure here
	--@userId varchar(20)
	@userId nvarchar(20)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--SELECT * from SystemUsers where userID=@userId
	SELECT a.UserID,a.UserName,b.PostCode as post,
      b.Tel as phone,b.Mobile as GSM,b.Fax as phs 
    FROM V_Emp a,IMUser b WHERE a.UserID=b.UserID AND a.UserID=@userId

END
GO
CREATE PROCEDURE [UserChangePassword] 
	--@userID varchar(20),
	@userID nvarchar(20),
	@PasswordOld varchar(32),-- Password 32位MD5值
	@PasswordNew varchar(32) -- Password 32位MD5值
AS
BEGIN
	declare @userCount int-- Old record user password is correct 
	SET NOCOUNT ON;
    --select  @userCount=count(*) from systemUsers where UserID=@userID and password=@PasswordOld
    select  @userCount=count(*) from V_Emp where UserID=@userID and Pass=@PasswordOld
    
    if @userCount=0 
       return 0-- Return 0 Said the change password unsuccessful , The reason the old password is incorrect 
	
    --update systemUsers  set Password=@PasswordNew where UserID=@userID 
    update V_Emp  set Pass=@PasswordNew where UserID=@userID 
    
    return 1 -- Return 1 Change Password represent success 
END
GO
CREATE PROC [UpdateSystemData]
AS
BEGIN
DECLARE @OldUsersVersion varchar(50)
DECLARE @OldDepsVersion varchar(50)
DECLARE @OldUsersCount int
DECLARE @OldDepsCount int
DECLARE @NewUsersVersion varchar(50)
DECLARE @NewDepsVersion varchar(50)
DECLARE @NewUsersCount int
DECLARE @NewDepsCount int

-- Department 
DELETE FROM IMDept WHERE DeptID NOT IN (SELECT DeptID FROM V_Dept)
INSERT INTO IMDept(DeptID,orderID,isAbandan) SELECT DeptID,1,'False' FROM V_Dept
  WHERE DeptID NOT IN (SELECT DeptID FROM IMDept)

-- User 
DELETE FROM IMUser WHERE UserID NOT IN (SELECT UserID FROM V_Emp)
INSERT INTO IMUser(UserID,orderID,headPicIdx,CreateGroupMax,isSendSMS,isEditUserData,isAdmin,isAbandan,
  DepsVersion,UsersVersion) SELECT UserID,1,0,10,0,0,'False','False',0,0  FROM V_Emp
  WHERE UserID NOT IN (SELECT UserID FROM IMUser)

-- Judge for the first time 
SELECT @OldUsersCount=COUNT(*) FROM IMUser WHERE UsersVersion<>0
IF @OldUsersCount=0
  BEGIN
    UPDATE IMUser SET UsersVersion=1
  END

SELECT @OldDepsCount=COUNT(*) FROM IMUser WHERE DepsVersion<>0
IF @OldDepsCount=0
  BEGIN
    UPDATE IMUser SET DepsVersion=1
  END

  SELECT TOP 1 @OldUsersVersion=UsersVersion,@OldDepsVersion=DepsVersion,@OldUsersCount=UsersCount,
    @OldDepsCount=DepsCount FROM IMUser WHERE DepsVersion<>0
  SELECT @NewUsersCount=count(*) FROM IMUser
  SELECT @NewDepsCount=count(*) FROM IMDept
  SET @NewUsersVersion=CAST(CAST(@OldUsersVersion as int)+1 as varchar)
  SET @NewDepsVersion=CAST(CAST(@OldDepsVersion as int)+1 as varchar)

  update IMUser set UsersCount=@NewUsersCount,UsersVersion=@NewUsersVersion,DepsCount=@NewDepsCount,DepsVersion=@NewDepsVersion

END
GO
CREATE PROCEDURE [smsSendSuccess] 
	-- Add the parameters for the stored procedure here
	@ID numeric(18,0) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	update smsUser set isSendSuccess=1,lastDate=getdate() where id=@ID
    SET NOCOUNT off;
END
GO
CREATE PROCEDURE [smsSendFailed] 
	-- Add the parameters for the stored procedure here
	@ID numeric(18,0)  
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	update smsUser set sendFailedCount=sendFailedCount+1,lastDate=getdate() where ID=@ID
    SET NOCOUNT off;
END
GO
CREATE PROCEDURE [smsInsert] 
	-- Add the parameters for the stored procedure here
	@ReceiveID varchar(8000) ,
    @sendID varchar(20) ,
	@msgContent varchar(1000) 
AS
BEGIN
    declare @MsgId numeric(18,0)
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	insert SMS(sendID,smsMsgContent) values(@sendID,@msgContent)

    set @MsgId=  @@IDENTITY

    declare @next int  

	set @next=1
	while @next<dbo.Get_StrArrayLength(@ReceiveID,';') 
		begin
			insert into smsUser (MsgId,ReceiveID) values(@MsgId ,dbo.Get_StrArrayStrOfIndex(@ReceiveID,';',@next))
			set @next=@next+1
		end
    SET   NOCOUNT   OFF   
END
GO
CREATE PROCEDURE [smsGetSendSuccessfulCount] 
 
AS
BEGIN
 	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT count(*) from smsUser where isSendSuccess=1
END
GO
CREATE PROCEDURE [smsDelNoSendFailed] 
	 
AS
BEGIN
	 
	SET NOCOUNT ON;
    delete  from  smsUser where smsUser.isSendSuccess=0
END
GO
CREATE PROCEDURE [DepGetDepsPageBaseInfo] 
	-- Add the parameters for the stored procedure here
	--@UserID varchar(20),
	@UserID nvarchar(20),
	@PageSize int,
	@PageIndex int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   
	CREATE TABLE #PageIndex -- Create a temporary table 
	(
		IndexId int IDENTITY (1, 1) NOT NULL,
		DepID int -- For preservation department ID Set 
	)

    --INSERT INTO #PageIndex(DepID) select DepID from Department 
    --where DepId<>1 order by OrderID,DepID asc 
    INSERT INTO #PageIndex(DepID) select a.DeptID from V_Dept a,IMDept b WHERE a.DeptID=b.DeptID order by b.OrderID,a.DeptID asc 
    
    --select DepID,DepName,SuperiorId,OrderID from Department 
    --WHERE DepID in(select DepID from #PageIndex where IndexId BETWEEN @PageSize*(@PageIndex-1)+1 AND @PageSize*(@PageIndex))
    select a.DeptID as DepID,a.DeptName as DepName,a.ParentDeptID as SuperiorId,b.OrderID from V_Dept a,IMDept b WHERE a.DeptID=b.DeptID AND a.DeptID in(select DepID from 

#PageIndex where IndexId BETWEEN @PageSize*(@PageIndex-1)+1 AND @PageSize*(@PageIndex))


END
GO
CREATE PROCEDURE [DepGetData] 
	-- Add the parameters for the stored procedure here
	--@DepId varchar(20),
	@DepId int,
	@flag  bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if @flag = 0
	begin
		--SELECT * from Department where DepId=@DepId
		SELECT a.*,b.orderID from V_Dept a,IMDept b where a.DeptID=b.DeptID AND a.DeptID=@DepId
	end
	else
	begin
		--SELECT * from Department where SuperiorId=@DepId
		SELECT a.*,b.orderID from V_Dept a,IMDept b where a.DeptID=b.DeptID AND ParentDeptID=@DepId
	end

END
GO
/****** Object:  Default [DF_SMS_sendfailedCount]    Script Date: 06/24/2013 14:00:01 ******/
ALTER TABLE [SMS] ADD  CONSTRAINT [DF_SMS_sendfailedCount]  DEFAULT (getdate()) FOR [datetime]
GO
/****** Object:  Default [DF_sendCount_saveDate]    Script Date: 06/24/2013 14:00:01 ******/
ALTER TABLE [sendCount] ADD  CONSTRAINT [DF_sendCount_saveDate]  DEFAULT (getdate()) FOR [saveDate]
GO
/****** Object:  Default [DF_RecordMsgUser_isSendSuccess]    Script Date: 06/24/2013 14:00:01 ******/
ALTER TABLE [RecordMsgUser] ADD  CONSTRAINT [DF_RecordMsgUser_isSendSuccess]  DEFAULT ((0)) FOR [isSendSuccess]
GO
/****** Object:  Default [DF_RecordMsgUser_isSave]    Script Date: 06/24/2013 14:00:01 ******/
ALTER TABLE [RecordMsgUser] ADD  CONSTRAINT [DF_RecordMsgUser_isSave]  DEFAULT ((0)) FOR [isSave]
GO
/****** Object:  Default [DF_CustomGroup_CreateDateTime]    Script Date: 06/24/2013 14:00:01 ******/
ALTER TABLE [CustomGroup] ADD  CONSTRAINT [DF_CustomGroup_CreateDateTime]  DEFAULT (getdate()) FOR [CreateDateTime]
GO
/****** Object:  Default [DF_RecordMsg_msgDateTime]    Script Date: 06/24/2013 14:00:01 ******/
ALTER TABLE [RecordMsg] ADD  CONSTRAINT [DF_RecordMsg_msgDateTime]  DEFAULT (getdate()) FOR [msgDateTime]
GO
/****** Object:  Default [DF_RecordMsg_fontBold]    Script Date: 06/24/2013 14:00:01 ******/
ALTER TABLE [RecordMsg] ADD  CONSTRAINT [DF_RecordMsg_fontBold]  DEFAULT ((0)) FOR [fontBold]
GO
/****** Object:  Default [DF_RecordMsg_fontItalic]    Script Date: 06/24/2013 14:00:01 ******/
ALTER TABLE [RecordMsg] ADD  CONSTRAINT [DF_RecordMsg_fontItalic]  DEFAULT ((0)) FOR [fontItalic]
GO
/****** Object:  Default [DF_RecordMsg_fontStrikeout]    Script Date: 06/24/2013 14:00:01 ******/
ALTER TABLE [RecordMsg] ADD  CONSTRAINT [DF_RecordMsg_fontStrikeout]  DEFAULT ((0)) FOR [fontStrikeout]
GO
/****** Object:  Default [DF_RecordMsg_fontUnderline]    Script Date: 06/24/2013 14:00:01 ******/
ALTER TABLE [RecordMsg] ADD  CONSTRAINT [DF_RecordMsg_fontUnderline]  DEFAULT ((0)) FOR [fontUnderline]
GO
/****** Object:  Default [DF_RecordMsg_isSendSMS]    Script Date: 06/24/2013 14:00:01 ******/
ALTER TABLE [RecordMsg] ADD  CONSTRAINT [DF_RecordMsg_isSendSMS]  DEFAULT ((0)) FOR [SMSInfo]
GO
/****** Object:  Default [DF_RecordMsg_GroupID]    Script Date: 06/24/2013 14:00:01 ******/
ALTER TABLE [RecordMsg] ADD  CONSTRAINT [DF_RecordMsg_GroupID]  DEFAULT ((-1)) FOR [GroupID]
GO
/****** Object:  Default [DF_IMUser_orderID]    Script Date: 06/24/2013 14:00:01 ******/
ALTER TABLE [IMUser] ADD  CONSTRAINT [DF_IMUser_orderID]  DEFAULT ((1)) FOR [orderID]
GO
/****** Object:  Default [DF_IMUser_headPicIdx]    Script Date: 06/24/2013 14:00:01 ******/
ALTER TABLE [IMUser] ADD  CONSTRAINT [DF_IMUser_headPicIdx]  DEFAULT ((0)) FOR [headPicIdx]
GO
/****** Object:  Default [DF_IMUser_CreateGroupMax]    Script Date: 06/24/2013 14:00:01 ******/
ALTER TABLE [IMUser] ADD  CONSTRAINT [DF_IMUser_CreateGroupMax]  DEFAULT ((10)) FOR [CreateGroupMax]
GO
/****** Object:  Default [DF_IMUser_isSendSMS]    Script Date: 06/24/2013 14:00:01 ******/
ALTER TABLE [IMUser] ADD  CONSTRAINT [DF_IMUser_isSendSMS]  DEFAULT ((0)) FOR [isSendSMS]
GO
/****** Object:  Default [DF_IMUser_isEditUserData]    Script Date: 06/24/2013 14:00:01 ******/
ALTER TABLE [IMUser] ADD  CONSTRAINT [DF_IMUser_isEditUserData]  DEFAULT ((0)) FOR [isEditUserData]
GO
/****** Object:  Default [DF_IMUser_isAdmin]    Script Date: 06/24/2013 14:00:01 ******/
ALTER TABLE [IMUser] ADD  CONSTRAINT [DF_IMUser_isAdmin]  DEFAULT ((0)) FOR [isAdmin]
GO
/****** Object:  Default [DF_IMUser_isAbandan]    Script Date: 06/24/2013 14:00:01 ******/
ALTER TABLE [IMUser] ADD  CONSTRAINT [DF_IMUser_isAbandan]  DEFAULT ((0)) FOR [isAbandan]
GO
/****** Object:  Default [DF_IMUser_DepsVersion]    Script Date: 06/24/2013 14:00:01 ******/
ALTER TABLE [IMUser] ADD  CONSTRAINT [DF_IMUser_DepsVersion]  DEFAULT ((0)) FOR [DepsVersion]
GO
/****** Object:  Default [DF_IMUser_UsersVersion]    Script Date: 06/24/2013 14:00:01 ******/
ALTER TABLE [IMUser] ADD  CONSTRAINT [DF_IMUser_UsersVersion]  DEFAULT ((0)) FOR [UsersVersion]
GO
/****** Object:  Default [DF_IMDept_orderID]    Script Date: 06/24/2013 14:00:01 ******/
ALTER TABLE [IMDept] ADD  CONSTRAINT [DF_IMDept_orderID]  DEFAULT ((1)) FOR [orderID]
GO
/****** Object:  Default [DF_IMDept_isAbandan]    Script Date: 06/24/2013 14:00:01 ******/
ALTER TABLE [IMDept] ADD  CONSTRAINT [DF_IMDept_isAbandan]  DEFAULT ((0)) FOR [isAbandan]
GO
/****** Object:  Default [DF_smsUser_sendFailedCount]    Script Date: 06/24/2013 14:00:01 ******/
ALTER TABLE [smsUser] ADD  CONSTRAINT [DF_smsUser_sendFailedCount]  DEFAULT ((0)) FOR [sendFailedCount]
GO
/****** Object:  Default [DF_smsUser_isSendSuccess]    Script Date: 06/24/2013 14:00:01 ******/
ALTER TABLE [smsUser] ADD  CONSTRAINT [DF_smsUser_isSendSuccess]  DEFAULT ((0)) FOR [isSendSuccess]
GO
/****** Object:  ForeignKey [FK_smsUser_SMS]    Script Date: 06/24/2013 14:00:01 ******/
ALTER TABLE [smsUser]  WITH CHECK ADD  CONSTRAINT [FK_smsUser_SMS] FOREIGN KEY([msgID])
REFERENCES [SMS] ([msgID])
GO
ALTER TABLE [smsUser] CHECK CONSTRAINT [FK_smsUser_SMS]
GO
