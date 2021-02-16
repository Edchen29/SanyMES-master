/**
 * 通用工具库
 */
var firstText;
layui.define(['form', 'element', 'vue', 'layer', 'laydate', 'jquery', 'table'], function (exports) {
    var form = layui.form,
        layer = layui.layer,
        element = layui.element,
        laydate = layui.laydate,
        $ = layui.jquery,
        table = layui.table;

    //字符串常量
    var MOD_NAME = 'hhweb',
        THIS = 'layui-this',
        SHOW = 'layui-show',
        HIDE = 'layui-hide',
        DISABLED = 'layui-disabled';

    //外部接口
    var hhweb = {
        config: {} //全局配置项

        //设置全局项
        , set: function (options) {
            var that = this;
            that.config = $.extend({}, that.config, options);
            return that;
        }
        //事件监听
        , on: function (events, callback) {
            return layui.onevent.call(this, MOD_NAME, events, callback);
        }

        //删除
        , del: function (url, dataids, callback) {
            if (dataids == undefined || dataids == "" || dataids.length == 0) {
                layer.alert("至少选择一条记录", { icon: 5, shadeClose: true, title: "错误信息" });
                return;
            }
            layer.confirm('真的删除么', function (index) {
                $.post(url, { ids: dataids },
                    function (data) {
                        if (data.Code == 200) {
                            if (callback != undefined) callback();
                        } else {
                            layer.msg(data.Message);
                        }
                    }, "json");
                layer.close(index);
            });
        }

        , ValidTextVal(obj, type, msg) {
            if (type == "NumText") {
                var oldlength = $(obj).val().length;
                $(obj).val($(obj).val().replace(/\D/g, ''));
                if ($(obj).val().length != oldlength) {
                    layer.msg(msg)
                }
            }
            if (type == "NumDecText") {
                var oldlength = $(obj).val().length;
                $(obj).val($(obj).val().replace(/[^0-9.]/g, ''));
                if ($(obj).val().length != oldlength) {
                    layer.msg(msg)
                }
            }
        }

        , DomEnable(dom) {
            $(dom).removeAttr("disabled");
            $(dom).css("background-color", "white");
        }

        , DomDisable(dom) {
           
        }
        , DomDisableImportant(dom) {
            $(dom).attr({ disabled: "disabled" });
            $(dom).css("background-color", "#eee");
        }

        , CheckRequired(filterId, func) {
            var filter = ".required";
            if (func != undefined) {
                filter = filter + func;
            }

            if (filterId != undefined) {
                filter = filterId + " " + filter;
            }
            var rtn = null;
            var required = $(filter);
            required.each(function () {
                if ($(this).parent().parent().css("display") == "block") {
                    if ($(this).val() == null || $(this).val().replace(/^\s+|\s+$/g, "") == "") {
                        rtn = $(this);
                        return false;
                    }
                }
            });
            return rtn;
        }

        , blink(selector) {
            $(selector).fadeOut('normal', function () {
                $(this).fadeIn('normal', function () {
                    blink(this);
                });
            });
        }

        , ColumnSetting(tablelid, cols_arr) {
            var account = window.localStorage.getItem("Account");
            var paths = window.location.pathname.split("/");
            var controller = paths[paths.length - 2];
            var cookieName = account + "_" + controller + "_" + tablelid;
            var cookieval = window.localStorage.getItem(cookieName);
            var colsetting = {};
            if (cookieval !== undefined) {
                colsetting = JSON.parse(cookieval);
            }

            cols_arr[0].forEach(function (item, len) {
                if (colsetting != null) {
                    if (colsetting[item.field] !== undefined) {
                        item.hide = !(colsetting[item.field]);
                    }
                }
            });

            return cols_arr;
        }
    }

    $(document).ready(function () {
        $('.layui-date').each(function () {
            laydate.render({
                elem: this
                , btns: ['confirm']
                , type: "datetime"
                , done: function (value, date, endDate) {
                    try {
                        var Id = this.elem[0].attributes["name"].nodeValue;
                        var Expression = this.elem[0]['__v_model'].expression;

                        for (var item in hhweb.Config) {
                            var vueobj = item;
                            var vueclass = hhweb.Config[item];

                            if (vueobj === Id) {
                                vueclass[Expression] = value;
                            }
                        }
                    } catch (e) {

                    }
                }
            });
        });

        $(".NumText").keyup(function () {
            hhweb.ValidTextVal(this, "NumText", "请输入整数");
        }).bind("paste", function () {  //CTR+V事件处理    
            hhweb.ValidTextVal(this, "NumText", "请输入整数");
        }).css("ime-mode", "disabled"); //CSS设置输入法不可用 

        $(".NumDecText").keyup(function () {
            hhweb.ValidTextVal(this, "NumDecText", "请输入数值");
        }).bind("paste", function () {  //CTR+V事件处理    
            hhweb.ValidTextVal(this, "NumDecText", "请输入数值");
        }).css("ime-mode", "disabled"); //CSS设置输入法不可用    

        $(document).on('keydown', function (e) {
            //禁用页面F5刷新
            if (e.which == 116) {
                e.preventDefault(); //Skip default behavior of the enter key
            }

            //CTRL+ENTER 提交
            if (e.ctrlKey && e.keyCode == 13) {
                if ($(".layui-layer-btn0").length > 0) {
                    $(".layui-layer-btn0").click();
                }
            }
            //ESC 取消
            if (e.keyCode == 27) {
                if ($(".layui-layer-close1").length > 0) {
                    $(".layui-layer-close1").click();
                }
            }
        })

        //单击行勾选checkbox事件
        $(document).on("click", ".layui-table-body table.layui-table tbody tr", function () {
            var index = $(this).attr('data-index');
            var tableBox = $(this).parents('.layui-table-box');
            //存在固定列
            if (tableBox.find(".layui-table-fixed.layui-table-fixed-l").length > 0) {
                tableDiv = tableBox.find(".layui-table-fixed.layui-table-fixed-l");
            } else {
                tableDiv = tableBox.find(".layui-table-body.layui-table-main");
            }
            var checkCell = tableDiv.find("tr[data-index=" + index + "]").find("td div.laytable-cell-checkbox div.layui-form-checkbox I");
            if (checkCell.length > 0) {
                checkCell.click();
            }
        });

        $(document).on("click", "td div.laytable-cell-checkbox div.layui-form-checkbox", function (e) {
            e.stopPropagation();
        });

        form.on('checkbox()', function (data) {
            try {
                var attributes = data.elem.attributes;
                if (attributes['lay-filter'] !== undefined) {
                    if (attributes['lay-filter'].nodeValue == 'LAY_TABLE_TOOL_COLS') {
                        var tb = data.elem.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode;
                        var account = window.localStorage.getItem("Account");
                        var paths = window.location.pathname.split("/");
                        var controller = paths[paths.length - 2];
                        var tablelid = tb.previousElementSibling.id;
                        var checkflag = data.elem.checked;
                        var colname = attributes['name'].nodeValue;
                        var cookieName = account + "_" + controller + "_" + tablelid;
                        var cookieval = window.localStorage.getItem(cookieName);
                        var colsetting = {};
                        if (cookieval == undefined) {
                            colsetting[colname] = checkflag;
                        }
                        else {
                            colsetting = JSON.parse(cookieval);
                            colsetting[colname] = checkflag;
                        }
                        window.localStorage.setItem(cookieName, JSON.stringify(colsetting));
                        console.log("cookieName:" + cookieName + "=" + JSON.stringify(colsetting));
                    }
                }
            } catch (e) {
            }
        }); 

        //所有文本框绑定事件
        var input = $('input:text:not(:disabled)');
        input.each(function () {
            if (firstText == undefined) {
                firstText = $(this);
            }
            $(this).bind("keydown", function (e) {
                var n = input.length;
                if (e.which == 13) {
                    e.preventDefault(); //Skip default behavior of the enter key
                    if ($(this).val() != "") {
                        var nextIndex = input.index(this) + 1;
                        if (nextIndex < n) {
                            input[nextIndex].focus();
                        }
                        else {
                            input[nextIndex - 1].blur();
                        }
                    }
                    else {
                        $(this).focus();
                    }
                }
            });
            //绑定获取焦点事件
            $(this).bind('focus', function (event) {
                $(this).css("background-color", "yellow");
                //$(this).val("");
            });
            //绑定失去焦点事件
            $(this).bind('blur', function (event) {
                $(this).css("background-color", "white");
            });
        });

        $(firstText).focus();
    });

    exports(MOD_NAME, hhweb);
});