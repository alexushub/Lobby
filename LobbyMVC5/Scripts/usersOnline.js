//usersOnline.js

$(document).ready(function () {
    var usersHub = $.connection.usersOnlineHub;
    $.connection.hub.logging = true;

    $.connection.hub.start({ /*transport: 'longPolling'*/ }).done(function () {
        var userName = globalUserName;
        //if (userName) {
        usersHub.server.userOnline(userName);
        //}

        });

    
    $.connection.hub.error(function (error) {
        console.log('SignalR error: ' + error);
    });

    usersHub.client.showUsersOnline = function (users) {
        $("#usersOnlineTable").empty();
        $("#usersOnlineWait").empty();
        for (var i = 0; i < users.length; i++) {
            var user = users[i];
            $("#usersOnlineTable").append('<tr><td>' + user.UserName + '</td></tr>');
        }
    }
});