// module core {
export default class EventData {

    public messageID: string;

    public messageData: any;

    constructor(messageID: string, messageData?: any) {
        this.messageID = messageID;
        this.messageData = messageData;
    }
}
// }


