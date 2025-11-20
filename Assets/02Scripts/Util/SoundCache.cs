using System.Collections.Generic;
using System.IO;
using UnityEngine;
using FMOD;

public static class SoundCache
{
    private static readonly Dictionary<string, Sound> cache = new();

    public static bool TryGet(FMOD.System system, string path, out Sound sound)
    {
        sound = default;

        if (string.IsNullOrEmpty(path) || !File.Exists(path))
        {
            UnityEngine.Debug.LogWarning($"[SoundCache] 유효하지 않은 경로: {path}");
            return false;
        }

        if (cache.TryGetValue(path, out sound))
        {
            if (sound.hasHandle())
                return true;

            // 핸들이 사라졌다면 다시 로드
            cache.Remove(path);
        }

        RESULT result = system.createSound(path, MODE.DEFAULT, out sound);
        if (result != RESULT.OK)
        {
            UnityEngine.Debug.LogError($"[SoundCache] 사운드 생성 실패: {path} ({result})");
            sound = default;
            return false;
        }

        cache[path] = sound;
        return true;
    }

    public static void Clear(string path)
    {
        if (cache.TryGetValue(path, out Sound sound))
        {
            if (sound.hasHandle())
            {
                sound.release();
                sound.clearHandle();
            }

            cache.Remove(path);
        }
    }

    public static void ClearAll()
    {
        foreach (var kvp in cache)
        {
            Sound sound = kvp.Value;
            if (sound.hasHandle())
            {
                sound.release();
                sound.clearHandle();
            }
        }

        cache.Clear();
    }
}
