import { GameEngine } from './gameEngine.js';
// ゲームに配置するカードの枚数, ROW*COLUMNの値が偶数になるようにする
const ROW = 4;
const COLUMN = 5;
const IMAGE_FOLDER_PATH = './TopologyCards/images/';
const JSON_PATH = './TopologyCards/cards.json';
const FLIPPING_WAIT_TIME_MILLISECONDS = 1000;
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
     * 入力されたカードを裏返します
     * @param isFlipped どちらの面にするか(指定しない場合は現在と逆の状態にする)
     * @returns
     */
    flipCard(flipStatus) {
        // カードの面を指定されていない場合は反転させる。
        // 指定されている場合はその面に反転する
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
        if (this.flipStatus == FlipStatus.Front || selectesCards.length == 2) {
            return;
        }
        this.flipCard(FlipStatus.Front);
        selectesCards.push(this);
        if (selectesCards.length == 2) {
            if (selectesCards[0].pairKey == selectesCards[1].pairKey) {
                selectesCards = [];
            }
            else {
                setTimeout(() => {
                    for (let card of selectesCards) {
                        card.flipCard(FlipStatus.Back);
                    }
                    selectesCards = [];
                }, FLIPPING_WAIT_TIME_MILLISECONDS);
            }
        }
    }
}
let cards = [];
let selectesCards = [];
window.onload = () => {
    const gameBoard = document.getElementById('game-board');
    const resetButton = document.getElementById('reset-button');
    const gameEngine = new GameEngine(LoadTopologyCardsJson(JSON_PATH));
    const cardStatus = gameEngine.startGame(ROW * COLUMN);
    // カードの行と列の枚数を指定する
    gameBoard.style.setProperty('--cols', String(COLUMN));
    gameBoard.style.setProperty('--rows', String(ROW));
    for (let i = 0; i < ROW * COLUMN; i++) {
        // カードを追加していく
        const cardElement = document.createElement('div');
        cardElement.className = 'card';
        gameBoard.appendChild(cardElement);
        const card = new Card(cardElement);
        card.flipCard(FlipStatus.Back);
        card.changeCard(cardStatus[i].pairKey, cardStatus[i].imageName);
        cards.push(card);
    }
    resetButton.onclick = RestartGame;
};
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
        cards[i].changeCard(cardStatus[i].pairKey, cardStatus[i].imageName);
        cards[i].flipCard(FlipStatus.Back);
    }
    selectesCards = [];
}
