export default class TimeUtils {
    
    static formatDay = ( time: any ) => {
        // 格式化日期，获取今天的日期
        const Dates = new Date( time );
        const year: number = Dates.getFullYear();
        const month: any = ( Dates.getMonth() + 1 ) < 10 ? '0' + ( Dates.getMonth() + 1 ) : ( Dates.getMonth() + 1 );
        const day: any = Dates.getDate() < 10 ? '0' + Dates.getDate() : Dates.getDate();
        return year + '-' + month + '-' + day;
      };


    static formatDate = ( time: any ) => {
        // 格式化日期，获取今天的日期
        const Dates = new Date( time );
        const year: number = Dates.getFullYear();
        const month: any = ( Dates.getMonth() + 1 ) < 10 ? '0' + ( Dates.getMonth() + 1 ) : ( Dates.getMonth() + 1 );
        const day: any = Dates.getDate() < 10 ? '0' + Dates.getDate() : Dates.getDate();
        const hour: any =  Dates.getHours() < 10 ? '0' + Dates.getHours() : Dates.getHours();
        const minute:any =  Dates.getMinutes() < 10 ? '0' + Dates.getMinutes() : Dates.getMinutes();
        const second:any = Dates.getSeconds() < 10 ? '0' + Dates.getSeconds() : Dates.getSeconds();
        return year + '-' + month + '-' + day +' ' + hour + ':' + minute + ':' + second;
      };
  
    public static GetTimeNow(){
        var date = new Date();
        return this.formatDate( date.getTime() );
    }


    public static GetDayNow() {
        var date = new Date();
        return this.formatDay( date.getTime() );
    }
}