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
    var TableName = 'PrintingContainer';

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

    var barcodeStyle = {
        format: "CODE39",//选择要使用的条形码类型
        width: 1,//设置条之间的宽度
        height: 110,//高度
        displayValue: true,//是否在条形码下方显示文字
        // text:"456",//覆盖显示的文本
       // fontOptions: "bold",//使文字加粗体或变斜体
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
                , { field: 'Code', width: 150, sort: true, fixed: false, hide: false, title: '容器编码' }
                , { field: 'Type', width: 150, sort: true, fixed: false, hide: false, title: '容器类型', templet: function (d) { return GetLabel('Type', 'DictValue', 'DictLabel', d.Type) } }
                , { field: 'LocationId', width: 150, sort: true, fixed: false, hide: false, title: '库位id' }
                , { field: 'LocationCode', width: 150, sort: true, fixed: false, hide: false, title: '库位编码' }
                , { field: 'Status', width: 150, sort: true, fixed: false, hide: false, title: '状态', templet: function (d) { return GetLabel('Status', 'DictValue', 'DictLabel', d.Status) } }
                , { field: 'PrintCount', width: 150, sort: true, fixed: false, hide: false, title: '打印次数' }
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
                data[i].PrintCount += 1;
                var tr = 
                    '<div style:"">'+
                        '<img id="PCodeBar'+i+'">'+
                    '</div>'+
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
                    title: "容器标签打印", //不显示标题
                    area: ['400px', '250px'], //宽高
                    content: $('#printForm'), //捕获的元素
                    scrollbar: true,
                    btn: ['打印', '关闭'],
                    yes: function (index, layero) {
                        $('#printForm').jqprint();
                        layer.close(index);
                        $.ajax({
                            url: "/general/Container/UpData",
                            type: "POST",
                            data: { ids:data },
                            dataType: "json",
                            success: function (result) {
                                if (result.Code == 200 && result.Status) {
                                    layer.msg("打印成功!", { icon: 6, shade: 0.4, time: 1000 });
                                    layer.close(index);
                                    mainList.Load();

                                } else {
                                    layer.alert("打印失败:" + result.Message, { icon: 5, shadeClose: true, title: "错误信息" });
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
        }
    };

    var vml = new Array({
        vm: vm,
        vmq: vmq,
    });
    Universal.BindSelector(vml, selector);
    Universal.mmain(AreaName, TableName, vm, vmq, EditInfo, selfbtn, mainList);
});