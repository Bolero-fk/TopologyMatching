import { CardDomFactory } from '../dist/cardDomFactory.js';
import { TopologyCardJsonLoader } from '../dist/topologyCardJsonLoader.js';
import { GameElementInitializer } from '../dist/gameElementInitializer.js';
import { GameConfig } from '../dist/gameConfig.js';

const GAME_CONFIG = new GameConfig(
    /* row: */ 4,    // ゲームに配置するカードの枚数, ROW*COLUMNの値が偶数になるようにする
    /* column: */ 5, // FIXME: jsonに記されたカードのペアがROW * COLUMN以下のときに落ちるので注意する
    /* jsonPath: */ './TopologyCards/cards.json',
    /* imageFolderPath: */ './TopologyCards/images/',
    /* flippingWaitTimeMilliseconds: */ 1000,
    /* maxSelectableCard: */ 2 // FIXME: 現状の実装では選択可能枚数が2枚の時のみ実装されている
);

export function run() {
    const gameInitializer = new GameElementInitializer(document, GAME_CONFIG, new CardDomFactory, new TopologyCardJsonLoader);
    gameInitializer.initialize();
}