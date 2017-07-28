using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FOV
{
    public abstract class FieldOfView : MonoBehaviour
    {
        [SerializeField]
        protected LayerMask obstacleMask;
        [SerializeField]
        protected LayerMask targetMask;

        [SerializeField]
        private float coroutineDelay = 0.4f;

        [SerializeField]
        [Range(0, 360)]
        public float viewAngle;
        [SerializeField]
        public float viewRadius;

        public List<GameObject> VisibleObjects { get; private set; }

        protected virtual void Start()
        {
            VisibleObjects = new List<GameObject>();
            StartDetectingCoroutine();
        }

        public void StartDetectingCoroutine()
        {
            StartCoroutine("FindObjetsWithDelay", coroutineDelay); //detecting targets with float delay
        }

        protected abstract void Awake();

        private IEnumerator FindObjetsWithDelay(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                FindVisibleObjects();
                TakeActionOnVisibleObjects();
            }
        }

        private void FindVisibleObjects()
        {
            VisibleObjects.Clear();
            var targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

            //foreach (var target in targetsInViewRadius)
            //{
            //    VisibleObjects.Add(target.gameObject);
            //}

            foreach (var collider in targetsInViewRadius)
            {
                var target = collider.gameObject;
                var dirToTarget = (target.transform.position - transform.position).normalized;
                //dirToTarget.y = 0;
                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) //if target in field of view
                {
                    var dstToTarget = Vector3.Distance(transform.position, target.transform.position);
                    if (Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)) continue;
                    //Debug.Log(gameObject.name + ": " + target.gameObject.name + " spotted.");
                    VisibleObjects.Add(target);
                }
            }

        }

        protected abstract void TakeActionOnVisibleObjects();

        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
                angleInDegrees += transform.eulerAngles.y;
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0.0f, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    } 
}