using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

	public Door entranceDoor;
	public Door exitDoor;
	public List<Button> buttons;
	public List<Enemy> enemies;
	public EntranceTrigger entranceTrigger;

	bool hasButtons;


	// Use this for initialization
	void Start () {
		hasButtons = buttons.Count != 0;
		if(hasButtons){
			setAllButtonsEnabledTo(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(entranceTrigger.isTriggered()){
			if (entranceDoor.isOpened()) entranceDoor.Trigger();
			if (exitDoor.isOpened()) exitDoor.Trigger();
		}
		if(shouldReleaseDoors()){
			if (!entranceDoor.isOpened()) entranceDoor.Trigger();
            if (!exitDoor.isOpened()) exitDoor.Trigger();
		}
		if(hasButtons && areAllEnemiesDead()){
			setAllButtonsEnabledTo(true);
		}

	}

	bool shouldReleaseDoors(){
		if (!areAllEnemiesDead()) return false;

		if (!areAllButtonsPressed()) return false;

		return true;
	}

	bool areAllEnemiesDead(){
		foreach (Enemy enemy in enemies) {
            if (enemy != null && !enemy.isDead()) {
                return false;
            }
        }
		return true;
	}

	bool areAllButtonsPressed(){
		if (hasButtons) {
            foreach (Button button in buttons) {
                if (!button.isPressed()) {
                    return false;
                }
            }
        }
		return true;
	}

	void setAllButtonsEnabledTo(bool isEnabled){
        foreach (Button button in buttons) {
            button.gameObject.SetActive(isEnabled);
        }
	}
}
