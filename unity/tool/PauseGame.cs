using UnityEngine;
using System.Collections;

public class PauseGame: MonoBehaviour {
    public bool expensiveQualitySettings = true;
    private bool isPaused = false;

    private void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(isPaused) ResumeGameMode();
            else PauseGameMode();
        }
    }

    private void ResumeGameMode(){
        Time.timeScale = 1.0f;
        isPause = false;
        Screen.showCursor = false;
        GetComponent<MouseLook>().enable = true;
    }

    private void PauseGameMode(){
        Time.timeScale = 0.0f;
        isPaused = true;
        Screen.showCursor = true;
        GetComponent<MouseLook>().enable = false;
    }

    private void OnGUI(){
        if(isPause) PauseGameGUI();
    }

    private void PauseGameGUI(){
        string[] names = QualitySettings.names;
        string message = "GamePaused. Press ESC to resume or Select a new quality";
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical();

        for(int i=0; i<names.Length; i++){
            if(GUILayout.Button(names[i], GUILayout.Width(200))){
                QualitySettings.SetQualityLevel(i, expensiveQualitySettings);
            }
        }

        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndArea();

    }
}
