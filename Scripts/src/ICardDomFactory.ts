import { ICardDom } from '../dist/ICardDom.js';

export interface ICardDomFactory {
    create(element: HTMLElement): ICardDom;
}
