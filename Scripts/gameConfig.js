/**
 * ゲームの設定
 */
export class GameConfig {
    constructor(row, column, jsonPath, imageFolderPath, flippingWaitTimeMilliseconds, maxSelectableCard) {
        this.row = row;
        this.column = column;
        this.jsonPath = jsonPath;
        this.imageFolderPath = imageFolderPath;
        this.flippingWaitTimeMilliseconds = flippingWaitTimeMilliseconds;
        this.maxSelectableCard = maxSelectableCard;
    }
}
