layui.config({
    base: "/js/"
}).use(['form', 'element', 'vue', 'layer', 'laydate', 'jquery', 'table', 'hhweb', 'utils'], function () {
    var form = layui.form,
        layer = layui.layer,
        element = layui.element,
        laydate = layui.laydate,
        $ = layui.jquery,
        table = layui.table,
        hhweb = layui.hhweb,
        toplayer = top == undefined || top.layer === undefined ? layer : top.layer;  //顶层的LAYER

    //监听页面主按钮操作
    var active = {
        //检索区关闭按钮
        btnQueryData: function () {
            if ($("#ReportId").val() == "") {
                layer.alert("请选择要查询的报表！", { icon: 2, title: '提示' });
                return;
            }
            QueryData($("#ReportId").val(), $("#Filter").val());
        },

        btnExportData: function () {
            table.exportFile(ExportTitle, ExportData, 'csv')
        },
    }

    $('.layui-btn').on('click', function () {
        var type = $(this).data('type');
        active[type] ? active[type].call(this) : '';
    });


    var ReportFilter = {};
    var ExportTitle;
    var ExportData;

    $(document).ready(function () {
        LoadReportType();

        form.on('select(ReportId)', function (data) {
            $("#Filter").val(ReportFilter[data.value]);
        });
    });

    function LoadReportType() {
        $.ajax({
            async: true,
            type: "post",
            data: null,
            url: "/management/Excelreport/Load",
            dataType: "json",
            success: function (result) {
                if (result.count > 0) {
                    $("#ReportId").empty();
                    $("#ReportId").append("<option style='display: none'></option>");

                    for (var k = 0; k < result.count; k++) {
                        var name = result.data[k]["Name"];
                        var value = result.data[k]["Id"];
                        var filter = result.data[k]["Params"];
                        $("#ReportId").append("<option value = '" + value + "'>" + name + "</option>");
                        ReportFilter[value] = filter;
                    }
                    form.render('select'); //只渲染下拉框
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layer.alert(errorThrown, { icon: 2, title: '提示' });
            }
        });

        $('#Filter').on('click', function () {
            var DictKey = $("#Filter").val();
            if (DictKey != undefined && DictKey != null && DictKey != "") {
                layer.open({
                    title: "参数",
                    area: ["500px", "300px"],
                    type: 1,
                    content: $('#DivConfigParams'),
                    btn: ['保存', '关闭'],
                    success: function (layero, index) {
                        LoadParams(DictKey);
                    },
                    yes: function (index) {
                        $("#Filter").val(GetParams());
                        layer.close(index);
                    },
                    btn2: function (index) {
                        layer.close(index);
                    }
                });
            }
        });

        function LoadParams(DictKey) {
            var oldobj = {};
            if (DictKey != undefined && DictKey != null && DictKey != "") {
                oldobj = JSON.parse(DictKey);
            }

            $("#ConfigParams").html("");

            var dom = '';
            for (var item in oldobj) {
                var value = item;

                var oldvalue = '';
                if (oldobj[value] !== undefined) {
                    oldvalue = oldobj[value];
                }
                dom += '<div class="layui-col-sm12">';
                dom += '<label class="layui-form-label layui-col-sm3">' + value + '</label>';
                dom += '<div class="layui-input-inline layui-col-sm9">';
                dom += '<input data-model="' + value + '" class="layui-input ParamsKey" type="text" autocomplete="off" lay-verify="" value="' + oldvalue + '">'
                dom += '</div></div>';
            }
            $("#ConfigParams").html(dom);
            form.render();
        }

        function GetParams() {
            var Params = {};
            $.each($(".ParamsKey"), function (key, value) {
                Params[$(this).data("model")] = $(this).val();
            });
            return JSON.stringify(Params);
        }
    }

    function QueryData(ReportId, Filter) {
        $.ajax({
            url: "/report/ExcelReport/QueryData",
            async: true,
            type: "post",
            data: { ReportId: ReportId, Filter: Filter },
            dataType: "json",
            success: function (result) {
                if (result.code == 500) {
                    layer.alert(result.msg, { icon: 2, title: '提示' });
                    $("#tableContainer").html(result.msg);
                    return;
                }
                ExportTitle = new Array();
                ExportData = result.data;
                var shead = "";
                var sdata = "";
                for (var i = 0; i < result.count; i++) {
                    var data = result.data[i];
                    sdata += "<tr>";
                    for (var item in data) {
                        if (i == 0) {
                            shead += "<th lay-data=\"{field:'" + item + "'}\">" + item + "</th>"
                            ExportTitle.push(item);
                        }
                        sdata += "<td>" + data[item] + "</td>";
                    }
                    sdata += "</tr>";
                }

                var TableHtml = "<table ";
                TableHtml += " lay-data=\"{";
                TableHtml += " height: 'full-120', ";
                TableHtml += " page: true, ";
                TableHtml += " limit: 15, ";
                TableHtml += " limits: [15, 1000], ";
                TableHtml += " toolbar: false, ";
                TableHtml += " defaultToolbar: ['filter','exports'], ";
                TableHtml += "}\"";
                TableHtml += " lay-filter=\"parse-table\">";
                TableHtml += "    <thead>";
                TableHtml += "        <tr>";
                TableHtml += shead;
                TableHtml += "        </tr>";
                TableHtml += "    </thead>";
                TableHtml += "    <tbody>";
                TableHtml += sdata;
                TableHtml += "    </tbody>";
                TableHtml += "</table>";

                $("#tableContainer").html(TableHtml);

                table.init('parse-table', { //转化静态表格
                    //height: 'full-60'
                });
            }
        });
    }
});