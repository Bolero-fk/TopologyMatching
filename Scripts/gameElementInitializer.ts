import { CardDom } from './cardDom.js';
import { GameController } from './gameController.js';
import { TopologyCardJsonLoader } from './topologyCardJsonLoader.js';

export class GameElementInitializer {
    private gameController: GameController;
    private readonly row: number;
    private readonly column: number;
    private readonly jsonPath: string;
    private readonly imageFolderPath: string;
    private readonly flippingWaitTimeMilliseconds: number;
    private readonly maxSelectableCard: number;

    constructor(row: number, column: number, jsonPath: string, imageFolderPath: string, flippingWaitTimeMilliseconds: number, maxSelectableCard: number) {
        this.row = row;
        this.column = column;
        this.jsonPath = jsonPath;
        this.imageFolderPath = imageFolderPath;
        this.flippingWaitTimeMilliseconds = flippingWaitTimeMilliseconds;
        this.maxSelectableCard = maxSelectableCard;
        this.gameController = null;
    }

    public initialize(): void {
        this.initializeGameBoardElement();
        this.initializeRestartGameButtonElement();
    }

    private initializeGameBoardElement(): void {
        const gameBoard = document.getElementById('game-board');

        // カードの行と列の枚数を指定する
        gameBoard.style.setProperty('--cols', String(this.column));
        gameBoard.style.setProperty('--rows', String(this.row));

        this.initializeCardsOnBoardElement(gameBoard);
    }

    private initializeCardsOnBoardElement(gameBoard: HTMLElement): void {
        const topologyCardLoader = new TopologyCardJsonLoader();
        const cardData = topologyCardLoader.loadTopologyCardsJson(this.jsonPath);

        this.gameController = new GameController(cardData, this.maxSelectableCard, this.flippingWaitTimeMilliseconds, this.imageFolderPath);

        const cardDoms: CardDom[] = [];

        for (let i = 0; i < this.row * this.column; i++) {
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
