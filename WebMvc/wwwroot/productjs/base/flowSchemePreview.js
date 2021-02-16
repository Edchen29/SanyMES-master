layui.config({
    base: "/js/"
}).use(['form', 'vue', 'ztree', 'layer', 'utils', 'element', 'jquery', 'slimscroll',  'flow/gooflow', 'flowlayout'], function () {
    var form = layui.form, element = layui.element,
        layer = layui.layer,
        $ = layui.jquery;

    var index = layer.getFrameIndex(window.name); //获取窗口索引
    var id = $.getUrlParam("id");   //ID
    
    /*=========流程设计（begin）======================*/
    var flowDesignPanel = $('#flowPanel').flowdesign({
        height: 300,
        widht: 300,
        haveTool: false,
        OpenNode: function (object) {
            FlowDesignObject = object;  //为NodeInfo窗口提供调用
            $("[name='NodeName']").val(object.setInfo.NodeName),
                $("[name='NodeCode']").val(object.setInfo.NodeCode),
                $("[name='SortCode']").val(object.setInfo.SortCode),
            layer.open({
                type: 1,
                area: ['550px', '450px'], //宽高
                maxmin: true, //开启最大化最小化按钮
                title: '节点设置【' + object.name + '】',
                content: $('#nodeForm'),
                btn: ['确定', '取消'],
                yes: function (index, layero) {
                    layer.close(index);
                },
                cancel: function (index) {
                    layer.close(index);
                }
            });
        },
        OpenLine: function (id, object) {
            lay.msg("暂不能设置分支条件");
            return;
        }
    });
    /*=========流程设计（end）=====================*/


    $.getJSON('/flow/flowschemes/get?id=' + id,
        function (data) {
            var obj = data.Result;
            
            flowDesignPanel.loadData(JSON.parse(obj.SchemeContent));
            
            $("[name='SchemeCode']").val(obj.SchemeCode);
            $("[name='SchemeName']").val(obj.SchemeName);
            $("[name='SchemeType']").val(obj.SchemeType);
            $("[name='ProductId']").val(obj.ProductId);
            $("[name='MachineType']").val(obj.MachineType);
            if (obj.Disabled == 0) {
                $('input:checkbox[name="Disabled"]').prop('checked', false);
                form.render();
            } else {
                $("[name=Disabled]:checkbox").prop('checked', true);
                form.render();
            }
            $("[name='Description']").val(obj.Description);
        });


    flowDesignPanel.reinitSize($(window).width() - 30, $(window).height() - 100);
    $(window).resize(function () {
        flowDesignPanel.reinitSize($(window).width() - 30, $(window).height() - 100);
    });

    //让层自适应iframe
    layer.iframeAuto(index);

    $(".GooFlow_work").slimScroll({
        height: 'auto'
    });
})