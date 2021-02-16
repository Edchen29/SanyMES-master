
layui.define("jquery", function (exports) {
    var jQuery = layui.jquery,
        $ = layui.jquery;

    //获取url的参数值
    $.getUrlParam = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }

    //把name/value的数组转为obj对象
    $.arrayToObj = function (array) {
        var result = {};
        for (var i = 0; i < array.length; i++) {
            var field = array[i];
            if (field.name in result) {
                result[field.name] += ',' + field.value;
            } else {
                result[field.name] = field.value;
            }
        }
        return result;
    }

    //加载菜单
    $.fn.extend({
        loadMenus: function (modulecode, AreaMenus) {
            var dom = $(this);
            $.ajax("/base/SysModule/LoadAuthorizedMenus",
           {
               async: false
               , type: "post"
               , data: { modulecode: modulecode, AreaMenus: AreaMenus}
               , success: function (data) {
                   if (data == "") { return };
                   var obj = JSON.parse(data);
                   var sb = '';
                   $.each(obj,
                       function () {
                           var element = this;
                           sb += ("<a herf='javascript:;' " + " data-type='" + element.DomId +
                               "' " + " class='layui-btn layui-btn-sm " + element.Class +
                               "' " + ">");

                           if (element.Icon != null && element.Icon != '') {
                               sb += ("<i class='layui-icon'>" + element.Icon + "</i>");
                           }
                           sb += (element.Name + "</a>");
                       });

                   dom.html(dom.html() + sb);
               }
           });
        }
    });

    exports('utils');
});