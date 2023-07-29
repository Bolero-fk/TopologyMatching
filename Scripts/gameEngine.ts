class CardStatus {
    imageName: string;
    holeCount: number[];
    complexityLevel: number;
    pairId: number;

    constructor(imageName: string, holeCount: number[]) {
        this.imageName = imageName;
        this.holeCount = holeCount;
        this.complexityLevel = holeCount.reduce((sum, curr) => sum + curr, 0) + holeCount.length;
    }
};

export class GameEngine {

    // メンバー変数、ゲームの状態を保持
    private cards: CardStatus[] = new Array(); // Cardはカードを表現する型です

    private sortedCardWithcomplexityLevel: Map<number, CardStatus[]> = new Map();
    private selectedCards: CardStatus[];

    // コンストラクター、初期化処理を行う
    constructor(topologyCards) {
        topologyCards.forEach(topologyCard => {
            const card = new CardStatus(topologyCard.ImageName, topologyCard.HoleCount);
            this.cards.push(card);

            if (!this.sortedCardWithcomplexityLevel.has(card.complexityLevel))
                this.sortedCardWithcomplexityLevel.set(card.complexityLevel, []);

            this.sortedCardWithcomplexityLevel.get(card.complexityLevel).push(card);
        });

        this.sortedCardWithcomplexityLevel = new Map([...this.sortedCardWithcomplexityLevel.entries()].sort((a, b) => a[0] - b[0]));
    }

    // ゲーム開始時の初期化処理
    startGame() {
        // ここでカードをシャッフルし、ゲームボードをレンダリングします。
    }

    // カードを選択したときの処理
    selectCard() {
        // ここで選択したカードの処理を行います。
    }

    // ゲームが終了したかをチェックする
    checkIfGameEnded(): boolean {
        // ゲームが終了したかどうかをチェックし、結果を返します。
        return true;
    }
}
