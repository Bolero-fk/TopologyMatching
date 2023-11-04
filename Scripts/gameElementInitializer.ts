import { GameController } from './gameController.js';
import { ICardDom } from './ICardDom.js';
import { ICardDomFactory } from './ICardDomFactory.js';
import { ITopologyCardJsonLoader } from './ITopologyCardJsonLoader.js';

export class GameConfig {
    public row: number;
    public column: number;
    public jsonPath: string;
    public imageFolderPath: string;
    public flippingWaitTimeMilliseconds: number;
    public maxSelectableCard: number;
}

export class GameElementInitializer {
    private gameController: GameController;
    private readonly config: GameConfig;
    private readonly cardDomFactory: ICardDomFactory;
    private readonly topologyCardJsonLoader: ITopologyCardJsonLoader;

    constructor(config: GameConfig, cardDomFactory: ICardDomFactory, topologyCardJsonLoader: ITopologyCardJsonLoader) {
        this.config = config;
        this.gameController = null;
        this.cardDomFactory = cardDomFactory;
        this.topologyCardJsonLoader = topologyCardJsonLoader;
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
        const cardData = this.topologyCardJsonLoader.loadTopologyCardsJson(this.config.jsonPath);

        this.gameController = new GameController(cardData, this.config.maxSelectableCard, this.config.flippingWaitTimeMilliseconds, this.config.imageFolderPath);

        const cardDoms: ICardDom[] = [];

        for (let i = 0; i < this.config.row * this.config.column; i++) {
            const cardElement = document.createElement('div');
            cardElement.className = 'card';
            gameBoard.appendChild(cardElement);

            cardDoms.push(this.cardDomFactory.create(cardElement));
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