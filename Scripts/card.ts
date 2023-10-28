import { ICardDom } from "./ICardDom.js";

export enum FlipStatus {
    Front,
    Back,
}

const IMAGE_FOLDER_PATH = './TopologyCards/images/';

export class Card {
    private flipStatus: FlipStatus;
    private frontImageUrl: string;
    private onClickCallback: () => void;
    private frontBackgroundColor: string;
    private backBackgroundColor: string;
    private cardDom: ICardDom;

    public matchingKey: string;

    constructor(cardDom: ICardDom, onClickCallback: () => void, frontBackgroundColor: string, backBackgroundColor: string) {
        this.flipStatus = FlipStatus.Back;
        this.onClickCallback = onClickCallback;
        this.cardDom = cardDom;

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
        this.frontImageUrl = 'url(' + IMAGE_FOLDER_PATH + imageName + ')';
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