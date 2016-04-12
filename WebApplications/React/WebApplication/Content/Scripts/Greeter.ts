class Greeter {
    constructor(public greeting: string) { }

    getHTML() {
        return "<h1>" + this.greeting + "</h1>";
    }
};


//var greeter = new Greeter("Hello, world!");
//document.body.innerHTML = greeter.getHTML();

//var user = "Jane User";
//document.body.innerHTML = greeter(user);      