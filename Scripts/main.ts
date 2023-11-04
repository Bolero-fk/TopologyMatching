import { CardDom } from './cardDom.js';
import { GameController } from './gameController.js';
import { TopologyCardJsonLoader } from './topologyCardJsonLoader.js';
import { GameElementInitializer } from './gameElementInitializer.js';

// ゲームに配置するカードの枚数, ROW*COLUMNの値が偶数になるようにする
// FIXME: jsonに記されたカードのペアがROW * COLUMN以下のときに落ちるので注意する
const ROW = 4;
const COLUMN = 5;

const JSON_PATH = './TopologyCards/cards.json';

const IMAGE_FOLDER_PATH = './TopologyCards/images/';

const FLIPPING_WAIT_TIME_MILLISECONDS = 1000;

// FIXME: 現状の実装では選択可能枚数が2枚の時のみ実装されている
const MAX_SELECTABLE_CARD = 2;

window.onload = () => {
    const gameInitializer = new GameElementInitializer(ROW, COLUMN, JSON_PATH, IMAGE_FOLDER_PATH, FLIPPING_WAIT_TIME_MILLISECONDS, MAX_SELECTABLE_CARD);
    gameInitializer.initialize();
};
