var oracledb = require('oracledb');
var mysql = require('mysql');


//0:JJD_ID
//17:AF_ADDR
//4:YEAR
//5:MONTH
//6:DAY
//7:HH
//8:MM
//9:SS
//14:phone
//22:BJAY1
//23:BJAY2
//24:BJAY3
//25:BJAY4
//26:BJAY5
//27:FKAY1
//28:FKAY2
//29:FKAY3
//30:FKAY4
//31:FKAY5
//15:QY
//75:KEY_AREAS
//39:ROAD
//12:JJY_NAME
//11:JJY_ID
//16:CJDW
//104:CJY_NAME
//103:CJY_ID
//18:COMMET
//100:AMAP_GPS_X
//101:AMAP_GPS_Y


// var mySqlConfig={
//     host:'127.0.0.1',
//     user:'root',
//     password:'P@ssw0rd',
//     port:'3306',
//     database:'securitycitydb'
// };
var oracleConfig = {
    user: 'ja110_share',
    password: 'ja110_share',
    connectString: '10.17.56.128:1521/ja110'
};


function doRelease(connection) {
    connection.close(function(err) {
        if (err)
            console.log(err.message);
    });
}


var sqlconnection = mysql.createConnection({
    host: '127.0.0.1',
    user: 'strike2014',
    password: 'strike@2014',
    port: '3306',
    database: 'securitycitydb'
});

sqlconnection.connect(function(err) {
    if (err) {
        console.log(err);
        return;
    }
});

var RunMysqlExec = function(sqlCommand) {

    sqlconnection.query(sqlCommand, function(err, result) {
        if (err)
            console.log(err.message);
    });

}

oracledb.maxRows = 100000;

