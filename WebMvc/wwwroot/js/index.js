var $, tab;
layui.config({
    base: "/js/"
}).use(['bodyTab', 'form', 'element', 'layer', 'jquery'], function () {
    var form = layui.form,
        layer = layui.layer,
        element = layui.element;
    $ = layui.jquery;
    tab = layui.bodyTab({
        openTabNum: "50",  //最大可打开窗口数量
        url: "/base/UserSession/GetModulesTree" //获取菜单json地址
    });

    $(document).ready(
        function () {
            $("#Name").html(window.localStorage.getItem("Account"));
            $("#loginInfo").html("<i class= 'layui-icon'>&#xe66f;</i>&nbsp;" + window.localStorage.getItem("Name"));

            var myDate = new Date;
            //var year = myDate.getFullYear(); //获取当前年
            var mon = myDate.getMonth() + 1; //获取当前月
            var date = myDate.getDate(); //获取当前日
            var h = myDate.getHours();//获取当前小时数(0-23)
            var m = myDate.getMinutes();//获取当前分钟数(0-59)
            // var s = myDate.getSeconds();//获取当前秒
            //var week = myDate.getDay();
            //var weeks = ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"];

            mon = padleft0(mon);
            date = padleft0(date);
            h = padleft0(h);
            m = padleft0(m);
            $("#loginTime").html("<i class= 'layui-icon'>&#xe60e;</i>&nbsp;" + mon + "/" + date + " " + h + ":" + m);
        }
    );

    function padleft0(obj) {
        return obj.toString().replace(/^[0-9]{1}$/, "0" + obj);
    }

    //退出
    $(".signOut").click(function () {
        window.sessionStorage.removeItem("menu");
        menu = [];
        window.sessionStorage.removeItem("curmenu");
    })

    //隐藏左侧导航
    $(".hideMenu").click(function () {
        $(".layui-layout-admin").toggleClass("showMenu");
        //渲染顶部窗口
        tab.tabMove();
    })

    //渲染左侧菜单
    tab.render();

    //锁屏
    function lockPage() {
        layer.open({
            title: false,
            type: 1,
            content: '	<div class="admin-header-lock" id="lock-box">' +
                '<div class="admin-header-lock-img"><img src="/images/lock.png"/></div>' +
                '<div class="admin-header-lock-name" id="lockUserName">System</div>' +
                '<div class="input_btn">' +
                '<input type="password" class="admin-header-lock-input layui-input" autocomplete="off" placeholder="请输入密码解锁.." name="lockPwd" id="lockPwd" />' +
                '<button class="layui-btn" id="unlock">解锁</button>' +
                '</div>' +
                '</div>',
            closeBtn: 0,
            shade: 0.9
        })
        $(".admin-header-lock-input").focus();
        if ($("#usernametop").html() != "") {
            $("#lockUserName").html($("#usernametop").html());
        }
    }
    $(".lockcms").on("click", function () {
        window.sessionStorage.setItem("lockcms", true);
        lockPage();
    })
    // 判断是否显示锁屏
    if (window.sessionStorage.getItem("lockcms") == "true") {
        lockPage();
    }
    // 解锁
    $("body").on("click", "#unlock", function () {
        if ($(this).siblings(".admin-header-lock-input").val() == '') {
            layer.alert("请输入解锁密码！", { icon: 5, shadeClose: true, title: "错误信息" });
            $(this).siblings(".admin-header-lock-input").focus();
        } else {

            $.getJSON("/Login/Login"
                , {
                    username: $("#lockUserName").html(),
                    password: $(this).siblings(".admin-header-lock-input").val(),
                }
                , function (data) {
                    if (data.Code == 200) {
                        window.sessionStorage.setItem("lockcms", false);
                        $(this).siblings(".admin-header-lock-input").val('');
                        layer.closeAll("page");
                    } else {
                        layer.alert("密码错误，请重新输入！", { icon: 5, shadeClose: true, title: "错误信息" });
                        $(this).siblings(".admin-header-lock-input").val('').focus();
                    }
                });
        }
    });

    //手机设备的简单适配
    var treeMobile = $('.site-tree-mobile'),
        shadeMobile = $('.site-mobile-shade')

    treeMobile.on('click', function () {
        $('body').addClass('site-mobile');
    });

    shadeMobile.on('click', function () {
        $('body').removeClass('site-mobile');
    });

    // 添加新窗口
    $("body").on("click", ".layui-nav .layui-nav-item a", function () {
        //如果不存在子级
        if ($(this).siblings().length == 0) {
            addTab($(this));
            $('body').removeClass('site-mobile');  //移动端点击菜单关闭菜单层
        }
        $(this).parent("li").siblings().removeClass("layui-nav-itemed");
    })

    //关闭其他
    $(".closePageOther").on("click", function () {
        if ($("#top_tabs li").length > 2 && $("#top_tabs li.layui-this cite").text() != "首页") {
            var menu = JSON.parse(window.sessionStorage.getItem("menu"));
            $("#top_tabs li").each(function () {
                if ($(this).attr("lay-id") != '' && !$(this).hasClass("layui-this")) {
                    element.tabDelete("bodyTab", $(this).attr("lay-id")).init();
                    //此处将当前窗口重新获取放入session，避免一个个删除来回循环造成的不必要工作量
                    for (var i = 0; i < menu.length; i++) {
                        if ($("#top_tabs li.layui-this cite").text() == menu[i].title) {
                            menu.splice(0, menu.length, menu[i]);
                            window.sessionStorage.setItem("menu", JSON.stringify(menu));
                        }
                    }
                }
            })
        } else if ($("#top_tabs li.layui-this cite").text() == "首页" && $("#top_tabs li").length > 1) {
            $("#top_tabs li").each(function () {
                if ($(this).attr("lay-id") != '' && !$(this).hasClass("layui-this")) {
                    element.tabDelete("bodyTab", $(this).attr("lay-id")).init();
                    window.sessionStorage.removeItem("menu");
                    menu = [];
                    window.sessionStorage.removeItem("curmenu");
                }
            })
        } else {
            layer.alert("没有可以关闭的窗口了@_@", { icon: 5, shadeClose: true, title: "错误信息" });
        }
        //渲染顶部窗口
        tab.tabMove();
    })
    //关闭全部
    $(".closePageAll").on("click", function () {
        if ($("#top_tabs li").length > 1) {
            $("#top_tabs li").each(function () {
                if ($(this).attr("lay-id") != '') {
                    element.tabDelete("bodyTab", $(this).attr("lay-id")).init();
                    window.sessionStorage.removeItem("menu");
                    menu = [];
                    window.sessionStorage.removeItem("curmenu");
                }
            })
        } else {
            layer.alert("没有可以关闭的窗口了@_@", { icon: 5, shadeClose: true, title: "错误信息" });
        }
        //渲染顶部窗口
        tab.tabMove();
    })

    $(".changepwd").on("click", function () {
        changePwd();
    });

    //修改密码
    function changePwd() {
        layer.open({
            title: "修改密码",
            shift: 2,
            type: 2,
            content: '/base/SysUser/ChangePassword',
            area: ['450px', '280px'], //宽高
            shade: 0.9
        })
    }

    $(document).bind("keydown", function (e) {
        //禁用页面F5刷新
        if (e.which == 116) {
            e.preventDefault(); //Skip default behavior of the enter key
        }
    });
})

