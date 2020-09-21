using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainLoader : MonoBehaviour
{
    ////////////////////////////////////////////////////////
    /// Singelton 
    ////////////////////////////////////////////////////////
    private static MainLoader _instance;

    public static MainLoader Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    ////////////////////////////////////////////////////////
    /// Addressables 
    ////////////////////////////////////////////////////////
    public Slider progressbar;
    public Text size;
    private AssetReference menuRef;
    private AsyncOperationHandle opInProgress;

    public void LoadScene(string tag)
    {
        Debug.Log("Addressables Procedure Started for Loading Scene: " + tag + "@" + Time.time);
        Addressables.ClearDependencyCacheAsync(tag);
        Addressables.InitializeAsync().Completed += delegate {
            Debug.Log("Addressables.InitializeAsync.Completed:@" + Time.time);
            Addressables.GetDownloadSizeAsync(tag).Completed += delegate (AsyncOperationHandle<long> obj)
            {
                Debug.Log("Addressables.GetDownloadSizeAsync.Completed:@" + Time.time);
                float downloadSizeInMB = (float)obj.Result / 1024 / 1024;
                Debug.Log("GetDownloadSizeAsync: " + obj.Result + " bytes, which is " + downloadSizeInMB + " MB");

                if (size)
                    size.text += System.Environment.NewLine + "Size of download: " + downloadSizeInMB.ToString();

                Addressables.DownloadDependenciesAsync(tag).Completed += delegate (AsyncOperationHandle opDownloadDependencies)
                {
                    Debug.Log("Addressables.DownloadDependenciesAsync.Completed:" + opDownloadDependencies.Status + "@" + Time.time);
                    Addressables.LoadSceneAsync(tag).Completed += delegate (AsyncOperationHandle<SceneInstance> opLoadScene) {
                        Debug.Log("LoadSceneAsync.Completed: " + opLoadScene.Status + "@" + Time.time);
                        if (opLoadScene.Status == AsyncOperationStatus.Succeeded)
                        {
                        }
                    };
                };
            };
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadScene("default");
    }

    // Update is called once per frame
    void Update()
    {
        if (opInProgress.IsValid())
        {
            if (progressbar)
                progressbar.value = opInProgress.PercentComplete;
        }
    }
}
