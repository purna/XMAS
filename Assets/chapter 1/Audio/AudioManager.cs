using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GameObject audioPrefab;

    public void PlaySound(AudioClip audioClip, float volume, float pitch, float pitchVariation)
    {
        GameObject prefab = Instantiate(audioPrefab, transform.position, Quaternion.identity, transform);
        AudioSource audioSource = prefab.GetComponent<AudioSource>();

        prefab.name = audioClip.name;

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.pitch = pitch + Random.Range(-pitchVariation, + pitchVariation);
        audioSource.Play();

        Destroy(prefab, audioClip.length);
    }

    public void PlayRandomSound(AudioClip[] audioClip, float volume, float pitch, float pitchVariation)
    {
        GameObject prefab = Instantiate(audioPrefab, transform.position, Quaternion.identity, transform);
        AudioSource audioSource = prefab.GetComponent<AudioSource>();
        AudioClip randomClip = audioClip[Random.Range(0, audioClip.Length)];

        prefab.name = randomClip.name;


        audioSource.clip = randomClip;
        audioSource.volume = volume;
        audioSource.pitch = pitch + Random.Range(-pitchVariation, + pitchVariation);
        audioSource.Play();

        Destroy(prefab, randomClip.length);
    }
}
