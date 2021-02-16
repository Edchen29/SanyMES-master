layui.config({
    base: "/js/"
}).use(['form', 'element', 'vue', 'layer', 'laydate', 'jquery', 'table', 'hhweb', 'utils', 'Universal', 'easyui', 'cron'], function () {
    var form = layui.form,
        layer = layui.layer,
        element = layui.element,
        laydate = layui.laydate,
        $ = layui.jquery,
        table = layui.table,
        hhweb = layui.hhweb,
        Universal = layui.Universal;

    var AreaName = 'job';
    var TableName = 'SysJob';

    var vm = new Vue({
        el: '#modifyForm'
    });

    var vmq = new Vue({
        el: '#panelSearch',
        data: {
        }
    });
    
    hhweb.Config = {
        'LastFireTime': vm,
        'NextFireTime': vm,
        'CreateTime': vm,
        'UpdateTime': vm,

        'qLastFireTime': vmq,
        'qNextFireTime': vmq,
        'qCreateTime': vmq,
        'qUpdateTime': vmq,
    };
      
    var mainList = {
        Render: function () {
            var cols_arr = [[
                { checkbox: true, fixed: true }
                , {field:'Id', width:80, sort: true, fixed: false, hide: false, title: 'Id' }
                , {field:'JobName', width:150, sort: true, fixed: false, hide: false, title: '任务名称' }
                , { field: 'JobGroup', width: 150, sort: true, fixed: false, hide: true, title: '任务组名' }
                , { field: 'MethodName', width: 150, sort: true, fixed: false, hide: false, title: '任务方法', templet: function (d) { return GetLabel('MethodName', 'DictValue', 'DictLabel', d.MethodName) } }
                , {field:'MethodParams', width:150, sort: true, fixed: false, hide: false, title: '参数' }
                , {field:'CronExpression', width:150, sort: true, fixed: false, hide: false, title: 'cron执行表达式' }
                , {field:'LastFireTime', width:150, sort: true, fixed: false, hide: false, title: 'LastFireTime' }
                , {field:'NextFireTime', width:150, sort: true, fixed: false, hide: false, title: 'NextFireTime' }
                , { field: 'MisfirePolicy', width: 150, sort: true, fixed: false, hide: true, title: '错误策略' }
                , { field: 'Status', width: 150, sort: true, fixed: false, hide: false, title: '状态', templet: function (d) { return GetLabel('Status', 'DictValue', 'DictLabel', d.Status) } }
                , { field: 'Remark', width: 150, sort: true, fixed: false, hide: true, title: '备注信息' }
                , {field:'CreateTime', width:150, sort: true, fixed: false, hide: false, title: '创建时间' }
                , {field:'CreateBy', width:150, sort: true, fixed: false, hide: false, title: '创建用户' }
                , {field:'UpdateTime', width:150, sort: true, fixed: false, hide: false, title: '更新时间' }
                , {field:'UpdateBy', width:150, sort: true, fixed: false, hide: false, title: '更新用户' }
            ]];

            mainList.Table = table.render({
                elem: '#mainList'
                , url: "/" + AreaName + "/" + TableName + "/Load"
                , method: "post"
                , page: true //开启分页
                , cols: hhweb.ColumnSetting('mainList', cols_arr)
                , id: 'mainList'
                , limit: 20
                , limits: [20, 50, 100, 200, 500, 1000]
                , defaultToolbar: ['filter']
                , toolbar: '#toolbarTable'
                , height: 'full-1'
                , cellMinWidth: 80
                , size: 'sm'
                , done: function (res) { }
            });

            return mainList.Table;
        },
        Load: function () {
            if (mainList.Table == undefined) {
                mainList.Table = this.Render();
                return;
            }
            table.reload('mainList', {});
        }
    };

    //编辑
    var EditInfo = function (tabledata) {
        data = tabledata;
        vm.$set('$data', data);
        //表单修改时填充需修改的数据
        var list = {};
        $('.ClearSelector_' + TableName).each(function () {
            var selDom = ($(this));
            if (!$(selDom)[0].name.startsWith("q")) {
                list[$(selDom)[0].name] = data[$(selDom)[0].name] + "";
            }
        });
        //表单修改时填充需修改的数据
        form.val('modifyForm', list);
    };

    var selfbtn = {
        //自定义按钮
        PauseOrResume: function () {
            var checkStatus = table.checkStatus('mainList');
            var count = checkStatus.data.length;
            if (count == 1) {
                $.ajax({
                    url: "/job/SysJob/PauseOrResume",
                    type: "POST",
                    data: { Table_entity: checkStatus.data[0] },
                    dataType: "json",
                    success: function (result) {
                        if (result.Status) {
                            if (checkStatus.data[0].Status == "0") {
                                layer.msg('计划已暂停', { icon: 6, shade: 0.4, time: 1000 });
                            }
                            else {
                                layer.msg('计划已启用', { icon: 6, shade: 0.4, time: 1000 });
                            }

                            mainList.Load();//重载TABLE
                        } else {
                            layer.alert("操作失败" + result.Message, { icon: 5, shadeClose: true, title: "错误信息" });
                        }
                    }
                });
            }
            else
                layer.alert("请选择一条操作数据！", { icon: 5, shadeClose: true, title: "错误信息" });
        },
        DomConfig: function (AddOrUpdate) {
            if (AddOrUpdate) {
                hhweb.DomEnable($("#modifyForm [name='JobName']"));
                hhweb.DomEnable($("#modifyForm [name='JobGroup']"));
            }
            else {
                hhweb.DomDisable($("#modifyForm [name='JobName']"));
                hhweb.DomDisable($("#modifyForm [name='JobGroup']"));
            }
        },
        SaveBefore: function (AddOrEditOrDelete) {
            if (AddOrEditOrDelete in { Add: null, Edit: null }) {
                var rtn = hhweb.CheckRequired("#modifyForm", AddOrEditOrDelete);
                return rtn;
            }
        }
    };

    var selector = {
        'MethodName': {
            SelType: "FromDict",
            SelFrom: "jobtype",
            SelModel: "MethodName",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='MethodName']"), $("[name='qMethodName']")]
        },
        'Status': {
            SelType: "FromDict",
            SelFrom: "sys_job_status",
            SelModel: "Status",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='Status']"), $("[name='qStatus']")]
        },
    };

    var vml = new Array({
        vm: vm,
        vmq: vmq,
    });
    
    Universal.BindSelector(vml, selector);
    Universal.mmain(AreaName, TableName, vm, vmq, EditInfo, selfbtn, mainList);

    $('#MethodParams').on('click', function () {
        layer.open({
            title: "参数",
            area: ["500px", "300px"],
            type: 1,
            content: $('#DivConfigMethodParams'),
            btn: ['保存', '关闭'],
            success: function (layero, index) {
                var DictKey = vm.$data["MethodName"] + 'Para';
                LoadMethodParams(DictKey);
            },
            yes: function (index) {
                vm.$data["MethodParams"] = GetMethodParams();
                layer.close(index);
            },
            btn2: function (index) {
                layer.close(index);
            }
        });
    });

    $('.CronExpression').on('click', function () {
        $("#cron").val(vm.$data["CronExpression"]);
        btnFan();

        layer.open({
            title: "设置Cron表达式",
            area: ["900px", "550px"],
            type: 1,
            content: $('#CornSetting'),
            btn: ['保存', '关闭'],
            success: function (layero, index) {
                $("#CornSetting").show();
            },
            yes: function (index) {
                vm.$data["CronExpression"] = $("#cron").val();
                layer.close(index);
            },
            btn2: function (index) {
                layer.close(index);
            },
            end: function (layero, index) {
                $("#CornSetting").hide();
            }
        });
    });

    function LoadMethodParams(DictKey) {
        var oldobj = {};
        if (vm.$data["MethodParams"] != undefined && vm.$data["MethodParams"] != null && vm.$data["MethodParams"] != "") {
            oldobj = JSON.parse(vm.$data["MethodParams"]);
        }

        $("#ConfigMethodParams").html("");
        $.ajax({
            async: true,
            type: "post",
            data: { DictType: DictKey },
            url: "/base/SysDictData/FindSysDictData",
            dataType: "json",
            success: function (result) {
                if (result.count > 0) {
                    var dom = '';
                    for (var i = 0; i < result.count; i++) {
                        var name = result.data[i]["DictLabel"];
                        var value = result.data[i]["DictValue"];
                        var oldvalue = '';
                        if (oldobj[value] !== undefined) {
                            oldvalue = oldobj[value];
                        }
                        dom += '<div class="layui-col-sm12">';
                        dom += '<label class="layui-form-label layui-col-sm3">' + name + '</label>';
                        dom += '<div class="layui-input-inline layui-col-sm9">';
                        dom += '<input data-model="' + value + '" class="layui-input MethodParamsKey" type="text" autocomplete="off" lay-verify="" value="' + oldvalue + '">'
                        dom += '</div></div>';
                    }
                    $("#ConfigMethodParams").html(dom);
                    form.render();
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layer.alert(errorThrown, { icon: 2, title: '提示' });
            }
        });
    }

    function GetMethodParams() {
        var MethodParams = {};
        $.each($(".MethodParamsKey"), function (key, value) {
            MethodParams[$(this).data("model")] = $(this).val();
        });
        return JSON.stringify(MethodParams);
    }

    /*killIe*/
    $.parser.parse($("body"));
    var cpro_id = "u1331261";

    $(function () {
        $('.startOn').on('click', function () {
            startOn(this);
        });

        $('.everyTime').on('click', function () {
            everyTime(this);
        });

        $('.unAppoint').on('click', function () {
            unAppoint(this);
        });

        $('.weekOfDay').on('click', function () {
            weekOfDay(this);
        });

        $('.lastWeek').on('click', function () {
            lastWeek(this);
        });

        $('.cycle').on('click', function () {
            cycle(this);
        });

        $('.workDay').on('click', function () {
            workDay(this);
        });

        $('.lastDay').on('click', function () {
            lastDay(this);
        });

        $('#btnFan').on('click', function () {
            btnFan();
        });
    });

    function btnFan() {
        //获取参数中表达式的值
        var txt = $("#cron").val();
        if (txt) {
            var regs = txt.split(' ');
            $("input[name=v_second]").val(regs[0]);
            $("input[name=v_min]").val(regs[1]);
            $("input[name=v_hour]").val(regs[2]);
            $("input[name=v_day]").val(regs[3]);
            $("input[name=v_mouth]").val(regs[4]);
            $("input[name=v_week]").val(regs[5]);

            initObj(regs[0], "second");
            initObj(regs[1], "min");
            initObj(regs[2], "hour");
            initDay(regs[3]);
            initMonth(regs[4]);
            initWeek(regs[5]);

            if (regs.length > 6) {
                $("input[name=v_year]").val(regs[6]);
                initYear(regs[6]);
            }
        }
    }

    function initObj(strVal, strid) {
        var ary = null;
        var objRadio = $("input[name='" + strid + "'");
        if (strVal == "*") {
            objRadio.eq(0).attr("checked", "checked");
        } else if (strVal.split('-').length > 1) {
            ary = strVal.split('-');
            objRadio.eq(1).attr("checked", "checked");
            $("#" + strid + "Start_0").numberspinner('setValue', ary[0]);
            $("#" + strid + "End_0").numberspinner('setValue', ary[1]);
        } else if (strVal.split('/').length > 1) {
            ary = strVal.split('/');
            objRadio.eq(2).attr("checked", "checked");
            $("#" + strid + "Start_1").numberspinner('setValue', ary[0]);
            $("#" + strid + "End_1").numberspinner('setValue', ary[1]);
        } else {
            objRadio.eq(3).attr("checked", "checked");
            if (strVal != "?") {
                ary = strVal.split(",");
                for (var i = 0; i < ary.length; i++) {
                    $("." + strid + "List input[value='" + ary[i] + "']").attr("checked", "checked");
                }
            }
        }
    }

    function initDay(strVal) {
        var ary = null;
        var objRadio = $("input[name='day'");
        if (strVal == "*") {
            objRadio.eq(0).attr("checked", "checked");
        } else if (strVal == "?") {
            objRadio.eq(1).attr("checked", "checked");
        } else if (strVal.split('-').length > 1) {
            ary = strVal.split('-');
            objRadio.eq(2).attr("checked", "checked");
            $("#dayStart_0").numberspinner('setValue', ary[0]);
            $("#dayEnd_0").numberspinner('setValue', ary[1]);
        } else if (strVal.split('/').length > 1) {
            ary = strVal.split('/');
            objRadio.eq(3).attr("checked", "checked");
            $("#dayStart_1").numberspinner('setValue', ary[0]);
            $("#dayEnd_1").numberspinner('setValue', ary[1]);
        } else if (strVal.split('W').length > 1) {
            ary = strVal.split('W');
            objRadio.eq(4).attr("checked", "checked");
            $("#dayStart_2").numberspinner('setValue', ary[0]);
        } else if (strVal == "L") {
            objRadio.eq(5).attr("checked", "checked");
        } else {
            objRadio.eq(6).attr("checked", "checked");
            ary = strVal.split(",");
            for (var i = 0; i < ary.length; i++) {
                $(".dayList input[value='" + ary[i] + "']").attr("checked", "checked");
            }
        }
    }

    function initMonth(strVal) {
        var ary = null;
        var objRadio = $("input[name='mouth'");
        if (strVal == "*") {
            objRadio.eq(0).attr("checked", "checked");
        } else if (strVal == "?") {
            objRadio.eq(1).attr("checked", "checked");
        } else if (strVal.split('-').length > 1) {
            ary = strVal.split('-');
            objRadio.eq(2).attr("checked", "checked");
            $("#mouthStart_0").numberspinner('setValue', ary[0]);
            $("#mouthEnd_0").numberspinner('setValue', ary[1]);
        } else if (strVal.split('/').length > 1) {
            ary = strVal.split('/');
            objRadio.eq(3).attr("checked", "checked");
            $("#mouthStart_1").numberspinner('setValue', ary[0]);
            $("#mouthEnd_1").numberspinner('setValue', ary[1]);

        } else {
            objRadio.eq(4).attr("checked", "checked");

            ary = strVal.split(",");
            for (var i = 0; i < ary.length; i++) {
                $(".mouthList input[value='" + ary[i] + "']").attr("checked", "checked");
            }
        }
    }

    function initWeek(strVal) {
        var ary = null;
        var objRadio = $("input[name='week'");
        if (strVal == "*") {
            objRadio.eq(0).attr("checked", "checked");
        } else if (strVal == "?") {
            objRadio.eq(1).attr("checked", "checked");
        } else if (strVal.split('/').length > 1) {
            ary = strVal.split('/');
            objRadio.eq(2).attr("checked", "checked");
            $("#weekStart_0").numberspinner('setValue', ary[0]);
            $("#weekEnd_0").numberspinner('setValue', ary[1]);
        } else if (strVal.split('-').length > 1) {
            ary = strVal.split('-');
            objRadio.eq(3).attr("checked", "checked");
            $("#weekStart_1").numberspinner('setValue', ary[0]);
            $("#weekEnd_1").numberspinner('setValue', ary[1]);
        } else if (strVal.split('L').length > 1) {
            ary = strVal.split('L');
            objRadio.eq(4).attr("checked", "checked");
            $("#weekStart_2").numberspinner('setValue', ary[0]);
        } else {
            objRadio.eq(5).attr("checked", "checked");
            ary = strVal.split(",");
            for (var i = 0; i < ary.length; i++) {
                $(".weekList input[value='" + ary[i] + "']").attr("checked", "checked");
            }
        }
    }

    function initYear(strVal) {
        var ary = null;
        var objRadio = $("input[name='year'");
        if (strVal == "*") {
            objRadio.eq(1).attr("checked", "checked");
        } else if (strVal.split('-').length > 1) {
            ary = strVal.split('-');
            objRadio.eq(2).attr("checked", "checked");
            $("#yearStart_0").numberspinner('setValue', ary[0]);
            $("#yearEnd_0").numberspinner('setValue', ary[1]);
        }
    }

    /**
    * 每周期
    */
    function everyTime(dom) {
        var item = $("input[name=v_" + dom.name + "]");
        item.val("*");
        item.change();
    }

    /**
     * 不指定
     */
    function unAppoint(dom) {
        var name = dom.name;
        var val = "?";
        if (name == "year")
            val = "";
        var item = $("input[name=v_" + name + "]");
        item.val(val);
        item.change();
    }

    function appoint(dom) {

    }

    /**
     * 周期
     */
    function cycle(dom) {
        var name = dom.name;
        var ns = $(dom).parent().find(".numberspinner");
        var start = ns.eq(0).numberspinner("getValue");
        var end = ns.eq(1).numberspinner("getValue");
        var item = $("input[name=v_" + name + "]");
        item.val(start + "-" + end);
        item.change();
    }

    /**
     * 从开始
     */
    function startOn(dom) {
        var name = dom.name;
        var ns = $(dom).parent().find(".numberspinner");
        var start = ns.eq(0).numberspinner("getValue");
        var end = ns.eq(1).numberspinner("getValue");
        var item = $("input[name=v_" + name + "]");
        item.val(start + "/" + end);
        item.change();
    }

    function lastDay(dom) {
        var item = $("input[name=v_" + dom.name + "]");
        item.val("L");
        item.change();
    }

    function weekOfDay(dom) {
        var name = dom.name;
        var ns = $(dom).parent().find(".numberspinner");
        var start = ns.eq(0).numberspinner("getValue");
        var end = ns.eq(1).numberspinner("getValue");
        var item = $("input[name=v_" + name + "]");
        item.val(start + "#" + end);
        item.change();
    }

    function lastWeek(dom) {
        var item = $("input[name=v_" + dom.name + "]");
        var ns = $(dom).parent().find(".numberspinner");
        var start = ns.eq(0).numberspinner("getValue");
        item.val(start + "L");
        item.change();
    }

    function workDay(dom) {
        var name = dom.name;
        var ns = $(dom).parent().find(".numberspinner");
        var start = ns.eq(0).numberspinner("getValue");
        var item = $("input[name=v_" + name + "]");
        item.val(start + "W");
        item.change();
    }

    $(function () {
        $("#CornSetting").hide();
        $("#CornCover").hide();

        var vals = $("input[name^='v_']");
        var cron = $("#cron");
        vals.change(function () {
            var item = [];
            vals.each(function () {
                item.push(this.value);
            });
            //修复表达式错误BUG，如果后一项不为* 那么前一项肯定不为为*，要不然就成了每秒执行了
            //获取当前选中tab
            var currentIndex = 0;
            $(".tabs>li").each(function (i, item) {
                if ($(item).hasClass("tabs-selected")) {
                    currentIndex = i;
                    return false;
                }

            });
            //当前选中项之前的如果为*，则都设置成0
            for (var i = currentIndex; i >= 1; i--) {
                if (item[i] != "*" && item[i - 1] == "*") {
                    item[i - 1] = "0";
                }
            }
            //当前选中项之后的如果不为*则都设置成*
            if (item[currentIndex] == "*") {
                for (var i = currentIndex + 1; i < item.length; i++) {
                    if (i == 5) {
                        item[i] = "?";
                    } else {
                        item[i] = "*";
                    }
                }
            }
            cron.val(item.join(" ")).change();
        });

        cron.change(function () {
            btnFan();
        });

        var secondList = $(".secondList").children();
        $("#sencond_appoint").click(function () {
            if (this.checked) {
                if ($(secondList).filter(":checked").length == 0) {
                    $(secondList.eq(0)).attr("checked", true);
                }
                secondList.eq(0).change();
            }
        });

        secondList.change(function () {
            var sencond_appoint = $("#sencond_appoint").prop("checked");
            if (sencond_appoint) {
                var vals = [];
                secondList.each(function () {
                    if (this.checked) {
                        vals.push(this.value);
                    }
                });
                var val = "?";
                if (vals.length > 0 && vals.length < 59) {
                    val = vals.join(",");
                } else if (vals.length == 59) {
                    val = "*";
                }
                var item = $("input[name=v_second]");
                item.val(val);
                item.change();
            }
        });

        var minList = $(".minList").children();
        $("#min_appoint").click(function () {
            if (this.checked) {
                if ($(minList).filter(":checked").length == 0) {
                    $(minList.eq(0)).attr("checked", true);
                }
                minList.eq(0).change();
            }
        });

        minList.change(function () {
            var min_appoint = $("#min_appoint").prop("checked");
            if (min_appoint) {
                var vals = [];
                minList.each(function () {
                    if (this.checked) {
                        vals.push(this.value);
                    }
                });
                var val = "?";
                if (vals.length > 0 && vals.length < 59) {
                    val = vals.join(",");
                } else if (vals.length == 59) {
                    val = "*";
                }
                var item = $("input[name=v_min]");
                item.val(val);
                item.change();
            }
        });

        var hourList = $(".hourList").children();
        $("#hour_appoint").click(function () {
            if (this.checked) {
                if ($(hourList).filter(":checked").length == 0) {
                    $(hourList.eq(0)).attr("checked", true);
                }
                hourList.eq(0).change();
            }
        });

        hourList.change(function () {
            var hour_appoint = $("#hour_appoint").prop("checked");
            if (hour_appoint) {
                var vals = [];
                hourList.each(function () {
                    if (this.checked) {
                        vals.push(this.value);
                    }
                });
                var val = "?";
                if (vals.length > 0 && vals.length < 24) {
                    val = vals.join(",");
                } else if (vals.length == 24) {
                    val = "*";
                }
                var item = $("input[name=v_hour]");
                item.val(val);
                item.change();
            }
        });

        var dayList = $(".dayList").children();
        $("#day_appoint").click(function () {
            if (this.checked) {
                if ($(dayList).filter(":checked").length == 0) {
                    $(dayList.eq(0)).attr("checked", true);
                }
                dayList.eq(0).change();
            }
        });

        dayList.change(function () {
            var day_appoint = $("#day_appoint").prop("checked");
            if (day_appoint) {
                var vals = [];
                dayList.each(function () {
                    if (this.checked) {
                        vals.push(this.value);
                    }
                });
                var val = "?";
                if (vals.length > 0 && vals.length < 31) {
                    val = vals.join(",");
                } else if (vals.length == 31) {
                    val = "*";
                }
                var item = $("input[name=v_day]");
                item.val(val);
                item.change();
            }
        });

        var mouthList = $(".mouthList").children();
        $("#mouth_appoint").click(function () {
            if (this.checked) {
                if ($(mouthList).filter(":checked").length == 0) {
                    $(mouthList.eq(0)).attr("checked", true);
                }
                mouthList.eq(0).change();
            }
        });

        mouthList.change(function () {
            var mouth_appoint = $("#mouth_appoint").prop("checked");
            if (mouth_appoint) {
                var vals = [];
                mouthList.each(function () {
                    if (this.checked) {
                        vals.push(this.value);
                    }
                });
                var val = "?";
                if (vals.length > 0 && vals.length < 12) {
                    val = vals.join(",");
                } else if (vals.length == 12) {
                    val = "*";
                }
                var item = $("input[name=v_mouth]");
                item.val(val);
                item.change();
            }
        });

        var weekList = $(".weekList").children();
        $("#week_appoint").click(function () {
            if (this.checked) {
                if ($(weekList).filter(":checked").length == 0) {
                    $(weekList.eq(0)).attr("checked", true);
                }
                weekList.eq(0).change();
            }
        });

        weekList.change(function () {
            var week_appoint = $("#week_appoint").prop("checked");
            if (week_appoint) {
                var vals = [];
                weekList.each(function () {
                    if (this.checked) {
                        vals.push(this.value);
                    }
                });
                var val = "?";
                if (vals.length > 0 && vals.length < 7) {
                    val = vals.join(",");
                } else if (vals.length == 7) {
                    val = "*";
                }
                var item = $("input[name=v_week]");
                item.val(val);
                item.change();
            }
        });
    });
});