import { Card, FlipStatus } from "../dist/card.js";
import { ICardDom } from "../dist/ICardDom.js";

describe("Card", () => {
    let mockCardDom: ICardDom;
    let onClickMock: jest.Mock;

    const sampleMatchingKey1: string = "0,1,2";
    const sampleMatchingKey2: string = "0,1,2,3";
    const sampleImageUrl1: string = "sample1.svg";
    const sampleImageUrl2: string = "sample2.svg";
    const sampleImageUrl3: string = "sample3.svg";


    beforeEach(() => {
        onClickMock = jest.fn();
        mockCardDom = {
            flipToFront: jest.fn(),
            flipToBack: jest.fn(),
            onClick: jest.fn((handler) => handler())
        };
    });

    describe('constructor test', () => {
        test("should instantiate the Card class correctly", () => {
            const card = new Card(mockCardDom, sampleMatchingKey1, sampleImageUrl1, onClickMock);
            expect(card).toBeInstanceOf(Card);
            expect(mockCardDom.onClick).toHaveBeenCalled();
            expect(mockCardDom.flipToBack).toHaveBeenCalled();
        });
    });

    describe('card click test', () => {
        test("should handle card click correctly when card is at the back", () => {
            const card = new Card(mockCardDom, sampleMatchingKey1, sampleImageUrl1, onClickMock);
            expect(onClickMock).toHaveBeenCalled();
        });

        test("should not handle card click when card is at the front", () => {
            const card = new Card(mockCardDom, sampleMatchingKey1, sampleImageUrl1, onClickMock);
            card.flipCard(FlipStatus.Front);

            onClickMock.mockClear();
            (mockCardDom.onClick as jest.Mock).mock.calls[0][0]();

            expect(onClickMock).not.toHaveBeenCalled();
        });
    });

    describe('card flip test', () => {
        test("should flip to front correctly", () => {
            const card = new Card(mockCardDom, sampleMatchingKey1, sampleImageUrl1, onClickMock);
            card.flipCard(FlipStatus.Front);
            expect(mockCardDom.flipToFront).toHaveBeenCalledWith(sampleImageUrl1);
            expect(card.getFlipStatus()).toBe(FlipStatus.Front);
        });

        test("should flip to back correctly", () => {
            const card = new Card(mockCardDom, sampleMatchingKey1, sampleImageUrl1, onClickMock);
            card.flipCard(FlipStatus.Back);
            expect(mockCardDom.flipToBack).toHaveBeenCalled();
            expect(card.getFlipStatus()).toBe(FlipStatus.Back);
        });
    });

    describe('get flip status test', () => {
        test("should return the current flip status of the card", () => {
            const card = new Card(mockCardDom, sampleMatchingKey1, sampleImageUrl1, onClickMock);
            expect(card.getFlipStatus()).toBe(FlipStatus.Back);

            card.flipCard(FlipStatus.Front);
            expect(card.getFlipStatus()).toBe(FlipStatus.Front);

            card.flipCard(FlipStatus.Back);
            expect(card.getFlipStatus()).toBe(FlipStatus.Back);
        });
    });

    describe('match card test', () => {
        test("should match cards correctly", () => {

            const card1 = new Card(mockCardDom, sampleMatchingKey1, sampleImageUrl1, onClickMock);
            const card2 = new Card(mockCardDom, sampleMatchingKey1, sampleImageUrl2, onClickMock);
            const card3 = new Card(mockCardDom, sampleMatchingKey2, sampleImageUrl3, onClickMock);

            expect(Card.canMatchCard(card1, card2)).toBe(true);
            expect(Card.canMatchCard(card1, card3)).toBe(false);
        });
    });

    describe('cloneWithNewImage test', () => {
        test("should create a new card with updated image and matching key", () => {
            const card = new Card(mockCardDom, sampleMatchingKey1, sampleImageUrl1, onClickMock);
            const clonedCard = card.cloneWithNewImage(sampleMatchingKey2, sampleImageUrl2, onClickMock);

            expect(clonedCard.getFlipStatus()).toBe(FlipStatus.Back);

            (mockCardDom.onClick as jest.Mock).mock.calls[0][0]();
            expect(onClickMock).toHaveBeenCalled();
        });
    });
});
