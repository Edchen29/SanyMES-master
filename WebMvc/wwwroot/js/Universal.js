var SelectorSource = {};

layui.define(['form', 'vue', 'layer', 'element','jquery', 'table'],function (exports) {
    var form = layui.form,
        layer = layui.layer,
        element = layui.element,
        $ = layui.jquery,
        table = layui.table;

    var obj = {
        mmain: function (AreaName, TableName, vm, vmq, EditInfo, selfbtn, mainList) {
            //监听行单击事件（双击事件为：rowDouble）
            table.on('row(mainList)', function (obj) {
                table.on('checkbox(mainList)', function (obj) {
                    if (obj.checked == true) {
                        obj.tr.addClass('layui-table-click');
                    } else {
                        obj.tr.removeClass('layui-table-click');
                    }
                });
            });

            //监听表格排序
            table.on('sort(mainList)', function (obj) {
                table.reload('mainList', {
                    url: "/" + AreaName + "/" + TableName + '/Load',
                    initSort: obj //记录初始排序，如果不设的话，将无法标记表头的排序状态。 layui 2.1.1 新增参数
                    , method: "POST"
                    , where:
                    {
                        field: obj.field //排序字段
                        , order: obj.type//排序方式
                    }
                });
            });

            //头工具栏事件
            table.on('toolbar(mainList)', function (obj) {
                var checkStatus = table.checkStatus('mainList');
                var count = checkStatus.data.length;//选中的行数

                switch (obj.event) {
                    //新增数据
                    case 'tabAdd':
                        vm.$set('$data', {});

                        //新增前的处理方法
                        if (selfbtn['DomConfig'] !== undefined) {
                            selfbtn['DomConfig'].call(null, true);
                        }
                        layer.open({
                            title: '新增',
                            area: ["750px", "450px"],
                            maxmin: true,
                            type: 1,
                            content: $('#modifyForm'),
                            btn: ['保存', '关闭'],
                            yes: function (index) {
                                //保存前方法
                                if (selfbtn['SaveBefore'] !== undefined) {
                                    var rtn = selfbtn['SaveBefore'].call(null, "Add");
                                    if (rtn != null) {
                                        if ($(rtn[0]).context.tagName == "SELECT") {
                                            $($(rtn[0]).next()[0].children[0].firstChild).attr("placeholder", "此项为必填项");
                                        }
                                        layer.alert("必填项不能为空", { icon: 5, shadeClose: true, title: "错误信息" });
                                        $(rtn).css("background-color", "yellow");
                                        $(rtn).focus();
                                        return null;
                                    }
                                }

                                $.ajax({
                                    url: "/" + AreaName + "/" + TableName + "/Ins",
                                    type: "post",
                                    data: { Table_Entity: vm.$data },
                                    dataType: "json",
                                    success: function (result) {
                                        if (result.Code == 200 && result.Status) {
                                            layer.msg('新增成功', { icon: 6, shade: 0.4, time: 1000 });
                                            layer.close(index);
                                            mainList.Load();//重载TABLE

                                            //保存成功方法
                                            if (selfbtn['SaveSuccess'] !== undefined) {
                                                selfbtn['SaveSuccess'].call(null, "Add");
                                            }
                                        }
                                        else {
                                            layer.alert(result.Message, { icon: 5, shadeClose: true, title: "错误信息" });
                                        }
                                    },
                                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                                        layer.alert(errorThrown, { icon: 2, shadeClose: true, title: "错误信息" });
                                    }
                                });
                            },
                            btn2: function (index) {
                                layer.close(index);
                            }
                            , end: function (index, layero) {
                                ClearSelect(TableName, $, form);
                            }
                        });
                        break;
                    //编辑单笔数据
                    case 'btnEdit':
                        if (count == 1) {
                            EditInfo(checkStatus.data[0]);

                            //编辑前的处理方法
                            if (selfbtn['DomConfig'] !== undefined) {
                                selfbtn['DomConfig'].call(null, false);
                            }

                            layer.open({
                                title: '编辑',
                                area: ["750px", "450px"],
                                maxmin: true,
                                type: 1,
                                content: $('#modifyForm'),
                                btn: ['保存', '关闭'],
                                yes: function (index) {
                                    //保存前方法
                                    if (selfbtn['SaveBefore'] !== undefined) {
                                        var rtn = selfbtn['SaveBefore'].call(null, "Edit");
                                        if (rtn != null) {
                                            if ($(rtn[0]).context.tagName == "SELECT") {
                                                $($(rtn[0]).next()[0].children[0].firstChild).attr("placeholder", "此项为必填项");
                                            }
                                            layer.alert("必填项不能为空", { icon: 5, shadeClose: true, title: "错误信息" });
                                            $(rtn).css("background-color", "yellow");
                                            $(rtn).focus();
                                            return null;
                                        }
                                    }

                                    $.ajax({
                                        url: "/" + AreaName + "/" + TableName + "/Upd",
                                        type: "POST",
                                        data: { Table_Entity: vm.$data },
                                        dataType: "json",
                                        success: function (result) {
                                            if (result.Code == 200 && result.Status) {
                                                layer.msg("修改成功!", { icon: 6, shade: 0.4, time: 1000 });
                                                layer.close(index);
                                                mainList.Load();

                                                //保存成功方法
                                                if (selfbtn['SaveSuccess'] !== undefined) {
                                                    selfbtn['SaveSuccess'].call(null, "Edit");
                                                }

                                            } else {
                                                layer.alert(result.Message, { icon: 5, shadeClose: true, title: "错误信息" });
                                            }
                                        },
                                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                                            layer.alert(errorThrown, { icon: 2, shadeClose: true, title: "错误信息" });
                                        }
                                    });
                                },
                                btn2: function (index) {
                                    layer.close(index);
                                }
                                , end: function (index, layero) {
                                    ClearSelect(TableName, $, form);
                                }
                            });
                        }
                        else
                            layer.alert("请选择一条数据", { icon: 5, shadeClose: true, title: "错误信息" });
                        break;
                    //上传数据
                    case 'btnUpload':
                        Import();
                        break;
                    //导出数据
                    case 'btnExport':
                        var index = layer.load();
                        $.ajax({
                            url: "/" + AreaName + "/" + TableName + "/Export",
                            type: "POST",
                            data: vmq.$data,
                            dataType: "json",
                            success: function (result) {
                                layer.close(index);
                                var ExportTitle = new Array();
                                var ExportData = result.data;
                                for (var i = 0; i < result.count; i++) {
                                    var data = result.data[i];
                                    for (var item in data) {
                                        if (i == 0) {
                                            ExportTitle.push(item);
                                        }
                                    }
                                }
                                table.exportFile(ExportTitle, ExportData, 'csv')

                                layer.msg("导出完成！", { icon: 6, shade: 0.4, time: 1000 });
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                layer.close(index);
                                layer.alert(errorThrown, { icon: 2, shadeClose: true, title: "错误信息" });
                            }
                        });
                        break;
                    //检索按钮，弹出检索区
                    case 'btnSelect':
                        panelSearch();
                        break;
                    //刷新数据
                    case 'btnRefresh':
                        mainList.Load();
                        $('.layui-collapse').hide();
                        break;
                    //批量删除数据
                    case 'btnDelete':
                        if (count > 0) {
                            //删除前方法
                            if (selfbtn['SaveBefore'] !== undefined) {
                                selfbtn['SaveBefore'].call(null, "Delete");
                            }

                            layer.confirm('确定要删除所选信息', { icon: 3 }, function (index) {
                                var data = checkStatus.data; //获取选中行的数据

                                $.ajax({
                                    url: "/" + AreaName + "/" + TableName + "/DelByIds",
                                    type: "post",
                                    data: { ids: data.map(function (e) { return e.Id; }) },
                                    dataType: "json",
                                    success: function (result) {
                                        if (result.Code == 200 && result.Status) {
                                            layer.msg('删除成功', { icon: 6, shade: 0.4, time: 1000 });
                                            layer.close(index);
                                            mainList.Load();//重载TABLE

                                            //保存成功方法
                                            if (selfbtn['SaveSuccess'] !== undefined) {
                                                selfbtn['SaveSuccess'].call(null, "Delete");
                                            }

                                            $('.layui-collapse').hide();
                                        }
                                        else {
                                            layer.alert(result.Message, { icon: 5, shadeClose: true, title: "错误信息" });
                                        }
                                    },
                                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                                        layer.alert(errorThrown, { icon: 2, shadeClose: true, title: "错误信息" });
                                    }
                                });
                            });
                        }
                        else
                            layer.alert("请至少选择一条数据", { icon: 5, shadeClose: true, title: "错误信息" });
                        break;
                }

                //自定义按钮方法遍历
                for (var key in selfbtn) {
                    if (obj.event == key)
                        selfbtn[key]();
                }
            });

            //监听页面主按钮操作
            var active = {
                //检索区的查询按钮
                btnQuery: function () {
                    var inputName = $('.layui-date').map(function () { return $(this).attr("name"); }).toArray();
                    var inputDate = $('.layui-date').map(function () { return $(this).val(); }).toArray();

                    var editTime = function (tmpTime) {
                        if (tmpTime.length === 4) {
                            return tmpTime = tmpTime + "-01-01 00:00:00.001";
                        }
                        else if (tmpTime.length === 7) {
                            return tmpTime = tmpTime + "-01 00:00:00.002";
                        }
                        else if (tmpTime.length === 10) {
                            return tmpTime = tmpTime + " 00:00:00.003";
                        }
                        else if (tmpTime.length === 13) {
                            return tmpTime = tmpTime + ":00:00.004";
                        }
                        else if (tmpTime.length === 16) {
                            return tmpTime = tmpTime + ":00.005";
                        }
                        else {
                            return tmpTime = tmpTime + ".000";
                        }
                    };

                    inputDate.forEach(function (item, len) {
                        if (item.length !== 0) {
                            var tmpTime = item.replace(" 00:00:00", "").replace(":00:00", "").replace(":00", "");
                            if (item !== "") {
                                var dateString = inputName[len].substring(1, inputName[len].length);
                                vmq[dateString] = editTime(tmpTime);
                            }
                        }
                    });
                    table.reload('mainList', {
                        page: { curr: 1 }
                        , url: "/" + AreaName + "/" + TableName + "/Load"
                        , method: "POST"
                        , where: vmq.$data
                    });
                    panelSearch();

                },
                //检索区重置按钮
                btnReset: function () {
                    for (var item in vmq.$data) {
                        vmq.$data[item] = null;
                        $("[name='q" + item + "']").val('');
                        layui.form.render(); //下拉框等页面元素需要重渲染
                    }
                },
                //检索区关闭按钮
                btnClose: function () {
                    panelSearch();
                },
                //下载模板
                btnTemplate: function () {
                    var index = layer.load();
                    $.ajax({
                        url: "/" + AreaName + "/" + TableName + "/GetTemplate",
                        type: "POST",
                        dataType: "json",
                        success: function (result) {
                            layer.close(index);
                            var ExportTitle = new Array();
                            var ExportData = result.data;
                            for (var i = 0; i < result.count; i++) {
                                var data = result.data[i];
                                for (var item in data) {
                                    if (i == 0) {
                                        ExportTitle.push(item);
                                    }
                                }
                            }
                            table.exportFile(ExportTitle, ExportData, 'csv')

                            layer.msg("导出模板完成！", { icon: 6, shade: 0.4, time: 1000 });
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            layer.close(index);
                            layer.alert(errorThrown, { icon: 2, shadeClose: true, title: "错误信息" });
                        }
                    });
                }
            };

            $('.layui-btn').on('click', function () {
                var type = $(this).data('type');
                active[type] ? active[type].call(this) : '';
            });

            //监听页面主按钮操作 end

            mainList.Load();

            function panelSearch() {
                $('#panelSearch').toggle("fast");
            }

            function Import() {
                layer.open({
                    type: 1,
                    skin: 'layui-layer-molv',
                    anim: 1,
                    id: 'LAY_layuipro', //设定一个id，防止重复弹出
                    btnAlign: 'c',
                    moveType: 1, //拖拽模式，0或者1
                    title: "请选择需导入的文件：", //不显示标题
                    area: ['400px', '300px'], //宽高
                    content: $('#ImportData'), //捕获的元素
                    scrollbar: false,
                    btn: ['导入', '关闭'],
                    yes: function () {
                        var index1 = layer.load();
                        var fileObj = document.getElementById("excelfile").files[0]; // js 获取文件对象
                        if (typeof (fileObj) == "undefined" || fileObj.size <= 0) {
                            layer.alert("请先选择需要上传的文件！", { skin: 'layui-layer-molv', anim: 1, icon: 5 });
                            layer.close(index1);
                            return;
                        }
                        var formFile = new FormData();
                        formFile.append("excelfile", fileObj); //加入文件对象
                        var data = formFile;
                        $.ajax({
                            url: "/" + AreaName + "/" + TableName + "/Import",
                            data: data,
                            type: "Post",
                            dataType: "json",
                            cache: false,//上传文件无需缓存
                            processData: false,//用于对data参数进行序列化处理 这里必须false
                            contentType: false, //必须
                            success: function (result) {
                                if (result.Code == 200) {
                                    if (result.Status) {
                                        layer.msg(result.Message, { icon: 6 });
                                    }
                                    else {
                                        layer.alert(result.Message, { icon: 5, shade: 0.4, shadeClose: true, title: '导入失败' });
                                    }
                                    $('#excelfile').val("");
                                    mainList.Load();
                                }
                            }
                        });
                        layer.closeAll();
                    },
                    cancel: function (index) {
                        $("Hide").unbind();
                        layer.close(index);
                    }
                });
            }

            //自动刷新
            function AutoRefresh() {
                if ($("#autoRefresh").get(0).checked) {
                    mainList.Load();
                }
            }

            if ($('#autoRefresh').length != 0) {
                $(document).ready(
                    function () {
                        setInterval(AutoRefresh, 10000);
                    }
                );
            }
        }

        , BindSelector: function (vml, selector) {
            var SelectorObject = {};
            var vmList = new Array();
            vmList = vml[0];
            if (selector != null) {
                Object.keys(selector).forEach(function (key) {
                    if (selector[key].Dom != null) {
                        var SelectObj = {
                            Dom: selector[key].Dom,
                            SelType: selector[key].SelType,
                            SelFrom: selector[key].SelFrom,
                            SelModel: selector[key].SelModel,
                            SelLabel: selector[key].SelLabel,
                            SelValue: selector[key].SelValue
                        };
                        SelectorObject[key] = new Array(SelectObj);
                    }
                    else {
                        var SelectObj2 = {
                            SelType: selector[key].SelType,
                            SelFrom: selector[key].SelFrom,
                            SelModel: selector[key].SelModel
                        };
                        SelectorObject[key] = new Array(SelectObj2);
                    }
                });
            }

            //用jQuery的$.each()，自带回调函数，形成了函数作用域,替代解决JS的for循环包裹异步函数的问题
            $.each(SelectorObject, function (key, value) {
                var oItem = value[0];
                var SelType = oItem.SelType;
                var SelFrom = oItem.SelFrom;

                var data, url;
                if (SelType == "FromUrl") {
                    data = { limit: 1000 };
                    url = SelFrom;
                }
                else if (SelType == "FromDict") {
                    data = { DictType: SelFrom };
                    url = "/base/SysDictData/FindSysDictData";
                }

                $.ajax({
                    async: false,
                    type: "post",
                    data: data,
                    url: url,
                    dataType: "json",
                    success: function (result) {
                        if (result.count > 0) {
                            SelectorSource[oItem.SelModel] = result.data;

                            setTimeout(function () {
                                form.render('select'); //只渲染下拉框
                            }, 10);

                            form.on('select(qform)', function (data) {
                                vmList['vmq'].$data[$(data.elem).data("model")] = data.value;
                            });

                            form.on('select(eform)', function (data) {
                                if (vmList['vmDtl' + $(data.elem)[0].classList[1].split('_')[1]] == undefined) {
                                    vmList['vm'].$data[$(data.elem).data("model")] = data.value;
                                } else {
                                    vmList['vmDtl' + $(data.elem)[0].classList[1].split('_')[1]].$data[$(data.elem).data("model")] = data.value;
                                }
                            });
                        }
                    }
                });
            });

            Object.keys(SelectorObject).forEach(function (key) {
                if (SelectorObject[key][0].Dom != null) {
                    SelectorObject[key][0].Dom.forEach(function (value, i) {
                        var SelLabel = SelectorObject[key][0].SelLabel;
                        var SelValue = SelectorObject[key][0].SelValue;
                        for (var k = 0; k < value.length; k++) {
                            $(value[k]).empty();
                            $(value[k]).append("<option style='display: none'></option>");
                            if (SelectorSource[key] == undefined) {
                                console.log("数据源:[" + key + "]配置有误,请联系管理员!");
                            }
                            else {
                                for (var j = 0; j < SelectorSource[key].length; j++) {
                                    $(value[k]).append("<option value = '" + SelectorSource[key][j][SelValue] + "'>" + SelectorSource[key][j][SelLabel] + "</option>");
                                }
                            }
                        }
                    });
                }
            });
        }

        , mainDtl: function (selfbtn, All) {
            var DtlShowbool = {};//所有折叠的当前状态
            var Dtl = {};//当前点击All数据
            var Value = {};//主表数据缓存数据
            var Values = new Array();//主表数据缓存数据集合
            //所有折叠状态初始化
            for (i = 0; i < All.length; i++) {
                DtlShowbool[All[i].TableNameDtl] = false;
            }

            //监听单元格点击
            table.on('tool(mainList)', function (obj) {
                var Dtlbtn = {};
                var mainListDtl;
                for (i = 0; i < All.length; i++) {
                    if (obj.event === All[i].TableNameDtl) {
                        Dtl = All[i];
                    }
                }
                if (Dtl != {}) {
                    for (i = 0; i < Object.keys(Dtl.NameDtl).length; i++) {
                        //主表数据赋值
                        Value[Object.keys(Dtl.NameDtl)[i]] = obj.data[Dtl.NameDtl[Object.keys(Dtl.NameDtl)[i]]];
                        Values[Dtl.TableNameDtl] = Value;
                        //重载赋值(防止主子表字段名不一样)
                        if (obj.data[Dtl.NameDtl[Object.keys(Dtl.NameDtl)[i]]] != null) {
                            Dtl.vmqDtl.$data[Object.keys(Dtl.NameDtl)[i]] = obj.data[Dtl.NameDtl[Object.keys(Dtl.NameDtl)[i]]];
                        } else {
                            Dtl.vmqDtl.$data[Object.keys(Dtl.NameDtl)[i]] = '###';
                        }
                    }
                    //重载方法
                    mainListDtl = {
                        mainListDtl: function (mainDtl) {
                            mainDtl.mainListDtl.Load(mainDtl.vmqDtl.$data);
                        }
                    };
                    //监听折叠
                    element.on('collapse(DisplayDtl)', function (data) {
                        //记录当前折叠状态
                        DtlShowbool[data.title[0].id.split('_')[1]] = data.show;
                        //头工具栏事件
                        table.on('toolbar(mainListDtl' + data.title[0].id.split('_')[1] + ')', function (obj) {
                            for (i = 0; i < All.length; i++) {
                                if (obj.event.split('_')[1] === All[i].TableNameDtl) {
                                    Dtlbtn = All[i];
                                }
                            }
                            if (Dtlbtn != {}) {
                                var checkStatus = table.checkStatus('mainListDtl' + Dtlbtn.TableNameDtl);
                                var count = checkStatus.data.length;//选中的行数
                                switch (obj.event) {
                                    case 'tabAddDtl_' + Dtlbtn.TableNameDtl:  //跳转至单据页面，新增数据
                                        //新增前的处理方法
                                        if (selfbtn['DomConfig_' + Dtlbtn.TableNameDtl] !== undefined) {
                                            var rtn = selfbtn['DomConfig_' + Dtlbtn.TableNameDtl].call(null,"Add", Dtlbtn.vmqDtl._data);
                                            if (rtn == false) { return null; }
                                        }

                                        layer.open({
                                            type: 1
                                            , area: ['600px', '500px']
                                            , content: $('#modifyFormDtl_' + Dtlbtn.TableNameDtl)
                                            , success: function (layero, index) {
                                                for (i = 0; i < Object.keys(Values[Dtlbtn.TableNameDtl]).length; i++) {
                                                    Dtlbtn.vmDtl.$set(Object.keys(Values[Dtlbtn.TableNameDtl])[i], Value[Object.keys(Values[Dtlbtn.TableNameDtl])[i]])
                                                }
                                            }
                                            , btn: ['保存', '取消']
                                            , yes: function (index, layero) {
                                                //保存前方法
                                                if (selfbtn['SaveBefore_' + Dtlbtn.TableNameDtl] !== undefined) {
                                                    var rtn = selfbtn['SaveBefore_' + Dtlbtn.TableNameDtl].call(null, "Add");
                                                    if (rtn != null) {
                                                        if ($(rtn[0]).context.tagName == "SELECT") {
                                                            $($(rtn[0]).next()[0].children[0].firstChild).attr("placeholder", "此项为必填项");
                                                        }
                                                        layer.alert("必填项不能为空", { icon: 5, shadeClose: true, title: "错误信息" });
                                                        $(rtn).focus();
                                                        return null;
                                                    }
                                                }

                                                //新增
                                                $.ajax({
                                                    url: "/" + Dtlbtn.AreaNameDtl + "/" + Dtlbtn.TableNameDtl + "/Ins",
                                                    type: "post",
                                                    data: { Table_Entity: Dtlbtn.vmDtl.$data },
                                                    dataType: "json",
                                                    success: function (result) {
                                                        if (result.Code == 200 && result.Status) {
                                                            layer.msg('新增成功', { icon: 6, shade: 0.4, time: 1000 });
                                                            mainListDtl.mainListDtl(Dtlbtn);//重载TABLE

                                                            //新增成功回调方法
                                                            if (selfbtn['SaveSuccess_' + Dtlbtn.TableNameDtl] !== undefined) {
                                                                selfbtn['SaveSuccess_' + Dtlbtn.TableNameDtl].call(null, "Add");
                                                            }
                                                        }
                                                        else {
                                                            layer.alert(result.Message, { icon: 5, shadeClose: true, title: "错误信息" });
                                                        }
                                                    },
                                                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                                                        layer.alert(errorThrown, { icon: 2, shadeClose: true, title: "错误信息" });
                                                    }
                                                });
                                                layer.close(index);
                                            }
                                            , btn2: function (index, layero) {
                                                Dtlbtn.vmDtl.$set('$data', {});
                                                layer.close(index);
                                            }
                                            , end: function (index, layero) {
                                                ClearSelect(Dtlbtn.TableNameDtl, $, form);
                                                Dtlbtn.vmDtl.$set('$data', {});
                                            }
                                        });
                                        break;
                                    case 'btnRefreshDtl_' + Dtlbtn.TableNameDtl:  //刷新数据
                                        mainListDtl.mainListDtl(Dtlbtn);
                                        break;
                                    case 'btnEditDtl_' + Dtlbtn.TableNameDtl: //编辑单笔数据
                                        if (count == 1) {
                                            Dtlbtn.EditInfoDtl(checkStatus.data[0]);
                                            //编辑前的处理方法
                                            if (selfbtn['DomConfig_' + Dtlbtn.TableNameDtl] !== undefined) {
                                                var rtn = selfbtn['DomConfig_' + Dtlbtn.TableNameDtl].call(null, "Edit", Dtlbtn.vmqDtl._data);
                                                if (rtn == false) { return null; }
                                            }

                                            layer.open({
                                                type: 1
                                                , area: ['600px', '500px']
                                                , content: $('#modifyFormDtl_' + Dtlbtn.TableNameDtl)
                                                , success: function (layero, index) {
                                                    
                                                }
                                                , btn: ['保存', '取消']
                                                , yes: function (index, layero) {
                                                    //保存前方法
                                                    if (selfbtn['SaveBefore_' + Dtlbtn.TableNameDtl] !== undefined) {
                                                        var rtn = selfbtn['SaveBefore_' + Dtlbtn.TableNameDtl].call(null, "Edit");
                                                        if (rtn != null) {
                                                            if ($(rtn[0]).context.tagName == "SELECT") {
                                                                $($(rtn[0]).next()[0].children[0].firstChild).attr("placeholder", "此项为必填项");
                                                            }
                                                            layer.alert("必填项不能为空", { icon: 5, shadeClose: true, title: "错误信息" });
                                                            $(rtn).focus();
                                                            return null;
                                                        }
                                                    }

                                                    //编辑
                                                    $.ajax({
                                                        url: "/" + Dtl.AreaNameDtl + "/" + Dtlbtn.TableNameDtl + "/Upd",
                                                        type: "POST",
                                                        data: { Table_Entity: Dtlbtn.vmDtl.$data },
                                                        dataType: "json",
                                                        success: function (result) {
                                                            if (result.Code == 200 && result.Status) {
                                                                layer.msg("修改成功!", { icon: 6, shade: 0.4, time: 1000 });
                                                                mainListDtl.mainListDtl(Dtlbtn);

                                                                //保存成功方法
                                                                if (selfbtn['SaveSuccess_' + Dtlbtn.TableNameDtl] !== undefined) {
                                                                    selfbtn['SaveSuccess_' + Dtlbtn.TableNameDtl].call(null, "Edit");
                                                                }

                                                            } else {
                                                                layer.alert(result.Message, { icon: 5, shadeClose: true, title: "错误信息" });
                                                            }
                                                        }

                                                    });
                                                    layer.close(index);
                                                }
                                                , btn2: function (index, layero) {
                                                    Dtlbtn.vmDtl.$set('$data', {});
                                                    layer.close(index);
                                                }
                                                , end: function (index, layero) {
                                                    ClearSelect(Dtlbtn.TableNameDtl, $, form);
                                                    Dtlbtn.vmDtl.$set('$data', {});
                                                }
                                            });
                                        }
                                        else
                                            layer.alert("请选择一条数据", { icon: 5, shadeClose: true, title: "错误信息" });
                                        break;
                                    case 'btnDeleteDtl_' + Dtlbtn.TableNameDtl:   //批量删除数据
                                        if (count > 0) {
                                            //删除前方法
                                            if (selfbtn['DomConfig_' + Dtlbtn.TableNameDtl] !== undefined) {
                                                var rtn = selfbtn['DomConfig_' + Dtlbtn.TableNameDtl].call(null, "Delete", Dtlbtn.vmqDtl._data);
                                                if (rtn == false) { return null; }
                                            }

                                            layer.confirm('确定要删除所选信息', { icon: 3 }, function (index) {
                                                var data = checkStatus.data; //获取选中行的数据

                                                $.ajax({
                                                    url: "/" + Dtlbtn.AreaNameDtl + "/" + Dtlbtn.TableNameDtl + "/DelByIds",
                                                    type: "post",
                                                    data: { ids: data.map(function (e) { return e.Id; }) },
                                                    dataType: "json",
                                                    success: function (result) {
                                                        if (result.Code == 200 && result.Status) {
                                                            layer.msg('删除成功', { icon: 6, shade: 0.4, time: 1000 });
                                                            layer.close(index);
                                                            mainListDtl.mainListDtl(Dtlbtn);//重载TABLE

                                                            //保存成功方法
                                                            if (selfbtn['SaveSuccess_' + Dtlbtn.TableNameDtl] !== undefined) {
                                                                selfbtn['SaveSuccess_' + Dtlbtn.TableNameDtl].call(null, "Delete");
                                                            }
                                                        }
                                                        else {
                                                            layer.alert(result.Message, { icon: 5, shadeClose: true, title: "错误信息" });
                                                        }
                                                    },
                                                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                                                        layer.alert(errorThrown, { icon: 2, shadeClose: true, title: "错误信息" });
                                                    }
                                                });
                                            });
                                        }
                                        else
                                            layer.alert("请至少选择一条数据", { icon: 5, shadeClose: true, title: "错误信息" });
                                        break;
                                }

                                for (var key in selfbtn) {
                                    if (obj.event == key)
                                        selfbtn[key]();
                                }
                            }
                        });
                    });
                    //单元格对checkbox失效
                    $(obj.tr[1]).click();
                    //字体颜色转换
                    obj.tr.siblings().find('.' + Dtl.TableNameDtl + 'Class').css('color', 'cornflowerblue');
                    obj.tr.find('.' + Dtl.TableNameDtl + 'Class').css('color', 'sandybrown');
                    //显示折叠
                    $('.layui-collapse').show();
                    $('#DisplayDtl' + Dtl.TableNameDtl).show();
                    for (i = 0; i < Object.keys(DtlShowbool).length; i++) {
                        if (DtlShowbool[Object.keys(DtlShowbool)[i]] == true && Object.keys(DtlShowbool)[i] != DtlShowbool[Dtl.TableNameDtl]) {
                            //关闭其他折叠
                            $('#title_' + Object.keys(DtlShowbool)[i]).click();
                            DtlShowbool[Object.keys(DtlShowbool)[i]] = false;
                        }
                    }
                    if (DtlShowbool[Dtl.TableNameDtl] == false) {
                        //打开当前数据对应折叠
                        $('#title_' + Dtl.TableNameDtl).click();
                    }
                    mainListDtl.mainListDtl(Dtl);
                }
            });
        }
    };

    exports('Universal', obj);
});

function GetLabel(type, key, value, keyvalue) {
    var rtn;
    var DataList = SelectorSource[type];
    if (DataList == undefined) {
        return keyvalue;
    }
    for (var i = 0; i < DataList.length; i++) {
        if (keyvalue == true || keyvalue == false) {
            keyvalue = keyvalue + "";
        }
        if (DataList[i][key] == keyvalue) {
            if (DataList[i].CssClass == '' || DataList[i].CssClass == 'null' || DataList[i].CssClass == null) {
                rtn = '<label class="' + DataList[i].CssClass + '" style="color:black;font-size:12px;">' + DataList[i][value] + '</label>';
            } else {
                rtn = '<label class="' + DataList[i].CssClass + '" style="color:white;font-size:12px;">' + DataList[i][value] + '</label>';
            }
            break;
        }
    }
    if (rtn == undefined) {
        if (keyvalue == null) {
            return '';
        }
        return keyvalue;
    }
    else {
        return rtn;
    }
}

//清除下拉框数据
function ClearSelect(TableName, $, form) {
    $('.ClearSelector_' + TableName).each(function () {
        var selDom = ($(this));
        selDom.val("");
    });
    form.render("select");
}