var RunOracleExec = function(RunCB) {

    oracledb.getConnection(
        oracleConfig,
        function(err, connection) {
            if (err) {
                console.log(err.message);

                RunCB("error:" + err);
                return;
            }
            var myDate = new Date();
            var myLocalDate = new Date().toLocaleDateString().split("-");
            var year = myLocalDate[0];
            var month = myLocalDate[1];

            if (parseInt(month) < 10) {
                month = "0" + month;
            }

            var day = myLocalDate[2];

            if (parseInt(day) < 10) {
                day = "0" + day;
            }

            // var hour = myDate.getHours();
            // var minute = myDate.getMinutes();

            // var oldHour = hour - 1;
            // //var oldMinute = minute-1;
            // var oldMinute = 0;
            // var old60min = 0;


            var sqlCommand = "";
            // if (minute < 1) {
            //     sqlCommand = "SELECT * FROM RPT.PD_POLICE_ALL_DATA where YEAR=\'" + year + "\'" +
            //         " and MONTH =\'" + month + "\'" +
            //         " and DAY =\'" + day + "\'" +
            //         " and ((HH =" + hour.toString() +
            //         " and MM<=" + minute.toString() + ")" +
            //         " or(HH =" + oldHour.toString() + "and mm>" + old60min.toString() + "))";
            // } else {
            //     sqlCommand = "SELECT * FROM RPT.PD_POLICE_ALL_DATA where YEAR=\'" + year + "\'" +
            //         " and MONTH =\'" + month + "\'" +
            //         " and DAY =\'" + day + "\'" +
            //         " and HH =" + hour.toString() +
            //         " and MM>" + oldMinute.toString() + " and MM<=" + minute.toString();
            // }

            sqlCommand = "SELECT * FROM RPT.PD_POLICE_ALL_DATA where YEAR=\'" + year + "\'" +
                " and MONTH =\'" + month + "\'" +
                " and DAY =\'" + day + "\'";


            //insert into sys_110warningdb set `JJD_ID`='1003',`TIMESIGN` = 1001,`YEAR` = "2017" on DUPLICATE key update   
            //`YEAR` = "201777", `SS` = IF(`TIMESIGN` > 500, "aa", "bbb"), `QY` = IF(ISNULL(ROAD), "空", "不空");
            /*
            insert into sys_110warningdb set `JJD_ID`='1005',`TIMESIGN` = 1001,`YEAR` = "2017" on DUPLICATE
            key update  `QY` = IF( MM!='' AND ROAD='', "不空MM", "空MM");
            /*
                insert into sys_110warningdb set `JJD_ID`='1006',`TIMESIGN` = 1001,`YEAR` = "2017" on DUPLICATE
            key update  `QY` = IF( STEP1 + 0 != 0 AND ROAD!='', "111", "2222");
            */
            /*
            on DUPLICATE
            key update  `QY` = IF( STEP1 + 0 != 0 AND  STRCMP("ppp","ppp2") = 0, "111", "2222");
            key update  `QY` = IF( STEP1 + 0 != 0 AND  (STRCMP(NULL,'') = 0 OR ISNULL(NULL)) , "uuuu", "llll");
            */
            /*
                1、不为空
                Select   *   From   table Where id<>''
                Select   *   From   table Where id!=''
                2、为空
                Select   *   From   table Where id=''
                Select   *   From   table Where   ISNULL(id)
                具体情况具体分析，如果字段是char和varchar型用 id=''可以；如果是int型用 ISNULL好些
            */

            connection.execute(
                sqlCommand,
                function(err, result) {
                    if (err) {
                        console.log(err.message);

                        RunCB("oracle execute error:" + err);
                        return;
                    }

                    // console.log(sqlCommand);
                    for (var i = 0; i < result.rows.length; i++) {
                        var timesign = parseInt(Date.now() / 1000);

                        //console.log(date+" "+timesign);

                        var tesselect = 'insert into sys_110warningdb set JJD_ID=' + "\'" + result.rows[i][0] + "\'" +
                            ',AF_ADDR=' + "\'" + result.rows[i][17] + "\'" +
                            ',YEAR=' + "\'" + result.rows[i][4] + "\'" +
                            ',MONTH=' + "\'" + result.rows[i][5] + "\'" +
                            ',DAY=' + "\'" + result.rows[i][6] + "\'" +
                            ',HH=' + "\'" + result.rows[i][7] + "\'" +
                            ',MM=' + "\'" + result.rows[i][8] + "\'" +
                            ',SS=' + "\'" + result.rows[i][9] + "\'" +
                            ',BJAY1=' + "\'" + result.rows[i][22] + "\'" +
                            ',BJAY2=' + "\'" + result.rows[i][23] + "\'" +
                            ',BJAY3=' + "\'" + result.rows[i][24] + "\'" +
                            ',BJAY4=' + "\'" + result.rows[i][25] + "\'" +
                            ',BJAY5=' + "\'" + result.rows[i][26] + "\'" +
                            ',FKAY1=' + "\'" + result.rows[i][27] + "\'" +
                            ',FKAY2=' + "\'" + result.rows[i][28] + "\'" +
                            ',FKAY3=' + "\'" + result.rows[i][29] + "\'" +
                            ',FKAY4=' + "\'" + result.rows[i][30] + "\'" +
                            ',FKAY5=' + "\'" + result.rows[i][31] + "\'" +
                            ',QY=' + "\'" + result.rows[i][15] + "\'" +
                            ',KYE_AREAS=' + "\'" + result.rows[i][75] + "\'" +
                            ',ROAD=' + "\'" + result.rows[i][39] + "\'" +
                            ',JJY_NAME=' + "\'" + result.rows[i][12] + "\'" +
                            ',JJY_ID=' + "\'" + result.rows[i][11] + "\'" +
                            ',CJDW=' + "\'" + result.rows[i][16] + "\'" +
                            ',CJY_NAME=' + "\'" + result.rows[i][104] + "\'" +
                            ',CJY_ID=' + "\'" + result.rows[i][103] + "\'" +
                            ',COMMET=' + "\'" + result.rows[i][18] + "\'" +
                            ',AMAP_GPS_X=' + "\'" + result.rows[i][100] + "\'" +
                            ',AMAP_GPS_Y=' + "\'" + result.rows[i][101] + "\'" +
                            ',BJ_PHONE=' + "\'" + result.rows[i][14] + "\'" +
                            ',TIMESIGN=' + timesign.toString()
                            + " on duplicate key update "
                            + ' YEAR=' + "\'" + result.rows[i][4] + "\'"
                            + ", STEP1 = IF(STEP1 + 0 = 0 AND STRCMP(\'" + result.rows[i][104] + "\',\'\') !=0 ," + timesign.toString() + ", VALUES(STEP1))" 
                            + ", STEP2 = IF(STEP2 + 0 = 0 AND STRCMP(\'" + result.rows[i][20] + "\',\'\') !=0 ," + timesign.toString() + ", VALUES(STEP2))" 
                            + ", STEP3 = IF(STEP3 + 0 = 0 AND STRCMP(\'" + result.rows[i][58] + "\',\'系统删除\') ==0 ," + timesign.toString() + ", VALUES(STEP3))" 
                            ;

                       // `QY` = IF( STEP1 + 0 != 0 AND  STRCMP("ppp","ppp2") = 0, "111", "2222");
                       // `QY` = IF( MM!='' AND ROAD='', "不空MM", "空MM");
                       //`QY` = IF( STEP1 + 0 != 0 AND ROAD!='', "111", "2222");
                       // `QY` = IF( STEP1 + 0 != 0 AND  (STRCMP(NULL,'') = 0 OR ISNULL(NULL)) , "uuuu", "llll");
                       // console.log(tesselect);
                        RunMysqlExec(tesselect);

                    }

                    doRelease(connection);

                    RunCB("ok");
                }
            );
        }
    );

}


module.exports = function(callback) {

    var RunCB = function(message) {
        callback(null, message);
    }

    //setInterval(function() {
    RunOracleExec(RunCB);
    //}, 1 * 60 * 1000);

}