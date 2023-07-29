var cards = [];
var revealedCards = [];
function shuffleArray(array) {
    var _a;
    for (var i = array.length - 1; i > 0; i--) {
        var j = Math.floor(Math.random() * (i + 1));
        _a = [array[j], array[i]], array[i] = _a[0], array[j] = _a[1];
    }
}
window.onload = function () {
    var gameBoard = document.getElementById('game-board');
    var resetButton = document.getElementById('reset-button');
    var pairIds = [1, 1, 2, 2]; // For this simple game, we have 2 pairs.
    shuffleArray(pairIds);
    var _loop_1 = function (i) {
        var cardElement = document.createElement('div');
        cardElement.className = 'card';
        if (gameBoard)
            gameBoard.appendChild(cardElement);
        var card = {
            element: cardElement,
            revealed: false,
            pairId: pairIds[i]
        };
        card.element.onclick = function () {
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
                    setTimeout(function () {
                        for (var _i = 0, revealedCards_1 = revealedCards; _i < revealedCards_1.length; _i++) {
                            var card_1 = revealedCards_1[_i];
                            card_1.revealed = false;
                            card_1.element.textContent = '';
                        }
                        revealedCards = [];
                    }, 1000);
                }
            }
        };
        cards.push(card);
    };
    for (var i = 0; i < 4; i++) {
        _loop_1(i);
    }
    if (resetButton)
        resetButton.onclick = function () {
            for (var _i = 0, cards_1 = cards; _i < cards_1.length; _i++) {
                var card = cards_1[_i];
                card.revealed = false;
                card.element.textContent = '';
            }
            revealedCards = [];
            shuffleArray(pairIds);
            for (var i = 0; i < 4; i++) {
                cards[i].pairId = pairIds[i];
            }
        };
};
