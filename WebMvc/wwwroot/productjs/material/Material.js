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
    
    var AreaName = 'material';
    var TableName = 'Material';
    
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
                , {field:'Code', width:150, sort: true, fixed: false, hide: false, title: '物料编号' }
                , {field:'Name', width:150, sort: true, fixed: false, hide: false, title: '物料名称' }
                , { field: 'Type', width: 150, sort: true, fixed: false, hide: false, title: '物料类型' }
                , { field: 'DrawingNumber', width: 150, sort: true, fixed: false, hide: false, title: '图号' }
                , { field: 'FunctionClass', width: 150, sort: true, fixed: false, hide: false, title: '功能类别', templet: function (d) { return GetLabel('FunctionClass', 'DictValue', 'DictLabel', d.FunctionClass) } }
                , { field: 'BuildClass', width: 150, sort: true, fixed: false, hide: false, title: '产品类别', templet: function (d) { return GetLabel('BuildClass', 'DictValue', 'DictLabel', d.BuildClass) } }
                , {field:'BarCode', width:150, sort: true, fixed: false, hide: false, title: '物料条码' }
                , {field:'BarCode1', width:150, sort: true, fixed: false, hide: false, title: '关联条码' }
                , { field: 'Specification', width: 150, sort: true, fixed: false, hide: false, title: '品名规格' }
                , { field: 'Weight', width: 150, sort: true, fixed: false, hide: false, title: '重量' }
                , { field: 'Unit', width: 150, sort: true, fixed: false, hide: false, title: '单位', templet: function (d) { return GetLabel('Unit', 'Code', 'Name', d.Unit) } }
                , { field: 'MaxRow', width: 150, sort: true, fixed: false, hide: false, title: '摆放最大行' }
                , { field: 'MaxColumn', width: 150, sort: true, fixed: false, hide: false, title: '摆放最大列' }
                , { field: 'MaxLayer', width: 150, sort: true, fixed: false, hide: false, title: '摆放最大层' }
                , { field: 'ClassABC', width: 150, sort: true, fixed: false, hide: false, title: 'ABC分类', templet: function (d) { return GetLabel('ClassABC', 'DictValue', 'DictLabel', d.ClassABC) } }
                , { field: 'QcCheck', width: 150, sort: true, fixed: false, hide: false, title: '品检' }
                , { field: 'UniqueMark', width: 150, sort: true, fixed: false, hide: false, title: '唯一标识码' }
                , { field: 'CreateTime', width: 150, sort: true, fixed: false, hide: false, title: '创建时间' }
                , { field: 'CreateBy', width: 150, sort: true, fixed: false, hide: false, title: '创建用户' }
                , { field: 'UpdateTime', width: 150, sort: true, fixed: false, hide: false, title: '更新时间' }
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
        'FunctionClass': {
            SelType: "FromDict",
            SelFrom: "FunctionClass",
            SelModel: "FunctionClass",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='FunctionClass']"), $("[name='qFunctionClass']")]
        }, 'BuildClass': {
            SelType: "FromDict",
            SelFrom: "BuildClass",
            SelModel: "BuildClass",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='BuildClass']"), $("[name='qBuildClass']")]
        }, 'ClassABC': {
            SelType: "FromDict",
            SelFrom: "ABCType",
            SelModel: "ClassABC",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='ClassABC']"), $("[name='qClassABC']")]
        }, 'Unit': {
            SelType: "FromUrl",
            SelFrom: "/material/MaterialUnit/Load",
            SelModel: "Unit",
            SelLabel: "Name",
            SelValue: "Code",
            Dom: [$("[name='Unit']"), $("[name='qUnit']")]
        },
    };
    
    var vml = new Array({
        vm: vm,
        vmq: vmq,
    });
    
    Universal.BindSelector(vml, selector);
    Universal.mmain(AreaName, TableName, vm, vmq, EditInfo, selfbtn, mainList);
});