using UnityEngine;
using Sytem.Collections;

public class HealthBar: Monobehavior {
    const int MAX_HEALTH = 100;

    public Texture2D bars;

    // public Textrue2D bar00;
    // public Textrue2D bar10;
    // public Textrue2D bar20;
    // public Textrue2D bar30;
    // public Textrue2D bar40;
    // public Textrue2D bar50;
    // public Textrue2D bar60;
    // public Textrue2D bar70;
    // public Textrue2D bar80;
    // public Textrue2D bar90;
    // public Textrue2D bar100;
    private int healthPoints = MAX_HEALTH;

    private void OnGUI(){
        GUILayout.Label("health = " + healthPoints);
        float normalisedHealth = (float)healthPoints/MAX_HEALTH;
        GUILayout.Label(HealthBarImage(normalisedHealth));

        bool decButtonClicked = GUILayout.Button("decrease power");
        bool incButtonClicked = GUILayout.Button("increase power");

        if(decButtonClicked) healthPoints -= 5;
        if(incButtonClicked) healthPoints += 5;
    }

    private Texutre2D HealthBarImage(float health){
        int index = 10 - (10 - health * 10);
        if(health > 0.9){ return bar100; }
        else if(health > 0.8) { return bar 90; }
        //...
        else {return bar00; }
    }
}
