//lobbies.js

$(document).ready(function () {
    var lobbiesHub = $.connection.lobbiesHub;
    $.connection.hub.logging = true;

    $.connection.hub.start({ /*transport: 'longPolling'*/ }).done(function () {

        lobbiesHub.server.showLobbies(2);
        lobbiesHub.server.showLobbies(4);

        });

    
    $.connection.hub.error(function (error) {
        console.log('SignalR error: ' + error);
    });

    lobbiesHub.client.showLobbies = function (res) {
        var lobbies = res.lobbies;
        var reqUsersAmount = res.reqUsersAmount;
        debugger;
        var id = "#lobbiesWith" + reqUsersAmount + "UsersHeader";
        var cl = "lobbiesWith" + reqUsersAmount + "UsersRow";
        $(id).nextAll().empty();

        for (var i = 0; i < lobbies.length; i++) {
            var lobby = lobbies[i];

            

            var dt = new Date(lobby.CreationDate);
            var curr_date = dt.getDate();
            var curr_month = dt.getMonth() + 1;
            var curr_year = dt.getFullYear();
            //var curr_hour = dt.getHours();
            //var curr_min = dt.getMinutes();
            //var curr_sec = dt.getSeconds();
            $("." + cl).last().after("<tr class='" + cl + "'><td>" + "<a href='" + lobbyUrl + "/" + lobby.Id + "'>" + lobby.Name + "</a>" + "</td><td>" + curr_date + "." + curr_month + "." + curr_year + /*" " + curr_hour + ":" + curr_min + ":" + curr_sec +*/ "</td><td>" + lobby.Author.UserName + "</td><td>" + lobby.RequiredUsersAmount + "</td><td>" + lobby.CurrentUsersAmount + "</td></tr>");
        }
    }
});