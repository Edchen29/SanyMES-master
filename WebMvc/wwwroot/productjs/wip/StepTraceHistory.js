﻿layui.config({
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
    
    var AreaName = 'wip';
    var TableName = 'StepTraceHistory';
    
    var vm = new Vue({
        el: '#modifyForm'
    });
    var vmq = new Vue({
        el: '#panelSearch',
        data: {
        }
    });
    
    hhweb.Config = {
        'StationInTime': vm,
        'StationOutTime': vm,
        'LineInTime': vm,
        'LineOutTime': vm,
        'CreateTime': vm,
        'UpdateTime': vm,

        'qStationInTime': vmq,
        'qStationOutTime': vmq,
        'qLineInTime': vmq,
        'qLineOutTime': vmq,
        'qCreateTime': vmq,
        'qUpdateTime': vmq,
    };
      
    var mainList = {
        Render: function () {
            var cols_arr = [[
                { checkbox: true, fixed: true }
                , {field:'Id', width:80, sort: true, fixed: false, hide: false, title: 'Id' }
                , {field:'WONumber', width:150, sort: true, fixed: false, hide: false, title: '工单号' }
                , {field:'ProductCode', width:150, sort: true, fixed: false, hide: false, title: '产品' }
                , {field:'SerialNumber', width:150, sort: true, fixed: false, hide: false, title: '序列号' }
                , { field: 'LineId', width: 150, sort: true, fixed: false, hide: false, title: '线体', templet: function (d) { return GetLabel('LineId', 'Id', 'LineName', d.LineId) } }
                , { field: 'StepId', width: 150, sort: true, fixed: false, hide: false, title: '当前工序', templet: function (d) { return GetLabel('StepId', 'Id', 'Code', d.StepId) } }
                , { field: 'StationId', width: 150, sort: true, fixed: false, hide: false, title: '当前工位', templet: function (d) { return GetLabel('StationId', 'Id', 'Name', d.StationId) } }
                , { field: 'NextStepId', width: 150, sort: true, fixed: false, hide: false, title: '下一工序', templet: function (d) { return GetLabel('NextStepId', 'Id', 'Code', d.NextStepId) } }
                , { field: 'IsNG', width: 100, sort: true, fixed: false, hide: false, title: '是否不良', templet: function (d) { return GetLabel('IsNG', 'DictValue', 'DictLabel', d.IsNG) } }
                , { field: 'NGcode', width: 150, sort: true, fixed: false, hide: false, title: '不良代号', templet: function (d) { return GetLabel('NGcode', 'Code', 'Name', d.NGcode) } }
                , { field: 'IsInvalid', width: 100, sort: true, fixed: false, hide: false, title: '是否报废', templet: function (d) { return GetLabel('IsInvalid', 'DictValue', 'DictLabel', d.IsInvalid) } }
                , {field:'StationInTime', width:150, sort: true, fixed: false, hide: false, title: '进站时间' }
                , {field:'StationOutTime', width:150, sort: true, fixed: false, hide: false, title: '出站时间' }
                , {field:'LineInTime', width:150, sort: true, fixed: false, hide: false, title: '进线时间' }
                , {field:'LineOutTime', width:150, sort: true, fixed: false, hide: false, title: '出线时间' }
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
    };

    var selector = {
        'LineId': {
            SelType: "FromUrl",
            SelFrom: "/configure/Line/Load",
            SelModel: "LineId",
            SelLabel: "LineName",
            SelValue: "Id",
            Dom: [$("[name='LineId']"), $("[name='qLineId']"), $("[name='TLineId']"), $("[name='NGLineId']")]
        }, 'StepId': {
            SelType: "FromUrl",
            SelFrom: "/configure/Step/Load",
            SelModel: "StepId",
            SelLabel: "Code",
            SelValue: "Id",
            Dom: [$("[name='StepId']"), $("[name='qStepId']"), $("[name='NowStepId']"), $("[name='ToStep']"), $("[name='NGNowStepId']")]
        }, 'NextStepId': {
            SelType: "FromUrl",
            SelFrom: "/configure/Step/Load",
            SelModel: "NextStepId",
            SelLabel: "Code",
            SelValue: "Id",
            Dom: [$("[name='NextStepId']"), $("[name='qNextStepId']")]
        }, 'StationId': {
            SelType: "FromUrl",
            SelFrom: "/configure/Station/Load",
            SelModel: "StationId",
            SelLabel: "Name",
            SelValue: "Id",
            Dom: [$("[name='StationId']"), $("[name='qStationId']"), $("[name='NowStationId']"), $("[name='NGNowStationId']")]
        }, 'IsNG': {
            SelType: "FromDict",
            SelFrom: "Is_yes_no",
            SelModel: "IsNG",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='IsNG']"), $("[name='qIsNG']")]
        }, 'IsInvalid': {
            SelType: "FromDict",
            SelFrom: "Is_yes_no",
            SelModel: "IsInvalid",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='IsInvalid']"), $("[name='qIsInvalid']")]
        },'NGCode': {
            SelType: "FromUrl",
            SelFrom: "/repair/DefectCode/Load",
            SelModel: "NGCode",
            SelLabel: "Name",
            SelValue: "Code",
            Dom: [$("[name='NGcode']"), $("[name='qNGcode']"),$("[name='NGCode']")]
        }, 'NGType': {
            SelType: "FromDict",
            SelFrom: "NGType",
            SelModel: "NGType",
            SelLabel: "DictLabel",
            SelValue: "DictValue",
            Dom: [$("[name='NGType']")]
        },
    };
    
    var vml = new Array({
        vm: vm,
        vmq: vmq,
    });
    
    Universal.BindSelector(vml, selector);
    Universal.mmain(AreaName, TableName, vm, vmq, EditInfo, selfbtn, mainList);
});