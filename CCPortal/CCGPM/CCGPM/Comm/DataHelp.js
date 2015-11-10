// **************************************************************** //
// function Trim(value)
// --------------
// 功能：删除两端空格（= trim）
// 参数：       value 要格式化的字符串
// 返回：       格式化后的字符串
// **************************************************************** //
function Trim(value)
{
	var res = String(value).replace(/^[\s]+|[\s]+$/g,'');
	return res;
}

//分析日期字符串，形式为yyyy.mm.dd，如果正确则返回：yyyymmdd,错误则返回"Error:+错误信息"
//other_info是为了控件最小、最大日期使用的，格式为MIN:Date;MAX:Date;DEFAULT:Date; Date均为8位日期
function parseDate(s_date, colName, cache_id, other_info){
  var spera1, spera2 ;
  var ret_value = new Array() ;
  var dt = new Date() ;
  var date_string ;
  var pos1, pos2, pos3, i ;
  var succ = 1 ;
  var minDate = "", maxDate = "" , defaultDate = "" ;
  
  if( other_info == undefined || other_info == null ) other_info = "" ;
  if( other_info != "")
  {
    pos1 = other_info.indexOf("MIN:") ;
    if( pos1 >= 0 )
    {
      minDate = other_info.substring( pos1 + 4, pos1 + 12 ) ;
      if( minDate == null ) minDate = "" ;
      if( isNaN( minDate)) minDate = "" ;
    }
    pos1 = other_info.indexOf("MAX:") ;
    if( pos1 >= 0 )
    {
      maxDate = other_info.substring(pos1 + 4, pos1 + 12) ;
      if( maxDate == null ) maxDate = "" ;
      if( isNaN(maxDate)) maxDate = "" ;
    }
    pos1 = other_info.indexOf("DEFAULT:") ;
    if( pos1 >= 0 )
    {
      defaultDate = other_info.substring(pos1 + 4, pos1 + 12 ) ;
      if( defaultDate == null ) defaultDate = "" ;
      if( isNaN(defaultDate )) defaultDate = "" ;
    }
    po1 = 0;
  }

  date_string = dt.getFullYear()+"."+(dt.getMonth()+1)+"."+dt.getDate() ;
  spera1 = "." ;
  spera2 = "." ;
  
  if(s_date.length > 12 || s_date.length < 8 )
  {
    ret_value[0] = "Error" ;
    ret_value[1] = date_string ;
    ret_value[2] = "日期格式错误，应为YYYY-MM-DD或者YYYY.MM.DD或YYYY/MM/DD。" ;
    succ = 0 ;
    //return ret_value ;
  }
  if( succ == 1 )
  {
    s_date = Trim(s_date) ;
    if( s_date.indexOf(".") == 4 )
      spera1 = "." ;
    else if( s_date.indexOf("-") == 4)
      spera1 = "-" ;
    else if( s_date.indexOf("/") == 4 )
      spera1 = "/" ;
    else if( s_date.indexOf("年") == 4 )
      spera1 = "年" ;
    else
    {
      ret_value[0] = "Error" ;
      ret_value[1] = date_string ;
      ret_value[2] = "日期格式错误，应为YYYY-MM-DD或者YYYY.MM.DD或YYYY/MM/DD或YYYY年MM月DD日。" ;
      succ = 0 ;
    }
  }

  if( succ == 1 )
  {
    i = 0 ;
    pos1 = 5 ; //月的起始位置
    spera2 = spera1 ;
    if( spera1 == "年")
    {
      spera2 = "月" ;
    //  i = 1 ;
    }
    pos2 = s_date.lastIndexOf(spera2);
    if( pos2 != 6 + i && pos2 != 7 + i )
    {
      ret_value[0] = "Error" ;
      ret_value[1] = date_string ;
      ret_value[2] = "日期格式错误，应为YYYY-MM-DD或者YYYY.MM.DD或YYYY/MM/DD或YYYY年MM月DD日。" ;
      succ = 0 ;
    }
    
    if( succ == 1 )
    {
      pos3 = 0 ;
      if( spera1 == "年")
      {
        pos3 = s_date.indexOf("日") ;
      }
      else
      {
        pos3 = s_date.indexOf(spera1, pos2+1) ;
      }
      if( pos3 <= 0 )
        pos3 = s_date.length - 1 ;
      else
        pos3 -- ;
      var s_date2 ;
      s_date2=s_date.substring(0,4)+s_date.substring(pos1,pos2 )+s_date.substring(pos2+1,pos3+1);
      if(isNaN(s_date2))
      {
        ret_value[0] = "Error" ;
        ret_value[1] = date_string ;
        ret_value[2] = "年、月、日必须是数据。" ;
        succ = 0 ;
      }
      if( succ == 1 )
      {
        var year=parseInt(s_date.substring(0,4),10);
        if(year<1900)
        {
          ret_value[0] = "Error" ;
          ret_value[1] = date_string ;
          ret_value[2] = "年份小于最小年份1900." ;
          succ = 0 ;
        }
        if( succ == 1 )
        {
          var month=parseInt(s_date.substring(pos1, pos2 ),10);
          if(month<1||month>12)
          {
            ret_value[0] = "Error" ;
            ret_value[1] = date_string ;
            ret_value[2] = "月份只能在1-12之间。" ;
            succ = 0 ;
          }
        }
        if( succ == 1 )
        {
          var day=parseInt(s_date.substring(pos2+1+i, pos3 + 1),10);
          if(day<1||day>31)			//还需判断润年
          {
            ret_value[0] = "Error" ;
            ret_value[1] = date_string ;
            ret_value[2] = "日只能在1-31之间。" ;
            succ = 0 ;
          }
        }
        
        if( succ == 1 )
        {
          switch(month)
          {
	          case 4:
	          case 6:
	          case 9:
	          case 11:
	            if (day > 30)
              {
                ret_value[0] = "Error" ;
                ret_value[1] = date_string ;
                ret_value[2] = "本月的天数不能超过30。" ;
                succ = 0 ;
              }
	            break;
          	
	          case 2:
	            if (((year%4)==0)&&((year%100)!=0)||((year%400)==0))
              {
                if( day > 29 )
                {
                  ret_value[0] = "Error" ;
                  ret_value[1] = date_string ;
                  ret_value[2] = "本年2月份的天数不能超过29。" ;
                  succ = 0 ;
                }
              }
	            else if(day > 28)
              {
                ret_value[0] = "Error" ;
                ret_value[1] = date_string ;
                ret_value[2] = "本年2月份的天数不能超过28。" ;
                succ = 0 ;
              }
          }
        }
      }
    }
  }
  if( succ == 0 )
  {
    date_string = dt.getFullYear()+spera1+(dt.getMonth()+1)+spera2+dt.getDate() ;
    if( spera1 == "年") date_string += "日" ;
    if( minDate != "")
    {
      if( PanCompareDate(date_string, minDate) < 0 )
      {
        if( defaultDate == "")
        {
          date_string = minDate.substring(0,4) + spera1 + minDate.substring(4,6) + spera2 + minDate.substring(6,8) ;
        }
        else
        {
          date_string = defaultDate.substring(0,4) + spera1 + defaultDate.substring(4,6) + spera2 + defaultDate.substring(6,8) ;
        }
        if( spera1 == "年") date_string += "日" ;
      }
    }
    if( maxDate != "" )
    {
      if( PanCompareDate(date_string, maxDate) > 0 )
      {
        if( defaultDate == "")
        {
          date_string = maxDate.substring(0,4) + spera1 + maxDate.substring(4,6) + spera2 + maxDate.substring(6,8) ;
        }
        else
        {
          date_string = defaultDate.substring(0,4) + spera1 + defaultDate.substring(4,6) + spera2 + defaultDate.substring(6,8) ;
        }
        if( spera1 == "年") date_string += "日" ;
      }
    }
    ret_value[1] = date_string ;
    return ret_value ;
  }
 
  if( spera1 == "年"  && s_date.indexOf("日") <= 0 )
    s_date += "日" ;
  if( minDate != "")
  {
    if( PanCompareDate(s_date, minDate ) < 0 )
    {
      if( defaultDate == "")
      {
        date_string = minDate.substring(0,4) + spera1 + minDate.substring(4,6) + spera2 + minDate.substring(6,8) ;
      }
      else
      {
        date_string = defaultDate.substring(0,4) + spera1 + defaultDate.substring(4,6) + spera2 + defaultDate.substring(6,8) ;
      }
      if( spera1 == "年") date_string += "日" ;
      ret_value[0] = "Error" ;
      ret_value[1] = date_string ;
      ret_value[2] = "选择(或输入)的日期小于指定的最小日期" + minDate + "。" ;
      succ = 0 ;
    }
  }
  if( maxDate != "" )
  {
    if( PanCompareDate( s_date, maxDate ) > 0 )
    {
      if( defaultDate == "")
      {
        date_string = maxDate.substring(0,4) + spera1 + maxDate.substring(4,6) + spera2 + maxDate.substring(6,8) ;
      }
      else
      {
          date_string = defaultDate.substring(0,4) + spera1 + defaultDate.substring(4,6) + spera2 + defaultDate.substring(6,8) ;
      }      
      if( spera1 == "年") date_string += "日" ;
      ret_value[0] = "Error" ;
      ret_value[1] = date_string ;
      ret_value[2] = "选择(或输入)的日期大于指定的最大日期" + maxDate+ "。" ;
      succ = 0 ;
    }
  }
  if( succ != 0 )
    ret_value[0] = s_date ;
  return ret_value;
}

