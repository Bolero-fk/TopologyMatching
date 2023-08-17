class CardStatus {
    get imageName() {
        return this._imageName;
    }
    get matchingKey() {
        return this._matchingKey;
    }
    constructor(imageName, holeCount) {
        this._imageName = imageName;
        this._matchingKey = holeCount.toString();
    }
}
;
export class GameEngine {
    // コンストラクター、初期化処理を行う
    constructor(topologyCards) {
        this.cards = new Array();
        topologyCards.forEach(topologyCard => {
            const card = new CardStatus(topologyCard.ImageName, topologyCard.HoleCount);
            this.cards.push(card);
        });
    }
    // ゲーム開始時の初期化処理
    startGame(cardNum) {
        const cardGroups = this.initializeCardGroups();
        const selectedCards = new Array();
        for (let i = 0; i < cardNum / 2; i++) {
            // カードをランダムに2枚追加する
            selectedCards.push(...this.spliceRandomTwoCard(cardGroups));
        }
        // カードをシャッフルする
        return this.shuffleArray(selectedCards);
    }
    /**
     * 入力された配列をシャッフルしたものを返します
     * @param array - シャッフルしたい配列
     * @returns シャッフルされた配列
     */
    shuffleArray(array) {
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
    spliceRandomTwoCard(cardGroups) {
        // cardGroupsから各グループのカードの数の分だけキーを抜き出す
        const keysArray = new Array();
        for (const key of cardGroups.keys()) {
            const length = cardGroups.get(key).length;
            keysArray.push(...new Array(length).fill(key));
        }
        const randomIndex = Math.floor(Math.random() * keysArray.length);
        const randomKey = keysArray[randomIndex];
        return cardGroups.get(randomKey).splice(-2);
    }
    /**
     * cardGroupsを初期化します
     */
    initializeCardGroups() {
        const cardGroups = new Map;
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
