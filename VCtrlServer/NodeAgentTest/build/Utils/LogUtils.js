"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const TimeUtils_1 = require("./TimeUtils");
//https://www.npmjs.com/package/log
var NowDay = TimeUtils_1.default.GetDayNow();
var fs = require('fs'), Log = require('log'), log_system = new Log('system', fs.createWriteStream('system_' + NowDay + ".log"));
class LogUtils {
    static LogInfo_System(msg) {
        this.CheckDay();
        log_system.info(msg);
    }
    static LogError_System(msg) {
        this.CheckDay();
        log_system.error(msg);
    }
    static LogDebug_System(msg) {
        this.CheckDay();
        log_system.debug(msg);
    }
    static CheckDay() {
        var now = TimeUtils_1.default.GetDayNow();
        if (now != NowDay) {
            NowDay = now;
            log_system = new Log('system', fs.createWriteStream('system_' + NowDay + ".log"));
        }
    }
}
exports.default = LogUtils;
