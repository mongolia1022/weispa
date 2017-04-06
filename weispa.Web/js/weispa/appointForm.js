var $form = $('#form');
tool.initForm($form);
var lockobj = {};

$('#submitBtn').click(function () {
    var p = tool.getParams($form);
    if (p == null) {
        return false;
    }

    tool.request({
        url: '/Appoint/AppointPost',
        data: p,
        dataType: 'json',
        type: "POST",
        success: function (d) {
            if (d.success) {
                location.href = '/Appoint/Success';
            } else
                tool.toptip(d.msg,2);
        },
        error: function (e, d) {
            tool.toptip('操作失败');
        },
        loading:true

    }, lockobj);
});