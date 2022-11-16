using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class SpawnGenerator : MonoBehaviour
{
    public GameObject doodlePrefab;

    public int numberOfDoodles = 100;

    public int maxGeneration = 10;
    int generation = 1;
    public int nbKept = 10;

    List<GameObject> doodles = new List<GameObject>();
    List<GameObject> best = new List<GameObject>();

    Vector3 defaultSpawnPosition;
    float defaultMinHeight;
    float defaultLevelWidth = 0;

    CameraFollow cameraFollow;
    void Start()
    {
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
    }

    
    void Update()
    {
        if (generation < maxGeneration && doodles.Count > 0)
        {
            checkBestDoodle();
            if(best.Count == doodles.Count)
            {
                int[] gosKept = selection(nbKept);
                croisement(gosKept);
                //mutation();
                cameraFollow.resetCamera(defaultSpawnPosition);
                generation++;
                Debug.Log("Genération n°" + generation);
            }
        }
        else
        {
            Debug.Log("Fin des génération");
        }
    }

    void croisement(int[] gosKept)
    {
        for(int i = 0; i < numberOfDoodles; i++)
        {
            if (!gosKept.Contains(i))
            {
                int random1 = Random.Range(0, gosKept.Length);
                Player player1 = doodles[gosKept[random1]].GetComponent<Player>();
                Player player = doodles[i].GetComponent<Player>();
                player.initialisation(defaultSpawnPosition, defaultMinHeight, defaultLevelWidth, player1.getDirectionsToFollow().GetRange(0, player1.getNbDirectionsFollowed() - 1), player1.getJumpForcesToFollow().GetRange(0, player1.getNbJumpForcesFollowed() - 1));
            }
            else
            {
                Player player = doodles[i].GetComponent<Player>();
                player.initialisation(defaultSpawnPosition, defaultMinHeight, defaultLevelWidth, player.getDirectionsToFollow(), player.getJumpForcesToFollow());
            }
        }
    }

    int[] selection(int nbKept)
    {
        best.Sort((u1, u2) =>
        {
            Player p1 = u1.GetComponent<Player>();
            Player p2 = u2.GetComponent<Player>();
            int result = p1.getMaxHeightAchieved().CompareTo(p2.getMaxHeightAchieved());
            //return result == 0 ? p1.getNbDirectionsFollowed().CompareTo(p2.getNbDirectionsFollowed()) : result;
            return result;
        });
        best.Reverse();
        int[] gosKept = new int[nbKept];
        for(int i = 0; i < nbKept; i++)
        {
            gosKept[i] = doodles.IndexOf(best[i]);
        }
        best.Clear();
        return gosKept;
    }

    void checkBestDoodle()
    {
        for(int i = 0; i < doodles.Count; i++)
        {
            GameObject go = doodles[i];
            Player player = go.GetComponent<Player>();
            if (!player.isAlive() && !best.Contains(go))
            {
                best.Add(go);
                //Debug.Log(player);
            }
        }
    }

    public void genese(Vector3 firstPlateform, float levelWidth)
    {
        defaultMinHeight = firstPlateform.y;
        defaultSpawnPosition = firstPlateform + new Vector3(0, 2, 0);
        defaultLevelWidth = levelWidth;
        genese();
    }

    void genese()
    {
        for (int i = 0; i < numberOfDoodles; i++)
        {
            GameObject go = Instantiate(doodlePrefab, defaultSpawnPosition, Quaternion.identity);
            Player player = go.GetComponent<Player>();
            player.initialisation(defaultSpawnPosition, defaultMinHeight, defaultLevelWidth, new List<float>(), new List<float>());
            doodles.Add(go);
        }
    }
}
