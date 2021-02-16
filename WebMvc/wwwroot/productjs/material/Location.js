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
    var TableName = 'Location';
    
    var vm = new Vue({
        el: '#modifyForm'
    });
    
    var vmq = new Vue({
        el: '#panelSearch',
        data: {
        }
    });
    
    hhweb.Config = {
        'LastCycleCountDate': vm,
        'CreateTime': vm,
        'UpdateTime': vm,

        'qLastCycleCountDate': vmq,
        'qCreateTime': vmq,
        'qUpdateTime': vmq,
    };
      
    var mainList = {
        Render: function () {
            var cols_arr = [[
                { checkbox: true, fixed: true }
                , {field:'Id', width:80, sort: true, fixed: false, hide: false, title: 'Id' }
                , {field:'Code', width:150, sort: true, fixed: false, hide: false, title: '库位' }
                , { field: 'LineCode', width: 150, sort: true, fixed: false, hide: false, title: '线体', templet: function (d) { return GetLabel('LineCode', 'LineCode', 'LineName', d.LineCode) } }
                , {field:'Row', width:70, sort: true, fixed: false, hide: false, title: '行' }
                , {field:'Column', width:70, sort: true, fixed: false, hide: false, title: '列' }
                , {field:'Layer', width:70, sort: true, fixed: false, hide: false, title: '层' }
                , {field:'Grid', width:70, sort: true, fixed: false, hide: false, title: '格' }
                , { field: 'RowIndex1', width: 100, sort: true, fixed: false, hide: false, title: '1号堆垛机双伸位索引' }
                , { field: 'RowIndex2', width: 100, sort: true, fixed: false, hide: false, title: '2号堆垛机双伸位索引' }
                , {field:'Roadway', width:80, sort: true, fixed: false, hide: false, title: '巷道' }
                , { field: 'Type', width: 150, sort: true, fixed: false, hide: false, title: '库位类型', templet: function (d) { return GetLabel('Type', 'DictValue', 'DictLabel', d.Type) } }
                , { field: 'ContainerId', width: 150, sort: true, fixed: false, hide: true, title: '容器id号' }
                , { field: 'ContainerCode', width: 150, sort: true, fixed: false, hide: false, title: '容器编码' }
                , { field: 'Status', width: 130, sort: true, fixed: false, hide: false, title: '状态', templet: function (d) { return GetLabel('Status', 'DictValue', 'DictLabel', d.Status) } }
                , { field: 'IsStop', width: 100, sort: true, fixed: false, hide: false, title: '是否禁用', templet: function (d) { return GetLabel('IsStop', 'DictValue', 'DictLabel', d.IsStop) } }
                , { field: 'IsLock', width: 150, sort: true, fixed: false, hide: false, title: '是否锁定' }
                , {field:'MaxHeight', width:100, sort: true, fixed: false, hide: false, title: '高度上限' }
                , {field:'MaxWeight', width:100, sort: true, fixed: false, hide: false, title: '重量上限' }
                , { field: 'GoodsNo', width: 150, sort: true, fixed: false, hide: false, title: 'GoodsNo' }
                , {field:'LastCycleCountDate', width:150, sort: true, fixed: false, hide: false, title: '上次盘点日期' }
                , {field:'CreateTime', width:150, sort: true, fixed: false, hide: false, title: '创建时间' }
                , {field:'CreateBy', width:100, sort: true, fixed: false, hide: false, title: '创建用户' }
                , {field:'UpdateTime', width:150, sort: true, fixed: false, hide: false, title: '更新时间' }
                , {field:'UpdateBy', width:100, sort: true, fixed: false, hide: false, title: '更新用户' }
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
        btnIsStopTrue: function () {
            var checkStatus = table.checkStatus('mainList');
            var count = checkStatus.data.length;//选中的行数
            var data = checkStatus.data; //获取选中行的数据
            if (count > 0) {
                $.ajax({
                    url: "/" + AreaName + "/" + TableName + "/IsStope",
                    type: "post",
                    data: { ids: data.map(function (e) { return e.Id; }), stop: true },
                    dataType: "json",
                    success: function (result) {
                        if (result.code == 200) {
                            layer.msg('禁用成功', { icon: 6, shade: 0.4, time: 1000 });
                            table.reload('mainList', {});//重载TABLE
                        }
                        else {
                            layer.alert("禁用失败:" + result.Message, { icon: 5, shadeClose: true, title: "错误信息" });
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        layer.alert(errorThrown, { icon: 2, title: '提示' });
                    }
                });
            }
            else
                layer.alert("请至少选择一条数据", { icon: 5, shadeClose: true, title: "错误信息" });
        },
        btnIsStopFalse: function () {
            var checkStatus = table.checkStatus('mainList');
            var count = checkStatus.data.length;//选中的行数
            var data = checkStatus.data; //获取选中行的数据
            if (count > 0) {
                $.ajax({
                    url: "/" + AreaName + "/" + TableName + "/IsStope",
                    type: "post",
                    data: { ids: data.map(function (e) { return e.Id; }), stop: false },
                    dataType: "json",
                    success: function (result) {
                        if (result.code == 200) {
                            layer.msg('解禁成功', { icon: 6, shade: 0.4, time: 1000 });
                            table.reload('mainList', {});//重载TABLE
                        }
                        else {
                            layer.alert("解禁失败:" + result.Message, { icon: 5, shadeClose: true, title: "错误信息" });
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        layer.alert(errorThrown, { icon: 2, title: '提示' });
                    }
                });
            }
            else
                layer.alert("请至少选择一条数据", { icon: 5, shadeClose: true, title: "错误信息" });
        },
        DomConfig: function (AddOrUpdate) {
            if (AddOrUpdate) {
                hhweb.DomEnable($("#modifyForm [name='Code']"));
                hhweb.DomEnable($("#modifyForm [name='Row']"));
                hhweb.DomEnable($("#modifyForm [name='Line']"));
                hhweb.DomEnable($("#modifyForm [name='Layer']"));
                hhweb.DomEnable($("#modifyForm [name='Roadway']"));
                hhweb.DomEnable($("#modifyForm [name='Type']"));
                hhweb.DomEnable($("#modifyForm [name='Status']"));
            }
            else {
                hhweb.DomDisable($("#modifyForm [name='Code']"));
                hhweb.DomDisable($("#modifyForm [name='Row']"));
                hhweb.DomDisable($("#modifyForm [name='Line']"));
                hhweb.DomDisable($("#modifyForm [name='Layer']"));
                hhweb.DomDisable($("#modifyForm [name='Roadway']"));
                hhweb.DomDisable($("#modifyForm [name='Type']"));
                hhweb.DomDisable($("#modifyForm [name='Status']"));
            }
        },
        DomConfig_Inventory: function (AddOrUpdate) {
            if (AddOrUpdate) {
                hhweb.DomEnable($("#modifyFormDtl_Inventory [name='ContainerCode']"));
                hhweb.DomEnable($("#modifyFormDtl_Inventory [name='ContainerCode']"));
            }
            else {

            }
        },
        SaveBefore: function (AddOrEditOrDelete) {
            if (AddOrEditOrDelete in { Add: null, Edit: null }) {
                var rtn = hhweb.CheckRequired("#modifyForm", AddOrEditOrDelete);
                return rtn;
            }
        },
        SaveBefore_Inventory: function (AddOrEditOrDelete) {
            if (AddOrEditOrDelete in { Add: null, Edit: null }) {
                var rtn = hhweb.CheckRequired("#modifyFormDtl_Inventory", AddOrEditOrDelete);
                return rtn;
            }
        },
        BatchCreateLocation: function () {
            //重置from的所有数据
            document.getElementById("BtchForm").reset();
            layer.open({
                type: 1,
                //  skin: 'layui-layer-molv',
                moveType: 1, //拖拽模式，0或者1
                title: "批量创建", //不显示标题
                area: ["750px", "450px"], //宽高
                content: $('#TypeBtch'), //捕获的元素
                scrollbar: true,
                btn: ['创建', '关闭'],
                yes: function (index, layero) {
                    var BLine = $("#BLine").val();           //生产线体
                    var Type = $("#Type").val();           //库位类型
                    var Row = $("#Row").val();             //总行
                    var Line = $("#Line").val();           //总列
                    var Layer = $("#Layer").val();         //总层
                    var Grid = 0;
                    var Roadway = $("#Roadway").val();     //巷道
                    var MaxHeight = $("#MaxHeight").val();  //最大高度
                    var MaxWeight = $("#MaxWeight").val();  //最大重量
                    if (BLine == "") {
                        layer.alert("线体不能为空!", { icon: 5, shadeClose: true, title: "错误信息" });
                        return null;
                    }
                    if (Type == "") {
                        layer.alert("库位类型不能为空!", { icon: 5, shadeClose: true, title: "错误信息" });
                        return null;
                    }
                    if (Row == "") {
                        layer.alert("总行数不能为空!", { icon: 5, shadeClose: true, title: "错误信息" });
                        return null;
                    }
                    if (Line == "") {
                        layer.alert("总列数不能为空!", { icon: 5, shadeClose: true, title: "错误信息" });
                        return null;
                    }
                    if (Layer == "") {
                        layer.alert("总层数不能为空!", { icon: 5, shadeClose: true, title: "错误信息" });
                        return null;
                    }
                    if (Roadway == "") {
                        layer.alert("巷道不能为空!", { icon: 5, shadeClose: true, title: "错误信息" });
                        return null;
                    }
                    $.ajax({
                        url: "/material/Location/BtchAdd",
                        type: "POST",
                        data: {
                            BLine: BLine,
                            Type: Type,
                            Row: Row,
                            Line: Line,
                            Layer: Layer,
                            Grid: 0,
                            Roadway: Roadway,
                            Height: MaxHeight,
                            Weight: MaxWeight
                        },
                        dataType: "json",
                        success: function (result) {
                            if (result.status) {
                                layer.msg("创建成功!", { icon: 6, shade: 0.4, time: 1000 });
                                layer.close(index);
                                table.reload('mainList', {});

                            } else {
                                layer.alert("创建失败:" + result.Message, { icon: 5, shadeClose: true, title: "错误信息" });
                            }
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            layer.alert(errorThrown, { icon: 2, title: '提示' });
                        }
                    });
                },
                cancel: function (index) {
                    layer.close(index);
                }
            });
        }
    };
    
    var selector = {
        'Status': {
            SelType: "FromDict",
            SelFrom: "locationStatus",
            SelModel: "Status",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='Status']"), $("[name='qStatus']")]
        }, 'ContainerStatus': {
            SelType: "FromDict",
            SelFrom: "containerStatus",
            SelModel: "ContainerStatus",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='ContainerStatus']")]
        },
        'IsStop': {
            SelType: "FromDict",
            SelFrom: "IsStop",
            SelModel: "IsStop",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='IsStop']"), $("[name='qIsStop']")]
        },'Type': {
            SelType: "FromDict",
            SelFrom: "warehouseType",
            SelModel: "Type",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='Type']"), $("[name='qType']")]
        }, 'LineCode': {
            SelType: "FromUrl",
            SelFrom: "/configure/Line/Load",
            SelModel: "LineCode",
            SelLabel: "LineName",
            SelValue: "LineCode",
            Dom: [$("[name='LineCode']"), $("[name='qLineCode']")]
        }, 'BLine': {
            SelType: "FromUrl",
            SelFrom: "/configure/Line/Load",
            SelModel: "BLine",
            SelLabel: "LineName",
            SelValue: "LineCode",
            Dom: [$("[name='BLine']")]
        },
    };
    
    var vml = new Array({
        vm: vm,
        vmq: vmq,
    });
    
    Universal.BindSelector(vml, selector);
    Universal.mmain(AreaName, TableName, vm, vmq, EditInfo, selfbtn, mainList);
});