function PanCompareDate(　date1, date2 )
{
  if( date1 == date2 ) return 0 ;
  if( date1 == undefined || date1 == null || date1 == "" ) return -1 ;
  if( date2 == undefined || date2 == null || date2 == "" ) return 1 ;
  var _date1 = PanNumStrFromDate(date1) ;
  var _date2 = PanNumStrFromDate(date2) ;
  if( _date1 == _date2 ) return 0 ;
  if( _date1 > _date2 ) return 1 ;
  if( _date1 < _date2 ) return -1 ;
  return 0 ;
}

//根据一个日期得到一个8位的年月日字符串
function PanNumStrFromDate( dateVal )
{
  if( dateVal == undefined || dateVal == null || dateVal == "" ) return "" ;
  var year = "", mon = "", day = "" ;
  if( !isNaN(dateVal) && dateVal.length == 8 )
  {
    return dateVal ;
  }
  if( dateVal.length < 8 ) return "Small" ;
  //得到年
  year = dateVal.substring(0,4) ;
  var i = 4 ;
  var j = 0 ;
  while( (i < dateVal.length) && (j < 2) )
  {
    if( !isNaN( dateVal.substring(i, i+1 )))
    {
      mon = mon + dateVal.substring(i,i+1) ;
      j ++ ;
    }
    else
    {
      if( j > 0 ) break ;
    }
    i ++ ;
  }

  if( mon == "") return "Mon" ;
  if( mon.length == 1 ) mon = "0" + mon ;
  j = 0 ;
  while( i < dateVal.length && j < 2 )
  {
    if( !isNaN( dateVal.substring(i, i +1)))
    {
      day = day + dateVal.substring(i,i+1) ;
      j ++ ;
    }
    else
    {
      if( j > 0 ) break ;
    }
    i ++ ;
  }
  if( day == "") return "Day" ;
  if( day.length == 1 ) day = "0" + day ;
  return year + mon + day ;
}
//Return the bill detail data object
function GetDictInfo_Item( dictid, no, currentrow,startpos, propty_lst,relation_lst, cacheid, relationEx, outCacheID, outRow, whereList)//, mustDetail  )
{
	var value=new Array();	
  if( relationEx == undefined || relationEx == null ) relationEx = "" ;
  if( outCacheID == undefined || outCacheID == null ) outCacheID = "" ;
  if( outRow == undefined || outRow == null ) outRow = 0 ;
  if( whereList == undefined || whereList == null ) whereList = "" ;
  
  /*
  if( no != undefined && no != null && Trim(no) != "")
  {
    var temp = 0 ;
    temp = parseInt( no ) ;
    if( isNaN(temp) || temp == null || temp == undefined )
    {
      var url = GetVirtualPath() ;
      url += "Common/GetDataHelp.htm"; //"?DataHelpID="+dataHelpID+"&ReturnColumn="+returnColumn +"&CurrentRow="+(currentrow)+"&StartPos="+(startpos)+"&DataCacheID="+cacheid +"&WhereList="+whereList+"&RelationList="+relationList ;
      page_style = "dialogWidth:400px;dialogHeight:420px;" ;
      url += "?DataHelpID="+dictid+"&ReturnColumn="+propty_lst +"&CurrentRow="+(currentrow)+"&StartPos="+(startpos)+"&DataCacheID="+cacheid +"&WhereList="+whereList+"&RelationList="+relation_lst ;
      url += "&RelationEx="+relationEx+"&OutCacheID=" + outCacheID + "&OutRow="+outRow + "&ListID="+ no ;
      value = window.showModalDialog( url, "", page_style ) ;
      if( value == undefined ) value = null ;
      if( value == null ) value = new Array() ;
      //if( value.length != 1 ||  value[0] != "--NoData--NoData--")
      //{
      if( value.length > 0 ) if( value[0] == "--NoData--NoData--" ) value = new Array() ;
        for( var i = value.length - 1; i >= 0; i -- )
        {
          value[i + 1] = value[i] ;
        }
        value[0] = "--List--List--" ;
        return value ;
      //}
    }
  }*/
  value = new Array() ;
  var g_xml_doc = new ActiveXObject("Microsoft.XMLDOM");

	g_xml_doc.async = false;
	var xml_url= GetVirtualPath() ;

	xml_url += "Common/DataHelp/DataHelp.aspx?Oper_Type=GetDictInfoItemTable&dict_id="+dictid+"&dict_no=" + no + "&currentRow=" + (currentrow)+ "&startpos="+(startpos)+"&cache_id="+cacheid+"&full_list="+propty_lst +"&relation_list="+ relation_lst ;
    xml_url += "&RelationEx="+relationEx+"&OutCacheID=" + outCacheID + "&OutRow="+outRow+"&WhereList="+whereList ;//+"&MustDetail="+mustDetail ;
	var success=g_xml_doc.load(xml_url);
	if(success==false)
	{
	  return value ;
	}

	var root=g_xml_doc.documentElement;
	if(root.nodeName=="Error")
	{
	  return value ;
	}
	//  throw root.text;
	var node_List=root.childNodes;
	for(i=0;i<node_List.length;i++){
		value[i]=node_List.item(i).text;
	}
	return value ;
}


