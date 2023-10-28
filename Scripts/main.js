"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var gameEngine_1 = require("./gameEngine");
var card_1 = require("./card");
// ゲームに配置するカードの枚数, ROW*COLUMNの値が偶数になるようにする
// FIXME: jsonに記されたカードのペアがROW * COLUMN以下のときに落ちるので注意する
var ROW = 4;
var COLUMN = 5;
var JSON_PATH = './TopologyCards/cards.json';
var FLIPPING_WAIT_TIME_MILLISECONDS = 1000;
// FIXME: 現状の実装では選択可能枚数が2枚の時のみ実装されている
var MAX_SELECTABLE_CARD = 2;
var cardsOnBoard = [];
var selectedCards = [];
function cardClickedCallback(card) {
    if (MAX_SELECTABLE_CARD <= selectedCards.length) {
        return;
    }
    card.flipCard(card_1.FlipStatus.Front);
    selectedCards.push(card);
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
}
function flipSelectedCards() {
    selectedCards.forEach(function (selectedCard) {
        selectedCard.flipCard(card_1.FlipStatus.Back);
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
    var gameEngine = new gameEngine_1.GameEngine(LoadTopologyCardsJson());
    var cardStatus = gameEngine.startGame(ROW * COLUMN);
    var _loop_1 = function (i) {
        // カードを追加していく
        var cardElement = document.createElement('div');
        cardElement.className = 'card';
        gameBoard.appendChild(cardElement);
        var card = new card_1.Card(cardElement, function () { return cardClickedCallback(card); });
        card.flipCard(card_1.FlipStatus.Back);
        card.changeCard(cardStatus[i].matchingKey, cardStatus[i].imageName);
        cardsOnBoard.push(card);
    };
    for (var i = 0; i < ROW * COLUMN; i++) {
        _loop_1(i);
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
    var gameEngine = new gameEngine_1.GameEngine(LoadTopologyCardsJson());
    var cardStatus = gameEngine.startGame(ROW * COLUMN);
    for (var i = 0; i < cardStatus.length; i++) {
        cardsOnBoard[i].changeCard(cardStatus[i].matchingKey, cardStatus[i].imageName);
        cardsOnBoard[i].flipCard(card_1.FlipStatus.Back);
    }
    selectedCards.length = 0;
}
