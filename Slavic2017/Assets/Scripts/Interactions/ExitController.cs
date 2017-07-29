using Enums;
using Events;
using Managers;
using UnityEngine;

namespace Interactions
{
    public class ExitController : MonoBehaviour
    {

        public GameObject ExitTile;
        private GameObject player;

        // Use this for initialization
        void Start () {
		
            player = GameObject.FindGameObjectWithTag(TagsEnum.Player);
        }
	
        // Update is called once per frame
        void Update () {

            if (Vector3.Distance(player.transform.position, new Vector3(ExitTile.transform.position.x, player.transform.position.y, ExitTile.transform.position.z)) < 0.1f)
            {
                EventManager.Instance.InvokeEvent(new GameOverEvent(true));
            }
        }
    }
}
