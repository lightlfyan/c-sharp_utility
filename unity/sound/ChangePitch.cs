using UnityEngine;

public class ChangePitch: MonoBehaviour {
    public float speed = 0.0f;
    public float minSpeed = 0.0f;
    public float maxSpeed = 2.0f;
    public float animationSoundRatio = 1.0f;

    private Animator animator;

    void Start(){
        animator = GetComponent<Animator>();
    }

    void Update(){
        amimator.speed = speed;
        audio.pitch = speed * animationSoundRatio;
    }

    void OnGUI(){
        Rect rect = new Rect(10, 10, 100, 30);
        speed = GUI.HorizontalSlider(rect, speed, minSpeed, maxSpeed);
    }
}
