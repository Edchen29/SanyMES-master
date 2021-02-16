layui.config({
    base: "/js/"
}).use(['form', 'element', 'vue', 'layer', 'laydate', 'jquery', 'table', 'hhweb', 'utils', 'Universal'], function () {
    var form = layui.form,
        layer = (top == undefined || top.layer === undefined) ? layui.layer : top.layer,
        element = layui.element,
        laydate = layui.laydate,
        $ = layui.jquery,
        table = layui.table,
        hhweb = layui.hhweb,
        Universal = layui.Universal;
    var thiswin = (top == undefined) ? window : top.window;    
    var AreaName = 'flow';
    var TableName = 'FlowSchemes';
    
    var vm = new Vue({
        el: '#modifyForm'
    });
    
    var vmq = new Vue({
        el: '#panelSearch',
        data: {
        }
    });
 
    var mainList = {
        Render: function () {
            var cols_arr = [[
                { checkbox: true, fixed: true }
                , { field: 'Id', width: 80, sort: true, fixed: false, hide: false, title: 'Id' }
                , { field: 'SchemeCode', width: 100, sort: true, fixed: false, hide: false, title: '工艺编号' }
                , { field: 'SchemeName', width: 150, sort: true, fixed: false, hide: false, title: '工艺名称' }
                , { field: 'SchemeType', width: 100, sort: true, fixed: false, hide: false, title: '工艺分类' }
                , { field: 'SchemeContent', width: 150, sort: true, fixed: false, hide: false, title: '流程内容' }
                , { field: 'ProductId', width: 100, sort: true, fixed: false, hide: false, title: '产品' }
                , { field: 'MachineType', width: 100, sort: true, fixed: false, hide: false, title: '机型' }
                , { field: 'DeleteMark', width: 100, sort: true, fixed: false, hide: false, title: '删除标记' }
                , { field: 'Disabled', width: 100, sort: true, fixed: false, hide: false, title: '是否禁用' }
                , { field: 'Description', width: 150, sort: true, fixed: false, hide: false, title: '备注' }
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


    //添加（编辑）对话框
    var editDlg = function () {

        var update = false;  //是否为更新
        var show = function (data) {
            var title = update ? "编辑信息" : "添加";
            layer.open({
                type: 2,
                area: ['800px', '700px'], //宽高
                maxmin: true, //开启最大化最小化按钮
                title: title,
                content: '/flow/flowschemes/design?id=' + data.Id,
                btn: ['保存', '关闭'],
                yes: function (index, layero) {
                    var iframeWin = thiswin[layero.find('iframe')[0]['name']]; //得到iframe页的窗口对象，执行iframe页的方法：iframeWin.method();
                    console.log(iframeWin);
                    iframeWin.submit();
                    layer.close(index);
                    table.reload('mainList', {});
                },
                btn2: function (index) {
                    layer.close(index);
                    table.reload('mainList', {});
                },
                cancel: function (index) {
                    layer.close(index);
                    table.reload('mainList', {});
                }
            });
        }
        return {
            add: function () { //弹出添加
                update = false;
                show({
                    Id: ''
                });
            },
            update: function (data) { //弹出编辑框
                update = true;
                show(data);
            }
        };
    }();


    var selfbtn = {
        //自定义按钮
        btnDel: function () {      //批量删除
            var checkStatus = table.checkStatus('mainList')
                , data = checkStatus.data;
            openauth.del("/flow/FlowSchemes/Delete",
                data.map(function (e) { return e.Id; }),
                mainList);
        }
        , btnAdd: function () {  //添加
            editDlg.add();
        }
        , btnSelfEdit: function () {  //编辑
            var checkStatus = table.checkStatus('mainList')
                , data = checkStatus.data;
            if (data.length != 1) {
                layer.msg("请选择编辑的行，且同时只能编辑一行");
                return;
            }
            editDlg.update(data[0]);
        }

        , btnPreview: function () {  //预览
            var checkStatus = table.checkStatus('mainList')
                , data = checkStatus.data;
            if (data.length != 1) {
                layer.msg("请选择要处理的流程，且同时只能选择一条");
                return;
            }

            layer.open({
                type: 2,
                area: ['800px', '700px'], //宽高
                maxmin: true, //开启最大化最小化按钮
                title: '工序流程详情预览',
                content: ['/flow/flowSchemes/preview?id=' + data[0].Id, 'no'],
                btn: ['关闭']
            });
        }
    };
    
    var selector = {
    };
    
    var vml = new Array({
        vm: vm,
        vmq: vmq,
    });
    
    Universal.BindSelector(vml, selector);
    Universal.mmain(AreaName, TableName, vm, vmq, EditInfo, selfbtn, mainList);
});