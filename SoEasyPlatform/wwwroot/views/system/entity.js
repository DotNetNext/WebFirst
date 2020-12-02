var configs = {
    url: {
        Get: _root + "entity/gettable",
        GetDbType: _root + "entity/getdbtype",
        GetDatabase: _root + "system/getdatabase",
        GetTemp: _root + "system/getTemplate?type=1",
        CreateFile: _root + "entity/createfile",
    },
    text:
    {
        Cpr: "生成实体到项目",
        Cpa: "生成实体到指定目录"
    },
    w: {
        w: 850,
        h: 400
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


DatabaseName.$SelectTree({
    isMultiple: false,
    url: configs.url.GetDatabase,
    maxHeight: 180,
    rootIsSelect: false
})

saveTemplateName1.$SelectTree({
    isMultiple: false,
    url: configs.url.GetTemp,
    maxHeight: 180,
    rootIsSelect: false
})

btnReset.$Reset();


btnPath.$Open("#divPathOpen", {
    title: configs.text.Cpa,
    w: configs.w.w,
    h: configs.w.h,
    validate: function () {
        var gridInfo = divGrid.$GridInfo();
 
        saveFileName.value = "String.Format(\"{0}{1}Entity\" ,{表名}.Substring(0,1).ToUpper() , {表名}.Substring(1))";
        if (gridInfo.length == 0) {
            "请选择记录".$Alert();
            return false;
        } else {
            gridInfo = gridInfo[0];
            //frmSave.$FillControls(gridInfo);
            return true;
        }
    },
    yes: function () {
 
        SaveTable1.value = JSON.stringify(divGrid.$GridInfo());
        btnPath.$Loading();
        frmPathSave.$Form({
            url: configs.url.CreateFile + "?databaseId=" + Database.value,
            callback: function (msg) {
                if (msg.IsKeyValuePair) {
                    $sugar.$Validate(msg.Data, "save");
                } else {
                    $sugar.$Validate("clear");
                    if (msg.IsSuccess) {
                        "生成成功".$Alert();
                        btnSearch.click();
                        //$sugar.$CloseAll(divOpen.getAttribute("divPathOpen"));
                    } else
                    {
                        msg.Message.$Alert();
                    }
                }
                debugger
                btnPath.$CloseLoading();
             
            }
        });
    },
    btn: ['生成实体', '关闭']
});

btnProject.$Open("#divProjectOpen", {
    title: configs.text.Cpr,
    w: configs.w.w,
    h: configs.w.h,
    validate: function () {
        var gridInfo = divGrid.$GridInfo();
        if (gridInfo.length == 0) {
            "请选择记录".$Alert();
            return false;
        } else {
            gridInfo = gridInfo[0];
            //frmSave.$FillControls(gridInfo);
            return true;
        }
    },
    yes: function () {
        
        SaveTable2.value = JSON.stringify(divGrid.$GridInfo());
        frmProjectSave.$Form({
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

 
