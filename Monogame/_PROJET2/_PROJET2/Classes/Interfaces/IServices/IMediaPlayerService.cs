using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;


namespace BricksGame
{
    public interface IMediaPlayerService
    {
        enum Musics { menu, game, gameOver, victory}

         void PlayMusic(Song son, bool loop);

        void PauseMusic();

        void StopMusic();

        void ResumeMusic();

        void Play(SoundEffect soundEffect);


        void Stop(SoundEffect soundEffect);


        void AddToSoundEffectsList(SoundEffect soundEffect);

        Song GetMusic(Musics musics);

        void Play(Gamesystem.CharacterState state);


        void Play(Gamesystem.CharacterState state, float p_volume);


        void Play(Gamesystem.BallState state);


        void Play(Gamesystem.BallState state, float p_volume);


        void Play(string p_sound);


        void Stop();
    




    }
}
