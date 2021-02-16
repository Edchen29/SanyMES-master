layui.config({
    base: "/js/"
}).use(['form', 'vue', 'ztree', 'layer', 'jquery', 'table', 'droptree', 'hhweb', 'utils'], function () {
    var form = layui.form,
        layer = layui.layer,
        $ = layui.jquery;
    var table = layui.table;
    var hhweb = layui.hhweb;
    var id = $.getUrlParam("id");      //待分配的id
    layui.droptree("/base/UserSession/GetOrgs", "#Organizations", "#OrganizationIds");
    var menudata = new Array(); //菜单数据

    //主列表加载，可反复调用进行刷新
    var config = {};  //table的参数，如搜索key，点击tree的id
    var mainList = function (options) {
        if (options != undefined) {
            $.extend(config, options);
        }
        table.reload('mainList', {
            url: '/base/SysRole/Load'
            , method: 'POST'
            , where: config
            , done: function (res, curr, count) {
                //如果是异步请求数据方式，res即为你接口返回的信息。
                //如果是直接赋值的方式，res即为：{data: [], count: 99} data为当前页数据、count为数据总长度

                $.ajax("/base/SysRole/LoadForUser", {
                    async: true
                    , type: "post"
                    , data: { userId: id}
                    , dataType: 'json'
                    , success: function (result) {
                        if (result.Code == 500) return;
                        var roles = result.Result;
                        //循环所有数据，找出对应关系，设置checkbox选中状态
                        for (var i = 0; i < res.data.length; i++) {
                            //获取当前所有菜单的ID数组
                            menudata[i] = res.data[i].Id;

                            for (var j = 0; j < roles.length; j++) {
                                if (res.data[i].Id != roles[j]) continue;

                                //这里才是真正的有效勾选
                                res.data[i]["LAY_CHECKED"] = true;
                                //找到对应数据改变勾选样式，呈现出选中效果
                                var index = res.data[i]['LAY_TABLE_INDEX'];
                                $('.layui-table-fixed-l tr[data-index=' + index + '] input[type="checkbox"]').prop('checked', true);
                                $('.layui-table-fixed-l tr[data-index=' + index + '] input[type="checkbox"]').next().addClass('layui-form-checked');
                            }
                        }

                        //如果构成全选
                        var checkStatus = table.checkStatus('mainList');
                        if (checkStatus.isAll) {
                            $('.layui-table-header th[data-field="0"] input[type="checkbox"]').prop('checked', true);
                            $('.layui-table-header th[data-field="0"] input[type="checkbox"]').next().addClass('layui-form-checked');
                        }
                    }
                });
            }
        });
    }
    //左边树状机构列表
    var ztree = function () {
        var url = '/base/UserSession/GetOrgs';
        var zTreeObj;
        var setting = {
            view: { selectedMulti: false },
            data: {
                key: {
                    name: 'Name',
                    title: 'Name'
                },
                simpleData: {
                    enable: true,
                    idKey: 'Id',
                    pIdKey: 'ParentId',
                    rootPId: ""
                }
            },
            callback: {
                onClick: function (event, treeId, treeNode) {
                    mainList({ orgId: treeNode.Id });
                }
            }
        };
        var load = function () {
            $.getJSON(url, function (json) {
                zTreeObj = $.fn.zTree.init($("#tree"), setting);
                var newNode = { Name: "根节点", Id: null, ParentId: "" };
                json.push(newNode);
                zTreeObj.addNodes(null, json);
                mainList({ orgId: "" });
                zTreeObj.expandAll(true);
            });
        };
        load();
        return {
            reload: load
        }
    }();
    $("#tree").height($("div.layui-table-view").height());


    //分配及取消分配
    table.on('checkbox(list)', function (obj) {
        var editdata = GetObjData(obj);

        var url = "/base/SysRelevance/Assign";
        if (!obj.checked) {
            url = "/base/SysRelevance/UnAssign";
        }
        $.post(url, { type: "UserRole", firstId: id, secIds: editdata }
            , function (data) {
                layer.msg(data.Message);
            }
            , "json");
    });

    function GetObjData(obj) {
        var editdata;
        if (obj.type == "one") {
            if (obj.data.Id == undefined) {
                if (obj.checked) {
                    editdata = menudata
                }
                else {
                    var checkStatus = table.checkStatus('mainList');
                    var data = checkStatus.data; //获取选中行的数据
                    if (data.length == 0) {
                        editdata = menudata
                    }
                    else {
                        var tmpary = new Array();
                        var datacopy = new Array();
                        for (var i = 0; i < data.length; i++) {
                            datacopy[i] = data[i].Id;
                        }
                        for (var i = 0; i < menudata.length; i++) {
                            if (datacopy.indexOf(menudata[i]) < 0) {
                                tmpary[tmpary.length] = menudata[i];
                            }
                        }
                        editdata = tmpary;
                    }
                }
            }
            else {
                editdata = obj.data.Id;
            }
        }
        else {
            editdata = menudata
        }

        return editdata;
    }
    //监听页面主按钮操作 end
})