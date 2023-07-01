#nullable enable
using BricksGame;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

public class SoundContainer
{
    private List<SoundEffect?> sounds;
    public List<SoundEffect?> Sounds { get { return sounds; } private set { sounds = value; } }

    private SoundEffect? sound; 
    public SoundEffect? Sound { get { return sound;  } private set { sound = value; } }

    public SoundContainer(GameObject gameObject, int lvl)
    {
        sounds = new List<SoundEffect?>();
        string rootFolder = GetFolder(gameObject);


        if (gameObject is Monster)
        {
            string[] characterStates = Enum.GetNames<Gamesystem.CharacterState>();

            foreach (string state in characterStates)
            {
                string soundPath = "";

                if (!state.Contains("l_"))
                {
                    soundPath = "Sounds/" + rootFolder + state + "/" + lvl;
                }
                if (soundPath == "" || !Exists(soundPath))
                {
                  
                    Sounds.Add(null);
                }
                else
                {
                    sound = ServiceLocator.GetService<ContentManager>().Load<SoundEffect>(soundPath);
                    Sounds.Add(sound);
                }

            }
        }
    }

    public SoundContainer()
    {
        sounds = new List<SoundEffect?>();
    }


    public SoundContainer(GameObject gameObject)
    {
        Sounds = new List<SoundEffect?>();

        string rootFolder = GetFolder(gameObject);

        string[] gameObjectStates = null;

        if (gameObject is Monster || gameObject is Player)
        {
            gameObjectStates = Enum.GetNames<Gamesystem.CharacterState>();
        }
        else if (gameObject is Ball)
        {
            gameObjectStates = Enum.GetNames<Gamesystem.BallState>();
        }

            foreach (string state in gameObjectStates)
            {
                string soundPath = "";

                if (!state.Contains("l_"))
                {
                    soundPath = "Sounds/" + rootFolder + state;
                }

                if (soundPath == "" || !Exists(soundPath))
                {
                    Sounds.Add(null);
                }
                else
                {
                    sound = ServiceLocator.GetService<ContentManager>().Load<SoundEffect>(soundPath);
                    Sounds.Add(sound);
                }

            }

    }

    private string GetFolder(GameObject gameObject)
    {
        string folder = "";

        if (gameObject is Monster)
        {
            folder = "Monsters/";
        }

        else if (gameObject is Player)
        {
            folder = "Player/";
        }
        else if (gameObject is Ball)
        {
            folder = "Ball/";
        }


        else
        {
            folder = "/";
        }

        return folder;
    }

    public void Play(Gamesystem.CharacterState state)
    {
     if (Sounds[(int)state] != null)
        {
            Sounds[(int)state].Play();
        }
            
        
    }

    public void Play(Gamesystem.CharacterState state, float p_volume)
    {

        if (Sounds[(int)state] != null)
        {
            Sounds[(int)state].Play(p_volume, 0, 0);
        }
   
   
    }

    public void Play(Gamesystem.BallState state)
    {
            if (Sounds[(int)state] != null)
            {
                Sounds[(int)state].Play();
            }
        }

    public void Play(Gamesystem.BallState state, float p_volume)
    {

        if (Sounds[(int)state] != null)
        {
            Sounds[(int)state].Play(p_volume, 0, 0);
        }

    }

    public void Play(string p_sound)
    {
        if (p_sound != "")
        {
            SoundEffect localSound = ServiceLocator.GetService<ContentManager>().Load<SoundEffect>("Sounds/"+p_sound);
            localSound.Play();
        }
    }

    public void Stop()
    {
        Sound.Dispose();
    }

    public static bool Exists(string path)
    {
        Debug.WriteLine("SOUNDCONTAINGER : Je regarde si : " + ServiceLocator.GetService<ContentManager>().RootDirectory + "/" + path + ".xnb" + " existe et je dis " + File.Exists(ServiceLocator.GetService<ContentManager>().RootDirectory + "/" + path + ".xnb"));
      return File.Exists(ServiceLocator.GetService<ContentManager>().RootDirectory + "/" + path + ".xnb");
    }
}