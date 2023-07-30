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

    private cards: CardStatus[] = new Array();

    private cardGroups: Map<string, CardStatus[]> = new Map();

    // コンストラクター、初期化処理を行う
    constructor(topologyCards) {
        topologyCards.forEach(topologyCard => {
            const card = new CardStatus(topologyCard.ImageName, topologyCard.HoleCount);
            this.cards.push(card);
        });
    }

    // ゲーム開始時の初期化処理
    startGame(cardNum: number): CardStatus[] {
        this.initializeCardGroups();

        const selectedCards = new Array();

        for (let i = 0; i < cardNum / 2; i++) {
            // カードをランダムに2枚追加する
            selectedCards.push(...this.getAndDeleteRandomTwoCard());
        }

        // カードをシャッフルする
        return this.shuffleArray(selectedCards);
    }

    /**
     * 入力された配列をシャッフルしたものを返します
     * @param array - シャッフルしたい配列
     * @returns シャッフルされた配列
     */
    shuffleArray<T>(array: T[]): T[] {
        const newArray = array.slice(); // 元の配列を破壊しないためにコピーを作成

        for (let i = newArray.length - 1; i > 0; i--) {
            const j = Math.floor(Math.random() * (i + 1));
            [newArray[i], newArray[j]] = [newArray[j], newArray[i]]; // 要素の入れ替え
        }

        return newArray;
    }

    /**
     * cardGroupsからランダムに二枚取得し、それらをcardGroupsから削除します
     * @returns 取得したカード
     */
    getAndDeleteRandomTwoCard(): CardStatus[] {
        const keysArray = new Array<string>();

        for (const key of this.cardGroups.keys()) {
            const length: number = this.cardGroups.get(key).length;
            keysArray.push(... new Array(length).fill(key));
        }

        const randomIndex = Math.floor(Math.random() * keysArray.length);
        const randomKey = keysArray[randomIndex];

        const result = this.cardGroups.get(randomKey).splice(-2);

        if (this.cardGroups.get(randomKey).length == 0)
            this.cardGroups.delete(randomKey);

        return result;
    }

    /**
     * cardGroupsを初期化します
     */
    initializeCardGroups(): void {
        this.cardGroups = new Map<string, CardStatus[]>;

        // cardsをholeCountごとにまとめる
        this.cards.forEach(card => {
            if (!this.cardGroups.has(card.holeCount.toString()))
                this.cardGroups.set(card.holeCount.toString(), []);

            this.cardGroups.get(card.holeCount.toString()).push(card);
        });

        // holeCountごとに偶数になるように各groupの枚数を調整する
        const deleteKeys: string[] = [];
        this.cardGroups.forEach((value, key) => {
            if (value.length % 2 == 1) {
                value.pop();
            }
            if (value.length == 0)
                deleteKeys.push(key);
        });

        for (const key of deleteKeys) {
            this.cardGroups.delete(key);
        }

        // それぞれのcardGroupをシャッフルする
        this.cardGroups.forEach(cardGroup => {
            cardGroup = this.shuffleArray(cardGroup);
        });
    }
}
