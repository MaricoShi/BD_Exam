(function ($) {
    $.setting = {
        //wsurl:"http://localhost:8077/WS/BGSPORT.asmx",
    },
    //禁用
    $.ban = {
        //禁用右键,需要再文档加载完毕时候使用
        banMouseRight: function (fn) {
            $("body").bind("contextmenu", function () { return false; });
            if (fn) {
                fn();
            }
        },
        //禁用全选
        banSelectAll: function (fn) {
            $("body").bind("selectstart", function () { return false; });
            if (fn) {
                fn();
            }
        },
        //禁用F12
        banF12: function (fn) {
            $("body").bind('keypress', function (e) {
                if (e.keyCode == 123) return false;
            });
        },
        //禁止所有
        banAll: function () {
            $.ban.banMouseRight();
            $.ban.banSelectAll();
            $.ban.banF12();
        }
    },
    $.web = {
        //获取今日日期
        today:function (_s) {
            var date = new Date(),
                MM = date.getMonth()+1,
                Month = MM < 10 ? "0" + MM : MM,
                Day = date.getDate() < 10 ?"0" + date.getDate():date.getDate();
                return _s ? date.getFullYear() + _s + Month + _s + Day : date.getFullYear() + "-" + Month + "-" + Day;
        },
        //获取当前时间
        Now: function (_s) {
            var date = new Date(),
                MM = date.getMonth() + 1,
                Month = MM < 10 ? "0" + MM : MM,
                Day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate(),
                HH = date.getHours(),
                mm = date.getMinutes(),
                ss = date.getSeconds();
            return _s ? date.getFullYear() + _s + Month + _s + Day + " " + HH + ":" + mm + ":" + ss : date.getFullYear() + "-" + Month + "-" + Day + " " + HH + ":" + mm + ":" + ss;
        },
        ///在现有时间上加上指定的天数，返回新的时间字符串
        dateAddDays: function (date, days) {
            dateArray = String(date).split('-');
            if (dateArray.length != 3) {
                return;
            }
            var newDate = new Date(new Date(dateArray[0], eval(dateArray[1]) - 1, dateArray[2]).valueOf() + days * 24 * 60 * 60 * 1000), // 日期加上指定的天数
                newYear = newDate.getFullYear(),
                newMonth = (newDate.getMonth() < 9 ? "0" : "") + (newDate.getMonth() + 1),
                newDay = (newDate.getDate() < 10 ? "0":"") + newDate.getDate();
                return newYear + "-" + newMonth + "-" + newDay;
        },
        changeDateFormat: function (cellval, timeformat) {
            if (!cellval) {
                return "";
            }
            if (cellval.indexOf("/Date") == 0) {
                d = new Date();
                //localOffset = d.getTimezoneOffset() * 60000;
                //var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10) + localOffset);
                var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
                var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
                var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
                var type = "-";
                type = timeformat ? timeformat : type;
                return date.getFullYear() + type + month + type + currentDate;
            }
            else {
                return cellval;
            }
        },
        //将json中的DateTime转换成toLocaleTime
        changetoLocaleTimeString: function (cellval, timeformat) {
            if (!cellval) {
                return "";
            }
            d = new Date();
            //localOffset = d.getTimezoneOffset() * 60000;
            //var date = new Date(parseInt(cellval.substr(6)) + localOffset);
            var date = new Date(parseInt(cellval.substr(6)));
            var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
            var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
            var hour = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
            var minute = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
            var seconds = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();

            var type = "-";
            type = timeformat ? timeformat : type;
            return date.getFullYear() + type + month + type + currentDate + " " + hour + ":" + minute + ":" + seconds;
        },
        //将json中的DateTime转换成toLocaleTime
        changetoLocaleTimeString2hm: function (cellval, timeformat) {
            if (!cellval) {
                return "";
            }
            d = new Date();
            //localOffset = d.getTimezoneOffset() * 60000;
            //var date = new Date(parseInt(cellval.substr(6)) + localOffset);
            var date = new Date(parseInt(cellval.substr(6)));
            var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
            var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
            var hour = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
            var minute = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
            //var seconds = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();

            var type = "-";
            type = timeformat ? timeformat : type;
            return date.getFullYear() + type + month + type + currentDate + " " + hour + ":" + minute;
        },
        //获取地址栏参数
        getUrlParam: function (name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
            var r = window.location.search.substr(1).match(reg);  //匹配目标参数
            if (r != null) {
                return unescape(r[2]);
            }
            return null; //返回参数值
        },
        //判断是否为空或null
        IsNullOrEmpty: function (_v) {
            if (_v) {
                var ret;
                if (_v == null || _v == "") {
                    ret = true;
                }
                else {
                    ret = false;
                }
                return ret;
            }
        },
        //字符串截取
        Substr: function (_str,_len,_t) {
            if (_str&&_len) {
                return _str.length > _len ? _str.substr(0, _len) +(_t||""): _str;
            }
            return "";
        },
        //加载数据动画
        AddLoading: function (_txt) {
            if ($('#wxloading').length == 0) {
                $('<div>', { id: 'wxloading' })
                    .append(
                        $('<div>', { 'class': 'wx_loading_inner' })
                            .append($('<svg>', { 'class': 'wx_loading_icon' }))
                            .append($('<span>').text(_txt ? _txt : "正在加载..."))
                    ).appendTo($('body'));
            }
            else {
                $('#wxloading').show();
            }
        },
        //移除加载动画
        RemoveLoading: function () {
            $('#wxloading').remove();
        },
        AddLoadingSetClose: function (_txt, _fn, _time) {
            $.web.AddLoading(_txt);
            setTimeout(function () {
                if (_fn) {
                    _fn();
                }
                $.web.RemoveLoading();
            }, _time || 1000);
        },
        get: function (_url, successFnback, failFnback) {
            $.ajax({
                url: encodeURI(_url),
                async: false,
                type: "GET",
                dataType: "json"
            })
            .done(function (xhr) {
                if (successFnback) {
                    successFnback(xhr);
                }
            })
            .fail(function (err) {
                if (failFnback) {
                    failFnback(err);
                }
            });
        },
        getAsync: function (_url, successFnback, failFnback) {
            $.ajax({
                url: encodeURI(_url),
                type: "GET",
                dataType: "json"
            })
            .done(function (xhr) {
                if (successFnback) {
                    successFnback(xhr);
                }
            })
            .fail(function (err) {
                if (failFnback) {
                    failFnback(err);
                }
            });
        },
        post: function (_url, _data, successFnback, failFnback) {
            $.ajax({
                url: encodeURI(_url),
                async: false,
                type: "POST",
                data: JSON.stringify(_data),
                contentType: "application/json;application/x-www-form-urlencoded;charset=utf-8",
                dataType: 'json'
            })
            .done(function (xhr) {
                if (successFnback) {
                    successFnback(xhr);
                }
            })
            .fail(function (err) {
                if (failFnback) {
                    failFnback(err);
                }
            });
        },
        postAsync: function (_url, _data, successFnback, failFnback)
        {
            $.ajax({
                url: encodeURI(_url),
                type: "POST",
                data: JSON.stringify(_data),
                contentType: "application/json;application/x-www-form-urlencoded;charset=utf-8",
                dataType: 'json'
            })
            .done(function (xhr) {
                if (successFnback) {
                    successFnback(xhr);
                }
            })
            .fail(function (err) {
                if (failFnback) {
                    failFnback(err);
                }
            });
        },
        //默认为异步
        postWithOption: function (_option, _funobj) {
            try
            {
                var ajaxconfig =
                {
                    type: "POST",
                    contentType: "application/json;application/x-www-form-urlencoded;charset=utf-8",
                    dataType: 'json'
                },
                funconfig = {
                    OnSuccess: function () {

                    },
                    onFailure: function () {

                    },
                    OnComplete: function () {

                    }
                };
                _option.url = encodeURI(_option.url);//地址栏里转码
                _option.data = JSON.stringify(_option.data);//序列化
                $.extend(ajaxconfig, _option);
                if (_funobj) {
                    $.extend(funconfig, _funobj);
                }
                $.ajax(ajaxconfig)
                .done(function (xhr) {
                    funconfig.OnSuccess(xhr);
                })
                .fail(function (err) {
                    funconfig.onFailure(err);
                })
                .always(function () {
                    funconfig.OnComplete();
                });
            }
            catch (e)
            {

            }
        }
    }

})(jQuery)