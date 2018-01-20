var JSLogger = require('../Utils/JSLogger.js');

module.exports = function (callback) {

    const message = 'HHH: ' + new Date();

    JSLogger.Debug('AAA');
    JSLogger.Error('AAA');
    JSLogger.Warn('AAA');
    

    callback(
        null,
        message
    );
}
