using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    public HexCoordinates hexCoordinates;

    bool isEnable;

    public ParticleSystem fogOfWarParticlePrefab;

    public void InstatiateFog(Vector3 position, HexCoordinates coordinates)
    {
        var fogOfWar = Instantiate<ParticleSystem>(fogOfWarParticlePrefab);
        fogOfWar.gameObject.transform.SetParent(transform);
        fogOfWar.gameObject.transform.position = new Vector3(position.x, 20, position.z);

        hexCoordinates = coordinates;
    }
}
