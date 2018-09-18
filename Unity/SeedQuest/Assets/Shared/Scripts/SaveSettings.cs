using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveSettings {


    public static Settings settings;


    public static void saveSettings()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedSettings.gd");
        bf.Serialize(file, settings);
        file.Close();

    }

    public static void loadSettings()
    {
        if (File.Exists(Application.persistentDataPath + "/savedSettings.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedSettings.gd", FileMode.Open);
            settings = (Settings)bf.Deserialize(file);
            file.Close();
        }
    }

    public static void getSettings(float masterVol, float musicVol, float sfxVol, bool mute)
    {
        settings.masterVol = masterVol;
        settings.musicVol = musicVol;
        settings.sfxVol = sfxVol;
        settings.mute = mute;
    }

}
