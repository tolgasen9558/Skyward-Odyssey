using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    Transform followTarget;
	[SerializeField]
	float smoothSpeed = 10;
    //float followViewportWidth = 1f;

    CapsuleController player;
	float zOffset;

	// Use this for initialization
	void Start () {
        if(followTarget != null){
            player = followTarget.GetComponent<CapsuleController>();
        }
		zOffset = transform.position.z;
	}

	void FixedUpdate() {
		if(player != null && !player.isDead()){
			Vector2 smoothedPosition = Vector2.Lerp(
				transform.position, player.transform.position, smoothSpeed * Time.deltaTime);
			transform.position = new Vector3(smoothedPosition.x
                    , smoothedPosition.y, zOffset);
		}	
	}
}
