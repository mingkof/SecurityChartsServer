"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const psshell = require('node-powershell');
class PSRunner {
    Run(commandList, callback) {
        commandList.forEach(function (cmd, index, array) {
            let ps = new psshell({
                executionPolicy: 'Bypass',
                noProfile: true
            });
            ps.addCommand(cmd);
            ps.invoke()
                .then(output => {
                if (callback != null) {
                    callback(index, output);
                }
                // console.log(output);
            })
                .catch(err => {
                console.log(err);
                ps.dispose();
            });
        });
    }
}
exports.default = PSRunner;
