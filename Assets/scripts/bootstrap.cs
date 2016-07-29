using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class bootstrap : MonoBehaviour {
	private Text fala;

	void Start () {
		fala = GameObject.Find ("fala").GetComponent<Text> ();

	}

	void Update () {
	}

	public void Quit(){
		Quit ();
	}

}


