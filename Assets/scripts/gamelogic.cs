using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;


public class gamelogic : MonoBehaviour {

	public List<Question> questions;
	public List<Animal>  possibleAnimals;
	public List<string> aux_categories = new List<string>();

	/* resposta
	 * 0 - sim
	 * 1 - nao
	 * 2 - aguardando
	 */
	private int resp = 2; 
	private int question_index = 0;
	private string question_category;
	private int computer = 0;
	private int player = 0;
	private Text fala;
	private GameObject novo_animal;
	private int learning_type_element = 0;
	private string learning_animal;
	private string learning_categoria;
	static private string systemPath = System.Environment.CurrentDirectory;
	static private string filespath = systemPath + "/Assets/data/";


	void Start () { 
		this.fala = GameObject.Find ("fala").GetComponent<Text> ();
		this.questions = new List<Question> ();
		this.possibleAnimals = new List<Animal> ();
		this.novo_animal  = GameObject.Find("novo_animal");

		Reboot ();
	}

	void Update () {
		
	}

	void Reboot(){
		this.novo_animal.SetActive (false);
		this.question_index = 0;
		this.question_category = "fala";
		this.aux_categories.Clear();
		this.questions.Clear();
		this.possibleAnimals.Clear();

		// Learning Bootstrap
		this.learning_type_element = 0;
		this.learning_animal = "";
		this.learning_categoria = "";

		foreach (string file in System.IO.Directory.GetFiles(filespath,"*.animal.json")) {
			var filestream = JSON.Parse(LoadJson(file)); 
			Animal animal = new Animal ();
			animal.name = filestream ["name"].Value;
			foreach (object category in filestream["categories"].AsArray) {
				animal.categories.Add (category.ToString().Trim('"'));
			}; 
			this.possibleAnimals.Add (animal);
		}

		foreach (string file in System.IO.Directory.GetFiles(filespath,"*.question.json")) {
			var filestream = JSON.Parse(LoadJson(file)); 
			Question question = new Question ();
			//question.id = filestream ["id"].AsInt;
			question.description = filestream ["description"].Value;
			question.category = filestream ["category"].Value;
			this.questions.Add (question);
		}
		this.fala.text = this.questions[this.question_index].description;
		statistics ();

	}

	public void CheckResp(string category, int response){
		
		switch (category) {

		default:
			process_response(category, response);
			if (this.possibleAnimals.Count == 1) {
				this.question_category = "result";
				this.fala.text = "O Animal que você pensou: " + this.possibleAnimals [0].name + " !";
			} else {
				NextQuestion ();
			}
			break;

		case "result":
			if (response == 1) {
				leaderboard (false);
				this.question_category = "learning";
				fala.text = "Qual o animal ? Digite abaixo:";
				this.novo_animal.SetActive (true);
			} else {
				leaderboard (true);
				string leaderboard_txt = "CPU: " + this.computer.ToString () + "Player: " + this.player.ToString ();
				this.fala.text = leaderboard_txt;
				this.question_category = "leaderboard";
			}
			break;	
			
		case "learning":

			// pegue o animal novo
			if (this.learning_type_element == 0) {
				Text resp = GameObject.Find ("resp").GetComponent<Text> ();
				this.learning_animal = resp.text;
				resp.text = "digite o nome do animal"; // limpa
			}

			// pegue nova categoria
			this.fala.text = "Qual nova categoria ele pertence? (ex: voa, emplumado...)"; 
			if (this.learning_type_element == 1) {
				Text resp = GameObject.Find ("resp").GetComponent<Text> ();
				this.learning_categoria = resp.text;
				resp.text = "digite a categoria"; // limpa
				// crie questão baseado na categoria
				Question question = new Question();
				question.category = this.learning_categoria;
				question.description = "Esse animal (é):" + this.learning_categoria +" ?";
				question.saveToFile (filespath);
				// adicione na lista de categorias auxiliares
				this.aux_categories.Add(this.learning_categoria);
				// salve animal
				Animal animal = new Animal ();
				animal.categories = new List<string> (this.aux_categories);
				animal.name = this.learning_animal;
				animal.saveToFile (filespath);
				// reinicie
				Reboot();

			}
		
			this.learning_type_element = 1;

			Debug.Log (this.learning_animal);
			Debug.Log (this.learning_categoria);
			break;
			
		case "fala":
			NextQuestion ();
			if (this.possibleAnimals.Count == 1) {
				this.fala.text = "O Animal que você pensou: " + this.possibleAnimals [0].name + " !";
			}
			break;

		case "leaderboard":
			if (response == 0) {
				Reboot ();
			} else {
				ExitGame ();
			}
			break;
				
		}

		this.resp = 2;
	}

	public string LoadJson(string file) // Essa função deveria estar em outro arquivo 
	{
		StreamReader r = new StreamReader (file);
		string json = r.ReadToEnd();
		r.Close ();
		return json;
	}

	public void NextQuestion(){
		if (this.question_index + 1 < this.questions.Count) {
			this.question_index++;
			this.question_category = this.questions [this.question_index].category;
			this.fala.text = this.questions[this.question_index].description;
		} else { 
			this.question_category = "result";
		}
		//statistics ();
	}

	public void response(int value){
		this.resp = value;
		CheckResp(this.question_category, resp);
	}

	public void statistics(){
		Debug.Log ("Question Index: " + this.question_index);
		Debug.Log ("Quantidade de Animais: " + this.possibleAnimals.Count);
		Debug.Log ("Quantidade de Questões: " + this.questions.Count);
		Debug.Log ("Categoria da Questão: " + this.question_category);
	}

	/*
	 * Computer = true
	 * Player = false
	 */
	public void leaderboard(bool machine_win){
		if (machine_win) {
			this.computer++;
		} else {
			this.player++;
		}
	}

	public void learn(){

	}

	public void process_response(string category, int response){
		
		List<Animal> temp_animals = new List<Animal> (this.possibleAnimals);
		List<Question> temp_question = new List<Question> (this.questions);

		if (this.possibleAnimals.Count > 1) {
			foreach (Animal animal in possibleAnimals) {
				if (response == 1) {
					if (animal.CompareCategory (category)) {
						temp_animals.Remove (animal);
					}
				} else {
					if (!animal.CompareCategory (category)) {
						temp_animals.Remove (animal);
						aux_categories.Add (category);
					}
				}
				this.possibleAnimals = new List<Animal> (temp_animals);

				foreach (Question question in questions) {
					if (response == 1) {	
						if (question.CompareTo (category)) {
							temp_question.Remove (question);
						}
					} else {
						if (!question.CompareTo (category)) {
							temp_question.Remove (question);
						}
					}	
				}
				this.questions = new List<Question> (temp_question);
			}
		}
		statistics ();
	}

	void ExitGame() {
	}
}