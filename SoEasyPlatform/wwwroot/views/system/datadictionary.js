var configs = {
    url: {
        Get: _root + "system/getdatadictionary",
        Del: _root + "system/deletedatadictionary",
        SaveSystem: _root + "system/savedatadictionary",
        GetTree: _root + "system/getdatadictionaryAll"
    },
    window: {
        h: 350,
        w:500
    },
    tree: {
        mh:100
    }
};

TreeParentName.$SelectTree({
    isMultiple: false,
    url: configs.url.GetTree
})
 
saveParentName.$SelectTree({
    isMultiple: false,
    url: configs.url.GetTree,
    maxHeight: configs.tree.mh
})
 
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

btnReset.$Reset();


btnAdd.$Open("#divOpen", {
    title: "添加数据字典",
    w: configs.window.w,
    h: configs.window.h,
    validate: function () {
        frmSave.$ClearControls();
        return true;
    },
    yes: function () {
        frmSave.$Form({
            url: configs.url.SaveSystem,
            callback: function (msg) {
                debugger
                if (msg.IsKeyValuePair) {
                    $sugar.$Validate(msg.Data, "save");
                } else {
                    $sugar.$Validate("clear");
                    msg.Data.$Alert();
                    if (msg.IsSuccess) {
                        btnSearch.click();
                        $sugar.$CloseAll(divOpen.getAttribute("dataindex"));
                    }
                }
            }
        });
    },
    btn: ['添加', '关闭']
});

btnEdit.$Open("#divOpen", {
    title: "编辑数据字典",
    w: configs.window.w,
    h: configs.window.h,
    validate: function () {
        var gridInfo = divGrid.$GridInfo();
        if (gridInfo.length == 0) {
            "请选择记录".$Alert();
            return false;
        } else {
            gridInfo = gridInfo[0];
            frmSave.$FillControls(gridInfo);
            return true;
        }
    },
    yes: function () {
        frmSave.$Form({
            url: configs.url.SaveSystem,
            callback: function (msg) {
                if (msg.IsKeyValuePair) {
                    $sugar.$Validate(msg.Data, "save");
                } else {
                    $sugar.$Validate("clear");
                    msg.Data.$Alert();
                    if (msg.IsSuccess) {
                        btnSearch.click();
                        $sugar.$CloseAll(divOpen.getAttribute("dataindex"));
                    }
                }
            }
        });
    },
    btn: ['保存', '关闭']
});


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
                data: { "dataDictionarys": JSON.stringify(gridInfo) }
            })
        } else {
            "请选择一条数据".$Alert();
        }
    }
})


