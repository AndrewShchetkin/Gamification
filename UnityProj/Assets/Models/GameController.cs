﻿using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using Assets.Models;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour
{
    static Dictionary<string, bool> readyGameObjects = new Dictionary<string, bool>()
    {
        { nameof(HexGrid), false },
        { nameof(HexMapEditor), false },
        { nameof(GameController), false },
        { nameof(MaskRenderer), false }
    };

    static Dictionary<string, bool> readyPlayerInfo = new Dictionary<string, bool>()
    {
        { nameof(UserName), false },
        { nameof(Teams), false }
    };

    public static readonly Color[] TeamsColors =
    {
        Color.green,
        Color.red,
        Color.blue,
        Color.gray,
        Color.yellow
    };

    public static int AnimationController { get; set; }
    public Canvas editUI;
    /// <summary>
    /// Ограничение кадров
    /// </summary>
    public int targetFrameRate = 30;

    // для трейлера
    static int demoPoints = 30;
    public static int DemoPoints
    {
        get
        {
            return demoPoints;
        }
        set
        {
            demoPoints = value;
            GameObject.Find("Points").GetComponent<Text>().text = value.ToString();
        }
    }
    static string userName;
    /// <summary>
    /// Имя текущего игрока
    /// </summary>
    static string UserName { 
        get 
        {
            return userName;
        } 
        set
        {
            userName = value;
            SetReadyPlayerInfo(nameof(UserName));
        }
    }

    static User currentPlayer;
    static public User CurrentPlayer
    {
        get
        {
            return currentPlayer;
        }
        set 
        {
            //if (value.role == "Admin")
            //{
            //editUI.gameObject.SetActive(true);
            //}
            currentPlayer = value;
            GameObject.Find("PlayerName").GetComponent<Text>().text = CurrentPlayer.userName;
            //GameObject.Find("Points").GetComponent<Text>().text = CurrentPlayer.points.ToString(); для трейлера
            GameObject.Find("Points").GetComponent<Text>().text = DemoPoints.ToString();
        } 
    }

    static Team playerTeam;
    public static Team PlayerTeam
    {
        get
        {
            return playerTeam;
        }
        set
        {
            playerTeam = value;
        }
    }
    
    static List<Team> teams;
    /// <summary>
    /// Участвующие в игре команды
    /// </summary>
    public static List<Team> Teams
    { 
        get 
        { 
            return teams; 
        }
        set
        {            
            teams = value;
            SetReadyPlayerInfo(nameof(Teams));
            SetTeamsColors();
        }
    }

    /// <summary>
    /// Метод контроллера, служит для опроса обьектов на готовность их работы
    /// </summary>
    /// <param name="typeOfObject"></param>
    public static void SetReadyObject(Type typeOfObject)
    {
        readyGameObjects[typeOfObject.Name] = true;
        if (readyGameObjects.All(g => g.Value == true))
        {
            StartForReadyObjects();
        }
    }

    public static void SetReadyPlayerInfo(string Name)
    {
        readyPlayerInfo[Name] = true;
        if (readyPlayerInfo.All(g => g.Value == true))
        {
            SetPlayerInfo();
        }
    }

    static void StartForReadyObjects()
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
		GetCurrentUser();
        GetAllTeams();
#endif
#if UNITY_EDITOR == true
        var hexMapEditor = FindObjectOfType<HexMapEditor>();
        hexMapEditor.SetMapData("");
#endif
        HexMapEditor.Load();
    }

    static void SetPlayerInfo()
    {
        PlayerTeam = GetPlayerTeam();
        CurrentPlayer = GetCurrentPlayer();
    }

    public static User GetCurrentPlayer()
    {
        return PlayerTeam
            .users
            .Where(u => u.userName == UserName)
            .SingleOrDefault();
    }
    public static Color GetTeamColor(string ownerId)
    {
        return Teams.Where(t => t.id == ownerId).SingleOrDefault().colorIndex;
    }

    private static void SetTeamsColors()
    {
        int counter = 0;
        Teams.ForEach(t => t.colorIndex = TeamsColors[counter++]);
    }

    public void Awake()
    {
#if UNITY_EDITOR == true
        var allTeams = "[{ \"id\":\"33e198ff-bceb-4cec-a89e-2f97daca2930\",\"teamName\":\"PechalBeda\",\"users\":[{ \"userName\":\"vlad\",\"id\":\"f51c9e93-ffd1-491a-c95a-08da3b1a2a6b\",\"points\":10}],\"points\":20,\"colorIndex\":{ \"r\":0.0,\"g\":1.0,\"b\":0.0,\"a\":1.0} }," +
            "{ \"id\":\"33e198ff-bceb-4cec-a89e-2f97daca2931\",\"teamName\":\"GMCS\",\"users\":[{ \"userName\":\"neVlad\",\"id\":\"43e94935-72c5-4108-8d5a-08da3b42f870\",\"points\":15}],\"points\":25,\"colorIndex\":{ \"r\":1.0,\"g\":0.0,\"b\":0.0,\"a\":1.0} }," +
            "{ \"id\":\"d8d25b0a-2491-412c-b446-5bc489b1a300\",\"teamName\":\"DreamTeam\",\"users\":[{ \"userName\":\"SomeOne\",\"id\":\"692b6354-fd36-43cf-9c70-d525c9bf2610\",\"points\":12}],\"points\":18,\"colorIndex\":{ \"r\":1.0,\"g\":1.0,\"b\":0.0,\"a\":1.0} }]";

        SetTeams(allTeams);
        UserName = "vlad";
#endif
    }

    public static Team GetPlayerTeam()
    {
        var playerTeam = Teams.Where(t => t.users.Any(u => u.userName == UserName)).SingleOrDefault();
        Debug.Log($"GetPlayerTeam | {playerTeam.id}");
        return playerTeam;
    }

    public void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
        SetReadyObject(this.GetType());
    }

    #region [Unity -- React methods]

    [DllImport("__Internal")]
    private static extern void GameOver(string userName, int score);
    public void SomeMethod()
    {
    #if UNITY_WEBGL == true && UNITY_EDITOR == false
    GameOver ("Player1", 100);
    #endif
    }
    public void SpawnEnemies(int amount)
    {
        Debug.Log($"Spawning {amount} enemies!");
    }

    [DllImport("__Internal")]
    private static extern void GetAllTeams();

    [DllImport("__Internal")]
    private static extern void GetCurrentUser();

    public void SetPlayer(string UserName)
    {
        Debug.Log($"UserName from React: {UserName}");
        GameController.UserName = UserName;
    }

    public void SetTeams(string allTeams)
    {
        Teams teams = JsonUtility.FromJson<Teams>("{\"teams\" :" + allTeams + "}");
        Teams = teams.teams;
        Debug.Log(JsonUtility.ToJson(teams));
    }

    [DllImport("__Internal")]
    private static extern void SubtractPoints(User currentPlayer, int points);
    public void SubtractPoints(int points)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
				SubtractPoints(CurrentPlayer, points);
#endif
    }
    #endregion
}
