btnEdit2.onclick = function ()
{
    btnEdit.click();
}
btnDel2.onclick = function () {
    btnDel.click();
}
btnAddView.$Open("#divView", {
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
                    else {
                        msg.Data.$Alert();
                    }
                }
            });
        } else {
            "请选择一条数据".$Alert();
        }
    },
    btn: ['创建类', '关闭']
});