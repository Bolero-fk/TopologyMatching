"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.CardDom = void 0;
var CardDom = /** @class */ (function () {
    function CardDom(element) {
        this.element = element;
        this.frontBackgroundColor = getComputedStyle(this.element).getPropertyValue("--front-background-color");
        this.backBackgroundColor = getComputedStyle(this.element).getPropertyValue("--back-background-color");
    }
    CardDom.prototype.flipToFront = function (url) {
        this.setBackgroundColor(this.frontBackgroundColor);
        this.setBackgroundImage(url);
    };
    CardDom.prototype.flipToBack = function () {
        this.setBackgroundColor(this.backBackgroundColor);
        this.setBackgroundImage();
    };
    CardDom.prototype.onClick = function (callback) {
        this.element.onclick = callback;
    };
    CardDom.prototype.setBackgroundColor = function (color) {
        this.element.style.backgroundColor = color;
    };
    CardDom.prototype.setBackgroundImage = function (url) {
        this.element.style.backgroundImage = url || '';
    };
    return CardDom;
}());
exports.CardDom = CardDom;
