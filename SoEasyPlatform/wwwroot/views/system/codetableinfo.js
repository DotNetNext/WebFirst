var mynewtable;
var data = {
    Columns:
        [
            ["Id", "Id", "", "编号", 1, 1, 1]
        ]
};
for (var i = 0; i < 8; i++) {
    data.Columns.push([]);
}
var selectTypeData;
function GetData() {
    var json = {
        "ClassName": $("#txtClassName").val(),
        "TableName": $("#txtTableName").val(),
        "Description": $("#txtDesc").val(),
        "ColumnInfoList": []
    };
    var columns = mynewtable.getData();
    if (columns != null) {
        $.each(columns, function (i, v) {
            if (v.length >= 7) {
                var propertyName = v[0];
                var fieldName = v[1];
                var ctype = v[2];
                var desc = v[3];
                var required = v[4];
                var isPk = v[5];
                var isIdentity = v[6];
                json.ColumnInfoList.push({
                    ClassProperName: propertyName,
                    DbColumnName: fieldName,
                    Required: required,
                    IsIdentity: isIdentity,
                    IsPrimaryKey: isPk,
                    Description: desc,
                    CodeType: ctype
                });
            }
        })
    }
    return json;
}
function InitEelement() {
    mynewtable = $('#examplex').editTable({
        field_templates: {
            'checkbox': {
                html: '<input type="checkbox"/>',
                getValue: function (input) {
                    return $(input).is(':checked');
                },
                setValue: function (input, value) {
                    if (value) {
                        return $(input).attr('checked', true);
                    }
                    return $(input).removeAttr('checked');
                }
            },
            'textarea': {
                html: '<textarea/>',
                getValue: function (input) {
                    return $(input).val();
                },
                setValue: function (input, value) {
                    return $(input).text(value);
                }
            },
            'select': {
                html: '<select class="selCstype"><option  class="option_init"></option></select>',
                getValue: function (input) {
                    return $(input).val();
                },
                setValue: function (input, value) {
                    var select = $(input);
                    select.find('option').filter(function () {
                        return $(this).val() == value;
                    }).attr('selected', true);
                    return select;
                }
            }
        },
        row_template: ['text', 'text', 'select', 'text', 'checkbox', 'checkbox', 'checkbox'],
        headerCols: ['实体属性', '数据字段(可不填)', "类型", '备注', '必填', '主键', '自增'],
        first_row: false,
        data: data.Columns,
        tableClass: 'inputtable custom'
    });
    var ajaxParam = {
        callback: function (msg) {
            $(".selCstype").each(function () {
                var th = $(this);
                th.html("");
                selectTypeData = msg.Data;
                $.each(msg.Data, (function (i, v) {
                    th.append("<option>" + v.title + "</option>");
                }));
            });
        }
    };
    (_root + "system/getdatatype").$Ajax(ajaxParam);
}
function InitEevent() {
    $("#txtClassName").blur(function () {
        var th = $(this);
        var thVal = th.val();
        var table = $("#txtTableName");
        var tableValue = table.val();
        if (tableValue == null || tableValue == "") {
            table.val(thVal);
        }
    });
}
InitEelement();
InitEevent();
setInterval(function () {
    var size = $(".option_init").size();
    if (size > 0) {
        var sel = $(".option_init").parent();
        sel.html("");
        $.each(selectTypeData, (function (i, v) {
            sel.append("<option>" + v.title + "</option>");
        }));
    }
}, 600);