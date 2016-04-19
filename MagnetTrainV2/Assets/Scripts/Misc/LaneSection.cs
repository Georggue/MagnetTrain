using UnityEngine;
using System.Collections;

public class LaneSection : MonoBehaviour {
    

    void OnBecameInvisible()
    {
        //Debug.Log("Start OnInvisible");
        if (this.gameObject!=null && Util.instance.IsBehindPlayer(this.gameObject))
        {           
            Invoke("CleanMeUp", 3);            
        }
    }
    void CleanMeUp()
    {
        if(this.gameObject != null)
        {
            ObjectPool.instance.returnLaneSectionToPool(this.gameObject);
            LaneManager.instance.triggerNewLane();           
        }
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    }
}
