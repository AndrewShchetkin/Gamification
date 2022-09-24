import React, { useState, useEffect } from "react";
import { HttpTransportType, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import Unity, { UnityContext } from "react-unity-webgl";

const unityContext = new UnityContext({
  loaderUrl: process.env.PUBLIC_URL + "Build/public.loader.js",
  dataUrl: process.env.PUBLIC_URL + "Build/public.data.gz",
  frameworkUrl: process.env.PUBLIC_URL + "Build/public.framework.js.gz",
  codeUrl: process.env.PUBLIC_URL + "Build/public.wasm.gz",
});

function Map() {
  function spawnEnemies() {
    unityContext.send("GameController", "SpawnEnemies", 100);
  }

  const connection = new HubConnectionBuilder()
    .configureLogging(LogLevel.Debug)
    .withUrl("/hubs/map")
    .configureLogging(LogLevel.Information)
    .build();

  connection.on("UpdateCell", (cell: any) => {
    unityContext.send("Hex Map Editor", "GrabCell", JSON.stringify(cell))
  });

  connection.start().catch(err => document.write(err));

  const updateCell = async (cell: any) => {  
    try {
        await connection.send('UpdateCell', cell);
    }
    catch(e) {
        console.log(e);
    }
  }

  useEffect(function () {
      unityContext.on("SaveMap", function (map) {   
        const requestOptions = {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: map
      };
      fetch('/api/map/save-map', requestOptions)
      }
    );
  }, []);

  useEffect(function () {
    unityContext.on("UpdateCell", function (cell) {   
      const requestOptions = {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: cell
    };    
    fetch('/api/map/update-cell', requestOptions)
    .then(() => updateCell(cell))
    
    }
  );
  }, []);

  useEffect(function () {
    unityContext.on("LoadMap", function () {   
      const requestOptions = {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' }        
    };
    fetch('/api/map/load-map', requestOptions)
    .then(response => response.json())
    .then(data => unityContext.send("Hex Map Editor", "SetMapData", JSON.stringify(data)))
    }
  );
  }, []);

  useEffect(function () {
    unityContext.on("GetCurrentUser", function () {   
      const requestOptions = {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' }        
    };
    fetch('/api/auth/user', requestOptions)
    .then(response => response.json())
    .then(data => {
      console.log(data)
        unityContext.send("GameController", "SetPlayer", data.username)
      })
    }
  );
  }, []);

  useEffect(function () {
    unityContext.on("GetAllTeams", function () {   
      const requestOptions = {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' }        
    };
    fetch('/api/team/getallteams', requestOptions)
    .then(response => response.json())
    .then(data => {
        console.log(data)
        unityContext.send("GameController", "SetTeams", JSON.stringify(data)) 
      })
    }
  );
  }, []);

  return (
    <div>
      <button onClick={spawnEnemies}>Spawn a bunch!</button>      
      <Unity style={{ width: '1366px', height: '720px'}} unityContext={unityContext} />
    </div>
  );
}

export default Map;
