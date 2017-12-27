var myDate = new Date();

var fs = require("fs");

var timeutil=require("./TimeUtil.js");

var obj = {
    Debug: function (txt) {
        var date = timeutil.getNowFormatDate();
        var time = timeutil.getNowFormatTime();
        var d=process.argv;
        fs.appendFile(date + ".log", date + " " + time + " Debug:" + txt + '\r\n', function (err) {
            if (err) throw err;
        });
    },
    Warn: function (txt) {
        var date = timeutil.getNowFormatDate();
        var time = timeutil.getNowFormatTime();

        fs.appendFile(date + ".log", date + " " + time + " Warn:" + txt + '\r\n', function (err) {
            if (err) throw err;
        });
    },
    Error: function (txt) {
        var date = timeutil.getNowFormatDate();
        var time = timeutil.getNowFormatTime();

        fs.appendFile(date + ".log", date + " " + time + " Error:" + txt + '\r\n', function (err) {
            if (err) throw err;
        });
    }
}

module.exports = obj;