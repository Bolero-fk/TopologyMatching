import { GameEngine } from './gameEngine.js';

class Card {
    element: HTMLElement;
    isFlipped: boolean;
    pairKey: string;
    frontImageUrl: string;
}

let cards: Card[] = [];
let selectesCards: Card[] = [];

/**
 * 入力されたカードを裏返します
 * @param card 裏返したいカード
 * @param isFlipped どちらの面にするか
 * @returns 
 */
function flipCard(card: Card, isFlipped: boolean = undefined) {
    // カードの面を指定されていない場合は反転させる。
    // 指定されている場合はその面に反転する
    if (isFlipped === undefined)
        card.isFlipped = !card.isFlipped;
    else
        card.isFlipped = isFlipped;

    // カードの面ごとに色と画像を設定する
    if (card.isFlipped) {
        card.element.style.backgroundColor = getComputedStyle(card.element).getPropertyValue("--front-background-color");
        card.element.style.backgroundImage = card.frontImageUrl;
    }
    else {
        card.element.style.backgroundColor = getComputedStyle(card.element).getPropertyValue("--back-background-color");
        card.element.style.backgroundImage = '';
    }
}

// ゲームに配置するカードの枚数, ROW*COLUMNの値が偶数になるようにする
const ROW = 4;
const COLUMN = 5;

window.onload = () => {
    const gameBoard = document.getElementById('game-board');
    const resetButton = document.getElementById('reset-button');

    const gameEngine = new GameEngine(LoadTopologyCardsJson("./TopologyCards/cards.json"));
    const cardStatus = gameEngine.startGame(ROW * COLUMN);

    // カードの行と列の枚数を指定する
    gameBoard.style.setProperty('--cols', String(COLUMN));
    gameBoard.style.setProperty('--rows', String(ROW));

    for (let i = 0; i < ROW * COLUMN; i++) {
        // カードを追加していく
        const cardElement = document.createElement('div');
        cardElement.className = 'card';
        gameBoard.appendChild(cardElement);

        const card: Card = {
            element: cardElement,
            isFlipped: false,
            pairKey: cardStatus[i].pairKey,
            frontImageUrl: 'url(./TopologyCards/images/' + cardStatus[i].imageName + ')'
        };

        card.element.onclick = () => {
            if (card.isFlipped || selectesCards.length == 2) {
                return;
            }

            flipCard(card);
            selectesCards.push(card);

            if (selectesCards.length == 2) {
                if (selectesCards[0].pairKey == selectesCards[1].pairKey) {
                    selectesCards = [];
                } else {
                    setTimeout(() => {
                        for (let card of selectesCards) {
                            flipCard(card);
                        }

                        selectesCards = [];
                    }, 1000);
                }
            }
        };

        cards.push(card);
    }

    resetButton.onclick = RestartGame;
};

// トポロジーカードを読み込む
function LoadTopologyCardsJson(url: string): any {
    let result: any;
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
function RestartGame(): void {
    const gameEngine = new GameEngine(LoadTopologyCardsJson("./TopologyCards/cards.json"));
    const cardStatus = gameEngine.startGame(ROW * COLUMN);

    for (let i = 0; i < cardStatus.length; i++) {
        cards[i].pairKey = cardStatus[i].pairKey;
        cards[i].frontImageUrl = 'url(./TopologyCards/images/' + cardStatus[i].imageName + ')';
        flipCard(cards[i], false);
    }

    selectesCards = [];
}
