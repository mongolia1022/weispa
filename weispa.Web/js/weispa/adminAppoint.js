var $bottomTab = $('#bottomTab');
var $searchForm = $('#searchForm');
var $list = $('#list');
var $loader = $('#loader');
var searchParms = tool.getParams($searchForm);

//筛选
(function () {
    var lockobj = {};
    $(document).on("open", "#searchForm", function () {
        $bottomTab.hide();
    }).on("close", "#searchForm", function () {
        $bottomTab.show();
    });

    ////提交
    //$searchForm.find('.submit').click(function () {
    //    if (!lockobj.isdone && lockobj.isdone != undefined)
    //        return;

    //    lockobj.isdone = false;
    //    $list.empty();
    //    $loader.show();

    //    searchParms = tool.getParams($searchForm);
    //    searchParms.pn = 1;
    //    tool.request({
    //        url: '/Appoint/AdminAppointListAjax',
    //        data: searchParms,
    //        dataType: 'html',
    //        type: "POST",
    //        success: function (d) {
    //            var $d = $(d);
    //            $list.append($d);
    //            orders.bindList($d.find('.weui_media_box'));

    //            searchParms.Pn++;
    //            $loader.hide();
    //            lockobj.isdone = true;
    //        },
    //        error: function (e, d) {
    //            tool.toptip('操作失败');
    //            $loader.hide();
    //            lockobj.isdone = true;
    //        },
    //        loading: false
    //    }, null);
    //});

    //$bottomTab.find('[status]').click(function () {
    //    if (!lockobj.isdone && lockobj.isdone != undefined)
    //        return;

    //    lockobj.isdone = false;
    //    $list.empty();
    //    $loader.show();

    //    searchParms.pn = 1;
    //    searchParms.Status = $(this).attr('status');
    //    tool.request({
    //        url: '/Appoint/AdminAppointListAjax',
    //        data: searchParms,
    //        dataType: 'html',
    //        type: "POST",
    //        success: function (d) {
    //            var $d = $(d);
    //            $list.append($d);
    //            orders.bindList($d.find('.weui_media_box'));

    //            $loader.hide();
    //            lockobj.isdone = true;
    //        },
    //        error: function (e, d) {
    //            tool.toptip('操作失败');
    //            $loader.hide();
    //            lockobj.isdone = true;
    //        },
    //        loading: false
    //    }, null);
    //});

    //翻页
    var pageCount = parseInt($loader.attr('pagecount'));
    searchParms.Pn = 2;

    $('#contentWrap').infinite().on("infinite", function () {
        if (!lockobj.isdone && lockobj.isdone != undefined)
            return;

        lockobj.isdone = false;
        $loader.show();

        if (searchParms.Pn >= pageCount) {
            $loader.html('没有更多了');
            return;
        }

        tool.request({
            url: '/Appoint/AdminAppointListAjax',
            data: searchParms,
            dataType: 'html',
            type: "GET",
            success: function (d) {
                var $d = $(d);
                $list.append($d);
                orders.bindList($d.find('.weui_media_box'));

                searchParms.Pn++;
                $loader.hide();
                lockobj.isdone = true;
            },
            error: function (e, d) {
                tool.toptip('操作失败');
                $loader.hide();
                lockobj.isdone = true;
            },
            loading: false
        });
    });
})();

//修改订单状态
var orders=(function () {
    var lockobj = {};
    //status 0:取消订单 1:确认订单 3：标为已完成 4.标为未完成 
    function changeStatus(title, btnname, toStatus, $t) {
        $.confirm(title, btnname, function () {
            var orderId = $t.attr('orderId');
            tool.request({
                url: '/Appoint/ChangeStatus',
                data: { toStatus: toStatus, orderId: orderId },
                dataType: 'json',
                type: "POST",
                success: function (d) {
                    if (d.success) {
                        $t.find('.status').text(d.msg.status);
                        $t.find('.optStatus').val(d.msg.opts);
                        tool.toptip("操作成功", 1);
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

    //绑定列表事件
    this.bindList= function($items) {
        $items.click(function() {
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
                actions: actions,
                onClose: function() {
                    $bottomTab.show();
                }
            });

            //控制底部tab
            $bottomTab.hide();

            function openBottomPop() {
                $bottomTab.show();
                $(this).unbind('click', openBottomPop);
            }

            $('.weui_actions_mask').click(openBottomPop);
        });
    }

    bindList($list.find('.weui_media_box'));

    return this;
})();

