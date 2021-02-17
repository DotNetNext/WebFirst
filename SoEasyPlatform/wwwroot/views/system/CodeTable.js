var configs = {
    url: {
        Get: _root + "codetable/getcodetablelist",
        Del: _root + "codetable/deleteCodetable",
        GetDatabase: _root + "system/getdatabase",
        Info: "/CodeTableInfo",
        Save: _root + "codetable/savecodetable",
        SaveImport: _root + "codetable/savecodetableimport",
        Import: "/CodeTableImport",
        GetNetVersion: _root + "system/getnetversion",
        GetTemp: _root + "system/getTemplate?type=1",
        GetNuget: _root + "system/getnuget",
        CreateFile: _root + "codetable/createfile",
    },
    text:
    {
        add: "创建虚拟类",
        addDbFirst: "导入虚拟类",
        edit: "修改虚拟类"
    },
    w: {
        w: "100%",
        h: "100%"
    }
};

divFrom.$Form({
    url: configs.url.Get,
    callback: function (msg) {
        msg.Data.Dblfunc = function () {
            btnEdit.click();
        };
        divGrid.$Grid(msg.Data);
    }
})

btnSearch.$Button({
    url: configs.url.Get,
    callback: function (msg) {
        msg.Data.Dblfunc = function () {
            btnEdit.click();
        };
        divGrid.$Grid(msg.Data);
    }
});

txtDbIdName.$SelectTree({
    isMultiple: false,
    url: configs.url.GetDatabase,
    maxHeight: 180,
    rootIsSelect: false
})

txtDbIdName.onchange = function () {
    btnSearch.click();
}

btnReset.$Reset();

saveTemplateName1.$SelectTree({
    isMultiple: false,
    url: configs.url.GetTemp,
    maxHeight: 180,
    rootIsSelect: false
})

saveNetVersionName.$SelectTree({
    isMultiple: false,
    url: configs.url.GetNetVersion,
    maxHeight: 180,
    rootIsSelect: false
})

saveNetVersionName.onchange = function () {
    if (saveNetVersion.value <= 1) {
        divLib.$AddClass("none")
    }
    else {
        divLib.$RemoveClass("none")
    }
}

saveNugetName.$SelectTree({
    isMultiple: false,
    url: configs.url.GetNuget,
    maxHeight: 180,
    rootIsSelect: false
})

btnAdd.$Open("#divOpen", {
    title: configs.text.add,
    w: configs.w.w,
    h: configs.w.h,
    url: configs.url.Info,
    validate: function () {

        if (txtDbId.value == null || txtDbId.value == "" || txtDbId.value == "0") {
            "请选择数据库".$Alert();
            return false;
        }
        else {
            return true;
        }
    },
    yes: function () {
        var data = document.getElementsByTagName("iframe")[0].contentWindow.GetData();
        data.DbId = txtDbId.value;
        configs.url.Save.$Ajax({
            callback: function (msg) {
                if (msg.IsSuccess) {
                    "添加成功".$Alert();
                    $sugar.$CloseAll(divOpen.getAttribute("dataindex"));
                    btnSearch.click();
                }
                else {
                    msg.Data.$Alert();
                }
            },
            data: { "model": JSON.stringify(data) }
        })
    },
    btn: ['保存', '关闭']
});

btnEdit.$Open("#divOpen", {
    title: configs.text.edit,
    w: configs.w.w,
    h: configs.w.h,
    url: configs.url.Info,
    format: function (msg) {
        msg.url = configs.url.Info + "?id=" + divGrid.$GridInfo()[0].Id;
    },
    validate: function () {
        var gridInfo = divGrid.$GridInfo();
        if (gridInfo.length == 0) {
            if (txtDbId.value == null || txtDbId.value == "" || txtDbId.value == "0") {
                "请选择数据库".$Alert();
                return false;
            } else if (gridInfo.length == 0) {
                "请选择记录".$Alert();
                return false;
            }
        }
        return true;
    },
    yes: function () {
        var data = document.getElementsByTagName("iframe")[0].contentWindow.GetData();
        data.DbId = txtDbId.value;
        configs.url.Save.$Ajax({
            callback: function (msg) {
                if (msg.IsSuccess) {
                    "添加成功".$Alert();
                    $sugar.$CloseAll(divOpen.getAttribute("dataindex"));
                    btnSearch.click();
                }
                else {
                    msg.Data.$Alert();
                }
            },
            data: { "model": JSON.stringify(data) }
        })
    },
    btn: ['保存', '关闭']
});

btnDbFirstAdd.$Open("#divOpen", {
    validate: function () {
        if (txtDbId.value == null || txtDbId.value == "" || txtDbId.value == "0") {
            "请选择数据库".$Alert();
            return false;
        }
        else {
            return true;
        }
    },
    title: configs.text.addDbFirst,
    w: configs.w.w,
    h: configs.w.h,
    url: configs.url.Import,
    format: function (msg) {
        msg.url = configs.url.Import + "?dbId=" + txtDbId.value;
    },
    yes: function () {
        var data = document.getElementsByTagName("iframe")[0].contentWindow.GetData();
        configs.url.SaveImport.$Ajax({
            callback: function (msg) {
                if (msg.IsSuccess) {
                    "添加成功".$Alert();
                    $sugar.$CloseAll(divOpen.getAttribute("dataindex"));
                    btnSearch.click();
                }
                else {
                    msg.Data.$Alert();
                }
            },
            data: { "dbid": txtDbId.value, "model": JSON.stringify(data) }
        })
    },
    btn: ['导入', '关闭']
})

btnDel.$Confirm({
    title: "是否删除记录",
    ok: function () {
        var gridInfo = divGrid.$GridInfo();
        if (gridInfo.length > 0) {
            configs.url.Del.$Ajax({
                callback: function (msg) {
                    if (msg.IsSuccess) {
                        "删除成功".$Alert();
                        btnSearch.click();
                    }
                    else {
                        msg.Data.$Alert();
                    }
                },
                data: { "model": JSON.stringify(gridInfo) }
            })
        } else {
            "请选择一条数据".$Alert();
        }
    }
})

btnPath.$Open("#divPath", {
    title: configs.text.add,
    w: 600,
    h: 480,
    validate: function () {
        var gridInfo = divGrid.$GridInfo();
        if (txtDbId.value == null || txtDbId.value == "" || txtDbId.value == "0") {
            "请选择数据库".$Alert();
            return false;
        } else if (gridInfo.length == 0) {
            "请选择记录".$Alert();
            return false;
        } else {
            return true;
        }
    },
    yes: function () {
        var gridInfo = divGrid.$GridInfo();
        if (gridInfo.length > 0) {
            SaveTable1.value = JSON.stringify(gridInfo);
            frmPathSave.$Form({
                url: configs.url.CreateFile,
                callback: function (msg) {
                    if (msg.IsKeyValuePair) {
                        $sugar.$Validate(msg.Data, "save");
                    } else {
                        $sugar.$Validate("clear");
                        msg.Message.$Alert();
                    }
                }
            });
        } else {
            "请选择一条数据".$Alert();
        }
    },
    btn: ['保存', '关闭']
});

btnProject.$Open("#divProject", {
    title: configs.text.add,
    w: configs.w.w,
    h: configs.w.h,
    validate: function () {

        if (txtDbId.value == null || txtDbId.value == "" || txtDbId.value == "0") {
            "请选择数据库".$Alert();
            return false;
        }
        else {
            return true;
        }
    },
    yes: function () {

    },
    btn: ['保存', '关闭']
});


