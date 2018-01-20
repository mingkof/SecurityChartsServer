"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const WebSocket = require("ws");
class SocketTest {
    constructor() {
        const ws = new WebSocket('ws://localhost:26605/ws');
        ws.on('open', function open() {
            const array = new Float32Array(5);
            for (var i = 0; i < array.length; ++i) {
                array[i] = i / 2;
            }
            console.log("哈哈哈");
            ws.send(array);
        });
    }
}
exports.default = SocketTest;
