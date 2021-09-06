using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtoPlatform : MonoBehaviour
{
    
    public GameObject cube;
    public GameObject cylinder;
    public Transform generationPosition;


    private const int CreateChancePercent = 10;
    private const int MaxCreateChancePercent = 200;
    private const float PlatformDistance = 10;
    private readonly int GridLength = 100;
    private const float MinScaleRate = 1f;
    private const float MaxScaleRate = 3f;
    private const float MaxDeviation = 0.2f;
    private const float minYdevilation = 5f;





    
  
   

    void Awake()
    {
        Generator();
    }

   
    void Generator() 
    {
        float leftBottomX;
        float leftBottomZ;

        if (GridLength % 2 == 0)
        {
            leftBottomX = generationPosition.position.x - (GridLength / 2 - 0.5f) * PlatformDistance;
            leftBottomZ = generationPosition.position.z - (GridLength / 2 - 0.5f) * PlatformDistance;
        }
        else
        {
            leftBottomX = generationPosition.position.x - ((GridLength - 1) / 2) * PlatformDistance;
            leftBottomZ = generationPosition.position.z - ((GridLength - 1) / 2) * PlatformDistance;
        }
        FillGrid(leftBottomX, leftBottomZ);
        Disable();
    }
    void FillGrid(float leftBottomPositionX, float leftBottomPositionZ)
    {
        for (int z = 0; z < GridLength; z++)
        {
            for (int x = 0; x < GridLength; x++)
            {
                Vector3 position = new Vector3(leftBottomPositionX + x * PlatformDistance, 0, leftBottomPositionZ + z * PlatformDistance);
                SpawnItem(position);
            }
        }
    }
    void SpawnItem(Vector3 position)
    {
        if (RandomChance())
        {
            Vector3 scaleFactor;
            float positionFactorY;

            GameObject prefabType;

            int itemType = Random.Range(1,3);

            if (itemType == 1) 
            {
                prefabType = cube;
                scaleFactor = RandomCube();
                positionFactorY = 0.5f;
            }


            else 
            {
                prefabType = cylinder;
                scaleFactor = RandomCylinder();
                positionFactorY = 1f;
            }
            GameObject item = Instantiate(prefabType, position + RandomPosition(), Quaternion.identity);

            float localScaleX = item.transform.localScale.x * scaleFactor.x;
            float localScaleY = item.transform.localScale.y * scaleFactor.y;
            float localScaleZ = item.transform.localScale.z * scaleFactor.z;
            item.transform.localScale = new Vector3(localScaleX, localScaleY, localScaleZ);

            float positionY = item.transform.localScale.y * positionFactorY;
            item.transform.position = new Vector3(item.transform.position.x, positionY, item.transform.position.z);
        }

    }
   
    bool RandomChance()
    {
        int createChance = Random.Range(0, MaxCreateChancePercent);
        if (createChance < CreateChancePercent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    Vector3 RandomPosition()
    {
        float deviationRateX = Random.Range(0, MaxDeviation);
        float x = PlatformDistance * deviationRateX * RandomSign();

        float deviationRateZ = Random.Range(0, MaxDeviation);
        float z = PlatformDistance * deviationRateZ * RandomSign();
       

        return new Vector3(x, 0, z);
    }
    Vector3 RandomCylinder()
    {
        float xz = Random.Range(MinScaleRate, MaxScaleRate);
        float y = Random.Range(MinScaleRate, MaxScaleRate);

        return new Vector3(xz, y, xz);
    }
    Vector3 RandomCube()
    {
        float x = Random.Range(MinScaleRate, MaxScaleRate);
        float y = Random.Range(MinScaleRate, MaxScaleRate);
        float z = Random.Range(MinScaleRate, MaxScaleRate);

        return new Vector3(x, y, z);
    }
  

    int RandomSign()
    {
        int chance = Random.Range(0, 2);
        if (chance == 1)
        {
            return 1;
        }
        else return -1;
    }
   
    void Disable()
    {
        this.enabled = false;
    }
}
