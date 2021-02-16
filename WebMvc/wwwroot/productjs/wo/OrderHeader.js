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
    var TableName = 'OrderHeader';
    
    var vm = new Vue({
        el: '#modifyForm'
    });
    var vmr = new Vue({
        el: '#reviseForm'
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
                , {field:'Id', width:60, sort: true, fixed: false, hide: false, title: 'Id' }
                , { field: 'Code', width: 150, sort: true, fixed: false, hide: false, event: 'OrderDetiail', templet: '#ShowDtlCode', title: '工单号' }
                , { field: 'ProductCode', width: 120, sort: true, fixed: false, hide: false, title: '产品' }
                , { field: 'MachineType', width: 120, sort: true, fixed: false, hide: false, templet: function (d) { return GetLabel('MachineType', 'Code', 'MachineType', d.MachineType) }, title: '机型' }
                , { field: 'PartMaterialCode', width: 150, sort: true, fixed: false, hide: true, title: '部件料号' }
                , {field:'PlanQty', width:90, sort: true, fixed: false, hide: false, title: '计划量' }
                , {field:'CompleteQty', width:90, sort: true, fixed: false, hide: false, title: '完成量' }
                , {field:'NGQty', width:90, sort: true, fixed: false, hide: false, title: '不良量' }
                , { field: 'Status', width: 100, sort: true, fixed: false, hide: false, templet: function (d) { return GetLabel('Status', 'DictValue', 'DictLabel', d.Status) }, title: '工单状态' }
                , { field: 'Type', width: 100, sort: true, fixed: false, hide: false, templet: function (d) { return GetLabel('Type', 'DictValue', 'DictLabel', d.Type) }, title: '工单类型' }
                , { field: 'WorkShop', width: 100, sort: true, fixed: false, hide: false, templet: function (d) { return GetLabel('WorkShop', 'Code', 'Name', d.WorkShop) }, title: '生产车间' }
                , { field: 'LineCode', width: 100, sort: true, fixed: false, hide: false,title: '线体代号' }
                , { field: 'LineId', width: 130, sort: true, fixed: false, hide: false, templet: function (d) { return GetLabel('LineId', 'Id', 'LineName', d.LineId) },  title: '线体名称' }
                , { field: 'LotNo', width: 150, sort: true, fixed: false, hide: false, title: '批次号' }
                , { field: 'Priority', width: 100, sort: true, fixed: false, hide: false, templet: function (d) { return GetLabel('Priority', 'DictValue', 'DictLabel', d.Priority) }, title: '优先级' }
                , { field: 'PlanStartTime', width: 150, sort: true, fixed: false, hide: false, title: '计划开始时间' }
                , { field: 'PlanEndTime', width: 150, sort: true, fixed: false, hide: false, title: '预计完成时间' }
                , { field: 'ActualStartTime', width: 150, sort: true, fixed: false, hide: false, title: '实际开始时间' }
                , { field: 'ActualEndTime', width: 150, sort: true, fixed: false, hide: false, title: '实际结束时间' }
                , { field: 'ReserveNo', width: 150, sort: true, fixed: false, hide: false, title: '预留号' }
                , { field: 'ReserveRowNo', width: 150, sort: true, fixed: false, hide: false, title: '预留行号' }
                , { field: 'WorkFactory', width: 150, sort: true, fixed: false, hide: false, title: '生产工厂' }
                , {field:'CreateTime', width:150, sort: true, fixed: false, hide: false, title: '建立时间' }
                , {field:'CreateBy', width:90, sort: true, fixed: false, hide: false, title: '建立者' }
                , {field:'UpdateTime', width:150, sort: true, fixed: false, hide: false, title: '更新时间' }
                , {field:'UpdateBy', width:90, sort: true, fixed: false, hide: false, title: '更新者' }
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
        $("[name='ProductCode']").html("");
        var option1 = $("<option>").val(list.ProductCode).text(list.ProductCode);
        $("[name='ProductCode']").append(option1);
        form.render('select'); 
    };

    var selfbtn = {
        //自定义按钮
        btnCreateSN: function () {
            var checkStatus = table.checkStatus('mainList');
            var count = checkStatus.data.length;//选中的行数
            if (count > 0) {
                var data = checkStatus.data; //获取选中行的数据
                layer.confirm('确定系统自动分配此单据？', {
                    btn: ['确定', '取消']
                }, function () {
                    //自动分配数据写入任务表
                    $.ajax({
                        url: "/wo/OrderHeader/CreateSN",
                        type: "post",
                        data: { orderheaderlist: data },
                        dataType: "json",
                        // async: false,
                        success: function (result) {
                            if (result.code = 200) {
                                mainList.Load();
                                layer.alert(result.msg, { icon: 1, shadeClose: true, title: "分配信息" });
                            } else {
                                layer.alert('分配失败：' + result.msg, { icon: 5, shadeClose: true, title: "错误信息" });
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
                layer.alert("请最少选中一条工单！", { icon: 5, shadeClose: true, title: "错误信息" });
            }
        },
        CreateMaterialList: function () {
            var checkStatus = table.checkStatus('mainList');
            var count = checkStatus.data.length;//选中的行数
            if (count > 0) {
                var data = checkStatus.data; //获取选中行的数据
                layer.confirm('确定手动生成物料需求清单？', {
                    btn: ['确定', '取消']
                }, function () {
                    //自动分配数据写入任务表
                    $.ajax({
                        url: "/wo/OrderHeader/AddMaterialList",
                        type: "post",
                        data: { orderheaderlist: data },
                        dataType: "json",
                        // async: false,
                        success: function (result) {
                            if (result.code = 200) {
                                layer.alert(result.msg + "请在【物料需求】页面查看！", { icon: 1, shade: 0.4, title: "信息提示" });
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
        }, 
        btnRevise: function () {
            var checkStatus = table.checkStatus('mainList');
            var count = checkStatus.data.length;//选中的行数
            if (count > 0) {
                var data = checkStatus.data; //获取选中行的数据
                //$("[name='ROrderCode']").val(data[0].Code);
                //$("[name='RProductCode']").val(data[0].ProductCode);
                //$("[name='RPartCode']").val(data[0].PartMaterialCode);
                vm.$set('$data', data);
                layer.open({
                    type: 1,
                    area: ['450px', '350px'], //宽高
                    maxmin: true, //开启最大化最小化按钮
                    title: '订单修正作业',
                    content: $('#reviseForm'),
                    btn: ['修正', '取消'],
                    yes: function (index, layero) {
                        //修正订单作业
                        $.ajax({
                            url: "/wo/OrderHeader/Revise",
                            type: "post",
                            data: { orderheaderlist: data, revisetype: $("[name='revise']").val()},
                            dataType: "json",
                            // async: false,
                            success: function (result) {
                                if (result.code = 200) {
                                    layer.alert(result.msg, { icon: 1, shade: 0.4, title: "信息提示" });
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
               
            }
            else {
                layer.alert("请最少选中一条需要作业的工单！", { icon: 5, shadeClose: true, title: "错误信息" });
            }
        },  
    };


    //子表逻辑
    var All = new Array();
    var AreaNameDtlOrderDetiail = 'wo';
    var TableNameDtlOrderDetiail = 'OrderDetiail';
    //{子表字段：主表字段}
    var NameDtlOrderDetiail = { OrderCode: 'Code', OrderHeaderId:'Id' };
    var vmDtlOrderDetiail = new Vue({
        el: '#modifyFormDtl_' + TableNameDtlOrderDetiail
    });
    var vmqDtlOrderDetiail = new Vue({
        data: { DictType: '', HeaderId: '' }
    });
    //编辑
    var EditInfoDtlOrderDetiail = function (tabledata) {
        data = tabledata;
        vmDtlOrderDetiail.$set('$data', tabledata);
        var list = {};
        $('.ClearSelector_' + TableNameDtlOrderDetiail).each(function () {
            var selDom = ($(this));
            if (!$(selDom)[0].name.startsWith("q")) {
                list[$(selDom)[0].name] = data[$(selDom)[0].name] + "";
            }
        });
        //表单修改时填充需修改的数据
        form.val('modifyFormDtl_' + TableNameDtlOrderDetiail, list);
    };

    var mainListDtlOrderDetiail = {
        Render: function () {
            var cols_arr = [[
                { checkbox: true, fixed: true }
                , { field: 'Id', width: 60, sort: true, fixed: false, hide: false, title: 'Id' }
                , { field: 'OrderCode', width: 150, sort: true, fixed: false, hide: false, title: '工单号' }
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

            mainListDtlOrderDetiail.Table = table.render({
                elem: '#mainListDtl' + TableNameDtlOrderDetiail
                , url: "/" + AreaNameDtlOrderDetiail + "/" + TableNameDtlOrderDetiail + "/Load"
                , where: vmqDtlOrderDetiail.$data
                , method: "post"
                , page: true //开启分页
                , cols: hhweb.ColumnSetting('mainListDtl' + TableNameDtlOrderDetiail, cols_arr)
                , id: 'mainListDtl' + TableNameDtlOrderDetiail
                , limit: 10
                , limits: [10, 50, 100, 200, 500, 1000]
                , defaultToolbar: ['filter']
                , toolbar: '#toolbarTable' + TableNameDtlOrderDetiail
                , height: '455'
                , cellMinWidth: 80
                , size: 'sm'
                , done: function (res) { }
            });

            return mainListDtlOrderDetiail.Table;
        },
        Load: function () {
            if (mainListDtlOrderDetiail.Table == undefined) {
                mainListDtlOrderDetiail.Table = this.Render();
                return;
            }
            table.reload('mainListDtl' + TableNameDtlOrderDetiail, {
                url: "/" + AreaNameDtlOrderDetiail + "/" + TableNameDtlOrderDetiail + "/Load"
                , where: vmqDtlOrderDetiail.$data
                , method: "post"
                , page: { curr: 1 }
            });
        }
    };
    All.push({ AreaNameDtl: AreaNameDtlOrderDetiail, TableNameDtl: TableNameDtlOrderDetiail, vmqDtl: vmqDtlOrderDetiail, vmDtl: vmDtlOrderDetiail, EditInfoDtl: EditInfoDtlOrderDetiail, NameDtl: NameDtlOrderDetiail, mainListDtl: mainListDtlOrderDetiail });

    var selector = {
        'MachineType': {
            SelType: "FromUrl",
            SelFrom: "/configure/ProductHeader/DistinctLoad",
            SelModel: "MachineType",
            SelLabel: "MachineType",
            SelValue: "MachineType",
            Dom: [$("[name='MachineType']"), $("[name='qMachineType']")]
        },
        //'ProductCode': {
        //    SelType: "FromUrl",
        //    SelFrom: "/configure/ProductHeader/Load",
        //    SelModel: "ProductCode",
        //    SelLabel: "MachineType",
        //    SelValue: "Code",
        //    Dom: [$("[name='ProductCode']"), $("[name='qProductCode']")]
        //},
        'Status': {
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
        }, 'LineCode': {
            SelType: "FromUrl",
            SelFrom: "/configure/Line/Load",
            SelModel: "LineCode",
            SelLabel: "LineName",
            SelValue: "LineCode",
            Dom: [$("[name='LineCode']"), $("[name='qLineCode']")]
        }, 'LineId': {
            SelType: "FromUrl",
            SelFrom: "/configure/Line/Load",
            SelModel: "LineId",
            SelLabel: "LineName",
            SelValue: "Id"
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
        },'Priority': {
            SelType: "FromDict",
            SelFrom: "priority",
            SelModel: "Priority",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='Priority']"), $("[name='qPriority']")]
        }, 'WorkShop': {
            SelType: "FromUrl",
            SelFrom: "/configure/Workshop/Load",
            SelModel: "WorkShop",
            SelLabel: "Name",
            SelValue: "Code",
            Dom: [$("[name='WorkShop']"), $("[name='qWorkShop']")]
        },
    };
    //实现下拉联动，一级下拉监听
    form.on('select(SelectMachineType)', function (data) {
        vm.$set('MachineType', data.value);
        console.log(data);
        $.ajax({
            type: 'POST',
            url: '/configure/ProductHeader/Load',
            data: {
                entity: { 'MachineType': data.value }
            },
            dataType: 'json',
            success: function (data) {
                console.log(data);
                $("[name='ProductCode']").html("");
                $.each(data.data, function (key, value) {
                    var option1 = $("<option>").val(value.Code).text(value.Code);
                    $("[name='ProductCode']").append(option1);
                    form.render('select');
                });
                vm.$set('ProductCode', $("[name='ProductCode']").get(0).value);//如果不选择默认取二级菜单第一个值
                //监听二级下拉事件
                form.on('select(SelectProductCode)', function (data) {
                    //   $("[name='PartMaterialCode']").get(0).selectedIndex = 0;
                    vm.$set('ProductCode', data.value);
                    form.render('select');
                });

            }
        });

        form.render('select');
    });

    form.on('select(qMachineType)', function (data) {
        vmq.$set('MachineType', data.value);
        $.ajax({
            type: 'POST',
            url: '/configure/ProductHeader/Load',
            data: {
                entity: { 'MachineType': data.value }
            },
            dataType: 'json',
            success: function (data) {
                $("[name='qProductCode']").html("");
                $.each(data.data, function (key, value) {
                    var option1 = $("<option>").val(value.Code).text(value.Code);
                    $("[name='qProductCode']").append(option1);
                    form.render('select');
                });
                vmq.$set('ProductCode', $("[name='qProductCode']").get(0).value);//如果不选择默认取二级菜单第一个值
                //监听二级下拉事件
                form.on('select(qProductCode)', function (data) {
                    vmq.$set('ProductCode', data.value);
                    form.render('select');
                });
            }
        });

        form.render('select');
    });

    form.on('radio(revise)', function (data) {
        $("[name='revise']").val(data.value);
        //console.log(data.value); //被点击的radio的value值
    });  

    var vml = new Array({
        vm: vm,
        vmq: vmq,
        vmDtlOrderDetiail: vmDtlOrderDetiail,
    });
    
    Universal.BindSelector(vml, selector);
    Universal.mmain(AreaName, TableName, vm, vmq, EditInfo, selfbtn, mainList);
    Universal.mainDtl(selfbtn, All);
});