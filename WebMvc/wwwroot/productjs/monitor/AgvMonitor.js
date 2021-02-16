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
    
    var AreaName = 'monitor';
    var TableName = 'AgvMonitor';
    
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
                , {field:'Id', width:80, sort: true, fixed: false, hide: false, title: 'ID' }
                , {field:'CarNo', width:150, sort: true, fixed: false, hide: false, title: 'AGV编号' }
                , {field:'TaskNo', width:150, sort: true, fixed: false, hide: false, title: '任务号' }
                , { field: 'PercentCapacity', width: 100, sort: true, fixed: false, hide: false, title: '电量', templet: function (d) { if (d.PercentCapacity >= 60) { return '<b style="color:green">' + d.PercentCapacity + '%<a class="layui-icon layui-icon-face-smile" style="position:absolute;right:30px;color:green"></a></b>' } else if (d.PercentCapacity >= 30 && d.PercentCapacity < 60) { return '<b style="color:orange">' + d.PercentCapacity + '%<a class="layui-icon layui-icon-face-surprised" style="position:absolute;right:30px;color:orange"></a></b>' } else { return '<b style="color:red">' + d.PercentCapacity + '%<a class="layui-icon layui-icon-face-cry" style="position:absolute;right:30px;color:red"></a></b>' } } }
                , { field: 'ExceptionFlag', width: 120, sort: true, fixed: false, hide: false, title: '异常标志', templet: function (d) { return GetLabel('ExceptionFlag', 'DictValue', 'DictLabel', d.ExceptionFlag) } }
                , { field: 'State', width: 150, sort: true, fixed: false, hide: false, title: '状态', templet: function (d) { return GetLabel('State', 'DictValue', 'DictLabel', d.State) } }
                , {field:'ExceptionInfo', width:200, sort: true, fixed: false, hide: false, title: '异常信息' }
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
          //  mainList.Table.reload({
          //      url: "/" + AreaName + "/" + TableName + "/Load"
          //      , method: "post"
         //   });
        }
    };

    var iRefreshTime = 10;
    var iRefresh = 10;

    function AutoRefresh() {
        if ($("#autoRefresh").get(0).checked) {
            iRefresh--;
            $("#lblRefreshTime").html(iRefresh + "秒后自动刷新");

            if (iRefresh == 0) {
                table.reload('mainList', {});

                iRefresh = iRefreshTime;
            }
        };
    }

    $(document).ready(
        function () {
            $("#lblRefreshTime").html(iRefresh + "秒后自动刷新");
            setInterval(AutoRefresh, 1000);
        });
    
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
    
    var selector = {
        'State': {
            SelType: "FromDict",
            SelFrom: "AGVState",
            SelModel: "State",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='State']"), $("[name='qState']")]
        }, 'ExceptionFlag': {
            SelType: "FromDict",
            SelFrom: "ExceptionFlag",
            SelModel: "ExceptionFlag",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='ExceptionFlag']"), $("[name='qExceptionFlag']")]
    },
    };
    
    var vml = new Array({
        vm: vm,
        vmq: vmq,
    });
    
    Universal.BindSelector(vml, selector);
    Universal.mmain(AreaName, TableName, vm, vmq, EditInfo, selfbtn, mainList);
});