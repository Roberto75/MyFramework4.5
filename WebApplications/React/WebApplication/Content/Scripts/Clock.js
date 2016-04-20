var Clock = (function () {
    function Clock(element) {
        this.element = element;
        this.element.innerHTML = "The time is: ";
        this.span = document.createElement("span");
        this.element.appendChild(this.span);
        this.span.innerText = new Date().toUTCString();
    }
    Clock.prototype.start = function () {
        var _this = this;
        this.timerToken = setInterval(function () { return _this.span.innerHTML = new Date().toUTCString(); }, 500);
    };
    Clock.prototype.stop = function () {
        clearTimeout(this.timerToken);
    };
    return Clock;
})();
;
//# sourceMappingURL=Clock.js.map