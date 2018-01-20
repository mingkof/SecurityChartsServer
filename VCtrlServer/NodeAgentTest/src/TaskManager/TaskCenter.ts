// require('./TaskStruct');
import Dictionary from '../core/data/Dictionary';
import PSRunner  from '../AgentRunner/PSRunner';

import TimeUtils from '../Utils/TimeUtils';

export default class TaskCenter {
    private static s_instance: TaskCenter;

    public static getInstance(): TaskCenter {
        if (TaskCenter.s_instance == null) {
            TaskCenter.s_instance = new TaskCenter();
        }
        return TaskCenter.s_instance;
    }
    //任务池
    private m_ReceivedTaskList: Dictionary<TaskStruct>;

    //正在处理中任务 <任务ID>
    private m_DoingTaskList: Dictionary<TaskStruct>;

    public constructor() {
        this.m_ReceivedTaskList = new Dictionary<TaskStruct>();
        this.m_DoingTaskList = new Dictionary<TaskStruct>();
    }


    //testdata:any = { 'TaskGuid' :'123', 'AuthorID':'345','TaskName':'fff','TaskDesc':'dec' } ;
    
    //接收分发的任务并保存到任务池
    public ReceiveTask(message:string) {
        try {
            var obj = JSON.parse(message);
            let Task = <TaskStruct>obj;

            // console.log(Task.TaskGuid);
            var queryTask = this.m_ReceivedTaskList.get(Task.TaskGuid);

            if(queryTask == null) {
                this.m_ReceivedTaskList.add(Task.TaskGuid, Task);
                // console.log(getTask.AuthorID);
            } else {
                //this.m_ReceivedTaskList[Task.TaskGuid] = Task;
                console.error("重复接收到任务:" + message);
                return;
            }
            
         } catch (error) {
         }
    }



    //自动处理任务池
    public StartTaskDispatcher()  {
        //每隔一段时间检查任务池, 如果有未处理任务，则开始执行此任务
        setInterval(() => {

            if(this.m_ReceivedTaskList.keys.length <= 0)
                return;

            var task = this.m_ReceivedTaskList.getRandomData();
            this.m_DoingTaskList.add(task.TaskGuid, task);
            this.m_ReceivedTaskList.remove(task.TaskGuid);

            this.OnTaskHandle(task);

        }, 5000);
    }



    private OnTaskHandle(task:TaskStruct) {

        console.log("正在执行任务：" + task.TaskGuid);

        if(task.AgentRunContent.RunnerType == 0) {
            // var result = ARunner.RunPS(["node --version"]);
            var ARunner = new PSRunner();
            ARunner.Run(task.AgentRunContent.CmdList, function cbHandler(index,ret)  {

                console.log(TimeUtils.GetTimeNow() + " - " +  "任务:" + task.TaskGuid + " PS脚本执行结果:[" + index + "]  " + ret);

            });
        }
    }

}