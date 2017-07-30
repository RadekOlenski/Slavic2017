using UnityEngine;

namespace Player
{
    public class RotationController : MonoBehaviour {

        // Use this for initialization
        void Start () {
		
        }
	
        // Update is called once per frame
        void Update () {
		
        }

        public void Rotate(Vector3 target)
        {
            Vector3 targetDir = target - transform.position;
            float step = 5f * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
            Debug.DrawRay(transform.position, newDir, Color.red);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }
}
