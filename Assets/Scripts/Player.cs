using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Character[] shapes;
    public float speed = 5f; // Adjust the speed as needed
    Animator myAnim;
    bool startRunning;
    Rigidbody rb;
    int currentShape;
    bool canFly;
    public float maxYPos;
    public enum Terrains
    {
        ground,water,stairs
    }
    public Terrains currentTerrain;

    private void Awake()
    {
        SwitchShape(0);
        rb = GetComponent<Rigidbody>();
    }
    public void SwitchShape(int index)
    {
        foreach(Character item in shapes)
        {
            item.gameObject.SetActive(false);
        }
        shapes[index].gameObject.SetActive(true);
        currentShape = index;
        if(shapes[index].GetComponent<Character>().myAnim)
        myAnim = shapes[index].GetComponent<Character>().myAnim;
        canFly = shapes[index].GetComponent<Character>().canFly;
        UpdateSpeed(index);
    }

    void UpdateSpeed(int index)
    {
        switch (currentTerrain)
        {
            case Terrains.ground:
                speed = shapes[index].groundSpeed;
                UpdateAnimState(1);
                break;
            case Terrains.water:
                speed = shapes[index].waterSpeed;
                UpdateAnimState(2);
                break;
            case Terrains.stairs:
                speed = shapes[index].stairsSpeed;
                UpdateAnimState(3);
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        PlayerMovement();
    }


    public void Begin()
    {
        startRunning = true;
        UpdateAnimState(1);
    }
    void PlayerMovement()
    {
        // Move the character forward based on its current rotation
        if (startRunning)
        {
            Vector3 direction = Vector3.forward;
            if (canFly)
                direction = new Vector3(0, 1, 1);
            else
                direction = new Vector3(0, -1, 1);
            rb.velocity = (direction * speed * Time.deltaTime);
            //transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, maxYPos, 0.23f), transform.position.z);
        }
    }

    void UpdateAnimState(int state)
    {
        myAnim.SetInteger("state", state);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("terrain"))
        {
            currentTerrain = other.GetComponent<Terrain>().terrain;
            UpdateSpeed(currentShape);
        }
    }
}