/*
//Return the bill detail data object
function GetDictInfo_Item( dictid, no, currentrow,startpos, propty_lst,relation_lst, cacheid, relationEx, outCacheID, outRow, whereList)//, mustDetail  )
{
	var value=new Array();	
  if( relationEx == undefined || relationEx == null ) relationEx = "" ;
  if( outCacheID == undefined || outCacheID == null ) outCacheID = "" ;
  if( outRow == undefined || outRow == null ) outRow = 0 ;
  if( whereList == undefined || whereList == null ) whereList = "" ;
  var isSelected = false ;

  var temp = 0;
  var len = 0 ;
  var str = "" ;
  var url = "" ;

  if( no != undefined && no != null && Trim(no) != "")
  {
    len = no.length ;
    temp = parseInt( no ) ;
    str = String(temp) ;
    str = Fill('0', len - str.length) + str ;
    if( isNaN(temp) || temp == null || temp == undefined || str !=  Trim(no))
    {
      url = GetVirtualPath() ;
      url += "Common/DataHelp/QuickSelectData.htm"; //?DataHelpID=Departments','','dialogWidth:405px;dialogHeight:400px;status:0;";
      url += "?DataHelpID="+dictid+"&ReturnColumn="+propty_lst +"&CurrentRow="+(currentrow)+"&StartPos="+(startpos)+"&DataCacheID="+cacheid +"&WhereList="+whereList+"&RelationList="+relation_lst ;
      url += "&RelationEx="+relationEx+"&OutCacheID=" + outCacheID + "&OutRow="+outRow + "&ListID="+ no + "&AutoSelect=true" ;

      value = window.showModalDialog( url, "", "dialogWidth:418px;dialogHeight:420px;status:0;" ) ;
      isSelected = true;
      if( value == undefined ) value = null ;
      if( value != null )
      {
        if( value.length > 0 ) if( value[0] == "--NoData--NoData--" ) value = new Array() ;
        for( var i = value.length - 1; i >= 0; i -- )
        {
          value[i + 1] = value[i] ;
        }
        value[0] = "--List--List--" ;
        return value ;
      }
      //}
    }
  }

  value = new Array() ;
  var g_xml_doc = new ActiveXObject("Microsoft.XMLDOM");

	g_xml_doc.async = false;
	var xml_url= GetVirtualPath() ;

	xml_url += "Common/DataHelp/DataHelp.aspx?Oper_Type=GetDictInfoItemTable&dict_id="+dictid+"&dict_no=" + no + "&currentRow=" + (currentrow)+ "&startpos="+(startpos)+"&cache_id="+cacheid+"&full_list="+propty_lst +"&relation_list="+ relation_lst ;
  xml_url += "&RelationEx="+relationEx+"&OutCacheID=" + outCacheID + "&OutRow="+outRow+"&WhereList="+whereList ;//+"&MustDetail="+mustDetail ;
	var success=g_xml_doc.load(xml_url);
	if(success)
	{
	  var root=g_xml_doc.documentElement;
	  if(root.nodeName != "error")
	  {
	    var node_List=root.childNodes;
	    for(i=0;i<node_List.length;i++){
		    value[i]=node_List.item(i).text;
	    }
	    if( value.length <= 1 )
	    {
	      if( value.length != 1 || value[0] != "--List--List--" )
	      {
	      }
	      else
	      {
	        return value ;
	      }
	    }
	    else if (value.length == 3 && value[1] == "Error-Error" )
	    {
	    }
	    else
	    {
	      return value ;
	    }
	  }
	}
		
	var selValue = new Array() ;
	if(isSelected)
	{
	  return value ;
	}
  url = GetVirtualPath() ;
  url += "Common/DataHelp/QuickSelectData.htm"; //?DataHelpID=Departments','','dialogWidth:405px;dialogHeight:400px;status:0;";
  url += "?DataHelpID="+dictid+"&ReturnColumn="+propty_lst +"&CurrentRow="+(currentrow)+"&StartPos="+(startpos)+"&DataCacheID="+cacheid +"&WhereList="+whereList+"&RelationList="+relation_lst ;
  url += "&RelationEx="+relationEx+"&OutCacheID=" + outCacheID + "&OutRow="+outRow + "&ListID="+ no + "&AutoSelect=true" ;

  selValue = window.showModalDialog( url, "", "dialogWidth:413px;dialogHeight:420px;status:0;" ) ;
  if( selValue == undefined ) selValue = null ;
  if( selValue != null )
  {
    if( selValue.length > 0 )
    {
      if( selValue[0] == "--NoData--NoData--" )
      {
        return value ;
      }
    }
    else
    {
      return value ;
    }
    for( var i = selValue.length - 1; i >= 0; i -- )
    {
      selValue[i + 1] = selValue[i] ;
    }
    selValue[0] = "--List--List--" ;
    return selValue ;
  }
  return value ;
  
}
*/
//Clear the table's row in the Session
function ClearGridTableRow( currentrow, startpos, cacheid )
{
  var g_xml_doc = new ActiveXObject("Microsoft.XMLDOM");
	g_xml_doc.async = false;
	var xml_url= GetVirtualPath() ;
	xml_url += "Common/DataHelp/DataHelp.aspx?Oper_Type=ClearGridTableRow&currentRow=" + (currentrow)+ "&startpos="+(startpos)+"&cache_id="+cacheid ;
  var success=g_xml_doc.load(xml_url);
	return success ;
}

