"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.GameController = void 0;
var gameEngine_js_1 = require("./gameEngine.js");
var card_js_1 = require("./card.js");
var GameController = /** @class */ (function () {
    /**
     * GameControllerクラスのコンストラクタ
     *
     * @param {TopologyCardJson[]} topologyCardsJson - カード情報を持つJSON配列
     * @param {number} maxSelectableCard - 選択可能なカードの最大数
     * @param {number} flippingWaitTimeMilliseconds - カードを裏返す待機時間（ミリ秒）
     * @param {string} imageFolderPath - 画像のフォルダパス
     */
    function GameController(topologyCardsJson, maxSelectableCard, flippingWaitTimeMilliseconds, imageFolderPath) {
        this.validateInputs(topologyCardsJson, maxSelectableCard, flippingWaitTimeMilliseconds, imageFolderPath);
        this.gameEngine = new gameEngine_js_1.GameEngine(topologyCardsJson);
        this.cardsOnBoard = [];
        this.selectedCards = [];
        this.maxSelectableCard = maxSelectableCard;
        this.flippingWaitTimeMilliseconds = flippingWaitTimeMilliseconds;
        this.imageFolderPath = imageFolderPath;
    }
    /**
     * 入力の検証を行います
     *
     * @param {TopologyCardJson[]} topologyCardsJson - カード情報を持つJSON配列
     * @param {number} maxSelectableCard - 選択可能なカードの最大数
     * @param {number} flippingWaitTimeMilliseconds - カードを裏返す待機時間（ミリ秒）
     * @param {string} imageFolderPath - 画像のフォルダパス
     */
    GameController.prototype.validateInputs = function (topologyCardsJson, maxSelectableCard, flippingWaitTimeMilliseconds, imageFolderPath) {
        topologyCardsJson.forEach(function (item) {
            var keys = Object.keys(item);
            if (item.HoleCount.some(function (count) { return count < 0; })) {
                throw new Error('HoleCount must be a positive integer');
            }
            if (item.ImageName.trim() === '') {
                throw new Error('ImageName must be a non-empty string');
            }
        });
        if (maxSelectableCard <= 0) {
            throw new Error('maxSelectableCard must be a positive integer');
        }
        if (flippingWaitTimeMilliseconds <= 0) {
            throw new Error('flippingWaitTimeMilliseconds must be a positive integer');
        }
        if (imageFolderPath.trim() === '') {
            throw new Error('imageFolderPath must be a non-empty string');
        }
    };
    /**
     * ゲームを開始します
     *
     * @param {CardDom[]} cardDoms - カードのDOM表現の配列
     */
    GameController.prototype.startGame = function (cardDoms) {
        var _this = this;
        var gameCardNumber = cardDoms.length;
        var cardStatus = this.gameEngine.startGame(gameCardNumber);
        var _loop_1 = function (i) {
            var card = new card_js_1.Card(cardDoms[i], cardStatus[i].matchingKey, this_1.getImagePath(cardStatus[i].imageName), function () { return _this.cardClickedCallback(card); });
            this_1.cardsOnBoard.push(card);
        };
        var this_1 = this;
        for (var i = 0; i < gameCardNumber; i++) {
            _loop_1(i);
        }
    };
    /**
     * ゲームに配置されたカードの枚数を返します
     *
     * @returns {number} ゲームに配置されたカードの枚数
     */
    GameController.prototype.getCardNumebr = function () {
        return this.cardsOnBoard.length;
    };
    /**
     * カードがクリックされたときのコールバック関数
     *
     * @param {Card} card - クリックされたカード
     */
    GameController.prototype.cardClickedCallback = function (card) {
        var _this = this;
        if (this.maxSelectableCard <= this.selectedCards.length) {
            return;
        }
        card.flipCard(card_js_1.FlipStatus.Front);
        this.selectedCards.push(card);
        if (this.maxSelectableCard <= this.selectedCards.length) {
            if (this.selectedCards.every(function (card) { return card_js_1.Card.canMatchCard(card, _this.selectedCards[0]); })) {
                this.selectedCards.length = 0;
            }
            else {
                setTimeout(function () {
                    _this.flipSelectedCardsToBack();
                    _this.selectedCards.length = 0;
                }, this.flippingWaitTimeMilliseconds);
            }
        }
    };
    /**
     * 選択されたカードを裏返します
     */
    GameController.prototype.flipSelectedCardsToBack = function () {
        this.selectedCards.forEach(function (selectedCard) {
            selectedCard.flipCard(card_js_1.FlipStatus.Back);
        });
    };
    /**
     * ゲームを再開します
     */
    GameController.prototype.restartGame = function () {
        var _this = this;
        var cardStatus = this.gameEngine.startGame(this.cardsOnBoard.length);
        var _loop_2 = function (i) {
            this_2.cardsOnBoard[i] = this_2.cardsOnBoard[i].cloneWithNewImage(cardStatus[i].matchingKey, this_2.getImagePath(cardStatus[i].imageName), function () { return _this.cardClickedCallback(_this.cardsOnBoard[i]); });
        };
        var this_2 = this;
        for (var i = 0; i < cardStatus.length; i++) {
            _loop_2(i);
        }
        this.selectedCards.length = 0;
    };
    /**
     * 画像のパスを取得します
     *
     * @param {string} imageFileName - 画像のファイル名
     * @returns {string} 画像の完全なパス
     */
    GameController.prototype.getImagePath = function (imageFileName) {
        return "url(".concat(this.imageFolderPath).concat(imageFileName, ")");
    };
    return GameController;
}());
exports.GameController = GameController;
