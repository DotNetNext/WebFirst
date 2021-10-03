var curls = {
    save: _root + "codetable/SaveCommField",
    getfields: _root + "system/GetCommonFiled"
}
saveReferenceName.$SelectTree({
    isMultiple: true,
    url: curls.getfields,
    maxHeight: 100,
    rootIsSelect: false
})
 
btnCommonFiled.$Open("#divCommonFiled", {
    title: "追加公共字段",
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
            frmCommField.$Form({
                url: curls.save,
                callback: function (msg) {
                    btnProject.$CloseLoading();
                    if (msg.IsSuccess) {
                        $sugar.$CloseAll(divCommonFiled.getAttribute("dataindex"));
                        btnCopyHide.click();
                    }
                    else {
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