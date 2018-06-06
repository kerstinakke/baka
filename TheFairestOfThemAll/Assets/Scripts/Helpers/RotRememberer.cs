using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;

public class RotRememberer : MonoBehaviour {
    [ContextMenuItem("Set correct rotations", "SetCorrectRot")]
    [ContextMenuItem("Set correct positions", "SetCorrectPos")]
    public TextAsset memoryFile;
    public static bool done;

    [MenuItem("Tools/Clear PlayerPrefs")]
    private static void ClearPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    private void SetCorrectRot()
    {
        Transform puzzle = FindObjectOfType<Puzzle>().transform;
        foreach (string line in memoryFile.text.Split('\n')) {
            if (line == "")
                break;
            string[] bits = line.Split(';');
            print(bits.Length);
            Vector3 correct = new Vector3();
            string[] rot = bits[2].Split(',');
            for(int i=0; i<3; i++){
                correct[i] = Mathf.Round(float.Parse(rot[i]));
            }
            puzzle.Find(bits[0]).GetComponent<MovablePiece>().correctRot =  correct;
        }
        
    }

    private void SetCorrectPos()
    {
        Transform puzzle = FindObjectOfType<Puzzle>().transform;
        foreach (string line in memoryFile.text.Split('\n'))
        {
            if (line == "")
                break;
            string[] bits = line.Split(';');
            print(bits.Length);
            Vector3 correct = new Vector3();
            string[] pos = bits[1].Split(',');
            for (int i = 0; i < 3; i++)
            {
                correct[i] = float.Parse(pos[i]);
            }
            puzzle.Find(bits[0]).GetComponent<MovablePiece>().setCorrectPos(correct);
        }

    }

    public static void WriteString(GameObject puzzle)
    {
        done = false;
        string path = "Assets/Rendering/textures/puzzles/corrects" + SceneManager.GetActiveScene().buildIndex + ".txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path);
        foreach (MovablePiece piece in puzzle.GetComponentsInChildren<MovablePiece>())
        {
            Transform body = piece.transform.Find("Body");
            writer.WriteLine(piece.name+";"+piece.transform.position.x + "," + piece.transform.position.y + "," + piece.transform.position.z + ";"
                +body.localEulerAngles.x+","+ body.localEulerAngles.y+","+body.localEulerAngles.z);
            print(body.localEulerAngles);
        }
        writer.Close();
        done = true;
        
    }
}
