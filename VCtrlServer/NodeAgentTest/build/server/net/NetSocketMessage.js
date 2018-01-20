"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const EventCenter_1 = require("../../core/EventCenter");
const EventID_1 = require("../../core/event/EventID");
const EventData_1 = require("../../core/event/EventData");
const WebSocket = require("ws");
class NetSocketMessage {
    constructor() {
        this.ws = new WebSocket('ws://localhost:26611');
    }
    ;
    init() {
        EventCenter_1.default.getInstance().addEventListener(EventID_1.default.SOCKET_CONNECT, this.onSocketConnect, this);
        EventCenter_1.default.getInstance().addEventListener(EventID_1.default.SOCKET_CLOSE, this.onSocketClose, this);
        EventCenter_1.default.getInstance().addEventListener(EventID_1.default.SOCKET_DATA, this.onSocketData, this);
        // EventCenter.getInstance().addEventListener(EventID.SOCKET_IOERROR, this.onSocketIOError, this);
        this.ws.onopen = function () {
            EventCenter_1.default.getInstance().sendEvent(new EventData_1.default(EventID_1.default.SOCKET_CONNECT, "ok"));
        };
        this.ws.onmessage = function (message) {
            // console.log(message.data);
            EventCenter_1.default.getInstance().sendEvent(new EventData_1.default(EventID_1.default.SOCKET_DATA, message.data));
        };
        this.ws.onclose = function () {
            EventCenter_1.default.getInstance().sendEvent(new EventData_1.default(EventID_1.default.SOCKET_CLOSE, "close"));
        };
    }
    onSocketConnect(data) {
        var msg = "已连接服务器" + data.messageData;
        this.ws.send(msg);
    }
    onSocketClose(data) {
        var msg = "关闭连接：" + data.messageData;
        console.log(msg);
    }
    onSocketData(data) {
        // var msgBase = '{"TaskGuid":"92d52b7a-c97d-4382-8b2e-52dd8c838bb6","AuthorID":"123","TaskName":"测试Task","TaskDesc":"测试Task","AgentRunContent":{"AgentType":2,"RunnerType":0,"CmdList":["node --version","npm --version"]}}';
        // console.log(obj.TaskGuid);
        console.log("服务器消息:" + data.messageData);
        // TaskCenter.getInstance().ReceiveTask(data.messageData);
    }
    // private onSocketIOError(data: any): void {
    // }
    CreateMessage(protoClassName) {
    }
    SendMessage(message) {
        console.log("发送消息:" + message);
        this.ws.send(message);
        // console.log("发送消息到服务器:" + message);
    }
}
exports.default = NetSocketMessage;
