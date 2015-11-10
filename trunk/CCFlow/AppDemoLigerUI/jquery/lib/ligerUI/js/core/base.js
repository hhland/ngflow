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


    //ligerui  Inherited methods 
    Function.prototype.ligerExtend = function (parent, overrides)
    {
        if (typeof parent != 'function') return this;
        // Save for a reference to the parent class 
        this.base = parent.prototype;
        this.base.constructor = parent;
        // Inherit 
        var f = function () { };
        f.prototype = parent.prototype;
        this.prototype = new f();
        this.prototype.constructor = this;
        // Additional property methods 
        if (overrides) $.extend(this.prototype, overrides);
    };
    // Delay Load 
    Function.prototype.ligerDefer = function (o, defer, args)
    {
        var fn = this;
        return setTimeout(function () { fn.apply(o, args || []); }, defer);
    };

    //  Core Objects 
    window.liger = $.ligerui = {
        version: 'V1.1.9',
        managerCount: 0,
        // Component Manager pool 
        managers: {},
        managerIdPrev: 'ligerui',
        // Error message 
        error: {
            managerIsExist: ' Manager id Already exists '
        },
        getId: function (prev)
        {
            prev = prev || this.managerIdPrev;
            var id = prev + (1000 + this.managerCount);
            this.managerCount++;
            return id;
        },
        add: function (manager)
        {
            if (arguments.length == 2)
            {
                var m = arguments[1];
                m.id = m.id || m.options.id || arguments[0].id;
                this.addManager(m);
                return;
            }
            if (!manager.id) manager.id = this.getId(manager.__idPrev());
            if (this.managers[manager.id])
                throw new Error(this.error.managerIsExist);
            this.managers[manager.id] = manager;
        },
        remove: function (arg)
        {
            if (typeof arg == "string" || typeof arg == "number")
            {
                delete $.ligerui.managers[arg];
            }
            else if (typeof arg == "object" && arg instanceof $.ligerui.core.Component)
            {
                delete $.ligerui.managers[arg.id];
            }
        },
        // Get ligerui Object 
        //1, Afferent ligerui ID
        //2, Afferent Dom Object Array(jQuery)
        get: function (arg, idAttrName)
        {
            idAttrName = idAttrName || "ligeruiid";
            if (typeof arg == "string" || typeof arg == "number")
            {
                return $.ligerui.managers[arg];
            }
            else if (typeof arg == "object" && arg.length)
            {
                if (!arg[0][idAttrName] && !$(arg[0]).attr(idAttrName)) return null;
                return $.ligerui.managers[arg[0][idAttrName] || $(arg[0]).attr(idAttrName)];
            }
            return null;
        },
        // Find one object depending on the type 
        find: function (type)
        {
            var arr = [];
            for (var id in this.managers)
            {
                var manager = this.managers[id];
                if (type instanceof Function)
                {
                    if (manager instanceof type)
                    {
                        arr.push(manager);
                    }
                }
                else if (type instanceof Array)
                {
                    if ($.inArray(manager.__getType(), type) != -1)
                    {
                        arr.push(manager);
                    }
                }
                else
                {
                    if (manager.__getType() == type)
                    {
                        arr.push(manager);
                    }
                }
            }
            return arr;
        },
        //$.fn.liger{Plugin} 和 $.fn.ligerGet{Plugin}Manager
        // This method is called , And pass a scope (this)
        //@parm [plugin]   Plug-in name 
        //@parm [args]  Parameters ( Array )
        //@parm [ext]  Extended Parameters , Namespace definition or id Property name 
        run: function (plugin, args, ext)
        {
            if (!plugin) return;
            ext = $.extend({
                defaultsNamespace: 'ligerDefaults',
                methodsNamespace: 'ligerMethods',
                controlNamespace: 'controls',
                idAttrName: 'ligeruiid',
                isStatic: false,
                hasElement: true,           // Whether it has element Subject ( Such as drag,resizable Other auxiliary plug-in does not have )
                propertyToElemnt: null      // Link to element The property name 
            }, ext || {});
            plugin = plugin.replace(/^ligerGet/, '');
            plugin = plugin.replace(/^liger/, '');
            if (this == null || this == window || ext.isStatic)
            {
                if (!$.ligerui.plugins[plugin])
                {
                    $.ligerui.plugins[plugin] = {
                        fn: $['liger' + plugin],
                        isStatic: true
                    };
                }
                return new $.ligerui[ext.controlNamespace][plugin]($.extend({}, $[ext.defaultsNamespace][plugin] || {}, $[ext.defaultsNamespace][plugin + 'String'] || {}, args.length > 0 ? args[0] : {}));
            }
            if (!$.ligerui.plugins[plugin])
            {
                $.ligerui.plugins[plugin] = {
                    fn: $.fn['liger' + plugin],
                    isStatic: false
                };
            }
            if (/Manager$/.test(plugin)) return $.ligerui.get(this, ext.idAttrName);
            this.each(function ()
            {
                if (this[ext.idAttrName] || $(this).attr(ext.idAttrName))
                {
                    var manager = $.ligerui.get(this[ext.idAttrName] || $(this).attr(ext.idAttrName));
                    if (manager && args.length > 0) manager.set(args[0]);
                    // Has been performed  
                    return;
                }
                if (args.length >= 1 && typeof args[0] == 'string') return;
                // As long as the first argument is not string Type , Perform work component instantiation 
                var options = args.length > 0 ? args[0] : null;
                var p = $.extend({}, $[ext.defaultsNamespace][plugin] || {}
                , $[ext.defaultsNamespace][plugin + 'String'] || {}, options || {});
                if (ext.propertyToElemnt) p[ext.propertyToElemnt] = this;
                if (ext.hasElement)
                {
                    new $.ligerui[ext.controlNamespace][plugin](this, p);
                }
                else
                {
                    new $.ligerui[ext.controlNamespace][plugin](p);
                }
            });
            if (this.length == 0) return null;
            if (args.length == 0) return $.ligerui.get(this, ext.idAttrName);
            if (typeof args[0] == 'object') return $.ligerui.get(this, ext.idAttrName);
            if (typeof args[0] == 'string')
            {
                var manager = $.ligerui.get(this, ext.idAttrName);
                if (manager == null) return;
                if (args[0] == "option")
                {
                    if (args.length == 2)
                        return manager.get(args[1]);  //manager get
                    else if (args.length >= 3)
                        return manager.set(args[1], args[2]);  //manager set
                }
                else
                {
                    var method = args[0];
                    if (!manager[method]) return; // This method does not exist 
                    var parms = Array.apply(null, args);
                    parms.shift();
                    return manager[method].apply(manager, parms);  //manager method
                }
            }
            return null;
        },

        // Expand 
        //1, Default parameters      
        //2, Localization expansion  
        defaults: {},
        //3, Methods Interface Extension 
        methods: {},
        // Namespace 
        // Core Controls , Encapsulates some common methods 
        core: {},
        // Namespace 
        // Collection components 
        controls: {},
        //plugin  A collection of plug-ins 
        plugins: {}
    };


    // Extended object 
    $.ligerDefaults = {};

    // Extended object 
    $.ligerMethos = {};

    // Associate 
    $.ligerui.defaults = $.ligerDefaults;
    $.ligerui.methods = $.ligerMethos;

    // Get ligerui Object 
    //@parm [plugin]   Plug-in name , May be empty 
    $.fn.liger = function (plugin)
    {
        if (plugin)
        {
            return $.ligerui.run.call(this, plugin, arguments);
        }
        else
        {
            return $.ligerui.get(this);
        }
    };


    // Component base class 
    //1, Complete the definition of parameters and parameter attribute initialization processing methods work 
    //2, Complete the definition of event handling methods, and events attribute initialization work 
    $.ligerui.core.Component = function (options)
    {
        // Event container 
        this.events = this.events || {};
        // Configuration parameters 
        this.options = options || {};
        // Subcomponents collection index 
        this.children = {};
    };
    $.extend($.ligerui.core.Component.prototype, {
        __getType: function ()
        {
            return '$.ligerui.core.Component';
        },
        __idPrev: function ()
        {
            return 'ligerui';
        },

        // Setting properties 
        // arg  Property name     value  Property Value  
        // arg  Property /值   value  Is set only event 
        set: function (arg, value)
        {
            if (!arg) return;
            if (typeof arg == 'object')
            {
                var tmp;
                if (this.options != arg)
                {
                    $.extend(this.options, arg);
                    tmp = arg;
                }
                else
                {
                    tmp = $.extend({}, arg);
                }
                if (value == undefined || value == true)
                {
                    for (var p in tmp)
                    {
                        if (p.indexOf('on') == 0)
                            this.set(p, tmp[p]);
                    }
                }
                if (value == undefined || value == false)
                {
                    for (var p in tmp)
                    {
                        if (p.indexOf('on') != 0)
                            this.set(p, tmp[p]);
                    }
                }
                return;
            }
            var name = arg;
            // Event parameters 
            if (name.indexOf('on') == 0)
            {
                if (typeof value == 'function')
                    this.bind(name.substr(2), value);
                return;
            }
            this.trigger('propertychange', arg, value);
            if (!this.options) this.options = {};
            this.options[name] = value;
            var pn = '_set' + name.substr(0, 1).toUpperCase() + name.substr(1);
            if (this[pn])
            {
                this[pn].call(this, value);
            }
            this.trigger('propertychanged', arg, value);
        },

        // Get property 
        get: function (name)
        {
            var pn = '_get' + name.substr(0, 1).toUpperCase() + name.substr(1);
            if (this[pn])
            {
                return this[pn].call(this, name);
            }
            return this.options[name];
        },

        hasBind: function (arg)
        {
            var name = arg.toLowerCase();
            var event = this.events[name];
            if (event && event.length) return true;
            return false;
        },

        // Trigger event 
        //data ( Optional ) Array( Optional ) Additional parameters passed to the event handler 
        trigger: function (arg, data)
        {
            var name = arg.toLowerCase();
            var event = this.events[name];
            if (!event) return;
            data = data || [];
            if ((data instanceof Array) == false)
            {
                data = [data];
            }
            for (var i = 0; i < event.length; i++)
            {
                var ev = event[i];
                if (ev.handler.apply(ev.context, data) == false)
                    return false;
            }
        },

        // Binding events 
        bind: function (arg, handler, context)
        {
            if (typeof arg == 'object')
            {
                for (var p in arg)
                {
                    this.bind(p, arg[p]);
                }
                return;
            }
            if (typeof handler != 'function') return false;
            var name = arg.toLowerCase();
            var event = this.events[name] || [];
            context = context || this;
            event.push({ handler: handler, context: context });
            this.events[name] = event;
        },

        // Unbind 
        unbind: function (arg, handler)
        {
            if (!arg)
            {
                this.events = {};
                return;
            }
            var name = arg.toLowerCase();
            var event = this.events[name];
            if (!event || !event.length) return;
            if (!handler)
            {
                delete this.events[name];
            }
            else
            {
                for (var i = 0, l = event.length; i < l; i++)
                {
                    if (event[i].handler == handler)
                    {
                        event.splice(i, 1);
                        break;
                    }
                }
            }
        },
        destroy: function ()
        {
            $.ligerui.remove(this);
        }
    });


    // Interface component base class , 
    //1, Complete interface initialization : Set Components id And stored in pools Component Manager , Initialization parameters 
    //2, Rendering work , Details to subclass implementation 
    //@parm [element]  Components corresponding dom element Object 
    //@parm [options]  Parameters components 
    $.ligerui.core.UIComponent = function (element, options)
    {
        $.ligerui.core.UIComponent.base.constructor.call(this, options);
        var extendMethods = this._extendMethods();
        if (extendMethods) $.extend(this, extendMethods);
        this.element = element;
        this._init();
        this._preRender();
        this.trigger('render');
        this._render();
        this.trigger('rendered');
        this._rendered();
    };
    $.ligerui.core.UIComponent.ligerExtend($.ligerui.core.Component, {
        __getType: function ()
        {
            return '$.ligerui.core.UIComponent';
        },
        // Extension Methods 
        _extendMethods: function ()
        {

        },
        _init: function ()
        {
            this.type = this.__getType();
            if (!this.element)
            {
                this.id = this.options.id || $.ligerui.getId(this.__idPrev());
            }
            else
            {
                this.id = this.options.id || this.element.id || $.ligerui.getId(this.__idPrev());
            }
            // Deposit manager pool 
            $.ligerui.add(this);

            if (!this.element) return;

            // Read attr Method , And loaded into the parameters , Such as ['url']
            var attributes = this.attr();
            if (attributes && attributes instanceof Array)
            {
                for (var i = 0; i < attributes.length; i++)
                {
                    var name = attributes[i];
                    this.options[name] = $(this.element).attr(name);
                }
            }
            // Read ligerui This property , And loaded into the parameters , Such as  ligerui = "width:120,heigth:100"
            var p = this.options;
            if ($(this.element).attr("ligerui"))
            {
                try
                {
                    var attroptions = $(this.element).attr("ligerui");
                    if (attroptions.indexOf('{') != 0) attroptions = "{" + attroptions + "}";
                    eval("attroptions = " + attroptions + ";");
                    if (attroptions) $.extend(p, attroptions);
                }
                catch (e) { }
            }
        },
        // Pre-rendered , Inheritance can be used to extend 
        _preRender: function ()
        {

        },
        _render: function ()
        {

        },
        _rendered: function ()
        {
            if (this.element)
            {
                $(this.element).attr("ligeruiid", this.id);
            }
        },
        // Returns must be converted into ligerui Attribute parameters , Such as ['url']
        attr: function ()
        {
            return [];
        },
        destroy: function ()
        {
            if (this.element) $(this.element).remove();
            this.options = null;
            $.ligerui.remove(this);
        }
    });


    // Form Control base class 
    $.ligerui.controls.Input = function (element, options)
    {
        $.ligerui.controls.Input.base.constructor.call(this, element, options);
    };

    $.ligerui.controls.Input.ligerExtend($.ligerui.core.UIComponent, {
        __getType: function ()
        {
            return '$.ligerui.controls.Input';
        },
        attr: function ()
        {
            return ['nullText'];
        },
        setValue: function (value)
        {
            return this.set('value', value);
        },
        getValue: function ()
        {
            return this.get('value');
        },
        setEnabled: function ()
        {
            return this.set('disabled', false);
        },
        setDisabled: function ()
        {
            return this.set('disabled', true);
        },
        updateStyle: function ()
        {

        }
    });

    // Global window object 
    $.ligerui.win = {
        // Top display 
        top: false,

        // Mask 
        mask: function (win)
        {
            function setHeight()
            {
                if (!$.ligerui.win.windowMask) return;
                var h = $(window).height() + $(window).scrollTop();
                $.ligerui.win.windowMask.height(h);
            }
            if (!this.windowMask)
            {
                this.windowMask = $("<div class='l-window-mask' style='display: block;'></div>").appendTo('body');
                $(window).bind('resize.ligeruiwin', setHeight);
                $(window).bind('scroll', setHeight);
            }
            this.windowMask.show();
            setHeight();
            this.masking = true;
        },

        // Unmask 
        unmask: function (win)
        {
            var jwins = $("body > .l-dialog:visible,body > .l-window:visible");
            for (var i = 0, l = jwins.length; i < l; i++)
            {
                var winid = jwins.eq(i).attr("ligeruiid");
                if (win && win.id == winid) continue;
                // Get ligerui Object 
                var winmanager = $.ligerui.get(winid);
                if (!winmanager) continue;
                // Whether modal window 
                var modal = winmanager.get('modal');
                // If there are other modal window , It does not cancel the mask 
                if (modal) return;
            }
            if (this.windowMask)
                this.windowMask.hide();
            this.masking = false;
        },

        // Display the taskbar 
        createTaskbar: function ()
        {
            if (!this.taskbar)
            {
                this.taskbar = $('<div class="l-taskbar"><div class="l-taskbar-tasks"></div><div class="l-clear"></div></div>').appendTo('body');
                if (this.top) this.taskbar.addClass("l-taskbar-top");
                this.taskbar.tasks = $(".l-taskbar-tasks:first", this.taskbar);
                this.tasks = {};
            }
            this.taskbar.show();
            this.taskbar.animate({ bottom: 0 });
            return this.taskbar;
        },

        // Close Taskbar 
        removeTaskbar: function ()
        {
            var self = this;
            self.taskbar.animate({ bottom: -32 }, function ()
            {
                self.taskbar.remove();
                self.taskbar = null;
            });
        },
        activeTask: function (win)
        {
            for (var winid in this.tasks)
            {
                var t = this.tasks[winid];
                if (winid == win.id)
                {
                    t.addClass("l-taskbar-task-active");
                }
                else
                {
                    t.removeClass("l-taskbar-task-active");
                }
            }
        },
        // Get task 
        getTask: function (win)
        {
            var self = this;
            if (!self.taskbar) return;
            if (self.tasks[win.id]) return self.tasks[win.id];
            return null;
        },
        // Increase task 
        addTask: function (win)
        {
            var self = this;
            if (!self.taskbar) self.createTaskbar();
            if (self.tasks[win.id]) return self.tasks[win.id];
            var title = win.get('title');
            var task = self.tasks[win.id] = $('<div class="l-taskbar-task"><div class="l-taskbar-task-icon"></div><div class="l-taskbar-task-content">' + title + '</div></div>');
            self.taskbar.tasks.append(task);
            self.activeTask(win);
            task.bind('click', function ()
            {
                self.activeTask(win);
                if (win.actived)
                    win.min();
                else
                    win.active();
            }).hover(function ()
            {
                $(this).addClass("l-taskbar-task-over");
            }, function ()
            {
                $(this).removeClass("l-taskbar-task-over");
            });
            return task;
        },

        hasTask: function ()
        {
            for (var p in this.tasks)
            {
                if (this.tasks[p])
                    return true;
            }
            return false;
        },

        // Remove tasks 
        removeTask: function (win)
        {
            var self = this;
            if (!self.taskbar) return;
            if (self.tasks[win.id])
            {
                self.tasks[win.id].unbind();
                self.tasks[win.id].remove();
                delete self.tasks[win.id];
            }
            if (!self.hasTask())
            {
                self.removeTaskbar();
            }
        },

        // Front display 
        setFront: function (win)
        {
            var wins = $.ligerui.find($.ligerui.core.Win);
            for (var i in wins)
            {
                var w = wins[i];
                if (w == win)
                {
                    $(w.element).css("z-index", "9200");
                    this.activeTask(w);
                }
                else
                {
                    $(w.element).css("z-index", "9100");
                }
            }
        }
    };


    // Window base class  window,dialog
    $.ligerui.core.Win = function (element, options)
    {
        $.ligerui.core.Win.base.constructor.call(this, element, options);
    };

    $.ligerui.core.Win.ligerExtend($.ligerui.core.UIComponent, {
        __getType: function ()
        {
            return '$.ligerui.controls.Win';
        },
        mask: function ()
        {
            if (this.options.modal)
                $.ligerui.win.mask(this);
        },
        unmask: function ()
        {
            if (this.options.modal)
                $.ligerui.win.unmask(this);
        },
        min: function ()
        {
        },
        max: function ()
        {
        },
        active: function ()
        {
        }
    });


    $.ligerui.draggable = {
        dragging: false
    };

    $.ligerui.resizable = {
        reszing: false
    };


    $.ligerui.toJSON = typeof JSON === 'object' && JSON.stringify ? JSON.stringify : function (o)
    {
        var f = function (n)
        {
            return n < 10 ? '0' + n : n;
        },
		escapable = /[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,
		quote = function (value)
		{
		    escapable.lastIndex = 0;
		    return escapable.test(value) ?
				'"' + value.replace(escapable, function (a)
				{
				    var c = meta[a];
				    return typeof c === 'string' ? c :
						'\\u' + ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
				}) + '"' :
				'"' + value + '"';
		};
        if (o === null) return 'null';
        var type = typeof o;
        if (type === 'undefined') return undefined;
        if (type === 'string') return quote(o);
        if (type === 'number' || type === 'boolean') return '' + o;
        if (type === 'object')
        {
            if (typeof o.toJSON === 'function')
            {
                return $.ligerui.toJSON(o.toJSON());
            }
            if (o.constructor === Date)
            {
                return isFinite(this.valueOf()) ?
                   this.getUTCFullYear() + '-' +
                 f(this.getUTCMonth() + 1) + '-' +
                 f(this.getUTCDate()) + 'T' +
                 f(this.getUTCHours()) + ':' +
                 f(this.getUTCMinutes()) + ':' +
                 f(this.getUTCSeconds()) + 'Z' : null;
            }
            var pairs = [];
            if (o.constructor === Array)
            {
                for (var i = 0, l = o.length; i < l; i++)
                {
                    pairs.push($.ligerui.toJSON(o[i]) || 'null');
                }
                return '[' + pairs.join(',') + ']';
            }
            var name, val;
            for (var k in o)
            {
                type = typeof k;
                if (type === 'number')
                {
                    name = '"' + k + '"';
                } else if (type === 'string')
                {
                    name = quote(k);
                } else
                {
                    continue;
                }
                type = typeof o[k];
                if (type === 'function' || type === 'undefined')
                {
                    continue;
                }
                val = $.ligerui.toJSON(o[k]);
                pairs.push(name + ':' + val);
            }
            return '{' + pairs.join(',') + '}';
        }
    };

})(jQuery);