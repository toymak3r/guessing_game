using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using SimpleJSON;
using UnityEngine;

public class Question 
{
	public string category;
	public string description;
	
	public bool loadFromJson(string json){

		return true;
	}

	public bool CompareTo(string category_passed)
	{
		if (category_passed == category) {
			return true;
		} else {
			return false;
		}
	}

	public bool saveToFile(string filepath)
	{
		string respJson = JsonUtility.ToJson (this);
		string file = filepath + this.category + ".question.json";
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