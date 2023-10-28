export interface ICardDom {
    setBackgroundColor(color: string): void;
    setBackgroundImage(url?: string): void;
    onClick(handler: () => void): void;
}
