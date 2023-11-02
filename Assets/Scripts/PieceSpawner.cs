using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{

    public PieceType type;
    private Piece currentPiece;


    public void Spawn()
    {
        int amObj = 0;
        switch(type)
        {
            case PieceType.jump:
                amObj = LevelManager.Instance.jumps.Count; 
                break;

            case PieceType.slide:
                amObj = LevelManager.Instance.slides.Count;
                break;
            case PieceType.longblock:
                amObj = LevelManager.Instance.longblocks.Count;
                break;

            case PieceType.ramp:
                amObj = LevelManager.Instance.ramps.Count;
                break;
        }

        ///Get a new piece from the pool
        currentPiece = LevelManager.Instance.GetPiece(type, Random.Range(0,amObj));
        currentPiece.gameObject.SetActive(true);
        currentPiece.transform.SetParent(transform, false);
    }

    public void Despawn()
    {
        currentPiece.gameObject.SetActive(false);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
