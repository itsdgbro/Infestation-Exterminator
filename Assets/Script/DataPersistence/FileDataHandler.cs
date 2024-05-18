using UnityEngine;
using System;
using System.IO;
public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "GurungDipak";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load()
    {
        // use Path.Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                // Load the serialized data from the file
                string dataToLoad = "";
                using (FileStream stream = new(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // decrypt if encrypted
                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                // deserialize the data from Json back into the C# object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        // use Path.Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {

            // create the directory the file will be written to if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // serialize the C# game data object into Json
            string dataToStore = JsonUtility.ToJson(data, true);

            // encryption option
            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            // write the serialized data to the file
            using (FileStream stream = new(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new(stream))
                {
                    writer.Write(dataToStore);
                }
            }

        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    // implementation of XOR encryption
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
}
