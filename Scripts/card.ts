import { ICardDom } from "./ICardDom.js";

export enum FlipStatus {
    Front,
    Back,
}

export class Card {
    private flipStatus: FlipStatus;
    private frontImageUrl: string;
    private onClickCallback: () => void;
    private frontBackgroundColor: string;
    private backBackgroundColor: string;
    private cardDom: ICardDom;
    private imageFolderPath: string;

    public matchingKey: string;

    constructor(cardDom: ICardDom, onClickCallback: () => void, frontBackgroundColor: string, backBackgroundColor: string, imageFolderPath: string) {
        this.flipStatus = FlipStatus.Back;
        this.onClickCallback = onClickCallback;
        this.cardDom = cardDom;
        this.imageFolderPath = imageFolderPath;

        this.frontBackgroundColor = frontBackgroundColor;
        this.backBackgroundColor = backBackgroundColor;

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
        if (this.flipStatus == FlipStatus.Front) {
            this.cardDom.setBackgroundColor(this.frontBackgroundColor);
            this.cardDom.setBackgroundImage(this.frontImageUrl);
        }
        else {
            this.cardDom.setBackgroundColor(this.backBackgroundColor);
            this.cardDom.setBackgroundImage('');
        }
    }

    private onClick(): void {
        if (this.flipStatus == FlipStatus.Front) {
            return;
        }

        this.onClickCallback();
    }
}