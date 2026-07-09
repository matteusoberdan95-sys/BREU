namespace BREU.Scripts.Audio;

public static class AudioResourceLoader
{
    public static AudioStream? TryLoad(string path)
    {
        if (string.IsNullOrWhiteSpace(path) || !ResourceLoader.Exists(path))
        {
            return null;
        }

        return ResourceLoader.Load<AudioStream>(path);
    }
}
