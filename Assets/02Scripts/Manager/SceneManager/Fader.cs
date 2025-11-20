using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Fader : Singleton<Fader>
{        
    public enum TYPE
    {
        None,
        FadeInReady,
        AutoFadeIn
    }

    [SerializeField] private Image blindImage;

    [SerializeField] private float fadeTime;
    [SerializeField] private float delayTime;

    [SerializeField] private Fader.TYPE fadeInType;
    [SerializeField] private UnityEvent onEndFadeIn;

    private Action onEndFadeOut;

    public static bool isFading { get; private set; }
    public bool isStateFadeIn { get; private set; }

    private void Start()
    {
        this.blindImage.color = Color.black;
        this.blindImage.enabled = (this.fadeInType > Fader.TYPE.None);
        this.isStateFadeIn = false;
        if (this.fadeInType == Fader.TYPE.AutoFadeIn)
        {
            this.FadeIn(-1f);
        }
    }

    public void ChangeFadeTime(float fadeTime)
    {
        this.fadeTime = fadeTime;
    }

    // -1 이면 설정값으로 아니면 매계함수 값으로
    public void FadeIn(float time = -1f)
    {
        if (Fader.isFading)
        {
            return;
        }
        time = ((time == -1f) ? this.fadeTime : time);
        base.StartCoroutine(this.IEFadeIn(time));
    }

    public void FadeOut(Action onEndFadeOut, float time = -1f)
    {
        if (Fader.isFading)
        {
            return;
        }
        this.onEndFadeOut = onEndFadeOut;
        time = ((time == -1f) ? this.fadeTime : time);
        base.StartCoroutine(this.IEFadeOut(time));
    }

    public void Shutdown()
    {
        this.blindImage.enabled = true;
        this.blindImage.color = Color.black;
    }

    private IEnumerator IEFadeIn(float fadeTime)
    {
        Fader.isFading = true;
        this.isStateFadeIn = true;
        this.blindImage.color = Color.black;
        this.blindImage.enabled = true;

        yield return new WaitForSeconds(this.delayTime);

        float time = fadeTime;
        while (time > 0f)
        {
            time = Mathf.Clamp(time - Time.deltaTime, 0f, fadeTime);
            this.blindImage.color = new Color(0f, 0f, 0f, time / fadeTime);
            yield return null;
        }
        this.blindImage.enabled = false;
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        Fader.isFading = false;
        onEndFadeIn?.Invoke();
        yield break;
    }

    private IEnumerator IEFadeOut(float fadeTime)
    {
        Fader.isFading = true;
        this.isStateFadeIn = false;
        this.blindImage.color = Color.clear;
        this.blindImage.enabled = true;

        yield return new WaitForSeconds(this.delayTime);

        float time = 0f;
        AudioManager.Instance.PlaySFX(SFX.ChangeOption.ToString());
        while (time < fadeTime)
        {
            time = Mathf.Clamp(time + Time.deltaTime, 0f, fadeTime);
            this.blindImage.color = new Color(0f, 0f, 0f, time / fadeTime);
            yield return null;
        }
        this.blindImage.color = Color.black;

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        Fader.isFading = false;
        onEndFadeOut?.Invoke();
        yield break;
    }
}