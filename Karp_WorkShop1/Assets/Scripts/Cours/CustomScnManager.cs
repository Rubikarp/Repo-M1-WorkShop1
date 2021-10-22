using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Flags]
public enum Element
{
    water = 1 <<0, // pow(2,0) = 1
    fire = 1 << 1,// pow(2,1) = 2
    wind = 1 << 2,// pow(2,2) = 4
    earth = 1 << 4// pow(2,4) = 8
}

public class CustomScnManager : MonoBehaviour
{
    public int firstScnLoad;
    public List<int> openScenes = new List<int>();
    //OPTIMISATION
    //unsigned int on peu le considérer comme un tableau de 32bit
    uint openScenesOpti;

    //SingletonPattern
    public static CustomScnManager instance;
    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        OpenScene(firstScnLoad);
    }

    public bool IsSceneOpen(int index)
    {
        //opérateur AND : a&b
        // => discrimine
        //opérateur OR  : a|b
        // => inclue/addition
        //opérateur XOR : a^b
        // => peut être utiliser pour de l'aléatoire ou crypter des fichiers
        //https://en.wikipedia.org/wiki/XOR_cipher
        //opérateur NOT : ~a
        // => inverse

        //uint singleBit = 1;           00000001
        //singleBit <<= index; bitShift 00100000

        //index de 0 à 31 pour uint ou 0 à 64 si ulong
        uint singleBit = (uint)1 << index;

        uint mask = openScenesOpti & singleBit;
        return mask > 0;
    }
    public void MarkSceneOpen(int index)
    {
        uint singleBit = (uint)1 << index;

        //openScenesOpti = openScenesOpti | singleBit;
        openScenesOpti |= singleBit;
    }
    public void MarkSceneClosed(int index)
    {
        uint singleBit = (uint)1 << index;
        uint inversedSingleBit = ~singleBit;
        openScenesOpti &= inversedSingleBit;
    }
    public void OpenScene(int index)
    {

        if (openScenes.Contains(index))
        {
            return;
        }

        SceneManager.LoadScene(index, LoadSceneMode.Additive);


        openScenes.Add(index);
    }
    public IEnumerator OpenSceneAsync(int index)
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        // Wait until the asynchronous scene fully loads
        while (asyncLoad.progress <0.9f)
        {
            //chargement fini = 0.9 parce que unity
            Debug.Log(asyncLoad.progress);
            yield return null;
        }
        //Scene charge wait activation
        asyncLoad.allowSceneActivation = true;
    }
}
