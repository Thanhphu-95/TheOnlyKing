using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class CutsceneStarter : MonoBehaviour
{
    [SerializeField] private PlayableDirector directorScene;
    public UnityEvent onStarted;
    public UnityEvent onCompleted;

    void Reset()
    {
        directorScene = GetComponent<PlayableDirector>();
    }

    public void PlayCutscene()
    {
        if (directorScene == null)
        {
            Debug.LogError("[CutsceneStarter] Chưa gán PlayableDirector.");
            return;
        }

        // Đảm bảo cutscene chạy được
        if (Time.timeScale == 0f) Time.timeScale = 1f;

        // Tránh đăng ký trùng
        directorScene.stopped -= OnDirectorStopped;
        directorScene.stopped += OnDirectorStopped;

        onStarted?.Invoke();
        directorScene.Play();
        Debug.Log("[CutsceneStarter] Play()");
    }

    private void OnDirectorStopped(PlayableDirector d)
    {
        d.stopped -= OnDirectorStopped;
        onCompleted?.Invoke();
        Debug.Log("[CutsceneStarter] Stopped");
    }
}
