using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

    public int hitsToGoDown = 2;
    [SerializeField]
    Sprite pressedSprite;
    [SerializeField]
    Sprite notPressedSprite;



    bool triggered;
    Flasher flasher;
    SpriteRenderer spriteRenderer;

    void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        flasher = GetComponent<Flasher>();
        triggered = false;
        spriteRenderer.sprite = notPressedSprite;
    }

    public bool isPressed()
    {
        return triggered;
    }

    public void Press(){
        flasher.flash(spriteRenderer);
        hitsToGoDown--;
        if(hitsToGoDown <= 0){
            GoDown();
        }
    }

    void GoDown(){
        spriteRenderer.sprite = pressedSprite;
        triggered = true;
        Destroy(GetComponent<BoxCollider2D>());
    }
}
