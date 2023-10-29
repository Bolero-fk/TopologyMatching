import { GameEngine } from './gameEngine.js';
import { Card, FlipStatus } from './card.js';
import { CardDom } from './cardDom.js';

export class GameController {
    private readonly gameEngine: GameEngine;
    private readonly cardsOnBoard: Card[];
    private readonly selectedCards: Card[];
    private readonly maxSelectableCard: number;
    private readonly flippingWaitTimeMilliseconds: number;
    private readonly imageFolderPath: string;

    constructor(topologyCardsJson: any[], maxSelectableCard: number, flippingWaitTimeMilliseconds: number, imageFolderPath: string) {
        this.validateInputs(topologyCardsJson, maxSelectableCard, flippingWaitTimeMilliseconds, imageFolderPath);

        this.gameEngine = new GameEngine(topologyCardsJson);
        this.cardsOnBoard = [] as Card[];
        this.selectedCards = [] as Card[];
        this.maxSelectableCard = maxSelectableCard;
        this.flippingWaitTimeMilliseconds = flippingWaitTimeMilliseconds;
        this.imageFolderPath = imageFolderPath;
    }

    private validateInputs(topologyCardsJson: any[], maxSelectableCard: number, flippingWaitTimeMilliseconds: number, imageFolderPath: string): void {

        topologyCardsJson.forEach(item => {
            const keys = Object.keys(item);
            if (keys.length !== 2 || !keys.includes('ImageName') || !keys.includes('HoleCount')) {
                throw new Error('Each item in topologyCardsJson must only have the keys "ImageName" and "HoleCount"');
            }

            if (typeof item.ImageName !== 'string' || item.ImageName.trim() === '') {
                throw new Error('ImageName must be a non-empty string');
            }

            if (!Array.isArray(item.HoleCount) || !item.HoleCount.every(val => typeof val === 'number')) {
                throw new Error('HoleCount must be an array of numbers');
            }
        });

        if (maxSelectableCard <= 0) {
            throw new Error('maxSelectableCard must be a positive integer');
        }

        if (flippingWaitTimeMilliseconds <= 0) {
            throw new Error('flippingWaitTimeMilliseconds must be a positive integer');
        }

        if (imageFolderPath.trim() === '') {
            throw new Error('imageFolderPath must be a non-empty string');
        }
    }

    public startGame(cardDoms: CardDom[]): void {
        const gameCardNumber = cardDoms.length;
        const cardStatus = this.gameEngine.startGame(gameCardNumber);

        for (let i = 0; i < gameCardNumber; i++) {
            const card: Card = new Card(cardDoms[i], cardStatus[i].matchingKey, this.getImagePath(cardStatus[i].imageName), () => this.cardClickedCallback(card));

            this.cardsOnBoard.push(card);
        }
    }

    private cardClickedCallback(card: Card): void {
        if (this.maxSelectableCard <= this.selectedCards.length) {
            return;
        }

        card.flipCard(FlipStatus.Front);
        this.selectedCards.push(card);

        if (this.maxSelectableCard <= this.selectedCards.length) {
            if (this.selectedCards.every((card) => Card.canMatchCard(card, this.selectedCards[0]))) {
                this.selectedCards.length = 0;
            } else {
                setTimeout(() => {
                    this.flipSelectedCardsToBack();
                    this.selectedCards.length = 0;
                }, this.flippingWaitTimeMilliseconds);
            }
        }
    }

    private flipSelectedCardsToBack(): void {
        this.selectedCards.forEach(selectedCard => {
            selectedCard.flipCard(FlipStatus.Back);
        });
    }

    public restartGame(): void {
        const cardStatus = this.gameEngine.startGame(this.cardsOnBoard.length);

        for (let i = 0; i < cardStatus.length; i++) {
            this.cardsOnBoard[i] = this.cardsOnBoard[i].cloneWithNewImage(cardStatus[i].matchingKey, this.getImagePath(cardStatus[i].imageName), () => this.cardClickedCallback(this.cardsOnBoard[i]));
        }

        this.selectedCards.length = 0;
    }

    private getImagePath(imageFileName: string): string {
        return `url(${this.imageFolderPath}${imageFileName})`;
    }
}