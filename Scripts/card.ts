const IMAGE_FOLDER_PATH = './TopologyCards/images/';

export enum FlipStatus {
    Front,
    Back,
}

export class Card {
    private element: HTMLElement;
    private flipStatus: FlipStatus;
    private frontImageUrl: string;
    private onClickCallback: () => void;

    public matchingKey: string;

    constructor(element: HTMLElement, onClickCallback: () => void) {
        this.element = element;
        this.flipStatus = FlipStatus.Back;
        this.onClickCallback = onClickCallback;
        this.element.onclick = () => {
            this.onClick();
        };
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
            this.element.style.backgroundColor = getComputedStyle(this.element).getPropertyValue("--front-background-color");
            this.element.style.backgroundImage = this.frontImageUrl;
        }
        else {
            this.element.style.backgroundColor = getComputedStyle(this.element).getPropertyValue("--back-background-color");
            this.element.style.backgroundImage = '';
        }
    }

    private onClick(): void {
        if (this.flipStatus == FlipStatus.Front) {
            return;
        }

        this.onClickCallback();
    }
}