import { ICardDom } from './ICardDom.js';

export class CardDom implements ICardDom {
    constructor(private element: HTMLElement) { }

    onClick(callback: () => void): void {
        this.element.onclick = callback;
    }

    setBackgroundColor(color: string): void {
        this.element.style.backgroundColor = color;
    }

    setBackgroundImage(url?: string): void {
        this.element.style.backgroundImage = url || '';
    }

    getComputedStyleProperty(propertyName: string): string {
        return getComputedStyle(this.element).getPropertyValue(propertyName);
    }
}