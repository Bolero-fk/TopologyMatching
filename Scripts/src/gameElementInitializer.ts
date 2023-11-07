import { GameController } from '../dist/gameController.js';
import { GameConfig } from '../dist/gameConfig.js';
import { ICardDom } from '../dist/ICardDom.js';
import { ICardDomFactory } from '../dist/ICardDomFactory.js';
import { ITopologyCardJsonLoader } from '../dist/ITopologyCardJsonLoader.js';

/**
 * ゲームのHTML要素を初期化するクラス
 */
export class GameElementInitializer {
    private readonly htmlDocument: Document;
    private gameController: GameController;
    private readonly config: GameConfig;
    private readonly cardDomFactory: ICardDomFactory;
    private readonly topologyCardJsonLoader: ITopologyCardJsonLoader;

    /**
     * @param htmlDocument ゲームの描画を行うhtmlDocument
     * @param config ゲームの設定
     * @param cardDomFactory ICardDomインスタンスを生成するためのファクトリ
     * @param topologyCardJsonLoader カード情報のJSONを読み込むローダ
     */
    constructor(htmlDocument: Document, config: GameConfig, cardDomFactory: ICardDomFactory, topologyCardJsonLoader: ITopologyCardJsonLoader) {
        this.validate(htmlDocument);

        this.htmlDocument = htmlDocument;
        this.config = config;
        this.gameController = null;
        this.cardDomFactory = cardDomFactory;
        this.topologyCardJsonLoader = topologyCardJsonLoader;
    }

    private validate(htmlDocument: Document) {
        if (!htmlDocument.getElementById('game-board')) {
            throw new Error("game-board element is missing");
        }

        if (!htmlDocument.getElementById('restart-button')) {
            throw new Error("restart-button element is missing");
        }
    }

    /**
     * html上に配置する要素を初期化します
     */
    public initialize(): void {
        this.initializeGameBoardElement();
        this.initializeRestartGameButtonElement();
    }

    /**
     * game boardを初期化します
     */
    private initializeGameBoardElement(): void {
        const gameBoard = this.htmlDocument.getElementById('game-board');

        // カードの行と列の枚数を指定する
        gameBoard.style.setProperty('--cols', String(this.config.column));
        gameBoard.style.setProperty('--rows', String(this.config.row));

        this.initializeCardsOnBoardElement(gameBoard);
    }

    /**
     * game board上のカードを初期化します
     * @param gameBoard カードを配置するHtml Element
     */
    private initializeCardsOnBoardElement(gameBoard: HTMLElement): void {
        const cardData = this.topologyCardJsonLoader.loadTopologyCardsJson(this.config.jsonPath);

        this.gameController = new GameController(cardData, this.config.maxSelectableCard, this.config.flippingWaitTimeMilliseconds, this.config.imageFolderPath);

        const cardDoms: ICardDom[] = [];

        for (let i = 0; i < this.config.row * this.config.column; i++) {
            const cardElement = this.htmlDocument.createElement('div');
            cardElement.className = 'card';
            gameBoard.appendChild(cardElement);

            cardDoms.push(this.cardDomFactory.create(cardElement));
        }

        this.gameController.startGame(cardDoms);
    }

    /**
     * Restart Game ボタンを初期化します
     */
    private initializeRestartGameButtonElement(): void {
        this.htmlDocument.getElementById('restart-button').onclick = this.restartGame.bind(this);
    }

    /**
     * カードセットを新しく読み込んでゲームを再スタートします。
     */
    private restartGame(): void {
        this.gameController.restartGame();
    }
}