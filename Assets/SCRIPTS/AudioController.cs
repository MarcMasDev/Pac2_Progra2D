using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SoundType
{
    Jump,
    Coin,
    PowerUp,
    PowerUpLost,
    Hit,
    BlockDestroyed,
    GameOver
}

[System.Serializable]
public class SoundEntry
{
    public SoundType type;
    public AudioSource source;
}

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    [SerializeField] private SoundEntry[] sounds; //multiples audiosources per si volem escoltar 2 sons a la vegada.


    //Sigleton: us d'un manager per tenir tots els audiosource centralitzats.
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Play(SoundType soundType)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].type == soundType)
            {
                if (sounds[i].source != null)
                {
                    sounds[i].source.Play();
                }
                return;
            }
        }
    }
}
