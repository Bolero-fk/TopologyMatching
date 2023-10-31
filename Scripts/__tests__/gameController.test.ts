import { GameController } from './../gameController.js';
import { ICardDom } from './../ICardDom.js';
import { TopologyCardJson } from './../JsonType.js';
import { CardStatus, GameEngine } from './../gameEngine.js';

describe('GameController', () => {
    let sampleTopologyCardsJson: TopologyCardJson[];
    let sampleCardDoms: ICardDom[];

    const SAMPLE_MAX_SELECTABLE_CARD: number = 2;
    const SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS: number = 1000;
    const SAMPLE_IMAGE_FOLDER_PATH: string = "sample/";

    function createMockCardDom(): ICardDom {
        return {
            flipToFront: jest.fn(),
            flipToBack: jest.fn(),
            onClick: jest.fn()
        };
    }

    beforeEach(() => {
        sampleTopologyCardsJson = [
            { ImageName: 'image1.svg', HoleCount: [0] },
            { ImageName: 'image2.svg', HoleCount: [0] }
        ];

        sampleCardDoms = [createMockCardDom(), createMockCardDom()];
    });

    describe('constructor positive tests', () => {
        test('should initialize object correctly with valid data', () => {
            expect(() => {
                new GameController(sampleTopologyCardsJson, SAMPLE_MAX_SELECTABLE_CARD, SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS, SAMPLE_IMAGE_FOLDER_PATH);
            }).not.toThrow();
        });
    });

    describe('constructor negative tests', () => {
        test('should throw error if input invalid json', () => {
            expect(() => {
                new GameController([{ ImageName: 'image1.svg', HoleCount: [-1] }], SAMPLE_MAX_SELECTABLE_CARD, SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS, SAMPLE_IMAGE_FOLDER_PATH);
            }).toThrowError('HoleCount must be a positive integer');

            expect(() => {
                new GameController([{ ImageName: '', HoleCount: [2] }], SAMPLE_MAX_SELECTABLE_CARD, SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS, SAMPLE_IMAGE_FOLDER_PATH);
            }).toThrowError('ImageName must be a non-empty string');
        });

        test('should throw error if input invalid selectablecard', () => {
            expect(() => {
                new GameController(sampleTopologyCardsJson, 0, SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS, SAMPLE_IMAGE_FOLDER_PATH);
            }).toThrowError('maxSelectableCard must be a positive integer');

            expect(() => {
                new GameController(sampleTopologyCardsJson, -1, SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS, SAMPLE_IMAGE_FOLDER_PATH);
            }).toThrowError('maxSelectableCard must be a positive integer');
        });

        test('should throw error if input invalid fllipingTime', () => {
            expect(() => {
                new GameController(sampleTopologyCardsJson, SAMPLE_MAX_SELECTABLE_CARD, 0, SAMPLE_IMAGE_FOLDER_PATH);
            }).toThrowError('flippingWaitTimeMilliseconds must be a positive integer');

            expect(() => {
                new GameController(sampleTopologyCardsJson, SAMPLE_MAX_SELECTABLE_CARD, -1, SAMPLE_IMAGE_FOLDER_PATH);
            }).toThrowError('flippingWaitTimeMilliseconds must be a positive integer');
        });

        test('should throw error if input invalid imagePath', () => {
            expect(() => {
                new GameController(sampleTopologyCardsJson, SAMPLE_MAX_SELECTABLE_CARD, SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS, '');
            }).toThrowError('imageFolderPath must be a non-empty string');
        });
    });

    describe('startGame tests', () => {
        test('should initialize cards on board correctly', () => {
            const gameController = new GameController(sampleTopologyCardsJson, SAMPLE_MAX_SELECTABLE_CARD, SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS, SAMPLE_IMAGE_FOLDER_PATH);
            gameController.startGame(sampleCardDoms);

            expect(gameController.getCardNumebr()).toBe(2);
        });
    });

    describe('cardClickedCallback tests', () => {

        const callBackTestJson = [
            { ImageName: 'image0.svg', HoleCount: [0] },
            { ImageName: 'image1.svg', HoleCount: [0] },
            { ImageName: 'image2.svg', HoleCount: [1] },
            { ImageName: 'image3.svg', HoleCount: [1] },
        ];

        let callBackTestDoms: ICardDom[];

        beforeEach(() => {
            // gemeEngineがランダムに並び替えるとテストが難しくなるので返り値を固定する
            jest.spyOn(GameEngine.prototype, 'startGame').mockReturnValue([
                new CardStatus('image0.svg', [0]),
                new CardStatus('image1.svg', [0]),
                new CardStatus('image2.svg', [1]),
                new CardStatus('image3.svg', [1]),
            ]);

            jest.useFakeTimers();
            callBackTestDoms = [createMockCardDom(), createMockCardDom(), createMockCardDom(), createMockCardDom()];
        });

        afterEach(() => {
            jest.clearAllTimers();
            jest.clearAllMocks();
        });

        test('should select only one card when one card is clicked', () => {
            const gameController = new GameController(callBackTestJson, SAMPLE_MAX_SELECTABLE_CARD, SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS, SAMPLE_IMAGE_FOLDER_PATH);
            gameController.startGame(callBackTestDoms);

            (callBackTestDoms[0].onClick as jest.Mock).mock.calls[0][0]();

            expect(gameController.getSelectedCardNumber()).toBe(1);
        });

        test('should reset selected cards when two cards with the same holeCount are clicked', () => {
            const gameController = new GameController(callBackTestJson, SAMPLE_MAX_SELECTABLE_CARD, SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS, SAMPLE_IMAGE_FOLDER_PATH);
            gameController.startGame(callBackTestDoms);

            (callBackTestDoms[0].onClick as jest.Mock).mock.calls[0][0]();
            (callBackTestDoms[1].onClick as jest.Mock).mock.calls[0][0]();

            expect(gameController.getSelectedCardNumber()).toBe(0);
        });

        test('should not allow a third card to be selected within flipping wait time when two cards with different holeCount are clicked', () => {
            const gameController = new GameController(callBackTestJson, SAMPLE_MAX_SELECTABLE_CARD, SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS, SAMPLE_IMAGE_FOLDER_PATH);
            gameController.startGame(callBackTestDoms);

            (callBackTestDoms[0].onClick as jest.Mock).mock.calls[0][0]();
            (callBackTestDoms[2].onClick as jest.Mock).mock.calls[0][0]();

            jest.advanceTimersByTime(SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS - 1);

            expect(gameController.getSelectedCardNumber()).toBe(2);

            (callBackTestDoms[3].onClick as jest.Mock).mock.calls[0][0]();
            expect(gameController.getSelectedCardNumber()).toBe(2);
        });

        test('should reset selected cards after flipping wait time when two cards with different holeCount are clicked', () => {
            const gameController = new GameController(callBackTestJson, SAMPLE_MAX_SELECTABLE_CARD, SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS, SAMPLE_IMAGE_FOLDER_PATH);
            gameController.startGame(callBackTestDoms);

            (callBackTestDoms[0].onClick as jest.Mock).mock.calls[0][0]();
            (callBackTestDoms[2].onClick as jest.Mock).mock.calls[0][0]();

            expect(gameController.getSelectedCardNumber()).toBe(2);

            jest.advanceTimersByTime(SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS);

            expect(gameController.getSelectedCardNumber()).toBe(0);
        });
    });

    describe('restartGame tests', () => {

        const restartTestJson = [
            { ImageName: 'image0.svg', HoleCount: [0] },
            { ImageName: 'image1.svg', HoleCount: [0] },
            { ImageName: 'image2.svg', HoleCount: [1] },
            { ImageName: 'image3.svg', HoleCount: [1] },
        ];

        let restartTestDoms: ICardDom[];

        beforeEach(() => {
            jest.spyOn(GameEngine.prototype, 'startGame').mockReturnValue([
                new CardStatus('image0.svg', [0]),
                new CardStatus('image1.svg', [0]),
                new CardStatus('image2.svg', [1]),
                new CardStatus('image3.svg', [1]),
            ]);

            jest.useFakeTimers();
            restartTestDoms = [createMockCardDom(), createMockCardDom(), createMockCardDom(), createMockCardDom()];
        });

        afterEach(() => {
            jest.clearAllTimers();
            jest.clearAllMocks();
        });

        test('should reset the game state correctly', () => {
            const gameController = new GameController(restartTestJson, SAMPLE_MAX_SELECTABLE_CARD, SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS, SAMPLE_IMAGE_FOLDER_PATH);
            gameController.startGame(restartTestDoms);

            (restartTestDoms[0].onClick as jest.Mock).mock.calls[0][0]();
            (restartTestDoms[1].onClick as jest.Mock).mock.calls[0][0]();

            expect(gameController.getSelectedCardNumber()).toBe(0);

            gameController.restartGame();

            expect(gameController.getSelectedCardNumber()).toBe(0);
            expect(restartTestDoms[0].flipToBack).toHaveBeenCalled();
            expect(restartTestDoms[1].flipToBack).toHaveBeenCalled();
            expect(restartTestDoms[2].flipToBack).toHaveBeenCalled();
            expect(restartTestDoms[3].flipToBack).toHaveBeenCalled();
        });

        test('should clear all timers', () => {
            const gameController = new GameController(restartTestJson, SAMPLE_MAX_SELECTABLE_CARD, SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS, SAMPLE_IMAGE_FOLDER_PATH);
            gameController.startGame(restartTestDoms);

            (restartTestDoms[0].onClick as jest.Mock).mock.calls[0][0]();
            (restartTestDoms[2].onClick as jest.Mock).mock.calls[0][0]();

            expect(jest.getTimerCount()).toBeGreaterThan(0);

            gameController.restartGame();

            expect(jest.getTimerCount()).toBe(0);
        });

        test('should call startGame on restart', () => {
            const gameController = new GameController(restartTestJson, SAMPLE_MAX_SELECTABLE_CARD, SAMPLE_FLIPPING_WAIT_TIME_MILLISECONDS, SAMPLE_IMAGE_FOLDER_PATH);

            const startGameSpy = jest.spyOn(GameEngine.prototype, 'startGame');

            gameController.startGame(restartTestDoms);
            gameController.restartGame();

            expect(startGameSpy).toHaveBeenCalledTimes(2);
        });

    });

});
