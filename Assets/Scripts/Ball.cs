using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public GameObject effect;
    public float rate;

    public float scale { get; set; }

    float currentScale;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        currentScale = Mathf.Lerp(currentScale, scale, Time.deltaTime / rate);
        transform.localScale = currentScale * Vector3.one;
	}

    void OnCollisionEnter(Collision other) {
        Destroy(gameObject);
        var colliders = Physics.OverlapSphere(transform.position, currentScale);
        foreach (var obj in colliders) {
            var body = obj.GetComponent<Rigidbody>();
            if (body) {
                body.AddExplosionForce(currentScale * 5, transform.position, currentScale, 3.0f, ForceMode.Impulse);
            }
        }
        var e = Instantiate(effect, transform.position, Quaternion.identity) as GameObject;
        Destroy(e, 5);
    }
}
