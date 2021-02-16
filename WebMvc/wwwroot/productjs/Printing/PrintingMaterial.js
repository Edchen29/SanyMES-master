layui.config({
    base: "/js/"
}).use(['form', 'element', 'vue', 'layer', 'laydate', 'jquery', 'table', 'hhweb', 'utils', 'JsBarcode', 'Universal', 'jqprint',], function () {
    var form = layui.form,
        layer = layui.layer,
        element = layui.element,
        laydate = layui.laydate,
        $ = layui.jquery,
        table = layui.table,
        hhweb = layui.hhweb,
        Universal = layui.Universal;
    var JsBarcode = layui.JsBarcode;
    var jqprint = layui.jqprint;
    var AreaName = 'Printing';
    var TableName = 'PrintingMaterial';

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

    //一维码参数设置
    var barcodeStyle = {
        format: "CODE128",//选择要使用的条形码类型
        width: 1,//设置条之间的宽度
        height: 45,//高度
        displayValue: true,//是否在条形码下方显示文字
        // text:"456",//覆盖显示的文本
        // fontOptions:"bold italic",//使文字加粗体或变斜体
        font: "monospace",//设置文本的字体fantasy
        textAlign: "center",//设置文本的水平对齐方式
        textPosition: "bottom",//设置文本的垂直位置
        textMargin: 1,//设置条形码和文本之间的间距
        fontSize: 12,//设置文本的大小
        background: "#ffffff",//设置条形码的背景
        lineColor: "#000000",//设置条和文本的颜色。
        margin: 1//设置条形码周围的空白边距
    };

    var mainList = {
        Render: function () {
            var cols_arr = [[
                { checkbox: true, fixed: true }
                , { field: 'Id', width: 80, sort: true, fixed: false, hide: false, title: 'Id' }
                , { field: 'Code', width: 150, sort: true, fixed: false, hide: false, title: '物料编号' }
                , { field: 'Name', width: 150, sort: true, fixed: false, hide: false, title: '物料名称' }
                , { field: 'Type', width: 150, sort: true, fixed: false, hide: false, title: '物料类型' }
                , { field: 'DrawingNumber', width: 150, sort: true, fixed: false, hide: false, title: '图号' }
                , { field: 'FunctionClass', width: 150, sort: true, fixed: false, hide: false, title: '功能类别', templet: function (d) { return GetLabel('FunctionClass', 'DictValue', 'DictLabel', d.FunctionClass) } }
                , { field: 'BuildClass', width: 150, sort: true, fixed: false, hide: false, title: '产品类别', templet: function (d) { return GetLabel('BuildClass', 'DictValue', 'DictLabel', d.BuildClass) } }
                , { field: 'BarCode', width: 150, sort: true, fixed: false, hide: false, title: '物料条码' }
                , { field: 'BarCode1', width: 150, sort: true, fixed: false, hide: false, title: '关联条码' }
                , { field: 'Specification', width: 150, sort: true, fixed: false, hide: false, title: '品名规格' }
                , { field: 'Weight', width: 150, sort: true, fixed: false, hide: false, title: '重量' }
                , { field: 'Unit', width: 150, sort: true, fixed: false, hide: false, title: '单位', templet: function (d) { return GetLabel('Unit', 'Code', 'Name', d.Unit) } }
                , { field: 'ClassABC', width: 150, sort: true, fixed: false, hide: false, title: 'ABC分类' }
                , { field: 'QcCheck', width: 150, sort: true, fixed: false, hide: false, title: '品检' }
                , { field: 'UniqueMark', width: 150, sort: true, fixed: false, hide: false, title: '唯一标识码' }
                , { field: 'CreateTime', width: 150, sort: true, fixed: false, hide: false, title: '创建时间' }
                , { field: 'CreateBy', width: 150, sort: true, fixed: false, hide: false, title: '创建用户' }
                , { field: 'UpdateTime', width: 150, sort: true, fixed: false, hide: false, title: '更新时间' }
                , { field: 'UpdateBy', width: 150, sort: true, fixed: false, hide: false, title: '更新用户' }
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
        btnPrint: function () {
            var checkStatus = table.checkStatus('mainList');
            var count = checkStatus.data.length;
            var myDate = new Date();
            var data = checkStatus.data;
            var html1 = '';
            $('#tanchuang').empty();  //清空上一次数据
            //根据数据批量生成table
            for (var i = 0; i < count; i++) {
                var Pid = 'PCodeBar' + i;
                var tr =
                    '<tr>' +
                    '<td rowspan = "6" >' +
                    '<div id="PCodeBar' + i + '" style="float:left;padding:0px 5px;"></div>' +
                    '</td >' +
                    '<td style=" font-size: xx-small !important;font-family: 黑体;">' +
                    '<span style="text-align: center" id="Code' + i + '">物料编码：</span>' +
                    '</td>' +
                    '<td style=" font-size: xx-small !important;font-family: 黑体;">' +
                    '<label class="PCode" name="PCode" style="border:0px;width:auto">' + data[i].Code + '</label>' +
                    '</td>' +
                    '</tr >' +
                    '<tr>' +
                    '<td style=" font-size: xx-small !important;font-family: 黑体;">' +
                    '<span style="text-align: center">物料类别：</span>' +
                    '</td>' +
                    '<td style=" font-size: xx-small !important;font-family: 黑体;">' +
                    '<label class="PType" name="PType" style="border:0px;width:auto">' + data[i].Type + '</label>' +
                    '</td>' +
                    '</tr>' +
                    '<tr>' +
                    '<td style=" font-size: xx-small !important;font-family: 黑体;">' +
                    '<span style="text-align: center">图号：</span>' +
                    '</td>' +
                    '<td style=" font-size: xx-small !important;font-family: 黑体;">' +
                    '<label class="PDrawingNumber" name="PDrawingNumber" style="border:0px;width:auto">' + data[i].DrawingNumber + '</label>' +
                    '</td>' +
                    '</tr>' +
                    '<tr>' +
                    '<td style=" font-size: xx-small !important;font-family: 黑体;">' +
                    '<span style="text-align: center">规格：</span>' +
                    '</td>' +
                    '<td style=" font-size: xx-small !important;font-family: 黑体;">' +
                    '<label class="PSpecification" name="PSpecification" style="border:0px;width:auto">' + data[i].Specification + '</label>' +
                    '</td>' +
                    '</tr>' +
                    '<tr>' +
                    '<td style=" font-size: xx-small !important;font-family: 黑体;">' +
                    '<span style="text-align: center">重量/数量：</span>' +
                    '</td>' +
                    '<td style=" font-size: xx-small !important;font-family: 黑体;">' +
                    '<label class="PWeight" name="PWeight" style="border:0px;width:auto">' + data[i].Weight + '</label>' +
                    '</td>' +
                    '</tr>' +
                    '<tr>' +
                    '<td style=" font-size: xx-small !important;font-family: 黑体;">' +
                    '<span style="text-align: center">名称：</span>' +
                    '</td>' +
                    '<td style=" font-size: xx-small !important;font-family: 黑体;">' +
                    '<label class="PName" name="PName" style="border:0px;width:auto">' + data[i].Name + '</label>' +
                    '</td>' +
                    '</tr>' +
                    '<tr>' +
                    '<td colspan="3" align="center" style="background-color:white">' +
                    '<hr style="height:2px;border:none;border-top:1px ridge green;" />' +
                    '</td>' +
                    '</tr >'
                $table = $(tr)
                $('#tanchuang').append($table)
                //二维码参数设置
                var qrcode = new QRCode(Pid , {
                    text: '',
                    width: 80,
                    height: 80,
                    colorDark: '#000000',
                    colorLight: '#ffffff',
                    correctLevel: QRCode.CorrectLevel.H
                });
                qrcode.makeCode(data[i].Code);
            }

            //弹窗打印
            layer.open({
                type: 1,
                //  skin: 'layui-layer-molv',
                btnAlign: 'c',
                moveType: 1, //拖拽模式，0或者1
                title: "物料信息打印", //不显示标题
                area: ['400px', '300px'], //宽高
                content: $('#Ptan'), //捕获的元素
                scrollbar: true,
                btn: ['打印', '关闭'],
                yes: function (index, layero) {
                    $('#Ptan').jqprint();
                    layer.close(index);
                },
                cancel: function (index) {
                    layer.close(index);
                }
            })
        }
    };

    var selector = {
        'Type': {
            SelType: "FromDict",
            SelFrom: "materialType",
            SelModel: "Type",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='Type']"), $("[name='qType']")]
        },'MasterUnit': {
            SelType: "FromDict",
            SelFrom: "unitType",
            SelModel: "MasterUnit",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='MasterUnit']"), $("[name='qMasterUnit']")]
        },'AssistUnit': {
            SelType: "FromDict",
            SelFrom: "unitType",
            SelModel: "AssistUnit",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='AssistUnit']"), $("[name='qAssistUnit']")]
        }, 'FunctionClass': {
            SelType: "FromDict",
            SelFrom: "FunctionClass",
            SelModel: "FunctionClass",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='FunctionClass']"), $("[name='qFunctionClass']")]
        }, 'BuildClass': {
            SelType: "FromDict",
            SelFrom: "BuildClass",
            SelModel: "BuildClass",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='BuildClass']"), $("[name='qBuildClass']")]
        }, 'Unit': {
            SelType: "FromUrl",
            SelFrom: "/material/MaterialUnit/Load",
            SelModel: "Unit",
            SelLabel: "Name",
            SelValue: "Code",
            Dom: [$("[name='Unit']"), $("[name='qUnit']")]
        },
    };

    var vml = new Array({
        vm: vm,
        vmq: vmq,
    });

    Universal.BindSelector(vml, selector);
    Universal.mmain(AreaName, TableName, vm, vmq, EditInfo, selfbtn, mainList);
});