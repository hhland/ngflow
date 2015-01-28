if ($.fn.pagination){
	$.fn.pagination.defaults.beforePageText = 'Page';
	$.fn.pagination.defaults.afterPageText = 'Total {pages}';
	$.fn.pagination.defaults.displayMsg = ' Show {from} To {to},Total {total} Records ';
}
if ($.fn.datagrid){
	$.fn.datagrid.defaults.loadMsg = ' Being processed , Please wait ...';
}
if ($.fn.treegrid && $.fn.datagrid){
	$.fn.treegrid.defaults.loadMsg = $.fn.datagrid.defaults.loadMsg;
}
if ($.messager){
	$.messager.defaults.ok = ' Determine ';
	$.messager.defaults.cancel = ' Cancel ';
}
if ($.fn.validatebox){
	$.fn.validatebox.defaults.missingMessage = ' The entry is losing items ';
	$.fn.validatebox.defaults.rules.email.message = ' Please enter a valid email address ';
	$.fn.validatebox.defaults.rules.url.message = ' Please enter a valid URL Address ';
	$.fn.validatebox.defaults.rules.length.message = ' Enter the content length must be between {0}和{1} Between ';
	$.fn.validatebox.defaults.rules.remote.message = ' Please fix this field ';
}
if ($.fn.numberbox){
	$.fn.numberbox.defaults.missingMessage = ' The entry is losing items ';
}
if ($.fn.combobox){
	$.fn.combobox.defaults.missingMessage = ' The entry is losing items ';
}
if ($.fn.combotree){
	$.fn.combotree.defaults.missingMessage = ' The entry is losing items ';
}
if ($.fn.combogrid){
	$.fn.combogrid.defaults.missingMessage = ' The entry is losing items ';
}
if ($.fn.calendar){
	$.fn.calendar.defaults.weeks = ['日','一','二','三','四','五','六'];
	$.fn.calendar.defaults.months = [' January ',' February ',' March ',' April ',' May ',' June ',' July ',' August ',' September ',' October ',' November ',' December '];
}
if ($.fn.datebox){
	$.fn.datebox.defaults.currentText = ' Today ';
	$.fn.datebox.defaults.closeText = ' Shut down ';
	$.fn.datebox.defaults.okText = ' Determine ';
	$.fn.datebox.defaults.missingMessage = ' The entry is losing items ';
	$.fn.datebox.defaults.formatter = function(date){
		var y = date.getFullYear();
		var m = date.getMonth()+1;
		var d = date.getDate();
		return y+'-'+(m<10?('0'+m):m)+'-'+(d<10?('0'+d):d);
	};
	$.fn.datebox.defaults.parser = function(s){
		if (!s) return new Date();
		var ss = s.split('-');
		var y = parseInt(ss[0],10);
		var m = parseInt(ss[1],10);
		var d = parseInt(ss[2],10);
		if (!isNaN(y) && !isNaN(m) && !isNaN(d)){
			return new Date(y,m-1,d);
		} else {
			return new Date();
		}
	};
}
if ($.fn.datetimebox && $.fn.datebox){
	$.extend($.fn.datetimebox.defaults,{
		currentText: $.fn.datebox.defaults.currentText,
		closeText: $.fn.datebox.defaults.closeText,
		okText: $.fn.datebox.defaults.okText,
		missingMessage: $.fn.datebox.defaults.missingMessage
	});
}
