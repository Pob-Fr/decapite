using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AudioOptionController : MonoBehaviour {

    public static void LoadSettings() {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OTSU/Decapite/Audio.xml";
        if (File.Exists(path)) { // load existing settings
            System.Xml.Serialization.XmlSerializer reader =
                new System.Xml.Serialization.XmlSerializer(typeof(AudioOption));
            StreamReader file = new StreamReader(path);
            options = (AudioOption)reader.Deserialize(file);
            file.Close();
        } else { // load default settings
            options = BuildDefaultAudio();
        }
    }

    public static void SaveSettings() {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OTSU/Decapite";
        Directory.CreateDirectory(path);
        System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(AudioOption));
        FileStream file = System.IO.File.Create(path + "/Audio.xml");
        writer.Serialize(file, options);
        file.Close();
    }

    private static AudioOption options;

    private static AudioOption BuildDefaultAudio() {
        return new AudioOption();
    }

    private void Start() {
        UpdateDisplay();
    }

    public void UpdateDisplay() {
        general.value = options.GENERAL_VOLUME;
        players.value = options.PLAYERS_VOLUME;
        zombies.value = options.ZOMBIES_VOLUME;
        jingles.value = options.JINGLES_VOLUME;
        music.value = options.MUSIC_VOLUME;
    }

    public Slider general;
    public Slider players;
    public Slider zombies;
    public Slider jingles;
    public Slider music;

    public void ChangeGeneralVolume(float value) {
        options.GENERAL_VOLUME = value;
    }
    public void ChangePlayersVolume(float value) {
        options.PLAYERS_VOLUME = value;
    }
    public void ChangeZombiesVolume(float value) {
        options.ZOMBIES_VOLUME = value;
    }
    public void ChangeJinglesVolume(float value) {
        options.JINGLES_VOLUME = value;
    }
    public void ChangeMusicVolume(float value) {
        options.MUSIC_VOLUME = value;
    }

    public static float GetPlayersVolume() {
        return options.GENERAL_VOLUME * options.PLAYERS_VOLUME / 10000;
    }
    public static float GetZombiesVolume() {
        return options.GENERAL_VOLUME * options.ZOMBIES_VOLUME / 10000;
    }
    public static float GetJinglesVolume() {
        return options.GENERAL_VOLUME * options.JINGLES_VOLUME / 10000;
    }
    public static float GetMusicVolume() {
        return options.GENERAL_VOLUME * options.MUSIC_VOLUME / 10000;
    }
}


public class AudioOption {
    public float GENERAL_VOLUME = 100;
    public float PLAYERS_VOLUME = 100;
    public float ZOMBIES_VOLUME = 80;
    public float JINGLES_VOLUME = 100;
    public float MUSIC_VOLUME = 80;
}

