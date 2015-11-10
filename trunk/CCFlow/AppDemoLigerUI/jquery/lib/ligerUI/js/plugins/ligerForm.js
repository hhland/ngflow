/**
* jQuery ligerUI 1.1.9
* 
* http://ligerui.com
*  
* Author daomi 2012 [ gd_star@163.com ] 
* 
*/
(function ($)
{
    $.fn.ligerForm = function ()
    {
        return $.ligerui.run.call(this, "ligerForm", arguments);
    };

    $.ligerDefaults = $.ligerDefaults || {};
    $.ligerDefaults.Form = {
        // Width control 
        inputWidth: 180,
        // Label width 
        labelWidth: 90,
        // Interval Width 
        space: 40,
        rightToken: ':',
        // Label Alignment 
        labelAlign: 'left',
        // Controls alignment 
        align: 'left',
        // Field 
        fields: [],
        // Whether to create additional form elements ID
        appendID: true,
        // Generate form elements ID Prefix 
        prefixID: "",
        //json Analytic Functions 
        toJSON: $.ligerui.toJSON
    };

    //@description  The default constructor Extension Form Editor ( If you are not satisfied with the effect created form   Recommend overloading )
    //@param {jinput}  Form elements jQuery Object   Such as input,select,textarea 
    $.ligerDefaults.Form.editorBulider = function (jinput)
    {
        // Here this Is form的ligerui Object 
        var g = this, p = this.options;
        var inputOptions = {};
        if (p.inputWidth) inputOptions.width = p.inputWidth;
        if (jinput.is("select"))
        {
            jinput.ligerComboBox(inputOptions);
        }
        else if (jinput.is(":text") || jinput.is(":password"))
        {
            var ltype = jinput.attr("ltype");
            switch (ltype)
            {
                case "select":
                case "combobox":
                    jinput.ligerComboBox(inputOptions);
                    break;
                case "spinner":
                    jinput.ligerSpinner(inputOptions);
                    break;
                case "date":
                    jinput.ligerDateEditor(inputOptions);
                    break;
                case "float":
                case "number":
                    inputOptions.number = true;
                    jinput.ligerTextBox(inputOptions);
                    break;
                case "int":
                case "digits":
                    inputOptions.digits = true;
                default:
                    jinput.ligerTextBox(inputOptions);
                    break;
            }
        }
        else if (jinput.is(":radio"))
        {
            jinput.ligerRadio(inputOptions);
        }
        else if (jinput.is(":checkbox"))
        {
            jinput.ligerCheckBox(inputOptions);
        }
        else if (jinput.is("textarea"))
        {
            jinput.addClass("l-textarea");
        }
    }

    // Form Components 
    $.ligerui.controls.Form = function (element, options)
    {
        $.ligerui.controls.Form.base.constructor.call(this, element, options);
    };

    $.ligerui.controls.Form.ligerExtend($.ligerui.core.UIComponent, {
        __getType: function ()
        {
            return 'Form'
        },
        __idPrev: function ()
        {
            return 'Form';
        },
        _init: function ()
        {
            $.ligerui.controls.Form.base._init.call(this);
        },
        _render: function ()
        {
            var g = this, p = this.options;
            var jform = $(this.element);
            // Automatically create a form 
            if (p.fields && p.fields.length)
            {
                if (!jform.hasClass("l-form"))
                    jform.addClass("l-form");
                var out = [];
                var appendULStartTag = false;
                $(p.fields).each(function (index, field)
                {
                    var name = field.name || field.id;
                    if (!name) return;
                    if (field.type == "hidden")
                    {
                        out.push('<input type="hidden" id="' + name + '" name="' + name + '" />');
                        return;
                    }
                    var newLine = field.renderToNewLine || field.newline;
                    if (newLine == null) newLine = true;
                    if (field.merge) newLine = false;
                    if (field.group) newLine = true;
                    if (newLine)
                    {
                        if (appendULStartTag)
                        {
                            out.push('</ul>');
                            appendULStartTag = false;
                        }
                        if (field.group)
                        {
                            out.push('<div class="l-group');
                            if (field.groupicon)
                                out.push(' l-group-hasicon');
                            out.push('">');
                            if (field.groupicon)
                                out.push('<img src="' + field.groupicon + '" />');
                            out.push('<span>' + field.group + '</span></div>');
                        }
                        out.push('<ul>');
                        appendULStartTag = true;
                    }
                    //append label
                    out.push(g._buliderLabelContainer(field));
                    //append input 
                    out.push(g._buliderControlContainer(field));
                    //append space
                    out.push(g._buliderSpaceContainer(field));
                });
                if (appendULStartTag)
                {
                    out.push('</ul>');
                    appendULStartTag = false;
                }
                jform.append(out.join(''));
            }
            // Generate ligerui Form Style 
            $("input,select,textarea", jform).each(function ()
            {
                p.editorBulider.call(g, $(this));
            });
        },
        // Tags section 
        _buliderLabelContainer: function (field)
        {
            var g = this, p = this.options;
            var label = field.label || field.display;
            var labelWidth = field.labelWidth || field.labelwidth || p.labelWidth;
            var labelAlign = field.labelAlign || p.labelAlign;
            if (label) label += p.rightToken;
            var out = [];
            out.push('<li style="');
            if (labelWidth)
            {
                out.push('width:' + labelWidth + 'px;');
            }
            if (labelAlign)
            {
                out.push('text-align:' + labelAlign + ';');
            }
            out.push('">');
            if (label)
            {
                out.push(label);
            }
            out.push('</li>');
            return out.join('');
        },
        // Controls section 
        _buliderControlContainer: function (field)
        {
            var g = this, p = this.options;
            var width = field.width || p.inputWidth;
            var align = field.align || field.textAlign || field.textalign || p.align;
            var out = [];
            out.push('<li style="');
            if (width)
            {
                out.push('width:' + width + 'px;');
            }
            if (align)
            {
                out.push('text-align:' + align + ';');
            }
            out.push('">');
            out.push(g._buliderControl(field));
            out.push('</li>');
            return out.join('');
        },
        // Interval section 
        _buliderSpaceContainer: function (field)
        {
            var g = this, p = this.options;
            var spaceWidth = field.space || field.spaceWidth || p.space;
            var out = [];
            out.push('<li style="');
            if (spaceWidth)
            {
                out.push('width:' + spaceWidth + 'px;');
            }
            out.push('">');
            out.push('</li>');
            return out.join('');
        },
        _buliderControl: function (field)
        {
            var g = this, p = this.options;
            var width = field.width || p.inputWidth;
            var name = field.name || field.id;
            var out = [];
            if (field.comboboxName && field.type == "select")
            {
                out.push('<input type="hidden" id="' + p.prefixID + name + '" name="' + name + '" />');
            }
            if (field.textarea || field.type == "textarea")
            {
                out.push('<textarea ');
            }
            else if (field.type == "checkbox")
            {
                out.push('<input type="checkbox" ');
            }
            else if (field.type == "radio")
            {
                out.push('<input type="radio" ');
            }
            else if (field.type == "password")
            {
                out.push('<input type="password" ');
            }
            else
            {
                out.push('<input type="text" ');
            }
            if (field.cssClass)
            {
                out.push('class="' + field.cssClass + '" ');
            }
            if (field.type)
            {
                out.push('ltype="' + field.type + '" ');
            }
            if (field.attr)
            {
                for (var attrp in field.attr)
                {
                    out.push(attrp + '="' + field.attr[attrp] + '" ');
                }
            }
            if (field.comboboxName && field.type == "select")
            {
                out.push('name="' + field.comboboxName + '"');
                if (p.appendID)
                {
                    out.push(' id="' + p.prefixID + field.comboboxName + '" ');
                }
            }
            else
            {
                out.push('name="' + name + '"');
                if (p.appendID)
                {
                    out.push(' id="' + name + '" ');
                }
            }
            // Parameters 
            var fieldOptions = $.extend({
                width: width - 2
            }, field.options || {});
            out.push(" ligerui='" + p.toJSON(fieldOptions) + "' ");
            // Authentication parameters 
            if (field.validate)
            {
                out.push(" validate='" + p.toJSON(field.validate) + "' ");
            }
            out.push(' />');
            return out.join('');
        }
    });
})(jQuery);