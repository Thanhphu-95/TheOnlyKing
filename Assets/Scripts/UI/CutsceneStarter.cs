using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneStarter : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;

    public void PlayCutscene()
    {
        if (director == null) return;

        director.gameObject.SetActive(true);

        // dùng timeUpdateMode cho tất cả các phiên bản Unity
        director.timeUpdateMode = DirectorUpdateMode.UnscaledGameTime;

        director.Play();
    }
}
