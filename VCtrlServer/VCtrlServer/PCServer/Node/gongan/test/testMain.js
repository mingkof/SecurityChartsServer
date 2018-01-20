var queryResult = require('./QueryQCZResult.js');
var queryTicket = require('./QueryQCZTicket.js');


function getResult(err,dataArray) {
    console.log("Result: "+dataArray.length.toString());
}

function getTicket(err,dataArray) {
    console.log("Result: "+dataArray.length.toString());
}


queryResult(getResult,"123");
queryTicket(getTicket,"123");
