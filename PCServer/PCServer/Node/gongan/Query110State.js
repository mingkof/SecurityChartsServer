var oracledb = require('oracledb');

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

oracledb.maxRows = 1000000;

module.exports = function(callback, arg) {
    oracledb.getConnection(
        oracleConfig,
        function(err, connection) {
            if (err) {
                // console.log(err.message);
                callback(null, "Error:" + err);
                return;
            }

            //sqlCommand ="SELECT DISTINCT (O.PCS) AS NAME, NVL(CNT, 0) AS FEIZI, NVL(FZ, 0) AS FEIMU,((FN_DIVIDE(CNT, FZ, 3)) - 1) * 100 || '%' INCRESS FROM PD_POLICE_PCS O LEFT OUTER JOIN (SELECT CJDW, COUNT(*) AS CNT FROM PD_POLICE_ALL_DATA WHERE 1 = 1 AND YEAR = TO_CHAR(SYSDATE, 'yyyy') AND MONTH = TO_CHAR(SYSDATE, 'mm') AND DAY = TO_CHAR(SYSDATE, 'dd') AND HH BETWEEN '00' AND TO_CHAR(SYSDATE, 'HH24') AND DELETE_STATUS NOT IN ('分局人工(系统)','分局技防(系统)','外区(系统)','外区(审核)') AND XC_FLAG <> '1' AND BJAY1 = '报警类案件' GROUP BY CJDW) B ON B.CJDW = O.PCS LEFT OUTER JOIN (SELECT CJDW, SUM(FZ) AS FZ FROM PD_POLICE_BJAY_FZ WHERE 1 = 1 AND YEAR = TO_CHAR(ADD_MONTHS(SYSDATE, -12), 'yyyy') AND HH BETWEEN '00' AND TO_CHAR(SYSDATE, 'HH24') AND BJAY1 = '报警类案件' AND BJAY2 IS NULL GROUP BY CJDW) C ON C.CJDW = O.PCS WHERE O.QY = '静安'";

            sqlCommand = "SELECT DISTINCT (O.PCS) AS NAME, NVL(CNT, 0) AS FEIZI, NVL(FZ, 0) AS FEIMU, ((FN_DIVIDE(CNT, FZ, 3)) - 1) * 100 || '%' INCRESS, CFT FROM PD_POLICE_PCS O LEFT OUTER JOIN (SELECT CJDW, COUNT(*) AS CNT FROM PD_POLICE_ALL_DATA WHERE 1 = 1 AND YEAR = TO_CHAR(SYSDATE, 'yyyy') AND MONTH = TO_CHAR(SYSDATE, 'mm') AND DAY = TO_CHAR(SYSDATE, 'dd') AND HH BETWEEN '00' AND TO_CHAR(SYSDATE, 'HH24') AND DELETE_STATUS NOT IN ('分局人工(系统)','分局技防(系统)','外区(系统)','外区(审核)') AND XC_FLAG <> '1' AND BJAY1 = '报警类案件' GROUP BY CJDW) B ON B.CJDW = O.PCS LEFT OUTER JOIN (SELECT CJDW, SUM(FZ) AS FZ FROM PD_POLICE_BJAY_FZ WHERE 1 = 1 AND YEAR = TO_CHAR(ADD_MONTHS(SYSDATE, -12), 'yyyy') AND HH BETWEEN '00' AND TO_CHAR(SYSDATE, 'HH24') AND BJAY1 = '报警类案件' AND BJAY2 IS NULL GROUP BY CJDW) C ON C.CJDW = O.PCS LEFT OUTER JOIN (SELECT CJDW, COUNT(*) AS CFT FROM PD_POLICE_ALL_DATA WHERE 1 = 1 AND YEAR = TO_CHAR(SYSDATE, 'yyyy') AND MONTH = TO_CHAR(SYSDATE, 'mm') AND DAY = TO_CHAR(SYSDATE, 'dd') AND HH BETWEEN '00' AND TO_CHAR(SYSDATE, 'HH24') AND DELETE_STATUS NOT IN ('分局人工(系统)', '分局技防(系统)', '外区(系统)', '外区(审核)') AND XC_FLAG <> '1' GROUP BY CJDW) D ON D.CJDW = O.PCS WHERE O.QY = '静安'";

            connection.execute(
                sqlCommand,
                function(err, result) {
                    if (err) {
                        callback(null, "Error:" + err);
                        return;
                    }

                    //var _result = result.rows[0][0].toString();
                    //console.log(sqlCommand);
                    //console.log(result.rows[0][0]);

                    var jstr = JSON.stringify(result.rows);
                    doRelease(connection);
                    callback(null, jstr);
                }
            );
        }
    );
}