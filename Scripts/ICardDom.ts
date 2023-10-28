export interface ICardDom {
    setBackgroundColor(color: string): void;
    setBackgroundImage(url?: string): void;
    getComputedStyleProperty(propertyName: string): string;
    onClick(handler: () => void): void;
}
