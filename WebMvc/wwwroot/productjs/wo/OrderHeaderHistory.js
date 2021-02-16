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
    
    var AreaName = 'wo';
    var TableName = 'OrderHeaderHistory';
    
    var vm = new Vue({
        el: '#modifyForm'
    });
    
    var vmq = new Vue({
        el: '#panelSearch',
        data: {
        }
    });
    
    hhweb.Config = {
        'PlanStartTime': vm,
        'PlanEndTime': vm,
        'ActualStartTime': vm,
        'ActualEndTime': vm,
        'CreateTime': vm,
        'UpdateTime': vm,

        'qPlanStartTime': vmq,
        'qPlanEndTime': vmq,
        'qActualStartTime': vmq,
        'qActualEndTime': vmq,
        'qCreateTime': vmq,
        'qUpdateTime': vmq,
    };
      
    var mainList = {
        Render: function () {
            var cols_arr = [[
                { checkbox: true, fixed: true }
                , { field: 'Id', width: 60, sort: true, fixed: false, hide: false, title: 'Id' }
                , { field: 'Code', width: 120, sort: true, fixed: false, hide: false, event: 'OrderDetiailHistory', templet: '#ShowDtlCode', title: '工单号' }
                , { field: 'ProductCode', width: 120, sort: true, fixed: false, hide: false, title: '产品' }
                , { field: 'ProductCode', width: 120, sort: true, fixed: false, hide: false, templet: function (d) { return GetLabel('TProductCode', 'Code', 'MachineType', d.ProductCode) }, title: '机型' }
                , { field: 'PartMaterialCode', width: 150, sort: true, fixed: false, hide: false, title: '部件料号' }
                , { field: 'PlanQty', width: 90, sort: true, fixed: false, hide: false, title: '计划量' }
                , { field: 'CompleteQty', width: 90, sort: true, fixed: false, hide: false, title: '完成量' }
                , { field: 'NGQty', width: 90, sort: true, fixed: false, hide: false, title: '不良量' }
                , { field: 'Status', width: 100, sort: true, fixed: false, hide: false, templet: function (d) { return GetLabel('Status', 'DictValue', 'DictLabel', d.Status) }, title: '工单状态' }
                , { field: 'Type', width: 100, sort: true, fixed: false, hide: false, templet: function (d) { return GetLabel('Type', 'DictValue', 'DictLabel', d.Type) }, title: '工单类型' }
                , { field: 'LineId', width: 120, sort: true, fixed: false, hide: false, templet: function (d) { return GetLabel('LineId', 'Id', 'LineCode', d.LineId) }, title: '线体' }
                , { field: 'LotNo', width: 150, sort: true, fixed: false, hide: false, title: '批次号' }
                , { field: 'Priority', width: 100, sort: true, fixed: false, hide: false, templet: function (d) { return GetLabel('Priority', 'DictValue', 'DictLabel', d.Priority) }, title: '优先级' }
                , { field: 'PlanStartTime', width: 150, sort: true, fixed: false, hide: false, title: '计划开始时间' }
                , { field: 'PlanEndTime', width: 150, sort: true, fixed: false, hide: false, title: '预计完成时间' }
                , { field: 'ActualStartTime', width: 150, sort: true, fixed: false, hide: false, title: '实际开始时间' }
                , { field: 'ActualEndTime', width: 150, sort: true, fixed: false, hide: false, title: '实际结束时间' }
                , { field: 'ReserveNo', width: 150, sort: true, fixed: false, hide: false, title: '预留号' }
                , { field: 'ReserveRowNo', width: 150, sort: true, fixed: false, hide: false, title: '预留行号' }
                , { field: 'WorkFactory', width: 150, sort: true, fixed: false, hide: false, title: '生产工厂' }
                , { field: 'CreateTime', width: 150, sort: true, fixed: false, hide: false, title: '建立时间' }
                , { field: 'CreateBy', width: 90, sort: true, fixed: false, hide: false, title: '建立者' }
                , { field: 'UpdateTime', width: 150, sort: true, fixed: false, hide: false, title: '更新时间' }
                , { field: 'UpdateBy', width: 90, sort: true, fixed: false, hide: false, title: '更新者' }
            ]];

            mainList.Table = table.render({
                elem: '#mainList'
                , url: "/" + AreaName + "/" + TableName + "/Load"
                , method: "post"
                , page: true //开启分页
                , cols: hhweb.ColumnSetting('mainList', cols_arr)
                , id: 'mainList'
                , limit: 10
                , limits: [10, 50, 100, 200, 500, 1000]
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
          //  mainList.Table.reload({
          //      url: "/" + AreaName + "/" + TableName + "/Load"
          //      , method: "post"
         //   });
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
            if ($(selDom)[0].name.search("q") == -1) {
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
    var AreaNameDtlOrderDetiailHistory = 'wo';
    var TableNameDtlOrderDetiailHistory = 'OrderDetiailHistory';
    //{子表字段：主表字段}
    var NameDtlOrderDetiailHistory = { OrderCode: 'Code', OrderHeaderId: 'Id' };
    var vmDtlOrderDetiailHistory = new Vue({
        el: '#modifyFormDtl_' + TableNameDtlOrderDetiailHistory
    });
    var vmqDtlOrderDetiailHistory = new Vue({
        data: { DictType: '', HeaderId: '' }
    });
    //编辑
    var EditInfoDtlOrderDetiailHistory = function (tabledata) {
        data = tabledata;
        vmDtlOrderDetiailHistory.$set('$data', tabledata);
        var list = {};
        $('.ClearSelector_' + TableNameDtlOrderDetiailHistory).each(function () {
            var selDom = ($(this));
            if (!$(selDom)[0].name.startsWith("q")) {
                list[$(selDom)[0].name] = data[$(selDom)[0].name] + "";
            }
        });
        //表单修改时填充需修改的数据
        form.val('modifyFormDtl_' + TableNameDtlOrderDetiailHistory, list);
    };

    var mainListDtlOrderDetiailHistory = {
        Render: function () {
            var cols_arr = [[
                { checkbox: true, fixed: true }
                , { field: 'Id', width: 60, sort: true, fixed: false, hide: false, title: 'Id' }
                , { field: 'OrderCode', width: 120, sort: true, fixed: false, hide: false, title: '工单号' }
                , { field: 'OrderHeaderId', width: 100, sort: true, fixed: false, hide: false, title: '关联头表' }
                , { field: 'DrawingNumber', width: 150, sort: true, fixed: false, hide: false, title: '产品图号' }
                , { field: 'SerialNumber', width: 200, sort: true, fixed: false, hide: false, title: '序列号' }
                , { field: 'ExecuteStatus', width: 100, sort: true, fixed: false, hide: false, title: '执行状态', templet: function (d) { return GetLabel('ExecuteStatus', 'DictValue', 'DictLabel', d.ExecuteStatus) } }
                , { field: 'QualityStatus', width: 100, sort: true, fixed: false, hide: false, title: '质量状态', templet: function (d) { return GetLabel('QualityStatus', 'DictValue', 'DictLabel', d.QualityStatus) } }
                , { field: 'StationTraceId', width: 120, sort: true, fixed: false, hide: false, title: '跟踪工位' }
                , { field: 'StartTime', width: 150, sort: true, fixed: false, hide: false, title: '开始时间' }
                , { field: 'EndTime', width: 150, sort: true, fixed: false, hide: false, title: '结束时间' }
                , { field: 'CreateTime', width: 150, sort: true, fixed: false, hide: false, title: '建立时间' }
                , { field: 'CreateBy', width: 90, sort: true, fixed: false, hide: false, title: '建立者' }
                , { field: 'UpdateTime', width: 150, sort: true, fixed: false, hide: false, title: '更新时间' }
                , { field: 'UpdateBy', width: 90, sort: true, fixed: false, hide: false, title: '更新者' }
            ]];

            mainListDtlOrderDetiailHistory.Table = table.render({
                elem: '#mainListDtl' + TableNameDtlOrderDetiailHistory
                , url: "/" + AreaNameDtlOrderDetiailHistory + "/" + TableNameDtlOrderDetiailHistory + "/Load"
                , where: vmqDtlOrderDetiailHistory.$data
                , method: "post"
                , page: true //开启分页
                , cols: hhweb.ColumnSetting('mainListDtl' + TableNameDtlOrderDetiailHistory, cols_arr)
                , id: 'mainListDtl' + TableNameDtlOrderDetiailHistory
                , limit: 10
                , limits: [10, 50, 100, 200, 500, 1000]
                , defaultToolbar: ['filter']
                , toolbar: '#toolbarTable' + TableNameDtlOrderDetiailHistory
                , height: '455'
                , cellMinWidth: 80
                , size: 'sm'
                , done: function (res) { }
            });

            return mainListDtlOrderDetiailHistory.Table;
        },
        Load: function () {
            if (mainListDtlOrderDetiailHistory.Table == undefined) {
                mainListDtlOrderDetiailHistory.Table = this.Render();
                return;
            }
            table.reload('mainListDtl' + TableNameDtlOrderDetiailHistory, {
                url: "/" + AreaNameDtlOrderDetiailHistory + "/" + TableNameDtlOrderDetiailHistory + "/Load"
                , where: vmqDtlOrderDetiailHistory.$data
                , method: "post"
                , page: { curr: 1 }
            });
        }
    };
    All.push({ AreaNameDtl: AreaNameDtlOrderDetiailHistory, TableNameDtl: TableNameDtlOrderDetiailHistory, vmqDtl: vmqDtlOrderDetiailHistory, vmDtl: vmDtlOrderDetiailHistory, EditInfoDtl: EditInfoDtlOrderDetiailHistory, NameDtl: NameDtlOrderDetiailHistory, mainListDtl: mainListDtlOrderDetiailHistory });

    var selector = {
         'TProductCode': {
            SelType: "FromUrl",
            SelFrom: "/configure/ProductHeader/Load",
            SelModel: "TProductCode",
            SelLabel: "MachineType",
            SelValue: "Code"
        }, 'Status': {
            SelType: "FromDict",
            SelFrom: "WOStatus",
            SelModel: "Status",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='Status']"), $("[name='qStatus']")]
        }, 'Type': {
            SelType: "FromDict",
            SelFrom: "WOType",
            SelModel: "Type",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='Type']"), $("[name='qType']")]
        }, 'LineId': {
            SelType: "FromUrl",
            SelFrom: "/configure/Line/Load",
            SelModel: "LineId",
            SelLabel: "LineCode",
            SelValue: "Id",
            Dom: [$("[name='LineId']"), $("[name='qLineId']")]
        }, 'ExecuteStatus': {
            SelType: "FromDict",
            SelFrom: "ExecuteStatus",
            SelModel: "ExecuteStatus",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='ExecuteStatus']")]
        }, 'QualityStatus': {
            SelType: "FromDict",
            SelFrom: "QualityStatus",
            SelModel: "QualityStatus",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='QualityStatus']")]
        }, 'Priority': {
            SelType: "FromDict",
            SelFrom: "priority",
            SelModel: "Priority",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='Priority']"), $("[name='qPriority']")]
        }
    };
    
    var vml = new Array({
        vm: vm,
        vmq: vmq,
        vmDtlOrderDetiailHistory: vmDtlOrderDetiailHistory,
    });

    Universal.BindSelector(vml, selector);
    Universal.mmain(AreaName, TableName, vm, vmq, EditInfo, selfbtn, mainList);
    Universal.mainDtl(selfbtn, All);
});