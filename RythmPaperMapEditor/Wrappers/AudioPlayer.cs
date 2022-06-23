using System;
using NAudio.Wave;

namespace RythmPaperMapEditor.Wrappers
{
    public class AudioPlayer
    {
        public AudioFileReader AudioFileReader { get; private set; }

        private DirectSoundOut _output;

        public event Action OnPlaybackResume;
        public event Action OnPlaybackStop;
        public event Action OnPlaybackPause;

        public AudioPlayer(string filepath, float volume)
        {
            AudioFileReader = new AudioFileReader(filepath) { Volume = volume };

            _output = new DirectSoundOut(50);
            _output.PlaybackStopped += PlaybackStopped;

            var wc = new WaveChannel32(AudioFileReader);
            wc.PadWithZeroes = false;

            _output.Init(wc);
        }

        public void Play(PlaybackState playbackState, double currentVolumeLevel)
        {
            if (playbackState == PlaybackState.Stopped || playbackState == PlaybackState.Paused)
            {
                _output.Play();
            }

            AudioFileReader.Volume = (float) currentVolumeLevel;

            OnPlaybackResume?.Invoke();
        }

        private void PlaybackStopped(object sender, StoppedEventArgs e)
        {
            Dispose();
            OnPlaybackStop?.Invoke();
        }

        public void Stop()
        {
            _output?.Stop();
        }

        public void Pause()
        {
            if (_output == null) return;
            _output.Pause();
            OnPlaybackPause?.Invoke();
        }

        public void TogglePlayPause(double currentVolumeLevel)
        {
            if (_output != null)
            {
                if (_output.PlaybackState == PlaybackState.Playing)
                {
                    Pause();
                }
                else
                {
                    Play(_output.PlaybackState, currentVolumeLevel);
                }
            }
            else
            {
                Play(PlaybackState.Stopped, currentVolumeLevel);
            }
        }

        public void Dispose()
        {
            if (_output != null)
            {
                if (_output.PlaybackState == PlaybackState.Playing)
                {
                    _output.Stop();
                }
                _output.Dispose();
                _output = null;
            }

            if (AudioFileReader == null) return;
            
            AudioFileReader.Dispose();
            AudioFileReader = null;
        }

        public double GetLenghtInSeconds()
        {
            if (AudioFileReader != null)
            {
                return AudioFileReader.TotalTime.TotalSeconds;
            }

            return 0;
        }

        public double GetPositionInSeconds()
        {
            return AudioFileReader != null ? AudioFileReader.CurrentTime.TotalSeconds : 0;
        }

        public float GetVolume()
        {
            if (AudioFileReader != null)
            {
                return AudioFileReader.Volume;
            }
            return 1;
        }

        public void SetPosition(double value)
        {
            if (AudioFileReader != null)
            {
                AudioFileReader.CurrentTime = TimeSpan.FromSeconds(value);
            }
        }

        public void SetVolume(float value)
        {
            if (_output != null)
            {
                AudioFileReader.Volume = value;
            }
        }
    }
}