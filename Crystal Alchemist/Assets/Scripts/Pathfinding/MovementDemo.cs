using CrystalAlchemist;
using System.Collections.Generic;
using UnityEngine;

public class MovementDemo : MonoBehaviour
{
    [SerializeField]
    private float speed = 4;

    [SerializeField]
    private float accuracy = 0.25f;

    [SerializeField]
    private float updateDelay = 0.5f;

    [SerializeField]
    private Vector2 targetPosition;

    private PathSeeker seeker = null;
    private List<Vector2> path;
    private int index;
    private Rigidbody2D myRigidbody;

    private void Awake()
    {
        this.seeker = this.GetComponent<PathSeeker>();
        this.myRigidbody = this.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        InvokeRepeating("UpdatePath", 0, this.updateDelay);
    }

    private void UpdatePath()
    {
        this.index = 0;
        this.path = this.seeker.FindPath(this.transform.position, this.targetPosition);
    }

    private void FixedUpdate()
    {
        MoveTroughPaths();         
    }

    private void MoveTroughPaths()
    {
        if (this.path != null && this.index >= this.path.Count)
        {
            this.index = 0;
            this.path = null;
        }

        if (path != null)
        {
            Vector2 pathPosition = this.path[this.index];

            if (Vector2.Distance(this.transform.position, pathPosition) > this.accuracy) MoveToPosition(pathPosition);
            else this.index++;
        }
    }

    private void MoveToPosition(Vector2 position)
    {
        Vector2 direction = (position - (Vector2)this.transform.position).normalized;
        if (direction != Vector2.zero) this.myRigidbody.velocity = direction * this.speed;
    }
}
