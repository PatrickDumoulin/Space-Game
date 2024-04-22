using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
	private float startTime;
	[SerializeField] float mainThrust = 100000f;
	[SerializeField] float rotationThrust = 1f;
	[SerializeField] AudioClip mainEngine;
	[SerializeField] AudioClip backgroundEngine;
	[SerializeField] AudioClip engineRelease;
	[SerializeField] AudioClip leftEngineRotateStart;
	[SerializeField] AudioClip leftEngineRotateStop;
	[SerializeField] AudioClip rightEngineRotateStart;
	[SerializeField] AudioClip rightEngineRotateStop;


	Rigidbody rb;
	

	AudioSource audioSourceMainEngine;
	AudioSource audioSourceBackgroundEngine;
	AudioSource audioSourceEngineRelease;
	AudioSource audioSourceLeftEngineRotateStart;
	AudioSource audioSourceLeftEngineRotateStop;
	AudioSource audioSourceRightEngineRotateStart;
	AudioSource audioSourceRightEngineRotateStop;

	[SerializeField] ParticleSystem mainEngineParticles;
	[SerializeField] ParticleSystem mainEngineParticles2;
	[SerializeField] ParticleSystem leftEngineParticles;
	[SerializeField] ParticleSystem rightEngineParticles;

	bool isEngineStarted = false;
	string currentScene;

	// Start is called before the first frame update
	void Start()
	{
		currentScene = SceneManager.GetActiveScene().name;
		startTime = Time.time;
		rb = GetComponent<Rigidbody>();
		AudioSource[] audioSources = GetComponents<AudioSource>();
		audioSourceMainEngine = audioSources[0];
		audioSourceBackgroundEngine = audioSources[1];
		audioSourceEngineRelease = audioSources[2];
		audioSourceLeftEngineRotateStart = audioSources[3];
		audioSourceLeftEngineRotateStop = audioSources[4];
		audioSourceRightEngineRotateStart = audioSources[5];
		audioSourceRightEngineRotateStop = audioSources[6];

	}

	// Update is called once per frame
	void Update()
	{
		StartEngine();

		if(currentScene == "TakeOff")
		{
			if (isEngineStarted && Time.time >= startTime + 20f)
			{
				ProcessThrust();

			}
		}
		else if(currentScene == "Lvl 1")
		{
			ProcessThrust();
			ProcessRotation();
		}

		
	}

	void StartEngine()
	{
		if (Input.GetKeyDown(KeyCode.E) && !isEngineStarted)
		{

			if (!audioSourceBackgroundEngine.isPlaying)
			{
				isEngineStarted = true;
				audioSourceBackgroundEngine.PlayOneShot(backgroundEngine);
			}
		}

	}

	void ProcessThrust()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			StartThrusting();
		}
		else if (Input.GetKeyUp(KeyCode.Space))
		{
			StopThrusting();
		}
	}

	void ProcessRotation()
	{

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			StartLeftRotation();
		}
		else if (Input.GetKeyUp(KeyCode.LeftArrow))
		{
			StopLeftRotation();
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			StartRightRotation();
		}
		else if (Input.GetKeyUp(KeyCode.RightArrow))
		{
			StopRightRotation();
		}

	}

	void StartThrusting()
	{
		rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

		if (!audioSourceMainEngine.isPlaying)
		{
			audioSourceEngineRelease.Stop();

			audioSourceMainEngine.PlayOneShot(mainEngine);

			mainEngineParticles.Play();
			mainEngineParticles2.Play();
		}




	}

	void StopThrusting()
	{
		audioSourceMainEngine.Stop();

		if (!audioSourceEngineRelease.isPlaying)
		{
			audioSourceEngineRelease.PlayOneShot(engineRelease);
		}

		mainEngineParticles.Stop();
		mainEngineParticles2.Stop();
	}

	private void StartRightRotation()
	{
		RotateRight();

		//Particles
		if (!leftEngineParticles.isPlaying)
		{
			leftEngineParticles.Play();
		}

		//Audio
		if (!audioSourceLeftEngineRotateStart.isPlaying)
		{
			audioSourceLeftEngineRotateStop.Stop();
			audioSourceLeftEngineRotateStart.PlayOneShot(leftEngineRotateStart);
		}
	}


	private void StopRightRotation()
	{
		leftEngineParticles.Stop();

		//audio
		if (audioSourceLeftEngineRotateStart.isPlaying)
		{
			audioSourceLeftEngineRotateStart.Stop();

			if (!audioSourceLeftEngineRotateStop.isPlaying)
			{
				audioSourceLeftEngineRotateStop.PlayOneShot(leftEngineRotateStop);
			}
		}
	}

	private void StartLeftRotation()
	{
		RotateLeft();

		//Particles
		if (!rightEngineParticles.isPlaying)
		{
			rightEngineParticles.Play();
		}

		//Audio
		if (!audioSourceRightEngineRotateStart.isPlaying)
		{
			audioSourceRightEngineRotateStop.Stop();
			audioSourceRightEngineRotateStart.PlayOneShot(rightEngineRotateStart);
		}
	}

	private void StopLeftRotation()
	{
		rightEngineParticles.Stop();

		//audio
		if (audioSourceRightEngineRotateStart.isPlaying)
		{
			audioSourceRightEngineRotateStart.Stop();

			if (!audioSourceRightEngineRotateStop.isPlaying)
			{
				audioSourceRightEngineRotateStop.PlayOneShot(rightEngineRotateStop);
			}
		}
	}

	private void RotateLeft()
	{
		ApplyRotation(rotationThrust);
	}


	private void RotateRight()
	{
		ApplyRotation(-rotationThrust);
	}

	void ApplyRotation(float rotationThisFrame)
	{
		rb.freezeRotation = true; // freezing rotation so we can manually rotate
		rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, 0f, rotationThisFrame * Time.deltaTime));
		rb.freezeRotation = false; // unfreezing rotation so the physics system can take over
	}

	
}
