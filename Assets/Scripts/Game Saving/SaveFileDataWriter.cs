using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveFileDataWriter
{
    public string saveDataDirectoryPath = "";
    public string saveFileName = "";

    // Check if one of this Character Slot already exists (Max 10 character slots)
    public bool CheckToSeeIfFileExists()
    {
        if(File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Used to delete character save files
    public void DeleteSaveFile()
    {
        File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
    }

    // Used to create a save file upon starting a new game
    public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
    {
        // Make a path to save the file (a location on the machine)
        string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

        try
        {
            // Create the Directory the file will be written to, if is does not already exists
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            Debug.Log("Creating save file, at save path: " + savePath);

            // Serialize the C# game data object into JSON
            string dataToStore = JsonUtility.ToJson(characterData, true);

            // Write the file to out System
            using (FileStream stream = new FileStream(savePath, FileMode.Create))
            {
                using (StreamWriter fileWriter = new StreamWriter(stream))
                {
                    fileWriter.Write(dataToStore);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR WHILST TRYING TO SAVE CHARACTER DATA, GAME NOT SAVED" + savePath + "\n" + ex);
        }
    }

    // Used to load a save file upon loading a previous game
    public CharacterSaveData LoadSaveFile()
    {
        CharacterSaveData characterData = null;
        // Make a path to load the file (a location on the machine)
        string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

        if (File.Exists(loadPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // Deserialize the data from JSON to UNITY
                characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
            }
            catch
            {

            }
        }

        return characterData;
    }
}
