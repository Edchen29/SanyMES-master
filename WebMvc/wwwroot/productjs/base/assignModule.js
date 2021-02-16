layui.config({
    base: "/js/"
}).use(['form', 'vue', 'ztree', 'layer', 'jquery', 'utils', 'table'], function () {
    var layer = (top == undefined || top.layer === undefined) ? layui.layer : top.layer,
        $ = layui.jquery;
    var table = layui.table;
    var id = $.getUrlParam("id");      //待分配的id
    var type = $.getUrlParam("type");  //待分配的类型
    var menuType = $.getUrlParam("menuType");  //待分配菜单的类型
    var menudata = new Array(); //菜单数据

    //菜单列表
    var menucon = {};  //table的参数，如搜索key，点击tree的id

    var mainList = function (options) {
        if (options != undefined) {
            $.extend(menucon, options);
        }
        table.reload('mainList', {
            url: '/base/SysModule/LoadMenus'
            , method: 'POST'
            , where: menucon
            , done: function (res, curr, count) {
                //如果是异步请求数据方式，res即为你接口返回的信息。
                //如果是直接赋值的方式，res即为：{data: [], count: 99} data为当前页数据、count为数据总长度
                var url = "/base/SysModule/LoadMenusForUser";
                if (type.indexOf("Role") != -1) {
                    url = "/base/SysModule/LoadMenusForRole";
                }

                $.ajax(url, {
                    async: true
                    , type: "post"
                    , data: {
                        firstId: id
                        , moduleId: options.moduleId
                    }
                    , dataType: "json"
                    , success: function (result) {
                        //循环所有数据，找出对应关系，设置checkbox选中状态
                        for (var i = 0; i < res.data.length; i++) {
                            //获取当前所有菜单的ID数组
                            menudata[i] = res.data[i].Id;

                            for (var j = 0; j < result.length; j++) {
                                if (res.data[i].Id != result[j].Id) continue;

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

    //模块列表
    var ztree = function () {
        var url = '/base/UserSession/GetModules';
        var zTreeObj;
        var setting = {
            view: { selectedMulti: true },
            check: {
                enable: true,
                chkStyle: "checkbox",
                chkboxType: { "Y": "", "N": "" } //去掉勾选时级联
            },
            data: {
                key: {
                    name: 'Name',
                    title: 'Name'
                },
                simpleData: {
                    enable: true,
                    idKey: 'Id',
                    pIdKey: 'ParentId',
                    rootPId: 'null'
                }
            },
            callback: {
                onClick: function (event, treeId, treeNode) {
                    mainList({ moduleId: treeNode.Id });
                },
                onCheck: function (event, treeId, treeNode) {
                    var url = "/base/SysRelevance/Assign";
                    if (!treeNode.checked) {
                        url = "/base/SysRelevance/UnAssign";
                    }

                    $.post(url, { type: type, firstId: id, secIds: [treeNode.Id] }
                        , function (data) {
                            layer.msg(data.Message);
                        }
                        , "json");
                }
            }
        };
        var load = function () {
            $.getJSON(url, function (json) {
                zTreeObj = $.fn.zTree.init($("#tree"), setting);
                zTreeObj.addNodes(null, json);
                //如果该用户已经分配模块了，则设置相应的状态
                var url = "/base/SysModule/LoadForUser";
                if (type.indexOf("Role") != -1) {
                    url = "/base/SysModule/LoadForRole";
                }
                $.getJSON(url, { firstId: id }
                    , function (data) {
                        $.each(data,
                            function (i) {
                                var that = this;
                                var node = zTreeObj.getNodeByParam("Id", that.Id, null);
                                zTreeObj.checkNode(node, true, false);
                            });
                    });

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
        $.post(url, { type: menuType, firstId: id, secIds: editdata }
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