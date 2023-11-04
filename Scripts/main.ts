import { CardDom } from './cardDom.js';
import { GameController } from './gameController.js';
import { TopologyCardJsonLoader } from './topologyCardJsonLoader.js';
import { GameElementInitializer, GameConfig } from './gameElementInitializer.js';

const GAME_CONFIG: GameConfig = {
    row: 4,    // ゲームに配置するカードの枚数, ROW*COLUMNの値が偶数になるようにする
    column: 5, // FIXME: jsonに記されたカードのペアがROW * COLUMN以下のときに落ちるので注意する
    jsonPath: './TopologyCards/cards.json',
    imageFolderPath: './TopologyCards/images/',
    flippingWaitTimeMilliseconds: 1000,
    maxSelectableCard: 2 // FIXME: 現状の実装では選択可能枚数が2枚の時のみ実装されている
};

window.onload = () => {
    const gameInitializer = new GameElementInitializer(GAME_CONFIG);
    gameInitializer.initialize();
};
