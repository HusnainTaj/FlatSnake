using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LBScore {
	public string name;
	public string dificulty;
	public int score;

	public LBScore(string name, string dificulty, int score) {
		this.name = name;
		this.dificulty = dificulty;
		this.score = score;
	}
}
