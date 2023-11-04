import { GameController } from './gameController.js';
export class GameConfig {
}
export class GameElementInitializer {
    constructor(config, cardDomFactory, topologyCardJsonLoader) {
        this.config = config;
        this.gameController = null;
        this.cardDomFactory = cardDomFactory;
        this.topologyCardJsonLoader = topologyCardJsonLoader;
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
        const cardData = this.topologyCardJsonLoader.loadTopologyCardsJson(this.config.jsonPath);
        this.gameController = new GameController(cardData, this.config.maxSelectableCard, this.config.flippingWaitTimeMilliseconds, this.config.imageFolderPath);
        const cardDoms = [];
        for (let i = 0; i < this.config.row * this.config.column; i++) {
            const cardElement = document.createElement('div');
            cardElement.className = 'card';
            gameBoard.appendChild(cardElement);
            cardDoms.push(this.cardDomFactory.create(cardElement));
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
