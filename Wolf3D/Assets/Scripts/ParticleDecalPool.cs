using ch.sycoforge.Decal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDecalPool : MonoBehaviour
{
    public int maxDecals;
    public float decalSizeMin;
    public float decalSizeMax;
    public Texture[] bloodSplatterTextures;
    [SerializeField]
	private GameObject bloodSplatPrefab;
	[SerializeField]
	private GameObject bloodDripPrefab;
    [SerializeField]
    private GameObject decalManager;

    //public ParticleSystem decalParticleSystem;
    //private int particleDecalDataIndex;
    private BloodParticleData[] particleData;
    private ParticleSystem.Particle[] particles;

    private Queue<EasyDecal> decalsInPool;
    private Queue<GameObject> decalsActiveInWorld;

	public ParticleSystem particleLauncher;
	public ParticleSystem bloodSprayLauncher;
    public Gradient particleColorGradient;
	public ParticleSystem splatterParticles;
	public ParticleSystem dripParticles;
    
	private bool CR_Running = false;
	public List<EasyDecal> decalsOnCeiling;

    List<ParticleCollisionEvent> collisionEvents;

    private void Awake()
	{
		//Override EasyDecal's Instantiation delegation with our own one which uses the EZ_Pooling pool.
        EasyDecal.Instantiation = PoolInstantiation;
    }
	
	//Whenever a EasyDecal command instantiates a decal (either through Project, ProjectAt, Instantiate, etc...), this method will be used to spawn a decal from the EZ_Pooling pool rather than through EasyDecal's method.
	private static EasyDecal PoolInstantiation(GameObject decalPrefab, GameObject parent, Vector3 position, Quaternion rotation)
	{
		string PoolName = decalPrefab.name;
		EasyDecal clone = EZ_Pooling.EZ_PoolManager.GetPool(PoolName).Spawn(decalPrefab.transform, position, rotation).GetComponent<EasyDecal>();
		clone.Reset(true);

        return clone;
    }

    // Start is called before the first frame update
    void Start()
    {
        //InitializeDecals();

	    collisionEvents = new List<ParticleCollisionEvent>();
	    decalsOnCeiling = new List<EasyDecal>();

        particles = new ParticleSystem.Particle[maxDecals];
        particleData = new BloodParticleData[maxDecals];
        for (int i = 0; i < maxDecals; i++)
        {
            particleData[i] = new BloodParticleData();
        }
    }
    
	void Update()
	{
		if (decalsOnCeiling.Count > 0 && !CR_Running)
		{
			InvokeRepeating("drips", 0.0f, 9.0f);
			CR_Running = true;
		}
	}

    //private void InitializeDecals()
    //{
    //    decalsInPool = new Queue<EasyDecal>();
    //    decalsActiveInWorld = new Queue<GameObject>();

    //    for (int i = 0; i < maxDecals; i++)
    //    {
    //        InstantiateDecal();
    //    }
    //}

    //private void InstantiateDecal()
    //{
    //    GameObject spawned = Instantiate(bloodSplatPrefab, decalManager.transform);
    //    //spawned.transform.SetParent(decalManager.transform);
    //    int rand = Random.Range(0, bloodSplatterTextures.Length);
    //    spawned.GetComponent<Renderer>().material.SetTexture("_MainTex", bloodSplatterTextures[rand]);

    //    //decalsInPool.Enqueue(spawned);
    //    spawned.SetActive(false);
    //}

    private void spawnDecal(ParticleCollisionEvent particleCollisionEvent, GameObject surface)
    {
        float randScale = Random.Range(decalSizeMin, decalSizeMax);
	    float randAngle = Random.Range(0, 180);
	    
        //decal.transform.parent = particleCollisionEvent.colliderComponent.gameObject.transform;
        //decal.transform.position = particleCollisionEvent.intersection;
        //decal.transform.rotation = Quaternion.FromToRotation(-Vector3.forward, particleCollisionEvent.normal);
        //float randScale = Random.Range(decalSizeMin, decalSizeMax);

        //decal.transform.localScale = new Vector3(randScale, randScale, randScale);
        //float randAngle = Random.Range(0, 180);

        //if (Mathf.Abs(Vector3.Dot(transform.up, Vector3.down)) < 0.125f)
        //{
        //    decal.transform.localEulerAngles += new Vector3(0, 0, randAngle);
        //} else
        //{
        //    decal.transform.localEulerAngles += new Vector3(0, randAngle, 0);
	    //}
	    
	    //float castRadius = 1f;
	    
	    //RaycastHit[] hits = Physics.SphereCastAll(particleCollisionEvent.intersection, castRadius, Vector3.zero, Vector3.Distance(Camera.main.transform.position, particleCollisionEvent.intersection) + 2);
	    //Vector3 averageNormal = particleCollisionEvent.normal;
 
	    //// Check if sphere cast hit something
	    //if (hits.Length > 0)
	    //{
		//    Debug.Log(hits.Length);
		//    foreach (RaycastHit hit in hits)
		//    {
		//	    // Sum all collison point normals
		//	    averageNormal += hit.normal;
		//    }
	    //}
                 
	    //// Normalize normal
	    //averageNormal /= hits.Length + 1;
	    
	    float dotUpDown = Vector3.Dot(particleCollisionEvent.normal, Vector3.down);
	    
	    if ((dotUpDown >= -1.01f && dotUpDown <= -0.99f) || (dotUpDown > 0.99f && dotUpDown <= 1.01f))
		{
	        //decal.GetComponent<Renderer>().material.color = particleColorGradient.Evaluate(Random.Range(0f, 1f));
		    EasyDecal decal = EasyDecal.ProjectAt(bloodSplatPrefab.gameObject, particleCollisionEvent.colliderComponent.gameObject, particleCollisionEvent.intersection, particleCollisionEvent.normal, randAngle, new Vector3(randScale, 0.06f, randScale));
		    //decal.transform.position -= decal.transform.up * 0.01f;
		    //if (Mathf.Abs(Vector3.Dot(decal.transform.up, Vector3.down)) < 0.125f)
		    //{
			//    //decalsOnWalls.Add(decal);
		    //}
		    
		    float dotDown = Vector3.Dot(decal.transform.up, Vector3.down);
		    
		    if ((dotDown > 0.99f && dotDown <= 1.01f))
		    {
			    decalsOnCeiling.Add(decal);
		    } else
		    {
		    	if (decalsOnCeiling.Contains(decal))
		    	{
		    		decalsOnCeiling.Remove(decal);
		    	}
		    }
		}
	    else
	    {
	    	float rand = Random.Range(0f, 1f);
	    	//float rand = 0.01f;
	    	if (rand <= 0.11f)
	    	{
	    		float randScaleDripX = Random.Range(0.35f, 0.6f);
	    		float randScaleDripZ = Random.Range(0.15f, 0.24f);
		    	EasyDecal decal = EasyDecal.ProjectAt(bloodDripPrefab.gameObject, particleCollisionEvent.colliderComponent.gameObject, particleCollisionEvent.intersection, particleCollisionEvent.normal, 0, new Vector3(randScaleDripX, 0.06f, randScaleDripZ));
			    decal.Baked = false;
			    decal.transform.rotation = Quaternion.LookRotation(Vector3.up, particleCollisionEvent.normal);
			    decal.transform.RotateAround(decal.transform.position, particleCollisionEvent.normal, 90);
		    }
		    else
		    {
		    	EasyDecal decal = EasyDecal.ProjectAt(bloodSplatPrefab.gameObject, particleCollisionEvent.colliderComponent.gameObject, particleCollisionEvent.intersection, particleCollisionEvent.normal, randAngle, new Vector3(randScale, 0.06f, randScale));
		    }
	    }
        
	    //if (decalsOnWalls.Count > 0 && !CR_Running)
	    //{
	    //	StartCoroutine(moveDown());
	    //}

        //decal.SetActive(true);
        //decalsActiveInWorld.Enqueue(decal);

        //if (decalsInPool.Count < maxDecals-1) {
        //    decalsInPool.Enqueue(decal);
        //} else
        //{
        //    EasyDecal oldestDecal = decalsInPool.Dequeue();
        //    //oldestDecal.des
        //}

        //if (Mathf.Abs(Vector3.Dot(decal.transform.up, Vector3.down)) < 0.125f)
        //{
        //    StartCoroutine(moveDown(decal));
        //}

        //decal = EasyDecal.ProjectAt(bloodSplatPrefab.gameObject, decalManager, particleCollisionEvent.intersection, particleCollisionEvent.normal);
        //decal.enabled = true;

        //decalsActiveInWorld.Enqueue(decal);
    }
    
	private void drips()
	{
		//EasyDecal[] decals = new EasyDecal[decalsOnCeiling.Count];
		//decalsOnCeiling.CopyTo(decals);
		foreach (EasyDecal decal in decalsOnCeiling)
		{
			float randTime = Random.Range(2.0f, 9.0f);
			var emitParams = new ParticleSystem.EmitParams();
			emitParams.applyShapeToPosition = true;
			emitParams.position = decal.gameObject.transform.position;
			StartCoroutine(drip(emitParams, randTime));			
		}
		
		if (decalsOnCeiling.Count == 0)
		{
			CR_Running = false;
		}
	}
	
	private IEnumerator drip(ParticleSystem.EmitParams emitParams, float time)
	{
		yield return new WaitForSeconds(time);
		dripParticles.Emit(emitParams, 1);
	}
	
	//public IEnumerator moveDown()
	//{
	//	CR_Running = true;
	//	EasyDecal[] decals = new EasyDecal[decalsOnCeiling.Count];
	//	decalsOnCeiling.CopyTo(decals);
	//	foreach (EasyDecal decal in decals)
	//	{
	//		Vector3 originalPosition = decal.GetComponent<DecalPooling>().originalPosition;
	//		yield return new WaitForSeconds(0.01f);
	//		transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y - 0.0001f, transform.position.z), 1);
	//		Debug.Log("Original y-Position: " + originalPosition.y + ", Current y-Position: " + transform.position.y);
	//		if (originalPosition.y - 0.1f < transform.position.y)
	//		{
	//			decalsOnCeiling.Remove(decal);
	//		}
	//	}
	//	if (decalsOnCeiling.Count > 0)
	//	{
	//		StartCoroutine(moveDown());
	//	}
	//	CR_Running = false;
	//	//if ()
	//	//{
	//	//    StartCoroutine(moveDown(easyDecal));
	//	//    Debug.Log("Starting Another Routine!");
	//	//}
	//}

    private void OnParticleCollision(GameObject other)
    {
	    ParticlePhysicsExtensions.GetCollisionEvents(particleLauncher, other, collisionEvents);

        for (int i = 0; i < collisionEvents.Count; i++)
        {
            //EmitAtLocation(collisionEvents[i]);

            //Mesh mesh = collisionEvents[i].colliderComponent.GetComponent<Mesh>();

	        spawnDecal(collisionEvents[i], other);
        }
    }

    void EmitAtLocation(ParticleCollisionEvent particleCollisionEvent)
    {
        splatterParticles.transform.position = particleCollisionEvent.intersection;
        splatterParticles.transform.rotation = Quaternion.LookRotation(particleCollisionEvent.normal);
        ParticleSystem.MainModule psMain = splatterParticles.main;
        psMain.startColor = particleColorGradient.Evaluate(Random.Range(0f, 1f));

        splatterParticles.Emit(2);
    }

	public void spawnBloodParticles(int damage, Vector3 enemyPos)
	{
		particleLauncher.transform.position = enemyPos;
		
		particleLauncher.Emit((int)damage/3);
		bloodSprayLauncher.Emit((int)damage/4);
    }

    //public void ParticleHit(ParticleCollisionEvent particleCollisionEvent, Gradient colorGradient)
    //{
    //    SetParticleData(particleCollisionEvent, colorGradient);
    //    DisplayParticles();
    //}

    //void SetParticleData(ParticleCollisionEvent particleCollisionEvent, Gradient colorGradient)
    //{
    //    if (particleDecalDataIndex >= maxDecals)
    //    {
    //        particleDecalDataIndex = 0;
    //    }

    //    //Record collision position, rotation, size, color
    //    particleData[particleDecalDataIndex].position = particleCollisionEvent.intersection;

    //    if (particleCollisionEvent.normal != Vector3.zero)
    //    {
    //        Vector3 particleRotationEuler = Quaternion.LookRotation(particleCollisionEvent.normal).eulerAngles;
    //        particleRotationEuler.z = Random.Range(0, 360);
    //        particleData[particleDecalDataIndex].rotation = particleRotationEuler;
    //    } else
    //    {
    //        particleData[particleDecalDataIndex].rotation = Vector3.zero;
    //    }

    //    particleData[particleDecalDataIndex].size = Random.Range(decalSizeMin, decalSizeMax);
    //    particleData[particleDecalDataIndex].color = colorGradient.Evaluate(Random.Range(0f, 1f));

    //    particleDecalDataIndex++;
    //}

    //void DisplayParticles()
    //{
    //    //for (int i = 0; i < particleData.Length; i++)
    //    //{
    //    //    particles[i].position = particleData[i].position;
    //    //    particles[i].rotation3D = particleData[i].rotation;
    //    //    particles[i].startSize = particleData[i].size;
    //    //    particles[i].startColor = particleData[i].color;
    //    //}

    //    var emitParams = new ParticleSystem.EmitParams();
    //    emitParams.position = particleData[particleDecalDataIndex-1].position;
    //    emitParams.rotation3D = particleData[particleDecalDataIndex - 1].rotation;
    //    emitParams.startSize = particleData[particleDecalDataIndex - 1].size;
    //    emitParams.startColor = particleData[particleDecalDataIndex - 1].color;

    //    decalParticleSystem.Emit(emitParams, 1);

    //    //decalParticleSystem.SetParticles(particles, particles.Length);
    //}
}
