
var configs = {
    url: {
        BuilderProjects: _root + "projectgroup/BuilderProjects",
        BuilderProjectsByHttp: _root + "projectgroup/BuilderProjectsByHttp"
    },
    newWin: function newWin(url, id) {

        var a = document.createElement('a');
        a.setAttribute('href', url);
        a.setAttribute('target', '_blank');
        a.setAttribute('id', id);
        // 防止反复添加
        if (!document.getElementById(id)) document.body.appendChild(a);
        a.click();
    }
};
btnStudent.onclick = function () {
    "用该功能之前需要先配置实体（菜单3种方式）".$Alert();
}
btnBack.onclick = function () {
    window.location.href = "/Solution";
}
btnProjectGroup.onclick = function () {
    var gridInfo = divGrid.$GridInfo();
    if (gridInfo.length > 0) {
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
            data: { "model": JSON.stringify(gridInfo), pgid: hidProjectGroupid.value, dbid: txtDbId.value }
        })
    } else {
        "请选择一条数据".$Alert();
    }
}

btnProjectGroupHttp.onclick = function () {
    var gridInfo = divGrid.$GridInfo();
    if (gridInfo.length > 0) {
        btnProjectGroup.$Loading();
        configs.url.BuilderProjectsByHttp.$Ajax({
            callback: function (msg) {
                if (msg.IsSuccess) {

                    configs.newWin("/" + msg.Data, "a_openurl");
                    "生成成功".$Alert();
                    btnSearch.click();
                }
                else {
                    msg.Data.$Alert();
                }
                btnProjectGroup.$CloseLoading();
            },
            data: { "model": JSON.stringify(gridInfo), pgid: hidProjectGroupid.value, dbid: txtDbId.value }
        })
    } else {
        "请选择一条数据".$Alert();
    }
}
 