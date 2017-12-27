var mysql = require('mysql');

var mySqlConfig={
    host:'127.0.0.1',
    user:'strike2014',
    password:'strike@2014',
    port:'3306',
    database:'securitycitydb'
};

var sqlconnection = mysql.createConnection(mySqlConfig);

sqlconnection.connect(function (err) {
    if (err) {
        console.log(err);
        return;
    }
});


var RunMysqlExec = function (RunCB, sqlCommand) {
    sqlconnection.query(sqlCommand, function (err, result) {
        if (err) {
            console.log(err.message);
            RunCB("error:" + err);
        }

        RunCB("ok");
    });
}


module.exports = function (callback) {

    var RunCB = function (message) {
        console.log(message);
        callback(
            null,
            message
        );
    }

    var message = 'RES: ' + new Date();

    var sqlcmd = 'insert into db_jjds set af_addr = "' + message + "\"";
    RunMysqlExec(RunCB, sqlcmd);
}