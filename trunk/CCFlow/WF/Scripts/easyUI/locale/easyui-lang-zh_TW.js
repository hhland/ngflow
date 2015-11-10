if ($.fn.pagination){
	$.fn.pagination.defaults.beforePageText = '第';
	$.fn.pagination.defaults.afterPageText = '共{pages}頁';
	$.fn.pagination.defaults.displayMsg = ' Show {from}到{to},共{total} Record ';
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
}
if ($.fn.datetimebox && $.fn.datebox){
	$.extend($.fn.datetimebox.defaults,{
		currentText: $.fn.datebox.defaults.currentText,
		closeText: $.fn.datebox.defaults.closeText,
		okText: $.fn.datebox.defaults.okText,
		missingMessage: $.fn.datebox.defaults.missingMessage
	});
}
