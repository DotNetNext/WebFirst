var configs = {
    url: {
        Get: _root + "database/getdatabase",
        Del: _root + "database/deletedatabase",
        SaveSystem: _root + "database/savedatabase",
        GetDbType: _root+"system/getdbtype"
    },
    text:
    {
        add: "添加数据库连接",
        edit:"修改数据库连接"
    },
    w: {
        w: 600,
        h:300
    }
};
divFrom.$Form({
    before: function () {
        btnSearch.$Loading();
    },
    url: configs.url.Get,
    callback: function (msg) {
        msg.Data.Dblfunc = function () {
            btnEdit.click();
        };
        divGrid.$Grid(msg.Data);
        btnSearch.$CloseLoading();
    }
})
btnSearch.$Button({
    before: function () {
        btnSearch.$Loading();
    },
    url: configs.url.Get,
    callback: function (msg) {
        msg.Data.Dblfunc = function () {
            btnEdit.click();
        };
        divGrid.$Grid(msg.Data);
        btnSearch.$CloseLoading();
    }
});


saveDbTypeName.$SelectTree({
    isMultiple: false,
    url: configs.url.GetDbType,
    maxHeight: 180,
    rootIsSelect: false
})

btnReset.$Reset();


btnAdd.$Open("#divOpen", {
    title: configs.text.add,
    w: configs.w.w,
    h: configs.w.h,
    validate: function () {
        frmSave.$ClearControls();
        return true;
    },
    yes: function () {
        btnAdd.$Loading();
        frmSave.$Form({
            url: configs.url.SaveSystem,
            callback: function (msg) {
                btnAdd.$CloseLoading();
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
            saveDbTypeName.value = saveDbType.value;
            return true;
        }

    },
    yes: function () {
        btnEdit.$Loading();
        frmSave.$Form({
            url: configs.url.SaveSystem,
            callback: function (msg) {
                btnEdit.$CloseLoading();
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
                    else
                    {
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


saveDbTypeName.change = function () {

    if (saveDbTypeName.value == "SqlServer")
    {
        saveConnection.value = "server=.;uid=sa;pwd=sasa;database=SQLSUGAR4XTEST";
    }
    if (saveDbTypeName.value == "MySql")
    {
        saveConnection.value = "server=localhost;Database=SqlSugar4xTest;Uid=root;Pwd=haosql";
    }
    if (saveDbTypeName.value == "Sqlite")
    {
        saveConnection.value = "DataSource=C:\\Demo\\SqlSugar4xTest.sqlite";
    }
    if (saveDbTypeName.value == "Oracle") {
        saveConnection.value = "Data Source=localhost/orcl;User ID=system;Password=haha;";
    }
    if (saveDbTypeName.value == "PostgreSQL") {
        saveConnection.value = "PORT=5432;DATABASE=SqlSugar4xTest;HOST=localhost;PASSWORD=haosql;USER ID=postgres";
    }
    if (saveDbTypeName.value == "Dm") {
        saveConnection.value = " Server=localhost; User Id=SYSDBA; PWD=SYSDBA;DATABASE=新DB";
    }
    if (saveDbTypeName.value == "Kdbndp") {
        saveConnection.value = " Server=127.0.0.1;Port=54321;UID=SYSTEM;PWD=system;database=SQLSUGAR4XTEST1";
    }
}