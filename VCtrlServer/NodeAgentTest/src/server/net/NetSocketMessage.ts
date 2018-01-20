import EventCenter from '../../core/EventCenter';
import EventID from '../../core/event/EventID';
import EventData from '../../core/event/EventData';
import * as WebSocket from 'ws';

import TaskCenter from '../../TaskManager/TaskCenter';




export default class NetSocketMessage {
	public constructor() {

  }

  public ws:WebSocket = new WebSocket('ws://localhost:26611');;

  public init(): void {
    EventCenter.getInstance().addEventListener(EventID.SOCKET_CONNECT, this.onSocketConnect, this);
    EventCenter.getInstance().addEventListener(EventID.SOCKET_CLOSE, this.onSocketClose, this);
    EventCenter.getInstance().addEventListener(EventID.SOCKET_DATA, this.onSocketData, this);
    // EventCenter.getInstance().addEventListener(EventID.SOCKET_IOERROR, this.onSocketIOError, this);

    this.ws.onopen = function(){
      EventCenter.getInstance().sendEvent(new EventData(EventID.SOCKET_CONNECT, "ok"));
    };
    this.ws.onmessage = function(message) {
      // console.log(message.data);
      EventCenter.getInstance().sendEvent(new EventData(EventID.SOCKET_DATA, message.data));
    };
    this.ws.onclose = function() {
      EventCenter.getInstance().sendEvent(new EventData(EventID.SOCKET_CLOSE, "close"));
    };

}

private onSocketConnect(data: any): void {
    var msg = "已连接服务器" + data.messageData;
    this.ws.send(msg);
 }

private onSocketClose(data: any): void {
  var msg = "关闭连接：" + data.messageData;
  console.log(msg);
}

private onSocketData(data: any): void {
  // var msgBase = '{"TaskGuid":"92d52b7a-c97d-4382-8b2e-52dd8c838bb6","AuthorID":"123","TaskName":"测试Task","TaskDesc":"测试Task","AgentRunContent":{"AgentType":2,"RunnerType":0,"CmdList":["node --version","npm --version"]}}';
  // console.log(obj.TaskGuid);
  console.log("服务器消息:" + data.messageData);
  // TaskCenter.getInstance().ReceiveTask(data.messageData);
}

// private onSocketIOError(data: any): void {
// }

public CreateMessage(protoClassName:string) {
}


public SendMessage(message: any) {
  console.log("发送消息:" + message);
  this.ws.send(message);
  // console.log("发送消息到服务器:" + message);
}


}