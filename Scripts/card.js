export var FlipStatus;
(function (FlipStatus) {
    FlipStatus[FlipStatus["Front"] = 0] = "Front";
    FlipStatus[FlipStatus["Back"] = 1] = "Back";
})(FlipStatus || (FlipStatus = {}));
export class Card {
    constructor(cardDom, onClickCallback, imageFolderPath) {
        this.flipStatus = FlipStatus.Back;
        this.onClickCallback = onClickCallback;
        this.cardDom = cardDom;
        this.imageFolderPath = imageFolderPath;
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
        this.frontImageUrl = 'url(' + this.imageFolderPath + imageName + ')';
    }
    /**
     * 入力されたカードを指定された方向に返します
     */
    flipCard(flipStatus) {
        this.flipStatus = flipStatus;
        // カードの面ごとに色と画像を設定する
        if (this.flipStatus === FlipStatus.Front) {
            this.cardDom.flipToFront(this.frontImageUrl);
        }
        else {
            this.cardDom.flipToBack();
        }
    }
    onClick() {
        if (this.flipStatus === FlipStatus.Front) {
            return;
        }
        this.onClickCallback();
    }
    static canMatchCard(card1, card2) {
        return card1.matchingKey === card2.matchingKey;
    }
}
