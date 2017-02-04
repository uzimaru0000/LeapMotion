using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceParent : MonoBehaviour {

    public GameObject obj;
    public int num;
    public bool isKrush;

    float timer;

	void Start () {
		for(int i = 0; i < num; i++) {
            var pos = transform.position + Random.insideUnitSphere;
            var rotate = Random.rotation;
            var go = Instantiate(obj, pos, rotate) as GameObject;
            go.transform.localScale = Random.insideUnitSphere * 3;
            go.transform.SetParent(transform);
        }
	}
	
	void Update () {
        if(isKrush) {
            foreach(Transform child in transform) {
                child.GetComponent<Ice>().isKrush = true;
            }

            var colliders = Physics.OverlapSphere(transform.position, 10);
            foreach (var collider in colliders) {
                var body = collider.GetComponent<Rigidbody>();
                if (body) {
                    body.AddExplosionForce(3, transform.position, 1, 0.1f, ForceMode.Impulse);
                }
            }

            Destroy(gameObject, 0.1f);
        }
	}
}
