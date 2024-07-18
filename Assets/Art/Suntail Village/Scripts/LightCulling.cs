using UnityEngine;

/*Script to disable lighting and shadows 
when moving away at a set distance*/
namespace Suntail
{
    public class LightCulling : MonoBehaviour
    {
        [SerializeField] private GameObject playerCamera;
        private float shadowCullingDistance = 15f;
        private float lightCullingDistance = 30f;
        private Light _light;
        public bool enableShadows = false;

        private void Awake()
        {
            _light = GetComponent<Light>();
            // _light.enabled = true;
            // _light.shadows = LightShadows.Soft;

        }

        private void Update()
        {
            //Calculate the distance between a given object and the light source
            float cameraDistance = Vector3.Distance(playerCamera.transform.position, gameObject.transform.position);
        
            if (cameraDistance <= shadowCullingDistance && enableShadows)
            {
                _light.shadows = LightShadows.Soft;
            }
            else
            {
                _light.shadows = LightShadows.None;
            }
        
            if (cameraDistance <= lightCullingDistance)
            {
                _light.enabled = true;
            }
            else
            {
                _light.enabled = false;
            }
        }
    }
}