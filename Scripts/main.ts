import { GameEngine } from './gameEngine.js';

interface Card {
    element: HTMLElement;
    revealed: boolean;
    pairKey: string;
    frontImageUrl: string;
}

let cards: Card[] = [];
let revealedCards: Card[] = [];

function shuffleArray(array: any[]) {
    for (let i = array.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [array[i], array[j]] = [array[j], array[i]];
    }
}

function flipCard(card: Card, flipstatus: boolean = undefined) {
    if (flipstatus === undefined)
        card.revealed = !card.revealed;
    else
        card.revealed = flipstatus;

    if (card.revealed) {
        card.element.style.backgroundColor = getComputedStyle(card.element).getPropertyValue("--front-background-color");
        card.element.style.backgroundImage = card.frontImageUrl;
    }
    else {
        card.element.style.backgroundColor = getComputedStyle(card.element).getPropertyValue("--back-background-color");
        card.element.textContent = '';
        card.element.style.backgroundImage = '';
    }
    return;
}

const ROW = 4; // Change these values to your preferred grid size
const COLUMN = 5;

window.onload = () => {
    const gameBoard = document.getElementById('game-board');
    const resetButton = document.getElementById('reset-button');

    const gameEngine = new GameEngine(LoadTopologyCardsJson("./TopologyCards/cards.json"));
    const cardStatus = gameEngine.startGame(ROW * COLUMN);

    // Set the CSS variables
    gameBoard.style.setProperty('--cols', String(COLUMN));
    gameBoard.style.setProperty('--rows', String(ROW));

    let pairIds = Array.from({ length: ROW * COLUMN / 2 }, (_, i) => i + 1);  // Generate pair IDs
    pairIds = pairIds.concat(pairIds);  // Duplicate the array to create pairs
    shuffleArray(pairIds);

    for (let i = 0; i < ROW * COLUMN; i++) {
        const cardElement = document.createElement('div');
        cardElement.className = 'card';
        gameBoard.appendChild(cardElement);

        const card: Card = {
            element: cardElement,
            revealed: false,
            pairKey: cardStatus[i].pairKey,
            frontImageUrl: 'url(./TopologyCards/images/' + cardStatus[i].imageName + ')'
        };

        card.element.onclick = () => {
            if (card.revealed || revealedCards.length == 2) {
                return;
            }

            flipCard(card);
            revealedCards.push(card);

            if (revealedCards.length == 2) {
                if (revealedCards[0].pairKey == revealedCards[1].pairKey) {
                    revealedCards = [];
                } else {
                    setTimeout(() => {
                        for (let card of revealedCards) {
                            flipCard(card);
                        }

                        revealedCards = [];
                    }, 1000);
                }
            }
        };

        cards.push(card);
    }

    resetButton.onclick = RestartGame;
};

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

function RestartGame(): void {
    const gameEngine = new GameEngine(LoadTopologyCardsJson("./TopologyCards/cards.json"));
    const cardStatus = gameEngine.startGame(ROW * COLUMN);

    for (let i = 0; i < cardStatus.length; i++) {
        cards[i].pairKey = cardStatus[i].pairKey;
        cards[i].frontImageUrl = 'url(./TopologyCards/images/' + cardStatus[i].imageName + ')';
        flipCard(cards[i], false);
    }

    revealedCards = [];
}
