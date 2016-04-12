var Greeter = (function () {
    function Greeter(greeting) {
        this.greeting = greeting;
    }
    Greeter.prototype.getHTML = function () {
        return "<h1>" + this.greeting + "</h1>";
    };
    return Greeter;
}());
;
//var greeter = new Greeter("Hello, world!");
//document.body.innerHTML = greeter.getHTML();
//var user = "Jane User";
//document.body.innerHTML = greeter(user);       
//# sourceMappingURL=Greeter.js.map