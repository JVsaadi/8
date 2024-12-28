using UnityEngine;
using System;

public class DeterministicCollider : MonoBehaviour
{
    
    public int width = 2;
    public int height = 7;
    public int mass = 10; // Define the mass variable

    private int[] collisionGrid;
    private int velocityX;
    private int velocityY;
    private int x; // Declare x as a class-level variable
    private int y; // Declare y as a class-level variable
    public DeterministicCollider other; // Reference to the other object

    void Start()
    {
        collisionGrid = new int[width * height];
    }

    void FixedUpdate()
    {
        // Update the collision grid's position based on the GameObject's position
        x = (int)transform.position.x;
        y = (int)transform.position.y;

        // Update the collision grid
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int gridIndex = i + j * width;
                collisionGrid[gridIndex] = 1; // Mark the grid cell as occupied
            }
        }

        // Check for collisions with other objects
        if (IsColliding(other))
        {
            PreventCollision(other);
        }

        // Update the position of the object based on its velocity
        x += velocityX;
        y += velocityY;
        transform.position = new Vector3Int(x, y, 0);
        
    }

    public bool IsColliding(DeterministicCollider other)
    {
        // Check if the two objects are colliding by comparing their collision grids
        int otherX = other.x;
        int otherY = other.y;

        // Check if the objects are overlapping in the x and y directions
        if (x < otherX + other.width && x + width > otherX && y < otherY + other.height && y + height > otherY)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int gridIndex = i + j * width;
                    if (collisionGrid[gridIndex] == 1 && other.collisionGrid[gridIndex] == 1)
                    {   
                        return true;
                    }
                }
            }
        }

        return false;
    }


    private int GetMass() // Add the GetMass method
    {
        return mass;
    }

    private void PreventCollision(DeterministicCollider other)
{
    // Calculate the centers of the objects
    int centerX1 = x + width / 2;
    int centerY1 = y + height / 2;
    int centerX2 = other.x + other.width / 2;
    int centerY2 = other.y + other.height / 2;

    // Calculate the overlap in the x and y directions
    int overlapX = Math.Min(centerX1, centerX2 + other.width / 2) - Math.Max(centerX1 + width / 2, centerX2);
    int overlapY = Math.Min(centerY1, centerY2 + other.height / 2) - Math.Max(centerY1 + height / 2, centerY2);

    // Calculate the direction of the overlap
    int directionX = (centerX1 < centerX2) ? -1 : 1;
    int directionY = (centerY1 < centerY2) ? -1 : 1;

    // Calculate the masses of the objects
    int mass1 = GetMass();
    int mass2 = other.GetMass();

    if (IsColliding(other) && (mass2 == 150))
        {
            GamestateP1.p1grounded = 1;
        }

    // Determine which object to move
    if (mass1 < mass2)
    {
        // Move the "Slime" object
        if (GamestateP1.p1grounded == 0)
        { 
            x += directionX * overlapX;
        }
        y += directionY * overlapY;


        // Add a small offset to the position of the "Slime" object
        if (GamestateP1.p1grounded == 0)
        {
            x += directionX * (width + other.width) / 2;
        }
        y += directionY * (height + other.height) / 2;

        // Calculate the new velocity of the "Slime" object
        int newVelocityX = velocityX - directionX * (mass2 / (mass1 + mass2)) * (overlapX / (width + other.width));
        int newVelocityY = velocityY - directionY * (mass2 / (mass1 + mass2)) * (overlapY / (height + other.height));

        // Update the velocity of the "Slime" object
        velocityX = newVelocityX;
        velocityY = newVelocityY;

        
        
    }
    else
    {
        // Move the "Jahn" object
        other.x -= directionX * overlapX;
        other.y -= directionY * overlapY;

        // Add a small offset to the position of the "Jahn" object
        other.x -= directionX * (width + other.width) / 2;
        other.y -= directionY * (height + other.height) / 2;

        // Calculate the new velocity of the "Jahn" object
        int newVelocityX = other.velocityX + directionX * (mass1 / (mass1 + mass2)) * (overlapX / (width + other.width));
        int newVelocityY = other.velocityY + directionY * (mass1 / (mass1 + mass2)) * (overlapY / (height + other.height));

        // Update the velocity of the "Jahn" object
        other.velocityX = newVelocityX;
        other.velocityY = newVelocityY;
    }



}
}