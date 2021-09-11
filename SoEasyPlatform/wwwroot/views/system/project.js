var configs = {
    url: {
        Get: _root + "Project/getProjectlist",
        Del: _root + "Project/deleteProject",
        SaveSystem: _root + "Project/saveProject",
        GetType: _root + "system/GetTemplateType"
    },
    text:
    {
        add: "添加方案",
        edit: "修改方案"
    },
    w: {
        w: 600,
        h:450
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

 

txtModelIdName.$SelectTree({
    isMultiple: false,
    url: configs.url.GetType,
    maxHeight: 180,
 
    rootIsSelect: true
})

btnReset.$Reset();



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


