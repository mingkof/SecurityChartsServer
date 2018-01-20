var oracledb = require('oracledb');
var mysql = require('mysql');


var oracleConfig = {
    user:'ja_qbk',
    password:'ja_qbk',
    connectString:'10.17.32.94:1521/orcl'
};

function doRelease(connection)
{
    connection.close(function (err) {
        if(err)
            console.log(err.message);
    });
}

oracledb.maxRows=1000000;

module.exports = function(callback, arg) {
    oracledb.getConnection(
        oracleConfig,
        function (err,connection) {
            if(err){
                console.log(err.message);

		callback(null,'err: oracle conn err :' + err);
                return;
            }

            sqlCommand ="select * from jabd.t_result_info";

            connection.execute(
                sqlCommand,
                function (err,result) {
                    if(err){
                        console.log(err.message);
			callback(null,'err: oracle query :' + sqlCommand + "  =¡· ERROR STRING: " + err);
                        return;
                    }

                    //var _result = result.rows[0][0].toString();

                    //console.log(sqlCommand);
                    //console.log(result.rows[0][0]);


		    var jstr = JSON.stringify(result.rows);

                    callback(null,jstr );
 
                    doRelease(connection);
                }
            );
        }
    );
}


