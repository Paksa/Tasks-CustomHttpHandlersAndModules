(function() {
    var $ = $ || function (selector) {
        var removeClass = function(className) {
            if (this.constructor === Array) {
                for (var i = 0; i < this.length; i++) {
                    this[i].classList.remove(className);
                };
                return;
            }
            this.classList.remove(className);
        };

        var addClass = function(className) {
            if (this.constructor === Array) {
                for (var i = 0; i < this.length; i++) {
                    this[i].classList.add(className);
                };
                return;
            }
            this.classList.add(className);
        };
        
        var elementName = selector.slice(1, selector.length);
        if (elementName === undefined || elementName === null) return null;
        var elements = null;
        if (selector[0] === '.') {
            elements = document.getElementsByClassName(elementName);
        } else
        if (selector[0] === '#') {
            elements = document.getElementById(elementName);
        }
        if (elements === null || elements === undefined) return null;
        if (elements.constructor === Array) {
            if (elements.length === 1) elements = elements[0];
        }
        if (elements === null || elements === undefined) return null;
        if (elements.prototype === undefined || elements.prototype === null) {
            elements.addClass = addClass;
            elements.removeClass = removeClass;
        } else {
            elements.prototype.removeClass = removeClass;
            elements.prototype.addClass = addClass;
        }
        return elements;
    };

    window.$ = $;

    var auth = auth || (function () {
        var module = {};
        var sendCredentialsWithAjax = function(url, username, password, callback, requestType) {
            var ajax = new XMLHttpRequest();
            ajax.open("POST", url, true);
            //ajax.contentType = "application/json";
            ajax.onreadystatechange = function() {
                if (ajax.readyState === 4) {
                    if (ajax.status === 200) {
                        //ajax call successful:
                        if (callback !== undefined && callback !== null) {
                            if (callback instanceof Function) callback();
                        }
                    } else {
                        alert("Error: " + ajax.response);
                    }
                }
            };
            var credentials = {};
            credentials.Username = username;
            credentials.Password = password;
            if (requestType !== undefined && requestType !== null) credentials.RequestType = requestType;
            else credentials.RequestType = "Login";
            var jsonString = JSON.stringify(credentials);
            ajax.send(jsonString);
        };

        var checkUsername = function (username) {
            var pattern = new RegExp("^[a-zA-Z0-9\.-]{4,}$");
            var checkResult = pattern.exec(username);
            if (!checkResult) {
                alert("Username must be greater than 4 characters");
                return false;
            }
            return true;
        };

        var checkPassword = function (password) {
            //var pattern = new RegExp("^(?=.*\d)(?=.*[a-zA-Z]).{4,64}$");
            var pattern = new RegExp("^[a-zA-Z0-9]{4,64}$");
            var checkResult = pattern.test();
            if (!checkResult) {
                alert("Password must contain at least one digit and be longer than 4 characters without spaces");
                return false;
            }
            return true;
        };

        module.Login = function login(callback) {
            var username = $("#username-field").value;
            var password = $("#password-field").value;

            if (checkUsername(username) && checkPassword(password)) {
                sendCredentialsWithAjax("index.html", username, password, callback);
            }
        };

        module.Register = function(callback) {
            var username = $("#username-field").value;
            var password = $("#password-field").value;

            if (checkUsername(username) && checkPassword(password)) {
                sendCredentialsWithAjax("index.html", username, password, callback, "Register");
            }
        }

        module.Logout = function(callback) {
            sendCredentialsWithAjax("index.html", "", "", callback, "Logout");
        };

        return module;
    })();

    $.auth = auth;

    $.IsCookiePresented = function(cookieName)
    {
        var cookieMatch = document.cookie.match(cookieName);
        if (cookieMatch === undefined || cookieMatch === null) return false;
        return true;
    }
})();




window.onload = function () {
    //this function called upon successful authentication
    var loginCallback = function() {
        $("#login-register-block").addClass("hidden");
        $("#logout-button").removeClass("hidden");
        $("#json-text").removeClass("hidden");
        $("#json-tree").removeClass("hidden");
    };

    //if cookie presented, then current request is authenticated
    if (!$.IsCookiePresented("authentication-token")) {
        $("#logout-button").addClass("hidden");
        $("#login-register-block").removeClass("hidden");
        
        //Login functionality:
        var buttonForGenericHandler = $("#login-button");
        buttonForGenericHandler.onclick = function () {
            $.auth.Login(loginCallback);
        };
    }
    else
    {
        $("#json-text").removeClass("hidden");
        $("#json-tree").removeClass("hidden");
    }
    
    //Logout handler:
    var logoutButton = $("#logout-button");
    logoutButton.onclick = function () {
        $.auth.Logout(function() {
            $("#logout-button").addClass("hidden");
            $("#login-register-block").removeClass("hidden");
            //$("#json-text").addClass("hidden");
            //$("#json-tree").addClass("hidden");
            $("#json-text").removeClass("hidden");
            $("#json-tree").removeClass("hidden");
            //needed to refresh cookie list (actually, cookie must be deleted after such call)
            window.location = "";
        });
    }

    //GenericHandler contact loader handler:
    $("#get-json-text-button").onclick = function() {
        $.LoadContacts("CustomHandlers/ContactJsonHandler.ashx", function (responseText) {
            $("#json-text-container").innerHTML = responseText;
        });
    };

    var loginRegisterSwitch = $("#login-register-switch-button");
    loginRegisterSwitch.onclick = function () {
        var loginButton = $("#login-button");
        var signInText = "Sign In";
        var signUpText = "Sign Up";
        var currentInputFuntionNamePlaceholder = $("#authentication-type-name-bold");
        if (currentInputFuntionNamePlaceholder.innerHTML === signInText) {
            currentInputFuntionNamePlaceholder.innerHTML = signUpText;
            loginRegisterSwitch.value = "Login";
            loginButton.value = "Register";
            loginButton.onclick = function() {
                $.auth.Register(loginCallback);
            }
        } else {
            currentInputFuntionNamePlaceholder.innerHTML = signInText;
            loginRegisterSwitch.value = "Register";
            loginButton.value = "Login";
            loginButton.onclick = function () {$.auth.Login(loginCallback);}
        }
    };
};