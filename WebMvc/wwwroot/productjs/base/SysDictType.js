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

    var AreaName = 'base';
    var TableName = 'SysDictType';

    var vm = new Vue({
        el: '#modifyForm'
    });

    var vmq = new Vue({
        el: '#panelSearch',
        data: {
        }
    });

    var mainList = {
        Render: function () {
            var cols_arr = [[
                { checkbox: true, fixed: true }
                , {field:'Id', width:80, sort: true, fixed: false, hide: false, title: ' id' }
                , {field:'DictName', width:150, sort: true, fixed: false, hide: false, title: '字典名称' }
                , {field:'DictType', width:150, sort: true, fixed: false, hide: false,event: 'SysDictData',templet: '#ShowDtlDictType', title: '字典类型' }
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
                , limit: 10
                , limits: [10, 20, 50, 100, 200, 500, 1000]
                , defaultToolbar: ['filter']
                , toolbar: '#toolbarTable'
                , height: '435'
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

    Universal.mmain(AreaName, TableName, vm, vmq, EditInfo, selfbtn, mainList);

    //子表逻辑
    var All = new Array();
    var AreaNameDtlSysDictData = 'base';
    var TableNameDtlSysDictData = 'SysDictData';
    //{子表字段：主表字段}
    var NameDtlSysDictData = { DictType: 'DictType', HeaderId: 'Id' };
    var vmDtlSysDictData = new Vue({
        el: '#modifyFormDtl_' + TableNameDtlSysDictData
    });
    var vmqDtlSysDictData = new Vue({
        data: { DictType: '', HeaderId: '' }
    });
    //编辑
    var EditInfoDtlSysDictData = function (tabledata) {
        data = tabledata;
        vmDtlSysDictData.$set('$data', tabledata);
        var list = {};
        $('.ClearSelector_' + TableNameDtlSysDictData).each(function () {
            var selDom = ($(this));
            if (!$(selDom)[0].name.startsWith("q")) {
                list[$(selDom)[0].name] = data[$(selDom)[0].name] + "";
            }
        });
        //表单修改时填充需修改的数据
        form.val('modifyFormDtl_' + TableNameDtlSysDictData, list);
    };

    var mainListDtlSysDictData = {
        Render: function () {
            var cols_arr = [[
                { checkbox: true, fixed: true }
                , { field: 'Id', width: 80, sort: true, fixed: false, hide: false, title: 'id' }
                , { field: 'HeaderId', width: 150, sort: true, fixed: false, hide: true, title: '头表id' }
                , { field: 'DictSort', width: 150, sort: true, fixed: false, hide: false, title: '字典排序' }
                , { field: 'DictLabel', width: 150, sort: true, fixed: false, hide: false, templet: '#TplDictLabel', title: '字典标签' }
                , { field: 'DictValue', width: 150, sort: true, fixed: false, hide: false, title: '字典键值' }
                , { field: 'DictType', width: 150, sort: true, fixed: false, hide: false, title: '字典类型' }
                , { field: 'CssClass', width: 150, sort: true, fixed: false, hide: false, templet: '#TplCssClass', title: '样式属性' }
                , { field: 'ListClass', width: 150, sort: true, fixed: false, hide: false, title: '回显样式' }
                , { field: 'IsDefault', width: 150, sort: true, fixed: false, hide: false, title: '是否默认（Y是 N否）' }
                , { field: 'Remark', width: 150, sort: true, fixed: false, hide: false, title: '备注' }
                , { field: 'CreateTime', width: 150, sort: true, fixed: false, hide: false, title: '创建时间' }
                , { field: 'CreateBy', width: 150, sort: true, fixed: false, hide: false, title: '创建用户' }
                , { field: 'UpdateTime', width: 150, sort: true, fixed: false, hide: false, title: '更新时间' }
                , { field: 'UpdateBy', width: 150, sort: true, fixed: false, hide: false, title: '更新用户' }
            ]];

            mainListDtlSysDictData.Table = table.render({
                elem: '#mainListDtl' + TableNameDtlSysDictData
                , url: "/" + AreaNameDtlSysDictData + "/" + TableNameDtlSysDictData + "/Load"
                , where: vmqDtlSysDictData.$data
                , method: "post"
                , page: true //开启分页
                , cols: hhweb.ColumnSetting('mainListDtl' + TableNameDtlSysDictData, cols_arr)
                , id: 'mainListDtl' + TableNameDtlSysDictData
                , limit: 8
                , limits: [8, 50, 100, 200, 500, 1000]
                , defaultToolbar: ['filter']
                , toolbar: '#toolbarTable' + TableNameDtlSysDictData
                , height: '350'
                , cellMinWidth: 80
                , size: 'sm'
                , done: function (res) { }
            });

            return mainListDtlSysDictData.Table;
        },
        Load: function () {
            if (mainListDtlSysDictData.Table == undefined) {
                mainListDtlSysDictData.Table = this.Render();
                return;
            }
            table.reload('mainListDtl' + TableNameDtlSysDictData, {
                url: "/" + AreaNameDtlSysDictData + "/" + TableNameDtlSysDictData + "/Load"
                , where: vmqDtlSysDictData.$data
                , method: "post"
                , page: { curr: 1 }
            });
        }
    };

    All.push({ AreaNameDtl: AreaNameDtlSysDictData, TableNameDtl: TableNameDtlSysDictData, vmqDtl: vmqDtlSysDictData, vmDtl: vmDtlSysDictData, EditInfoDtl: EditInfoDtlSysDictData, NameDtl: NameDtlSysDictData, mainListDtl: mainListDtlSysDictData });

    var selector = {
        'CssClass': {
            SelType: "FromDict",
            SelFrom: "dictCss",
            SelModel: "CssClass",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='CssClass']"), $("[name='qCssClass']")]
        }
    };

    var vml = new Array({
        vm: vm,
        vmq: vmq,
        vmDtlSysDictData: vmDtlSysDictData,
    });

    Universal.BindSelector(vml, selector);

    Universal.mainDtl(selfbtn, All);
});