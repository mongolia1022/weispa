var tool = (function () {
    //定时关闭提示
    this.tip = function (str, callback) {
        $.toast(str, callback);
    }
    this.toptip = function(str, type) {
        var i;
        switch (type) {
        case 0:
            i = 'error';
            break;
        case 1:
            i = 'success';
            break;
        case 2:
            i = 'warning';
            break;
        default:
            i = 'error';
            break;
        }
        $.toptip(str, i);
    }

    //等待动画
    this.wait = function () {
        $.showLoading();
    }

    //模态提示框
    this.modaltip = function (str) {
        dialog({
            content: str,
            okValue: '返回',
            ok: function () { this.close().remove() }
        }).showModal();
    }

    //关闭等待动画
    this.delWait = function () {
        $.hideLoading();
    }

    //统计中文字数
    this.gbcount = function (str) {
        str = str.replace(/\s/ig, "").replace(/[^\u4e00-\u9fa5]/g, '');
        var lenE = str.length;
        return lenE;
    }

    //统计中英文字数 中文算2个
    this.txtlength = function (str) {
        var lenAll = str.length;
        var ch = str.match(/[^\x00-\xff]/g);
        var lenCh = 0;
        if (ch != null)
            lenCh = ch.length;

        var lenEg = lenAll - lenCh;
        return lenEg + lenCh * 2;
    }

    //判断是否为空
    this.isEmpty = function (o) {
        return o === null || o === undefined || o === '';
    };
    //表单序列化为json
    this.getParams = function ($form) {
        var success = true;

        function getParams($o, prefix) {
            if (!success) return null;

            prefix = tool.isEmpty(prefix) ? '' : prefix + '-';
            var key = prefix + 'prop-name';
            var p;

            function getProp($that) {
                if (!success) return null;

                var propname = $that.attr(key);
                var datatype = $that.attr('prop-datatype');
                var result;
                if (datatype == 'list') { //列表
                    result = [];
                    $that.find('[' + propname + '-item]').each(function () {
                        var tempPrefix = prefix + propname;
                        if ($(this).find('[' + tempPrefix + '-prop-name]').length == 0) {
                            var $itemval = $(this).find('[' + tempPrefix + '-item-val]');
                            if ($itemval.length == 0)
                                result.push(getProp($(this)));
                            else
                                result.push(getProp($itemval));
                        } else
                            result.push(getParams($(this), tempPrefix));
                    });
                } else if (datatype == 'item') { //复杂类型
                    result = getParams($(this), prefix + propname);
                } else if (datatype == 'radio') { //单选
                    result = $that.find('input[type=radio]:checked').val();
                } else if (datatype == 'checkbox') { //复选
                    result = [];
                    $that.find('input[type=checkbox]:checked').each(function () {
                        result.push($(this).val());
                    });
                } else if ($that.is('input')) { //input
                    var type = $that.attr('type');
                    if (type == 'checkbox' || type == 'radio')
                        result = $that.prop("checked");
                    else
                        result = $that.val();
                } else if ($that.is('textarea') || $that.is('select')) { //textarea select
                    result = $that.val();
                } else if ($that.is('img')) {
                    result = $that.attr('src');
                } else { //普通文本
                    result = $that.text();
                }

                //默认值
                if (datatype == 'num') {
                    result = result == '' ? '0' : '';
                }

                var defval = $that.attr('def');
                if (defval) {
                    result = result == '' ? '0' : defval;
                }

                //空判断
                var empty = $that.attr('empty');
                if (!tool.isEmpty(empty)) {
                    if (tool.isEmpty(result) ||
                        tool.isArr(result) && result.length === 0) {
                        success = false;
                        tool.toptip(empty);
                        $that.focus();
                    }
                }

                //限制格式
                var reg = $that.attr('reg-exp');
                if (!tool.isEmpty(empty)) {
                    var re = new RegExp(reg);
                    if (!re.exec($that.val())) {
                        success = false;
                        tool.toptip($that.attr('reg-msg'));
                        $that.focus();
                    }
                }

                return result;
            }

            var $items = $o.find('[' + key + ']');
            if ($items.length > 0) {
                p = {};
                $items.each(function () {
                    var propname = $(this).attr(key);
                    p[propname] = getProp($(this));
                });
            } else
                p = getProp($o);

            return p;
        }

        var p = getParams($form);
        if (!success)
            return null;

        return p;
    }

    //json填充表单
    this.bindFormData = function (p, $form, prefix) {
        if (p == null) return;

        prefix = tool.isEmpty(prefix) ? '' : prefix + '-';
        var key = prefix + 'prop-name';

        function bindProp($that, pval) {
            var propname = $that.attr(key);

            var datatype = $that.attr('prop-datatype');
            if (datatype == 'list') { //列表
                if (!tool.isEmpty(pval)) {
                    var itemTemp = $that.find('[' + propname + '-item]:eq(0)');
                    $that.empty();
                    for (var j = 0; j < pval.length; j++) {
                        var temp = itemTemp.clone();
                        $that.append(temp);
                        var tempPrefix = prefix + propname;
                        if (temp.find('[' + tempPrefix + '-prop-name]').length == 0) {
                            var $itemval = temp.find('[' + tempPrefix + '-item-val]');
                            if ($itemval.length == 0) {
                                bindProp(temp, pval[j]);
                            } else {
                                bindProp($itemval, pval[j]);
                            }
                        } else
                            tool.bindFormData(pval[j], temp, tempPrefix);
                    }
                }
            } else if (datatype == 'item') { //复杂类型
                tool.bindFormData(pval, $(this), prefix + propname);
            } else if (datatype == 'radio') { //单选
                $that.find('input[type=radio]').each(function () {
                    $(this).prop('checked', $(this).val() == pval);
                });
            } else if (datatype == 'checkbox') { //复选
                function inarr(obj, arr) {
                    var i = arr.length;
                    while (i--) {
                        if (arr[i] == obj) {
                            return true;
                        }
                    }
                    return false;
                }
                $that.find('input[type=checkbox]').each(function () {
                    $(this).prop('checked', inarr($(this).val(), pval));
                });
            } else if ($that.is('input')) {//input
                var type = $that.attr('type');
                if (type == 'checkbox' || type == 'radio')
                    $that.prop("checked", pval);
                else
                    $that.val(pval);
            } else if ($that.is('textarea') || $that.is('select')) {//textarea select
                $that.val(pval);
            } else if ($that.is('img')) {
                $that.attr('src', pval);
            } else { //普通文本
                $that.text(pval);
            }
        }

        var $items = $form.find('[' + key + ']');
        if ($items.length > 0) {
            $items.each(function () {
                var $that = $(this);
                var propname = $that.attr(key);
                if (!p.hasOwnProperty(propname))
                    return;

                bindProp($that, p[propname]);
            });
        } else
            bindProp($form, p);
    };

    this.initForm = function ($form) {
        var preattr = 'temp-preval';
        //最大长度
        $form.find('[max-length]').keyup(function () {
            var $that = $(this);
            var max = parseInt($that.attr('max-length'));
            if (tool.txtlength($that.val()) > max) {
                $that.val($that.attr(preattr));
            } else
                $that.attr(preattr, $that.val());
        });
        
    }

    //发出请求
    this.request = function (opt, lockobj) {
        lockobj = lockobj || {};
        if (!lockobj.isdone && lockobj.isdone != undefined)
            return;

        lockobj.isdone = false;
        if (opt.loading)
            opt.wait = tool.wait();

        var successHandler = opt.success;
        var errorHandler = opt.error;

        opt.success = function (d) {
            lockobj.isdone = true;
            if (opt.loading)
                tool.delWait();

            successHandler(d);
        };

        opt.error = function (e, d) {
            lockobj.isdone = true;
            if (opt.loading)
                tool.delWait();

            if (opt.error)
                errorHandler(e, d);
        }

        opt.url = opt.url + (opt.url.indexOf('?') == -1 ? '?' : '') + 'r=' + Date.parse(new Date());
        opt.dataType = opt.dataType || 'json';

        $.ajax(opt);
    }

    //post方式刷新页面
    this.reload = function () {
        $('form:eq(0)').append('<input type="hidden" name="_pagereload" value="1">').submit();
    };

    return this;
})();

//类型判断扩展
(function () {
    var type = {
        str: '[object String]',
        arr: '[object Array]',
        num: '[object Number]',
        func: '[object Function]',
        obj: '[object Object]',
        bool: '[object Boolean]',
        date: '[object Date]',
        unde: '[object Undefined]',
        nil: '[object Null]'
    }

    function getType(arg) {
        return Object.prototype.toString.call(arg);
    }


    tool.isDate = function (arg) {
        return getType(arg) === type.date;
    }
    tool.isNull = function (arg) {
        return getType(arg) === type.nil;
    }
    tool.isNum = function (arg) {
        return getType(arg) === type.num;
    }
    tool.isFunc = function (arg) {
        return getType(arg) === type.func;
    }
    tool.isArr = function (arg) {
        return getType(arg) === type.arr;
    }
    tool.isObj = function (arg) {
        return getType(arg) === type.obj;
    }
    tool.isUndefined = function (arg) {
        return getType(arg) === type.unde;
    }
    tool.isBool = function (arg) {
        return getType(arg) === type.bool;
    }
    tool.isStr = function (arg) {
        return getType(arg) === type.str;
    }
})();