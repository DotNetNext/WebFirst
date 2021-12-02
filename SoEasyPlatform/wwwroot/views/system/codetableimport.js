var configs = {
    url: {
        Get: _root + "DbTable/GetTableList?dbid=" + "dbId".$QueryString(),
    }
};
btnSearch.$Button({
    url: configs.url.Get,
    callback: function (msg) {
        msg.Data.Dblfunc = function () {
            //btnEdit.click();
        };
        divGrid.$Grid(msg.Data);
    }
});
 
divFrom.$Form({
    url: configs.url.Get,
    callback: function (msg) {
        msg.Data.Dblfunc = function () {
            //btnEdit.click();
        };
        divGrid.$Grid(msg.Data);
    }
})

document.onkeydown = function (e) {
    var ev = (typeof event != 'undefined') ? window.event : e;
    if (ev.keyCode == 13) {
        btnSearch.click();
        return false;
    }
}

function GetData()
{
    var gridInfo = divGrid.$GridInfo();
    return gridInfo;
}