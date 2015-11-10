

var  CurrEnsName ; // 类名称．　实体名称．
var  CurrKeys; // 外键，主键，枚举类型形成的 string.
var  WebPath; // 虚拟当前的虚拟目录.

/*在Datagirn */
function OnDGMousedown(path, id, className, keys )
{
   if ( event.button != 2)
      return true;
      
    CurrEnsName = className;
    CurrKeys = keys;
    WebPath = path;

    ShowMenu( id,className,keys);
    document.oncontextmenu=new Function("event.returnValue=false;");
}

/* 打开方法影射. */
function RefMethod(   index , warning, target, ensName )
{
   if (CurrEnsName==null)
      return;

 if (warning==null ||  warning=='' )
 {
 }
 else
 {
  if ( confirm(  warning ) ==false)   
      return false;
 }
 
  var url= "/Comm/RefMethod.aspx?Index="+index+"&EnsName="+ ensName + CurrKeys;
  
  //alert( url );
 
    if (target==null)
      var a=window.location.href=url; 
    else
      var b=window.showModalDialog( url , 'ass' ,'dialogHeight: 400px; dialogWidth: 600px; dialogTop: 100px; dialogLeft: 110px; center: yes; help: no'); 
   return true;
}

/* 打开方法影射. */
function RefMethodExt(  index , warning, target, ensName, key , path)
{
 if (warning==null ||  warning=='' )
 {
 }
 else
 {
 
  if ( confirm(  warning ) ==false)   
      return false;
 }
 
 
  var url= "/Comm/RefMethod.aspx?Index="+index+"&EnsName="+ ensName +"&PK="+ key;
 
    if (target==null)
      var a=window.location.href=url; 
    else
      var b=window.showModalDialog( url , 'ass' ,'dialogHeight: 400px; dialogWidth: 600px; dialogTop: 100px; dialogLeft: 110px; center: yes; help: no'); 
   return true;
}


/*编辑一对多的关系*/
function EditOneVsM( vsMName , attrKey)
{
 if (CurrEnsName==null)
      return ;
   var url=WebPath+'/Comm/'+'UIEn1ToM.aspx?EnsName='+CurrEnsName+"&AttrKey="+attrKey+CurrKeys;
   var a=window.showModalDialog( url , 'OneVs' ,'dialogHeight: 600px; dialogWidth: 800px; dialogTop: 100px; dialogLeft: 110px; center: yes; help: no'); 
   
  // WinShowModalDialog(url,'onevsM');
  //EnsName=BP.Port.Emp&PK=1&AttrKey=BP.Port.EmpDepts
  //var newWindow=window.open( WebPath+'/Comm/'+'UIEn1ToM.aspx?EnsName='+CurrEnsName+"&AttrKey="+attrKey+CurrKeys,'chosecol', 'width=700,top=100,left=200,height=400,scrollbars=yes,resizable=yes,toolbar=false,location=false' );
  // newWindow.focus();
   return true;
}
/*编辑明晰 */
function EditDtl( dtlName, refKey)
{
 if (CurrEnsName==null)
      return ;
   var newWindow=window.open( WebPath+'/Comm/'+'UIEnDtl.aspx?EnsName='+dtlName+"&Key="+refKey+"&MainEnsName="+ CurrEnsName +CurrKeys,'chosecol', 'width=700,top=100,left=200,height=400,scrollbars=yes,resizable=yes,toolbar=false,location=false' );
   newWindow.focus();
   return true;
}

