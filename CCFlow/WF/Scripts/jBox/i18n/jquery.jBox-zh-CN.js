
/* jBox  Global Settings  */
var jBoxConfig = {};

jBoxConfig.defaults = {
    id: null, /*  In the page only id, If it is null Automatically generate a random id, A id Only show a jBox */
    top: '15%', /*  The distance from the top of the window , May be a percentage or pixels (如 '100px') */
    border: 5, /*  The outer border of the window size in pixels , Must be 0 Integer of at least  */
    opacity: 0.1, /*  Transparent window separating layer , If set to 0, Isolation layer is not displayed  */
    timeout: 0, /*  Automatically turn off after the window shows how many milliseconds , If set to 0, Not automatically turn off  */
    showType: 'fade', /*  Type of display window , Optional values are :show,fade,slide */
    showSpeed: 'fast', /*  Speed window display , Optional values are :'slow','fast', Integer milliseconds  */
    showIcon: true, /*  Whether to display the window title icon ,true Show ,false Do not show , Or custom CSS Style class name （ That icon as the background ） */
    showClose: true, /*  Whether to display the top right corner of the window close button  */
    draggable: true, /*  Whether you can drag the window  */
    dragLimit: true, /*  In case you can drag the window , Whether to limit the visible range  */
    dragClone: false, /*  In case you can drag the window , Whether the window when the mouse is pressed clone window  */
    persistent: true, /*  In the case where the spacer layer is displayed , Click isolation layer , Whether to insist window does not close  */
    showScrolling: true, /*  Whether to display the browser scrollbar  */
    ajaxData: {},  /*  Use the window contents get:或post: Prefix case identified ,ajax post Data , Such as :{ id: 1 } 或 "id=1" */
    iframeScrolling: 'auto', /*  Use the window contents iframe: Prefix case identified ,iframe的scrolling Property Value , Optional values are :'auto','yes','no' */

    title: 'jBox', /*  The title of the window  */
    width: 350, /*  Width of the window , Value 'auto' Or represents an integer pixels  */
    height: 'auto', /*  Height of the window , Value 'auto' Or represents an integer pixels  */
    bottomText: '', /*  Button on the left side of the window contents , When there is no button for this setting is invalid  */
    buttons: { ' Determine ': 'ok' }, /*  Button in the window  */
    buttonsFocus: 0, /*  Represents the first of several button is default , Index from 0 Begin  */
    loaded: function (h) { }, /*  Window loading function performed after the completion of , Note that , In the case of ajax或iframe Also have to wait to load completely http Request considered finished loading window , Parameters h Said the window contents jQuery Object  */
    submit: function (v, h, f) { return true; }, /*  Click the window button callback function after , Return true When expressed Close , There are three parameters ,v Button said that the return value of the point ,h Said the window contents jQuery Object ,f Lane said the window contents form Form key  */
    closed: function () { } /*  Functions performed after the window is closed  */
};

jBoxConfig.stateDefaults = {
    content: '', /*  Content states , Does not support prefix designation  */
    buttons: { ' Determine ': 'ok' }, /*  State button  */
    buttonsFocus: 0, /*  Represents the first of several button is default , Index from 0 Begin  */
    submit: function (v, h, f) { return true; } /*  Click the status button callback after , Return true When expressed Close , There are three parameters ,v Button said that the return value of the point ,h Said the window contents jQuery Object ,f Lane said the window contents form Form key  */
};

jBoxConfig.tipDefaults = {
    content: '', /*  Content tips , Does not support prefix designation  */
    icon: 'info', /*  Tip icon , Optional values are 'info','success','warning','error','loading', The default value is 'info', As for 'loading'时,timeout Value will be set to 0, That it would not automatically shut down . */
    top: '40%', /*  Tips from the top distance , May be a percentage or pixels (如 '100px') */
    width: 'auto', /*  Highly suggestive of , Value 'auto' Or represents an integer pixels  */
    height: 'auto', /*  Highly suggestive of , Value 'auto' Or represents an integer pixels  */
    opacity: 0, /*  Transparent window separating layer , If set to 0, Isolation layer is not displayed  */
    timeout: 3000, /*  Prompt is displayed automatically turn off after the number of milliseconds , Must be greater than 0 Integer  */
    closed: function () { } /*  Tip function performed after closing  */
};

jBoxConfig.messagerDefaults = {
    content: '', /*  Content information , Does not support prefix designation  */
    title: 'jBox', /*  Header information  */
    icon: 'none', /*  Information icon , Value 'none' When the icon is not displayed , Optional values are 'none','info','question','success','warning','error' */
    width: 350, /*  Height information , Value 'auto' Or represents an integer pixels  */
    height: 'auto', /*  Height information , Value 'auto' Or represents an integer pixels  */
    timeout: 3000, /*  Information display automatically turns off after how many milliseconds , If set to 0, Not automatically turn off  */
    showType: 'slide', /*  The type of information displayed , Optional values are :show,fade,slide */
    showSpeed: 600, /*  Speed information displayed , Optional values are :'slow','fast', Integer milliseconds  */
    border: 0, /*  Outer border pixel size information , Must be 0 Integer of at least  */
    buttons: {}, /*  Button information  */
    buttonsFocus: 0, /*  Represents the first of several button is default , Index from 0 Begin  */
    loaded: function (h) { }, /*  Window loading function performed after the completion of , Parameters h Said the window contents jQuery Object  */
    submit: function (v, h, f) { return true; }, /*  Click the Info button callback after , Return true When expressed Close , There are three parameters ,v Button said that the return value of the point ,h Said the window contents jQuery Object ,f Lane said the window contents form Form key  */
    closed: function () { } /*  Functions performed after close  */
};

jBoxConfig.languageDefaults = {
    close: ' Shut down ', /*  Top right corner of the window close button prompts  */
    ok: ' Determine ', /* $.jBox.prompt()  Series method [ Determine ] Button text  */
    yes: '是', /* $.jBox.warning()  Methods [是] Button text  */
    no: '否', /* $.jBox.warning()  Methods [否] Button text  */
    cancel: ' Cancel ' /* $.jBox.confirm() 和 $.jBox.warning()  Methods [ Cancel ] Button text  */
};

$.jBox.setDefaults(jBoxConfig);
