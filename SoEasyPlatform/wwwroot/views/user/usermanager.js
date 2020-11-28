var configs = {
    url: {
        GetUsers: _root + "user/getusers",
        Del: _root + "user/deleteuser",
        SaveSystem: _root + "user/saveuser"
    }
};
divFrom.$Form({
    url: configs.url.GetUsers,
    callback: function (msg) {
        divGrid.$Grid(msg.Data);
    }
})
btnSearch.$Button({
    url: configs.url.GetUsers,
    callback: function (msg) {
        divGrid.$Grid(msg.Data);
    }
});

btnReset.$Reset();

btnAdd.$Open("#divOpen", {
    title: "添加用户",
    w: 950,
    h: 400,
    validate: function () {
        frmSave.$ClearControls();
        return true;
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
    btn: ['添加', '关闭']
});

btnEdit.$Open("#divOpen", {
    title: "编辑用户",
    w: 800,
    h: 400,
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
    btn: ['保存', '关闭']
});

btnDel.$Confirm({
    title:"是否删除记录",
    ok: function () {
        var gridInfo = divGrid.$GridInfo();
        if (gridInfo.length > 0) {
            configs.url.Del.$Ajax({
                callback: function (msg) {
                    "删除成功".$Alert();
                    btnSearch.click();
                },
                data: { "users": JSON.stringify(gridInfo) }
            })
        } else
        {
            "请选择一条数据".$Alert();
        }
    }
})