var obj = {
    getNowFormatDate: function () {
        var date = new Date();
        var seperator = "-";
        var month = date.getMonth() + 1;
        var strDate = date.getDate();
        if (month >= 1 && month <= 9) {
            month = "0" + month;
        }
        if (strDate >= 0 && strDate <= 9) {
            strDate = "0" + strDate;
        }
        var currentdate = date.getFullYear() + seperator + month + seperator + strDate;
        return currentdate;
    },
    getNowFormatTime: function () {
        var date = new Date();
        var seperator = ":";
        var hour=date.getHours();
        var minute=date.getMinutes();
        var second=date.getSeconds();
        if (hour >= 0 && hour <= 9) {
            hour = "0" + hour;
        }
        if (minute >= 0 && minute <= 9) {
            minute = "0" + minute;
        }
        if (second >= 0 && second <= 9) {
            second = "0" + second;
        }
        var currenttime = hour + seperator + minute + seperator + second;
        return currenttime;
    }
}
module.exports = obj;