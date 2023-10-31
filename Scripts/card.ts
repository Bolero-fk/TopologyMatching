import { ICardDom } from "./ICardDom.js";

/**
 * カードの表面または裏面を示す列挙型
 */
export enum FlipStatus {
    Front,
    Back,
}

/**
 * カードの振る舞いと状態を管理するクラス
 */
export class Card {
    private flipStatus: FlipStatus;
    private frontImageUrl: string;
    private readonly onClickCallback: () => void;
    private readonly cardDom: ICardDom;
    private readonly matchingKey: string;

    /**
     * Cardクラスのコンストラクタ
     * @param cardDom カードのDOM操作を行うインターフェース
     * @param matchingKey カードの一致判定に使用するキー
     * @param frontImageUrl 表面の画像のURL
     * @param onClickCallback カードがクリックされた際の処理
     */
    constructor(cardDom: ICardDom, matchingKey: string, frontImageUrl: string, onClickCallback: () => void) {
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
    cloneWithNewImage(matchingKey: string, frontImageUrl: string, onClickCallback: () => void): Card {
        return new Card(this.cardDom, matchingKey, frontImageUrl, onClickCallback);
    }

    /**
     * カードの向きを指定された状態に変更します
     * @param flipStatus カードの新しい向き (表/裏)
     */
    flipCard(flipStatus: FlipStatus): void {
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
    private handleCardClick(): void {
        if (this.flipStatus === FlipStatus.Front) {
            return;
        }

        this.onClickCallback();
    }

    /**
     * カードが今どちら向きを示しているかを返します
     */
    public getFlipStatus(): FlipStatus {
        return this.flipStatus;
    }

    /**
     * 2枚のカードが一致するか判定します
     * @param card1 1つ目のカード
     * @param card2 2つ目のカード
     * @returns 2枚のカードが一致する場合はtrue、それ以外はfalse
     */
    public static canMatchCard(card1: Card, card2: Card): boolean {
        return card1.matchingKey === card2.matchingKey;
    }
}