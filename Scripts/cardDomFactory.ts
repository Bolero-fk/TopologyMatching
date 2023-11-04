import { ICardDom } from './ICardDom.js';
import { CardDom } from './cardDom.js';

export class CardDomFactory {
    create(element: HTMLElement): ICardDom {
        return new CardDom(element);
    }
}