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

    constructor(row, column, jsonPath, imageFolderPath, flippingWaitTimeMilliseconds, maxSelectableCard) {
        this.Validate(row, column, jsonPath, imageFolderPath, flippingWaitTimeMilliseconds, maxSelectableCard);
        this.row = row;
        this.column = column;
        this.jsonPath = jsonPath;
        this.imageFolderPath = imageFolderPath;
        this.flippingWaitTimeMilliseconds = flippingWaitTimeMilliseconds;
        this.maxSelectableCard = maxSelectableCard;
    }

    Validate(row, column, jsonPath, imageFolderPath, flippingWaitTimeMilliseconds, maxSelectableCard) {
        if (row <= 0) {
            throw new Error('Row must be positive values.');
        }
        if (column <= 0) {
            throw new Error('Column must be positive values.');
        }
        if (row * column % 2 !== 0) {
            throw new Error('The product of row and column must be an even number.');
        }
        if (jsonPath.trim() === '') {
            throw new Error('jsonPath must be a non-empty string');
        }
        if (imageFolderPath.trim() === '') {
            throw new Error('imageFolderPath must be a non-empty string');
        }
        if (flippingWaitTimeMilliseconds <= 0) {
            throw new Error('flippingWaitTimeMilliseconds must be a positive integer');
        }
        if (maxSelectableCard <= 0) {
            throw new Error('maxSelectableCard must be a positive integer');
        }
    }
}
