"use strict";

var messageQueue = [];
var isMessagePlaying = false;

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;


connection.on("ReceiveSongRequest", function (videoURL) {
    var videoPlayer = $('#songRequestPlayer');
    videoPlayer.attr('src', videoURL);

    if ($('#isPlayerEnabled').val() == 'on') {
        setTimeout(function () {
            videoPlayer.click();
        }, 2000); // delay playing the video to ensure the video gets loaded
    }
});

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});

connection.on("ReceiveMessageAudio", function (messageAudio) {

    messageQueue.push(messageAudio);
    ProcessMessageQueue();
});

function ProcessMessageQueue() {
    if (isMessagePlaying || messageQueue.length < 1)
        return;

    // play all the messages in the queue while removing the played messages
    var audioPlayer = new Audio("data:audio/wav;base64," + messageQueue[0]);
    audioPlayer.volume = $('#ttsVolume').val() / 100;

    audioPlayer.addEventListener("ended", function () {
        isMessagePlaying = false;
        ProcessMessageQueue();
    });

    messageQueue.shift();
    isMessagePlaying = true;

    try { // If the user didn't interact with the page before the audio plays an exception will be thrown - try catch to prevent crash
        audioPlayer.play();
    }
    catch (error) { }
    
}

connection.on("ReceiveMessageAudio", function (messageAudio) {

    document.getElementById("audioPlayer");
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});