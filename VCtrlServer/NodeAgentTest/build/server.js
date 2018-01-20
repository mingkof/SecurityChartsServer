"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const MessageCenter_1 = require("./server/net/MessageCenter");
const LogUtils_1 = require("./Utils/LogUtils");
LogUtils_1.default.LogInfo_System('Startup Server Success!');
//初始化Socket消息管理器,连接websocket服务器
MessageCenter_1.default.getInstance().Init();
//定时发送心跳包
setInterval(() => {
    MessageCenter_1.default.s_socket.SendMessage("heart");
    LogUtils_1.default.LogInfo_System('1234456!');
    MessageCenter_1.default.s_socket.SendMessage("{'type':'cmd', 'value':'1234', 'autho':'auth123'}");
}, 5000);
//开始启动任务管理-处理任务模块
// TaskCenter.getInstance().StartTaskDispatcher();
// //测试AgentRunner
// var ARunner = new AgentRunner();
// // var result = ARunner.RunPS(["node --version"]);
// ARunner.Run(["node --version", "npm --version", "echo 'hi'"],cbHandler);
// function cbHandler(index,ret)  {
//     console.log(index + " " + ret);
// }