//Get grid items value
function GetBillGridValue( currentrow, startpos, column_lst, cacheid )
{
  var value = new Array() ;
  if( column_lst == "" ) return value ;
  var g_xml_doc = new ActiveXObject("Microsoft.XMLDOM");
	g_xml_doc.async = false;
	var xml_url= GetVirtualPath() ;
	xml_url += "Common/DataHelp/DataHelp.aspx?Oper_Type=GetGridValue&currentRow=" + (currentrow)+ "&startpos="+(startpos)+"&cache_id="+cacheid+"&col_name="+column_lst ;
	var success=g_xml_doc.load(xml_url);
	if( success == false ) return value ;
	
	var root=g_xml_doc.documentElement;
	if(root.nodeName=="error")
	{
	  return value ;
	}
	//  throw root.text;
	var node_List=root.childNodes;
	
	if( node_List.length == 2 )
	{
	  alert("Error") ;
	  if( node_List.item(0).text == "Error-Error" )
	  {
	  alert( node_List.item(1).text ) ;
	  return value ;
	  }
	}
  var str = "" ;
	for(i=0;i<node_List.length;i++){
		value[i]=node_List.item(i).text;
		str = str + "--" + value[i] ;
	}
	return value ;
	
}

// return the virtual path
function GetVirtualPath(){
	var p = window.location.pathname;
	if(p.substring(0,1)!="/")
		p = "/"+p;
	var str = p.replace(/^[/]\w*[/]/,"");			//将虚拟目录从字符串中清除
	return p.substring(0,p.length - str.length);	//返回虚拟目录
}

