import * as WebSocket from 'ws';
const ws = new WebSocket('ws://localhost:26605');

import EventCenter from '../../core/EventCenter';
import EventID from '../../core/event/EventID';

export default class SocketAPI {
    private static s_instance: SocketAPI;
    private m_webSocket: WebSocket;
    private m_host: string;
    private m_port: number;
    private m_state: WebSocketStateEnum = WebSocketStateEnum.CLOSED;
    private m_type: WebSocketTypeEnum = WebSocketTypeEnum.TYPE_STRING;

    public static get instance(): SocketAPI {
        if (SocketAPI.s_instance == null) {
            SocketAPI.s_instance = new SocketAPI();
        }
        return SocketAPI.s_instance;
    }

    public init(host:string) {
        this.m_host = host;


    }


}

/**
 * CONNECTING   正在尝试连接服务器
 * CONNECTED    已成功连接服务器 
 * CLOSING      正在断开服务器连接
 * CLOSED       已断开与服务器连接
 */
export enum WebSocketStateEnum {
    CONNECTING,
    CONNECTED,
    CLOSING,
    CLOSED
}

/**
 * TYPE_STRING 以字符串格式发送和接收数据
 * TYPE_BINARY 以二进制格式发送和接收数据
 */
export enum WebSocketTypeEnum {
    TYPE_STRING,
    TYPE_BINARY
}