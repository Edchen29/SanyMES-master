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
    
    var AreaName = 'configure';
    var TableName = 'ProductHeader';
    
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
                , {field:'Id', width:150, sort: true, fixed: false, hide: false, title: 'Id' }
                , { field: 'Code', width: 150, sort: true, fixed: false, hide: false, event: 'ProductDetail', templet: '#ShowDtlCode', title: '产品代号' }
                , {field:'Name', width:150, sort: true, fixed: false, hide: false, title: '产品名称' }
                , {field:'Type', width:150, sort: true, fixed: false, hide: false, title: '类别' }
                , {field:'DrawingNumber', width:150, sort: true, fixed: false, hide: false, title: '图号' }
                , {field:'Specification', width:150, sort: true, fixed: false, hide: false, title: '外形尺寸' }
                , {field:'Weight', width:150, sort: true, fixed: false, hide: false, title: '重量' }
                , {field:'MachineType', width:150, sort: true, fixed: false, hide: false, title: '机型' }
                , { field: 'WorkShop', width: 150, sort: true, fixed: false, hide: false, title: '生产车间', templet: function (d) { return GetLabel('WorkShop', 'Code', 'Name', d.WorkShop) } }
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
          //  mainList.Table.reload({
          //      url: "/" + AreaName + "/" + TableName + "/Load"
          //      , method: "post"
         //   });
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
    //子表逻辑
    var All = new Array();
    var AreaNameDtlProductDetail = 'configure';
    var TableNameDtlProductDetail = 'ProductDetail';
    //{子表字段：主表字段}
    var NameDtlProductDetail = { ProductHeaderId: 'Id' };
    var vmDtlProductDetail = new Vue({
        el: '#modifyFormDtl_' + TableNameDtlProductDetail
    });
    var vmqDtlProductDetail = new Vue({
        data: { DictType: '', HeaderId: '' }
    });
    //编辑
    var EditInfoDtlProductDetail = function (tabledata) {
        data = tabledata;
        vmDtlProductDetail.$set('$data', tabledata);
        var list = {};
        $('.ClearSelector_' + TableNameDtlProductDetail).each(function () {
            var selDom = ($(this));
            if (!$(selDom)[0].name.startsWith("q")) {
                list[$(selDom)[0].name] = data[$(selDom)[0].name] + "";
            }
        });
        //表单修改时填充需修改的数据
        form.val('modifyFormDtl_' + TableNameDtlProductDetail, list);
        $("[name='StepCode']").html("");
        var option1 = $("<option>").val(list.StepCode).text(list.StepCode);
        $("[name='StepCode']").append(option1);
        form.render('select'); 
    };

    var mainListDtlProductDetail = {
        Render: function () {
            var cols_arr = [[
                { checkbox: true, fixed: true }
                , { field: 'Id', width: 60, sort: true, fixed: false, hide: false, title: 'Id' }
                , { field: 'ProductHeaderId', width: 120, sort: true, fixed: false, hide: false, title: '产品代号', templet: function (d) { return GetLabel('ProductHeaderId', 'Id', 'Code', d.ProductHeaderId) } }
                , { field: 'LineId', width: 150, sort: true, fixed: false, hide: false, title: '线体', templet: function (d) { return GetLabel('LineId', 'Id', 'LineName', d.LineId) } }
                , { field: 'LineCode', width: 150, sort: true, fixed: false, hide: false, title: '线体代号' }
                , { field: 'StepId', width: 150, sort: true, fixed: false, hide: false, title: '工序', templet: function (d) { return GetLabel('StepId', 'Id', 'Name', d.StepId) } }
                , { field: 'StepCode', width: 150, sort: true, fixed: false, hide: false, title: '工序代号' }
                , { field: 'ProgramCode', width: 200, sort: true, fixed: false, hide: false, title: '工序配套程序' }
                , { field: 'CreateTime', width: 150, sort: true, fixed: false, hide: false, title: '建立时间' }
                , { field: 'CreateBy', width: 100, sort: true, fixed: false, hide: false, title: '建立者' }
                , { field: 'UpdateTime', width: 150, sort: true, fixed: false, hide: false, title: '更新时间' }
                , { field: 'UpdateBy', width: 100, sort: true, fixed: false, hide: false, title: '更新者' }
            ]];

            mainListDtlProductDetail.Table = table.render({
                elem: '#mainListDtl' + TableNameDtlProductDetail
                , url: "/" + AreaNameDtlProductDetail + "/" + TableNameDtlProductDetail + "/Load"
                , where: vmqDtlProductDetail.$data
                , method: "post"
                , page: true //开启分页
                , cols: hhweb.ColumnSetting('mainListDtl' + TableNameDtlProductDetail, cols_arr)
                , id: 'mainListDtl' + TableNameDtlProductDetail
                , limit: 8
                , limits: [8, 50, 100, 200, 500, 1000]
                , defaultToolbar: ['filter']
                , toolbar: '#toolbarTable' + TableNameDtlProductDetail
                , height: '338'
                , cellMinWidth: 80
                , size: 'sm'
                , done: function (res) { }
            });

            return mainListDtlProductDetail.Table;
        },
        Load: function () {
            if (mainListDtlProductDetail.Table == undefined) {
                mainListDtlProductDetail.Table = this.Render();
                return;
            }
            table.reload('mainListDtl' + TableNameDtlProductDetail, {
                url: "/" + AreaNameDtlProductDetail + "/" + TableNameDtlProductDetail + "/Load"
                , where: vmqDtlProductDetail.$data
                , method: "post"
                , page: { curr: 1 }
            });
        }
    };
    All.push({ AreaNameDtl: AreaNameDtlProductDetail, TableNameDtl: TableNameDtlProductDetail, vmqDtl: vmqDtlProductDetail, vmDtl: vmDtlProductDetail, EditInfoDtl: EditInfoDtlProductDetail, NameDtl: NameDtlProductDetail, mainListDtl: mainListDtlProductDetail });

    var selector = {
        'LineCode': {
            SelType: "FromUrl",
            SelFrom: "/configure/Line/Load",
            SelModel: "LineCode",
            SelLabel: "LineName",
            SelValue: "LineCode",
            Dom: [$("[name='LineCode']")]
        }, 'LineId': {
            SelType: "FromUrl",
            SelFrom: "/configure/Line/Load",
            SelModel: "LineId",
            SelLabel: "LineName",
            SelValue: "Id"
        }, 'StepId': {
            SelType: "FromUrl",
            SelFrom: "/configure/Step/Load",
            SelModel: "StepId",
            SelLabel: "Name",
            SelValue: "Id"
        }, 'StepCode': {
            SelType: "FromUrl",
            SelFrom: "/configure/Step/Load",
            SelModel: "StepCode",
            SelLabel: "Name",
            SelValue: "Code",
            Dom: [$("[name='StepCode']")]
        }, 'ProductHeaderId': {
            SelType: "FromUrl",
            SelFrom: "/configure/ProductHeader/Load",
            SelModel: "ProductHeaderId",
            SelLabel: "Code",
            SelValue: "Id"
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
    form.on('select(SelectLineCode)', function (data) {
        vmDtlProductDetail.$set('LineCode', data.value);
        $.ajax({
            type: 'POST',
            url: '/configure/Step/Load',
            data: {
                entity: { 'ProductId': $("[name='ProductHeaderId']").val() }
            },
            dataType: 'json',
            success: function (data) {
                $("[name='StepCode']").html("");
                $.each(data.data, function (key, value) {
                    var option1 = $("<option>").val(value.Code).text(value.Name);
                    $("[name='StepCode']").append(option1);
                    form.render('select');
                });
                vmDtlProductDetail.$set('StepCode', $("[name='StepCode']").get(0).value);//如果不选择默认取二级菜单第一个值
                //监听二级下拉事件
                form.on('select(SelectStepCode)', function (data) {
                    vmDtlProductDetail.$set('StepCode', data.value);
                    form.render('select');
                });

            }
        });

        form.render('select');
    });


    var vml = new Array({
        vm: vm,
        vmq: vmq,
        vmDtlProductDetail, vmDtlProductDetail,
    });
    
    Universal.BindSelector(vml, selector);
    Universal.mmain(AreaName, TableName, vm, vmq, EditInfo, selfbtn, mainList);
    Universal.mainDtl(selfbtn, All);
});