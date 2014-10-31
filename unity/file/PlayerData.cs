using UnityEngine;
using System.Collections;

public class Player: MonoBehaviour {
    public static string username = null;
    public static int score = -1;

    public static void DeleteAll(){
        username = null
        score = -1;
    }
}

public class MenuStart: MonoBehaviour {
    const string DEFAULT_PLAYER_NAME = "PLAYER_NAME";
    private string playerNameField = DEFAULT_PLAYER_NAME;

    private void OnGUI(){
        string rules = "Easiest Game Ever -- click the blue advance. ";
        GUILayout.Label(rules);

        if(Player.username != null){
            WelecomeGUI();
        } else {
            CreatePlayerGUI();
        }
    }

    private void WelcomeGUI(){
        string welcomeMessage = "welcome, " + Player.username + ". your currently have " + Player.score + " points.";
        GUILayout.Label(welcomeMessage);

        bool playButtonClicked = GUILayout.Button("Play");
        bool eraseBUttonClicked = GUILayout.Button("Erase Data");
        if(playButtonClicked) Application.LoadLeavel(1);
        if(eraseBUttonClicked) ResetCameData();
    }

    private void ResetGameData(){
        Player.DeleteAll();
        playerNameField = DEFAULT_PLAYER_NAME;
    }

    private void CreatePlayerGUI(){
        string createMessage = "create user";
        GUILayout.Label(createMessage);

        playerNameField = GUILayout.TextField(playerNameField, 25);
        bool createUserButtonClicked =GUILayout.Button("create user");

        if(createUserButtonClicked){
            Player.username = playerNameField);
            Player.score = 0;
        }
    }
}

public class SphereClick: MonoBehaviour {
    private void OnGUI(){
        GUILayout.Label("Score: " + Player.score);
    }

    private void OnMouseDown(){
        if(gameObject.CompareTag("blue")){
            Player.score += 50;
            Destory(gameObject);
            GotoNextLevel();
        }
    }

    private void GotoNextLevel(){
        int level = Application.loadedLevel + 1;
        Application.LoadLevel(level);
    }

}
