import { CardDom } from './cardDom.js';
import { GameController } from './gameController.js';
import { TopologyCardJsonLoader } from './topologyCardJsonLoader.js';
export class GameElementInitializer {
    constructor(row, column, jsonPath, imageFolderPath, flippingWaitTimeMilliseconds, maxSelectableCard) {
        this.row = row;
        this.column = column;
        this.jsonPath = jsonPath;
        this.imageFolderPath = imageFolderPath;
        this.flippingWaitTimeMilliseconds = flippingWaitTimeMilliseconds;
        this.maxSelectableCard = maxSelectableCard;
        this.gameController = null;
    }
    initialize() {
        this.initializeGameBoardElement();
        this.initializeRestartGameButtonElement();
    }
    initializeGameBoardElement() {
        const gameBoard = document.getElementById('game-board');
        // カードの行と列の枚数を指定する
        gameBoard.style.setProperty('--cols', String(this.column));
        gameBoard.style.setProperty('--rows', String(this.row));
        this.initializeCardsOnBoardElement(gameBoard);
    }
    initializeCardsOnBoardElement(gameBoard) {
        const topologyCardLoader = new TopologyCardJsonLoader();
        const cardData = topologyCardLoader.loadTopologyCardsJson(this.jsonPath);
        this.gameController = new GameController(cardData, this.maxSelectableCard, this.flippingWaitTimeMilliseconds, this.imageFolderPath);
        const cardDoms = [];
        for (let i = 0; i < this.row * this.column; i++) {
            const cardElement = document.createElement('div');
            cardElement.className = 'card';
            gameBoard.appendChild(cardElement);
            cardDoms.push(new CardDom(cardElement));
        }
        this.gameController.startGame(cardDoms);
    }
    initializeRestartGameButtonElement() {
        document.getElementById('restart-button').onclick = this.restartGame.bind(this);
    }
    restartGame() {
        this.gameController.restartGame();
    }
}
