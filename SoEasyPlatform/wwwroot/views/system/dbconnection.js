var configs = {
    url: {
        GetUsers: _root + "system/getdbconnection",
        Del: _root + "system/deletedbconnection",
        SaveSystem: _root + "system/savedbconnection"
    },
    text:
    {
        add: "添加数据库连接",
        edit:"修改数据库连接"
    }
};
divFrom.$Form({
    url: configs.url.GetUsers,
    callback: function (msg) {
        msg.data.Dblfunc = function () {
            btnEdit.click();
        };
        divGrid.$Grid(msg.data);
    }
})
btnSearch.$Button({
    url: configs.url.GetUsers,
    callback: function (msg) {
        msg.data.Dblfunc = function () {
            btnEdit.click();
        };
        divGrid.$Grid(msg.data);
    }
});

btnReset.$Reset();


btnAdd.$Open("#divOpen", {
    title: configs.text.add,
    w: 400,
    h: 300,
    validate: function () {
        frmSave.$ClearControls();
        return true;
    },
    yes: function () {
        frmSave.$Form({
            url: configs.url.SaveSystem,
            callback: function (msg) {
                if (msg.isKeyValuePair) {
                    $sugar.$Validate(msg.data, "save");
                } else {
                    $sugar.$Validate("clear");
                    msg.data.$Alert();
                    if (msg.isSuccess) {
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
    title: configs.text.edit,
    w: 400,
    h: 300,
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
                    $sugar.$Validate(msg.data, "save");
                } else {
                    $sugar.$Validate("clear");
                    msg.data.$Alert();
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
                    else
                    {
                        msg.data.$Alert();
                    }
                },
                data: { "systems": JSON.stringify(gridInfo) }
            })
        } else {
            "请选择一条数据".$Alert();
        }
    }
})


 