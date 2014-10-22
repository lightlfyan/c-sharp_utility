using UnityEngine;

public class Functional {
    public delegate void Action();
    public delegate void Action<T0>(T0 a0);
    public delegate void Action<T0, T1>(T0 a0, T1 a1);
    public delegate void Action<T0, T1, T2>(T0 a0, T1 a1, T2 a2);
    public delegate void Action<T0, T1, T2, T3>(T0 a0, T1 a1, T2 a2, T3 a3);
}

public class SwitchCameras: MonoBehaviour, Functional {

    public GameObject[] cameras;
    public string[] shorcuts;
    public int currindex = 0;
    public bool changeAudioListener = true;
    void update() {
        int i = 0;
        for(i=0; i<cameras.Lenght; i++){
            if(Input.GetKeyUp(shorcuts[i])){
                switchCameras(i);
            }
        }
    }
    void switch(int index, bool b){
        if(changeAudioListener){
        cameras[index].GetComponent<AudioListener>().enabled = b;
        }
        cameras[index].camera.enabled = b;
    }

    void switchCameras(int index){
        if(index == currindex){
            return;
        }
        switch(index, true);
        switch(currindex, false);

    }

}