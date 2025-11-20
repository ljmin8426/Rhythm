using System.IO;
using UnityEngine;

public class Paths : MonoBehaviour
{
    public static readonly string HIT_PATH = Path.Combine(Application.dataPath, "AssetBundle/Hit");

    public static readonly string MUSIC_PATH = Path.Combine(Application.dataPath, "AssetBundle/Music");

    public static readonly string STELL_PATH = Path.Combine(Application.dataPath, "AssetBundle/Stellive");

    public static readonly string MAIN_VIDEO_PATH = Path.Combine(Application.dataPath, "AssetBundle/MainVideo");

    public static readonly string BUNDLE_PATH = Path.Combine(Application.dataPath, "AssetBundle/Bundle");
}
