import { ICardDom } from "./ICardDom.js";

export enum FlipStatus {
    Front,
    Back,
}

export class Card {
    private flipStatus: FlipStatus;
    private frontImageUrl: string;
    private readonly onClickCallback: () => void;
    private readonly cardDom: ICardDom;
    private readonly imageFolderPath: string;
    private matchingKey: string;

    constructor(cardDom: ICardDom, onClickCallback: () => void, imageFolderPath: string) {
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
    changeCard(matchingKey: string, imageName: string): void {
        this.matchingKey = matchingKey;
        this.frontImageUrl = 'url(' + this.imageFolderPath + imageName + ')';
    }

    /**
     * 入力されたカードを指定された方向に返します
     */
    flipCard(flipStatus: FlipStatus): void {
        this.flipStatus = flipStatus;

        // カードの面ごとに色と画像を設定する
        if (this.flipStatus === FlipStatus.Front) {
            this.cardDom.flipToFront(this.frontImageUrl);
        }
        else {
            this.cardDom.flipToBack();
        }
    }

    private onClick(): void {
        if (this.flipStatus === FlipStatus.Front) {
            return;
        }

        this.onClickCallback();
    }

    public static canMatchCard(card1: Card, card2: Card): boolean {
        return card1.matchingKey === card2.matchingKey;
    }
}