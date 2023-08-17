class CardStatus {
    imageName: string;
    pairKey: string;

    constructor(imageName: string, holeCount: number[]) {
        this.imageName = imageName;
        this.pairKey = holeCount.toString();
    }
};

export class GameEngine {

    private cards: CardStatus[];
    private cardGroups: Map<string, CardStatus[]>;

    // コンストラクター、初期化処理を行う
    constructor(topologyCards: any[]) {
        this.cards = new Array();
        this.cardGroups = new Map();
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
            selectedCards.push(...this.spliceRandomTwoCard());
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
        const shuffledArray = array.slice(); // 元の配列を破壊しないためにコピーを作成

        for (let i = shuffledArray.length - 1; i > 0; i--) {
            const j = Math.floor(Math.random() * (i + 1));
            [shuffledArray[i], shuffledArray[j]] = [shuffledArray[j], shuffledArray[i]]; // 要素の入れ替え
        }

        return shuffledArray;
    }

    /**
     * cardGroupsからランダムに二枚取得し、それらをcardGroupsから削除します
     * @returns 取得したカード
     */
    spliceRandomTwoCard(): CardStatus[] {
        // cardGroupsから各グループのカードの数の分だけキーを抜き出す
        const keysArray = new Array<string>();

        for (const key of this.cardGroups.keys()) {
            const length: number = this.cardGroups.get(key).length;
            keysArray.push(... new Array(length).fill(key));
        }

        const randomIndex = Math.floor(Math.random() * keysArray.length);
        const randomKey = keysArray[randomIndex];

        return this.cardGroups.get(randomKey).splice(-2);
    }

    /**
     * cardGroupsを初期化します
     */
    initializeCardGroups(): void {
        this.cardGroups = new Map<string, CardStatus[]>;

        // cardsをholeCountごとにまとめる
        this.cards.forEach(card => {
            if (!this.cardGroups.has(card.pairKey))
                this.cardGroups.set(card.pairKey, []);

            this.cardGroups.get(card.pairKey).push(card);
        });

        // それぞれのcardGroupをシャッフルする
        this.cardGroups.forEach((cardGroup, key) => {
            this.cardGroups.set(key, this.shuffleArray(cardGroup));
        });

        // holeCountごとに偶数になるように各groupの枚数を調整する
        this.cardGroups.forEach((value, key) => {
            if (value.length % 2 == 1) {
                value.pop();
            }
        });
    }
}
