using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class LoadingSceneManager : MonoBehaviour
{
    private static LoadingSceneManager instance = null;
    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static LoadingSceneManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    public string currentscene;
    public string nextscene;
    [Header("=== UI ===")]
    public GameObject Shadow;
    Image progressbar;
    public Action SceneLoadedHandler;
    public void OnSceneLoaded()
    {
        SceneLoadedHandler?.Invoke();
    }
    public void loadScene(string scenename)
    {
        StartCoroutine(Fade_LoadScene(scenename));
    }
    IEnumerator Fade_LoadScene(string scenename)
    {
        if(GameManager.Instance != null) GameManager.Instance.movable = false;
        Color color = Color.black;
        Shadow.SetActive(true);
        float lerpelasped = 0;
        while (lerpelasped <= 1)
        {
            color.a = Mathf.Lerp(0, 1, lerpelasped);
            Shadow.GetComponent<Image>().color = color;
            lerpelasped += Time.deltaTime;
            yield return null;
        }
        currentscene = SceneManager.GetActiveScene().name; //로딩창 꾸미기
        nextscene = scenename;
        SceneManager.LoadScene("Loading");
        yield return null;
        Shadow.SetActive(false);
        for (int i = 0; i < GameManager.Instance?.transform.childCount; i++)
        {
            GameManager.Instance.transform.GetChild(i).gameObject.SetActive(false);
        }
        AsyncOperation op = SceneManager.LoadSceneAsync(nextscene);
        op.allowSceneActivation = false;
        progressbar = GameObject.Find("loadingbar").GetComponent<Image>();
        float elasped = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            elasped += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                progressbar.fillAmount = Mathf.Lerp(progressbar.fillAmount, op.progress, elasped);
                if (progressbar.fillAmount >= op.progress)
                {
                    elasped = 0f;
                }
            }
            else
            {
                progressbar.fillAmount = Mathf.Lerp(progressbar.fillAmount, 1f, elasped);
                if (progressbar.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;
                }
            }
        }
        for (int i = 0; i < GameManager.Instance.transform.childCount; i++)
        {
            GameManager.Instance.transform.GetChild(i).gameObject.SetActive(true);
        }
        Shadow.SetActive(true);
        lerpelasped = 0;
        while (lerpelasped <= 1)
        {
            color.a = Mathf.Lerp(1,0 , lerpelasped);
            Shadow.GetComponent<Image>().color = color;
            lerpelasped += Time.deltaTime;
            yield return null;
        }
        Shadow.SetActive(false);
        GameManager.Instance.movable = true;
        OnSceneLoaded();
    }
}