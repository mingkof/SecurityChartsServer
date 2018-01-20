class AgentRunContent {
    public AgentType:number; //0:unknown, 1:buildShader, 2: Test
    public RunnerType:number; // 0-powershell
    public CmdList:Array<string>;
  }
  
class TaskStruct {
    public TaskGuid:string;
    public AuthorID:string;
    public TaskName:string;
    public TaskDesc:string;
    public AgentRunContent:AgentRunContent;
}
