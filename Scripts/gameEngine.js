class CardStatus {
    constructor(imageName, holeCount) {
        this.imageName = imageName;
        this.holeCount = holeCount;
        this.complexityLevel = holeCount.reduce((sum, curr) => sum + curr, 0) + holeCount.length;
    }
}
;
export class GameEngine {
    // コンストラクター、初期化処理を行う
    constructor(topologyCards) {
        topologyCards.forEach(topologyCard => {
            const cardStatus = new CardStatus(topologyCard.ImageName, topologyCard.HoleCount);
        });
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
    checkIfGameEnded() {
        // ゲームが終了したかどうかをチェックし、結果を返します。
        return true;
    }
}
