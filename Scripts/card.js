"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Card = exports.FlipStatus = void 0;
var IMAGE_FOLDER_PATH = './TopologyCards/images/';
var FlipStatus;
(function (FlipStatus) {
    FlipStatus[FlipStatus["Front"] = 0] = "Front";
    FlipStatus[FlipStatus["Back"] = 1] = "Back";
})(FlipStatus || (exports.FlipStatus = FlipStatus = {}));
var Card = /** @class */ (function () {
    function Card(element, onClickCallback) {
        var _this = this;
        this.element = element;
        this.flipStatus = FlipStatus.Back;
        this.onClickCallback = onClickCallback;
        this.element.onclick = function () {
            _this.onClick();
        };
    }
    /**
     * カードを変更します
     * @param matchingKey カードの種類を指定するキー
     * @param imageName カードの表面に表示する画像のurl
     */
    Card.prototype.changeCard = function (matchingKey, imageName) {
        this.matchingKey = matchingKey;
        this.frontImageUrl = 'url(' + IMAGE_FOLDER_PATH + imageName + ')';
    };
    /**
     * 入力されたカードを指定された方向に返します
     */
    Card.prototype.flipCard = function (flipStatus) {
        this.flipStatus = flipStatus;
        // カードの面ごとに色と画像を設定する
        if (this.flipStatus == FlipStatus.Front) {
            this.element.style.backgroundColor = getComputedStyle(this.element).getPropertyValue("--front-background-color");
            this.element.style.backgroundImage = this.frontImageUrl;
        }
        else {
            this.element.style.backgroundColor = getComputedStyle(this.element).getPropertyValue("--back-background-color");
            this.element.style.backgroundImage = '';
        }
    };
    Card.prototype.onClick = function () {
        if (this.flipStatus == FlipStatus.Front) {
            return;
        }
        this.onClickCallback();
    };
    return Card;
}());
exports.Card = Card;
