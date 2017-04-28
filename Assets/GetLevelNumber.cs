using UnityEngine;
using System.Collections;

public class GetLevelNumber : MonoBehaviour {
    public TextMesh tm;
	// Use this for initialization
	void Start () {
        tm.text = transform.parent.name;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
