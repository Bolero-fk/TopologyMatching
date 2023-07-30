class CardStatus {
    imageName: string;
    holeCount: number[];
    complexityLevel: number;
    pairKey: string;

    constructor(imageName: string, holeCount: number[]) {
        this.imageName = imageName;
        this.holeCount = holeCount;
        this.complexityLevel = holeCount.reduce((sum, curr) => sum + curr, 0) + holeCount.length;
        this.pairKey = holeCount.toString();
    }
};

export class GameEngine {

    // メンバー変数、ゲームの状態を保持
    private cards: CardStatus[] = new Array(); // Cardはカードを表現する型です

    private sortedCardWithcomplexityLevel: Map<string, CardStatus[]> = new Map();
    private selectedCards: CardStatus[];

    // コンストラクター、初期化処理を行う
    constructor(topologyCards) {
        topologyCards.forEach(topologyCard => {
            const card = new CardStatus(topologyCard.ImageName, topologyCard.HoleCount);
            this.cards.push(card);

            if (!this.sortedCardWithcomplexityLevel.has(card.holeCount.toString()))
                this.sortedCardWithcomplexityLevel.set(card.holeCount.toString(), []);

            this.sortedCardWithcomplexityLevel.get(card.holeCount.toString()).push(card);
        });

        this.sortedCardWithcomplexityLevel.forEach((value, key) => {
            if (value.length % 2 == 1)
                console.log(key);
        });
    }

    // ゲーム開始時の初期化処理
    startGame(cardNum: number): CardStatus[] {
        this.selectedCards = this.shuffleArray(this.cards);

        while (this.selectedCards.length > cardNum) {
            this.selectedCards.pop();
        }

        return this.selectedCards;
    }

    shuffleArray<T>(array: T[]): T[] {
        const newArray = array.slice(); // 元の配列を破壊しないためにコピーを作成

        for (let i = newArray.length - 1; i > 0; i--) {
            const j = Math.floor(Math.random() * (i + 1));
            [newArray[i], newArray[j]] = [newArray[j], newArray[i]]; // 要素の入れ替え
        }

        return newArray;
    }
}
