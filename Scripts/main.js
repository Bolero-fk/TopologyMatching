import { GameEngine } from './gameEngine.js';
// ゲームに配置するカードの枚数, ROW*COLUMNの値が偶数になるようにする
const ROW = 4;
const COLUMN = 5;
const IMAGE_FOLDER_PATH = './TopologyCards/images/';
const JSON_PATH = './TopologyCards/cards.json';
const FLIPPING_WAIT_TIME_MILLISECONDS = 1000;
// FIXME: 現状の実装では選択可能枚数が2枚の時のみ実装されている
const MAX_SELECTABLE_CARD = 2;
var FlipStatus;
(function (FlipStatus) {
    FlipStatus[FlipStatus["Front"] = 0] = "Front";
    FlipStatus[FlipStatus["Back"] = 1] = "Back";
})(FlipStatus || (FlipStatus = {}));
class Card {
    constructor(element) {
        this.element = element;
        this.flipStatus = FlipStatus.Back;
        this.element.onclick = () => {
            this.onClick();
        };
    }
    changeCard(pairKey, imageName) {
        this.pairKey = pairKey;
        this.frontImageUrl = 'url(' + IMAGE_FOLDER_PATH + imageName + ')';
    }
    /**
     * 入力されたカードを指定された方向に返します
     * @returns
     */
    flipCard(flipStatus) {
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
    onClick() {
        if (this.flipStatus == FlipStatus.Front) {
            return;
        }
        this.flipCard(FlipStatus.Front);
        selectedCards.push(this);
        if (MAX_SELECTABLE_CARD <= selectedCards.length) {
            if (selectedCards[0].pairKey == selectedCards[1].pairKey) {
                selectedCards = [];
            }
            else {
                setTimeout(() => {
                    for (let card of selectedCards) {
                        card.flipCard(FlipStatus.Back);
                    }
                    selectedCards = [];
                }, FLIPPING_WAIT_TIME_MILLISECONDS);
            }
        }
    }
}
let cardsOnBoard = [];
let selectedCards = [];
window.onload = () => {
    initializeElements();
};
function initializeElements() {
    initializeGameBoardElement();
    initializeRestartGemeButtonElement();
}
function initializeGameBoardElement() {
    const gameBoard = document.getElementById('game-board');
    // カードの行と列の枚数を指定する
    gameBoard.style.setProperty('--cols', String(COLUMN));
    gameBoard.style.setProperty('--rows', String(ROW));
    initializeCardsOnBoardElement(gameBoard);
}
function initializeCardsOnBoardElement(gameBoard) {
    const gameEngine = new GameEngine(LoadTopologyCardsJson(JSON_PATH));
    const cardStatus = gameEngine.startGame(ROW * COLUMN);
    for (let i = 0; i < ROW * COLUMN; i++) {
        // カードを追加していく
        const cardElement = document.createElement('div');
        cardElement.className = 'card';
        gameBoard.appendChild(cardElement);
        const card = new Card(cardElement);
        card.flipCard(FlipStatus.Back);
        card.changeCard(cardStatus[i].pairKey, cardStatus[i].imageName);
        cardsOnBoard.push(card);
    }
}
function initializeRestartGemeButtonElement() {
    document.getElementById('restart-button').onclick = RestartGame;
}
// トポロジーカードを読み込む
function LoadTopologyCardsJson(url) {
    let result;
    $.ajax({
        url: url,
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
    const gameEngine = new GameEngine(LoadTopologyCardsJson(JSON_PATH));
    const cardStatus = gameEngine.startGame(ROW * COLUMN);
    for (let i = 0; i < cardStatus.length; i++) {
        cardsOnBoard[i].changeCard(cardStatus[i].pairKey, cardStatus[i].imageName);
        cardsOnBoard[i].flipCard(FlipStatus.Back);
    }
    selectedCards = [];
}
