using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class key : MonoBehaviour
{
    [SerializeField] private GameObject room;
    [SerializeField] private Tilemap wallsTilemap;
    private float MaxConDistanceX = 25f;
    private float MaxConDistanceY = 15f;
    [SerializeField] private LayerMask roomLayer;
    [SerializeField] private Vector3Int coordsDoorsUp1;
    [SerializeField] private Vector3Int coordsDoorsUp2;
    [SerializeField] private Vector3Int coordsDoorsDown1;
    [SerializeField] private Vector3Int coordsDoorsDown2;
    [SerializeField] private Vector3Int coordsDoorsRight1;
    [SerializeField] private Vector3Int coordsDoorsRight2;
    [SerializeField] private Vector3Int coordsDoorsLeft1;
    [SerializeField] private Vector3Int coordsDoorsLeft2;
    [SerializeField] private TileBase doorUp1;
    [SerializeField] private TileBase doorUp2;
    [SerializeField] private TileBase doorDown1;
    [SerializeField] private TileBase doorDown2;
    [SerializeField] private TileBase doorRight1;
    [SerializeField] private TileBase doorRight2;
    [SerializeField] private TileBase doorLeft1;
    [SerializeField] private TileBase doorLeft2;
    [SerializeField] private GameObject ChangeLevelUp;
    [SerializeField] private GameObject ChangeLevelLeft;
    [SerializeField] private GameObject ChangeLevelRight;
    [SerializeField] private GameObject ChangeLevelDown;

    private void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(this.gameObject);

            Vector2[] CheckDirections = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

            foreach (Vector2 direction in CheckDirections)
            {
                float maxDistance = direction == Vector2.up || direction == Vector2.down ? MaxConDistanceY : MaxConDistanceX;

                Vector2 raycastOrigin = room.GetComponentInChildren<Grid>().transform.position;

                RaycastHit2D[] hits = Physics2D.RaycastAll(raycastOrigin, direction, maxDistance, roomLayer);
                Debug.DrawRay(raycastOrigin, direction * maxDistance, Color.red, 10f);

                foreach (RaycastHit2D hit in hits)
                {
                    if(hit.collider != null && hit.collider.CompareTag("Floor"))
                    {
                        if(direction == Vector2.up)
                        {
                            Instantiate(ChangeLevelUp, room.transform.position + new Vector3(0, 3.9f, 0), transform.rotation);
                            wallsTilemap.SetTile(coordsDoorsUp1, doorUp1);
                            wallsTilemap.SetTile(coordsDoorsUp2, doorUp2);
                        }
                        if(direction == Vector2.down)
                        {
                            Instantiate(ChangeLevelDown, room.transform.position - new Vector3(0, 3.9f, 0), transform.rotation);
                            wallsTilemap.SetTile(coordsDoorsDown1, doorDown1);
                            wallsTilemap.SetTile(coordsDoorsDown2, doorDown2);
                        }
                        if (direction == Vector2.left)
                        {
                            Instantiate(ChangeLevelLeft, room.transform.position - new Vector3(8.9f, 0, 0), transform.rotation);
                            wallsTilemap.SetTile(coordsDoorsLeft1, doorLeft1);
                            wallsTilemap.SetTile(coordsDoorsLeft2, doorLeft2);
                        }
                        if(direction == Vector2.right)
                        {
                            Instantiate(ChangeLevelRight, room.transform.position + new Vector3(8.9f, 0, 0), transform.rotation);
                            wallsTilemap.SetTile(coordsDoorsRight1, doorRight1);
                            wallsTilemap.SetTile(coordsDoorsRight2, doorRight2);
                        }
                    }
                }
            }
        }
    }
}
