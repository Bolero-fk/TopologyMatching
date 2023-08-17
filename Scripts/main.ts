import { GameEngine } from './gameEngine.js';

// ゲームに配置するカードの枚数, ROW*COLUMNの値が偶数になるようにする
const ROW = 4;
const COLUMN = 5;

const IMAGE_FOLDER_PATH = './TopologyCards/images/';
const JSON_PATH = './TopologyCards/cards.json';

const FLIPPING_WAIT_TIME_MILLISECONDS = 1000;

// FIXME: 現状の実装では選択可能枚数が2枚の時のみ実装されている
const MAX_SELECTABLE_CARD = 2;

enum FlipStatus {
    Front,
    Back,
}

class Card {
    private element: HTMLElement;
    private flipStatus: FlipStatus;
    private matchingKey: string;
    private frontImageUrl: string;

    constructor(element: HTMLElement) {
        this.element = element;
        this.flipStatus = FlipStatus.Back;
        this.element.onclick = () => {
            this.onClick();
        };
    }

    /**
     * カードを変更します
     * @param matchingKey カードの種類を指定するキー
     * @param imageName カードの表面に表示する画像のurl
     */
    changeCard(matchingKey: string, imageName: string): void {
        this.matchingKey = matchingKey;
        this.frontImageUrl = 'url(' + IMAGE_FOLDER_PATH + imageName + ')';
    }

    /**
     * 入力されたカードを指定された方向に返します
     */
    flipCard(flipStatus: FlipStatus): void {
        this.flipStatus = flipStatus;

        // カードの面ごとに色と画像を設定する
        if (this.flipStatus == FlipStatus.Front) {
            this.element.style.backgroundColor = getComputedStyle(this.element).getPropertyValue("--front-background-color");
            this.element.style.backgroundImage = this.frontImageUrl;
        }
        else {
            this.element.style.backgroundColor = getComputedStyle(this.element).getPropertyValue("--back-background-color");
            this.element.style.backgroundImage = '';
        }
    }

    /**
     * カードクリック時の挙動を定義します
     */
    private onClick(): void {
        if (this.flipStatus == FlipStatus.Front) {
            return;
        }
        else if (MAX_SELECTABLE_CARD <= selectedCards.length) {
            return;
        }

        this.flipCard(FlipStatus.Front);
        selectedCards.push(this);

        if (MAX_SELECTABLE_CARD <= selectedCards.length) {
            if (selectedCards[0].matchingKey == selectedCards[1].matchingKey) {
                selectedCards.length = 0;
            } else {
                setTimeout(() => {
                    for (let card of selectedCards) {
                        card.flipCard(FlipStatus.Back);
                    }

                    selectedCards.length = 0;
                }, FLIPPING_WAIT_TIME_MILLISECONDS);
            }
        }
    }
}

const cardsOnBoard: Card[] = [];
const selectedCards: Card[] = [];

window.onload = () => {
    initializeElements();
};

/**
 * html上に配置する要素を初期化します
 */
function initializeElements(): void {
    initializeGameBoardElement();
    initializeRestartGemeButtonElement();
}

/**
 * game boardを初期化します
 */
function initializeGameBoardElement(): void {
    const gameBoard = document.getElementById('game-board');

    // カードの行と列の枚数を指定する
    gameBoard.style.setProperty('--cols', String(COLUMN));
    gameBoard.style.setProperty('--rows', String(ROW));

    initializeCardsOnBoardElement(gameBoard);
}

/**
 * game board上のカードを初期化します
 */
function initializeCardsOnBoardElement(gameBoard: HTMLElement): void {
    const gameEngine = new GameEngine(LoadTopologyCardsJson());
    const cardStatus = gameEngine.startGame(ROW * COLUMN);

    for (let i = 0; i < ROW * COLUMN; i++) {
        // カードを追加していく
        const cardElement = document.createElement('div');
        cardElement.className = 'card';
        gameBoard.appendChild(cardElement);

        const card: Card = new Card(cardElement);
        card.flipCard(FlipStatus.Back);
        card.changeCard(cardStatus[i].matchingKey, cardStatus[i].imageName);

        cardsOnBoard.push(card);
    }
}

/**
 * Restart Game ボタンを初期化します
 */
function initializeRestartGemeButtonElement(): void {
    document.getElementById('restart-button').onclick = RestartGame;
}

/**
 * トポロジーカードをjsonから読み込む
 */
function LoadTopologyCardsJson(): any {
    let result: any;
    $.ajax({
        url: JSON_PATH,
        dataType: "json",
        async: false,
        success: function (data) {
            result = data;
        }
    });

    return result;
}

/**
 * カードセットを新しく読み込んでゲームを再スタートします。
 */
function RestartGame(): void {
    const gameEngine = new GameEngine(LoadTopologyCardsJson());
    const cardStatus = gameEngine.startGame(ROW * COLUMN);

    for (let i = 0; i < cardStatus.length; i++) {
        cardsOnBoard[i].changeCard(cardStatus[i].matchingKey, cardStatus[i].imageName);
        cardsOnBoard[i].flipCard(FlipStatus.Back);
    }

    selectedCards.length = 0;
}
