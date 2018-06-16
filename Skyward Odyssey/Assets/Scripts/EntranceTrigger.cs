using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceTrigger : MonoBehaviour {

	bool m_isTriggered;

	// Use this for initialization
	void Start () {
		m_isTriggered = false;
	}
	
	public bool isTriggered(){
		return m_isTriggered;
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if(collision.GetComponent<CapsuleController>() != null){
			m_isTriggered = true;
		}
	}
}
