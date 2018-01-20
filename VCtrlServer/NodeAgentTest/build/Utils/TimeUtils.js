"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class TimeUtils {
    static GetTimeNow() {
        var date = new Date();
        return this.formatDate(date.getTime());
    }
    static GetDayNow() {
        var date = new Date();
        return this.formatDay(date.getTime());
    }
}
TimeUtils.formatDay = (time) => {
    // 格式化日期，获取今天的日期
    const Dates = new Date(time);
    const year = Dates.getFullYear();
    const month = (Dates.getMonth() + 1) < 10 ? '0' + (Dates.getMonth() + 1) : (Dates.getMonth() + 1);
    const day = Dates.getDate() < 10 ? '0' + Dates.getDate() : Dates.getDate();
    return year + '-' + month + '-' + day;
};
TimeUtils.formatDate = (time) => {
    // 格式化日期，获取今天的日期
    const Dates = new Date(time);
    const year = Dates.getFullYear();
    const month = (Dates.getMonth() + 1) < 10 ? '0' + (Dates.getMonth() + 1) : (Dates.getMonth() + 1);
    const day = Dates.getDate() < 10 ? '0' + Dates.getDate() : Dates.getDate();
    const hour = Dates.getHours() < 10 ? '0' + Dates.getHours() : Dates.getHours();
    const minute = Dates.getMinutes() < 10 ? '0' + Dates.getMinutes() : Dates.getMinutes();
    const second = Dates.getSeconds() < 10 ? '0' + Dates.getSeconds() : Dates.getSeconds();
    return year + '-' + month + '-' + day + ' ' + hour + ':' + minute + ':' + second;
};
exports.default = TimeUtils;
