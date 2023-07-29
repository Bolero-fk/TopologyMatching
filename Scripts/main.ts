interface Card {
    element: HTMLElement;
    revealed: boolean;
    pairId: number;
}

let cards: Card[] = [];
let revealedCards: Card[] = [];

function shuffleArray(array: any[]) {
    for (let i = array.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [array[i], array[j]] = [array[j], array[i]];
    }
}

window.onload = () => {
    const gameBoard = document.getElementById('game-board');
    const resetButton = document.getElementById('reset-button');

    const pairIds = [1, 1, 2, 2];  // For this simple game, we have 2 pairs.
    shuffleArray(pairIds);

    for (let i = 0; i < 4; i++) {
        const cardElement = document.createElement('div');
        cardElement.className = 'card';
        if (gameBoard)
            gameBoard.appendChild(cardElement);

        const card: Card = {
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
                } else {
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

    if (resetButton)
        resetButton.onclick = () => {
            for (let card of cards) {
                card.revealed = false;
                card.element.textContent = '';
            }

            revealedCards = [];

            shuffleArray(pairIds);
            for (let i = 0; i < 4; i++) {
                cards[i].pairId = pairIds[i];
            }
        };
};
