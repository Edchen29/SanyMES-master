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
    var TableName = 'Inventory';
    
    var vm = new Vue({
        el: '#modifyForm'
    });
    
    var vmq = new Vue({
        el: '#panelSearch',
        data: {
        }
    });
    
    hhweb.Config = {
        'ManufactureDate': vm,
        'ExpirationDate': vm,
        'CreateTime': vm,
        'UpdateTime': vm,

        'qManufactureDate': vmq,
        'qExpirationDate': vmq,
        'qCreateTime': vmq,
        'qUpdateTime': vmq,
    };
      
    var mainList = {
        Render: function () {
            var cols_arr = [[
                { checkbox: true, fixed: true }
                , {field:'Id', width:80, sort: true, fixed: false, hide: false, title: 'Id' }
                , {field:'FactoryId', width:150, sort: true, fixed: false, hide: false, title: '车间标识' }
                , {field:'FactoryCode', width:150, sort: true, fixed: false, hide: false, title: '车间代号' }
                , {field:'LineId', width:150, sort: true, fixed: false, hide: false, title: '线体标识' }
                , {field:'LineCode', width:150, sort: true, fixed: false, hide: false, title: '线体代号' }
                , {field:'WarehouseCode', width:150, sort: true, fixed: false, hide: false, title: '仓库类型' }
                , {field:'LocationId', width:150, sort: true, fixed: false, hide: false, title: '库位id' }
                , {field:'LocationCode', width:150, sort: true, fixed: false, hide: false, title: '库位编号' }
                , {field:'ContainerCode', width:150, sort: true, fixed: false, hide: false, title: '容器编码' }
                , {field:'SourceCode', width:150, sort: true, fixed: false, hide: false, title: '上游系统单号' }
                , {field:'MaterialId', width:150, sort: true, fixed: false, hide: false, title: '物料Id' }
                , {field:'MaterialCode', width:150, sort: true, fixed: false, hide: false, title: '物料编码' }
                , {field:'Batch', width:150, sort: true, fixed: false, hide: false, title: '批次' }
                , {field:'Lot', width:150, sort: true, fixed: false, hide: false, title: '批号' }
                , {field:'DrawingCode', width:150, sort: true, fixed: false, hide: false, title: '图号' }
                , {field:'ManufactureDate', width:150, sort: true, fixed: false, hide: false, title: '生产日期' }
                , {field:'ExpirationDate', width:150, sort: true, fixed: false, hide: false, title: '失效日期' }
                , {field:'Status', width:150, sort: true, fixed: false, hide: false, title: '库存状态' }
                , {field:'ContainerStatus', width:150, sort: true, fixed: false, hide: false, title: '容器状态' }
                , {field:'Qty', width:150, sort: true, fixed: false, hide: false, title: '数量' }
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
});