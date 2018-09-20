using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveSettings {
    

    public static void saveSettings()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedSettings.gd");
        bf.Serialize(file, Settings.settingsHere);
        file.Close();

    }

    public static void loadSettings()
    {
        if (File.Exists(Application.persistentDataPath + "/savedSettings.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedSettings.gd", FileMode.Open);
            Settings.settingsHere = (Settings)bf.Deserialize(file);
            file.Close();
            //Debug.Log(Settings.settingsHere);
        }
        else
        {
            Settings.masterVol = 1f;  
            Settings.musicVol = 1f;    
            Settings.sfxVol = 1f;    
            Settings.mute = false;    
        }
    }

    public static void getSettings(float masterVol, float musicVol, float sfxVol, bool mute)
    {
        Settings.masterVol = masterVol;
        Settings.musicVol = musicVol;
        Settings.sfxVol = sfxVol;
        Settings.mute = mute;
    }

}
