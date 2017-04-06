var lockobj = {};
//锁定房间
function lockRoom(p, $t) {
    $.confirm('确认锁定？', '确定', function () {
        tool.request({
            url: '/Appoint/LockRoom',
            data: p,
            dataType: 'json',
            type: "POST",
            success: function (d) {
                if (d.success) {
                    $t.find('.optStatus').val(d.msg.opts);
                    $t.find('img').attr('src', '../images/fw_gray.png');
                    $t.find('.orderId').val(d.msg.orderId);

                    tool.toptip("锁定成功", 1);
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

//解锁
function unLockRoom(p, $t) {
    $.confirm('确认解锁？', '确定', function () {
        tool.request({
            url: '/Appoint/UnLockRoom',
            data: { orderId: $t.find('.orderId').val() },
            dataType: 'json',
            type: "POST",
            success: function (d) {
                if (d.success) {
                    $t.find('.optStatus').val(d.msg.opts);
                    $t.find('img').attr('src', '../images/fw_green.png');
                    $t.find('.orderId').val('');

                    tool.toptip("解锁成功", 1);
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

//预定列表
$('#appointList').find('a').click(function () {
    var $t = $(this);

    var actions = [];
    var optStatus = $t.find('.optStatus').val().split(',');
    var p = tool.getParams($t);

    for (var i in optStatus) {
        var optObj;
        switch (parseInt(optStatus[i])) {
            case 0:
                optObj = {
                    text: "预约",
                    className: "color-primary",
                    onClick: function () {
                        location.href = "Appoint/AppointForm?Start=" + p.Start + '&End=' + p.End + '&Room=' + p.Room;
                    }
                };
                break;
            case 1:
                optObj = {
                    text: "锁定",
                    className: "color-danger",
                    onClick: function () {
                        lockRoom(p,$t);
                    }
                };
                break;
            case 2:
                optObj = {
                    text: "解锁",
                    className: 'color-warning',
                    onClick: function () {
                        unLockRoom(p, $t);
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