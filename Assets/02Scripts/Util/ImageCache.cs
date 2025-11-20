using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ImageCache
{
    private static Dictionary<string, Sprite> cache = new Dictionary<string, Sprite>(); 
     
    public static Sprite Get(string path) 
    { 
        // 파일 경로가 없으면 종료 
        if (!File.Exists(path)) 
            return null; 

        if (string.IsNullOrEmpty(path))
            return null;

        // Dictionary 에서 캐싱한 Sprite를 반환
        if (cache.TryGetValue(path, out Sprite sprite))
        {
            return sprite;
        }

        // 파일 경로가 있으면 새로운 데이터 캐싱 
        byte[] fileData = File.ReadAllBytes(path); 
        Texture2D texture = new Texture2D(2, 2); 

        if (!texture.LoadImage(fileData))
            return null;

        sprite = Sprite.Create(texture, 
                               new Rect(0, 0, texture.width, texture.height),
                               new Vector2(0.5f, 0.5f));

        cache[path] = sprite;
        return sprite;
    }
}
