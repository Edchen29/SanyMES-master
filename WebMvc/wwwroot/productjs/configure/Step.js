layui.config({
    base: "/js/"
}).use(['form', 'element', 'vue', 'layer', 'laydate', 'jquery', 'table', 'hhweb', 'utils', 'Universal', 'upload'], function () {
    var form = layui.form,
        layer = layui.layer,
        element = layui.element,
        laydate = layui.laydate,
        $ = layui.jquery,
        table = layui.table,
        hhweb = layui.hhweb,
        upload = layui.upload,
        Universal = layui.Universal;
    
    var AreaName = 'configure';
    var TableName = 'Step';
    
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

    function proofSOP(d) {
        var proofSOP = d.LinkSop;
        if ('' == proofSOP || null == proofSOP || undefined == proofSOP) {
            return '';
        }
        if (proofSOP.length > 0) {
            return '<a class="layui-table-link" href="' + d.LinkSop+'" lay-event="LookSOP">查看作业指导书</a>'
        }
    };
    var mainList = {
        Render: function () {
            var cols_arr = [[
                { checkbox: true, fixed: true }
                , { field: 'Id', width: 80, sort: true, fixed: false, hide: false, title: 'Id' }
                , { field: 'ProductId', width: 150, sort: true, fixed: false, hide: false, title: '产品标识', templet: function (d) { return GetLabel('ProductId', 'Id', 'Code', d.ProductId) } }
                , { field: 'MachineType', width: 150, sort: true, fixed: false, hide: false, title: '机型', templet: function (d) { return GetLabel('MachineType', 'MachineType', 'MachineType', d.MachineType) } }
                , { field: 'ProductType', width: 120, sort: true, fixed: false, hide: false, title: '产品类别' }
                , { field: 'MaterialId', width: 150, sort: true, fixed: false, hide: false, title: '物料名称', templet: function (d) { return GetLabel('MaterialId', 'Id', 'Name', d.MaterialId) } }
                , { field: 'MaterialCode', width: 150, sort: true, fixed: false, hide: false, title: '物料编码' }
                , { field: 'Code', width: 150, sort: true, fixed: false, hide: false, title: '工序代号' }
                , { field: 'Name', width: 150, sort: true, fixed: false, hide: false, title: '工序名称' }
                , { field: 'CycleTime', width: 80, sort: true, fixed: false, hide: false, title: '周期' }
                , { field: 'Sequence', width: 80, sort: true, fixed: false, hide: false, title: '顺序' }
                , { field: 'StepType', width: 150, sort: true, fixed: false, hide: false, title: '工序类型', templet: function (d) { return GetLabel('StepType', 'DictValue', 'DictLabel', d.StepType) }  }
                , { field: 'LinkSop', width: 150, sort: true, fixed: false, hide: false, title: '作业指导书', templet: proofSOP }
                , { field: 'CtrlCode', width: 100, sort: true, fixed: false, hide: false, title: '控制码' }
                , { field: 'MesSeqence', width: 110, sort: true, fixed: false, hide: false, title: 'MES顺序号' }
                , { field: 'WorkCenter', width: 120, sort: true, fixed: false, hide: false, title: '工作中心' }
                , { field: 'PlanStartTime', width: 150, sort: true, fixed: false, hide: false, title: '计划开始时间' }
                , { field: 'PlanEndTime', width: 150, sort: true, fixed: false, hide: false, title: '计划结束时间' }
                , { field: 'ActualStartTime', width: 150, sort: true, fixed: false, hide: false, title: '实际开始时间' }
                , { field: 'ActualEndTime', width: 150, sort: true, fixed: false, hide: false, title: '实际结束时间' }
                , { field: 'CreateTime', width: 150, sort: true, fixed: false, hide: false, title: '建立时间' }
                , { field: 'CreateBy', width: 100, sort: true, fixed: false, hide: false, title: '建立者' }
                , { field: 'UpdateTime', width: 150, sort: true, fixed: false, hide: false, title: '更新时间' }
                , { field: 'UpdateBy', width: 100, sort: true, fixed: false, hide: false, title: '更新者' }
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
    //var uploadInst = upload.render({
    //        elem: '#test1'
    //        , url: '/wwwroot/upload/' //上传接口
    //        , method: 'post'  //可选项。HTTP类型，默认post
    //        // , data: { id: 123} //可选项。额外的参数，如：{id: 123, abc: 'xxx'}
    //        , done: function (res, index, upload) {
    //            //上传完毕回调
    //            layer.msg('上传成功');
    //            console.log(res)
    //        }
    //        , error: function () {
    //            //请求异常回调
    //        }
    //    });
    var selfbtn = {
        //自定义按钮
        btnWorkSOP: function () {
            var checkStatus = table.checkStatus('mainList');
            var count = checkStatus.data.length;//选中的行数
            if (count == 1) {
                var tdata = checkStatus.data; //获取选中行的数据
                var renamef = tdata[0].ProductCode + tdata[0].Code;
                
                layer.open({
                    type: 1,
                    skin: 'layui-layer-molv',
                    anim: 1,
                    id: 'LAY_layuipro2', //设定一个id，防止重复弹出
                    btnAlign: 'c',
                    moveType: 1, //拖拽模式，0或者1
                    title: "请选择需要上传的作业指导书：", //不显示标题
                    area: ['400px', '300px'], //宽高
                    content: $('#WorkSOPData'), //捕获的元素
                    scrollbar: false,
                    btn: ['上传', '关闭'],
                    yes: function () {
                        var index1 = layer.load();
                        var fileObj = document.getElementById("file").files[0]; // js 获取文件对象
                        if (typeof (fileObj) == "undefined" || fileObj.size <= 0) {
                            layer.alert("请先选择需要上传的文件！", { skin: 'layui-layer-molv', anim: 1, icon: 5 });
                            layer.close(index1);
                            return;
                        }
                        var formData = new FormData();
                        formData.append("file", fileObj); //加入文件对象
                        var pcode = tdata[0].ProductCode;
                        var scode = tdata[0].Code;
                        formData.append("pcode", pcode)
                        formData.append("scode", scode)
                        var data = formData;
                       // console.log(formFile);
                        $.ajax({
                            url: "/" + AreaName + "/" + TableName + "/UploadWorkSOP",
                            data: formData,
                            type: "Post",
                           // dataType: "formData",
                            dataType: "json",
                            cache: false,//上传文件无需缓存
                            processData: false,//用于对data参数进行序列化处理 这里必须false
                            contentType: false, //必须
                            success: function (result) {
                                if (result.Code == 200) {
                                    
                                    if (result.Status) {
                                        layer.msg(result.Message, { icon: 6 });
                                    }
                                    else {
                                        layer.alert(result.Message, { icon: 5, shade: 0.4, shadeClose: true, title: '上传失败' });
                                    }
                                    $('#file').val("");
                                    table.reload('mainList', {});
                                }
                            }
                        });
                        layer.closeAll();
                    },
                    cancel: function (index) {
                        $("Hide").unbind();
                        layer.close(index);
                    }
                });
            }
            else {
                layer.alert("请选中一笔需要上传作业指导书的工序信息！", { icon: 5, shadeClose: true, title: "错误信息" });
            }
        },
    };
    
    var selector = {
        'MaterialId': {
            SelType: "FromUrl",
            SelFrom: "/material/Material/Load",
            SelModel: "MaterialId",
            SelLabel: "MaterialName",
            SelValue: "MaterialId"
        },  'ProductId': {
            SelType: "FromUrl",
            SelFrom: "/configure/ProductHeader/Load",
            SelModel: "ProductId",
            SelLabel: "Code",
            SelValue: "Id",
            Dom: [$("[name='ProductId']"), $("[name='qProductId']")]
        }, 'MachineType': {
                SelType: "FromUrl",
                SelFrom: "/configure/ProductHeader/DistinctLoad",
                SelModel: "MachineType",
                SelLabel: "MachineType",
                SelValue: "MachineType",
                Dom: [$("[name='MachineType']"), $("[name='qMachineType']")]
        }, 'StepType': {
            SelType: "FromDict",
            SelFrom: "StepType",
            SelModel: "StepType",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='StepType']"), $("[name='qStepType']")]
        }, 
    };
    
    var vml = new Array({
        vm: vm,
        vmq: vmq,
    });
    
    Universal.BindSelector(vml, selector);
    Universal.mmain(AreaName, TableName, vm, vmq, EditInfo, selfbtn, mainList);
});