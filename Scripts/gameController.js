import { GameEngine } from './gameEngine.js';
import { Card, FlipStatus } from './card.js';
export class GameController {
    constructor(topologyCardsJson, maxSelectableCard, flippingWaitTimeMilliseconds, imageFolderPath) {
        this.gameEngine = new GameEngine(topologyCardsJson);
        this.cardsOnBoard = [];
        this.selectedCards = [];
        this.maxSelectableCard = maxSelectableCard;
        this.flippingWaitTimeMilliseconds = flippingWaitTimeMilliseconds;
        this.imageFolderPath = imageFolderPath;
    }
    startGame(cardDoms) {
        const gameCardNumber = cardDoms.length;
        const cardStatus = this.gameEngine.startGame(gameCardNumber);
        for (let i = 0; i < gameCardNumber; i++) {
            const imagePath = 'url(' + this.imageFolderPath + cardStatus[i].imageName + ')';
            const card = new Card(cardDoms[i], cardStatus[i].matchingKey, imagePath, () => this.cardClickedCallback(card));
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
            if (Card.canMatchCard(this.selectedCards[0], this.selectedCards[1])) {
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
    ;
    restartGame() {
        const cardStatus = this.gameEngine.startGame(this.cardsOnBoard.length);
        for (let i = 0; i < cardStatus.length; i++) {
            const imagePath = 'url(' + this.imageFolderPath + cardStatus[i].imageName + ')';
            this.cardsOnBoard[i] = this.cardsOnBoard[i].cloneWithNewImage(cardStatus[i].matchingKey, imagePath, () => this.cardClickedCallback(this.cardsOnBoard[i]));
        }
        this.selectedCards.length = 0;
    }
}
