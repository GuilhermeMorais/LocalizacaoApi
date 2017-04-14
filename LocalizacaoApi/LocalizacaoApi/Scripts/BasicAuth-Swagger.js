﻿(function () {
    $(function () {
        var basicAuthUI =
            '<div class="input"><input placeholder="username" id="input_username" name="username" type="text" size="10"></div>' +
                '<div class="input"><input placeholder="password" id="input_password" name="password" type="password" size="10"></div>';
        $(basicAuthUI).insertBefore('#api_selector div.input:last-child');
        $("#input_apiKey").hide();
        $('#input_username').change(addAuthorization);
        $('#input_password').change(addAuthorization);
    });

    function addAuthorization() {
        var username = $('#input_username').val();
        var password = $('#input_password').val();
        if (username.trim() !== "" && password.trim() !== "") {
            var basicAuth = new SwaggerClient.PasswordAuthorization('Tce.Checkin', username, password);
            window.swaggerUi.api.clientAuthorizations.add("basicAuth", basicAuth);
        }
    }
})();