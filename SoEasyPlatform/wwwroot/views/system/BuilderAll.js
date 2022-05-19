
var configs = {
    url: {
        BuilderProjects: _root + "projectgroup/BuilderProjects"
    }
};
btnStudent.onclick = function () {
    "用该功能之前需要先配置实体（菜单3种方式）".$Alert();
}
btnBack.onclick = function () {
    window.location.href =  "/Solution";
}
btnProjectGroup.onclick = function ()
{
    var gridInfo = divGrid.$GridInfo();
    if (gridInfo.length >0) {
        btnProjectGroup.$Loading();
        configs.url.BuilderProjects.$Ajax({
            callback: function (msg) {
                if (msg.IsSuccess) {
                    "生成成功".$Alert();
                    btnSearch.click();
                }
                else {
                    msg.Data.$Alert();
                }
                btnProjectGroup.$CloseLoading();
            },
            data: { "model": JSON.stringify(gridInfo), pgid: hidProjectGroupid.value, dbid:txtDbId.value}
        })
    } else {
        "请选择一条数据".$Alert();
    }
}