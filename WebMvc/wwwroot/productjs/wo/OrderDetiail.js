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
    var TableName = 'OrderDetiail';
    
    var vm = new Vue({
        el: '#modifyForm'
    });
    
    var vmq = new Vue({
        el: '#panelSearch',
        data: {
        }
    });
    
    hhweb.Config = {
        'StartTime': vm,
        'EndTime': vm,
        'CreateTime': vm,
        'UpdateTime': vm,

        'qStartTime': vmq,
        'qEndTime': vmq,
        'qCreateTime': vmq,
        'qUpdateTime': vmq,
    };
      
    var mainList = {
        Render: function () {
            var cols_arr = [[
                { checkbox: true, fixed: true }
                , {field:'Id', width:150, sort: true, fixed: false, hide: false, title: 'Id' }
                , {field:'OrderCode', width:150, sort: true, fixed: false, hide: false, title: '工单号' }
                , {field:'OrderHeaderId', width:150, sort: true, fixed: false, hide: false, title: '关联头表' }
                , { field:'DrawingNumber', width:150, sort: true, fixed: false, hide: false, title: '产品图号' }
                , {field:'SerialNumber', width:150, sort: true, fixed: false, hide: false, title: '序列号' }
                , {field:'ExecuteStatus', width:150, sort: true, fixed: false, hide: false, title: '执行状态' }
                , {field:'QualityStatus', width:150, sort: true, fixed: false, hide: false, title: '质量状态' }
                , {field:'StationTraceId', width:150, sort: true, fixed: false, hide: false, title: '跟踪工位' }
                , {field:'StartTime', width:150, sort: true, fixed: false, hide: false, title: '开始时间' }
                , {field:'EndTime', width:150, sort: true, fixed: false, hide: false, title: '结束时间' }
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
            mainList.Table.reload({
                url: "/" + AreaName + "/" + TableName + "/Load"
                , method: "post"
            });
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