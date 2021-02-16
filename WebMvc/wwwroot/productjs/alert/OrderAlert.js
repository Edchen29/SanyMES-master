layui.config({
    base: "/js/"
}).use(['form', 'element', 'vue', 'layer', 'laydate', 'jquery', 'table', 'hhweb', 'utils', 'Universal'], function () {
    var form = layui.form,
        layer = layui.layer,
        element = layui.element,
        laydate = layui.laydate,
        $ = layui.jquery,
        table = layui.table,
        hhweb = layui.hhweb,
        Universal = layui.Universal;

    var AreaName = 'alert';
    var TableName = 'OrderAlert';

    var vm = new Vue({
        el: '#modifyForm'
    });

    var vmq = new Vue({
        el: '#panelSearch',
        data: {
        }
    });

    hhweb.Config = {
        'JobStartTime': vm,
        'UpdateTime': vm,
        'CreateTime': vm,

        'qJobStartTime': vmq,
        'qUpdateTime': vmq,
        'qCreateTime': vmq,
    };

    var mainList = {
        Render: function () {
            var cols_arr = [[
                { checkbox: true, fixed: true }
                , { field: 'Id', width: 80, sort: true, fixed: false, hide: false, title: '行标识' }
                , { field: 'OrderCode', width: 150, sort: true, fixed: false, hide: false, title: '订单号' }
                , { field: 'ProductCode', width: 150, sort: true, fixed: false, hide: false, title: '产品代号' }
                , { field: 'PartMaterialCode', width: 150, sort: true, fixed: false, hide: false, title: '产品料号' }
                , { field: 'SerialNumber', width: 150, sort: true, fixed: false, hide: false, title: '序列号' }
                , { field: 'Status', width: 120, sort: true, fixed: false, hide: false, templet: function (d) { return GetLabel('Status', 'DictValue', 'DictLabel', d.Status) }, title: '订单状态' }
                , { field: 'AlertMsg', width: 220, sort: true, fixed: false, hide: false, title: '预警信息' }
                , { field: 'Flag', width: 80, sort: true, fixed: false, hide: false, title: '标识符' }
                , { field: 'IsSpeak', width: 150, sort: true, fixed: false, hide: false, title: '是否播报', templet: '#checkboxTpl', unresize: true }
                , { field: 'CreateTime', width: 150, sort: true, fixed: false, hide: false, title: '创建时间' }
                , { field: 'CreateBy', width: 150, sort: true, fixed: false, hide: false, title: '创建用户' }
                , { field: 'Updatetime', width: 150, sort: true, fixed: false, hide: false, title: '更新时间' }
                , { field: 'UpdateBy', width: 150, sort: true, fixed: false, hide: false, title: '更新用户' }
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
                , done: function (res) {
                      if (res.count > 0) {
                        for (var i = 0; i < res.count; i++) {
                            if (res.data[i].IsSpeak) {
                                var tt = '订单号' + res.data[i].OrderCode + res.data[i].AlertMsg;
                                var text = new SpeechSynthesisUtterance(tt);
                                window.speechSynthesis.speak(text);
                            }
                        }
                    }
                }
            });

            return mainList.Table;
        },
        Load: function () {
            if (mainList.Table == undefined) {
                mainList.Table = this.Render();
                return;
            }
            table.reload('mainList', {
                done: function (res, curr, count) {
                    if (res.count > 0) {
                        for (var i = 0; i < res.count; i++) {
                            if (res.data[i].IsSpeak) {
                                var tt = '订单号' + res.data[i].OrderCode + res.data[i].AlertMsg;
                                var text = new SpeechSynthesisUtterance(tt);
                                window.speechSynthesis.speak(text);
                            }
                        }
                    }
                }
            });
        }
    };

    form.on('checkbox(lock)', function (obj) {
        $.ajax({
            url: "/" + AreaName + "/" + TableName + "/Update",
            type: "post",
            data: { Id: this.value, IsSpeak: obj.elem.checked },
            dataType: "json"
        });
    });

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
        DomConfig: function (AddOrUpdate) {
            if (AddOrUpdate) {
                hhweb.DomEnable($("#modifyForm [name='OrderCode']"));
            }
            else {
                hhweb.DomDisable($("#modifyForm [name='OrderCode']"));
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
        'Status': {
            SelType: "FromDict",
            SelFrom: "WOStatus",
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
});