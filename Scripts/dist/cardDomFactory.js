import { CardDom } from './cardDom.js';
export class CardDomFactory {
    create(element) {
        return new CardDom(element);
    }
}
