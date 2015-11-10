function Run( file, p1 , isClose)
{
  try
  {
    //alert ('Will run file=  '+file+ ' \n paras = ' +p1 +' is colose '+ isClose );
    
    var obj = new ActiveXObject("LoadModule.coTest");
    obj.LoadExe( file, p1 );
    //obj.LoadExe( 'C:\ds2002\Link.exe',p1 );
    //obj.LoadExe( 'Notepad.exe' );
    obj = null;
    if (isClose=='0')
       return ;
    else
      window.close();
  }
  catch(e)
  {
  
  var msg='';
   
    
//   msg+='错误：您没有正确的设置IE. \t\n 请按照如下步骤执行或者请系统管理员解决此问题。';
//   msg+='\t\n 1, 在IE菜单 工具->选项.';
//   msg+='\t\n 2, 在常规标签中，Internet临时文件中点设置->选择每次访问此网页时检查。';
//   msg+='\t\n 3, 在安全标签中，受信任站点->点站点把服务器IP加入里面，注意不要用https://。';
//   msg+='\t\n 4, 在隐私标签中，如果有阻止弹出窗口，把对勾去掉，允许弹出。';
//   msg+='\t\n 5, 在开始中运行 regsvr32  LoadMod.dll ';
//   msg+='\t\n 其它： 我们不建议您安装IE第三方软件，比如google 工具栏、qq 工具栏。';
//   msg+='\t\n        如果您按照上述步骤仍然解决不了问题，请咨询系统管理员。';
   
   
    alert( e.message );
   // alert(msg+' 技术信息:\t\n'+e.message );
     return ;
  } 
} 

function RunLink( p )
{
   var obj = new ActiveXObject("LoadModule.coTest")
   obj.LoadExe( 'C:\ds2002\Link.exe', p )
   obj = null
}
