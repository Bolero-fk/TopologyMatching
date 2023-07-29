let cards = [];
let revealedCards = [];
function shuffleArray(array) {
    for (let i = array.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [array[i], array[j]] = [array[j], array[i]];
    }
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
            pairId: pairIds[i]
        };
        card.element.onclick = () => {
            if (card.revealed || revealedCards.length == 2) {
                return;
            }
            card.revealed = true;
            card.element.textContent = String(card.pairId);
            revealedCards.push(card);
            if (revealedCards.length == 2) {
                if (revealedCards[0].pairId == revealedCards[1].pairId) {
                    revealedCards = [];
                }
                else {
                    setTimeout(() => {
                        for (let card of revealedCards) {
                            card.revealed = false;
                            card.element.textContent = '';
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
            card.revealed = false;
            card.element.textContent = '';
        }
        revealedCards = [];
        shuffleArray(pairIds);
        for (let i = 0; i < ROW * COLUMN; i++) {
            cards[i].pairId = pairIds[i];
        }
    };
};
