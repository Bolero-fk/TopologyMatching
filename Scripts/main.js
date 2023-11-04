import { GameElementInitializer } from './gameElementInitializer.js';
const GAME_CONFIG = {
    row: 4,
    column: 5,
    jsonPath: './TopologyCards/cards.json',
    imageFolderPath: './TopologyCards/images/',
    flippingWaitTimeMilliseconds: 1000,
    maxSelectableCard: 2 // FIXME: 現状の実装では選択可能枚数が2枚の時のみ実装されている
};
window.onload = () => {
    const gameInitializer = new GameElementInitializer(GAME_CONFIG);
    gameInitializer.initialize();
};
