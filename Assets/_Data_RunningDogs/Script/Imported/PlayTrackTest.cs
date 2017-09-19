using UnityEngine;
using System.Collections;

public class PlayTrackTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GlobalBGMManager.Instance.PlayTrack(2);
	}
	
}
