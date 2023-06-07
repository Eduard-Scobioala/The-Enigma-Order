using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "EnigmaOrder";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    #region Save&Load

    public GameData Load(string profileId)
    {
        // Create the full path to the file
        string fullPath = Path.Combine(dataDirPath, profileId,  dataFileName);
        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                // Load the serialized data from the file
                string dataToLoad = "";
                using FileStream stream = new(fullPath, FileMode.Open);
                using StreamReader reader = new(stream);
                dataToLoad = reader.ReadToEnd();

                // Optionaly decrypt the data
                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                // Deserialize the data from the Json back to C# GameData
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.LogError("Error occured when trying to load the file: " + fullPath
                + "\n" + e.ToString());
            }
        }

        return loadedData;
    }

    public void Save(GameData data, string profileId)
    {
        // Create the full path to the file
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        try
        {
            // Create the directory for the file, if it does't exit already
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Serialize the C# GameData object into Json
            string dataToStore = JsonUtility.ToJson(data, true);

            // Optionaly encrypt the data
            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            // Write the serialized data to the file
            using FileStream stream = new (fullPath, FileMode.Create);
            using StreamWriter writer = new (stream);
            writer.Write(dataToStore);
        }
        catch(Exception e)
        {
            Debug.LogError("Error occured when trying to save the file: " + fullPath
                + "\n" + e.ToString());
        }
    }

    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profilesDictionary = new ();

        // Loop over all the directories in the data directory path
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;

            // Check the data to exist, otherwise the folder is not a profile
            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping directory which is not a profile: " + profileId);
                continue;
            }

            // Load the data from the profile and add it to the profiles dictionary
            GameData profileData = Load(profileId);
            if (profileData != null)
            {
                profilesDictionary.Add(profileId, profileData);
            }
            else
            {
                Debug.LogError("Trying to load profile, something went wrong. ProfileId: " + profileId);
            }
        }

        return profilesDictionary;
    }

    #endregion

    #region Encryption/Decryption

    // simple XOR encryption
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";

        // The process of XOR encryption/decryption
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }

        return modifiedData;
    }

    #endregion
}
