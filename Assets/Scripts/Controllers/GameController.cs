using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
    void Start () {
        Debug.Log("Welcome to Dungeons in SPAAAACE!");
    }

    void Update () {
        // Close if escape is pushed
        if (Input.GetKeyDown("escape")) {
            Application.Quit();
        }
    }
}
