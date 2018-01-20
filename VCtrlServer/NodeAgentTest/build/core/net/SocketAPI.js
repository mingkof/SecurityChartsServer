"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const WebSocket = require("ws");
const ws = new WebSocket('ws://localhost:26605');
class SocketAPI {
    constructor() {
        this.m_state = WebSocketStateEnum.CLOSED;
        this.m_type = WebSocketTypeEnum.TYPE_STRING;
    }
    static get instance() {
        if (SocketAPI.s_instance == null) {
            SocketAPI.s_instance = new SocketAPI();
        }
        return SocketAPI.s_instance;
    }
    init(host) {
        this.m_host = host;
    }
}
exports.default = SocketAPI;
/**
 * CONNECTING   正在尝试连接服务器
 * CONNECTED    已成功连接服务器
 * CLOSING      正在断开服务器连接
 * CLOSED       已断开与服务器连接
 */
var WebSocketStateEnum;
(function (WebSocketStateEnum) {
    WebSocketStateEnum[WebSocketStateEnum["CONNECTING"] = 0] = "CONNECTING";
    WebSocketStateEnum[WebSocketStateEnum["CONNECTED"] = 1] = "CONNECTED";
    WebSocketStateEnum[WebSocketStateEnum["CLOSING"] = 2] = "CLOSING";
    WebSocketStateEnum[WebSocketStateEnum["CLOSED"] = 3] = "CLOSED";
})(WebSocketStateEnum = exports.WebSocketStateEnum || (exports.WebSocketStateEnum = {}));
/**
 * TYPE_STRING 以字符串格式发送和接收数据
 * TYPE_BINARY 以二进制格式发送和接收数据
 */
var WebSocketTypeEnum;
(function (WebSocketTypeEnum) {
    WebSocketTypeEnum[WebSocketTypeEnum["TYPE_STRING"] = 0] = "TYPE_STRING";
    WebSocketTypeEnum[WebSocketTypeEnum["TYPE_BINARY"] = 1] = "TYPE_BINARY";
})(WebSocketTypeEnum = exports.WebSocketTypeEnum || (exports.WebSocketTypeEnum = {}));
