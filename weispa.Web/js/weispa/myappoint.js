var role = $('#role').val();
var lockobj = {};
//status 0:取消订单 1:确认订单 3：标为已完成 4.标为未完成 
function changeStatus(title,btnname,toStatus,$t) {
    $.confirm(title, btnname, function () {
        var orderId = $t.attr('orderId');
        tool.request({
            url: '/Appoint/ChangeStatus',
            data: { toStatus: toStatus ,orderId:orderId},
            dataType: 'json',
            type: "POST",
            success: function (d) {
                if (d.success) {
                    $t.find('.room_r').text(d.msg.status);
                    $t.find('.optStatus').val(d.msg.opts);
                    tool.toptip("操作成功",1);
                } else
                    tool.toptip(d.msg, 2);
            },
            error: function (e, d) {
                tool.toptip('操作失败');
            },
            loading: true

        }, lockobj);
    }, function () {
        
    });
}

$('.book_b').click(function () {
    var $t = $(this);

    var actions = [];
    var optStatus = $t.find('.optStatus').val().split(',');
    for (var i in optStatus) {
        var optObj;
        switch (parseInt(optStatus[i])) {
        case 1:
            optObj = {
                text: "确认预约",
                className: "color-primary",
                onClick: function() {
                    changeStatus('您确认本次预约有效吗？', '确认预约', 1, $t);
                }
            };
            break;
        case 2:
            optObj = {
                text: "标为完成",
                className: "color-primary",
                onClick: function() {
                    changeStatus('您确认本次预定已完成服务吗？', '确认已完成', 2, $t);
                }
            };
            break;
        case 3:
            optObj = {
                text: "标为未完成",
                className: 'color-warning',
                onClick: function() {
                    changeStatus('您确认本次预定未完成服务吗？', '确认未完成', 3, $t);
                }
            };
            break;
        case 4:
            optObj = {
                text: "取消预定",
                className: "color-danger",
                onClick: function() {
                    changeStatus('您确定要取消吗？取消后将不可恢复', '确认取消', 4, $t);
                }
            };
            break;
        }
        if (optObj)
        actions.push(optObj);
    }

    $.actions({
        title: "请选择您需要的操作",
        actions: actions
    });
});