/*　相关功能 */
function EnsRefFunc( OID )
{
 if (CurrEnsName==null)
      return ;

   var newWindow=window.open( WebPath+'/Comm/'+'RefFuncLink.aspx?RefFuncOID='+OID+'&MainEnsName='+ CurrEnsName +CurrKeys,'chosecol', 'width=100,top=400,left=400,height=50,scrollbars=yes,resizable=yes,toolbar=false,location=false' );
   newWindow.focus();

  return true;
}
/*　相关功能 */
function Delete()
{
 if (CurrEnsName==null)
      return ;      
      
 if (confirm('Will delete it , are you sure? ')==false)
     return;
     
   var url = WebPath+'/Comm/'+'FuncLink.aspx?Flag=DeleteEn&MainEnsName='+ CurrEnsName +CurrKeys,
   var newWindow=showModalDialog( url, 'deleteen',
    'width=500,top=400,left=150,height=270,scrollbars=yes,resizable=yes,toolbar=false,location=false' );
   window.localhost.reload();
}

function Update()
{
 if (CurrEnsName==null)
      return ;
  return true;
}

function New()
{
   if (CurrEnsName==null)
      return ;
      
   var url= "/Comm/"+'UIEn.aspx?EnName='+CurrEnsName; 
   var newWindow=window.open( url,'card', 'width=850,top=50,left=50,height=500,scrollbars=yes,resizable=yes,toolbar=false,location=false' );
   newWindow.focus();
   return;
}

function Card()
{
  if (CurrEnsName==null)
      return ;   
    OpenCard(WebPath, CurrEnsName , CurrKeys ) ;
}


function Adjunct( className )
{
  if (CurrEnsName==null)
      return ; 
   var newWindow=window.open( WebPath+'/Comm/'+'FileManager.aspx?EnsName='+className+CurrKeys,'files', 'width=900,top=60,left=60,height=500,scrollbars=yes,resizable=yes,toolbar=false,location=false' );
   newWindow.focus();
   return;
}

function GroupBy(className, key)
{
   var newWindow=window.open( 'Search.aspx?EnsName='+className+'&GroupKey='+key, 'mainfrm', 'width=900,top=60,left=60,height=500,scrollbars=yes,resizable=yes,toolbar=false,location=false' );
}

function GroupEns( className )
{
 if (CurrEnsName==null)
      return ;
   //document.location='UIEnsCols.aspx?EnsName='+className;
   var newWindow=window.open( WebPath+'/Comm/Group.aspx?EnsName='+className ,'mainfrm', 'width=900,top=60,left=60,height=500,scrollbars=yes,resizable=yes,toolbar=false,location=false' );
   return true;
}

function WinOpen( url )
{
   var newWindow=window.open( url  ,'mainfr1m', 'width=900,top=60,left=60,height=500,scrollbars=yes,resizable=yes,toolbar=false,location=false' );
}

function UIEnsCols( className )
{
 if (CurrEnsName==null)
      return ;
   //document.location='UIEnsCols.aspx?EnsName='+className;
   var newWindow=window.open( '/Comm/UIEnsCols.aspx?EnsName='+className ,'mainfrm', 'width=900,top=60,left=60,height=500,scrollbars=yes,resizable=yes,toolbar=false,location=false' );
   
   return true;   
}

/* 隐藏 Menum */
function HideMenu( id )
{
   document.getElementById( id ).style.visibility='hidden';
}

/* 显示 Menum */
function ShowMenu( id ,className, keys)
{
   var rightedge=document.body.clientWidth-event.clientX;
   var bottomedge=document.body.clientHeight-event.clientY;
   
   //菜单定位
   if (rightedge < document.getElementById( id ).offsetWidth )
      document.getElementById( id ).style.left=document.body.scrollLeft+event.clientX-document.getElementById( id ).offsetWidth;
   else
      document.getElementById( id ).style.left=document.body.scrollLeft+event.clientX;

   if (bottomedge< document.getElementById( id ).offsetHeight)
      document.getElementById( id ).style.top=document.body.scrollTop+event.clientY-document.getElementById( id ).offsetHeight;
   else
      document.getElementById( id ).style.top=document.body.scrollTop+event.clientY;

   document.getElementById( id ).style.visibility="visible";

//   document.body.onclick=HideMenu(id);
   return false;
}

 


