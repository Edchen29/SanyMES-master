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
    var TableName = 'MbomHeader';
    
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
                , { field: 'Id', width: 80, sort: true, fixed: false, hide: false, title: 'Id' }
                , { field: 'ProductId', width: 120, sort: true, fixed: false, hide: false,  title: '产品标识', templet: function (d) { return GetLabel('ProductId', 'Id', 'Name', d.ProductId) } }
                , { field: 'ProductCode', width: 120, sort: true, fixed: false, hide: false, title: '产品', event: 'MbomDetail', templet: '#ShowDtlProductCode' }
                , { field: 'LineId', width: 150, sort: true, fixed: false, hide: false, title: '生产线标识', templet: function (d) { return GetLabel('LineId', 'Id', 'LineName', d.LineId) } }
                , { field: 'DrawingNumber', width: 150, sort: true, fixed: false, hide: false, title: '图号' }
                , { field: 'ChangeOrder', width: 150, sort: true, fixed: false, hide: false, title: '变更单号' }
                , {field:'Version', width:100, sort: true, fixed: false, hide: false, title: '版本' }
                , {field:'Verifyer', width:100, sort: true, fixed: false, hide: false, title: '审核员' }
                , {field:'CreateTime', width:150, sort: true, fixed: false, hide: false, title: '建立时间' }
                , {field:'CreateBy', width:100, sort: true, fixed: false, hide: false, title: '建立者' }
                , {field:'UpdateTime', width:150, sort: true, fixed: false, hide: false, title: '更新时间' }
                , {field:'UpdateBy', width:100, sort: true, fixed: false, hide: false, title: '更新者' }
            ]];

            mainList.Table = table.render({
                elem: '#mainList'
                , url: "/" + AreaName + "/" + TableName + "/Load"
                , method: "post"
                , page: true //开启分页
                , cols: hhweb.ColumnSetting('mainList', cols_arr)
                , id: 'mainList'
                , limit: 6
                , limits: [6, 50, 100, 200, 500, 1000]
                , defaultToolbar: ['filter']
                , toolbar: '#toolbarTable'
                , height: '333'
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
    var AreaNameDtlMbomDetail = 'mes';
    var TableNameDtlMbomDetail = 'MbomDetail';
    //{子表字段：主表字段}
    var NameDtlMbomDetail = { MbomHeaderId: 'Id' };
    var vmDtlMbomDetail = new Vue({
        el: '#modifyFormDtl_' + TableNameDtlMbomDetail
    });
    var vmqDtlMbomDetail = new Vue({
        data: { DictType: '', HeaderId: '' }
    });
    //编辑
    var EditInfoDtlMbomDetail = function (tabledata) {
        data = tabledata;
        vmDtlMbomDetail.$set('$data', tabledata);
        var list = {};
        $('.ClearSelector_' + TableNameDtlMbomDetail).each(function () {
            var selDom = ($(this));
            if (!$(selDom)[0].name.startsWith("q")) {
                list[$(selDom)[0].name] = data[$(selDom)[0].name] + "";
            }
        });
        //表单修改时填充需修改的数据
        form.val('modifyFormDtl_' + TableNameDtlMbomDetail, list);
    };

    var mainListDtlMbomDetail = {
        Render: function () {
            var cols_arr = [[
                { checkbox: true, fixed: true }
                , { field: 'Id', width: 80, sort: true, fixed: false, hide: false, title: 'Id' }
                , { field: 'MbomHeaderId', width: 100, sort: true, fixed: false, hide: false, title: '主表标识' }
                , { field: 'ProductCode', width: 150, sort: true, fixed: false, hide: false, title: '产品代号' }
                , { field: 'StepCode', width: 150, sort: true, fixed: false, hide: false, title: '工序代号' }
                , { field: 'StepId', width: 150, sort: true, fixed: false, hide: false, title: '工序名称', templet: function (d) { return GetLabel('StepId', 'Id', 'Name', d.StepId) } }
                , { field: 'MaterialCode', width: 150, sort: true, fixed: false, hide: false, title: '物料' }
                , { field: 'DrawingNumber', width: 150, sort: true, fixed: false, hide: false, title: '图号' }
                , { field: 'BaseQty', width: 100, sort: true, fixed: false, hide: false, title: '基数数量' }
                , { field: 'IsCheck', width: 100, sort: true, fixed: false, hide: false, title: '需要质检', templet: function (d) { return GetLabel('IsCheck', 'DictValue', 'DictLabel', d.IsCheck) } }
                , { field: 'CreateTime', width: 150, sort: true, fixed: false, hide: false, title: '建立时间' }
                , { field: 'CreateBy', width: 100, sort: true, fixed: false, hide: false, title: '建立者' }
                , { field: 'UpdateTime', width: 150, sort: true, fixed: false, hide: false, title: '更新时间' }
                , { field: 'UpdateBy', width: 100, sort: true, fixed: false, hide: false, title: '更新者' }
            ]];

            mainListDtlMbomDetail.Table = table.render({
                elem: '#mainListDtl' + TableNameDtlMbomDetail
                , url: "/" + AreaNameDtlMbomDetail + "/" + TableNameDtlMbomDetail + "/Load"
                , where: vmqDtlMbomDetail.$data
                , method: "post"
                , page: true //开启分页
                , cols: hhweb.ColumnSetting('mainListDtl' + TableNameDtlMbomDetail, cols_arr)
                , id: 'mainListDtl' + TableNameDtlMbomDetail
                , limit: 10
                , limits: [10, 50, 100, 200, 500, 1000]
                , defaultToolbar: ['filter']
                , toolbar: '#toolbarTable' + TableNameDtlMbomDetail
                , height: '455'
                , cellMinWidth: 80
                , size: 'sm'
                , done: function (res) { }
            });

            return mainListDtlMbomDetail.Table;
        },
        Load: function () {
            if (mainListDtlMbomDetail.Table == undefined) {
                mainListDtlMbomDetail.Table = this.Render();
                return;
            }
            table.reload('mainListDtl' + TableNameDtlMbomDetail, {
                url: "/" + AreaNameDtlMbomDetail + "/" + TableNameDtlMbomDetail + "/Load"
                , where: vmqDtlMbomDetail.$data
                , method: "post"
                , page: { curr: 1 }
            });
        }
    };
    All.push({ AreaNameDtl: AreaNameDtlMbomDetail, TableNameDtl: TableNameDtlMbomDetail, vmqDtl: vmqDtlMbomDetail, vmDtl: vmDtlMbomDetail, EditInfoDtl: EditInfoDtlMbomDetail, NameDtl: NameDtlMbomDetail, mainListDtl: mainListDtlMbomDetail });

    var selector = {
        'LineId': {
            SelType: "FromUrl",
            SelFrom: "/configure/Line/Load",
            SelModel: "LineId",
            SelLabel: "LineName",
            SelValue: "Id",
            Dom: [$("[name='LineId']"), $("[name='qLineId']")]
        }, 'ProductId': {
            SelType: "FromUrl",
            SelFrom: "/configure/ProductHeader/Load",
            SelModel: "ProductId",
            SelLabel: "Name",
            SelValue: "Id"
        }, 'ProductCode': {
            SelType: "FromUrl",
            SelFrom: "/configure/ProductHeader/Load",
            SelModel: "ProductCode",
            SelLabel: "Code",
            SelValue: "Code",
            Dom: [$("[name='ProductCode']"), $("[name='qProductCode']")]
        },'StepCode': {
            SelType: "FromUrl",
            SelFrom: "/configure/Step/Load",
            SelModel: "StepCode",
            SelLabel: "Name",
            SelValue: "Code",
            Dom: [$("[name='StepCode']")]
        }, 'StepId': {
            SelType: "FromUrl",
            SelFrom: "/configure/Step/Load",
            SelModel: "StepId",
            SelLabel: "Name",
            SelValue: "Id"
        }, 'IsCheck': {
            SelType: "FromDict",
            SelFrom: "IsCheck",
            SelModel: "IsCheck",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='IsCheck']")]
        },
    };
    
    var vml = new Array({
        vm: vm,
        vmq: vmq,
        vmDtlMbomDetail: vmDtlMbomDetail,
    });
    
    Universal.BindSelector(vml, selector);
    Universal.mmain(AreaName, TableName, vm, vmq, EditInfo, selfbtn, mainList);
    Universal.mainDtl(selfbtn, All);
});