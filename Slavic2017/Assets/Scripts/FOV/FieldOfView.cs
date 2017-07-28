using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FOV
{
    public abstract class FieldOfView : MonoBehaviour
    {
        [SerializeField] protected LayerMask ObstacleMask;
        [SerializeField] protected LayerMask TargetMask;

        [SerializeField] private float coroutineDelay = 0.4f;

        [SerializeField] [Range(0, 360)] public float ViewAngle;
        [SerializeField] public float ViewRadius;

        public List<GameObject> VisibleObjects { get; private set; }

        protected virtual void Start()
        {
            VisibleObjects = new List<GameObject>();
            StartDetectingCoroutine();
        }

        public void StartDetectingCoroutine()
        {
            StartCoroutine("FindObjectsWithDelay", coroutineDelay); //detecting targets with float delay
        }

        protected abstract void Awake();

        private IEnumerator FindObjectsWithDelay(float delay)
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
            var targetsInViewRadius = Physics.OverlapSphere(transform.position, ViewRadius, TargetMask);

            //foreach (var target in targetsInViewRadius)
            //{
            //    VisibleObjects.Add(target.gameObject);
            //}

            foreach (var collider in targetsInViewRadius)
            {
                var target = collider.gameObject;
                var dirToTarget = (target.transform.position - transform.position).normalized;
                //dirToTarget.y = 0;
                if (Vector3.Angle(transform.forward, dirToTarget) < ViewAngle / 2) //if target in field of view
                {
                    var dstToTarget = Vector3.Distance(transform.position, target.transform.position);
                    if (Physics.Raycast(transform.position, dirToTarget, dstToTarget, ObstacleMask)) continue;
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
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0.0f,
                Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}