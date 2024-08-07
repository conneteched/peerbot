"use strict";

var messageQueue = [];
var isMessagePlaying = false;


var songQueue = [];

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.on("ReceiveMessageAudio", function (messageAudio) {
    if (messageAudio.length < 1)
        return; 

    messageQueue.push(messageAudio);
    ProcessMessageQueue();
});

// Plays all the messages in the queue
function ProcessMessageQueue() {
    if (isMessagePlaying || messageQueue.length < 1)
        return;

    try {
        let currentMessage = messageQueue[0];
        messageQueue.shift();

        // play all the messages in the queue while removing the played messages
        let audioPlayer = new Audio("data:audio/wav;base64," + currentMessage);
        audioPlayer.volume = $('#ttsVolume').val() / 100;

        audioPlayer.addEventListener("ended", function () {
            isMessagePlaying = false;
            ProcessMessageQueue();
        });

        isMessagePlaying = true;
        audioPlayer.play().catch(function (e) {
            isMessagePlaying = false;
        });
    }
    catch (error)
    {
        isMessagePlaying = false;
    }
}

connection.on("ReceiveSongRequest", function (videoURL) {
    songQueue.push(videoURL);
    ProcessSongQueue();
});

// Handles playing music
function ProcessSongQueue() {
    if (songQueue.length < 1)
        return;

    var videoPlayer = $('#songRequestPlayer');

    if ($('#isPlayerEnabled').val() == 'on') {

        videoPlayer.attr('src', songQueue[0]);
        songQueue.shift();

        setTimeout(function () {
            videoPlayer.click();
        }, 2000); // delay playing the video to ensure the video gets loaded
    }
}

connection.start();