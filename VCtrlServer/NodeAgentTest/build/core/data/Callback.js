"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class Callback {
    constructor(callback, thisObj) {
        this.callback = callback;
        this.thisObj = thisObj;
    }
}
exports.default = Callback;
