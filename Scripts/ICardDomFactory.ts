import { ICardDom } from './ICardDom.js';

export interface ICardDomFactory {
    create(element: HTMLElement): ICardDom;
}
