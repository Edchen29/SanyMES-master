layui.config({
    base: "/js/"
}).use(['form', 'vue', 'ztree', 'layer', 'jquery', 'table', 'droptree', 'hhweb', 'utils'], function () {
    var form = layui.form,
        layer = layui.layer,
        $ = layui.jquery;
    var table = layui.table;
    var hhweb = layui.hhweb;
    var toplayer = (top == undefined || top.layer === undefined) ? layer : top.layer;  //顶层的LAYER
    layui.droptree("/base/UserSession/GetOrgs", "#Organizations", "#OrganizationIds");

    $("#menus").loadMenus("SysUser", 1);

    //主列表加载，可反复调用进行刷新
    var config = {};  //table的参数，如搜索key，点击tree的id
    var mainList = function (options) {
        if (options != undefined) {
            $.extend(config, options);
        }
        table.reload('mainList', {
            url: '/base/SysUser/Load'
            , method: 'POST'
            , where: config
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

    //添加（编辑）对话框
    var editDlg = function () {
        var vm = new Vue({
            el: "#formEdit"
        });
        var update = false;  //是否为更新
        var show = function (data) {
            var title = update ? "编辑信息" : "添加";
            if (update) {
                $(".Password").hide();
            }
            else {
                $(".Password").show();
            }
        
            index = layer.open({
                title: title,
                area: ["500px", "400px"],
                type: 1,
                content: $('#divEdit'),
                success: function () {
                    vm.$set('$data', data);

                    $(":radio[name='Sex'][value='" + data.Sex + "']").prop("checked", "checked");
                    $("input:checkbox[name='Status']").prop("checked", data.Status == 1);

                    form.render();
                }
            });
            var url = "/base/SysUser/Add";
            if (update) {
                url = "/base/SysUser/Update";
            }
            //提交数据
            form.on('submit(formSubmit)',
                function (data) {
                    //密码强度正则，最少8位，包括至少1个大写字母，1个小写字母，1个数字
                    var pPattern = /^.*(?=.{8,})(?=.*\d)(?=.*[A-Z])(?=.*[a-z]).*$/;
                    if (data.field.Password == data.field.Account) {
                        layer.alert('密码不能用账户一致');
                        return false;
                    }
                    //else if (!pPattern.test(data.field.Password)) {
                    //    layer.alert('密码不符合规则！！密码最少8位，包括至少1个大写字母，1个小写字母，1个数字');
                    //    return false;
                    //}
                    else {
                        $.post(url,
                            data.field,
                            function (data) {
                                layer.msg(data.Message);
                                mainList();
                                layer.close(index);
                            },
                            "json");
                    }
                    return false;
                });
        };
        return {
            add: function () { //弹出添加
                update = false;
                show({
                    Id: '',
                    Status: 1,
                    Password: '123456',
                });
            },
            update: function (data) { //弹出编辑框
                update = true;
                show(data);
            }
        };
    }();

    //监听页面主按钮操作
    var active = {
        btnDel: function () {      //批量删除
            var checkStatus = table.checkStatus('mainList')
                , data = checkStatus.data;
            hhweb.del("/base/SysUser/Delete",
                data.map(function (e) { return e.Id; }),
                mainList);
        }
        , btnAdd: function () {  //添加
            editDlg.add();
        }
        , btnEdit: function () {  //编辑
            var checkStatus = table.checkStatus('mainList')
                , data = checkStatus.data;
            if (data.length != 1) {
                layer.alert("请选择编辑的行，且同时只能编辑一行", { icon: 5, shadeClose: true, title: "错误信息" });
                return;
            }
            editDlg.update(data[0]);
        }

        , search: function () {   //搜索
            mainList({ key: $('#key').val() });
        }
        , btnRefresh: function () {
            mainList();
        }
        , btnAccessModule: function () {
            var checkStatus = table.checkStatus('mainList')
                , data = checkStatus.data;
            if (data.length != 1) {
                toplayer.msg("请选择要分配的用户");
                return;
            }

            var index = toplayer.open({
                title: "为用户【" + data[0].Name + "】分配模块",
                type: 2,
                area: ['750px', '600px'],
                content: "/base/SysModule/Assign?type=UserModule&menuType=UserElement&id=" + data[0].Id,
                success: function (layero, index) {

                }
            });
        }
        , btnAccessRole: function () {
            var checkStatus = table.checkStatus('mainList')
                , data = checkStatus.data;
            if (data.length != 1) {
                toplayer.msg("请选择要分配的用户");
                return;
            }

            var index = toplayer.open({
                title: "为用户【" + data[0].Name + "】分配角色",
                type: 2,
                area: ['750px', '600px'],
                content: "/base/SysRole/Assign?type=UserRole&id=" + data[0].Id,
                success: function (layero, index) {

                }
            });
        }
        , btnResetPwd: function () {  //编辑
            var checkStatus = table.checkStatus('mainList')
                , data = checkStatus.data;
            if (data.length != 1) {
                layer.msg("只能编辑一行");
                return;
            }
            $.post("/base/SysUser/ResetPassword",
                {
                    user: data[0],
                },
                function (result) {
                    if (result.Code != 500) {
                        layer.msg("密码重设成功");
                    } else {
                        layer.alert(result.Message, { icon: 2, shadeClose: true, title: "操作失败" });
                    }
                },
                "json");
        }
    };

    $('.toolList .layui-btn').on('click', function () {
        var type = $(this).data('type');
        active[type] ? active[type].call(this) : '';
    });

    //监听页面主按钮操作 end
});