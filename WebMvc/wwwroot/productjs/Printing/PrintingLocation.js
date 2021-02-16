layui.config({
    base: "/js/"
}).use(['form', 'element', 'vue', 'layer', 'laydate', 'jquery', 'table', 'hhweb', 'utils', 'JsBarcode', 'Universal', 'jqprint'], function () {
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
    var TableName = 'PrintingLocation';

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

    var barcodeStyle = {
        format: "CODE39",//选择要使用的条形码类型
        width: 1,//设置条之间的宽度
        height: 110,//高度
        displayValue: true,//是否在条形码下方显示文字
        // text:"456",//覆盖显示的文本
        fontOptions: "bold",//使文字加粗体或变斜体
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
                , { field: 'Code', width: 150, sort: true, fixed: false, hide: false, title: '库位' }
                , { field: 'Status', width: 150, sort: true, fixed: false, hide: false, title: '状态', templet: function (d) { return GetLabel('Status', 'DictValue', 'DictLabel', d.Status) } }
                , { field: 'Roadway', width: 150, sort: true, fixed: false, hide: false, title: '巷道' }
                , { field: 'Type', width: 150, sort: true, fixed: false, hide: false, title: '库位类型', templet: function (d) { return GetLabel('Type', 'DictValue', 'DictLabel', d.Type) } }
                , { field: 'IsStop', width: 150, sort: true, fixed: false, hide: false, title: '是否禁用', templet: function (d) { return GetLabel('IsStop', 'DictValue', 'DictLabel', d.IsStop) } }
                , { field: 'Row', width: 150, sort: true, fixed: false, hide: false, title: '行' }
                , { field: 'Column', width: 150, sort: true, fixed: false, hide: false, title: '列' }
                , { field: 'Layer', width: 150, sort: true, fixed: false, hide: false, title: '层' }
                , { field: 'Grid', width: 150, sort: true, fixed: false, hide: false, title: '格' }
                , { field: 'RowIndex', width: 150, sort: true, fixed: false, hide: false, title: '双伸位索引' }
                , { field: 'ContainerId', width: 150, sort: true, fixed: false, hide: false, title: '容器id号' }
                , { field: 'ContainerCode', width: 150, sort: true, fixed: false, hide: false, title: '容器编码' }
                , { field: 'GoodsNo', width: 150, sort: true, fixed: false, hide: false, title: '物料编号' }
                , { field: 'LastCycleCountDate', width: 150, sort: true, fixed: false, hide: false, title: '上次盘点日期' }
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
            $('#tanchuang').empty();  //清空上一次数据
            var checkStatus = table.checkStatus('mainList');
            var count = checkStatus.data.length;
            var myDate = new Date();
            var data = checkStatus.data;
            for (var i = 0; i < count; i++) {
                var tr =
                    '<div style:"">' +
                    '<img id="PCodeBar' + i + '">' +
                    '</div>' +
                    '<hr style="height:2px;border:none;border-top:1px ridge green;" />'
                $table = $(tr)
                $('#tanchuang').append($table)
                JsBarcode("#PCodeBar" + i, data[i].Code, barcodeStyle);
            }
                
                //弹窗打印
                layer.open({
                    type: 1,
                    //  skin: 'layui-layer-molv',
                    btnAlign: 'c',
                    moveType: 1, //拖拽模式，0或者1
                    title: "库位标签打印", //不显示标题
                    area: ['400px', '240px'], //宽高
                    content: $('#printForm'), //捕获的元素
                    scrollbar: true,
                    btn: ['打印', '关闭'],
                    yes: function (index, layero) {
                        var option = {
                            beforePrint: function () {
                                console.log("Print Start");
                            },
                            afterPrint: function () {
                                console.log("Print OK");
                            }
                        }
                        $('#printForm').jqprint(option);
                        layer.close(index);
                    },
                    cancel: function (index) {
                        layer.close(index);
                    }
                });
        }
    };

    var selector = {
    };

    var vml = new Array({
        vm: vm,
        vmq: vmq,
    });

    var selector = {
        'Status': {
            SelType: "FromDict",
            SelFrom: "locationStatus",
            SelModel: "Status",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='Status']"), $("[name='qStatus']")]
        },
        'IsStop': {
            SelType: "FromDict",
            SelFrom: "IsStop",
            SelModel: "IsStop",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='IsStop']"), $("[name='qIsStop']")]
        },
        'Type': {
            SelType: "FromDict",
            SelFrom: "locationType",
            SelModel: "Type",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='Type']"), $("[name='qType']")]
        }
    };

    Universal.BindSelector(vml, selector);
    Universal.mmain(AreaName, TableName, vm, vmq, EditInfo, selfbtn, mainList);
});