import { GameElementInitializer } from '../dist/gameElementInitializer.js';
import { GameController } from '../dist/gameController.js';
import { ICardDom } from '../dist/ICardDom.js';
import { ICardDomFactory } from '../dist/ICardDomFactory.js';
import { ITopologyCardJsonLoader } from '../dist/ITopologyCardJsonLoader.js';

describe('GameElementInitializer', () => {
    let mockCardDomFactory: ICardDomFactory;
    let mockTopologyCardJsonLoader: ITopologyCardJsonLoader;
    let gameConfig: any;

    beforeEach(() => {
        document.body.innerHTML = `
            <div id="game-board"></div>
            <button id="restart-button"></button>
        `;

        function createMockCardDom(): ICardDom {
            return {
                flipToFront: jest.fn(),
                flipToBack: jest.fn(),
                onClick: jest.fn()
            };
        }

        mockCardDomFactory = {
            create: jest.fn().mockReturnValue(createMockCardDom()),
        };

        mockTopologyCardJsonLoader = {
            loadTopologyCardsJson: jest.fn().mockReturnValue([
                { ImageName: 'card1', HoleCount: [1, 2] },
                { ImageName: 'card2', HoleCount: [1, 2] },
                { ImageName: 'card3', HoleCount: [0, 0, 0] },
                { ImageName: 'card4', HoleCount: [0, 0, 0] },
            ]),
        };

        gameConfig = {
            row: 2,
            column: 2,
            jsonPath: 'dummy path',
            imageFolderPath: 'dummy path',
            flippingWaitTimeMilliseconds: 1000,
            maxSelectableCard: 2,
        };
    });

    afterEach(() => {
        jest.clearAllMocks();
    });

    describe('Positive test', () => {
        test('should initialize game elements correctly', () => {
            const initializer = new GameElementInitializer(document, gameConfig, mockCardDomFactory, mockTopologyCardJsonLoader);

            initializer.initialize();

            expect(document.getElementById('game-board').style.getPropertyValue('--cols')).toBe('2');
            expect(document.getElementById('game-board').style.getPropertyValue('--rows')).toBe('2');
        });

        test('should restart game when restart button is clicked', () => {
            const mockGameController = {
                restartGame: jest.fn(),
            };
            jest.spyOn(GameController.prototype, 'restartGame').mockImplementation(mockGameController.restartGame);

            const initializer = new GameElementInitializer(document, gameConfig, mockCardDomFactory, mockTopologyCardJsonLoader);

            initializer.initialize();

            document.getElementById('restart-button').click();

            expect(mockGameController.restartGame).toHaveBeenCalled();
        });
    });

    describe('Negative test', () => {
        describe('when required HTML elements are missing', () => {
            test('should throw an error if game-board is missing', () => {
                document.body.innerHTML = `
                <button id="restart-button"></button>
            `;

                expect(() => new GameElementInitializer(document, gameConfig, mockCardDomFactory, mockTopologyCardJsonLoader)).toThrowError("game-board element is missing");
            });

            test('should throw an error if restart-button is missing', () => {
                document.body.innerHTML = `
                <div id="game-board"></div>
            `;

                expect(() => new GameElementInitializer(document, gameConfig, mockCardDomFactory, mockTopologyCardJsonLoader)).toThrowError("restart-button element is missing");
            });
        });
    });
});
