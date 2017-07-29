using System.Collections.Generic;
using Enums;
using Grid;
using UnityEngine;

namespace FOV
{
    public class PlayerFieldOfView : FieldOfView
    {
        [SerializeField] private float meshResolution;
        [SerializeField] private float edgeDistanceThreshold;
        [SerializeField] private int edgeResolveIterations;
        [SerializeField] private MeshFilter viewMeshFilterBasic;

        public bool DrawFOV;
        public GridController GridController;

        private Mesh viewMesh;

        #region Unity Methods

        protected override void Awake()
        {
            viewMesh = new Mesh {name = "View Mesh"};
            viewMeshFilterBasic.mesh = viewMesh;
        }

        void Update()
        {
        }

        private void LateUpdate()
        {
            if (DrawFOV)
            {
                DrawFieldOfView(viewMesh, ViewRadius, ViewAngle);
            }
            else
            {
                RemoveFieldOfView();
            }
        }

        private void OnDisable()
        {
            RemoveFieldOfView();
        }

        #endregion

        protected override void TakeActionOnVisibleObjects()
        {
            foreach (Tile tile in GridController.Tiles)
            {
                tile.CurrentTileType = tile.BaseTileType;
            }

            foreach (GameObject visibleObject in VisibleObjects)
            {
                if (visibleObject.layer == LayerMask.NameToLayer(LayersEnum.Grid))
                {
                    visibleObject.GetComponent<Tile>().CurrentTileType = TilesEnum.MonsterBlocked;
                }
            }
        }

        #region FOV Draw Methods

        private void DrawFieldOfView(Mesh viewMesh, float distance, float viewAngle)
        {
            int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
            float stepAngleSize = viewAngle / stepCount;
            List<Vector3> viewPoints = new List<Vector3>();
            ViewCastInfo oldViewCast = new ViewCastInfo();
            for (int i = 0; i <= stepCount; i++)
            {
                float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
                ViewCastInfo newViewCast = ViewCast(angle, distance);
                if (i > 0)
                {
                    bool edgeDistanceThresholdExceeded =
                        Mathf.Abs(oldViewCast.Distance - newViewCast.Distance) > edgeDistanceThreshold;
                    if (oldViewCast.IsHit != newViewCast.IsHit || (oldViewCast.IsHit && edgeDistanceThresholdExceeded))
                    {
                        EdgeInfo edge = FindEdge(oldViewCast, newViewCast, distance);
                        if (edge.PointA != Vector3.zero)
                        {
                            viewPoints.Add(edge.PointA);
                        }
                        if (edge.PointB != Vector3.zero)
                        {
                            viewPoints.Add(edge.PointB);
                        }
                    }
                }
                viewPoints.Add(newViewCast.Point);
                oldViewCast = newViewCast;
            }

            int vertexCount = viewPoints.Count + 1;
            Vector3[] vertices = new Vector3[vertexCount];
            int[] triangles = new int[(vertexCount + 2) * 3];
            vertices[0] = Vector3.zero;
            for (int i = 0; i < vertexCount - 1; i++)
            {
                vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
                if (i < vertexCount - 2)
                {
                    triangles[i * 3] = 0;
                    triangles[i * 3 + 1] = i + 1;
                    triangles[i * 3 + 2] = i + 2;
                }
            }
            viewMesh.Clear();
            viewMesh.vertices = vertices;
            viewMesh.triangles = triangles;
            viewMesh.RecalculateNormals();
        }

        public void RemoveFieldOfView()
        {
            viewMesh.Clear();
        }

        private ViewCastInfo ViewCast(float globalAngle, float distance)
        {
            Vector3 dir = DirFromAngle(globalAngle, true);
            RaycastHit hit;
            return Physics.Raycast(transform.position, dir, out hit, distance, ObstacleMask)
                ? new ViewCastInfo(true, hit.point, hit.distance, globalAngle)
                : new ViewCastInfo(false, transform.position + dir * distance, distance, globalAngle);
        }

        private EdgeInfo
            FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast,
                float distance) //bisection for optimizing rendered view
        {
            float minAngle = minViewCast.Angle;
            float maxAngle = maxViewCast.Angle;
            Vector3 minPoint = minViewCast.Point;
            Vector3 maxPoint = maxViewCast.Point;
            for (int i = 0; i < edgeResolveIterations; i++)
            {
                float angle = (minAngle + maxAngle) / 2;
                ViewCastInfo newViewCast = ViewCast(angle, distance);
                bool edgeDistanceThresholdExceeded = Mathf.Abs(minViewCast.Distance - newViewCast.Distance) >
                                                     edgeDistanceThreshold;
                if (newViewCast.IsHit == minViewCast.IsHit && !edgeDistanceThresholdExceeded)
                {
                    minAngle = angle;
                    minPoint = newViewCast.Point;
                }
                else
                {
                    maxAngle = angle;
                    maxPoint = newViewCast.Point;
                }
            }
            return new EdgeInfo(minPoint, maxPoint);
        }

        public struct ViewCastInfo
        {
            public bool IsHit;
            public Vector3 Point;
            public float Distance;
            public float Angle;

            public ViewCastInfo(bool isHit, Vector3 point, float distance, float angle)
            {
                IsHit = isHit;
                Point = point;
                Distance = distance;
                Angle = angle;
            }
        }

        public struct EdgeInfo
        {
            public Vector3 PointA;
            public Vector3 PointB;

            public EdgeInfo(Vector3 pointA, Vector3 pointB)
            {
                PointA = pointA;
                PointB = pointB;
            }
        }

        #endregion
    }
}