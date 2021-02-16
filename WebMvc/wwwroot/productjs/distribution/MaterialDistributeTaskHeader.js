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
    var TableName = 'MaterialDistributeTaskHeader';
    
    var vm = new Vue({
        el: '#modifyForm'
    });
    var vmc = new Vue({
        el: '#confirmForm'
    });
    var vmq = new Vue({
        el: '#panelSearch',
        data: {
        }
    });
    
    hhweb.Config = {
        'NeedTime': vm,
        'ResponseTime': vm,
        'FinishTime': vm,
        'CreateTime': vm,
        'UpdateTime': vm,

        'qNeedTime': vmq,
        'qResponseTime': vmq,
        'qFinishTime': vmq,
        'qCreateTime': vmq,
        'qUpdateTime': vmq,
    };
      
    var mainList = {
        Render: function () {
            var cols_arr = [[
                { checkbox: true, fixed: true }
                , {field: 'Id', width: 80, sort: true, fixed: false, hide: false, title: 'Id' }
                , { field: 'TaskNo', width: 150, sort: true, fixed: false, hide: false, event: 'MaterialDistributeTaskDetail', templet: '#ShowDtlTaskNo', title: '任务号' }
                , { field: 'MaterialCallId', width: 100, sort: true, fixed: false, hide: false, title: '叫料标识' }
                , { field: 'ProductCode', width: 150, sort: true, fixed: false, hide: false, title: '产品代号' }
                , {field:'CarNo', width:140, sort: true, fixed: false, hide: false, title: '小车编号' }
                , { field: 'ContainerCode', width: 130, sort: true, fixed: false, hide: false, title: '料框编码' }
                , { field: 'ContainerType', width: 100, sort: true, fixed: false, hide: false, title: '料框类型' }
                , { field: 'NeedStation', width: 140, sort: true, fixed: false, hide: false, title: '需求工位', templet: function (d) { return GetLabel('NeedStation', 'Code', 'Name', d.NeedStation) } }
                , { field: 'LocationCode', width: 100, sort: true, fixed: false, hide: false, title: '需求位置' }
                , { field: 'CallType', width: 110, sort: true, fixed: false, hide: false, title: '任务类型', templet: function (d) { return GetLabel('CallType', 'DictValue', 'DictLabel', d.CallType) } }
                , { field: 'StartPlace', width: 110, sort: true, fixed: false, hide: false, title: '起始位置' }
                , { field: 'EndPlace', width: 110, sort: true, fixed: false, hide: false, title: '目的位置' }
                , { field: 'Status', width: 150, sort: true, fixed: false, hide: false, title: '状态', templet: function (d) { return GetLabel('Status', 'DictValue', 'DictLabel', d.Status) } }
                , {field:'NeedTime', width:150, sort: true, fixed: false, hide: false, title: '需求时间' }
                , { field: 'ResponseTime', width: 150, sort: true, fixed: false, hide: false, title: '响应需求时间' }
                , { field: 'FinishTime', width: 150, sort: true, fixed: false, hide: false, title: '结束时间' }
                , { field: 'UserCode', width: 110, sort: true, fixed: false, hide: false, title: '配送操作员' }
                , { field: 'MaterialConfirm', width: 150, sort: true, fixed: false, hide: false, title: '备料确认', templet: function (d) { return GetLabel('MaterialConfirm', 'DictValue', 'DictLabel', d.MaterialConfirm) }}
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
        LoginTest: function () {
            //
            $.ajax({
                url: "/distribution/MaterialDistributeTaskHeader/LoginTest",
                type: "post",
                data: { url: "http://localhost:23512/api/Login/Login" },
                dataType: "json",
                // async: false,
                success: function (result) {
                    if (result.code = 200) {
                        layer.alert(result.msg, { icon: 1, shade: 0.4, title: "信息提示" });
                    } else {
                        layer.alert('工单配料清单生成失败：' + result.msg, { icon: 5, shadeClose: true, title: "错误信息" });
                    }
                },
                error: function (error) {
                    layer.alert(error.responseText, { icon: 2, title: '提示' });
                }
            })
        },
        SendAGVTask: function () {
            var checkStatus = table.checkStatus('mainList');
            var count = checkStatus.data.length;//选中的行数
            if (count > 0) {
                var data = checkStatus.data; //获取选中行的数据
                layer.confirm('确定下发AGV配送任务？', {
                    btn: ['确定', '取消']
                }, function () {
                    //自动分配数据写入任务表
                    $.ajax({
                        url: "/distribution/MaterialDistributeTaskHeader/SendAGVTaskAPI",
                        type: "post",
                        data: { senddata: data, url:"http://localhost:23512/api/AGVInfo/AGVTaskTest" },
                        dataType: "json",
                        // async: false,
                        success: function (result) {
                            if (result.code = 200) {
                                layer.alert(result.msg, { icon: 1, shade: 0.4, title: "信息提示" });
                                table.reload('mainList', {});
                            } else {
                                layer.alert('工单配料清单生成失败：' + result.msg, { icon: 5, shadeClose: true, title: "错误信息" });
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
                layer.alert("请最少选中一条需要配料的工单！", { icon: 5, shadeClose: true, title: "错误信息" });
            }
        }, TaskCancel: function () {
            var checkStatus = table.checkStatus('mainList');
            var count = checkStatus.data.length;//选中的行数
            var data = checkStatus.data;
            if (count > 0) {
                layer.confirm('确定要取消所选任务', { icon: 3 }, function (index) {
                    //取消所选任务
                    $.ajax({
                        url: "/distribution/MaterialDistributeTaskHeader/TaskCancel",
                        type: "post",
                        data: { mdthlist: data },
                        dataType: "json",
                        // async: false,
                        success: function (result) {
                            if (result.code = 200) {
                                layer.alert(result.msg, { icon: 1, shade: 0.4, title: "信息提示" });
                                table.reload('mainList', {});
                            } else {
                                layer.msg('任务取消失败：' + result.msg, { icon: 7, shade: 0.4, time: 3000 });
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
            else
                layer.alert("请至少选择一条数据", { icon: 5, shadeClose: true, title: "错误信息" });
        },btnMaterialConfirm: function () {
            var checkStatus = table.checkStatus('mainList');
            var count = checkStatus.data.length;//选中的行数
            var data = checkStatus.data;
            if (count == 1) {
                if ((data[0].ContainerType == "A" || data[0].ContainerType == "B") && (data[0].ProductCode != "" && data[0].ProductCode != null) && data[0].MaterialConfirm !=2) {
                    vmc.$set('$data', data);
                    $("[name='CProductCode']").val(data[0].ProductCode);
                    $("[name='CNeedStation']").val(data[0].NeedStation);
                    //$("[name='CLocationCode']").val(data[0].LocationCode);
                    $("[name='CNeedTime']").val(data[0].NeedTime);
                    $("[name='CContainerType']").val(data[0].ContainerType);
                    form.val('confirmForm',
                        {
                            //'CProductCode': data[0].ProductCode,
                            //'CNeedStation': data[0].NeedStation,
                            //'CLocationCode': data[0].LocationCode,
                            //'CNeedTime': data[0].NeedTime,
                            //'CContainerType': data[0].ContainerType,
                        });
                    $.ajax({
                        url: "/distribution/MaterialDistributeTaskDetail/Load",
                        type: "post",
                        data: { entity: { 'MaterialDistributeTaskHeaderId': data[0].Id } },
                        dataType: "json",
                        // async: false,
                        success: function (result) {
                            table.render({
                                elem: '#confirmdetail'
                                , cols: [[ //标题栏
                                    { field: 'Id', width: 65, sort: true, title: 'Id' }
                                    , { field: 'OrderCode', width: 150, sort: true, title: '工单号' }
                                    , { field: 'SerialNumber', width: 150, title: '生产序号' }
                                    , { field: 'MaterialCode', width: 120, sort: true, title: '料号' }
                                    , { field: 'Qty', width: 97, edit: 'text', title: '数量' }
                                ]]
                                , data: result.data
                                , even: true
                                , page: true //是否显示分页
                                , limits: [10, 20, 50,100]
                                , limit: 10 //每页默认显示的数量
                            });
                        },
                        error: function (error) {
                            layer.alert(error.responseText, { icon: 2, title: '提示' });
                        }
                    })
                    //监听单元格编辑
                    table.on('edit(confirmdetail)', function (obj) {
                        var value = obj.value //得到修改后的值
                            , data = obj.data //得到所在行所有键值
                            , field = obj.field; //得到字段
                        //layer.msg('[ID: ' + data.Id + '] ' + field + ' 字段更改为：' + value);
                        $.ajax({
                            url: "/distribution/MaterialDistributeTaskHeader/UpdateDetail",
                            type: "post",
                            data: { entity: obj.data },
                            dataType: "json",
                            success: function (result) {
                                if (result.code = 200) {
                                    layer.msg(result.msg, { icon: 1});
                                } else {
                                    layer.alert('操作失败：' + result.msg, { icon: 5, shadeClose: true, title: "错误信息" });
                                }
                            },
                            error: function (error) {
                                layer.alert(error.responseText, { icon: 2, title: '提示' });
                            }
                        })

                    });
                    layer.open({
                        type: 1,
                        area: ['590px', '660px'], //宽高
                        maxmin: true, //开启最大化最小化按钮
                        title: '配送前备料确认',
                        content: $('#confirmForm'),
                        btn: ['确认', '取消'],
                        yes: function (index, layero) {
                            $.ajax({
                                url: "/distribution/MaterialDistributeTaskHeader/MaterialConfirm",
                                type: "post",
                                // data: { entity: { 'MaterialDistributeTaskHeaderId': data[0].Id } },
                                data: { 'headerid': data[0].Id, containercode: $("[name='CContainerCode']").val(), stockplace: $("[name='CStockPlace']").val()},
                                dataType: "json",
                                // async: false,
                                success: function (result) {
                                    if (result.code = 200) {
                                        layer.msg(result.msg, { icon: 1, shade: 0.4, time: 2000 });
                                        table.reload('mainList', {});
                                    } else {
                                        layer.alert('操作失败：' + result.msg, { icon: 5, shadeClose: true, title: "错误信息" });
                                    }
                                },
                                error: function (error) {
                                    layer.alert(error.responseText, { icon: 2, title: '提示' });
                                }
                            })
                            layer.close(index);
                        },
                        cancel: function (index) {
                            layer.close(index);
                        }
                    });
                } else {
                    layer.msg("无需确认或已确认过！", { icon: 5, shade: 0.4, time: 2000});
                }
            }
            else
                layer.msg("请选择一条数据", { icon: 5, shadeClose: true, title: "错误信息" });
        },
    };
    //子表逻辑
    var All = new Array();
    var AreaNameDtlMaterialDistributeTaskDetail = 'distribution';
    var TableNameDtlMaterialDistributeTaskDetail = 'MaterialDistributeTaskDetail';
    //{子表字段：主表字段}
    var NameDtlMaterialDistributeTaskDetail = { MaterialDistributeTaskHeaderId: 'Id'};
    var vmDtlMaterialDistributeTaskDetail = new Vue({
        el: '#modifyFormDtl_' + TableNameDtlMaterialDistributeTaskDetail
    });
    var vmqDtlMaterialDistributeTaskDetail = new Vue({
        data: { DictType: '', HeaderId: '' }
    });
    //编辑
    var EditInfoDtlMaterialDistributeTaskDetail = function (tabledata) {
        data = tabledata;
        vmDtlMaterialDistributeTaskDetail.$set('$data', tabledata);
        var list = {};
        $('.ClearSelector_' + TableNameDtlMaterialDistributeTaskDetail).each(function () {
            var selDom = ($(this));
            if (!$(selDom)[0].name.startsWith("q")) {
                list[$(selDom)[0].name] = data[$(selDom)[0].name] + "";
            }
        });
        //表单修改时填充需修改的数据
        form.val('modifyFormDtl_' + TableNameDtlMaterialDistributeTaskDetail, list);
    };

    var mainListDtlMaterialDistributeTaskDetail = {
        Render: function () {
            var cols_arr = [[
                { checkbox: true, fixed: true }
                , { field: 'Id', width: 80, sort: true, fixed: false, hide: false, title: 'Id' }
                , { field: 'MaterialDistributeTaskHeaderId', width: 100, sort: true, fixed: false, hide: false, title: '头表标识' }
                , { field: 'OrderCode', width: 150, sort: true, fixed: false, hide: false, title: '订单编号' }
                , { field: 'ContainerCode', width: 150, sort: true, fixed: false, hide: false, title: '容器编码【料框编码】' }
                , { field: 'MaterialCode', width: 150, sort: true, fixed: false, hide: false, title: '物料编码' }
                , { field: 'SerialNumber', width: 180, sort: true, fixed: false, hide: false, title: '序列号' }
                , { field: 'Qty', width: 80, sort: true, fixed: false, hide: false, title: '数量' }
                , { field: 'UserCode', width: 110, sort: true, fixed: false, hide: false, title: '配盘操作员' }
                , { field: 'CreateTime', width: 150, sort: true, fixed: false, hide: false, title: '建立时间' }
                , { field: 'CreateBy', width: 100, sort: true, fixed: false, hide: false, title: '建立者' }
                , { field: 'UpdateTime', width: 150, sort: true, fixed: false, hide: false, title: '更新时间' }
                , { field: 'UpdateBy', width: 100, sort: true, fixed: false, hide: false, title: '更新者' }
            ]];

            mainListDtlMaterialDistributeTaskDetail.Table = table.render({
                elem: '#mainListDtl' + TableNameDtlMaterialDistributeTaskDetail
                , url: "/" + AreaNameDtlMaterialDistributeTaskDetail + "/" + TableNameDtlMaterialDistributeTaskDetail + "/Load"
                , where: vmqDtlMaterialDistributeTaskDetail.$data
                , method: "post"
                , page: true //开启分页
                , cols: hhweb.ColumnSetting('mainListDtl' + TableNameDtlMaterialDistributeTaskDetail, cols_arr)
                , id: 'mainListDtl' + TableNameDtlMaterialDistributeTaskDetail
                , limit: 8
                , limits: [8, 50, 100, 200, 500, 1000]
                , defaultToolbar: ['filter']
                , toolbar: '#toolbarTable' + TableNameDtlMaterialDistributeTaskDetail
                , height: '350'
                , cellMinWidth: 80
                , size: 'sm'
                , done: function (res) { }
            });

            return mainListDtlMaterialDistributeTaskDetail.Table;
        },
        Load: function () {
            if (mainListDtlMaterialDistributeTaskDetail.Table == undefined) {
                mainListDtlMaterialDistributeTaskDetail.Table = this.Render();
                return;
            }
            table.reload('mainListDtl' + TableNameDtlMaterialDistributeTaskDetail, {
                url: "/" + AreaNameDtlMaterialDistributeTaskDetail + "/" + TableNameDtlMaterialDistributeTaskDetail + "/Load"
                , where: vmqDtlMaterialDistributeTaskDetail.$data
                , method: "post"
                , page: { curr: 1 }
            });
        }
    };
    All.push({ AreaNameDtl: AreaNameDtlMaterialDistributeTaskDetail, TableNameDtl: TableNameDtlMaterialDistributeTaskDetail, vmqDtl: vmqDtlMaterialDistributeTaskDetail, vmDtl: vmDtlMaterialDistributeTaskDetail, EditInfoDtl: EditInfoDtlMaterialDistributeTaskDetail, NameDtl: NameDtlMaterialDistributeTaskDetail, mainListDtl: mainListDtlMaterialDistributeTaskDetail });

    var selector = {
        'NeedStation': {
            SelType: "FromUrl",
            SelFrom: "/configure/Station/Load",
            SelModel: "NeedStation",
            SelLabel: "Name",
            SelValue: "Code",
            Dom: [$("[name='NeedStation']"), $("[name='qNeedStation']"), $("[name='CNeedStation']")]
        }, 'Status': {
            SelType: "FromDict",
            SelFrom: "AGVStatus",
            SelModel: "Status",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='Status']"), $("[name='qStatus']")]
        },  'CallType': {
            SelType: "FromDict",
            SelFrom: "DistributeType",
            SelModel: "CallType",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='CallType']"), $("[name='qCallType']")]
        }, 'MaterialConfirm': {
            SelType: "FromDict",
            SelFrom: "MaterialConfirm",
            SelModel: "MaterialConfirm",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='MaterialConfirm']"), $("[name='qMaterialConfirm']")]
        }, 'CStockPlace': {
            SelType: "FromUrl",
            SelFrom: "/material/Location/FindStockPlace",
            SelModel: "CStockPlace",
            SelLabel: "Code",
            SelValue: "Code",
            Dom: [$("[name='CStockPlace']")]
        },
    };
    
    var vml = new Array({
        vm: vm,
        vmq: vmq,
        vmc: vmc,
        vmDtlMaterialDistributeTaskDetail: vmDtlMaterialDistributeTaskDetail,
    });
    
    Universal.BindSelector(vml, selector);
    Universal.mmain(AreaName, TableName, vm, vmq, EditInfo, selfbtn, mainList);
    Universal.mainDtl(selfbtn, All);
});