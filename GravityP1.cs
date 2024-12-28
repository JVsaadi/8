using UnityEngine;
using System.Collections.Generic;

public class GravityP1 : MonoBehaviour
{
    // Tidsskrittet for fysikken (16 = 1000/60)
    private int tidsskritt = 16;

    // Den nåværende tiden
    private int tid = 0;

    // Liste over objekter som skal kjøres fysikken på
    private List<GameObject> listep1 = new List<GameObject>();
    
    // Tidsskritt-teller
    private int tidsskrittTeller = 0;
    
    // Struktur for å representere posisjon og hastighet med integer-verdier
    private struct Position
    {
        public int x;
        public int y;
    }

    void Start()
    {
        GameObject placeholderp1 = GameObject.Find("p1");
        LeggTilp1(placeholderp1);
        
    }

    // Legg til et objekt til fysikken
    public void LeggTilp1(GameObject p1)
    {
        listep1.Add(p1);
    }

    void Update()
    {
        // Kjør fysikken med fast tidsskritt
        tidsskrittTeller++;
        if (tidsskrittTeller >= tidsskritt)
        { 
            // Kjør fysikken på alle objekter
            foreach (GameObject p1 in listep1)
            {
                KjørFysikk(p1);
            }
            tidsskrittTeller = 0;
        }
    }

    // Kjør fysikken på et objekt
    private void KjørFysikk(GameObject p1)
    {
        
        // Hent objektets posisjon og hastighet
        Position posisjon = new Position();
        posisjon.x = (int)p1.transform.position.x;
        posisjon.y = (int)p1.transform.position.y;

        // Hent objektets akselerasjon
        int akselerasjonX = 0;
        int akselerasjonY = -1; // Tyngdekraft

        if (GamestateP1.p1grounded != 0)
        {
            // Legg til gravitasjon i y-aksen
            akselerasjonY = 0;
        }



        // Check if the object is colliding with another object
        DeterministicCollider collider = p1.GetComponent<DeterministicCollider>();
        if (collider != null && collider.IsColliding(collider.other))
        {
            // Set the vertical speed to 0
            akselerasjonY = 0;
        }

    

        // Oppdater hastighet og posisjon
        posisjon.x += akselerasjonX;
        posisjon.y += akselerasjonY;

        // Oppdater objektets posisjon
        p1.transform.position = new Vector3(posisjon.x, posisjon.y, 0);
    }

    
}