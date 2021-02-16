var sbList = {};

function loadMenus(modulecode, AreaMenus) {
    var sb = '';
    if (sbList[AreaMenus] != undefined) { return sbList[AreaMenus]; }
    layui.jquery.ajax("/base/SysModule/LoadAuthorizedMenus?modulecode=" + modulecode + "&AreaMenus=" + AreaMenus,
        {
            async: false
            , success: function (data) {
                if (data == "") { return };
                var obj = JSON.parse(data);
                layui.jquery.each(obj,
                    function () {
                        var element = this;
                        sb += ("<a herf='javascript:;' " + " lay-event='" + element.DomId +
                            "' " + " class='layui-btn layui-btn-sm " + element.Class +
                            "' " + ">");

                        if (element.Icon != null && element.Icon != '') {
                            sb += ("<i class='layui-icon'>" + element.Icon + "</i>");
                        }
                        sb += (element.Name + "</a>");
                    });
                sbList[AreaMenus] = sb;
            }
        });
    loadMenusFlag = true;
    return sb;
}

Array.prototype.indexOf = function (val) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == val) return i;
    }
    return -1;
};

Array.prototype.remove = function (val) {
    var index = this.indexOf(val);
    if (index > -1) {
        this.splice(index, 1);
    }
};

