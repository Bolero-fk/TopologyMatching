export var FlipStatus;
(function (FlipStatus) {
    FlipStatus[FlipStatus["Front"] = 0] = "Front";
    FlipStatus[FlipStatus["Back"] = 1] = "Back";
})(FlipStatus || (FlipStatus = {}));
export class Card {
    constructor(cardDom, matchingKey, frontImageUrl, onClickCallback) {
        this.flipStatus = FlipStatus.Back;
        this.matchingKey = matchingKey;
        this.frontImageUrl = frontImageUrl;
        this.onClickCallback = onClickCallback;
        this.cardDom = cardDom;
        this.cardDom.onClick(() => {
            this.onClick();
        });
        this.flipCard(FlipStatus.Back);
    }
    /**
     * カードを新しい柄に変更します
     * @param matchingKey カードの種類を指定するキー
     * @param imageName カードの表面に表示する画像のurl
     */
    cloneWithNewImage(matchingKey, frontImageUrl, onClickCallback) {
        return new Card(this.cardDom, matchingKey, frontImageUrl, onClickCallback);
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
