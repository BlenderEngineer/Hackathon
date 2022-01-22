using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private GameObject _lobbyCanvas;
    [SerializeField] private Image _progressBar;
    [SerializeField] private AudioSource Sound;
    private float _target;

    public static LevelLoader Instance;
    // Start is called before the first frame update
    void Awake()
    {
        //Sound = GetComponent<AudioSource>();
        // Sound.clip = audio;
        //Sound.Play();
        _loaderCanvas.SetActive(true);
        StartCoroutine(WaitForSound(Sound));
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void LoadScene(string sceneName)
    {
        _target = 0;
        _progressBar.fillAmount = 0;

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        _loaderCanvas.SetActive(true);

        do
        {
            await Task.Delay(100);
            _target = scene.progress;
        } while (scene.progress < .9f);

        await Task.Delay(1000);

        scene.allowSceneActivation = true;
        _loaderCanvas.SetActive(false);
    }
    
    public IEnumerator WaitForSound(AudioSource Sound)
    {
        yield return new WaitUntil(() => Sound.isPlaying == false);
        // or yield return new WaitWhile(() => audiosource.isPlaying == true);
        if (gameObject != null)
            _loaderCanvas.SetActive(false);
             _lobbyCanvas.SetActive(true);
    }

    private void Update()
    {
        _progressBar.fillAmount = Mathf.MoveTowards(_progressBar.fillAmount, _target, 3 * Time.deltaTime);
    }
}