//Right key help
//The last param not use
function GetDataHelp( dataHelpID, returnColumn, currentrow, startpos, cacheid, whereList, relationList, listid, helptype, relationEx, outCacheID, outRow, mustDetail )
{
  var value = new Array() ;
  var page_style = "dialogWidth:400px;dialogHeight:420px;" ;
  if( helptype == undefined || helptype == null || helptype == "" ) helptype = "Normal" ;
  if( mustDetail == undefined || mustDetail == null || mustDetail == "" ) mustDetail = "false" ;
  if( listid == undefined || listid == null ) listid = "" ;
  
  var add_param = "" ;
  /*
  if( helptype == "Level" )
  {
    url += "Common/GetDataHelpLevel.htm" ;//"?DataHelpID="+dataHelpID+"&ReturnColumn="+returnColumn +"&CurrentRow="+(currentrow)+"&StartPos="+(startpos)+"&DataCacheID="+cacheid +"&WhereList="+whereList+"&RelationList="+relationList ;
    page_style = "dialogWidth:550px;dialogHeight:400px;status:0;" ;
    add_param = "&MustDetail="+mustDetail ;
  }
  else
  {
    url += "Common/GetDataHelp.htm"; //"?DataHelpID="+dataHelpID+"&ReturnColumn="+returnColumn +"&CurrentRow="+(currentrow)+"&StartPos="+(startpos)+"&DataCacheID="+cacheid +"&WhereList="+whereList+"&RelationList="+relationList ;
    page_style = "dialogWidth:400px;dialogHeight:400px;status:0;" ;
  }
  */
  var url ;
  url = GetVirtualPath() ;

  url += "Common/DataHelp/QuickSelectData.htm"; //?DataHelpID=Departments','','dialogWidth:415px;dialogHeight:415px;status:0;";
  if( relationEx == undefined || relationEx == null ) relationEx = "" ;
  if( outCacheID == undefined || outCacheID == null ) outCacheID = "" ;
  if( outRow == undefined || outRow == null ) outRow = 0 ;
  
  add_param = "DataHelpID="+dataHelpID+"&ReturnColumn="+returnColumn +"&CurrentRow="+(currentrow)+"&StartPos="+(startpos)+"&DataCacheID="+cacheid +"&WhereList="+whereList+"&RelationList="+relationList ;
  add_param += "&ListID=" + listid + "&RelationEx="+relationEx+"&OutCacheID=" + outCacheID + "&OutRow="+outRow ;
//alert(add_param);
  value = window.showModalDialog( url, add_param, "dialogWidth:418px;dialogHeight:420px;status:0;" ) ;
  if( value == undefined ) value = null ;
  return value ;
}


