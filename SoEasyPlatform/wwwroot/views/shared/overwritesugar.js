/// <reference path="../../vendors/jquery/dist/jquery.js" />
/// <reference path="../../vendors/jquery-forms/jquery.forms.js" />
/// <reference path="../../vendors/sugarjs/sugar.js" />
//重写sugar.js接口实现
var _root = hidRoot.value;
var SugarContext = {
    Form: function (element, value) {
        $(function () {
            if (value.before != null) {
                value.before();
            }
            var url = value.url;
            var callback = value.callback;
            var ajaxFormOption = {
                type: 'post',
                url: url,
                success: function (msg) { //提交成功的回调函数 
                    if (callback != null) {
                        callback(msg);
                    }
                },
                error: function (msg) {
                    layer.msg("服务器请求失败.");
                }
            };
            $(element).ajaxSubmit(ajaxFormOption);
        })
    },
    SelectTree: function (element, value) {
        if (value == "bind") {
            var warp = $(element).closest(".comboTreeWrapper");
            var ids = warp.next().val().split(",");
            var text = "";
            warp.find("[data-id]").each(function (i, v) {
                var th = $(this);
                var thid = th.data("id")+"";
                if (ids.indexOf(thid)>=0)
                {
                    text += th.text()+",";
                }
            });
            element.value = text.substr(0, text.length - 1);
            return;
        }
        if (value.cascadeSelect == undefined)
            value.cascadeSelect = null;
        if (value.collapse == undefined)
            value.collapse = null;
        $.ajax({
            url: value.url,
            type: "post",
            success: function (msg) {
                var url = value.url;
                var selectTree = $(element).comboTree({
                    source: [{ id: 0, title: "根目录", subs: msg.Data, isSelectable: value.rootIsSelect }],
                    isMultiple: value.isMultiple,
                    cascadeSelect: value.cascadeSelect,
                    collapse: value.collapse
                });
                $(element).data("treeobj", selectTree);
                if (value.maxHeight != null) {
                    $(element).parent().next().css("max-height", value.maxHeight);
                    $(element).parent().next().find(".comboTreeItemTitle ").first().next().css("height", value.maxHeight + 20);
                }

                if (value.width != null) {
                    $(element).parent().css("width", value.width);
                }
                //selectTree.setSource(SampleJSONData2);
                var treeParent = $(element).parent().parent().find(".ComboTreeItemParent ");
                treeParent.click(function () {
                    treeParent.closest(".comboTreeDropDownContainer").hide();
                    setTimeout(function () {
                        treeParent.find("ul").show();
                    }, 150);
                });
                if (value.isMultiple == true) {
                    $(element).change(function () {
                        var cbs = treeParent.closest(".comboTreeDropDownContainer").find(":checked");
                        var ids = [];
                        $(cbs).each(function (i, v) {
                            ids.push($(this).parent().data("id"));
                        });
                        $(element).closest(".comboTreeWrapper").next().val(ids.join(','));
                    })
                }
            },
            error: function (msg) {
                layer.msg("服务器请求失败.");
            }
        })
    },
    RestSelectTree: function (element, value)
    {
        $.ajax({
            url: value.url,
            type: "post",
            success: function (msg) {
                $(saveProjectName).data("treeobj").setSource(msg.Data);
            },
            error: function (msg) {
                layer.msg("服务器请求失败.");
            }
        })
    },
    Fill: function (element, value) {
        if (value.SugarUrl != null) {
            $.ajax({
                url: value.SugarUrl,
                type: "post",
                success: function (msg) {

                    var isArray = $.isArray(msg.Data)
                    var vm = new Vue({
                        el: '#' + element.id,
                        data: isArray ? msg : msg.Data
                    })
                    if (msg.Issuccess == false && msg.Url == "error") {
                        window.location.href = "/";
                    }
                },
                error: function (msg) {
                    layer.msg("服务器请求失败.");
                }
            })
        } else {
            var vm = new Vue({
                el: '#' + element.id,
                data: value
            })
        }
    },
    FillControls: function (element, json, pre) {
        if (pre == undefined)
            pre = "";
        for (var p in json) {
            var control = $(element).find("[name='" + pre + p + "']");
            if (control.attr("type") == "checkbox")
            {
                if (json[p] == true)
                {
                    control.prop('checked', true)
                }
            }
            else if (control.is(":input")) {
                control.val(json[p]);
            }
        }
    },
    ClearControls: function (element) {
        $(element).find(":input").val("");
        $(element).find(":hidden").val("");
        $(element).find("[type='checkbox']").prop("checked", false);
        $(element).find("[type='checkbox']").val(true);
    },
    Grid: function (element, data) {

        
        $(element).bootstrapTable('destroy');
        data.onDblClickRow = function (row, ele) {
            var column = data.columns[1];
            $(element).bootstrapTable("checkBy", { field: column.field, values: [row[column.field]] });
            if (data.Dblfunc != null) {
                data.Dblfunc(row, ele);
            }
            $(element).bootstrapTable("uncheckBy", { field: column.field, values: [row[column.field]] });
        }
        data.pagination = false;
        if (data.pagination == null) {
            this.Alert(data);
            return;
        }

        $(element).bootstrapTable(data);
        $(element).bootstrapTable("refresh");
        if ($(".navbar-nav").size() > 0)
        {
            $(element).bootstrapTable('resetView', { height: $(window).height() - 420 });
        }
        else
        {
            $(element).bootstrapTable('resetView', { height: $(window).height() - 210 });
        }
        var totalpage = (data.total + data.pageSize - 1) / data.pageSize;
        if (data.total == 0 || data.total < data.pageSize) {
            totalpage = 1;
        }
        var options = {
            itemTexts: function (type, page, current) {
                switch (type) {
                    case "first": return "首页";
                    case "prev": return "上一页";
                    case "next": return "下一页";
                    case "last": return "末页";
                    case "page": return page;
                }
            },
            numberOfPages: 10,
            currentPage: data.pageNumber,
            totalPages: totalpage,
            onPageClicked: function (event, originalEvent, type, page) {
                $("[name=PageIndex]").val(page);
                btnSearch.click();
            }
        }

        $('#' + element.id + 'Page').bootstrapPaginator(options);
        $('#' + element.id + 'Page').append("<li class='page-total'>共" + data.total + "条<li>")
    },
    GridInfo: function (element) {
        var rows = $(element).bootstrapTable('getSelections');
        return rows;
    },
    Button: function (formElement, value) {
        if (value.before != null)
        {
            value.before();
        }
        var context = this;
        SugarContext.Form($(formElement).closest("form")[0], value);
    },
    Reset: function (formElement) {
        $(formElement).closest("form")[0].reset();
    },
    Tip: function (element, value) {
        layer.tips(value, "#" + element.id);
    },
    Alert: function (message) {
        layer.msg(message.toString());
    },
    Confirm: function (data) {
        layer.confirm(data.title, {
            btn: ['确定', '取消'] //按钮
        }, function () {
            data.ok();
        }, function () {

        });
    },
    Open: function (divElement, data) {
        $(".sugar-tip").removeClass("sugar-tip");
        $(".sugar-tip-message ").remove();
        if (data.validate != null && data.validate() == false) {
            return false;
        }
        if (data == null) {
            data = {};
        }
        var w = data.w == null ? 800 : data.w;
        var h = data.h == null ? 500 : data.h;
        if (w == "100%") {
            w = $(window).width() - $(window).width() / 5;
        }
        if (h == "100%") {
            h = $(window).height() - $(window).height() / 5;
        }
        if (data.format != null)
        {
            data.format(data);
        }
        var content = $(divElement);
        var type = 1;
        if (data.url != null) {
            type = 2;
            content = data.url;
        }
        var index = layer.open({
            title: data.title,
            type: type,
            skin: 'layui-layer-rim', //加上边框
            area: [w + 'px', h + 'px'], //宽高
            content: content,
            btn: data.btn,
            yes: data.yes
        });
        $(divElement).attr("dataIndex", index)
    },
    CloseAll: function (index) {

        layer.close(index);
    },
    Loading: function (divElement) {
        //loading层

        var index = layer.load(1, {
            shade: [0.1, '#fff'] //0.1透明度的白色背景
        });
        $(divElement).data("loadingIndex", index);
        $(".layui-layer-loading").css("z-index", 999999999999999);
        $(".layui-layer-shade").css("z-index", 999999999999998);
        return index;
    },
    CloseLoading: function (divElement) {
        //loading层
        var index = $(divElement).data("loadingIndex");
        layer.close(index);
        $(".layui-layer-shade").remove();
        return index;
    },
    Validate: function (json, IdPrev) {
        $(".sugar-tip").removeClass("sugar-tip");
        $(".sugar-tip-message ").remove();
        if (json == "clear") return;
        $.each(json, function (i, v) {

            $("#" + IdPrev + v.Key).addClass("sugar-tip");
            var mid = IdPrev + v.Key + "message";
            $("#" + mid).remove();
            $("#" + IdPrev + v.Key).after("<div id='" + mid + "' class='sugar-tip-message' >" + v.Value + "</div>")
        });

    },
    Ajax: function (url, data) {
        $.ajax({
            beforeSend: function (xhr) {
                xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            },
            type: 'post',
            url: url.toString(),
            data: data.data,
            dataType: 'json',
            success: data.callback,
            error: function (msg) {
                layer.msg("服务器请求失败.");
            }
        })
    },
    QueryString: function (obj) {
        var variable = obj.toString();
        var query = window.location.search.substring(1);
        var vars = query.split("&");
        for (var i = 0; i < vars.length; i++) {
            var pair = vars[i].split("=");
            if (pair[0] == variable) { return pair[1]; }
        }
        return (false);
    },
    AddClass: function (element,className)
    {
        $(element).addClass(className);
    },
    RemoveClass: function (element, className)
    {
        $(element).removeClass(className);
    },
    AddCss: function (element, obj)
    {
        $(element).css(obj)
    },
    WindowHeight: function ()
    {
        var result = $(window).height();
        return result;
    },
    AjaxAopLoadingInit: function () {
        //AJAX AOP处理Loading
        $(function () {
            $.ajax({
                beforeSend: function () {
                    SugarContext.Loading(document.getElementsByTagName("body")[0])
                },
                complete: function () {
                    SugarContext.CloseLoading(document.getElementsByTagName("body")[0])
                }
                // ...
            });
        })
    },
    Onresize: function () {
        window.onresize = function ()
        {

            $(".x_panel").css({ height: $(window).height() - 45 });
        }
    }
};
$sugar.init(SugarContext);
SugarContext.AjaxAopLoadingInit();
SugarContext.Onresize();

