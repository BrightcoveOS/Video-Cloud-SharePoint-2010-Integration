isPlayerAdded = false;

function addPlayer(videoID, playerID, playerName, autoStart, bgColor, playerWidth, playerHeight, isVid, playerWMode, htmlElementID, playlistTabs, playlistCombo, videoList) {

	//remove an obect if it's there first
	if(isPlayerAdded == true) {
		removePlayer(playerName);	
	}
	//initialize the params object
	var params = {};
	params.playerID = playerID;
	
	//set the video id
	params.videoId = videoID;
		
	//set autostart
	if(autoStart){
		params.autoStart = autoStart;
	}
	
	//set player mode
	if(playerWMode != ""){
		params.bgcolor = bgColor;
		params.wmode = playerWMode;	
	}
	
	//set dimensions
	params.width = playerWidth;
	params.height = playerHeight;
	params.isVid = isVid;
	
	//build the object with brightcove
	var player = brightcove.createElement("object");
	player.id = playerName;	
	var parameter;
	
	//set the is ui
	if(videoList != "-1" || playlistTabs.length > 0 || playlistCombo.length > 0){
		params.isUI = "true";
	}
	//if it's a playlist player then set those types else set the video id
	if(videoList != "-1"){
		parameter = brightcove.createElement("param");
		parameter.name = "@videoList";
		parameter.value = videoList;
		player.appendChild(parameter);
	}
	else if(playlistTabs.length > 0){
		parameter = brightcove.createElement("param");
		parameter.name = "@playlistTabs";
		parameter.value = playlistTabs;
		player.appendChild(parameter);
	}
	else if(playlistCombo.length > 0){
		parameter = brightcove.createElement("param");
		parameter.name = "@playlistCombo";
		parameter.value = playlistCombo;
		player.appendChild(parameter);
	}
	for (var i in params) {
		parameter = brightcove.createElement("param");
		parameter.name = i;
		parameter.value = params[i];
		player.appendChild(parameter);
	}
	
	//write out object tag
	var playerContainer = document.getElementById(htmlElementID);
	brightcove.createExperience(player, playerContainer, true);
	isPlayerAdded = true;
		
	return false;
}

function removePlayer(playerName) {
	if(isPlayerAdded == true) {
		isPlayerAdded = false;
		brightcove.removeExperience(playerName);
	}
}