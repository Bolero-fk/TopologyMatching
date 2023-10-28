"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var gameEngine_js_1 = require("./gameEngine.js");
// ゲームに配置するカードの枚数, ROW*COLUMNの値が偶数になるようにする
// FIXME: jsonに記されたカードのペアがROW * COLUMN以下のときに落ちるので注意する
var ROW = 4;
var COLUMN = 5;
var IMAGE_FOLDER_PATH = './TopologyCards/images/';
var JSON_PATH = './TopologyCards/cards.json';
var FLIPPING_WAIT_TIME_MILLISECONDS = 1000;
// FIXME: 現状の実装では選択可能枚数が2枚の時のみ実装されている
var MAX_SELECTABLE_CARD = 2;
var FlipStatus;
(function (FlipStatus) {
    FlipStatus[FlipStatus["Front"] = 0] = "Front";
    FlipStatus[FlipStatus["Back"] = 1] = "Back";
})(FlipStatus || (FlipStatus = {}));
var Card = /** @class */ (function () {
    function Card(element) {
        var _this = this;
        this.element = element;
        this.flipStatus = FlipStatus.Back;
        this.element.onclick = function () {
            _this.onClick();
        };
    }
    /**
     * カードを変更します
     * @param matchingKey カードの種類を指定するキー
     * @param imageName カードの表面に表示する画像のurl
     */
    Card.prototype.changeCard = function (matchingKey, imageName) {
        this.matchingKey = matchingKey;
        this.frontImageUrl = 'url(' + IMAGE_FOLDER_PATH + imageName + ')';
    };
    /**
     * 入力されたカードを指定された方向に返します
     */
    Card.prototype.flipCard = function (flipStatus) {
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
    };
    /**
     * カードクリック時の挙動を定義します
     */
    Card.prototype.onClick = function () {
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
            }
            else {
                setTimeout(function () {
                    flipSelectedCards();
                    selectedCards.length = 0;
                }, FLIPPING_WAIT_TIME_MILLISECONDS);
            }
        }
    };
    return Card;
}());
var cardsOnBoard = [];
var selectedCards = [];
function flipSelectedCards() {
    selectedCards.forEach(function (selectedCard) {
        selectedCard.flipCard(FlipStatus.Back);
    });
}
;
window.onload = function () {
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
    var gameBoard = document.getElementById('game-board');
    // カードの行と列の枚数を指定する
    gameBoard.style.setProperty('--cols', String(COLUMN));
    gameBoard.style.setProperty('--rows', String(ROW));
    initializeCardsOnBoardElement(gameBoard);
}
/**
 * game board上のカードを初期化します
 */
function initializeCardsOnBoardElement(gameBoard) {
    var gameEngine = new gameEngine_js_1.GameEngine(LoadTopologyCardsJson());
    var cardStatus = gameEngine.startGame(ROW * COLUMN);
    for (var i = 0; i < ROW * COLUMN; i++) {
        // カードを追加していく
        var cardElement = document.createElement('div');
        cardElement.className = 'card';
        gameBoard.appendChild(cardElement);
        var card = new Card(cardElement);
        card.flipCard(FlipStatus.Back);
        card.changeCard(cardStatus[i].matchingKey, cardStatus[i].imageName);
        cardsOnBoard.push(card);
    }
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
    var result;
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
    var gameEngine = new gameEngine_js_1.GameEngine(LoadTopologyCardsJson());
    var cardStatus = gameEngine.startGame(ROW * COLUMN);
    for (var i = 0; i < cardStatus.length; i++) {
        cardsOnBoard[i].changeCard(cardStatus[i].matchingKey, cardStatus[i].imageName);
        cardsOnBoard[i].flipCard(FlipStatus.Back);
    }
    selectedCards.length = 0;
}
