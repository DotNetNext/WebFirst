var configs = {
    url: {
        Get: _root + "codetable/getcodetablelist",
        getbyid: _root +"Project/GetProjectById",
        Del: _root + "codetable/deleteCodetable",
        GetDatabase: _root + "system/getdatabase",
        Info: "/CodeTableInfo",
        Save: _root + "codetable/savecodetable",
        SaveImport: _root + "codetable/savecodetableimport",
        Import: "/CodeTableImport",
        GetTemp: _root + "system/getTemplate?type=" + hidType.value,
        GetProjet: _root + "system/GetProject?typeId=" + hidType.value, 
        GetFileInfo: _root + "system/getFileInfo",
        CreateFile: _root + "codetable/createfile",
        CreateFile2: _root + "codetable/CreateFileByProjectId",
        GetRazorModel: _root + "FileInfo/GetRazorModel",
        UpdateEntity: _root + "codetable/UpdateEntity",
        CreateTable: _root + "codetable/CreateTables",
        GetProjectAll: _root + "system/GetProjectAll",
        Copy: _root + "codetable/Copy",
        Export: _root + "codetable/exportfile",
        Tag: "/TagPropertyInfo",
        SaveTagProperty: _root +"codetable/SaveTagProperty"
    },
    text:
    {
        add: "创建虚拟类",
        addPath: "添加方案",
        EditPath: "修改方案",
        addProject: "方案生成",
        addDbFirst: "导入虚拟类",
        edit: "修改虚拟类",
        copy: "预览复制",
        tag:"配置附加属性"
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
    saveDbId.value = txtDbId.value;
    //var tablebody = document.getElementsByClassName("fixed-table-body");
    //if (tablebody.length>0)
    // tablebody[0].$AddCss({ height:200})
}

btnReset.$Reset();

saveTemplateName1.$SelectTree({
    isMultiple: false,
    url: configs.url.GetTemp,
    maxHeight: 180,
    rootIsSelect: false
})

 

saveProjectName.$SelectTree({
    isMultiple: false,
    url: configs.url.GetProjet,
    maxHeight: 100,
    rootIsSelect: false
})

saveReferenceName.$SelectTree({
    isMultiple: true,
    url: configs.url.GetProjectAll,
    maxHeight: 100,
    rootIsSelect: false
})

saveFileInfoName.$SelectTree({
    isMultiple: true,
    url: configs.url.GetFileInfo,
    maxHeight: 180,
    rootIsSelect: false
})

saveFileInfoName.onchange = function () {
    var value = saveFileInfo.value;
    if (value != null && value != "") {
        var ajaxParam = {
            data: { id: value },
            callback: function (msg) {
                if (msg.IsSuccess)
                {
                    saveFileModel.value = msg.Data;
                }
                else {
                    msg.Data.$Alert();
                }
            }
        };
        configs.url.GetRazorModel.$Ajax(ajaxParam);
    }
    else
    {
        saveFileModel.value = "[{ \"name\":\"命名空间\"}]";
    }
 } 
 

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
                    "保存成功".$Alert();
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
//打开导入
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
        btnDbFirstAdd.$Loading();
        var data = document.getElementsByTagName("iframe")[0].contentWindow.GetData();
        configs.url.SaveImport.$Ajax({
            callback: function (msg) {
                btnDbFirstAdd.$CloseLoading();
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

btnTableToEntity.onclick = function () {

    btnTableToEntity.$Loading();
    var gridInfo = divGrid.$GridInfo();
    if (gridInfo.length > 0) {
        configs.url.UpdateEntity.$Ajax({
            callback: function (msg) {
                btnTableToEntity.$CloseLoading();
                if (msg.IsSuccess) {
                    "更新成功".$Alert();
                    btnSearch.click();
                }
                else {
                    msg.Data.$Alert();
                }
            },
            data: { "model": JSON.stringify(gridInfo), dbid: txtDbId.value}
        })
    } else {
        "请选择一条数据".$Alert();
        btnTableToEntity.$CloseLoading();
    }
} 

btnTable.onclick = function () {

    var gridInfo = divGrid.$GridInfo();
    if (gridInfo.length > 0) {
        btnTable.$Loading();
        configs.url.CreateTable.$Ajax({
            callback: function (msg) {
                btnTable.$CloseLoading();
                if (msg.IsSuccess) {
                    "更新成功".$Alert();
                    btnSearch.click();
                }
                else {
                    msg.Data.$Alert();
                }
            },
            data: { "model": JSON.stringify(gridInfo), dbid: txtDbId.value }
        })
    } else {
        "请选择一条数据".$Alert();
        btnTable.$CloseLoading();
    }

}

btnPath.$Open("#divPath", {
    title: configs.text.addPath,
    w: 600,
    h: 600,
    validate: function () {
        var gridInfo = divGrid.$GridInfo();
        if (txtDbId.value == null || txtDbId.value == "" || txtDbId.value == "0") {
            "请选择数据库".$Alert();
            return false;
        } else if (gridInfo.length == 0) {
            "请选择记录".$Alert();
            return false;
        } else {
            saveProjentName.value = "";
            saveTemplateId1.value = "";
            saveTemplateName1.value = "";
            saveFileModel.value = "[{ \"name\":\"命名空间\"}]";
            SaveId.value = "";
            saveFileInfoName.value = "";
            saveFileInfo.value = "";
            saveReferenceName.value = "";
            saveReference.value = "";
            return true;
        }
    },
    yes: function () {
        var gridInfo = divGrid.$GridInfo();
        if (gridInfo.length > 0) {
            SaveTable1.value = JSON.stringify(gridInfo);
            btnPath.$Loading();
            frmPathSave.$Form({
                url: configs.url.CreateFile,
                callback: function (msg) {
                    btnPath.$CloseLoading();
                    if (msg.IsKeyValuePair) {
                        $sugar.$Validate(msg.Data, "save");
                    } else {
                        saveProjectName.$RestSelectTree({ url: configs.url.GetProjet })
                        $sugar.$Validate("clear");
                        msg.Message.$Alert();
                        if (msg.IsSuccess)
                        {
                            $sugar.$CloseAll(divPath.getAttribute("dataindex"));
                        }
                    }
                }
            });
        } else {
            "请选择一条数据".$Alert();
        }
    },
    btn: ['创建方案', '关闭']
});

btnPathEdit.$Open("#divPath", {
    title: configs.text.EditPath,
    w: 600,
    h: 600,
    validate: function () {
        var gridInfo = divGrid.$GridInfo();
        if (txtDbId.value == null || txtDbId.value == "" || txtDbId.value == "0") {
            "请选择数据库".$Alert();
            return false;
        } else if (gridInfo.length == 0) {
            "请选择记录".$Alert();
            return false;
        } else if (saveProjectName.value == "") {
            "请选择方案".$Alert();
            return false;
        } else {
            (configs.url.getbyid + "?id=" + saveProjectId.value).$Ajax({
                callback: function (msg) {
                    frmPathSave.$FillControls(msg.Data);
                    saveTemplateName1.$SelectTree("bind");
                    saveFileInfoName.$SelectTree("bind");
                    saveReferenceName.$SelectTree("bind");
                } 
            })
            return true;
        }
    },
    yes: function () {
        var gridInfo = divGrid.$GridInfo();
        if (gridInfo.length > 0) {
            SaveTable1.value = JSON.stringify(gridInfo);
            btnPath.$Loading();
            frmPathSave.$Form({
                url: configs.url.CreateFile,
                callback: function (msg) {
                    btnPath.$CloseLoading();
                    if (msg.IsKeyValuePair) {
                        $sugar.$Validate(msg.Data, "save");
                    } else {
                        saveProjectName.$RestSelectTree({ url: configs.url.GetProjet })
                        $sugar.$Validate("clear");
                        msg.Message.$Alert();
                        if (msg.IsSuccess) {
                            $sugar.$CloseAll(divPath.getAttribute("dataindex"));
                        }
                    }
                }
            });
        } else {
            "请选择一条数据".$Alert();
        }
    },
    btn: ['修改方案', '关闭']
});

btnProject.$Open("#divProject", {
    title: configs.text.addProject,
    w: 600,
    h: 300,
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
            SaveTable2.value = JSON.stringify(gridInfo);
            btnProject.$Loading();
            frmProjectSave.$Form({
                url: configs.url.CreateFile2,
                callback: function (msg) {
                    btnProject.$CloseLoading();
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
    btn: ['生成方案', '关闭']
});

btnCopy.$Open("#divProject", {
    title: configs.text.copy,
    w: 600,
    h: 300,
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
            SaveTable2.value = JSON.stringify(gridInfo);
            btnProject.$Loading();
            frmProjectSave.$Form({
                url: configs.url.Copy,
                callback: function (msg) {
                    btnProject.$CloseLoading();
                    if (msg.IsSuccess) {
                        $sugar.$CloseAll(divProject.getAttribute("dataindex"));
                        btnCopyHide.click();
                    }
                    else
                    {
                        msg.Data.$Alert();
                    }
                }
            });
        } else {
            "请选择一条数据".$Alert();
        }
    },
    btn: ['预览', '关闭']
});
 
btnCopyHide.$Open("#divCopy", {
    title: configs.text.copy,
    w: 1000,
    h: 600,
    validate: function () {
        var gridInfo = divGrid.$GridInfo();
        if (txtDbId.value == null || txtDbId.value == "" || txtDbId.value == "0") {
            "请选择数据库".$Alert();
            return false;
        } else if (gridInfo.length == 0) {
            "请选择记录".$Alert();
            return false;
        }
        else {
            btnCopy.$Loading();
            frmProjectSave.$Form({
                url: configs.url.Copy,
                callback: function (msg) {
                    btnCopy.$CloseLoading();
                    txtCopy.value = msg.Data;
                }
            });
            return true;
        }
    },
    yes: function () {
        "请手动复制".$Alert();
    },
    btn: ['预览', '关闭']
});

btnExcel.onclick = function () {
    var gridInfo = divGrid.$GridInfo();
    if (gridInfo.length > 0) {
        btnExcel.$Loading();
        configs.url.Export.$Ajax({
            callback: function (msg) {
                btnExcel.$CloseLoading();
                if (msg.IsSuccess) {
                    "已经保存到根目录Excel文件夹".$Alert();
                    btnSearch.click();
                }
                else {
                    msg.Data.$Alert();
                }
            },
            data: { "model": JSON.stringify(gridInfo), dbid: txtDbId.value }
        })
    } else {
        "请选择一条数据".$Alert();
        btnExcel.$CloseLoading();
    }
}

//配置附加属性
btnTagProperty.$Open("#divOpen", {
    validate: function () {
        if (txtDbId.value == null || txtDbId.value == "" || txtDbId.value == "0") {
            "请选择数据库".$Alert();
            return false;
        }
        else {
            var gridInfo = divGrid.$GridInfo();
            if (gridInfo.length == 0)
            {
                "请选择一条记录".$Alert();
            }
            return true;
        }

    },
    title: configs.text.tag,
    w: configs.w.w,
    h: configs.w.h,
    url: configs.url.Tag,
    format: function (msg) {
        msg.url = configs.url.Tag + "?tableid=" + divGrid.$GridInfo()[0].Id;
    },
    yes: function () {
        btnTagProperty.$Loading();
        var data = document.getElementsByTagName("iframe")[0].contentWindow.GetData();
        configs.url.SaveTagProperty.$Ajax({
            callback: function (msg) {
                btnTagProperty.$CloseLoading();
                if (msg.IsSuccess) {
                    "保存成功".$Alert();
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
    btn: ['保存', '关闭']
})