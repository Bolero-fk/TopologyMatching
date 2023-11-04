import { CardDom } from './cardDom.js';
import { GameController } from './gameController.js';
import { TopologyCardJsonLoader } from './topologyCardJsonLoader.js';
export class GameConfig {
}
export class GameElementInitializer {
    constructor(config) {
        this.config = config;
        this.gameController = null;
    }
    initialize() {
        this.initializeGameBoardElement();
        this.initializeRestartGameButtonElement();
    }
    initializeGameBoardElement() {
        const gameBoard = document.getElementById('game-board');
        // カードの行と列の枚数を指定する
        gameBoard.style.setProperty('--cols', String(this.config.column));
        gameBoard.style.setProperty('--rows', String(this.config.row));
        this.initializeCardsOnBoardElement(gameBoard);
    }
    initializeCardsOnBoardElement(gameBoard) {
        const topologyCardLoader = new TopologyCardJsonLoader();
        const cardData = topologyCardLoader.loadTopologyCardsJson(this.config.jsonPath);
        this.gameController = new GameController(cardData, this.config.maxSelectableCard, this.config.flippingWaitTimeMilliseconds, this.config.imageFolderPath);
        const cardDoms = [];
        for (let i = 0; i < this.config.row * this.config.column; i++) {
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
