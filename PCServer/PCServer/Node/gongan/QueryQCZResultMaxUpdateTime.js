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


module.exports = function(callback) {
    oracledb.getConnection(
        oracleConfig,
        function (err,connection) {
            if(err){
                console.log(err.message);

		        callback(null,'err: oracle conn err :' + err);
                return;
            }

            sqlCommand ="select max(UPDATETIME) from jabd.t_result_info";

            connection.execute(
                sqlCommand,
                function (err,result) {
                    if(err){
                        console.log(err.message);
			            callback(null,'err: oracle query :' + sqlCommand + "  =¡· ERROR STRING: " + err);
                        return;
                    }

                    doRelease(connection);

                    callback(null, result.rows[0][0] );
                }
            );
        }
    );
}


