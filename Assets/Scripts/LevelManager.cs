using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    string fileName = "levelset1.txt";

    // Start is called before the first frame update
    void Start()
    {
        LoadFromFile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Parses through a level set text file
    /// http://ianparberry.com/research/sokoban/
    /// http://sokobano.de/wiki/index.php?title=Level_format
    /// </summary>
    void LoadFromFile()
    {
        StreamReader sr = new StreamReader(Application.dataPath + "/Data/LevelSets/" + fileName);
        string fileContents = sr.ReadToEnd();
        sr.Close();

        string[] lines = fileContents.Split("\n"[0]);
        foreach (string line in lines)
        {
            Debug.Log(line);
        }
    }
}
