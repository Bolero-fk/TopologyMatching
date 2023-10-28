import { GameEngine } from './gameEngine.js';
import { Card, FlipStatus } from './card.js';
import { CardDom } from './cardDom.js';

// ゲームに配置するカードの枚数, ROW*COLUMNの値が偶数になるようにする
// FIXME: jsonに記されたカードのペアがROW * COLUMN以下のときに落ちるので注意する
const ROW = 4;
const COLUMN = 5;

const JSON_PATH = './TopologyCards/cards.json';

const FLIPPING_WAIT_TIME_MILLISECONDS = 1000;

// FIXME: 現状の実装では選択可能枚数が2枚の時のみ実装されている
const MAX_SELECTABLE_CARD = 2;

const cardsOnBoard: Card[] = [];
const selectedCards: Card[] = [];

function cardClickedCallback(card: Card): void {
    if (MAX_SELECTABLE_CARD <= selectedCards.length) {
        return;
    }

    card.flipCard(FlipStatus.Front);
    selectedCards.push(card);

    if (MAX_SELECTABLE_CARD <= selectedCards.length) {
        if (selectedCards[0].matchingKey == selectedCards[1].matchingKey) {
            selectedCards.length = 0;
        } else {
            setTimeout(() => {
                flipSelectedCards();
                selectedCards.length = 0;
            }, FLIPPING_WAIT_TIME_MILLISECONDS);
        }
    }
}

function flipSelectedCards() {
    selectedCards.forEach(selectedCard => {
        selectedCard.flipCard(FlipStatus.Back);
    });
};

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

        const frontBackgroundColor = getComputedStyle(cardElement).getPropertyValue("--front-background-color");
        const backBackgroundColor = getComputedStyle(cardElement).getPropertyValue("--back-background-color");

        const card: Card = new Card(new CardDom(cardElement), () => cardClickedCallback(card), frontBackgroundColor, backBackgroundColor);
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
