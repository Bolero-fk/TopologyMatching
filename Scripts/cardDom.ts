import { ICardDom } from './ICardDom.js';

export class CardDom implements ICardDom {
    private readonly frontBackgroundColor: string;
    private readonly backBackgroundColor: string;
    private readonly element: HTMLElement;

    constructor(element: HTMLElement) {
        this.element = element;

        this.frontBackgroundColor = getComputedStyle(this.element).getPropertyValue("--front-background-color");
        this.backBackgroundColor = getComputedStyle(this.element).getPropertyValue("--back-background-color");
    }

    flipToFront(url: string): void {
        this.setBackgroundColor(this.frontBackgroundColor);
        this.setBackgroundImage(url);
    }

    flipToBack(): void {
        this.setBackgroundColor(this.backBackgroundColor);
        this.setBackgroundImage();
    }


    onClick(callback: () => void): void {
        this.element.onclick = callback;
    }

    setBackgroundColor(color: string): void {
        this.element.style.backgroundColor = color;
    }

    setBackgroundImage(url?: string): void {
        this.element.style.backgroundImage = url || '';
    }
}