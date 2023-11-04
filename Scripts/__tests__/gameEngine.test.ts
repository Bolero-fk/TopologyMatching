import { GameEngine, CardStatus } from '../gameEngine';

describe('GameEngine', () => {
    let topologyCards = [
        { ImageName: 'card1', HoleCount: [1, 2] },
        { ImageName: 'card2', HoleCount: [3, 4] },
        { ImageName: 'card3', HoleCount: [1, 2] },
        { ImageName: 'card4', HoleCount: [3, 4] },
        { ImageName: 'card5', HoleCount: [0, 0, 0] },
        { ImageName: 'card6', HoleCount: [0, 0, 0] },
        { ImageName: 'card7', HoleCount: [1] },
        { ImageName: 'card8', HoleCount: [1] },
        { ImageName: 'card9', HoleCount: [3, 4] },
        { ImageName: 'card10', HoleCount: [3, 4] },
        { ImageName: 'card11', HoleCount: [3, 4] },
        { ImageName: 'card12', HoleCount: [0] },
    ];

    const TEST_CARD_NUMBER = 4;

    let engine: GameEngine;

    describe('constructor positive test', () => {
        test('should initialize object correctly with valid data', () => {
            expect(() => new GameEngine(topologyCards)).not.toThrow();
        });
    });

    describe('startGame positive test', () => {
        beforeEach(() => {
            engine = new GameEngine(topologyCards);
        });

        test('should return an array of CardStatus with the specified length', () => {
            const result = engine.startGame(TEST_CARD_NUMBER);
            expect(result.length).toBe(TEST_CARD_NUMBER);
        });

        test('should return distinct card sets for different game starts', () => {
            const firstResult = engine.startGame(TEST_CARD_NUMBER);
            const secondResult = engine.startGame(TEST_CARD_NUMBER);
            expect(firstResult).not.toEqual(secondResult);
        });

        test('should not have duplicate cards in the card set', () => {
            const cards = engine.startGame(TEST_CARD_NUMBER);
            expect(() => ensureNoDuplicateCards(cards)).not.toThrow();
        });

        function ensureNoDuplicateCards(cards: CardStatus[]) {
            const cardSet = new Set<string>();
            cards.forEach(card => {
                if (cardSet.has(card.imageName)) {
                    throw new Error(`Duplicate card found: ${card.imageName}`);
                }
                cardSet.add(card.imageName);
            });
        }
    });

    describe('startGame negative test', () => {
        beforeEach(() => {
            engine = new GameEngine(topologyCards);
        });

        test('should throw an error when cardNum is less than or equal to 0', () => {
            expect(() => {
                engine.startGame(0);
            }).toThrowError("Card number must be greater than zero.");

            expect(() => {
                engine.startGame(-1);
            }).toThrowError("Card number must be greater than zero.");
        });

        test('should throw an error when cardNum is odd', () => {
            expect(() => {
                engine.startGame(3);
            }).toThrowError("Card number must be even.");
        });

        test('should throw an error if not enough cards are available', () => {
            expect(() => {
                engine.startGame(topologyCards.length + 2);
            }).toThrowError("Requested card number exceeds available cards.");
        });
    });
});