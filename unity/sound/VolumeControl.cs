using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VolumeControl: MonoBehaviour {
    bool separateSoundtrack = true;
    float maxVolume = 0.0f;
    float minVolume = 1.0f;
    float initialVolume = 1.0f;
    float soundtrackVolume = 1.0f;
    bool displaySliders = false;

    void Start(){
        if(separateSoundtrack){
            audio.ignoreListenerVolume = true;
        }
    }

    void Update(){
        AudioListener.volume = initialVolume;
        if(separateSoundtrack){
            audio.volume = soundtrackVolume;
        } else {
            audio.volume = initialVolume;
        }
    }

    void OnGUI(){
        Event e = Event.current;
        if(e.type == EventType.KeyUp && e.keyCode == KeyCode.Escape){
            displaySliders = !displayliders;
        }
        if(displaySLiders){
            if(!separateSoundtrack){
                GUI.Label(new Rect(10, 0, 100, 30), "Volume");
                initialVolume = GUI.HorizontalSlider(new Rect(10, 20, 100, 30), initialVolume, minVolume, maxVolume);
            } else {
                GUI.Label(new Rect(10, 0, 100, 30), "Sound FX");
                initialVolume = GUI.HorizontalSlider(new Rect(10, 20, 100, 30), initialVolume, minVolume, maxVolume);
                GUI.Label(new Rect(10, 40, 100, 30), "Music");
                soundtrackVolume = GUI.HorizontalSlider(new Rect(10, 60, 100, 30), soundtrackVolume, minVolume, maxVolume);
            }
        }
    }
}
