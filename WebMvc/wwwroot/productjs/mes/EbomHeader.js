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
    
    var AreaName = 'mes';
    var TableName = 'EbomHeader';
    
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
                , {field:'Id', width:150, sort: true, fixed: false, hide: false, title: 'Id' }
                , { field: 'ItemCode', width: 150, sort: true, fixed: false, hide: false, event: 'EbomDetail', templet: '#ShowDtlItemCode', title: '产品代号' }
                , {field:'Children', width:150, sort: true, fixed: false, hide: false, title: '子料' }
                , {field:'CreateTime', width:150, sort: true, fixed: false, hide: false, title: '建立时间' }
                , {field:'CreateBy', width:150, sort: true, fixed: false, hide: false, title: '建立者' }
                , {field:'UpdateTime', width:150, sort: true, fixed: false, hide: false, title: '更新时间' }
                , {field:'UpdateBy', width:150, sort: true, fixed: false, hide: false, title: '更新者' }
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
    //子表逻辑
    var All = new Array();
    var AreaNameDtlEbomDetail = 'mes';
    var TableNameDtlEbomDetail = 'EbomDetail';
    //{子表字段：主表字段}
    var NameDtlEbomDetail = { BomHeaderId: 'Id' };
    var vmDtlEbomDetail = new Vue({
        el: '#modifyFormDtl_' + TableNameDtlEbomDetail
    });
    var vmqDtlEbomDetail = new Vue({
        data: { DictType: '', HeaderId: '' }
    });
    //编辑
    var EditInfoDtlEbomDetail = function (tabledata) {
        data = tabledata;
        vmDtlEbomDetail.$set('$data', tabledata);
        var list = {};
        $('.ClearSelector_' + TableNameDtlEbomDetail).each(function () {
            var selDom = ($(this));
            if (!$(selDom)[0].name.startsWith("q")) {
                list[$(selDom)[0].name] = data[$(selDom)[0].name] + "";
            }
        });
        //表单修改时填充需修改的数据
        form.val('modifyFormDtl_' + TableNameDtlEbomDetail, list);
    };

    var mainListDtlEbomDetail = {
        Render: function () {
            var cols_arr = [[
                { checkbox: true, fixed: true }
                , { field: 'Id', width: 150, sort: true, fixed: false, hide: false, title: 'Id' }
                , { field: 'BomHeaderId', width: 150, sort: true, fixed: false, hide: false, title: '主表ID' }
                , { field: 'MaterialCode', width: 150, sort: true, fixed: false, hide: false, title: '物料编码' }
                , { field: 'MaterialName', width: 150, sort: true, fixed: false, hide: false, title: '物料名称' }
                , { field: 'MaterialSpec', width: 150, sort: true, fixed: false, hide: false, title: '品名规格' }
                , { field: 'Unit', width: 150, sort: true, fixed: false, hide: false, title: '单位' }
                , { field: 'Qty', width: 150, sort: true, fixed: false, hide: false, title: '数量' }
                , { field: 'CreateTime', width: 150, sort: true, fixed: false, hide: false, title: '建立时间' }
                , { field: 'CreateBy', width: 150, sort: true, fixed: false, hide: false, title: '建立者' }
                , { field: 'UpdateTime', width: 150, sort: true, fixed: false, hide: false, title: '更新时间' }
                , { field: 'UpdateBy', width: 150, sort: true, fixed: false, hide: false, title: '更新者' }
            ]];

            mainListDtlEbomDetail.Table = table.render({
                elem: '#mainListDtl' + TableNameDtlEbomDetail
                , url: "/" + AreaNameDtlEbomDetail + "/" + TableNameDtlEbomDetail + "/Load"
                , where: vmqDtlEbomDetail.$data
                , method: "post"
                , page: true //开启分页
                , cols: hhweb.ColumnSetting('mainListDtl' + TableNameDtlEbomDetail, cols_arr)
                , id: 'mainListDtl' + TableNameDtlEbomDetail
                , limit: 8
                , limits: [8, 50, 100, 200, 500, 1000]
                , defaultToolbar: ['filter']
                , toolbar: '#toolbarTable' + TableNameDtlEbomDetail
                , height: '350'
                , cellMinWidth: 80
                , size: 'sm'
                , done: function (res) { }
            });

            return mainListDtlEbomDetail.Table;
        },
        Load: function () {
            if (mainListDtlEbomDetail.Table == undefined) {
                mainListDtlEbomDetail.Table = this.Render();
                return;
            }
            table.reload('mainListDtl' + TableNameDtlEbomDetail, {
                url: "/" + AreaNameDtlEbomDetail + "/" + TableNameDtlEbomDetail + "/Load"
                , where: vmqDtlEbomDetail.$data
                , method: "post"
                , page: { curr: 1 }
            });
        }
    };
    All.push({ AreaNameDtl: AreaNameDtlEbomDetail, TableNameDtl: TableNameDtlEbomDetail, vmqDtl: vmqDtlEbomDetail, vmDtl: vmDtlEbomDetail, EditInfoDtl: EditInfoDtlEbomDetail, NameDtl: NameDtlEbomDetail, mainListDtl: mainListDtlEbomDetail });

    var selector = {
    };
    
    var vml = new Array({
        vm: vm,
        vmq: vmq,
        vmDtlEbomDetail: vmDtlEbomDetail,
    });
    
    Universal.BindSelector(vml, selector);
    Universal.mmain(AreaName, TableName, vm, vmq, EditInfo, selfbtn, mainList);
    Universal.mainDtl(selfbtn, All);
});