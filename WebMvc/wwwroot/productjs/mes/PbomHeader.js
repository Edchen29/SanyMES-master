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
    var TableName = 'PbomHeader';
    
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
                , { field: 'MaterialCode', width: 150, sort: true, fixed: false, hide: false, event: 'PbomDetail', templet: '#ShowDtlMaterialCode', title: '物料编码' }
                , {field:'Type', width:150, sort: true, fixed: false, hide: false, title: '类型' }
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
                , height: '450'
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
    var AreaNameDtlPbomDetail = 'mes';
    var TableNameDtlPbomDetail = 'PbomDetail';
    //{子表字段：主表字段}
    var NameDtlPbomDetail = { ParentMaterialCode: 'MaterialCode' };
    var vmDtlPbomDetail = new Vue({
        el: '#modifyFormDtl_' + TableNameDtlPbomDetail
    });
    var vmqDtlPbomDetail = new Vue({
        data: { DictType: '', HeaderId: '' }
    });
    //编辑
    var EditInfoDtlPbomDetail = function (tabledata) {
        data = tabledata;
        vmDtlPbomDetail.$set('$data', tabledata);
        var list = {};
        $('.ClearSelector_' + TableNameDtlPbomDetail).each(function () {
            var selDom = ($(this));
            if (!$(selDom)[0].name.startsWith("q")) {
                list[$(selDom)[0].name] = data[$(selDom)[0].name] + "";
            }
        });
        //表单修改时填充需修改的数据
        form.val('modifyFormDtl_' + TableNameDtlPbomDetail, list);
    };

    var mainListDtlPbomDetail = {
        Render: function () {
            var cols_arr = [[
                { checkbox: true, fixed: true }
                , { field: 'Id', width: 150, sort: true, fixed: false, hide: false, title: 'Id' }
                , { field: 'StepId', width: 150, sort: true, fixed: false, hide: false, title: '工序标识' }
                , { field: 'StationId', width: 150, sort: true, fixed: false, hide: false, title: '工位标识' }
                , { field: 'ParentMaterialCode', width: 150, sort: true, fixed: false, hide: false, title: '父料号' }
                , { field: 'ChildMaterialCode', width: 150, sort: true, fixed: false, hide: false, title: '子料号' }
                , { field: 'IsReplace', width: 150, sort: true, fixed: false, hide: false, title: '是否替代料' }
                , { field: 'Double', width: 150, sort: true, fixed: false, hide: false, title: '倍用量' }
                , { field: 'DrawingNumber', width: 150, sort: true, fixed: false, hide: false, title: '图号' }
                , { field: 'ManHours', width: 150, sort: true, fixed: false, hide: false, title: '工时' }
                , { field: 'AssembleTips', width: 150, sort: true, fixed: false, hide: false, title: '装配' }
                , { field: 'CreateTime', width: 150, sort: true, fixed: false, hide: false, title: '建立时间' }
                , { field: 'CreateBy', width: 150, sort: true, fixed: false, hide: false, title: '建立者' }
                , { field: 'UpdateTime', width: 150, sort: true, fixed: false, hide: false, title: '更新时间' }
                , { field: 'UpdateBy', width: 150, sort: true, fixed: false, hide: false, title: '更新者' }
            ]];

            mainListDtlPbomDetail.Table = table.render({
                elem: '#mainListDtl' + TableNameDtlPbomDetail
                , url: "/" + AreaNameDtlPbomDetail + "/" + TableNameDtlPbomDetail + "/Load"
                , where: vmqDtlPbomDetail.$data
                , method: "post"
                , page: true //开启分页
                , cols: hhweb.ColumnSetting('mainListDtl' + TableNameDtlPbomDetail, cols_arr)
                , id: 'mainListDtl' + TableNameDtlPbomDetail
                , limit: 8
                , limits: [8, 50, 100, 200, 500, 1000]
                , defaultToolbar: ['filter']
                , toolbar: '#toolbarTable' + TableNameDtlPbomDetail
                , height: '350'
                , cellMinWidth: 80
                , size: 'sm'
                , done: function (res) { }
            });

            return mainListDtlPbomDetail.Table;
        },
        Load: function () {
            if (mainListDtlPbomDetail.Table == undefined) {
                mainListDtlPbomDetail.Table = this.Render();
                return;
            }
            table.reload('mainListDtl' + TableNameDtlPbomDetail, {
                url: "/" + AreaNameDtlPbomDetail + "/" + TableNameDtlPbomDetail + "/Load"
                , where: vmqDtlPbomDetail.$data
                , method: "post"
                , page: { curr: 1 }
            });
        }
    };
    All.push({ AreaNameDtl: AreaNameDtlPbomDetail, TableNameDtl: TableNameDtlPbomDetail, vmqDtl: vmqDtlPbomDetail, vmDtl: vmDtlPbomDetail, EditInfoDtl: EditInfoDtlPbomDetail, NameDtl: NameDtlPbomDetail, mainListDtl: mainListDtlPbomDetail });

    var selector = {
    };
    
    var vml = new Array({
        vm: vm,
        vmq: vmq,
        vmDtlPbomDetail: vmDtlPbomDetail,
    });
    
    Universal.BindSelector(vml, selector);
    Universal.mmain(AreaName, TableName, vm, vmq, EditInfo, selfbtn, mainList);
    Universal.mainDtl(selfbtn, All);
});