/* */ 
function MTROn1(ctrl)
{
   ctrl.style.backgroundColor='royalblue';
}
function MTROut1(ctrl)
{
   ctrl.style.backgroundColor='Menu';
}


/* 处理工作。*/
function DealWork()
{ 
   var url='FlowS.aspx?EnName='+CurrEnsName+CurrKeys;
   var newWindow=window.open( url,'card1', 'width=850,top=50,left=50,height=500,scrollbars=yes,resizable=yes,toolbar=false,location=false' );
   newWindow.focus();
   return;
}



/* 流程处理。*/
function DealBatchWork()
{
  if (CurrEnsName==null)
      return ;
      
   var url='Link.aspx?Flag=DealBatchWork&EnName='+CurrEnsName+CurrKeys;
   
   alert( url );
   
   var newWindow=window.open( url,'FlowS', 'width=850,top=50,left=50,height=500,scrollbars=yes,resizable=yes,toolbar=false,location=false' );
   newWindow.focus();
   return;
}

/* 流程处理。*/
function DealWorkFlow()
{
  if (CurrEnsName==null)
      return ;

   var url='Link.aspx?Flag=DealWorkFlow&EnName='+CurrEnsName+CurrKeys;
   var newWindow=window.open( url,'FlowS', 'width=850,top=50,left=50,height=500,scrollbars=yes,resizable=yes,toolbar=false,location=false' );
   newWindow.focus();
   return;
}
/* 工作报告。*/
function Rpt()
{
  if (CurrEnsName==null)
      return ; 
   
   var url='Link.aspx?Flag=Rpt&EnName='+CurrEnsName+CurrKeys;
   var newWindow=window.open( url,'FlowS', 'width=850,top=50,left=50,height=500,scrollbars=yes,resizable=yes,toolbar=false,location=false' );
   newWindow.focus();
}
/* 质量考核。 */
function ReturnWork()
{
  if (CurrEnsName==null)
      return ;
   var url='Link.aspx?Flag=ReturnWork&EnName='+CurrEnsName+CurrKeys;
   var newWindow=window.open( url,'FlowS', 'width=850,top=50,left=50,height=500,scrollbars=yes,resizable=yes,toolbar=false,location=false' );
   newWindow.focus();
   return ;    
}

/* 质量考核。 */
function CH()
{
  if (CurrEnsName==null)
      return ;
   var url='Link.aspx?Flag=CH&EnName='+CurrEnsName+CurrKeys;
   var newWindow=window.open( url,'FlowS', 'width=850,top=50,left=50,height=500,scrollbars=yes,resizable=yes,toolbar=false,location=false' );
   newWindow.focus();
   return ;    
}
/* 分配工作。*/
function AllotTask()
{
  if (CurrEnsName==null)
      return ;
   var url='Link.aspx?Flag=AllotTask&EnName='+CurrEnsName+CurrKeys;
   var newWindow=window.open( url,'FlowS', 'width=850,top=50,left=50,height=500,scrollbars=yes,resizable=yes,toolbar=false,location=false' );
   newWindow.focus();
   return ;
}
function WorkHelp()
{
  if (CurrEnsName==null)
      return;
   var url='Link.aspx?Flag=WorkHelp&EnName='+CurrEnsName+CurrKeys;
   var newWindow=window.open( url ,'FlowS', 'width=850,top=50,left=50,height=500,scrollbars=yes,resizable=yes,toolbar=false,location=false' );
   newWindow.focus();
   return ;
}
function WorkAdjunct()
{
  if (CurrEnsName==null)
      return ;   
   var url='Link.aspx?Flag=WorkAdjunct&EnName='+CurrEnsName+CurrKeys;
   var newWindow=window.open( url,'FlowS', 'width=850,top=50,left=50,height=500,scrollbars=yes,resizable=yes,toolbar=false,location=false' );
   newWindow.focus();
   return ;
}





 


