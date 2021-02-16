layui.config({
    base: "/js/"
}).use(['form', 'layer'], function () {

    if (self !== top) {
        //如果在iframe中，则跳转
        top.location.replace("/Login/Index");
    }

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : parent.layer,
        $ = layui.jquery;

    // Cloud Float...
    var $main = $cloud = mainwidth = null;
    var offset1 = 450;
    var offset2 = 0;
    var offsetbg = 0;

    $(document).ready(
        function () {
            $main = $("#mainBody");
            $body = $("body");
            $cloud1 = $("#cloud1");
            $cloud2 = $("#cloud2");

            mainwidth = $main.outerWidth();
        }
    );

    //setInterval(function flutter() {
    //    if (offset1 >= mainwidth) {
    //        offset1 = -580;
    //    }

    //    if (offset2 >= mainwidth) {
    //        offset2 = -580;
    //    }

    //    offset1 += 1.1;
    //    offset2 += 1;
    //    $cloud1.css("background-position", offset1 + "px 100px")

    //    $cloud2.css("background-position", offset2 + "px 460px")
    //}, 70);

    //setInterval(function bg() {
    //    if (offsetbg >= mainwidth) {
    //        offsetbg = -580;
    //    }

    //    offsetbg += 0.9;
    //    $body.css("background-position", -offsetbg + "px 0")
    //}, 90);

    $(function () {
        $('.loginbox').css({ 'position': 'absolute', 'left': ($(window).width() - 692) / 2 });

        $(window).resize(function () {
            $('.loginbox').css({ 'position': 'absolute', 'left': ($(window).width() - 692) / 2 });
        })
    });

    //登录按钮事件
    form.on("submit(login)", function (data) {
        $.ajax({
            url: "/Login/Login",
            type: "post",
            data: data.field,
            dataType: "json",
            success: function (data) {
                if (data.Code === 200) {
                    window.localStorage.setItem("Account", data.Result[0]);
                    window.localStorage.setItem("Name", data.Result[1]);

                    window.location.href = "/Home/Index";
                } else {
                    layer.msg(data.Message);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layer.alert(errorThrown, { icon: 2, shadeClose: true, title: "错误信息" });
            }
        });

        return false;
    })
})
