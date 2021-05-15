using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSpectrum : MonoBehaviour
{
	public float scaleFactor = 10;

	public GameObject squarePrefab;

	public Transform baseLine;

	private AudioSource audioSource;

	private float[] samples;

	private GameObject[] squares;

	private GameObject spectrum;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();

		samples = new float[(int)RecordConductor.instance.songAudioSource.clip.length * 100];
		squares = new GameObject[(int)RecordConductor.instance.songAudioSource.clip.length * 100];

		GenerateSpectrum();

		CreateDisplayObjects();

	}

    private void Update()
    {
		spectrum.transform.position = Vector2.Lerp(baseLine.position, new Vector2(baseLine.position.x, -samples.Length * 0.1f + baseLine.position.y), (RecordConductor.instance.songAudioSource.time / RecordConductor.instance.songAudioSource.clip.length));
    }

	void GenerateSpectrum()
    {
		int sampleRate = (int)(RecordConductor.instance.songAudioSource.clip.frequency);

		Debug.Log(sampleRate);

		int samolesPerSec = sampleRate / 100;

		for (int i = 0; i < sampleRate * (int)RecordConductor.instance.songAudioSource.clip.length; i += samolesPerSec)
		{
			float[] x = new float[samolesPerSec];

			audioSource.clip.GetData(x, i);

			float sum = 0;

			for (int j = 0; j < samolesPerSec; j++)
			{
				sum += x[j];
			}

			samples[i / samolesPerSec] = sum / samolesPerSec;
		}
	}

    void CreateDisplayObjects()
	{
		spectrum = new GameObject();
		spectrum.name = "Spectrum";
		for (int i = 0; i < samples.Length; i++)
		{
			GameObject square = Instantiate(squarePrefab, Vector3.zero, Quaternion.identity);

			squares[i] = square;

			squares[i].transform.localScale = new Vector2((samples[i]) * scaleFactor * 0.1f, 0.1f);
			squares[i].transform.position = new Vector2(samples[i] * scaleFactor / 2 * 0.1f, i * 0.1f);
			squares[i].transform.SetParent(spectrum.transform);

		}
	}

}
