using UnityEngine;
using System.Collections;

public class MenuTitle : MonoBehaviour {

	public float SizeFactor;
	public float AddedYFactor;

	// Use this for initialization
	void Start () {

		float xScale = Screen.width * 0.001f * SizeFactor;
		float yScale = xScale * (9f / 16f);

		transform.localScale = new Vector3(xScale, yScale, 1.0f);

		float addedY = xScale * AddedYFactor;

		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + addedY, transform.localPosition.z);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
