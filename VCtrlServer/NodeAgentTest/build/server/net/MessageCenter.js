"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const NetSocketMessage_1 = require("./NetSocketMessage");
const EventCenter_1 = require("../../core/EventCenter");
const EventData_1 = require("../../core/event/EventData");
class MessageCenter {
    constructor() {
    }
    static getInstance() {
        if (MessageCenter.s_instance == null) {
            MessageCenter.s_instance = new MessageCenter();
        }
        return MessageCenter.s_instance;
    }
    Init() {
        MessageCenter.s_socket.init();
        /* 以下为测试

        console.log("MessageCenter 初始化!  1" );

        EventCenter.getInstance().addEventListener("test1", this.TestCB, this);

        console.log("MessageCenter 初始化!  2" );
        
        setInterval(() => this.Test1("testaaaaa"), 1000);
        setInterval(() => this.Test1("bbb"), 5000);
        
        console.log("MessageCenter 初始化! 3" );

       测试 */
    }
    Test1(arg) {
        EventCenter_1.default.getInstance().sendEvent(new EventData_1.default('test1', arg));
    }
    TestCB(data) {
        console.log("测试回调 TEST CB 成功!" + data.messageID + "  " + data.messageData);
    }
    //protoClassName: 'onlineproto.cs_0x0100_game_login'
    CreateMessage(protoClassName) {
        var message = MessageCenter.s_socket.CreateMessage(protoClassName);
        return message;
    }
    SendMessage(message) {
        // MessageCenter.s_socket.SendMessage(message);
    }
}
MessageCenter.s_socket = new NetSocketMessage_1.default();
exports.default = MessageCenter;
