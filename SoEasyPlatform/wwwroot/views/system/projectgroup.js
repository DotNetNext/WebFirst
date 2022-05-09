var configs = {
    url: {
        Get: _root + "projectgroup/GetProjectGroupList",
        SaveSystem: _root + "projectgroup/SaveProjectGroup",
        GetProjet: _root + "system/GetProject",
        BuilderProjects: _root + "projectgroup/BuilderProjects",
        Del: _root+"projectgroup/deleteProjectGroup"
    },
    text:
    {
        add: "添加一键生成",
        edit:"修改一键生成"
    },
    w: {
        w: 750,
        h:400
    }
};

saveProjectNamesName.$SelectTree({
    isMultiple: true,
    url: configs.url.GetProjet,
    maxHeight: 200,
    rootIsSelect: false
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
    title: configs.text.add,
    w: configs.w.w,
    h: configs.w.h,
    validate: function () {
        frmSave.$ClearControls();
        saveSort.value = 0;
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
    title: configs.text.edit,
    w: configs.w.w,
    h: configs.w.h,
    validate: function () {
        var gridInfo = divGrid.$GridInfo();
        if (gridInfo.length == 0) {
            "请选择记录".$Alert();
            return false;
        } else {
            gridInfo = gridInfo[0];
            frmSave.$FillControls(gridInfo);
            saveProjectNamesName.value = saveProjectNames.value;
            saveProjectNames.value="noupdate"
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

btnProject.onclick = function ()
{
    var gridInfo = divGrid.$GridInfo();
    if (gridInfo.length == 1)
    {
        window.location.href = "/all?id=" + gridInfo[0].Id + "&name=" + encodeURI(gridInfo[0].Name);
    }
    else if (gridInfo.length == 0)
    {
        "请选择一条数据".$Alert();
    }
    else {
        "只能选择一个方案组".$Alert();
    }
}
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
