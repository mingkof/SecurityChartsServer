var oracledb = require('oracledb');

var oracleConfig = {
    user: 'system',
    password: '123456',
    connectString: '101.132.43.91:1521/XE'
};

function doRelease(connection) {
    connection.close(function(err) {
        if (err)
            console.log(err.message);
    });
}

// RunCB(message)
// sqlArgument: []
var RunOracleExec = function (RunCB, sqlCommand, sqlArgument) {

    oracledb.getConnection(oracleConfig,

        function (err, connection) {
            if (err) {
                console.error(err.message);
                RunCB("error: " + err);
                return;
            }

            connection.execute(sqlCommand, sqlArgument,

                function (err, result) {

                    if (err) {
                        RunCB("error: " + err);
                        doRelease(connection);
                        return;
                    }

                    var jstr = JSON.stringify(result.rows);
                    RunCB(jstr);
                    doRelease(connection);
                }
            )

            /*
             //var message = 'RES: ' + new Date();
            connection.execute(
                // The statement to execute
                "SELECT * from USERTEST where ID = 1",

                // The "bind value" 180 for the "bind variable" :id
                [],
                // Optional execute options argument, such as the query result format
                // or whether to get extra metadata
                // { outFormat: oracledb.OBJECT, extendedMetaData: true },

                // The callback function handles the SQL execution results
                function (err, result) {

                    if (err) {
                        console.error(err.message);
                        doRelease(connection);

                        RunCB(message);
                        return;
                    }


                    console.log(result.metaData); // [ { name: 'DEPARTMENT_ID' }, { name: 'DEPARTMENT_NAME' } ]
                    console.log(result.rows); // [ [ 180, 'Construction' ] ]

                    var jstr = JSON.stringify(result.rows);
                    message = jstr;

                    doRelease(connection);

                    RunCB(message);
                });
            */

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

    var sqlcmd = 'SELECT * from USERTEST where ID = :id';
    var sqlargs = [1];

    // setInterval(function () {

        RunOracleExec(RunCB, sqlcmd, sqlargs);

    // }, 1 * 3 * 1000);

}