using UnityEngine;
using System.Collections;

public class effects : MonoBehaviour {
	private Light spark;
	private float limit_positive_x, limit_negative_x;
	private bool going = true;
	Vector3 position;

	void Start () {
		this.spark = GameObject.Find("spark").GetComponent<Light> ();
		this.limit_positive_x = 200f;
		this.limit_negative_x = 100f;

	}
	
	void Update () {
		CheckDirection ();
		if (this.going) {
			this.spark.transform.position = new Vector3 (position.x++, position.y, position.z);
		} else {
			this.spark.transform.position = new Vector3 (position.x--, position.y, position.z);
		};
	}

	void CheckDirection(){
		this.position = this.spark.transform.position;

		if (position.x == limit_positive_x) {
			this.going = false;
		};

		if (position.x == limit_negative_x ) {
			this.going = true;
		};	
	}
}
