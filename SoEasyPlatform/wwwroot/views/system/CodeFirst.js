var curls = {
    save: _root + "codetable/SaveCommField",
    getfields: _root + "system/GetCommonFiled"
}
saveFieldName.$SelectTree({
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
            cfmodel.value = JSON.stringify(gridInfo);
            btnProject.$Loading();
            frmCommField.$Form({
                url: curls.save,
                callback: function (msg) {
                    btnProject.$CloseLoading();
                    if (msg.IsSuccess) {
                        $sugar.$CloseAll(divCommonFiled.getAttribute("dataindex"));
                        msg.Data.$Alert();
                    }
                    else {
                        "追加失败，请选择字段".$Alert();
                    }
                }
            });
        } else {
            "请选择一条数据".$Alert();
        }
    },
    btn: ['追加', '关闭']
});