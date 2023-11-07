import { ICardDom } from '../dist/ICardDom.js';
import { CardDom } from '../dist/cardDom.js';

export class CardDomFactory {
    create(element: HTMLElement): ICardDom {
        return new CardDom(element);
    }
}