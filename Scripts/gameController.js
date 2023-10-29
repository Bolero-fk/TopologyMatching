import { GameEngine } from './gameEngine.js';
import { Card, FlipStatus } from './card.js';
export class GameController {
    constructor(topologyCardsJson, maxSelectableCard, flippingWaitTimeMilliseconds, imageFolderPath) {
        this.validateInputs(topologyCardsJson, maxSelectableCard, flippingWaitTimeMilliseconds, imageFolderPath);
        this.gameEngine = new GameEngine(topologyCardsJson);
        this.cardsOnBoard = [];
        this.selectedCards = [];
        this.maxSelectableCard = maxSelectableCard;
        this.flippingWaitTimeMilliseconds = flippingWaitTimeMilliseconds;
        this.imageFolderPath = imageFolderPath;
    }
    validateInputs(topologyCardsJson, maxSelectableCard, flippingWaitTimeMilliseconds, imageFolderPath) {
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
    startGame(cardDoms) {
        const gameCardNumber = cardDoms.length;
        const cardStatus = this.gameEngine.startGame(gameCardNumber);
        for (let i = 0; i < gameCardNumber; i++) {
            const card = new Card(cardDoms[i], cardStatus[i].matchingKey, this.getImagePath(cardStatus[i].imageName), () => this.cardClickedCallback(card));
            this.cardsOnBoard.push(card);
        }
    }
    cardClickedCallback(card) {
        if (this.maxSelectableCard <= this.selectedCards.length) {
            return;
        }
        card.flipCard(FlipStatus.Front);
        this.selectedCards.push(card);
        if (this.maxSelectableCard <= this.selectedCards.length) {
            if (this.selectedCards.every((card) => Card.canMatchCard(card, this.selectedCards[0]))) {
                this.selectedCards.length = 0;
            }
            else {
                setTimeout(() => {
                    this.flipSelectedCardsToBack();
                    this.selectedCards.length = 0;
                }, this.flippingWaitTimeMilliseconds);
            }
        }
    }
    flipSelectedCardsToBack() {
        this.selectedCards.forEach(selectedCard => {
            selectedCard.flipCard(FlipStatus.Back);
        });
    }
    restartGame() {
        const cardStatus = this.gameEngine.startGame(this.cardsOnBoard.length);
        for (let i = 0; i < cardStatus.length; i++) {
            this.cardsOnBoard[i] = this.cardsOnBoard[i].cloneWithNewImage(cardStatus[i].matchingKey, this.getImagePath(cardStatus[i].imageName), () => this.cardClickedCallback(this.cardsOnBoard[i]));
        }
        this.selectedCards.length = 0;
    }
    getImagePath(imageFileName) {
        return `url(${this.imageFolderPath}${imageFileName})`;
    }
}
