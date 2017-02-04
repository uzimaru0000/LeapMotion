using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour {

    public float growthSpeed;
    public float meltTime;

    bool _isKrush;
    Rigidbody body;
    float timer;
    Vector3 defaultScale;

    public bool isKrush {
        set {
            _isKrush = value;
            if (!body) body = GetComponent<Rigidbody>();
            body.useGravity = value;
        }
        get {
            return _isKrush;
        }
    }

	// Use this for initialization
	void Start () {
        defaultScale = transform.localScale;
        transform.localScale = Vector3.one * 0.1f;
    }

    // Update is called once per frame
    void Update () {
		if(_isKrush) {
            transform.localScale = Vector3.Lerp(defaultScale, Vector3.zero, timer / meltTime);
            timer += Time.deltaTime;
            if(timer >= meltTime) {
                Destroy(gameObject);
            }
            transform.SetParent(null);
        } else {
            transform.localScale = Vector3.Lerp(transform.localScale, defaultScale, Time.deltaTime * growthSpeed);
        }
    }
}
