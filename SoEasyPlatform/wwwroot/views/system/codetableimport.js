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

function GetData()
{
    var gridInfo = divGrid.$GridInfo();
    return gridInfo;
}