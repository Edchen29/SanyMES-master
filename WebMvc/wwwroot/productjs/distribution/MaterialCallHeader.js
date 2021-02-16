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
    
    var AreaName = 'distribution';
    var TableName = 'MaterialCallHeader';
    
    var vm = new Vue({
        el: '#modifyForm'
    });
    
    var vmq = new Vue({
        el: '#panelSearch',
        data: {
        }
    });
    
    hhweb.Config = {
        'CallTime': vm,
        'CreateTime': vm,
        'UpdateTime': vm,

        'qCallTime': vmq,
        'qCreateTime': vmq,
        'qUpdateTime': vmq,
    };
      
    var mainList = {
        Render: function () {
            var cols_arr = [[
                { checkbox: true, fixed: true }
                , { field: 'Id', width: 80, sort: true, fixed: false, hide: false, title: 'Id' }
                , { field: 'LineId', width: 150, sort: true, fixed: false, hide: true, title: '线体ID' }
                , { field: 'LineCode', width: 150, sort: true, fixed: false, hide: false, title: '线别', templet: function (d) { return GetLabel('LineCode', 'LineCode', 'LineName', d.LineCode) } }
                , { field: 'NeedStation', width: 150, sort: true, fixed: false, hide: false, event: 'MaterialCallDetail', templet: '#ShowDtlNeedStation', title: '需求工位代号' }
                , { field: 'NeedStation', width: 150, sort: true, fixed: false, hide: false, title: '需求工位名称', templet: function (d) { return GetLabel('NeedStation', 'Code', 'Name', d.NeedStation) } }
                , { field: 'LocationCode', width: 150, sort: true, fixed: false, hide: false, title: '需求位置' }
                , { field: 'CallType', width: 150, sort: true, fixed: false, hide: false, title: '呼叫类型', templet: function (d) { return GetLabel('CallType', 'DictValue', 'DictLabel', d.CallType) }  }
                , { field: 'StartPlace', width: 150, sort: true, fixed: false, hide: false, title: '起始位置' }
                , { field: 'EndPlace', width: 150, sort: true, fixed: false, hide: false, title: '目的位置' }
                , { field: 'CallTime', width: 150, sort: true, fixed: false, hide: false, title: '叫料时间' }
                , { field: 'Status', width: 130, sort: true, fixed: false, hide: false, title: '状态', templet: function (d) { return GetLabel('Status', 'DictValue', 'DictLabel', d.Status) } }
                , { field: 'Mode', width: 130, sort: true, fixed: false, hide: false, title: '模式', templet: function (d) { return GetLabel('Mode', 'DictValue', 'DictLabel', d.Mode) } }
                , { field: 'FromPlatform', width: 150, sort: true, fixed: false, hide: false, title: '来源平台', templet: function (d) { return GetLabel('FromPlatform', 'DictValue', 'DictLabel', d.FromPlatform) } }
                , {field:'UserCode', width:120, sort: true, fixed: false, hide: false, title: '叫料操作员' }
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
        CreateDistributeTask: function () {
            var checkStatus = table.checkStatus('mainList');
            var count = checkStatus.data.length;//选中的行数
            if (count > 0) {
                var data = checkStatus.data; //获取选中行的数据
                layer.confirm('确定生成配送任务？', {
                    btn: ['确定', '取消']
                }, function () {
                    $.ajax({
                        url: "/distribution/MaterialCallHeader/CreateDistributeTask",
                        type: "post",
                        data: { materialcallheaderlist: data },
                        dataType: "json",
                        // async: false,
                        success: function (result) {
                            if (result.code = 200) {
                                layer.alert(result.msg + "请在【物料配送任务】页面查看！", { icon: 1, shade: 0.4, title: "信息提示" });
                            } else {
                                layer.alert('物料配送任务生成失败：' + result.msg, { icon: 5, shadeClose: true, title: "错误信息" });
                            }
                        },
                        error: function (error) {
                            layer.alert(error.responseText, { icon: 2, title: '提示' });
                        }
                    })

                }, function () {
                    layer.close();
                });
            }
            else {
                layer.alert("请最少选中一条需要生成的数据！", { icon: 5, shadeClose: true, title: "错误信息" });
            }
        },
    };
    //子表逻辑
    var All = new Array();
    var AreaNameDtlMaterialCallDetail = 'distribution';
    var TableNameDtlMaterialCallDetail = 'MaterialCallDetail';
    //{子表字段：主表字段}
    var NameDtlMaterialCallDetail = { CallHeaderId: 'Id' };
    var vmDtlMaterialCallDetail = new Vue({
        el: '#modifyFormDtl_' + TableNameDtlMaterialCallDetail
    });
    var vmqDtlMaterialCallDetail = new Vue({
        data: { DictType: '', HeaderId: '' }
    });
    //编辑
    var EditInfoDtlMaterialCallDetail = function (tabledata) {
        data = tabledata;
        vmDtlMaterialCallDetail.$set('$data', tabledata);
        var list = {};
        $('.ClearSelector_' + TableNameDtlMaterialCallDetail).each(function () {
            var selDom = ($(this));
            if (!$(selDom)[0].name.startsWith("q")) {
                list[$(selDom)[0].name] = data[$(selDom)[0].name] + "";
            }
        });
        //表单修改时填充需修改的数据
        form.val('modifyFormDtl_' + TableNameDtlMaterialCallDetail, list);
    };

    var mainListDtlMaterialCallDetail = {
        Render: function () {
            var cols_arr = [[
                { checkbox: true, fixed: true }
                , { field: 'Id', width: 150, sort: true, fixed: false, hide: false, title: 'Id' }
                , { field: 'CallHeaderId', width: 150, sort: true, fixed: false, hide: false, title: '主表标识' }
                , { field: 'OrderCode', width: 150, sort: true, fixed: false, hide: false, title: '订单号' }
                , { field: 'ProductId', width: 150, sort: true, fixed: false, hide: true, title: '产品ID' }
                , { field: 'ProductCode', width: 150, sort: true, fixed: false, hide: false, title: '产品代号' }
                , { field: 'SerialNumber', width: 150, sort: true, fixed: false, hide: false, title: '序列号' }
                , { field: 'MachineType', width: 150, sort: true, fixed: false, hide: false, title: '机型' }
                , { field: 'Used', width: 150, sort: true, fixed: false, hide: false, title: '是否已用', templet: function (d) { return GetLabel('Used', 'DictValue', 'DictLabel', d.Used) } }
                , { field: 'CreateTime', width: 150, sort: true, fixed: false, hide: false, title: '建立时间' }
                , { field: 'CreateBy', width: 150, sort: true, fixed: false, hide: false, title: '建立者' }
                , { field: 'UpdateTime', width: 150, sort: true, fixed: false, hide: false, title: '更新时间' }
                , { field: 'UpdateBy', width: 150, sort: true, fixed: false, hide: false, title: '更新者' }
            ]];

            mainListDtlMaterialCallDetail.Table = table.render({
                elem: '#mainListDtl' + TableNameDtlMaterialCallDetail
                , url: "/" + AreaNameDtlMaterialCallDetail + "/" + TableNameDtlMaterialCallDetail + "/Load"
                , where: vmqDtlMaterialCallDetail.$data
                , method: "post"
                , page: true //开启分页
                , cols: hhweb.ColumnSetting('mainListDtl' + TableNameDtlMaterialCallDetail, cols_arr)
                , id: 'mainListDtl' + TableNameDtlMaterialCallDetail
                , limit: 10
                , limits: [10, 50, 100, 200, 500, 1000]
                , defaultToolbar: ['filter']
                , toolbar: '#toolbarTable' + TableNameDtlMaterialCallDetail
                , height: '350'
                , cellMinWidth: 80
                , size: 'sm'
                , done: function (res) { }
            });

            return mainListDtlMaterialCallDetail.Table;
        },
        Load: function () {
            if (mainListDtlMaterialCallDetail.Table == undefined) {
                mainListDtlMaterialCallDetail.Table = this.Render();
                return;
            }
            table.reload('mainListDtl' + TableNameDtlMaterialCallDetail, {
                url: "/" + AreaNameDtlMaterialCallDetail + "/" + TableNameDtlMaterialCallDetail + "/Load"
                , where: vmqDtlMaterialCallDetail.$data
                , method: "post"
                , page: { curr: 1 }
            });
        }
    };
    All.push({ AreaNameDtl: AreaNameDtlMaterialCallDetail, TableNameDtl: TableNameDtlMaterialCallDetail, vmqDtl: vmqDtlMaterialCallDetail, vmDtl: vmDtlMaterialCallDetail, EditInfoDtl: EditInfoDtlMaterialCallDetail, NameDtl: NameDtlMaterialCallDetail, mainListDtl: mainListDtlMaterialCallDetail });

    var selector = {
        'Mode': {
            SelType: "FromDict",
            SelFrom: "CallMode",
            SelModel: "Mode",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='Mode']"), $("[name='qMode']")]
        }, 'FromPlatform': {
            SelType: "FromDict",
            SelFrom: "fromPlatform",
            SelModel: "FromPlatform",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='FromPlatform']"), $("[name='qFromPlatform']")]
        }, 'LineCode': {
            SelType: "FromUrl",
            SelFrom: "/configure/Line/Load",
            SelModel: "LineCode",
            SelLabel: "LineName",
            SelValue: "LineCode",
            Dom: [$("[name='LineCode']"), $("[name='qLineCode']")]
        }, 'NeedStation': {
            SelType: "FromUrl",
            SelFrom: "/configure/Station/Load",
            SelModel: "NeedStation",
            SelLabel: "Name",
            SelValue: "Code",
            Dom: [$("[name='NeedStation']"), $("[name='qNeedStation']")]
        }, 'Status': {
            SelType: "FromDict",
            SelFrom: "DemandStatus",
            SelModel: "Status",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='Status']"), $("[name='qStatus']")]
        }, 'Used': {
            SelType: "FromDict",
            SelFrom: "sys_true_false",
            SelModel: "Used",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='Used']")]
        }, 'CallType': {
            SelType: "FromDict",
            SelFrom: "CallType",
            SelModel: "CallType",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='CallType']"), $("[name='qCallType']")]
        },
    };
    
    var vml = new Array({
        vm: vm,
        vmq: vmq,
        vmDtlMaterialCallDetail: vmDtlMaterialCallDetail,
    });
    
    Universal.BindSelector(vml, selector);
    Universal.mmain(AreaName, TableName, vm, vmq, EditInfo, selfbtn, mainList);
    Universal.mainDtl(selfbtn, All);
});