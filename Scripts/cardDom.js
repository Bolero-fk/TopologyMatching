export class CardDom {
    constructor(element) {
        this.element = element;
        this.frontBackgroundColor = getComputedStyle(this.element).getPropertyValue("--front-background-color");
        this.backBackgroundColor = getComputedStyle(this.element).getPropertyValue("--back-background-color");
    }
    flipToFront(url) {
        this.setBackgroundColor(this.frontBackgroundColor);
        this.setBackgroundImage(url);
    }
    flipToBack() {
        this.setBackgroundColor(this.backBackgroundColor);
        this.setBackgroundImage();
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
