let cards = [];
let revealedCards = [];
function shuffleArray(array) {
    for (let i = array.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [array[i], array[j]] = [array[j], array[i]];
    }
}
function flipCard(card, flipstatus = undefined) {
    if (flipstatus === undefined)
        card.revealed = !card.revealed;
    else
        card.revealed = flipstatus;
    if (card.revealed) {
        console.log();
        card.element.style.backgroundColor = getComputedStyle(card.element).getPropertyValue("--front-background-color");
        card.element.textContent = String(card.pairId);
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
    // Set the CSS variables
    gameBoard.style.setProperty('--cols', String(COLUMN));
    gameBoard.style.setProperty('--rows', String(ROW));
    let pairIds = Array.from({ length: ROW * COLUMN / 2 }, (_, i) => i + 1); // Generate pair IDs
    pairIds = pairIds.concat(pairIds); // Duplicate the array to create pairs
    shuffleArray(pairIds);
    for (let i = 0; i < ROW * COLUMN; i++) {
        const cardElement = document.createElement('div');
        cardElement.className = 'card';
        gameBoard.appendChild(cardElement);
        const card = {
            element: cardElement,
            revealed: false,
            pairId: pairIds[i],
            frontImageUrl: 'url(./TopologyCards/images/U+22F1.svg)'
        };
        card.element.onclick = () => {
            if (card.revealed || revealedCards.length == 2) {
                return;
            }
            flipCard(card);
            revealedCards.push(card);
            if (revealedCards.length == 2) {
                if (revealedCards[0].pairId == revealedCards[1].pairId) {
                    revealedCards = [];
                }
                else {
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
    resetButton.onclick = () => {
        for (let card of cards) {
            flipCard(card, false);
        }
        revealedCards = [];
        shuffleArray(pairIds);
        for (let i = 0; i < ROW * COLUMN; i++) {
            cards[i].pairId = pairIds[i];
        }
    };
};
