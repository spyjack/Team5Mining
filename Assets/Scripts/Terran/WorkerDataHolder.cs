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
    List<Sprite> malePortraits = new List<Sprite>();

    [SerializeField]
    List<Sprite> femalePortraits = new List<Sprite>();

    [SerializeField]
    List<Sprite> otherPortraits = new List<Sprite>();

    // Start is called before the first frame update
    void Start()
    {
        ReadString("FirstNames.txt", firstNames);
        ReadString("LastNames.txt", lastNames);
    }

    public string GetName(Gender gender)
    {
        bool isGenderCorrect = false;
        int randFirstName = Random.Range(0, firstNames.Count);
        int randLastName = Random.Range(0, lastNames.Count);

        string newName = "";
        while(!isGenderCorrect)
        {
            newName = firstNames[randFirstName];
            switch (gender)
            {
                case Gender.Male:
                    if (newName.Contains("-m"))
                    {
                        newName = newName.Replace("-m", "");
                        newName = newName.Replace(" ", "");
                        print("New Name " + newName);
                        isGenderCorrect = true;
                    }
                    else
                    {
                        if (randFirstName + 1 < firstNames.Count)
                        {
                            randFirstName++;
                        }
                        else
                        {
                            randFirstName = Random.Range(0, firstNames.Count);
                        }
                    }
                    break;
                case Gender.Female:
                    if (newName.Contains("-f"))
                    {
                        newName = newName.Replace("-f", "");
                        newName = newName.Replace(" ", "");
                        isGenderCorrect = true;
                    }
                    else
                    {
                        if (randFirstName + 1 < firstNames.Count)
                        {
                            randFirstName++;
                        }else
                        {
                            randFirstName = Random.Range(0, firstNames.Count);
                        }
                    }
                    break;
                case Gender.Other:
                    newName = newName.Replace("-m", "");
                    newName = newName.Replace("-f", "");
                    newName = newName.Replace(" ", "");
                    isGenderCorrect = true;
                    break;
            }
        }

        return newName + " " + lastNames[randLastName];
    }

    public Sprite GetPortrait(Gender gender)
    {
        Sprite image = null;
            switch (gender)
            {
                case Gender.Male:
                image = malePortraits[Random.Range(0, malePortraits.Count)];
                    break;
                case Gender.Female:
                image = femalePortraits[Random.Range(0, femalePortraits.Count)];
                break;
                case Gender.Other:
                image = otherPortraits[Random.Range(0, otherPortraits.Count)];
                break;
            }
        

        return image;
    }

    public static void ReadString(string filename, List<string> output)
    {
        string path = Application.dataPath + "/StreamingAssets/Eng/" + filename;

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
