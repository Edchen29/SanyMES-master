layui.config({
    base: "/js/"
}).use(['form', 'element', 'layer', 'jquery'], function () {
    var form = layui.form, layer = layui.layer;
    element = layui.element;
    $ = layui.jquery;

    var firstText;
    var input = $('input:text:not(:disabled)');
    input.each(function () {
        if (firstText == undefined) {
            firstText = $(this);
        }
    });

    $(firstText).focus();

    $("#btnSave").bind('click', function () {
        
        var pPattern = /^.*(?=.{8,})(?=.*\d)(?=.*[A-Z])(?=.*[a-z]).*$/;
        if ($("#Password").val() == $("#OldPassword").val()) {
            layer.alert('新密码不应与旧密码相同');
            return null;
        }
        else if (!pPattern.test($("#Password").val())) {
            layer.alert('密码不符合规则！！密码最少8位，包括至少1个大写字母，1个小写字母，1个数字');
            return null;
        }
        else if ($("#PasswordConfirm").val() != $("#Password").val() || $("#PasswordConfirm").val() == "") {
            layer.alert("新密码不能为空 且 必须与确认密码一致");
            return null;
        }
        else {
            $.post("/base/SysUser/ChangeUserPassword",
                {
                    OldPassword: $("#OldPassword").val(),
                    Password: $("#Password").val(),
                },
                function (result) {
                    if (result.Code != 500) {
                        layer.msg("密码修改成功");
                    } else {
                        layer.alert(result.Message, { icon: 2, shadeClose: true, title: "操作失败" });
                    }
                },
                "json");
        }
    });
});