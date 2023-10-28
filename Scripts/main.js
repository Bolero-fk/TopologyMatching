import { CardDom } from './cardDom.js';
import { GameController } from './gameController.js';
// ゲームに配置するカードの枚数, ROW*COLUMNの値が偶数になるようにする
// FIXME: jsonに記されたカードのペアがROW * COLUMN以下のときに落ちるので注意する
const ROW = 4;
const COLUMN = 5;
const JSON_PATH = './TopologyCards/cards.json';
const IMAGE_FOLDER_PATH = './TopologyCards/images/';
const FLIPPING_WAIT_TIME_MILLISECONDS = 1000;
// FIXME: 現状の実装では選択可能枚数が2枚の時のみ実装されている
const MAX_SELECTABLE_CARD = 2;
let gameController = null;
window.onload = () => {
    initializeElements();
};
/**
 * html上に配置する要素を初期化します
 */
function initializeElements() {
    initializeGameBoardElement();
    initializeRestartGemeButtonElement();
}
/**
 * game boardを初期化します
 */
function initializeGameBoardElement() {
    const gameBoard = document.getElementById('game-board');
    // カードの行と列の枚数を指定する
    gameBoard.style.setProperty('--cols', String(COLUMN));
    gameBoard.style.setProperty('--rows', String(ROW));
    initializeCardsOnBoardElement(gameBoard);
}
/**
 * game board上のカードを初期化します
 */
function initializeCardsOnBoardElement(gameBoard) {
    gameController = new GameController(LoadTopologyCardsJson(), MAX_SELECTABLE_CARD, FLIPPING_WAIT_TIME_MILLISECONDS, IMAGE_FOLDER_PATH);
    const cardDoms = new Array();
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
function initializeRestartGemeButtonElement() {
    document.getElementById('restart-button').onclick = RestartGame;
}
/**
 * トポロジーカードをjsonから読み込む
 */
function LoadTopologyCardsJson() {
    let result;
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
function RestartGame() {
    gameController.restartGame();
}
