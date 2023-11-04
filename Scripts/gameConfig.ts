/**
 * ゲームの設定
 */
export class GameConfig {
    public row: number;
    public column: number;
    public jsonPath: string;
    public imageFolderPath: string;
    public flippingWaitTimeMilliseconds: number;
    public maxSelectableCard: number;

    constructor(
        row: number,
        column: number,
        jsonPath: string,
        imageFolderPath: string,
        flippingWaitTimeMilliseconds: number,
        maxSelectableCard: number
    ) {
        this.row = row;
        this.column = column;
        this.jsonPath = jsonPath;
        this.imageFolderPath = imageFolderPath;
        this.flippingWaitTimeMilliseconds = flippingWaitTimeMilliseconds;
        this.maxSelectableCard = maxSelectableCard;
    }
}
