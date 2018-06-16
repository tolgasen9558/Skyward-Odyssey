using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(SpriteRenderer))]
public class Flasher : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Color flashColor = Color.red;
    [SerializeField]
    private float flashDuration;
    Color defaultColor; 

    public void Start()
    {
        defaultColor = GetComponentInChildren<SpriteRenderer>().material.color;
    }

    public void flash(SpriteRenderer spriteRenderer){
        this.spriteRenderer = spriteRenderer;
        StartCoroutine(flashEffect());
    }

    IEnumerator flashEffect(){
        float start = Time.time;
        spriteRenderer.sharedMaterial.color = flashColor;
        while(Time.time - start < flashDuration){
            yield return null;
        }
        spriteRenderer.sharedMaterial.color = defaultColor;
    }
}
