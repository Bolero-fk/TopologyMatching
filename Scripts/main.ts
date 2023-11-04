import { CardDom } from './cardDom.js';
import { GameController } from './gameController.js';
import { TopologyCardJson } from './JsonType.js';

// ゲームに配置するカードの枚数, ROW*COLUMNの値が偶数になるようにする
// FIXME: jsonに記されたカードのペアがROW * COLUMN以下のときに落ちるので注意する
const ROW = 4;
const COLUMN = 5;

const JSON_PATH = './TopologyCards/cards.json';

const IMAGE_FOLDER_PATH = './TopologyCards/images/';

const FLIPPING_WAIT_TIME_MILLISECONDS = 1000;

// FIXME: 現状の実装では選択可能枚数が2枚の時のみ実装されている
const MAX_SELECTABLE_CARD = 2;

let gameController: GameController = null;

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
    gameController = new GameController(loadTopologyCardsJson(), MAX_SELECTABLE_CARD, FLIPPING_WAIT_TIME_MILLISECONDS, IMAGE_FOLDER_PATH);

    const cardDoms = new Array<CardDom>();

    for (let i = 0; i < ROW * COLUMN; i++) {
        const cardElement = document.createElement('div');
        cardElement.className = 'card';
        gameBoard.appendChild(cardElement);

        cardDoms.push(new CardDom(cardElement));
    }

    gameController.startGame(cardDoms);
}

/**
 * Restart Game ボタンを初期化します
 */
function initializeRestartGemeButtonElement(): void {
    document.getElementById('restart-button').onclick = restartGame;
}


function loadTopologyCardsJson(): any {
    let result: any;
    $.ajax({
        url: JSON_PATH,
        dataType: "json",
        async: false,
        success: function (data) {
            result = data as TopologyCardJson[];;
        }
    });

    return result;
}

/**
 * カードセットを新しく読み込んでゲームを再スタートします。
 */
function restartGame(): void {
    gameController.restartGame();
}
