"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
// require('./TaskStruct');
const Dictionary_1 = require("../core/data/Dictionary");
const PSRunner_1 = require("../AgentRunner/PSRunner");
const TimeUtils_1 = require("../Utils/TimeUtils");
class TaskCenter {
    static getInstance() {
        if (TaskCenter.s_instance == null) {
            TaskCenter.s_instance = new TaskCenter();
        }
        return TaskCenter.s_instance;
    }
    constructor() {
        this.m_ReceivedTaskList = new Dictionary_1.default();
        this.m_DoingTaskList = new Dictionary_1.default();
    }
    //testdata:any = { 'TaskGuid' :'123', 'AuthorID':'345','TaskName':'fff','TaskDesc':'dec' } ;
    //接收分发的任务并保存到任务池
    ReceiveTask(message) {
        try {
            var obj = JSON.parse(message);
            let Task = obj;
            // console.log(Task.TaskGuid);
            var queryTask = this.m_ReceivedTaskList.get(Task.TaskGuid);
            if (queryTask == null) {
                this.m_ReceivedTaskList.add(Task.TaskGuid, Task);
                // console.log(getTask.AuthorID);
            }
            else {
                //this.m_ReceivedTaskList[Task.TaskGuid] = Task;
                console.error("重复接收到任务:" + message);
                return;
            }
        }
        catch (error) {
        }
    }
    //自动处理任务池
    StartTaskDispatcher() {
        //每隔一段时间检查任务池, 如果有未处理任务，则开始执行此任务
        setInterval(() => {
            if (this.m_ReceivedTaskList.keys.length <= 0)
                return;
            var task = this.m_ReceivedTaskList.getRandomData();
            this.m_DoingTaskList.add(task.TaskGuid, task);
            this.m_ReceivedTaskList.remove(task.TaskGuid);
            this.OnTaskHandle(task);
        }, 5000);
    }
    OnTaskHandle(task) {
        console.log("正在执行任务：" + task.TaskGuid);
        if (task.AgentRunContent.RunnerType == 0) {
            // var result = ARunner.RunPS(["node --version"]);
            var ARunner = new PSRunner_1.default();
            ARunner.Run(task.AgentRunContent.CmdList, function cbHandler(index, ret) {
                console.log(TimeUtils_1.default.GetTimeNow() + " - " + "任务:" + task.TaskGuid + " PS脚本执行结果:[" + index + "]  " + ret);
            });
        }
    }
}
exports.default = TaskCenter;
