import NetSocketMessage from './NetSocketMessage';

import EventCenter from '../../core/EventCenter';
import EventID from '../../core/event/EventID';

import EventData from '../../core/event/EventData';

export default class MessageCenter {

    private static s_instance: MessageCenter;

    public static s_socket: NetSocketMessage = new NetSocketMessage();

    public constructor() {
    }

    public static getInstance(): MessageCenter {
        if (MessageCenter.s_instance == null) {
            MessageCenter.s_instance = new MessageCenter();
        }
        return MessageCenter.s_instance;
    }


    public Init(): void {

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

    private Test1(arg:string) {
        EventCenter.getInstance().sendEvent(new EventData('test1', arg))
    }

    private TestCB(data: EventData) : void  {
        console.log("测试回调 TEST CB 成功!" + data.messageID + "  " + data.messageData);
    }

    //protoClassName: 'onlineproto.cs_0x0100_game_login'
    public CreateMessage(protoClassName): any {
        var message = MessageCenter.s_socket.CreateMessage(protoClassName);
        return message;
    }

    public SendMessage(message: any) {
        // MessageCenter.s_socket.SendMessage(message);
    }


}