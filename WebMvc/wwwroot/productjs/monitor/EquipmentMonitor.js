layui.config({
    base: "/js/"
}).use(['form', 'element', 'vue', 'layer', 'laydate', 'jquery', 'table', 'hhweb', 'utils', 'Universal', 'laytpl'], function () {
    var form = layui.form,
        layer = layui.layer,
        laytpl = layui.laytpl,
        element = layui.element,
        laydate = layui.laydate,
        $ = layui.jquery,
        table = layui.table,
        hhweb = layui.hhweb,
        Universal = layui.Universal;
    $(document).ready(function () {
        
    var AreaName = 'monitor';
    var TableName = 'EquipmentMonitor';

    var vm = new Vue({
        el: '#modifyForm'
    });
    var vmq = new Vue({
        el: '#panelSearch',
        data: {
        }
    });

    hhweb.Config = {

    };
        

    var mainList = {
        Render: function () {
            var cols_arr = [[
               // { field: 'Id', width: 60, sort: true, fixed: false, hide: false, title: 'Id' } ,
                { field: 'EquipmentCode', width: 150, sort: true, fixed: false, hide: false, title: '设备编号' }
                , { field: 'EquipmentName', width: 150, sort: true, fixed: false, hide: false, title: '设备名称' }
                , { field: 'FactoryName', width: 150, sort: true, fixed: false, hide: false, title: '生产工厂' }
                , { field: 'WorkshopName', width: 100, sort: true, fixed: false, hide: false, title: '生产车间' }
                , { field: 'LineName', width: 150, sort: true, fixed: false, hide: false, title: '线体' }
                , {
                    field: 'StepName', width: 150, sort: true, fixed: false, hide: false, title: '工序', templet: function (d) { if (d.StepName != null && d.StepName != '') { return '<b style="color:green">' + d.StepName + '</b>' } else { return '' } }
                }
                , { field: 'StationName', width: 150, sort: true, fixed: false, hide: false, title: '工位' }
                , { field: 'Enable', width: 100, sort: true, fixed: false, hide: false, title: '是否生效', templet: function (d) { if (d.Enable == '生效') { return '<b style="color:green">生效<a class="layui-icon layui-icon-ok" style="position:absolute;top:2px;right:30px;color:red"></a></b>' } else { return '<b style="color:red">失效<a class="layui-icon layui-icon-close" style="position:absolute;top:2px;right:30px;color:red"></a></b>' } } }
                , { field: 'BeReady', width: 100, sort: true, fixed: false, hide: false, title: '是否就绪', templet: function (d) { if (d.BeReady == '就绪') { return '<b style="color:Navy">' + d.BeReady + '</b>' } else { return '<b style="color:red">' + d.BeReady + '</b>' } } }
                , { field: 'Status', width: 110, sort: true, fixed: false, hide: false, title: '设备状态'
                    , templet: function (d) { if (d.Status == '生产') { return '<b style="color:green">生产中<a class="layui-icon layui-icon-loading" style="position:absolute;top:2px;right:30px;color:green"></a></b>' } else if (d.Status == '故障') { return '<b style="color:red">设备故障<a class="layui-icon layui-icon-util" style="position:absolute;top:2px;right:20px;color:red"></a></b>' } else if (d.Status == '报警') { return '<b style="color:orange">' + d.Status + '<a class="layui-icon layui-icon-util" style="position:absolute;top:2px;right:20px;color:orange"></a></b>' } else if (d.Status == '停机') { return '<b style="color:gray">' + d.Status + '<a class="layui-icon layui-icon-pause" style="position:absolute;top:2px;right:20px;color:gray"></a></b>' } else { return '<b style="color:DeepSkyBlue">空闲<a class="layui-icon layui-icon-log" style="position:absolute;top:2px;right:30px;color:DeepSkyBlue"></a></b>' } }}
                , { field: 'WONumber', width: 150, sort: true, fixed: false, hide: false, title: '工单号', templet: function (d) { if (d.WONumber != null) { return '<b style="color:green">' + d.WONumber + '</b>' } else { return '' } } }
                , { field: 'ProductCode', width: 150, sort: true, fixed: false, hide: false, title: '产品', templet: function (d) { if (d.ProductCode != null) { return '<b style="color:green">' + d.ProductCode + '</b>' } else { return '' } }  }
                , { field: 'SerialNumber', width: 150, sort: true, fixed: false, hide: false, title: '产品序号', templet: function (d) { if (d.SerialNumber != null) { return '<b style="color:green">' + d.SerialNumber + '</b>' } else { return '' } }  }
                , { field: 'TypeName', width: 100, sort: true, fixed: false, hide: false, title: '设备类型' }
                , { field: 'IP', width: 120, sort: true, fixed: false, hide: false, title: '设备IP' }
            ]];

            mainList.Table = table.render({
                elem: '#mainList'
                , url: "/" + AreaName + "/" + TableName + "/Load"
                , method: "post"
                , page: false //开启分页
                , cols: hhweb.ColumnSetting('mainList', cols_arr)
                , id: 'mainList'
                , limit: 500
                , limits: [500, 1000]
                , defaultToolbar: ['filter']
               // , toolbar: '#toolbarTable'
               // , height: 'full-1'
                , cellMinWidth: 80
                , size: 'sm'
                , done: function (res) {
                    var data1 = [], data2 = [], data3 = [], data4 = [], data5 = [];//岛一
                    var data21 = [], data22 = [], data23 = [], data24 = [], data25 = [];//岛二
                    var data31 = [], data32 = [], data33 = [], data34 = [], data35 = [];//岛三
                    for (var i = 0; i <= res.data.length; i++) {
                        if (typeof res.data[i] !== 'undefined') {
                            //console.log(res.data[i]);
                            if (res.data[i].WorkshopCode=="A1") {
                                if (res.data[i].Status == "生产") {
                                    data1.push({
                                        EquipmentCode: res.data[i].EquipmentCode,
                                        EquipmentName: res.data[i].EquipmentName,
                                        LineName: res.data[i].LineName,
                                        StationName: res.data[i].StationName,
                                        StepName: res.data[i].StepName,
                                        WONumber: res.data[i].WONumber,
                                        ProductCode: res.data[i].ProductCode,
                                        SerialNumber: res.data[i].SerialNumber,
                                        Status: res.data[i].Status
                                    });
                                } else if (res.data[i].Status == "空闲") {
                                    data2.push({
                                        EquipmentCode: res.data[i].EquipmentCode,
                                        EquipmentName: res.data[i].EquipmentName,
                                        LineName: res.data[i].LineName,
                                        StationName: res.data[i].StationName,
                                        StepName: res.data[i].StepName,
                                        WONumber: res.data[i].WONumber,
                                        ProductCode: res.data[i].ProductCode,
                                        SerialNumber: res.data[i].SerialNumber,
                                        Status: res.data[i].Status
                                    });
                                } else if (res.data[i].Status == "报警") {
                                    data4.push({
                                        EquipmentCode: res.data[i].EquipmentCode,
                                        EquipmentName: res.data[i].EquipmentName,
                                        LineName: res.data[i].LineName,
                                        StationName: res.data[i].StationName,
                                        StepName: res.data[i].StepName,
                                        WONumber: res.data[i].WONumber,
                                        ProductCode: res.data[i].ProductCode,
                                        SerialNumber: res.data[i].SerialNumber,
                                        Status: res.data[i].Status
                                    });
                                } else if (res.data[i].Status == "故障") {
                                    data5.push({
                                        EquipmentCode: res.data[i].EquipmentCode,
                                        EquipmentName: res.data[i].EquipmentName,
                                        LineName: res.data[i].LineName,
                                        StationName: res.data[i].StationName,
                                        StepName: res.data[i].StepName,
                                        WONumber: res.data[i].WONumber,
                                        ProductCode: res.data[i].ProductCode,
                                        SerialNumber: res.data[i].SerialNumber,
                                        Status: res.data[i].Status
                                    });
                                } else if (res.data[i].Status == "停机") {
                                    data3.push({
                                        EquipmentCode: res.data[i].EquipmentCode,
                                        EquipmentName: res.data[i].EquipmentName,
                                        LineName: res.data[i].LineName,
                                        StationName: res.data[i].StationName,
                                        StepName: res.data[i].StepName,
                                        WONumber: res.data[i].WONumber,
                                        ProductCode: res.data[i].ProductCode,
                                        SerialNumber: res.data[i].SerialNumber,
                                        Status: res.data[i].Status
                                    });
                                }
                            } else if (res.data[i].WorkshopCode == "A2") {
                                if (res.data[i].Status == "生产") {
                                    data21.push({
                                        EquipmentCode: res.data[i].EquipmentCode,
                                        EquipmentName: res.data[i].EquipmentName,
                                        LineName: res.data[i].LineName,
                                        StationName: res.data[i].StationName,
                                        StepName: res.data[i].StepName,
                                        WONumber: res.data[i].WONumber,
                                        ProductCode: res.data[i].ProductCode,
                                        SerialNumber: res.data[i].SerialNumber,
                                        Status: res.data[i].Status
                                    });
                                } else if (res.data[i].Status == "空闲") {
                                    data22.push({
                                        EquipmentCode: res.data[i].EquipmentCode,
                                        EquipmentName: res.data[i].EquipmentName,
                                        LineName: res.data[i].LineName,
                                        StationName: res.data[i].StationName,
                                        StepName: res.data[i].StepName,
                                        WONumber: res.data[i].WONumber,
                                        ProductCode: res.data[i].ProductCode,
                                        SerialNumber: res.data[i].SerialNumber,
                                        Status: res.data[i].Status
                                    });
                                } else if (res.data[i].Status == "报警") {
                                    data24.push({
                                        EquipmentCode: res.data[i].EquipmentCode,
                                        EquipmentName: res.data[i].EquipmentName,
                                        LineName: res.data[i].LineName,
                                        StationName: res.data[i].StationName,
                                        StepName: res.data[i].StepName,
                                        WONumber: res.data[i].WONumber,
                                        ProductCode: res.data[i].ProductCode,
                                        SerialNumber: res.data[i].SerialNumber,
                                        Status: res.data[i].Status
                                    });
                                } else if (res.data[i].Status == "故障") {
                                    data25.push({
                                        EquipmentCode: res.data[i].EquipmentCode,
                                        EquipmentName: res.data[i].EquipmentName,
                                        LineName: res.data[i].LineName,
                                        StationName: res.data[i].StationName,
                                        StepName: res.data[i].StepName,
                                        WONumber: res.data[i].WONumber,
                                        ProductCode: res.data[i].ProductCode,
                                        SerialNumber: res.data[i].SerialNumber,
                                        Status: res.data[i].Status
                                    });
                                } else if (res.data[i].Status == "停机") {
                                    data23.push({
                                        EquipmentCode: res.data[i].EquipmentCode,
                                        EquipmentName: res.data[i].EquipmentName,
                                        LineName: res.data[i].LineName,
                                        StationName: res.data[i].StationName,
                                        StepName: res.data[i].StepName,
                                        WONumber: res.data[i].WONumber,
                                        ProductCode: res.data[i].ProductCode,
                                        SerialNumber: res.data[i].SerialNumber,
                                        Status: res.data[i].Status
                                    });
                                }
                            } else {
                                if (res.data[i].Status == "生产") {
                                    data31.push({
                                        EquipmentCode: res.data[i].EquipmentCode,
                                        EquipmentName: res.data[i].EquipmentName,
                                        LineName: res.data[i].LineName,
                                        StationName: res.data[i].StationName,
                                        StepName: res.data[i].StepName,
                                        WONumber: res.data[i].WONumber,
                                        ProductCode: res.data[i].ProductCode,
                                        SerialNumber: res.data[i].SerialNumber,
                                        Status: res.data[i].Status
                                    });
                                } else if (res.data[i].Status == "空闲") {
                                    data32.push({
                                        EquipmentCode: res.data[i].EquipmentCode,
                                        EquipmentName: res.data[i].EquipmentName,
                                        LineName: res.data[i].LineName,
                                        StationName: res.data[i].StationName,
                                        StepName: res.data[i].StepName,
                                        WONumber: res.data[i].WONumber,
                                        ProductCode: res.data[i].ProductCode,
                                        SerialNumber: res.data[i].SerialNumber,
                                        Status: res.data[i].Status
                                    });
                                } else if (res.data[i].Status == "报警") {
                                    data34.push({
                                        EquipmentCode: res.data[i].EquipmentCode,
                                        EquipmentName: res.data[i].EquipmentName,
                                        LineName: res.data[i].LineName,
                                        StationName: res.data[i].StationName,
                                        StepName: res.data[i].StepName,
                                        WONumber: res.data[i].WONumber,
                                        ProductCode: res.data[i].ProductCode,
                                        SerialNumber: res.data[i].SerialNumber,
                                        Status: res.data[i].Status
                                    });
                                } else if (res.data[i].Status == "故障") {
                                    data35.push({
                                        EquipmentCode: res.data[i].EquipmentCode,
                                        EquipmentName: res.data[i].EquipmentName,
                                        LineName: res.data[i].LineName,
                                        StationName: res.data[i].StationName,
                                        StepName: res.data[i].StepName,
                                        WONumber: res.data[i].WONumber,
                                        ProductCode: res.data[i].ProductCode,
                                        SerialNumber: res.data[i].SerialNumber,
                                        Status: res.data[i].Status
                                    });
                                } else if (res.data[i].Status == "停机") {
                                    data33.push({
                                        EquipmentCode: res.data[i].EquipmentCode,
                                        EquipmentName: res.data[i].EquipmentName,
                                        LineName: res.data[i].LineName,
                                        StationName: res.data[i].StationName,
                                        StepName: res.data[i].StepName,
                                        WONumber: res.data[i].WONumber,
                                        ProductCode: res.data[i].ProductCode,
                                        SerialNumber: res.data[i].SerialNumber,
                                        Status: res.data[i].Status
                                    });
                                }
                            }
                        }

                    }
                    //渲染岛一模版
                    var gdata = { //数据
                        "title": "测试样式"
                        , "list": data1
                    };
                    var bdata = { //数据
                        "title": "测试样式"
                        , "list": data2
                    };
                    var sdata = { //数据
                        "title": "测试样式"
                        , "list": data3
                    };
                    var fdata = { //数据
                        "title": "测试样式"
                        , "list": data4
                    };
                    var beddata = { //数据
                        "title": "测试样式"
                        , "list": data5
                    };

                    //岛二
                    var gdata2 = { //数据
                        "title": "测试样式"
                        , "list": data21
                    };
                    var bdata2 = { //数据
                        "title": "测试样式"
                        , "list": data22
                    };
                    var sdata2 = { //数据
                        "title": "测试样式"
                        , "list": data23
                    };
                    var fdata2 = { //数据
                        "title": "测试样式"
                        , "list": data24
                    };
                    var beddata2 = { //数据
                        "title": "测试样式"
                        , "list": data25
                    };
                    //岛三
                    var gdata3 = { //数据
                        "title": "测试样式"
                        , "list": data31
                    };
                    var bdata3 = { //数据
                        "title": "测试样式"
                        , "list": data32
                    };
                    var sdata3 = { //数据
                        "title": "测试样式"
                        , "list": data33
                    };
                    var fdata3 = { //数据
                        "title": "测试样式"
                        , "list": data34
                    };
                    var beddata3 = { //数据
                        "title": "测试样式"
                        , "list": data35
                    };
                    //岛一
                    var getTpl = greendata.innerHTML
                        , view = document.getElementById('greenview');
                    laytpl(getTpl).render(gdata, function (html) {
                        view.innerHTML = html;
                    });
                    var getTp2 = bluedata.innerHTML
                        , view = document.getElementById('blueview');
                    laytpl(getTp2).render(bdata, function (html) {
                        view.innerHTML = html;
                    });

                    var getTp3 = graydata.innerHTML
                        , view = document.getElementById('grayview');
                    laytpl(getTp3).render(sdata, function (html) {
                        view.innerHTML = html;
                    });
                    var getTp4 = orangedata.innerHTML
                        , view = document.getElementById('orangeview');
                    laytpl(getTp4).render(fdata, function (html) {
                        view.innerHTML = html;
                    });
                    var getTp5 = reddata.innerHTML
                        , view = document.getElementById('redview');
                    laytpl(getTp5).render(beddata, function (html) {
                        view.innerHTML = html;
                    });
                    //岛二
                    var getTp2l = greendata2.innerHTML
                        , view2 = document.getElementById('greenview2');
                    laytpl(getTp2l).render(gdata2, function (html) {
                        view2.innerHTML = html;
                    });
                    var getTp22 = bluedata2.innerHTML
                        , view2 = document.getElementById('blueview2');
                    laytpl(getTp22).render(bdata2, function (html) {
                        view2.innerHTML = html;
                    });

                    var getTp23 = graydata2.innerHTML
                        , view2 = document.getElementById('grayview2');
                    laytpl(getTp23).render(sdata2, function (html) {
                        view2.innerHTML = html;
                    });
                    var getTp24 = orangedata2.innerHTML
                        , view2 = document.getElementById('orangeview2');
                    laytpl(getTp24).render(fdata2, function (html) {
                        view2.innerHTML = html;
                    });
                    var getTp25 = reddata2.innerHTML
                        , view2 = document.getElementById('redview2');
                    laytpl(getTp25).render(beddata2, function (html) {
                        view2.innerHTML = html;
                    });
                    //岛三
                    var getTp3l = greendata3.innerHTML
                        , view3 = document.getElementById('greenview3');
                    laytpl(getTp3l).render(gdata3, function (html) {
                        view3.innerHTML = html;
                    });
                    var getTp32 = bluedata3.innerHTML
                        , view3 = document.getElementById('blueview3');
                    laytpl(getTp32).render(bdata3, function (html) {
                        view3.innerHTML = html;
                    });

                    var getTp33 = graydata3.innerHTML
                        , view3 = document.getElementById('grayview3');
                    laytpl(getTp33).render(sdata3, function (html) {
                        view3.innerHTML = html;
                    });
                    var getTp34 = orangedata3.innerHTML
                        , view3 = document.getElementById('orangeview3');
                    laytpl(getTp34).render(fdata3, function (html) {
                        view3.innerHTML = html;
                    });
                    var getTp35 = reddata3.innerHTML
                        , view3 = document.getElementById('redview3');
                    laytpl(getTp35).render(beddata3, function (html) {
                        view3.innerHTML = html;
                    });
                }
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
        //clearInterval(timeTicket);
        //var timeTicket = setInterval(function () {
        //    table.reload('mainList', {});
        //}, 9000);
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
        console.log(list, ' ', data)
        //表单修改时填充需修改的数据
        form.val('modifyForm', list);
    };
    var selfbtn = {
        //自定义按钮
    };

    var selector = {
    };

    var vml = new Array({
        vm: vm,
        vmq: vmq,
    });

    Universal.BindSelector(vml, selector);
    Universal.mmain(AreaName, TableName, vm, vmq, EditInfo, selfbtn, mainList);
    })
});