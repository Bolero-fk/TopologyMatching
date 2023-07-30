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

        const deleteKeys: string[] = [];
        this.sortedCardWithcomplexityLevel.forEach((value, key) => {
            if (value.length % 2 == 1) {
                value.pop();
            }
            if (value.length == 0)
                deleteKeys.push(key);
        });

        for (const key of deleteKeys) {
            this.sortedCardWithcomplexityLevel.delete(key);
        }
    }

    // ゲーム開始時の初期化処理
    startGame(cardNum: number): CardStatus[] {
        this.selectedCards = new Array();

        for (let i = 0; i < cardNum / 2; i++) {
            this.selectedCards.push(...this.getAndDeleteRandomTwoCard());
        }

        this.selectedCards = this.shuffleArray(this.selectedCards);

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

    getAndDeleteRandomTwoCard(): CardStatus[] {
        const keysArray = new Array<string>();

        for (const key of this.sortedCardWithcomplexityLevel.keys()) {
            const length: number = this.sortedCardWithcomplexityLevel.get(key).length;
            keysArray.push(... new Array(length).fill(key));
        }

        Array.from(this.sortedCardWithcomplexityLevel.keys());
        const randomIndex = Math.floor(Math.random() * keysArray.length);
        const randomKey = keysArray[randomIndex];

        const result = this.sortedCardWithcomplexityLevel.get(randomKey).splice(-2);

        if (this.sortedCardWithcomplexityLevel.get(randomKey).length == 0)
            this.sortedCardWithcomplexityLevel.delete(randomKey);

        return result;
    }
}
