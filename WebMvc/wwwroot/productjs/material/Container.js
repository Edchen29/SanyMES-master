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
    var TableName = 'Container';
    
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
                , {field:'Code', width:150, sort: true, fixed: false, hide: false, title: '容器编码' }
                , { field: 'Type', width: 150, sort: true, fixed: false, hide: false, title: '容器类型', templet: function (d) { return GetLabel('Type', 'DictValue', 'DictLabel', d.Type) } }
                , { field: 'IsLock', width: 100, sort: true, fixed: false, hide: false, title: 'IsLock', templet: function (d) { return GetLabel('IsLock', 'DictValue', 'DictLabel', d.IsLock) } }
                , { field: 'LocationId', width: 150, sort: true, fixed: false, hide: true, title: '库位id' }
                , {field:'LocationCode', width:150, sort: true, fixed: false, hide: false, title: '库位编码' }
                , { field: 'Status', width: 100, sort: true, fixed: false, hide: false, title: '状态', templet: function (d) { return GetLabel('Status', 'DictValue', 'DictLabel', d.Status) } }
                , {field:'Height', width:100, sort: true, fixed: false, hide: false, title: '物料高度' }
                , {field:'Weight', width:100, sort: true, fixed: false, hide: false, title: '物料重量' }
                , {field:'PrintCount', width:100, sort: true, fixed: false, hide: false, title: '打印次数' }
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
        DomConfig: function (AddOrUpdate) {
            if (AddOrUpdate) {
                hhweb.DomEnable($("#modifyForm [name='Code']"));
                hhweb.DomEnable($("#modifyForm [name='IsLock']"));
                hhweb.DomEnable($("#modifyForm [name='PrintCount']"));
                hhweb.DomEnable($("#modifyForm [name='Type']"));
                hhweb.DomEnable($("#modifyForm [name='Status']"));
            }
            else {
                hhweb.DomDisable($("#modifyForm [name='Code']"));
                //hhweb.DomDisable($("#modifyForm [name='IsLock']"));
                hhweb.DomDisable($("#modifyForm [name='PrintCount']"));
                hhweb.DomDisable($("#modifyForm [name='Type']"));
                hhweb.DomDisable($("#modifyForm [name='Status']"));
            }
        },
        SaveBefore: function (AddOrEditOrDelete) {
            if (AddOrEditOrDelete in { Add: null, Edit: null }) {
                var rtn = hhweb.CheckRequired("#modifyForm", AddOrEditOrDelete);
                return rtn;
            }
        },
        BatchCreateContainer: function () {
            //重置from的所有数据
            document.getElementById("BtchForm").reset();
            layer.open({
                type: 1,
                //  skin: 'layui-layer-molv',
                moveType: 1, //拖拽模式，0或者1
                title: "批量创建", //不显示标题
                area: ["750px", "450px"], //宽高
                content: $('#BtchAdd'), //捕获的元素
                scrollbar: true,
                btn: ['创建', '关闭'],
                yes: function (index, layero) {
                    var BType = $("[name='BType']").val();           //容器类型
                    var Num = $("[name='Num']").val();             //总数
                    if (BType == "") {
                        layer.alert("容器类型不能为空!", { icon: 5, shadeClose: true, title: "错误信息" });
                        return null;
                    }
                    if (Num == "") {
                        layer.alert("总数不能为空!", { icon: 5, shadeClose: true, title: "错误信息" });
                        return null;
                    }
                    $.ajax({
                        url: "/material/Container/BtchAdd",
                        type: "POST",
                        data: {
                            Type: BType,
                            Num: Num,
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
            SelFrom: "containerStatus",
            SelModel: "Status",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='Status']"), $("[name='qStatus']")]
        },
        'Type': {
            SelType: "FromDict",
            SelFrom: "containerType",
            SelModel: "Type",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='Type']"), $("[name='qType']")]
        },
        'IsLock': {
            SelType: "FromDict",
            SelFrom: "IsLock",
            SelModel: "IsLock",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='IsLock']"), $("[name='qIsLock']")]
        },
        'BType': {
            SelType: "FromDict",
            SelFrom: "containerType",
            SelModel: "BType",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='BType']")]
        },
    };
    
    var vml = new Array({
        vm: vm,
        vmq: vmq,
    });
    
    Universal.BindSelector(vml, selector);
    Universal.mmain(AreaName, TableName, vm, vmq, EditInfo, selfbtn, mainList);
});