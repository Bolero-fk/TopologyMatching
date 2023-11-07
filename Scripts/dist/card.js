/**
 * カードの表面または裏面を示す列挙型
 */
export var FlipStatus;
(function (FlipStatus) {
    FlipStatus[FlipStatus["Front"] = 0] = "Front";
    FlipStatus[FlipStatus["Back"] = 1] = "Back";
})(FlipStatus || (FlipStatus = {}));
/*
 * カードの振る舞いと状態を管理するクラス
 */
export class Card {
    /**
     * Cardクラスのコンストラクタ
     * @param cardDom カードのDOM操作を行うインターフェース
     * @param matchingKey カードの一致判定に使用するキー
     * @param frontImageUrl 表面の画像のURL
     * @param onClickCallback カードがクリックされた際の処理
     */
    constructor(cardDom, matchingKey, frontImageUrl, onClickCallback) {
        this.flipStatus = FlipStatus.Back;
        this.matchingKey = matchingKey;
        this.frontImageUrl = frontImageUrl;
        this.onClickCallback = onClickCallback;
        this.cardDom = cardDom;
        this.cardDom.onClick(() => {
            this.handleCardClick();
        });
        this.flipCard(FlipStatus.Back);
    }
    /**
     * 新しい画像でカードのクローンを生成します
     * @param matchingKey 新しいカードの一致判定に使用するキー
     * @param frontImageUrl 新しい表面の画像のURL
     * @param onClickCallback 新しいカードがクリックされた際の処理
     */
    cloneWithNewImage(matchingKey, frontImageUrl, onClickCallback) {
        return new Card(this.cardDom, matchingKey, frontImageUrl, onClickCallback);
    }
    /**
     * カードの向きを指定された状態に変更します
     * @param flipStatus カードの新しい向き (表/裏)
     */
    flipCard(flipStatus) {
        this.flipStatus = flipStatus;
        if (this.flipStatus === FlipStatus.Front) {
            this.cardDom.flipToFront(this.frontImageUrl);
        }
        else {
            this.cardDom.flipToBack();
        }
    }
    /**
     * カードがクリックされた際の内部処理を定義します
     */
    handleCardClick() {
        if (this.flipStatus === FlipStatus.Front) {
            return;
        }
        this.onClickCallback();
    }
    /**
     * カードが今どちら向きを示しているかを返します
     */
    getFlipStatus() {
        return this.flipStatus;
    }
    /**
     * 2枚のカードが一致するか判定します
     * @param card1 1つ目のカード
     * @param card2 2つ目のカード
     * @returns 2枚のカードが一致する場合はtrue、それ以外はfalse
     */
    static canMatchCard(card1, card2) {
        return card1.matchingKey === card2.matchingKey;
    }
}
