import { TopologyCardJson } from '../dist/JsonType.js';

export class CardStatus {
    private _imageName: string;
    private _matchingKey: string;

    get imageName(): string {
        return this._imageName;
    }

    get matchingKey(): string {
        return this._matchingKey;
    }

    constructor(imageName: string, holeCount: number[]) {
        this._imageName = imageName;
        this._matchingKey = holeCount.toString();
    }
};

export class GameEngine {

    private cards: CardStatus[];

    // コンストラクター、初期化処理を行う
    constructor(topologyCards: TopologyCardJson[]) {
        this.cards = new Array();
        topologyCards.forEach(topologyCard => {
            const card = new CardStatus(topologyCard.ImageName, topologyCard.HoleCount);
            this.cards.push(card);
        });
    }

    /**
     * 神経衰弱ゲームに使用するカードを取得します。
     * @param cardNum - 神経衰弱ゲームにつかうカードの枚数
     * @returns 神経衰弱ゲームに使用できるカードセット
     */
    startGame(cardNum: number): CardStatus[] {

        this.validateCardNumber(cardNum);

        const cardGroups: Map<string, CardStatus[]> = this.initializeCardGroups();

        const selectedCards = new Array();

        for (let i = 0; i < cardNum / 2; i++) {
            // カードをランダムに2枚追加する
            selectedCards.push(...this.spliceRandomTwoCard(cardGroups));
        }

        // カードをシャッフルする
        return this.shuffleArray(selectedCards);
    }

    /**
     * カードの数を検証し、問題がある場合はエラーを投げる。
     * 
     * @param cardNum - 検証するカードの数
     * @throws {Error} - カードの数が0以下、奇数、または利用可能なカードの最大数を超えている場合
     */
    private validateCardNumber(cardNum: number): void {
        if (cardNum <= 0) {
            throw new Error("Card number must be greater than zero.");
        }

        if (cardNum % 2 !== 0) {
            throw new Error("Card number must be even.");
        }

        if (cardNum > this.cards.length) {
            throw new Error("Requested card number exceeds available cards.");
        }
    }

    /**
     * 入力された配列をシャッフルしたものを返します
     * @param array - シャッフルしたい配列
     * @returns シャッフルされた配列
     */
    private shuffleArray<T>(array: T[]): T[] {
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
    private spliceRandomTwoCard(cardGroups: Map<string, CardStatus[]>): CardStatus[] {
        // cardGroupsから各グループのカードの数の分だけキーを抜き出す
        const keysArray = new Array<string>();

        for (const key of Array.from(cardGroups.keys())) {
            const length: number = cardGroups.get(key).length;
            keysArray.push(... new Array(length).fill(key));
        }

        const randomIndex = Math.floor(Math.random() * keysArray.length);
        const randomKey = keysArray[randomIndex];

        return cardGroups.get(randomKey).splice(-2);
    }

    /**
     * cardGroupsを初期化します
     * @returns 初期化されたcardGroups
     */
    private initializeCardGroups(): Map<string, CardStatus[]> {
        const cardGroups = new Map<string, CardStatus[]>;

        // cardsをholeCountごとにまとめる
        this.cards.forEach(card => {
            if (!cardGroups.has(card.matchingKey)) {
                cardGroups.set(card.matchingKey, []);
            }

            cardGroups.get(card.matchingKey).push(card);
        });

        // それぞれのcardGroupをシャッフルする
        cardGroups.forEach((cardGroup, key) => {
            cardGroups.set(key, this.shuffleArray(cardGroup));
        });

        // holeCountごとに偶数になるように各groupの枚数を調整する
        cardGroups.forEach((value, key) => {
            if (value.length % 2 == 1) {
                value.pop();
            }
        });

        return cardGroups;
    }
}
