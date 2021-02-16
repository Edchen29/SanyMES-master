layui.config({
    base: "/js/"
}).use(['form', 'vue', 'ztree', 'layer', 'utils', 'element', 'jquery', 'table', 'droptree', 'flow/gooflow', 'flowlayout'], function () {
    var form = layui.form, element = layui.element,
        layer = layui.layer,
        $ = layui.jquery;
    var table = layui.table;

    var index = layer.getFrameIndex(window.name); //获取窗口索引
    var id = $.getUrlParam("id");   //ID
    var update = (id != null && id != '');
    //提交的URL
    var url = "/flow/FlowSchemes/Ins";

    var vm = new Vue({
        el: "#formEdit",
        data() {
            return {
                    Id: '',
                    ProductId: '',
                forms: [],
                frmPreview: ''
            }
        }
    });

    /*=========流程设计（begin）======================*/
    var flowDesignPanel = $('#flowPanel').flowdesign({
        height: 300,
        widht: 300,
        OpenNode: function (object) {
            FlowDesignObject = object;  //为NodeInfo窗口提供调用

            if (object.type == 'start round mix' || object.type == 'end round') {
                layer.msg("开始节点与结束节点不能设置");
                return false;
            }

            layer.open({
                type: 1,
                area: ['550px', '450px'], //宽高
                maxmin: true, //开启最大化最小化按钮
                title: '节点设置【' + object.name + '】',
                content: $('#nodeForm'),
                btn: ['确定', '取消'],
                yes: function (index, layero) {
                    var body = layer.getChildFrame('body', index);
                    var nodedata = {
                            NodeName: $("[name='NodeName']").val(),
                            NodeCode: $("[name='NodeCode']").val(),
                            SortCode: $("[name='SortCode']").val(),
                    };
                    flowDesignPanel.SetNodeEx(object.id, nodedata);
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

    if (update) {
        $.getJSON('/flow/FlowSchemes/get?id=' + id,
            function (data) {
                var obj = data.Result;
                
                url = "/flow/FlowSchemes/Upd";
                vm.$set('$data', obj);

                $('input:checkbox[name="Disabled"][value="' + obj.Disabled + '"]').prop('checked', true);
                form.render();

                flowDesignPanel.loadData(JSON.parse(obj.SchemeContent));
            });
    } else {
        vm.$set('$data',
            {
                Id: ''
                , SchemeCode: ''
            });
    }

    //提交数据
    form.on('submit(formSubmit)',
        function (data) {
            var content = flowDesignPanel.exportDataEx();
            if (content == -1) {
                return false; //阻止表单跳转。
            }
            var schemecontent = {
                SchemeContent: JSON.stringify(content)
            }

            $.extend(data.field, schemecontent);
            $.post(url,
                data.field,
                function (result) {
                    layer.msg(result.Message);
                },
                "json");

            return false; //阻止表单跳转。
        });

    flowDesignPanel.reinitSize($(window).width() - 30, $(window).height() - 100);
    $(window).resize(function () {
        flowDesignPanel.reinitSize($(window).width() - 30, $(window).height() - 100);
    });

    //该函数供给父窗口确定时调用
    submit = function () {
        //只能用隐藏的submit btn才行，用form.submit()时data.field里没有数据
        $("#btnSubmit").click();
    }

    //让层自适应iframe
    layer.iframeAuto(index);
    })