if ($.fn.pagination){
	$.fn.pagination.defaults.beforePageText = 'ページ';
	$.fn.pagination.defaults.afterPageText = '{pages} 中';
	$.fn.pagination.defaults.displayMsg = '全 {total} アイテム中 {from} から {to} を Representation ';
}
if ($.fn.datagrid){
	$.fn.datagrid.defaults.loadMsg = ' Treatment です.少々お待ちください...';
}
if ($.fn.treegrid && $.fn.datagrid){
	$.fn.treegrid.defaults.loadMsg = $.fn.datagrid.defaults.loadMsg;
}
if ($.messager){
	$.messager.defaults.ok = 'OK';
	$.messager.defaults.cancel = 'キャンセル';
}
if ($.fn.validatebox){
	$.fn.validatebox.defaults.missingMessage = ' Into force は Have to です.';
	$.fn.validatebox.defaults.rules.email.message = '正しいメールアドレスを Into force してください.';
	$.fn.validatebox.defaults.rules.url.message = '正しいURLを Into force してください.';
	$.fn.validatebox.defaults.rules.length.message = '{0} から {1} の Fan Tong の正しい値を Into force してください.';
	$.fn.validatebox.defaults.rules.remote.message = 'このフィールドを Correct してください.';
}
if ($.fn.numberbox){
	$.fn.numberbox.defaults.missingMessage = ' Into force は Have to です.';
}
if ($.fn.combobox){
	$.fn.combobox.defaults.missingMessage = ' Into force は Have to です.';
}
if ($.fn.combotree){
	$.fn.combotree.defaults.missingMessage = ' Into force は Have to です.';
}
if ($.fn.combogrid){
	$.fn.combogrid.defaults.missingMessage = ' Into force は Have to です.';
}
if ($.fn.calendar){
	$.fn.calendar.defaults.weeks = ['日','月','火','水','木','金','土'];
	$.fn.calendar.defaults.months = ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'];
}
if ($.fn.datebox){
	$.fn.datebox.defaults.currentText = ' Today ';
	$.fn.datebox.defaults.closeText = '閉じる';
	$.fn.datebox.defaults.okText = 'OK';
	$.fn.datebox.defaults.missingMessage = ' Into force は Have to です.';
}
if ($.fn.datetimebox && $.fn.datebox){
	$.extend($.fn.datetimebox.defaults,{
		currentText: $.fn.datebox.defaults.currentText,
		closeText: $.fn.datebox.defaults.closeText,
		okText: $.fn.datebox.defaults.okText,
		missingMessage: $.fn.datebox.defaults.missingMessage
	});
}
