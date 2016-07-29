using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using SimpleJSON;
using UnityEngine;

public class Animal 
{
	public List<string> categories = new List<string>();

	public string name;

	public bool CompareCategory(string category_passed)
	{
		foreach ( string category in categories ){
			if (category_passed == category) {
				return true;
			}
		}
		return false;
	}

	public bool saveToFile(string filepath)
	{
		string respJson = JsonUtility.ToJson (this);
		string file = filepath + this.name + ".animal.json";
		if (File.Exists (file)) {
			Debug.Log (file + " existente");
			return false;
		} else {
			var content = File.CreateText (file);
			content.WriteLine (respJson);
			content.Close ();
			return true;
		}
	}
}