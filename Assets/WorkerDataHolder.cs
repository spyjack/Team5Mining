using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WorkerDataHolder : MonoBehaviour
{
    [SerializeField]
    List<string> firstNames = new List<string>();
    [SerializeField]
    List<string> lastNames = new List<string>();

    [SerializeField]
    List<Sprite> portraits = new List<Sprite>();

    // Start is called before the first frame update
    void Start()
    {
        ReadString("FirstNames.txt", firstNames);
        ReadString("LastNames.txt", lastNames);
    }

    void ReadString(string filename, List<string> output)
    {
        string path = "Assets/Resources/Eng/" + filename;

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        while (!reader.EndOfStream)
        {
            string lineText = reader.ReadLine();
            output.Add(lineText);
        }
        reader.Close();
    }
}