//打开新窗口
function addTab(_this) {
    tab.tabAdd(_this);
}

var qp = {
    //全屏 类
    fullScreen: function () {
        var isFullScreen = false;
        var requestFullScreen = function () { //全屏
            var de = document.documentElement;
            if (de.requestFullscreen) {
                de.requestFullscreen();
            } else if (de.mozRequestFullScreen) {
                de.mozRequestFullScreen();
            } else if (de.webkitRequestFullScreen) {
                de.webkitRequestFullScreen();
            } else {
                alert("该浏览器不支持全屏");
            }
        };

        //退出全屏 判断浏览器种类
        var exitFull = function () {
            // 判断各种浏览器，找到正确的方法
            var exitMethod = document.exitFullscreen || //W3C
                document.mozCancelFullScreen || //Chrome等
                document.webkitExitFullscreen || //FireFox
                document.webkitExitFullscreen; //IE11
            if (exitMethod) {
                exitMethod.call(document);
            } else if (typeof window.ActiveXObject !== "undefined") { //for Internet Explorer
                var wscript = new ActiveXObject("WScript.Shell");
                if (wscript !== null) {
                    wscript.SendKeys("{F11}");
                }
            }
        };

        return {
            handleFullScreen: function ($this) {
                $this = $($this);
                if (isFullScreen) {
                    exitFull();
                    isFullScreen = false;
                    $this.find("i").removeClass("wb-contract");
                    $this.find("i").addClass("wb-expand");
                } else {
                    requestFullScreen();
                    isFullScreen = true;
                    $this.find("i").removeClass("wb-expand");
                    $this.find("i").addClass("wb-contract");
                }
            },
        };
    }()
}