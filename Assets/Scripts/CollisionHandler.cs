using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
	[SerializeField] AudioClip crash;
	[SerializeField] AudioClip success;
	float levelLoadDelayCrash = 15f;
	float levelLoadDelaySuccess = 4f;

	AudioSource audioSourceMainEngine;
	AudioSource audioSourceBackgroundEngine;
	AudioSource audioSourceCrash;
	AudioSource audioSourceSuccess;
	AudioSource audioSourceEngineRelease;

	[SerializeField] ParticleSystem successParticles;

	bool isTransitioning = false;
	bool collisionDisabled = false;

	//Fragment
	public GameObject originalObject;
	public GameObject fracturedObject;
	public GameObject explosionVFX;
	public float explosionMinForce = 40000000;
	public float explosionMaxForce = 10000000;
	public float explosionForceRadius = 100000;
	public float fragScaleFactor = 1;

	private GameObject fractObj;


	void Start()
	{
		AudioSource[] audioSources = GetComponents<AudioSource>();
		audioSourceMainEngine = audioSources[0];
		audioSourceBackgroundEngine = audioSources[1];
		audioSourceCrash = audioSources[2];
		audioSourceSuccess = audioSources[3];
		audioSourceEngineRelease = audioSources[4];

	}

	private void Update()
	{
		RespondToDebugKeys();

		if (originalObject.transform.position.y > 1800)
		{
			StartSuccessSequence();
		}
    }

	void RespondToDebugKeys()
	{
		if (Input.GetKey(KeyCode.L))
		{
			LoadNextLevel();
		}

		else if (Input.GetKey(KeyCode.C))
		{
			collisionDisabled = !collisionDisabled;
		}
	}


	private void OnCollisionEnter(Collision other)
	{
		if(isTransitioning || collisionDisabled) { return; } // ignore collisions when dead

		switch (other.gameObject.tag)
		{
			case "Friendly":
				Debug.Log("This thing is friendly");
				break;
			case "Finish":
				StartSuccessSequence();
				break;
			default:
				StartCrashSequence();
				break;
		}
	}

	void StartCrashSequence() 
	{
		
			isTransitioning = true;
			audioSourceBackgroundEngine.Stop();
			audioSourceMainEngine.Stop();
			audioSourceEngineRelease.Stop();
			audioSourceCrash.PlayOneShot(crash);

			//add particle effect on crash
			GetComponent<Movement>().enabled = false;
			Invoke("ReloadLevel", levelLoadDelayCrash);

		//Fragment
		Explode();

	}

	void StartSuccessSequence()
	{
		isTransitioning = true;
		audioSourceBackgroundEngine.Stop();
		audioSourceMainEngine.Stop();
		audioSourceEngineRelease.Stop();
		audioSourceSuccess.PlayOneShot(success);
		successParticles.Play();

		GetComponent<Movement>().enabled = false;
		Invoke("LoadNextLevel", levelLoadDelaySuccess);
		

	}
	
	void LoadNextLevel()
	{
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		int nextSceneIndex = currentSceneIndex + 1;
		if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
		{
			nextSceneIndex = 0;
		}
		SceneManager.LoadScene(nextSceneIndex);
	}


	void ReloadLevel()
	{
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(currentSceneIndex);
	}

	void Explode()
	{
		if(originalObject != null)
		{
			originalObject.SetActive(false);

			if(fracturedObject != null)
			{
				fractObj = Instantiate(fracturedObject, originalObject.transform.position, originalObject.transform.rotation) as GameObject;

				AudioSource explosionAudio = fractObj.GetComponent<AudioSource>();
				if (explosionAudio != null)
				{
					explosionAudio.PlayOneShot(crash);
				}

				foreach (Transform t in fractObj.transform)
				{
					var rb = t.GetComponent<Rigidbody>();

					if (rb != null)
					{
						rb.AddExplosionForce(Random.Range(explosionMinForce, explosionMaxForce), originalObject.transform.position, explosionForceRadius);

						//StartCoroutine(Shrink(t,2));

					}

					
 				}

				if (explosionVFX != null)
				{
					GameObject exploVFX = Instantiate(explosionVFX, originalObject.transform.position, originalObject.transform.rotation);
					Destroy(exploVFX, 7);
				}

			}

		}

			
	}
}