function SetCacheTableValue( cacheid, currentrow, column_lst, val_lst, startpos )
{
  var value = new Array() ;
  if( column_lst == "" ) return value ;

  if( startpos == undefined || startpos == null ) startpos = 0 ;
  if( isNaN(startpos)) startpos = 0 ;
  
  var g_xml_doc = new ActiveXObject("Microsoft.XMLDOM");
	g_xml_doc.async = false;
	var xml_url= GetVirtualPath() ;
	xml_url += "Common/DataHelp/DataHelp.aspx?Oper_Type=SetCacheTableValue&currentRow=" + (currentrow)+ "&startpos="+(startpos)+"&cache_id="+cacheid+"&col_list="+column_lst+"&value_list="+val_lst ;

	var success=g_xml_doc.load(xml_url);
	if( success == false ) return value ;
	
	var root=g_xml_doc.documentElement;
	if(root.nodeName=="error")
	{
	  return value ;
	}
	//  throw root.text;
	var node_List=root.childNodes;
	if( node_List.length == 2 )
	{
	  if( node_List.item(0).text == "Error-Error" )
	  {
	  alert( node_List.item(1).text ) ;
	  return value ;
	  }
	}
	for(i=0;i<node_List.length;i++){
		value[i]=node_List.item(i).text;
	}
	return value;

}

