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

    $.fn.ligerComboBox = function (options)
    {
        return $.ligerui.run.call(this, "ligerComboBox", arguments);
    };

    $.fn.ligerGetComboBoxManager = function ()
    {
        return $.ligerui.run.call(this, "ligerGetComboBoxManager", arguments);
    };

    $.ligerDefaults.ComboBox = {
        resize: true,           // Whether to adjust the size of the 
        isMultiSelect: false,   // Are multiple choice 
        isShowCheckBox: false,  // Check box is selected 
        columns: false,       // Form state 
        selectBoxWidth: false, // Width 
        selectBoxHeight: false, // Height 
        onBeforeSelect: false, // Select pre-event 
        onSelected: null, // Select the value of the event  
        initValue: null,
        initText: null,
        valueField: 'id',
        textField: 'text',
        valueFieldID: null,
        slide: true,           // Is displayed in the form of animation 
        split: ";",
        data: null,
        tree: null,            // Drop-down box is displayed in the form of a tree ,tree Parameters with LigerTree The argument is consistent  
        treeLeafOnly: true,   // Whether to select only the leaves 
        grid: null,              // Form 
        onStartResize: null,
        onEndResize: null,
        hideOnLoseFocus: true,
        url: null,              // Data Sources URL( Need to return JSON)
        onSuccess: null,
        onError: null,
        onBeforeOpen: null,      // Open the drop-down box before the event , By return false To prevent the continued operation , Using this parameter can be used to call other functions , For example, open a new window to select the value 
        render: null,            // Text box displays html Function 
        absolute: true         // Whether the selection box attached to the body, And absolute positioning 
    };

    // Extension Methods 
    $.ligerMethos.ComboBox = $.ligerMethos.ComboBox || {};


    $.ligerui.controls.ComboBox = function (element, options)
    {
        $.ligerui.controls.ComboBox.base.constructor.call(this, element, options);
    };
    $.ligerui.controls.ComboBox.ligerExtend($.ligerui.controls.Input, {
        __getType: function ()
        {
            return 'ComboBox';
        },
        _extendMethods: function ()
        {
            return $.ligerMethos.ComboBox;
        },
        _init: function ()
        {
            $.ligerui.controls.ComboBox.base._init.call(this);
            var p = this.options;
            if (p.columns)
            {
                p.isShowCheckBox = true;
            }
            if (p.isMultiSelect)
            {
                p.isShowCheckBox = true;
            }
        },
        _render: function ()
        {
            var g = this, p = this.options;
            g.data = p.data;
            g.inputText = null;
            g.select = null;
            g.textFieldID = "";
            g.valueFieldID = "";
            g.valueField = null; // Hidden Field ( Save value )
            // Box Initialization 
            if (this.element.tagName.toLowerCase() == "input")
            {
                this.element.readOnly = true;
                g.inputText = $(this.element);
                g.textFieldID = this.element.id;
            }
            else if (this.element.tagName.toLowerCase() == "select")
            {
                $(this.element).hide();
                g.select = $(this.element);
                p.isMultiSelect = false;
                p.isShowCheckBox = false;
                g.textFieldID = this.element.id + "_txt";
                g.inputText = $('<input type="text" readonly="true"/>');
                g.inputText.attr("id", g.textFieldID).insertAfter($(this.element));
            } else
            {
                // Other types are not supported 
                return;
            }
            if (g.inputText[0].name == undefined) g.inputText[0].name = g.textFieldID;
            // Hidden field initialization 
            g.valueField = null;
            if (p.valueFieldID)
            {
                g.valueField = $("#" + p.valueFieldID + ":input");
                if (g.valueField.length == 0) g.valueField = $('<input type="hidden"/>');
                g.valueField[0].id = g.valueField[0].name = p.valueFieldID;
            }
            else
            {
                g.valueField = $('<input type="hidden"/>');
                g.valueField[0].id = g.valueField[0].name = g.textFieldID + "_val";
            }
            if (g.valueField[0].name == undefined) g.valueField[0].name = g.valueField[0].id;
            // Switch 
            g.link = $('<div class="l-trigger"><div class="l-trigger-icon"></div></div>');
            // Drop-down box 
            g.selectBox = $('<div class="l-box-select"><div class="l-box-select-inner"><table cellpadding="0" cellspacing="0" border="0" class="l-box-select-table"></table></div></div>');
            g.selectBox.table = $("table:first", g.selectBox);
            // Outer 
            g.wrapper = g.inputText.wrap('<div class="l-text l-text-combobox"></div>').parent();
            g.wrapper.append('<div class="l-text-l"></div><div class="l-text-r"></div>');
            g.wrapper.append(g.link);
            // Add a parcel ,
            g.textwrapper = g.wrapper.wrap('<div class="l-text-wrapper"></div>').parent();

            if (p.absolute)
                g.selectBox.appendTo('body').addClass("l-box-select-absolute");
            else
                g.textwrapper.append(g.selectBox);

            g.textwrapper.append(g.valueField);
            g.inputText.addClass("l-text-field");
            if (p.isShowCheckBox && !g.select)
            {
                $("table", g.selectBox).addClass("l-table-checkbox");
            } else
            {
                p.isShowCheckBox = false;
                $("table", g.selectBox).addClass("l-table-nocheckbox");
            }
            // Switch   Event 
            g.link.hover(function ()
            {
                if (p.disabled) return;
                this.className = "l-trigger-hover";
            }, function ()
            {
                if (p.disabled) return;
                this.className = "l-trigger";
            }).mousedown(function ()
            {
                if (p.disabled) return;
                this.className = "l-trigger-pressed";
            }).mouseup(function ()
            {
                if (p.disabled) return;
                this.className = "l-trigger-hover";
            }).click(function ()
            {
                if (p.disabled) return;
                if (g.trigger('beforeOpen') == false) return false;
                g._toggleSelectBox(g.selectBox.is(":visible"));
            });
            g.inputText.click(function ()
            {
                if (p.disabled) return;
                if (g.trigger('beforeOpen') == false) return false;
                g._toggleSelectBox(g.selectBox.is(":visible"));
            }).blur(function ()
            {
                if (p.disabled) return;
                g.wrapper.removeClass("l-text-focus");
            }).focus(function ()
            {
                if (p.disabled) return;
                g.wrapper.addClass("l-text-focus");
            });
            g.wrapper.hover(function ()
            {
                if (p.disabled) return;
                g.wrapper.addClass("l-text-over");
            }, function ()
            {
                if (p.disabled) return;
                g.wrapper.removeClass("l-text-over");
            });
            g.resizing = false;
            g.selectBox.hover(null, function (e)
            {
                if (p.hideOnLoseFocus && g.selectBox.is(":visible") && !g.boxToggling && !g.resizing)
                {
                    g._toggleSelectBox(true);
                }
            });
            var itemsleng = $("tr", g.selectBox.table).length;
            if (!p.selectBoxHeight && itemsleng < 8) p.selectBoxHeight = itemsleng * 30;
            if (p.selectBoxHeight)
            {
                g.selectBox.height(p.selectBoxHeight);
            }
            // Drop-down box contents initialization 
            g.bulidContent();

            g.set(p);

            // Drop-down box width , Height initialization 
            if (p.selectBoxWidth)
            {
                g.selectBox.width(p.selectBoxWidth);
            }
            else
            {
                g.selectBox.css('width', g.wrapper.css('width'));
            }
        },
        destroy: function ()
        {
            if (this.wrapper) this.wrapper.remove();
            if (this.selectBox) this.selectBox.remove();
            this.options = null;
            $.ligerui.remove(this);
        },
        _setDisabled: function (value)
        {
            // Disable Styles 
            if (value)
            {
                this.wrapper.addClass('l-text-disabled');
            } else
            {
                this.wrapper.removeClass('l-text-disabled');
            }
        },
        _setLable: function (label)
        {
            var g = this, p = this.options;
            if (label)
            {
                if (g.labelwrapper)
                {
                    g.labelwrapper.find(".l-text-label:first").html(label + ':&nbsp');
                }
                else
                {
                    g.labelwrapper = g.textwrapper.wrap('<div class="l-labeltext"></div>').parent();
                    g.labelwrapper.prepend('<div class="l-text-label" style="float:left;display:inline;">' + label + ':&nbsp</div>');
                    g.textwrapper.css('float', 'left');
                }
                if (!p.labelWidth)
                {
                    p.labelWidth = $('.l-text-label', g.labelwrapper).outerWidth();
                }
                else
                {
                    $('.l-text-label', g.labelwrapper).outerWidth(p.labelWidth);
                }
                $('.l-text-label', g.labelwrapper).width(p.labelWidth);
                $('.l-text-label', g.labelwrapper).height(g.wrapper.height());
                g.labelwrapper.append('<br style="clear:both;" />');
                if (p.labelAlign)
                {
                    $('.l-text-label', g.labelwrapper).css('text-align', p.labelAlign);
                }
                g.textwrapper.css({ display: 'inline' });
                g.labelwrapper.width(g.wrapper.outerWidth() + p.labelWidth + 2);
            }
        },
        _setWidth: function (value)
        {
            var g = this;
            if (value > 20)
            {
                g.wrapper.css({ width: value });
                g.inputText.css({ width: value - 20 });
                g.textwrapper.css({ width: value });
            }
        },
        _setHeight: function (value)
        {
            var g = this;
            if (value > 10)
            {
                g.wrapper.height(value);
                g.inputText.height(value - 2);
                g.link.height(value - 4);
                g.textwrapper.css({ width: value });
            }
        },
        _setResize: function (resize)
        {
            // Resize Support 
            if (resize && $.fn.ligerResizable)
            {
                var g = this;
                g.selectBox.ligerResizable({ handles: 'se,s,e', onStartResize: function ()
                {
                    g.resizing = true;
                    g.trigger('startResize');
                }
                , onEndResize: function ()
                {
                    g.resizing = false;
                    if (g.trigger('endResize') == false)
                        return false;
                }
                });
                g.selectBox.append("<div class='l-btn-nw-drop'></div>");
            }
        },
        // Find Text, Applicable checkbox and radio 
        findTextByValue: function (value)
        {
            var g = this, p = this.options;
            if (value == undefined) return "";
            var texts = "";
            var contain = function (checkvalue)
            {
                var targetdata = value.toString().split(p.split);
                for (var i = 0; i < targetdata.length; i++)
                {
                    if (targetdata[i] == checkvalue) return true;
                }
                return false;
            };
            $(g.data).each(function (i, item)
            {
                var val = item[p.valueField];
                var txt = item[p.textField];
                if (contain(val))
                {
                    texts += txt + p.split;
                }
            });
            if (texts.length > 0) texts = texts.substr(0, texts.length - 1);
            return texts;
        },
        // Find Value, Applicable checkbox and radio 
        findValueByText: function (text)
        {
            var g = this, p = this.options;
            if (!text && text == "") return "";
            var contain = function (checkvalue)
            {
                var targetdata = text.toString().split(p.split);
                for (var i = 0; i < targetdata.length; i++)
                {
                    if (targetdata[i] == checkvalue) return true;
                }
                return false;
            };
            var values = "";
            $(g.data).each(function (i, item)
            {
                var val = item[p.valueField];
                var txt = item[p.textField];
                if (contain(txt))
                {
                    values += val + p.split;
                }
            });
            if (values.length > 0) values = values.substr(0, values.length - 1);
            return values;
        },
        removeItem: function ()
        {
        },
        insertItem: function ()
        {
        },
        addItem: function ()
        {

        },
        _setValue: function (value)
        {
            var g = this, p = this.options;
            var text = g.findTextByValue(value);
            if (p.tree)
            {
                g.selectValueByTree(value);
            }
            else if (!p.isMultiSelect)
            {
                g._changeValue(value, text);
                $("tr[value=" + value + "] td", g.selectBox).addClass("l-selected");
                $("tr[value!=" + value + "] td", g.selectBox).removeClass("l-selected");
            }
            else
            {
                g._changeValue(value, text);
                var targetdata = value.toString().split(p.split);
                $("table.l-table-checkbox :checkbox", g.selectBox).each(function () { this.checked = false; });
                for (var i = 0; i < targetdata.length; i++)
                {
                    $("table.l-table-checkbox tr[value=" + targetdata[i] + "] :checkbox", g.selectBox).each(function () { this.checked = true; });
                }
            }
        },
        selectValue: function (value)
        {
            this._setValue(value);
        },
        bulidContent: function ()
        {
            var g = this, p = this.options;
            this.clearContent();
            if (g.select)
            {
                g.setSelect();
            }
            else if (g.data)
            {
                g.setData(g.data);
            }
            else if (p.tree)
            {
                g.setTree(p.tree);
            }
            else if (p.grid)
            {
                g.setGrid(p.grid);
            }
            else if (p.url)
            {
                $.ajax({
                    type: 'post',
                    url: p.url,
                    cache: false,
                    dataType: 'json',
                    success: function (data)
                    {
                        g.data = data;
                        g.setData(g.data);
                        g.trigger('success', [g.data]);
                    },
                    error: function (XMLHttpRequest, textStatus)
                    {
                        g.trigger('error', [XMLHttpRequest, textStatus]);
                    }
                });
            }
        },
        clearContent: function ()
        {
            var g = this, p = this.options;
            $("table", g.selectBox).html("");
            //g.inputText.val("");
            //g.valueField.val("");
        },
        setSelect: function ()
        {
            var g = this, p = this.options;
            this.clearContent();
            $('option', g.select).each(function (i)
            {
                var val = $(this).val();
                var txt = $(this).html();
                var tr = $("<tr><td index='" + i + "' value='" + val + "'>" + txt + "</td>");
                $("table.l-table-nocheckbox", g.selectBox).append(tr);
                $("td", tr).hover(function ()
                {
                    $(this).addClass("l-over");
                }, function ()
                {
                    $(this).removeClass("l-over");
                });
            });
            $('td:eq(' + g.select[0].selectedIndex + ')', g.selectBox).each(function ()
            {
                if ($(this).hasClass("l-selected"))
                {
                    g.selectBox.hide();
                    return;
                }
                $(".l-selected", g.selectBox).removeClass("l-selected");
                $(this).addClass("l-selected");
                if (g.select[0].selectedIndex != $(this).attr('index') && g.select[0].onchange)
                {
                    g.select[0].selectedIndex = $(this).attr('index'); g.select[0].onchange();
                }
                var newIndex = parseInt($(this).attr('index'));
                g.select[0].selectedIndex = newIndex;
                g.select.trigger("change");
                g.selectBox.hide();
                var value = $(this).attr("value");
                var text = $(this).html();
                if (p.render)
                {
                    g.inputText.val(p.render(value, text));
                }
                else
                {
                    g.inputText.val(text);
                }
            });
            g._addClickEven();
        },
        setData: function (data)
        {
            var g = this, p = this.options;
            this.clearContent();
            if (!data || !data.length) return;
            if (g.data != data) g.data = data;
            if (p.columns)
            {
                g.selectBox.table.headrow = $("<tr class='l-table-headerow'><td width='18px'></td></tr>");
                g.selectBox.table.append(g.selectBox.table.headrow);
                g.selectBox.table.addClass("l-box-select-grid");
                for (var j = 0; j < p.columns.length; j++)
                {
                    var headrow = $("<td columnindex='" + j + "' columnname='" + p.columns[j].name + "'>" + p.columns[j].header + "</td>");
                    if (p.columns[j].width)
                    {
                        headrow.width(p.columns[j].width);
                    }
                    g.selectBox.table.headrow.append(headrow);

                }
            }
            for (var i = 0; i < data.length; i++)
            {
                var val = data[i][p.valueField];
                var txt = data[i][p.textField];
                if (!p.columns)
                {
                    $("table.l-table-checkbox", g.selectBox).append("<tr value='" + val + "'><td style='width:18px;'  index='" + i + "' value='" + val + "' text='" + txt + "' ><input type='checkbox' /></td><td index='" + i + "' value='" + val + "' align='left'>" + txt + "</td>");
                    $("table.l-table-nocheckbox", g.selectBox).append("<tr value='" + val + "'><td index='" + i + "' value='" + val + "' align='left'>" + txt + "</td>");
                } else
                {
                    var tr = $("<tr value='" + val + "'><td style='width:18px;'  index='" + i + "' value='" + val + "' text='" + txt + "' ><input type='checkbox' /></td></tr>");
                    $("td", g.selectBox.table.headrow).each(function ()
                    {
                        var columnname = $(this).attr("columnname");
                        if (columnname)
                        {
                            var td = $("<td>" + data[i][columnname] + "</td>");
                            tr.append(td);
                        }
                    });
                    g.selectBox.table.append(tr);
                }
            }
            // Custom checkbox support 
            if (p.isShowCheckBox && $.fn.ligerCheckBox)
            {
                $("table input:checkbox", g.selectBox).ligerCheckBox();
            }
            $(".l-table-checkbox input:checkbox", g.selectBox).change(function ()
            {
                if (this.checked && g.hasBind('beforeSelect'))
                {
                    var parentTD = null;
                    if ($(this).parent().get(0).tagName.toLowerCase() == "div")
                    {
                        parentTD = $(this).parent().parent();
                    } else
                    {
                        parentTD = $(this).parent();
                    }
                    if (parentTD != null && g.trigger('beforeSelect', [parentTD.attr("value"), parentTD.attr("text")]) == false)
                    {
                        g.selectBox.slideToggle("fast");
                        return false;
                    }
                }
                if (!p.isMultiSelect)
                {
                    if (this.checked)
                    {
                        $("input:checked", g.selectBox).not(this).each(function ()
                        {
                            this.checked = false;
                            $(".l-checkbox-checked", $(this).parent()).removeClass("l-checkbox-checked");
                        });
                        g.selectBox.slideToggle("fast");
                    }
                }
                g._checkboxUpdateValue();
            });
            $("table.l-table-nocheckbox td", g.selectBox).hover(function ()
            {
                $(this).addClass("l-over");
            }, function ()
            {
                $(this).removeClass("l-over");
            });
            g._addClickEven();
            // Select the item to initialize 
            g._dataInit();
        },
        //树
        setTree: function (tree)
        {
            var g = this, p = this.options;
            this.clearContent();
            g.selectBox.table.remove();
            if (tree.checkbox != false)
            {
                tree.onCheck = function ()
                {
                    var nodes = g.treeManager.getChecked();
                    var value = [];
                    var text = [];
                    $(nodes).each(function (i, node)
                    {
                        if (p.treeLeafOnly && node.data.children) return;
                        value.push(node.data[p.valueField]);
                        text.push(node.data[p.textField]);
                    });
                    g._changeValue(value.join(p.split), text.join(p.split));
                };
            }
            else
            {
                tree.onSelect = function (node)
                {
                    if (p.treeLeafOnly && node.data.children) return;
                    var value = node.data[p.valueField];
                    var text = node.data[p.textField];
                    g._changeValue(value, text);
                };
                tree.onCancelSelect = function (node)
                {
                    g._changeValue("", "");
                };
            }
            tree.onAfterAppend = function (domnode, nodedata)
            {
                if (!g.treeManager) return;
                var value = null;
                if (p.initValue) value = p.initValue;
                else if (g.valueField.val() != "") value = g.valueField.val();
                g.selectValueByTree(value);
            };
            g.tree = $("<ul></ul>");
            $("div:first", g.selectBox).append(g.tree);
            g.tree.ligerTree(tree);
            g.treeManager = g.tree.ligerGetTreeManager();
        },
        selectValueByTree: function (value)
        {
            var g = this, p = this.options;
            if (value != null)
            {
                var text = "";
                var valuelist = value.toString().split(p.split);
                $(valuelist).each(function (i, item)
                {
                    g.treeManager.selectNode(item.toString());
                    text += g.treeManager.getTextByID(item);
                    if (i < valuelist.length - 1) text += p.split;
                });
                g._changeValue(value, text);
            }
        },
        // Form 
        setGrid: function (grid)
        {
            var g = this, p = this.options;
            this.clearContent();
            g.selectBox.table.remove();
            g.grid = $("div:first", g.selectBox);
            grid.columnWidth = grid.columnWidth || 120;
            grid.width = "100%";
            grid.height = "100%";
            grid.heightDiff = -2;
            grid.InWindow = false;
            g.gridManager = g.grid.ligerGrid(grid);
            p.hideOnLoseFocus = false;
            if (grid.checkbox != false)
            {
                var onCheckRow = function ()
                {
                    var rowsdata = g.gridManager.getCheckedRows();
                    var value = [];
                    var text = [];
                    $(rowsdata).each(function (i, rowdata)
                    {
                        value.push(rowdata[p.valueField]);
                        text.push(rowdata[p.textField]);
                    });
                    g._changeValue(value.join(p.split), text.join(p.split));
                };
                g.gridManager.bind('CheckAllRow', onCheckRow);
                g.gridManager.bind('CheckRow', onCheckRow);
            }
            else
            {
                g.gridManager.bind('SelectRow', function (rowdata, rowobj, index)
                {
                    var value = rowdata[p.valueField];
                    var text = rowdata[p.textField];
                    g._changeValue(value, text);
                });
                g.gridManager.bind('UnSelectRow', function (rowdata, rowobj, index)
                {
                    g._changeValue("", "");
                });
            }
            g.bind('show', function ()
            {
                if (g.gridManager)
                {
                    g.gridManager._updateFrozenWidth();
                }
            });
            g.bind('endResize', function ()
            {
                if (g.gridManager)
                {
                    g.gridManager._updateFrozenWidth();
                    g.gridManager.setHeight(g.selectBox.height() - 2);
                }
            });
        },
        _getValue: function ()
        {
            return $(this.valueField).val();
        },
        getValue: function ()
        {
            // Gets the value of 
            return this._getValue();
        },
        updateStyle: function ()
        {
            var g = this, p = this.options;
            g._dataInit();
        },
        _dataInit: function ()
        {
            var g = this, p = this.options;
            var value = null; 
            if (p.initValue != null && p.initText != null)
            {
                g._changeValue(p.initValue, p.initText);
            }
            // According to the value to initialize 
            if (p.initValue != null)
            {
                value = p.initValue;
                if (p.tree)
                {
                    if(value)
                        g.selectValueByTree(value);
                }
                else
                {
                    var text = g.findTextByValue(value);
                    g._changeValue(value, text);
                }
            }
            // According to the text to initialize  
            else if (p.initText != null)
            {
                value = g.findValueByText(p.initText);
                g._changeValue(value, p.initText);
            }
            else if (g.valueField.val() != "")
            {
                value = g.valueField.val();
                if (p.tree)
                {
                    if(value)
                        g.selectValueByTree(value);
                }
                else
                {
                    var text = g.findTextByValue(value);
                    g._changeValue(value, text);
                }
            }
            if (!p.isShowCheckBox && value != null)
            {
                $("table tr", g.selectBox).find("td:first").each(function ()
                {
                    if (value == $(this).attr("value"))
                    {
                        $(this).addClass("l-selected");
                    }
                });
            }
            if (p.isShowCheckBox && value != null)
            {
                $(":checkbox", g.selectBox).each(function ()
                {
                    var parentTD = null;
                    var checkbox = $(this);
                    if (checkbox.parent().get(0).tagName.toLowerCase() == "div")
                    {
                        parentTD = checkbox.parent().parent();
                    } else
                    {
                        parentTD = checkbox.parent();
                    }
                    if (parentTD == null) return;
                    var valuearr = value.toString().split(p.split);
                    $(valuearr).each(function (i, item)
                    {
                        if (item == parentTD.attr("value"))
                        {
                            $(".l-checkbox", parentTD).addClass("l-checkbox-checked");
                            checkbox[0].checked = true;
                        }
                    });
                });
            }
        },
        // Set the value to   Text boxes and hidden fields 
        _changeValue: function (newValue, newText)
        {
            var g = this, p = this.options;
            g.valueField.val(newValue);
            if (p.render)
            {
                g.inputText.val(p.render(newValue, newText));
            }
            else
            {
                g.inputText.val(newText);
            }
            g.selectedValue = newValue;
            g.selectedText = newText;
            g.inputText.trigger("change").focus();
            g.trigger('selected', [newValue, newText]);
        },
        // Update selected value ( Checkbox )
        _checkboxUpdateValue: function ()
        {
            var g = this, p = this.options;
            var valueStr = "";
            var textStr = "";
            $("input:checked", g.selectBox).each(function ()
            {
                var parentTD = null;
                if ($(this).parent().get(0).tagName.toLowerCase() == "div")
                {
                    parentTD = $(this).parent().parent();
                } else
                {
                    parentTD = $(this).parent();
                }
                if (!parentTD) return;
                valueStr += parentTD.attr("value") + p.split;
                textStr += parentTD.attr("text") + p.split;
            });
            if (valueStr.length > 0) valueStr = valueStr.substr(0, valueStr.length - 1);
            if (textStr.length > 0) textStr = textStr.substr(0, textStr.length - 1);
            g._changeValue(valueStr, textStr);
        },
        _addClickEven: function ()
        {
            var g = this, p = this.options;
            // Click Options 
            $(".l-table-nocheckbox td", g.selectBox).click(function ()
            {
                var value = $(this).attr("value");
                var index = parseInt($(this).attr('index'));
                var text = $(this).html();
                if (g.hasBind('beforeSelect') && g.trigger('beforeSelect', [value, text]) == false)
                {
                    if (p.slide) g.selectBox.slideToggle("fast");
                    else g.selectBox.hide();
                    return false;
                }
                if ($(this).hasClass("l-selected"))
                {
                    if (p.slide) g.selectBox.slideToggle("fast");
                    else g.selectBox.hide();
                    return;
                }
                $(".l-selected", g.selectBox).removeClass("l-selected");
                $(this).addClass("l-selected");
                if (g.select)
                {
                    if (g.select[0].selectedIndex != index)
                    {
                        g.select[0].selectedIndex = index;
                        g.select.trigger("change");
                    }
                }
                if (p.slide)
                {
                    g.boxToggling = true;
                    g.selectBox.hide("fast", function ()
                    {
                        g.boxToggling = false;
                    })
                } else g.selectBox.hide();
                g._changeValue(value, text);
            });
        },
        updateSelectBoxPosition: function ()
        {
            var g = this, p = this.options;
            if (p.absolute)
            {
                g.selectBox.css({ left: g.wrapper.offset().left, top: g.wrapper.offset().top + 1 + g.wrapper.outerHeight() });
            }
            else
            {
                var topheight = g.wrapper.offset().top - $(window).scrollTop();
                var selfheight = g.selectBox.height() + textHeight + 4;
                if (topheight + selfheight > $(window).height() && topheight > selfheight)
                {
                    g.selectBox.css("marginTop", -1 * (g.selectBox.height() + textHeight + 5));
                }
            }
        },
        _toggleSelectBox: function (isHide)
        {
            var g = this, p = this.options;
            var textHeight = g.wrapper.height();
            g.boxToggling = true;
            if (isHide)
            {
                if (p.slide)
                {
                    g.selectBox.slideToggle('fast', function ()
                    {
                        g.boxToggling = false;
                    });
                }
                else
                {
                    g.selectBox.hide();
                    g.boxToggling = false;
                }
            }
            else
            {
                g.updateSelectBoxPosition();
                if (p.slide)
                {
                    g.selectBox.slideToggle('fast', function ()
                    {
                        g.boxToggling = false;
                        if (!p.isShowCheckBox && $('td.l-selected', g.selectBox).length > 0)
                        {
                            var offSet = ($('td.l-selected', g.selectBox).offset().top - g.selectBox.offset().top);
                            $(".l-box-select-inner", g.selectBox).animate({ scrollTop: offSet });
                        }
                    });
                }
                else
                {
                    g.selectBox.show();
                    g.boxToggling = false;
                    if (!g.tree && !g.grid && !p.isShowCheckBox && $('td.l-selected', g.selectBox).length > 0)
                    {
                        var offSet = ($('td.l-selected', g.selectBox).offset().top - g.selectBox.offset().top);
                        $(".l-box-select-inner", g.selectBox).animate({ scrollTop: offSet });
                    }
                }
            }
            g.isShowed = g.selectBox.is(":visible");
            g.trigger('toggle', [isHide]);
            g.trigger(isHide ? 'hide' : 'show');
        }
    });

    $.ligerui.controls.ComboBox.prototype.setValue = $.ligerui.controls.ComboBox.prototype.selectValue;
    // Set the value of the text box and hidden controls 
    $.ligerui.controls.ComboBox.prototype.setInputValue = $.ligerui.controls.ComboBox.prototype._changeValue;
    

})(jQuery);