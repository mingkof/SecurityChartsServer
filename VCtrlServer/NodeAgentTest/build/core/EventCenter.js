"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const Dictionary_1 = require("./data/Dictionary");
const Callback_1 = require("./data/Callback");
class EventCenter {
    constructor() {
        this.m_callbackMaps = new Dictionary_1.default();
        this.m_sendBuffer = [];
    }
    static getInstance() {
        if (EventCenter.s_instance == null) {
            EventCenter.s_instance = new EventCenter();
        }
        return EventCenter.s_instance;
    }
    /**
     * 注册事件监听
     */
    addEventListener(messageID, callback, thisObj, index) {
        if (callback && thisObj) {
            let data = new EventCallBack(callback, thisObj);
            data.index = index;
            data.messageID = messageID;
            let callbacks = this.m_callbackMaps.get(messageID);
            if (callbacks) {
                for (let i = 0, iLen = callbacks.length; i < iLen; i++) {
                    if (data.index < callbacks[i].index) {
                        callbacks.splice(i, 0, data);
                        return;
                    }
                }
                callbacks.push(data);
            }
            else {
                this.m_callbackMaps.add(messageID, [data]);
            }
        }
    }
    /**
     * 移除事件监听
     */
    removeEventListener(messageID, callback, thisObj) {
        let callbacks = this.m_callbackMaps.get(messageID);
        if (callbacks) {
            for (let i = 0, iLen = callbacks.length; i < iLen; i++) {
                let data = callbacks[i];
                if (data.callback === callback && data.thisObj === thisObj) {
                    data.isValid = false;
                }
            }
        }
    }
    /**
     * 发送消息
     */
    sendEvent(message) {
        // console.log("发送消息 sendEvent");
        this.m_sendBuffer.push(message);
        // egret.callLater(this.sendAll, this);
        this.sendAll();
    }
    /**
     * 发送所有消息
     */
    sendAll() {
        let t = Date.now();
        let max = 0;
        let max_data;
        while (this.m_sendBuffer.length > 0) {
            let event = this.m_sendBuffer.shift();
            let dataList = this.m_callbackMaps.get(event.messageID);
            if (dataList) {
                for (let i = dataList.length; i > 0; i--) {
                    let data = dataList[i - 1];
                    if (!data.isValid) {
                        dataList.splice(i - 1, 1);
                    }
                    else {
                        let t1 = Date.now();
                        data.callback.call(data.thisObj, event);
                        let t1_end = Date.now();
                        if (t1_end - t1 > max) {
                            max = t1_end - t1;
                            max_data = data;
                        }
                    }
                }
            }
            else {
                // egret.log("事件ID:" + event.messageID + "无监听回调");
            }
        }
        let t_end = Date.now() - t;
        // if (DebugUtils.EVENT_LOG && t_end > DebugUtils.EVENT_LIMIT && max_data) {
        //     egret.warn(`事件派发总耗时：${t_end} 最高耗时事件：${max_data.messageID} 耗时：${max}`);
        // }
    }
}
exports.default = EventCenter;
class EventCallBack extends Callback_1.default {
    constructor(callback, thisObj) {
        super(callback, thisObj);
        this.isValid = true;
    }
    clone() {
        let data = new EventCallBack(this.callback, this.thisObj);
        data.index = this.index;
        data.messageID = this.messageID;
        data.isValid = this.isValid;
        return data;
    }
}
