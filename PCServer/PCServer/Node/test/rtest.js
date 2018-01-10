 var oracledb = require('oracledb');

 var oracleConfig = {
    user:'ja_qbk',
    password:'ja_qbk',
    connectString:'38.104.30.22:1521/orcl'
};

function doRelease(connection) {
    connection.close(function(err) {
        if (err)
            console.log(err.message);
    });
}

// RunCB(message)
// sqlArgument: []
var RunOracleExec = function (RunCB) {

    oracledb.getConnection(oracleConfig,

        function (err, connection) {
            if (err) {
                console.error(err.message);
                RunCB("error: " + err);
                return;
            }
            RunCB("succ:链接成功 ");
            doRelease(connection);
            // connection.execute(sqlCommand, sqlArgument,

            //     function (err, result) {

            //         if (err) {
            //             RunCB("error: " + err);
            //             doRelease(connection);
            //             return;
            //         }

            //         var jstr = JSON.stringify(result.rows);
            //         RunCB(jstr);
            //         doRelease(connection);
            //     }
            // )

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

     RunOracleExec(RunCB);

}