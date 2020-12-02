/// <reference path="../../vendors/jquery/dist/jquery.js" />
/// <reference path="../../vendors/jquery-forms/jquery.forms.js" />
/// <reference path="../../vendors/sugarjs/sugar.js" />
var _root = "/api/";
var SugarContext = {
    Form: function (element, value) {
        $(function () {
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
                    source: [{ id: 0, title: "根目录", subs: msg.Data, isSelectable: value.rootIsSelect}],
                    isMultiple: value.isMultiple,
                    cascadeSelect: value.cascadeSelect,
                    collapse: value.collapse
                });
                if (value.maxHeight != null)
                {
                    $(element).parent().next().css("max-height", value.maxHeight);
                    $(element).parent().next().find(".comboTreeItemTitle ").first().next().css("height", value.maxHeight+20);
                }
                //selectTree.setSource(SampleJSONData2);
             
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
                    if (msg.Issuccess == false && msg.Url == "error")
                    {
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
            if (control.is(":input")) {
                control.val(json[p]);
            }
        }
    }
    ,
    ClearControls: function (element) {
        $(element).find(":input").val("");
        $(element).find(":hidden").val("");
    }
    ,
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
        if (data.pagination == null)
        {
            this.Alert(data);
            return;
        }
      
        $(element).bootstrapTable(data);
        $(element).bootstrapTable("refresh");

        $(element).bootstrapTable('resetView', { height: 600 });
 
        var totalpage = (data.total + data.pageSize - 1) / data.pageSize;
        if (data.total == 0 || data.total < data.pageSize)
        {
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
        var index = layer.open({
            title: data.title,
            type: 1,
            skin: 'layui-layer-rim', //加上边框
            area: [w + 'px', h + 'px'], //宽高
            content: $(divElement),
            btn: data.btn,
            yes: data.yes
        });
        $(divElement).attr("dataIndex", index)
    },
    CloseAll: function (index) {

        layer.close(index);
    },
    Loading: function (divElement)
    {
        //loading层

        var index = layer.load(1, {
            shade: [0.1, '#fff'] //0.1透明度的白色背景
        });
        $(divElement).data("loadingIndex", index);
        return index;
    },
    CloseLoading: function (divElement) {
        //loading层
        var index = $(divElement).data("loadingIndex");
        layer.close(index);
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
    }
};
$sugar.init(SugarContext);