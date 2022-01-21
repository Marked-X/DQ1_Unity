using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    class PlayerRayCheck : MonoBehaviour, IRayCheck
    {
        GameObject player;
        private void Awake()
        {
            player = GameManager.Instance.player;
        }
        public GameObject CheckTag(string tag)
        {
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position, player.GetComponent<PlayerMovement>().FacingDirection, 0.75f, LayerMask.GetMask("Interactable"));
            if (hit.collider != null && hit.collider.CompareTag(tag))
            {
                return hit.collider.gameObject;
            }
            else
            {
                return null;
            }
        }
    }
}
