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

module.exports=function(callback, arg) {
    oracledb.getConnection(
        oracleConfig,
        function (err,connection) {
            if(err){
                console.log(err.message);
                return;
            }


            sqlCommand ="";
            if(arg==0)
            {
                sqlCommand ="select * from jabd.t_ticket_info";
            }
            else
            {
                sqlCommand ="select * from jabd.t_ticket_info";
            }



            connection.execute(
                sqlCommand,
                function (err,result) {
                    if(err){
                        console.log(err.message);
                        return;
                    }

                    console.log(sqlCommand);
                    console.log(result.rows.length);

                    doRelease(connection);

                    callback(null,result.rows);
                }
            );
        }
    );
}

