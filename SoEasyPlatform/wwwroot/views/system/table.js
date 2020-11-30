var configs = {
    url: {
        Get: _root + "table/gettable",
        GetDbType: _root + "table/getdbtype",
        GetDatabase: _root + "system/getdatabase"
    },
    text:
    {
        add: "添加数据库连接",
        edit: "修改数据库连接"
    },
    w: {
        w: 600,
        h: 300
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

btnReset.$Reset();

 


