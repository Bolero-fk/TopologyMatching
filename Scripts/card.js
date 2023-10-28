const IMAGE_FOLDER_PATH = './TopologyCards/images/';
export var FlipStatus;
(function (FlipStatus) {
    FlipStatus[FlipStatus["Front"] = 0] = "Front";
    FlipStatus[FlipStatus["Back"] = 1] = "Back";
})(FlipStatus || (FlipStatus = {}));
export class Card {
    constructor(cardDom, onClickCallback) {
        this.flipStatus = FlipStatus.Back;
        this.onClickCallback = onClickCallback;
        this.cardDom = cardDom;
        this.frontBackgroundColor = this.cardDom.getComputedStyleProperty("--front-background-color");
        this.backBackgroundColor = this.cardDom.getComputedStyleProperty("--back-background-color");
        this.cardDom.onClick(() => {
            this.onClick();
        });
    }
    /**
     * カードを変更します
     * @param matchingKey カードの種類を指定するキー
     * @param imageName カードの表面に表示する画像のurl
     */
    changeCard(matchingKey, imageName) {
        this.matchingKey = matchingKey;
        this.frontImageUrl = 'url(' + IMAGE_FOLDER_PATH + imageName + ')';
    }
    /**
     * 入力されたカードを指定された方向に返します
     */
    flipCard(flipStatus) {
        this.flipStatus = flipStatus;
        // カードの面ごとに色と画像を設定する
        if (this.flipStatus == FlipStatus.Front) {
            this.cardDom.setBackgroundColor(this.frontBackgroundColor);
            this.cardDom.setBackgroundImage(this.frontImageUrl);
        }
        else {
            this.cardDom.setBackgroundColor(this.backBackgroundColor);
            this.cardDom.setBackgroundImage('');
        }
    }
    onClick() {
        if (this.flipStatus == FlipStatus.Front) {
            return;
        }
        this.onClickCallback();
    }
}