/*============================================*\
format the input textbox's value
  Param:
     value: the textbox's input value
     type: defined the value's type
     len:  defined the value's length
     precision: the Number value's precision
     other: other property
  return : return the valid value
\*============================================*/
function FormatStringValue(value, type, len, precision, other)
{
  if( Trim(value) == "") return null ;
  
  if( len == undefined || len == null ) len = 0 ;
  if( precision == undefined || precision == null ) precision = 0 ;
  if( other == undefined || other == null ) other = "" ;

  var ret = new Array() ;
  switch(type)
  {
    case "String":
      if( other  == 'a' )
      {
        ret[0] = value.toLowerCase() ;
      }
      else if( other == 'A') ;
      {
        ret[0] = value.toUpperCase() 
      }
      if( len <= 0 )
        ret[0] = ret[0] ;
      else
      {
        if( ret[0].length > len )
          ret[0] = ret[0].Substring( 0, len ) ;
      }
      break ;
    case "Number":
      if( isNaN(value))
      {
        ret[0] = "Error" ;
        ret[1] = "" ;
        ret[2] = "输入的数据["+value+"]不是一个合法的数值。" ;
        break ;
      }
      if( precision < 0 ) return value ;
      try
      {
        var i_value = parseInt( value ) ;
        if( isNaN(i_value)) i_value = 0 ;
        if( precision == 0 )
        {
          ret[0] = i_value.toString() ;
          break ;
        }
        var f_value = parseFloat(value) ;
        if( isNaN(f_value)) f_value = 0 ;
        var pre = Math.round((f_value - i_value) * Math.pow(10,precision )) ;
        if( Math.abs(pre / Math.pow(10, precision)) >= 1 )
        {
          i_value += Math.round( pre / Math.pow(10, precision)) ;
          pre = Math.abs(pre) ;
          pre = pre - Math.round( pre / Math.pow( 10, precision)) * Math.pow( 10, precision);
        }
        pre = Math.abs(pre) ;
        if( i_value == 0 && pre != 0 && f_value < 0 )
        {
          ret[0] = "-0." ;
        }
        else
        {
          ret[0] = i_value.toString() ;
          ret[0] += "." ;
        }
        if( pre != 0 )
        {
          var temp = pre.toString() ;
          temp = Fill("0", precision - temp.length) + temp ;
          ret[0] += temp ;//pre.toString() ;
        }
        else
        {
          ret[0] += Fill( "0", precision) ;
        }
      }
      catch(err)
      {
        return null ;
      }
      break ;
      
    case "Date":
      if( value.length > 12 && value.indexOf( " " ) > 0 )
      {
        var pos = value.indexOf(" ") ;
        value = value.substring(0, pos ) ;
      }
      ret = parseDate( value,"" ,"", other) ;
      return ret ;
      break ;
      
    default:
      ret  = value ;
  }
  return ret ;
}

function Fill( ch, len )
{
  var ret = "" ;
  if( len == undefined || len == null || len <= 0 ) return "" ;
  if( ch == undefined || ch == null || ch == "" ) ch = " " ;
  try
  {
    if( ch.length > 1 ) ch = ch.sustring(0, 1) ;
  }
  catch( err )
  {
    ch = " " ;
  }
  for( var i = 0; i < len ; i ++ )
  {
    ret += ch ;
  }
  return ret ;
}

function TextBoxValueFormat( textbox, format, len, precision, other )
{
  if( textbox == null || textbox == undefined ) return ;
  var data = textbox.value ;
  if( data == null || data == "" ) return  ;
  
  var value = new Array() ;
  value = FormatStringValue( data, format, len, precision, other )
  if( value == null ) return ;
  if( value.length == 3 && value[0] == "Error")
  {
    textbox.value = value[1] ;
    alert(value[2]) ;
    return ;
  }
  if( data != value[0]) textbox.value = value[0] ;
  return ;
}

// Clear Select object options
function ClearSelectOptions( obj )
{
  if( obj == undefined || obj == null ) return ;
  try
  {
    for( var i = obj.options.length - 1; i >= 0 ; i -- )
      obj.options.remove(i) ;
    return ;
  }
  catch(err)
  {
    return ;
  }
}

function AddSelectOption( obj, text, value )
{
  if( obj == undefined || obj == null ) return ;
  if( text == undefined || text == null ) return ;
  if( value == undefined || value == null || Trim(value) == "") value = text ;
  var theOption = document.createElement("OPTION"); 
  obj.options.add( theOption) ;
  theOption.innerText = text ;
  theOption.Value = value ;
  return obj.options.length ;
  
}

 