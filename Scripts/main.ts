import { CardDomFactory } from './cardDomFactory.js';
import { TopologyCardJsonLoader } from './topologyCardJsonLoader.js';
import { GameElementInitializer } from './gameElementInitializer.js';
import { GameConfig } from './gameConfig.js';

const GAME_CONFIG: GameConfig = {
    row: 4,    // ゲームに配置するカードの枚数, ROW*COLUMNの値が偶数になるようにする
    column: 5, // FIXME: jsonに記されたカードのペアがROW * COLUMN以下のときに落ちるので注意する
    jsonPath: './TopologyCards/cards.json',
    imageFolderPath: './TopologyCards/images/',
    flippingWaitTimeMilliseconds: 1000,
    maxSelectableCard: 2 // FIXME: 現状の実装では選択可能枚数が2枚の時のみ実装されている
};

export function run() {
    const gameInitializer = new GameElementInitializer(GAME_CONFIG, new CardDomFactory, new TopologyCardJsonLoader);
    gameInitializer.initialize();
}