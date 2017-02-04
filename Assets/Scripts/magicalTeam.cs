using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magicalTeam : MonoBehaviour {

    [ColorUsage(true, true, 0, 8, 1/3, 3)]
    public Color color;
    public float scaleSpeed;
    public float speed;

    float timer;
    Vector3 scale;
    bool isDestroy;

	// Use this for initialization
	void Start () {
        foreach(Transform child in transform) {
            var mate = child.GetComponent<MeshRenderer>().material;
            mate.EnableKeyword("_EmissionColor");
            mate.SetColor("_EmissionColor", color);
        }

        scale = transform.localScale;
        transform.localScale = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale = Vector3.Lerp(transform.localScale, scale, Time.deltaTime * scaleSpeed);

        int i = 0;
		foreach(Transform child in transform) {
            child.Rotate(Vector3.forward * speed * i * Time.deltaTime);
            i++;
        }

        if(isDestroy) {
            if(transform.localScale.sqrMagnitude < 0.1f) {
                Destroy(gameObject);
            }
        }

        timer += Time.deltaTime;
	}

    public void Finish() {
        scale = Vector3.zero;
        isDestroy = true;
        speed *= 2;
    }
}
