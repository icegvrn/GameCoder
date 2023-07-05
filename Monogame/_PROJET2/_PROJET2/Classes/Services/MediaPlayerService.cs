#nullable enable
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;


namespace BricksGame
{
    public class MediaPlayerService : IMediaPlayerService
    {

        private List<SoundEffect?> sounds;
        public List<SoundEffect?> Sounds { get { return sounds; } private set { sounds = value; } }

        private SoundEffect? sound;
        public SoundEffect? Sound { get { return sound; } private set { sound = value; } }

        public MediaPlayerService(GameObject gameObject, int lvl)
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

        public MediaPlayerService()
        {
            sounds = new List<SoundEffect?>();
        }


        public MediaPlayerService(GameObject gameObject)
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

        public Song GetMusic(IMediaPlayerService.Musics music)
        {
          ContentManager content =  ServiceLocator.GetService<ContentManager>();
            switch(music)
            {
                case IMediaPlayerService.Musics.menu:
                    return content.Load<Song>("cool");
                case IMediaPlayerService.Musics.game:
                    return content.Load<Song>("background_gameplay");
                case IMediaPlayerService.Musics.gameOver:
                return content.Load<Song>("Sounds/Musics/defeat");
                case IMediaPlayerService.Musics.victory:
                return content.Load<Song>("Sounds/Musics/victory");
            default:
                    return content.Load<Song>("cool");
            }
        }
        public void PlayMusic(Song son, bool loop)
        {
            MediaPlayer.IsRepeating = loop;
            MediaPlayer.Play(son);
        }
        public void PauseMusic()
        {
            MediaPlayer.Pause();
        }

        public void StopMusic()
        {
            MediaPlayer.Stop();
        }
        public void ResumeMusic()
        {
            MediaPlayer.Resume();
        }

        public void Play(SoundEffect soundEffect)
        {
            soundEffect.Play();
        }

        public void Stop(SoundEffect soundEffect)
        {
            soundEffect.Dispose();
        }

        public void AddToSoundEffectsList(SoundEffect soundEffect)
        {
            sounds.Add(soundEffect);
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
                SoundEffect localSound = ServiceLocator.GetService<ContentManager>().Load<SoundEffect>("Sounds/" + p_sound);
                localSound.Play();
            }
        }

        public void Stop()
        {
            Sound.Dispose();
        }

        public static bool Exists(string path)
        {
            return File.Exists(ServiceLocator.GetService<ContentManager>().RootDirectory + "/" + path + ".xnb");
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
    }

}

