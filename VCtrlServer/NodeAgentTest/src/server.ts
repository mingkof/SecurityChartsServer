import Test from './Test/Test';
import MessageCenter from './server/net/MessageCenter';
import { request } from 'https';
import PSRunner  from './AgentRunner/PSRunner';
import TaskCenter from './TaskManager/TaskCenter';
import TimeUtils from './Utils/TimeUtils';

import LogUtils from './Utils/LogUtils';

LogUtils.LogInfo_System('Startup Server Success!');


//初始化Socket消息管理器,连接websocket服务器
 MessageCenter.getInstance().Init();

//定时发送心跳包
setInterval(() => {
    MessageCenter.s_socket.SendMessage("heart");
    LogUtils.LogInfo_System('1234456!');

    MessageCenter.s_socket.SendMessage("{'type':'cmd', 'value':'1234', 'autho':'auth123'}");

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

