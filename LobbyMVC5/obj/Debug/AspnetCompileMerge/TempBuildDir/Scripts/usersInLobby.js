//usersInLobby.js

var lobbyHub;

function joinLobby(lobbyId, userName) {

    $("#joinBtn").addClass("disabled");

    var url = joinLobbyUrl;

    $.ajax({
        url: url,
        success: function (data) {
            if (data) {
                lobbyHub.server.userInLobby(userName, lobbyId);

                $("#exitBtn").removeClass("disabled");
            }
        }
    });
};

function disjoinLobby(lobbyId, userName) {

    $("#exitBtn").addClass("disabled");

    var url = disJoinLobbyUrl;
    //url += '?lobbyId=' + '@Model.Id' + '&userName=' + "@User.Identity.Name";
    $.ajax({
        url: url,
        success: function (data) {
            if (data) {
                lobbyHub.server.userLeaveLobby(userName, lobbyId);

                $("#joinBtn").removeClass("disabled");
            }
        }
    });
};

function startLobby(lobbyId) {

    $("#startBtn").addClass("disabled");

    var url = startLobbyUrl;

    $.ajax({
        url: url,
        success: function (data) {
            if (data) {
                lobbyHub.server.userStartLobby(lobbyId);
            }
        }
    });
};

function updateButtons() {
    if (isMyPenaltyIsActive) {
        $("#myPenaltyIsActive").show();
    } else {
        $("#myPenaltyIsActive").hide();
    }

    if (!amIJoined && isLobbyWaiting && !isEnoughUsers && !isMyPenaltyIsActive) {
        $("#joinBtn").removeClass("disabled");
    } else {
        $("#joinBtn").addClass("disabled");
    }

    if (amIJoined && !isLobbyFinished /*&& !amIAuthor*/) {
        $("#exitBtn").removeClass("disabled");
    } else {
        $("#exitBtn").addClass("disabled");
    }

    if (amIAuthor && amIJoined && isLobbyWaiting && isEnoughUsers) {
        $("#startBtn").removeClass("disabled");
    } else {
        $("#startBtn").addClass("disabled");
    }
}


$(document).ready(function () {

    lobbyHub = $.connection.usersInLobbyHub;
    $.connection.hub.logging = true;

    lobbyHub.client.setUserPenaltyState = function (isActive) {

        isMyPenaltyIsActive = isActive;
        updateButtons();
    }

    lobbyHub.client.showUsersInLobby = function (data) {
        var users = data.Users;
        var mess = data.Message;
        var isNeedUpdBtns = data.IsNeedToUpdateButtons;
        var lobby = data.Lobby;

        $("#usersInLobbyTable").empty();
        $("#usersInLobbyWait").empty();
        $("#usersInLobbyAmount").empty();

        $("#usersInLobbyAmount").append(users.length);
        for (var i = 0; i < users.length; i++) {
            var user = users[i];
            $("#usersInLobbyTable").append('<tr><td>' + user.UserName + '</td></tr>');
        }

        if (isNeedUpdBtns) {
            isLobbyWaiting = lobby.State == lobbyStateWaiting;
            isLobbyFinished = lobby.State == lobbyStateFinished;
            isLobbyActive = lobby.State == lobbyStateActive;
            amIJoined = lobby.CurrentUsersIds.indexOf(currentViewerUserId) > -1;
            isEnoughUsers = lobby.RequiredUsersAmount <= lobby.CurrentUsersAmount;

            $("#lobbyState").empty();
            $("#lobbyState").append(isLobbyWaiting ? "Waiting" : isLobbyFinished ? "Finished" : "Active");

            updateButtons();
        }

        if (mess) {
            alert(mess);
        }
    }


    //start executing
    var url = GetPenaltyStateForUserUrl;

    $.ajax({
        url: url,
        success: function(data){
                    
            isMyPenaltyIsActive = data == "True" ? true : false;

            updateButtons();
        }
    });

    $.connection.hub.error(function (error) {
        console.log('SignalR error: ' + error);
    });

    $.connection.hub.start({ /*transport: 'longPolling'*/ }).done(function () {
        var userName = globalUserName;
        //var lobbyId = @Model.Id;

        lobbyHub.server.showMeUsersInLobby(lobbyId);
    });


});