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
    
    var AreaName = 'equipment';
    var TableName = 'Equipment';
    
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
                , {field:'Code', width:150, sort: true, fixed: false, hide: false, title: '设备编号' }
                , { field: 'Name', width: 150, sort: true, fixed: false, hide: false, title: '设备名称' }
                , { field: 'FactoryId', width: 150, sort: true, fixed: false, hide: false, title: '工厂', templet: function (d) { return GetLabel('FactoryId', 'Id', 'FactoryName', d.FactoryId) } }
                , { field: 'WorkshopId', width: 150, sort: true, fixed: false, hide: false, title: '车间', templet: function (d) { return GetLabel('WorkshopId', 'Id', 'Name', d.WorkshopId) } }
                , { field: 'LineCode', width: 150, sort: true, fixed: false, hide: false, title: '线体', templet: function (d) { return GetLabel('LineCode', 'LineCode', 'LineName', d.LineCode) } }
                , {field:'LineId', width:150, sort: true, fixed: false, hide: false, title: '线体ID' }
                , { field: 'StationCode', width: 150, sort: true, fixed: false, hide: false, title: '工位', templet: function (d) { return GetLabel('StationCode', 'Code', 'Name', d.StationCode) } }
                , {field:'StationId', width:150, sort: true, fixed: false, hide: false, title: '工位ID' }
                , {field:'Ip', width:150, sort: true, fixed: false, hide: false, title: '设备IP地址' }
                , { field: 'EquipmentTypeId', width: 150, sort: true, fixed: false, hide: false, title: '设备类型', templet: function (d) { return GetLabel('EquipmentTypeId', 'Id', 'Name', d.EquipmentTypeId) } }
                , { field: 'Enable', width: 150, sort: true, fixed: false, hide: false, title: '是否启用', templet: function (d) { return GetLabel('Enable', 'DictValue', 'DictLabel', d.Enable) } }
                , { field: 'RoadWay', width: 150, sort: true, fixed: false, hide: false, title: '巷道' }
                , { field: 'DestinationArea', width: 150, sort: true, fixed: false, hide: false, title: '目标区域' }
                , { field: 'GoAddress', width: 150, sort: true, fixed: false, hide: false, title: '目的地址' }
                , { field: 'SelfAddress', width: 150, sort: true, fixed: false, hide: false, title: '自身地址' }
                , { field: 'BackAddress', width: 150, sort: true, fixed: false, hide: false, title: '回退地址' }
                , { field: 'WarehouseCode', width: 150, sort: true, fixed: false, hide: false, title: '仓库编码' }
                , { field: 'StationIndex', width: 150, sort: true, fixed: false, hide: false, title: '站台编码' }
                , { field: 'RowIndex1', width: 150, sort: true, fixed: false, hide: false, title: '第一台堆垛机对应的排索引' }
                , { field: 'RowIndex2', width: 150, sort: true, fixed: false, hide: false, title: '第二台堆垛机对应的排索引' }
                , { field: 'ColumnIndex', width: 150, sort: true, fixed: false, hide: false, title: '列' }
                , { field: 'LayerIndex', width: 150, sort: true, fixed: false, hide: false, title: '层' }
                , { field: 'Transport1', width: 150, sort: true, fixed: false, hide: false, title: 'Transport1' }
                , { field: 'Transport2', width: 150, sort: true, fixed: false, hide: false, title: 'Transport2' }
                , { field: 'BasePlcDB', width: 150, sort: true, fixed: false, hide: false, title: 'PLC的DB地址' }
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
            if (!$(selDom)[0].name.startsWith("q")){
                list[$(selDom)[0].name] = data[$(selDom)[0].name] + "";
            }
        });
        console.log(list, ' ', data)
        //表单修改时填充需修改的数据
        form.val('modifyForm', list);
    };

    var selfbtn = {
        //自定义按钮
    };
    
    var selector = {
        'Enable': {
            SelType: "FromDict",
            SelFrom: "IsEnable",
            SelModel: "Enable",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='Enable']"), $("[name='qEnable']")]
        }, 'LineCode': {
            SelType: "FromUrl",
            SelFrom: "/configure/Line/Load",
            SelModel: "LineCode",
            SelLabel: "LineName",
            SelValue: "LineCode",
            Dom: [$("[name='LineCode']"), $("[name='qLineCode']")]
        }, 'FactoryId': {
            SelType: "FromUrl",
            SelFrom: "/configure/Factory/Load",
            SelModel: "FactoryId",
            SelLabel: "FactoryName",
            SelValue: "Id",
            Dom: [$("[name='FactoryId']"), $("[name='qFactoryId']")]
        }, 'WorkshopId': {
            SelType: "FromUrl",
            SelFrom: "/configure/Workshop/Load",
            SelModel: "WorkshopId",
            SelLabel: "Name",
            SelValue: "Id",
            Dom: [$("[name='WorkshopId']"), $("[name='qWorkshopId']")]
        }, 'StationCode': {
            SelType: "FromUrl",
            SelFrom: "/configure/Station/Load",
            SelModel: "StationCode",
            SelLabel: "Name",
            SelValue: "Code",
            Dom: [$("[name='StationCode']"), $("[name='qStationCode']")]
        }, 'EquipmentTypeId': {
            SelType: "FromUrl",
            SelFrom: "/equipment/EquipmentType/Load",
            SelModel: "EquipmentTypeId",
            SelLabel: "Name",
            SelValue: "Id",
            Dom: [$("[name='EquipmentTypeId']"), $("[name='qEquipmentTypeId']")]
        },
    };
    
    var vml = new Array({
        vm: vm,
        vmq: vmq,
    });
    
    Universal.BindSelector(vml, selector);
    Universal.mmain(AreaName, TableName, vm, vmq, EditInfo, selfbtn, mainList);
});