using UnityEngine;
using System.Collections;

public class PickUpAction : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider coll)
    {
      this.gameObject.SetActive(false);
    }
}
