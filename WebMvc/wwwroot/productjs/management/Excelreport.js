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
    
    var AreaName = 'management';
    var TableName = 'Excelreport';
    
    var vm = new Vue({
        el: '#modifyForm'
    });
    
    var vmq = new Vue({
        el: '#panelSearch',
        data: {
        }
    });
    
    hhweb.Config = {
        'CreateTime': vm,
        'UpdateTime': vm,

        'qCreateTime': vmq,
        'qUpdateTime': vmq,
    };
      
    var mainList = {
        Render: function () {
            var cols_arr = [[
                { checkbox: true, fixed: true }
                , {field:'Id', width:80, sort: true, fixed: false, hide: false, title: 'Id' }
                , {field:'Name', width:150, sort: true, fixed: false, hide: false, title: '名称' }
                , {field:'Sql', width:350, sort: true, fixed: false, hide: false, title: 'sql' }
                , {field:'Params', width:150, sort: true, fixed: false, hide: false, title: '参数' }
                , {field:'Remark', width:150, sort: true, fixed: false, hide: false, title: '备注' }
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
    };
    
    var selector = {
    };
    
    var vml = new Array({
        vm: vm,
        vmq: vmq,
    });
    
    Universal.BindSelector(vml, selector);
    Universal.mmain(AreaName, TableName, vm, vmq, EditInfo, selfbtn, mainList);

    $('[name = "Params"]').on('click', function () {
        var DictKey = vm.$data["Sql"];
        if (DictKey != undefined && DictKey != null && DictKey != "") {
            layer.open({
                title: "参数",
                area: ["500px", "300px"],
                type: 1,
                content: $('#DivConfigParams'),
                btn: ['保存', '关闭'],
                success: function (layero, index) {
                    LoadParams(DictKey);
                },
                yes: function (index) {
                    vm.$data["Params"] = GetParams();
                    layer.close(index);
                },
                btn2: function (index) {
                    layer.close(index);
                }
            });
        }
    });

    function LoadParams(DictKey) {
        var oldobj = {};
        if (vm.$data["Params"] != undefined && vm.$data["Params"] != null && vm.$data["Params"] != "") {
            oldobj = JSON.parse(vm.$data["Params"]);
        }

        $("#ConfigParams").html("");
        //var Params = DictKey.match(/\[[^\]]+\]/g);
        var Params = DictKey.match(/\{[^\}]+\}/g);
        if (Params != null) {
            var dom = '';
            for (var i = 0; i < Params.length; i++) {
                var value = Params[i];

                var oldvalue = '';
                if (oldobj[value] !== undefined) {
                    oldvalue = oldobj[value];
                }
                dom += '<div class="layui-col-sm12">';
                dom += '<label class="layui-form-label layui-col-sm3">' + value + '</label>';
                dom += '<div class="layui-input-inline layui-col-sm9">';
                dom += '<input data-model="' + value + '" class="layui-input ParamsKey" type="text" autocomplete="off" lay-verify="" value="' + oldvalue + '">'
                dom += '</div></div>';
            }
            $("#ConfigParams").html(dom);
            form.render();
        }
    }

    function GetParams() {
        var Params = {};
        $.each($(".ParamsKey"), function (key, value) {
            Params[$(this).data("model")] = $(this).val();
        });
        return JSON.stringify(Params);
    }
});