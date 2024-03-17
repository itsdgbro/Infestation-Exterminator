using System;
using UnityEngine;


namespace FSR
{
    [RequireComponent(typeof(AudioSource))]
    public class FSR_Player : MonoBehaviour
    {
        private AudioSource m_AudioSource;
        public Transform foot;
        public float raycastSize = 10;
        [SerializeField] private FSR_Data data;

        private CharacterMovement character;

        private void Awake()
        {
            character = foot.GetComponentInParent<CharacterMovement>();
            if(character == null)
            {
                Debug.LogWarning("Not found");
            }
        }


        public void Start()
        {
            m_AudioSource = GetComponent<AudioSource>();
            if (foot == null)
            {
                Debug.Log("unassigned foot ");
            }
        }

        public void step()
        {
            RaycastHit hit;

            if (Physics.Raycast(foot.position, Vector3.down, out hit, raycastSize) && character.GetIsGrounded())
            {
                try {

                   FSR_SimpleSurface surface =  hit.transform.GetComponent<FSR_SimpleSurface>();
                    foreach (FSR_Data.SurfaceType surfaceData in data.surfaces)
                    {
                        if (surfaceData.name.Equals(surface.GetSurface()))
                        {
                            playSound(surfaceData);
                        }
                    }
                }
                catch
                {
                    try
                    {
                        FSR_TagedSurface surface = hit.transform.GetComponent<FSR_TagedSurface>();
                        foreach (FSR_Data.SurfaceType surfaceData in data.surfaces)
                        {
                            if (surfaceData.name.Equals(surface.GetSurface()))
                            {
                                playSound(surfaceData);
                            }
                        }
                    }
                    catch
                    {
                        try
                        {
                            FSR_TerrainSurface surface = hit.transform.GetComponent<FSR_TerrainSurface>();
                            foreach (FSR_Data.SurfaceType surfaceData in data.surfaces)
                            {
                                if (surfaceData.name.Equals(surface.GetSurface(transform.position)))
                                {
                                    playSound(surfaceData);
                                }
                            }

                        }
                        catch {

                            foreach (FSR_Data.SurfaceType surfaceData in data.surfaces)
                            {
                                if (surfaceData.name.Equals("GENERIC"))
                                {
                                    playSound(surfaceData);
                                }
                            }

                        }
                    }


                }
            }
        }


        private void OnDrawGizmos()
        {
            RaycastHit hit;
            //Vector3 raycastDirection = transform.position - foot.position;
            if (Physics.Raycast(foot.position, Vector3.down, out hit, raycastSize))
            {
                if(hit.transform.gameObject.CompareTag("Ground"))
                    Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawRay(foot.position, Vector3.down * raycastSize);
        }

        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        private void playSound(FSR_Data.SurfaceType surfaceType)
        {
            AudioClip[] soundEffects= surfaceType.soundEffects;

            int n = UnityEngine.Random.Range(0, soundEffects.Length);
            m_AudioSource.clip = soundEffects[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            soundEffects[n] = soundEffects[0];
            soundEffects[0] = m_AudioSource.clip;
        }

    }
}
