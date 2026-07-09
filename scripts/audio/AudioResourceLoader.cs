namespace BREU.Scripts.Audio;

public static class AudioResourceLoader
{
    public static AudioStream? TryLoad(string path, bool loop = false)
    {
        if (string.IsNullOrWhiteSpace(path) || !ResourceLoader.Exists(path))
        {
            return null;
        }

        var stream = ResourceLoader.Load<AudioStream>(path);
        if (stream == null)
        {
            return null;
        }

        if (!loop)
        {
            return stream;
        }

        if (stream is AudioStreamOggVorbis ogg)
        {
            var looped = (AudioStreamOggVorbis)ogg.Duplicate();
            looped.Loop = true;
            return looped;
        }

        if (stream is AudioStreamWav wav)
        {
            var looped = (AudioStreamWav)wav.Duplicate();
            looped.LoopMode = AudioStreamWav.LoopModeEnum.Forward;
            return looped;
        }

        return stream;
    }
}
