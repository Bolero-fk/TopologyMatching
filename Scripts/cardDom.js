export class CardDom {
    constructor(element) {
        this.element = element;
    }
    onClick(callback) {
        this.element.onclick = callback;
    }
    setBackgroundColor(color) {
        this.element.style.backgroundColor = color;
    }
    setBackgroundImage(url) {
        this.element.style.backgroundImage = url || '';
    }
}
