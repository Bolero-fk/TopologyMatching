export interface ICardDom {
    flipToFront(url: string): void;
    flipToBack(): void;
    onClick(handler: () => void): void;
}
