using UnityEngine;

public class BundlePackage : Singleton<BundlePackage>
{
    [SerializeField] private SoundBundle soundBundle;
    [SerializeField] private MusicBundle musicBundle;

    public SoundBundle SoundBundle => this.soundBundle;
    public MusicBundle MusicBundle => this.musicBundle;

    private void Start()
    {
        soundBundle?.LoadAllFromStreamingAssets();
    }
}
