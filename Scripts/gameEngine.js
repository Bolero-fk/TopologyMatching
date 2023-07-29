class CardStatus {
    constructor(imageName, holeCount) {
        this.imageName = imageName;
        this.holeCount = holeCount;
        this.complexityLevel = holeCount.reduce((sum, curr) => sum + curr, 0) + holeCount.length;
        this.pairKey = holeCount.toString();
    }
}
;
export class GameEngine {
    // コンストラクター、初期化処理を行う
    constructor(topologyCards) {
        // メンバー変数、ゲームの状態を保持
        this.cards = new Array(); // Cardはカードを表現する型です
        this.sortedCardWithcomplexityLevel = new Map();
        topologyCards.forEach(topologyCard => {
            const card = new CardStatus(topologyCard.ImageName, topologyCard.HoleCount);
            this.cards.push(card);
            if (!this.sortedCardWithcomplexityLevel.has(card.holeCount.toString()))
                this.sortedCardWithcomplexityLevel.set(card.holeCount.toString(), []);
            this.sortedCardWithcomplexityLevel.get(card.holeCount.toString()).push(card);
        });
        this.sortedCardWithcomplexityLevel.forEach((value, key) => {
            console.log(value);
        });
        console.log(this.sortedCardWithcomplexityLevel.size);
    }
    // ゲーム開始時の初期化処理
    startGame(cardNum) {
        this.selectedCards = this.shuffleArray(this.cards);
        while (this.selectCard.length > cardNum) {
            this.selectedCards.pop();
        }
        return this.selectedCards;
    }
    // カードを選択したときの処理
    selectCard() {
        // ここで選択したカードの処理を行います。
    }
    // ゲームが終了したかをチェックする
    checkIfGameEnded() {
        // ゲームが終了したかどうかをチェックし、結果を返します。
        return true;
    }
    shuffleArray(array) {
        const newArray = array.slice(); // 元の配列を破壊しないためにコピーを作成
        for (let i = newArray.length - 1; i > 0; i--) {
            const j = Math.floor(Math.random() * (i + 1));
            [newArray[i], newArray[j]] = [newArray[j], newArray[i]]; // 要素の入れ替え
        }
        return newArray;
    }
}
