var configs = {
    url: {
        Get: _root + "DbTable/GetTableList?dbid=" + "dbId".$QueryString(),
    }
};

configs.url.Get.$Ajax({
    callback: function (msg) {
        msg.Data.Dblfunc = function () {
            btnEdit.click();
        };
        divGrid.$Grid(msg.Data);
    }
})