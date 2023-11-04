import { GameConfig } from './../gameConfig.js';

describe('GameConfig', () => {

    const SAMPLE_ROW: number = 2;
    const SAMPLE_COLUMN: number = 2;
    const SAMPLE_JSON_PATH: string = "sample path";
    const SAMPLE_IMAGE_FOLDER_PATH: string = "sample path";
    const SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS: number = 1000;
    const SAMPLE_MAX_SELECTABLE_CARD: number = 2;

    describe('Positive test', () => {
        test('should construct with valid parameters', () => {
            expect(() => new GameConfig(
                SAMPLE_ROW,
                SAMPLE_COLUMN,
                SAMPLE_JSON_PATH,
                SAMPLE_IMAGE_FOLDER_PATH,
                SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS,
                SAMPLE_MAX_SELECTABLE_CARD
            )).not.toThrow();
        });
    });

    describe('Negative test', () => {
        test('should throw an error if row is not positive', () => {
            expect(() => new GameConfig(
                0,
                SAMPLE_COLUMN,
                SAMPLE_JSON_PATH,
                SAMPLE_IMAGE_FOLDER_PATH,
                SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS,
                SAMPLE_MAX_SELECTABLE_CARD
            )).toThrow('Row must be positive values.');

            expect(() => new GameConfig(
                -1,
                SAMPLE_COLUMN,
                SAMPLE_JSON_PATH,
                SAMPLE_IMAGE_FOLDER_PATH,
                SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS,
                SAMPLE_MAX_SELECTABLE_CARD
            )).toThrow('Row must be positive values.');
        });

        test('should throw an error if column is not positive', () => {
            expect(() => new GameConfig(
                SAMPLE_ROW,
                0,
                SAMPLE_JSON_PATH,
                SAMPLE_IMAGE_FOLDER_PATH,
                SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS,
                SAMPLE_MAX_SELECTABLE_CARD
            )).toThrow('Column must be positive values.');

            expect(() => new GameConfig(
                SAMPLE_ROW,
                -1,
                SAMPLE_JSON_PATH,
                SAMPLE_IMAGE_FOLDER_PATH,
                SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS,
                SAMPLE_MAX_SELECTABLE_CARD
            )).toThrow('Column must be positive values.');
        });

        test('should throw an error if row * column is not even', () => {
            expect(() => new GameConfig(
                3,
                3,
                SAMPLE_JSON_PATH,
                SAMPLE_IMAGE_FOLDER_PATH,
                SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS,
                SAMPLE_MAX_SELECTABLE_CARD
            )).toThrow('The product of row and column must be an even number.');
        });

        test('should throw an error if jsonPath is empty', () => {
            expect(() => new GameConfig(
                SAMPLE_ROW,
                SAMPLE_COLUMN,
                "",
                SAMPLE_IMAGE_FOLDER_PATH,
                SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS,
                SAMPLE_MAX_SELECTABLE_CARD
            )).toThrow('jsonPath must be a non-empty string');
        });

        test('should throw an error if imageFolderPath is empty', () => {
            expect(() => new GameConfig(
                SAMPLE_ROW,
                SAMPLE_COLUMN,
                SAMPLE_JSON_PATH,
                "",
                SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS,
                SAMPLE_MAX_SELECTABLE_CARD
            )).toThrow('imageFolderPath must be a non-empty string');
        });

        test('should throw an error if flippingWaitTimeMilliseconds is negative', () => {
            expect(() => new GameConfig(
                SAMPLE_ROW,
                SAMPLE_COLUMN,
                SAMPLE_JSON_PATH,
                SAMPLE_IMAGE_FOLDER_PATH,
                0,
                SAMPLE_MAX_SELECTABLE_CARD
            )).toThrow('flippingWaitTimeMilliseconds must be a positive integer');

            expect(() => new GameConfig(
                SAMPLE_ROW,
                SAMPLE_COLUMN,
                SAMPLE_JSON_PATH,
                SAMPLE_IMAGE_FOLDER_PATH,
                -1,
                SAMPLE_MAX_SELECTABLE_CARD
            )).toThrow('flippingWaitTimeMilliseconds must be a positive integer');
        });

        test('should throw an error if maxSelectableCard is not positive', () => {
            expect(() => new GameConfig(
                SAMPLE_ROW,
                SAMPLE_COLUMN,
                SAMPLE_JSON_PATH,
                SAMPLE_IMAGE_FOLDER_PATH,
                SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS,
                0
            )).toThrow('maxSelectableCard must be a positive integer');

            expect(() => new GameConfig(
                SAMPLE_ROW,
                SAMPLE_COLUMN,
                SAMPLE_JSON_PATH,
                SAMPLE_IMAGE_FOLDER_PATH,
                SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS,
                -1
            )).toThrow('maxSelectableCard must be a positive integer');
        });
    });
});
