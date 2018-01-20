import TimeUtils from './TimeUtils';

//https://www.npmjs.com/package/log

var NowDay = TimeUtils.GetDayNow();
var fs = require('fs'), Log = require('log'), log_system = new Log('system', fs.createWriteStream('system_' + NowDay + ".log"));


export default class LogUtils {
    
    public static LogInfo_System(msg:string) {
        this.CheckDay();
        log_system.info(msg);
    }

    public static LogError_System(msg:string) {
        this.CheckDay();
          log_system.error(msg);
    }
    public static LogDebug_System(msg:string) {
        this.CheckDay();
        log_system.debug(msg);
    }


    static CheckDay() {
        var now = TimeUtils.GetDayNow();
        if(now != NowDay) {
            NowDay = now;
            log_system = new Log('system', fs.createWriteStream('system_' + NowDay + ".log"));
        }
    }

}