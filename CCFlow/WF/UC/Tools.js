
function DoAutoTo( fk_emp, empName )
{
   if (window.confirm(' Are you sure you want to give your work authorization ['+fk_emp+']吗?')==false)
       return;

    var url='Do.aspx?DoType=AutoTo&FK_Emp='+fk_emp;
    WinShowModalDialog(url,'');
    alert(' Authorization successful , Please do not forget to withdraw .'); 
    window.location.href='Tools.aspx';
}

function ExitAuth(fk_emp) {
    if (window.confirm(' Are you sure you want to quit authorized landing mode ?') == false)
        return;
    var url = 'Do.aspx?DoType=ExitAuth&FK_Emp=' + fk_emp;
    WinShowModalDialog(url, '');
    window.location.href = 'Tools.aspx';
}

function TakeBack( fk_emp )
{
   if (window.confirm(' Are you sure you want to cancel ['+fk_emp+'] Authorized it ?')==false)
       return;
    var url='Do.aspx?DoType=TakeBack';
    WinShowModalDialog(url,'');
    alert(' You have successfully canceled .'); 
    window.location.reload();
}

function LogAs( fk_emp )
{
   if (window.confirm(' Are you sure you want to ['+fk_emp+'] License landed it ?')==false)
       return;
       
    var url='Do.aspx?DoType=LogAs&FK_Emp='+fk_emp;
    WinShowModalDialog(url,'');
    alert(' Successful landing , Now you can ['+fk_emp+'] Processing .'); 
    window.location.href='EmpWorks.aspx';
}

function CHPass()
{
    var url='Do.aspx?DoType=TakeBack';
   // WinShowModalDialog(url,'');
    alert(' Password changed successfully , Keep in mind that your new password .'); 
}