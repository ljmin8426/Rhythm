using System;
using System.Collections.Generic;

[Serializable]
public class UserData
{
    public float musicVolume;       // 음악 볼륨
    public float sfxVolume;         // 효과음 볼륨
    public float hitSoundVolume;    // 히트 사운드 볼륨
    public int hitSoundIndex;       // 히트 사운드 종류

    public int noteSpeed;           // 노트 스피드
    public int noteSync;            // 노트 싱크
    public int musicSync;           // 음악 싱크
    public int musicIndex;          // 현재 음악 인덱스

    public int categoryIndex;       // 현재 카테고리
    public int difficulty;          // 현재 난이도

    public int noteType;            // 노트 타입

    public int hardnessMode;        // 판정 난이도
    public int autoMode;            // 자동 모드

    public int gearPosition;        // 기어 포지션
    public int gearOpacity;         // 기어 투명도
    public int backGroundMute;      // 백그라운드 온오프

    public UserData()
    {
        this.musicVolume = 0.2f;
        this.sfxVolume = 0.1f;
        this.hitSoundVolume = 5;
        this.hitSoundIndex = 0;

        this.noteSpeed = 50;
        this.noteSync = 0;
        this.musicSync = 0;
        this.musicIndex = 0;

        this.categoryIndex = 0;
        this.difficulty = 0;

        this.hardnessMode = 0;
        this.autoMode = 0;

        this.gearPosition = 1;
        this.gearOpacity = 0;

        this.backGroundMute = 0;

        this.noteType = 0;
    }
}
