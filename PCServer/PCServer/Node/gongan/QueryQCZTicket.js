var oracledb = require('oracledb');
var mysql = require('mysql');


var oracleConfig = {
    user:'ja_qbk',
    password:'ja_qbk',
    connectString:'38.104.30.22:1521/orcl'
};

function doRelease(connection)
{
    connection.close(function (err) {
        if(err)
            console.log(err.message);
    });
}

oracledb.maxRows=1000000;

module.exports=function(callback, uuid) {
    oracledb.getConnection(
        oracleConfig,
        function (err,connection) {
            if(err){
                console.log(err.message);
                return;
            }

            sqlCommand = "select * from jabd.t_ticket_info where UUID = '" + uuid + "'";

            connection.execute(
                sqlCommand,
                function (err,result) {
                    if(err){
                        console.log(err.message);
                        return;
                    }

                    //console.log(sqlCommand);
                    //console.log(result.rows.length);
                    var jstr = JSON.stringify(result.rows);

                    doRelease(connection);
                    callback(null, jstr);
                }
            );
        }
    );
}

