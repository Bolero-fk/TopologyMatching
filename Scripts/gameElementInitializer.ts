import { CardDom } from './cardDom.js';
import { GameController } from './gameController.js';
import { TopologyCardJsonLoader } from './topologyCardJsonLoader.js';

export class GameConfig {
    row: number;
    column: number;
    jsonPath: string;
    imageFolderPath: string;
    flippingWaitTimeMilliseconds: number;
    maxSelectableCard: number;
}

export class GameElementInitializer {
    private gameController: GameController;
    private readonly config: GameConfig;

    constructor(config: GameConfig) {
        this.config = config;
        this.gameController = null;
    }

    public initialize(): void {
        this.initializeGameBoardElement();
        this.initializeRestartGameButtonElement();
    }

    private initializeGameBoardElement(): void {
        const gameBoard = document.getElementById('game-board');

        // カードの行と列の枚数を指定する
        gameBoard.style.setProperty('--cols', String(this.config.column));
        gameBoard.style.setProperty('--rows', String(this.config.row));

        this.initializeCardsOnBoardElement(gameBoard);
    }

    private initializeCardsOnBoardElement(gameBoard: HTMLElement): void {
        const topologyCardLoader = new TopologyCardJsonLoader();
        const cardData = topologyCardLoader.loadTopologyCardsJson(this.config.jsonPath);

        this.gameController = new GameController(cardData, this.config.maxSelectableCard, this.config.flippingWaitTimeMilliseconds, this.config.imageFolderPath);

        const cardDoms: CardDom[] = [];

        for (let i = 0; i < this.config.row * this.config.column; i++) {
            const cardElement = document.createElement('div');
            cardElement.className = 'card';
            gameBoard.appendChild(cardElement);

            cardDoms.push(new CardDom(cardElement));
        }

        this.gameController.startGame(cardDoms);
    }

    private initializeRestartGameButtonElement(): void {
        document.getElementById('restart-button').onclick = this.restartGame.bind(this);
    }

    private restartGame(): void {
        this.gameController.restartGame();
    }
}
