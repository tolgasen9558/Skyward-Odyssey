using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    
    [SerializeField]
    Sprite openedSprite;
    [SerializeField]
    Sprite closedSprite;
    [SerializeField]
    bool startsClosed;
	[SerializeField]
    List<Button> buttons;


    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;
	bool m_isOpened;
    bool triggered;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
		boxCollider.enabled = startsClosed;

        if(startsClosed){
            spriteRenderer.sprite = closedSprite;
            m_isOpened = false;
        }
        else{
            spriteRenderer.sprite = openedSprite;
            m_isOpened = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(!triggered && shouldTrigger()){
            Trigger();
        }
	}

	public bool isOpened(){
		return m_isOpened;
	}

    bool shouldTrigger(){
		if(buttons.Count == 0){
			return false;
		}
        //If one of the buttons is not pressed, return false
        foreach (Button button in buttons){
            if(!button.isPressed()){
                return false;
            }
        }
        return true;
    }

    public void Trigger(){
        triggered = true;

        if(m_isOpened){
            m_isOpened = false;
            spriteRenderer.sprite = closedSprite;
            boxCollider.enabled = true;
        }
        else{
            m_isOpened = true;
            spriteRenderer.sprite = openedSprite;
            boxCollider.enabled = false;
        }
    }
}
