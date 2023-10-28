"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.GameEngine = void 0;
var CardStatus = /** @class */ (function () {
    function CardStatus(imageName, holeCount) {
        this._imageName = imageName;
        this._matchingKey = holeCount.toString();
    }
    Object.defineProperty(CardStatus.prototype, "imageName", {
        get: function () {
            return this._imageName;
        },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(CardStatus.prototype, "matchingKey", {
        get: function () {
            return this._matchingKey;
        },
        enumerable: false,
        configurable: true
    });
    return CardStatus;
}());
;
var GameEngine = /** @class */ (function () {
    // コンストラクター、初期化処理を行う
    function GameEngine(topologyCards) {
        var _this = this;
        this.cards = new Array();
        topologyCards.forEach(function (topologyCard) {
            var card = new CardStatus(topologyCard.ImageName, topologyCard.HoleCount);
            _this.cards.push(card);
        });
    }
    /**
     * 神経衰弱ゲームに使用するカードを取得します。
     * @param cardNum - 神経衰弱ゲームにつかうカードの枚数
     * @returns 神経衰弱ゲームに使用できるカードセット
     */
    GameEngine.prototype.startGame = function (cardNum) {
        var cardGroups = this.initializeCardGroups();
        var selectedCards = new Array();
        for (var i = 0; i < cardNum / 2; i++) {
            // カードをランダムに2枚追加する
            selectedCards.push.apply(selectedCards, this.spliceRandomTwoCard(cardGroups));
        }
        // カードをシャッフルする
        return this.shuffleArray(selectedCards);
    };
    /**
     * 入力された配列をシャッフルしたものを返します
     * @param array - シャッフルしたい配列
     * @returns シャッフルされた配列
     */
    GameEngine.prototype.shuffleArray = function (array) {
        var _a;
        var shuffledArray = array.slice(); // 元の配列を破壊しないためにコピーを作成
        for (var i = shuffledArray.length - 1; i > 0; i--) {
            var j = Math.floor(Math.random() * (i + 1));
            _a = [shuffledArray[j], shuffledArray[i]], shuffledArray[i] = _a[0], shuffledArray[j] = _a[1]; // 要素の入れ替え
        }
        return shuffledArray;
    };
    /**
     * cardGroupsからランダムに二枚取得し、それらをcardGroupsから削除します
     * @returns 取得したカード
     */
    GameEngine.prototype.spliceRandomTwoCard = function (cardGroups) {
        // cardGroupsから各グループのカードの数の分だけキーを抜き出す
        var keysArray = new Array();
        for (var _i = 0, _a = Array.from(cardGroups.keys()); _i < _a.length; _i++) {
            var key = _a[_i];
            var length_1 = cardGroups.get(key).length;
            keysArray.push.apply(keysArray, new Array(length_1).fill(key));
        }
        var randomIndex = Math.floor(Math.random() * keysArray.length);
        var randomKey = keysArray[randomIndex];
        return cardGroups.get(randomKey).splice(-2);
    };
    /**
     * cardGroupsを初期化します
     * @returns 初期化されたcardGroups
     */
    GameEngine.prototype.initializeCardGroups = function () {
        var _this = this;
        var cardGroups = new Map;
        // cardsをholeCountごとにまとめる
        this.cards.forEach(function (card) {
            if (!cardGroups.has(card.matchingKey)) {
                cardGroups.set(card.matchingKey, []);
            }
            cardGroups.get(card.matchingKey).push(card);
        });
        // それぞれのcardGroupをシャッフルする
        cardGroups.forEach(function (cardGroup, key) {
            cardGroups.set(key, _this.shuffleArray(cardGroup));
        });
        // holeCountごとに偶数になるように各groupの枚数を調整する
        cardGroups.forEach(function (value, key) {
            if (value.length % 2 == 1) {
                value.pop();
            }
        });
        return cardGroups;
    };
    return GameEngine;
}());
exports.GameEngine = GameEngine;
