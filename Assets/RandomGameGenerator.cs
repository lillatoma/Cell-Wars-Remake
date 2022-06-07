using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGameGenerator : MonoBehaviour
{
    public int currentSeed;
    public GameObject matchInformation;
    public GameObject cellObject;

    public List<GameObject> generatedGameObjects = new List<GameObject>();
    public List<Cell> generatedCells = new List<Cell>();

    public Statistics statistics;
    public GameManager gameManager;

    IEnumerator RerandomizeCall()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        Rerandomize();
    }

    void Rerandomize()
    {
        Random.InitState(System.Environment.TickCount);
    }

    public static void RemakeObject(GameObject go)
    {
        GameObject newGo = Instantiate(go);
        newGo.transform.parent = go.transform.parent;
        newGo.transform.position = go.transform.position;
        newGo.transform.rotation = go.transform.rotation;
        newGo.transform.localScale = go.transform.localScale;
        Destroy(go);
    }

    void DestroyUnneededObjects()
    {
        var cells = FindObjectsOfType<Cell>();
        foreach (Cell cell in cells)
            Destroy(cell.gameObject);
        var controllers = FindObjectsOfType<AIController>();
        foreach (AIController controller in controllers)
            Destroy(controller.gameObject);
        foreach (GameObject go in generatedGameObjects)
            Destroy(go);
        FindObjectOfType<VirusDeployer>().Restart();
        //Destroy(.gameObject);
        //RemakeObject(FindObjectOfType<Statistics>().gameObject);
    }

    IEnumerator LateEnable()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        FindObjectOfType<Statistics>().StartCoroutine(FindObjectOfType<Statistics>().Restart());
        RemakeObject(FindObjectOfType<GameManager>().gameObject);
        RemakeObject(FindObjectOfType<MatchInformation>().gameObject);
    }

    Cell GenerateCell(Cell cell)
    {
        cell.ownerID = 0;
        cell.speedMod = 2f;
        float r = Random.value;
        if(r < 0.05f)
        {
            cell.capacity = 300;
            float r2 = Random.value;
            if(r2 < 0.05f)
            {
                cell.currentVirusCount = 200;
                cell.reproductivityMod = 100;
            }
            else if (r2 < 0.1f)
            {
                cell.currentVirusCount = 300;
                cell.reproductivityMod = 50;
                cell.powerMod = 2.5f;
            }
            else if (r2 < 0.15f)
            {
                cell.currentVirusCount = 300;
                cell.reproductivityMod = 60;
                cell.powerMod = 2;
            }
            else if (r2 < 0.2f)
            {
                cell.currentVirusCount = 200;
                cell.reproductivityMod = 75;
                cell.speedMod = 4;
            }
            else if (r2 < 0.5f)
            {
                cell.currentVirusCount = 100;
                cell.reproductivityMod = 50;
            }
            else if (r2 < 0.975f)
            {
                cell.currentVirusCount = 75;
                cell.reproductivityMod = 40;
            }
            else
            {
                cell.currentVirusCount = 300;
                cell.reproductivityMod = 100;
                cell.powerMod = 2;
                cell.speedMod = 4;
                cell.protectionMod = 2;
            }
        }
        else if (r < 0.1f)
        {
            cell.capacity = 250;
            float r2 = Random.value;
            if (r2 < 0.05f)
            {
                cell.currentVirusCount = 175;
                cell.reproductivityMod = 100;
            }
            else if (r2 < 0.1f)
            {
                cell.currentVirusCount = 250;
                cell.reproductivityMod = 50;
                cell.powerMod = 2.5f;
            }
            else if (r2 < 0.15f)
            {
                cell.currentVirusCount = 250;
                cell.reproductivityMod = 60;
                cell.powerMod = 2;
            }
            else if (r2 < 0.2f)
            {
                cell.currentVirusCount = 175;
                cell.reproductivityMod = 75;
                cell.speedMod = 4;
            }
            else if (r2 < 0.5f)
            {
                cell.currentVirusCount = 80;
                cell.reproductivityMod = 50;
            }
            else if (r2 < 0.975f)
            {
                cell.currentVirusCount = 75;
                cell.reproductivityMod = 40;
            }
            else
            {
                cell.currentVirusCount = 250;
                cell.reproductivityMod = 100;
                cell.powerMod = 2;
                cell.speedMod = 4;
            }
        }
        else if (r < 0.25f)
        {
            cell.capacity = 200;
            float r2 = Random.value;
            if (r2 < 0.05f)
            {
                cell.currentVirusCount = 125;
                cell.reproductivityMod = 75;
            }
            else if (r2 < 0.1f)
            {
                cell.currentVirusCount = 200;
                cell.reproductivityMod = 40;
                cell.powerMod = 2f;
            }
            else if (r2 < 0.15f)
            {
                cell.currentVirusCount = 200;
                cell.reproductivityMod = 60;
                cell.powerMod = 1.5f;
            }
            else if (r2 < 0.2f)
            {
                cell.currentVirusCount = 125;
                cell.reproductivityMod = 60;
                cell.speedMod = 4;
            }
            else if (r2 < 0.5f)
            {
                cell.currentVirusCount = 65;
                cell.reproductivityMod = 40;
            }
            else if (r2 < 0.975f)
            {
                cell.currentVirusCount = 50;
                cell.reproductivityMod = 30;
            }
            else
            {
                cell.currentVirusCount = 200;
                cell.reproductivityMod = 75;
                cell.powerMod = 2;
                cell.speedMod = 4;
            }
        }
        else if (r < 0.5f)
        {
            cell.capacity = 150;
            float r2 = Random.value;
            if (r2 < 0.05f)
            {
                cell.currentVirusCount = 80;
                cell.reproductivityMod = 50;
            }
            else if (r2 < 0.1f)
            {
                cell.currentVirusCount = 150;
                cell.reproductivityMod = 40;
                cell.powerMod = 2f;
            }
            else if (r2 < 0.2f)
            {
                cell.currentVirusCount = 80;
                cell.reproductivityMod = 40;
                cell.speedMod = 4;
            }
            else if (r2 < 0.5f)
            {
                cell.currentVirusCount = 40;
                cell.reproductivityMod = 30;
            }
            else if (r2 < 0.975f)
            {
                cell.currentVirusCount = 30;
                cell.reproductivityMod = 25;
            }
            else
            {
                cell.currentVirusCount = 150;
                cell.reproductivityMod = 50;
                cell.powerMod = 2;
                cell.speedMod = 4;
            }
        }
        else 
        {
            cell.capacity = 100;
            float r2 = Random.value;
            if (r2 < 0.05f)
            {
                cell.currentVirusCount = 50;
                cell.reproductivityMod = 45;
            }
            else if (r2 < 0.1f)
            {
                cell.currentVirusCount = 100;
                cell.reproductivityMod = 30;
                cell.powerMod = 2f;
            }
            else if (r2 < 0.2f)
            {
                cell.currentVirusCount = 50;
                cell.reproductivityMod = 30;
                cell.speedMod = 4;
            }
            else if (r2 < 0.5f)
            {
                cell.currentVirusCount = 30;
                cell.reproductivityMod = 25;
            }
            else 
            {
                cell.currentVirusCount = 20;
                cell.reproductivityMod = 20;
            }
        }
        return cell;
    }

    bool CheckAgainstCurrentCells(Vector3 position, float capacity, int symmetry)
    {
        float scale = 300f / 5.04f;
        foreach (Cell cell in generatedCells)
            if ((position - cell.transform.position).magnitude < ( capacity + cell.capacity) / scale * 0.5f)
                return true;
        if (symmetry == 4 && (position - new Vector3(position.x, 0, -position.z)).magnitude < (capacity * 2f) / scale * 0.5f)
            return true;
        if (symmetry != 0 && (position - new Vector3(-position.x, 0, position.z)).magnitude < (capacity * 2f) / scale * 0.5f)
            return true;
        if (symmetry == 4 && (position - new Vector3(-position.x, 0, -position.z)).magnitude < (capacity * 2f) / scale * 0.5f)
            return true;
        return false;
    }

    int GenerateAssymetricMap()
    {
        generatedCells = new List<Cell>();
        generatedGameObjects = new List<GameObject>();
        int iterationTries = 0;
        float r = Random.value;

        if (r < 0.33)
            iterationTries = 1000;
        else if (r < 0.4)
            iterationTries = 500;
        else if (r < 0.75)
            iterationTries = 75;
        else iterationTries = 25;

        while (iterationTries > 0 || generatedCells.Count < 8)
        {
            iterationTries--;
            if (iterationTries < -500)
                break;



            Cell c = new Cell();
            GenerateCell(c);


            float X = Random.Range(-9f, 9f);
            float Z = Random.Range(-4.5f, 4.5f);

            if (!CheckAgainstCurrentCells(new Vector3(X, 0, Z), c.capacity, 0))
            {
                GameObject go = Instantiate(cellObject, transform);
                go.GetComponent<Cell>().Copy(c);
                go.transform.position = new Vector3(X, 0, Z);
                generatedCells.Add(go.GetComponent<Cell>());
                generatedGameObjects.Add(go);
            }
        }

        int enemyCount = Random.Range(2, 9);
        if (enemyCount > generatedCells.Count / 3) 
            enemyCount = generatedCells.Count / 3;

        int counted = 0;
        while (counted < enemyCount)
        {
            int i = Random.Range(0, generatedCells.Count);
            if (generatedCells[i].ownerID == 0)
            {
                counted++;
                generatedCells[i].ownerID = counted;
            }
        }
        foreach (Cell cell in generatedCells)
            cell.ReassignMaterial();
        return enemyCount;
    }


    int GenerateSymmetricMap()
    {
        generatedCells = new List<Cell>();
        generatedGameObjects = new List<GameObject>();
        int iterationTries = 0;
        float r = Random.value;

        if (r < 0.33)
            iterationTries = 1000;
        else if (r < 0.4)
            iterationTries = 500;
        else if (r < 0.75)
            iterationTries = 12;
        else iterationTries = 7;

        int symmetry = 2;
        if (r < 0.5f)
            symmetry = 4;
        if (symmetry == 2)
        {
            while (iterationTries > 0 || generatedCells.Count < 8)
            {
                iterationTries--;
                if (iterationTries < -500)
                    break;



                Cell c = new Cell();
                GenerateCell(c);


                float X = Random.Range(1f, 9f);
                float Z = Random.Range(-4.5f, 4.5f);

                if (!CheckAgainstCurrentCells(new Vector3(X, 0, Z), c.capacity,symmetry))
                {
                    GameObject go = Instantiate(cellObject,transform);
                    go.GetComponent<Cell>().Copy(c);
                    go.transform.position = new Vector3(X, 0, Z);
                    generatedCells.Add(go.GetComponent<Cell>());
                    generatedGameObjects.Add(go);
                    GameObject go2 = Instantiate(go);
                    go2.transform.position = new Vector3(-X, 0, Z);
                    generatedCells.Add(go2.GetComponent<Cell>());
                    generatedGameObjects.Add(go2);
                    go2.transform.parent = transform;
                }
            }

        }
        else if (symmetry == 4)
        {
            while (iterationTries > 0 || generatedCells.Count < 12)
            {
                iterationTries--;
                if (iterationTries < -500)
                    break;

                Cell c = new Cell();
                GenerateCell(c);

                float X = Random.Range(1f, 9f);
                float Z = Random.Range(1f, 4.5f);

                if (!CheckAgainstCurrentCells(new Vector3(X, 0, Z), c.capacity,symmetry))
                {
                    GameObject go = Instantiate(cellObject, transform);
                    go.GetComponent<Cell>().Copy(c);
                    go.transform.parent = transform;
                    go.transform.position = new Vector3(X, 0, Z);
                    generatedCells.Add(go.GetComponent<Cell>());
                    GameObject go2 = Instantiate(go);
                    go2.transform.position = new Vector3(-X, 0, Z);
                    generatedCells.Add(go2.GetComponent<Cell>());
                    GameObject go3 = Instantiate(go);
                    go3.transform.position = new Vector3(X, 0, -Z);
                    generatedCells.Add(go3.GetComponent<Cell>());
                    GameObject go4 = Instantiate(go);
                    go4.transform.position = new Vector3(-X, 0, -Z);
                    generatedCells.Add(go4.GetComponent<Cell>());
                    go2.transform.parent = transform;
                    go3.transform.parent = transform;
                    go4.transform.parent = transform;
                    generatedGameObjects.Add(go);
                    generatedGameObjects.Add(go2);
                    generatedGameObjects.Add(go3);
                    generatedGameObjects.Add(go4);
                }

            }
        }
        int i = Random.Range(0, generatedCells.Count);
        i /= symmetry;
        i *= symmetry;
        for (int j = 0; j < symmetry; j++)
            generatedCells[i + j].ownerID = 1 + j;
        
        //Mid
        r = Random.value;

        if (r < 0.33)
            iterationTries = 1000;
        else if (r < 0.4)
            iterationTries = 100;
        else if (r < 0.75)
            iterationTries = 20;
        else iterationTries = 10;

        if (symmetry == 2)
        {
            while (iterationTries > 0)
            {
                iterationTries--;
                if (iterationTries < -500)
                    break;
                Cell c = new Cell();
                GenerateCell(c);

                float X = 0f;
                float Z = Random.Range(-4.5f, 4.5f);

                if (!CheckAgainstCurrentCells(new Vector3(X, 0, Z), c.capacity, 0))
                {
                    GameObject go = Instantiate(cellObject, transform);
                    go.GetComponent<Cell>().Copy(c);
                    go.transform.parent = transform;
                    go.transform.position = new Vector3(X, 0, Z);
                    generatedCells.Add(go.GetComponent<Cell>());
                    generatedGameObjects.Add(go);

                }
            }
        }
        else if (symmetry == 4)
        {
            while (iterationTries > 0)
            {
                iterationTries--;
                if (iterationTries < -500)
                    break;
                Cell c = new Cell();
                GenerateCell(c);
               
                float X = 0f;
                float Z = 0f;
                if(Random.value < 0.5f)
                {
                    
                    X = Random.Range(1f, 9f);
                    Z = 0f;
                    if (!CheckAgainstCurrentCells(new Vector3(X, 0, Z), c.capacity, 0))
                    {
                        GameObject go = Instantiate(cellObject, transform);
                        go.GetComponent<Cell>().Copy(c);
                        go.transform.parent = transform;
                        go.transform.position = new Vector3(X, 0, Z);
                        generatedCells.Add(go.GetComponent<Cell>());
                        generatedGameObjects.Add(go);
                        GameObject go2 = Instantiate(go);
                        go2.transform.position = new Vector3(-X, 0, Z);
                        generatedCells.Add(go2.GetComponent<Cell>());
                        generatedGameObjects.Add(go2);
                        go2.transform.parent = transform;
                    }
                }
                else 
                {
                    X = 0f;
                    Z = Random.Range(1f,4.5f);
                    if (!CheckAgainstCurrentCells(new Vector3(X, 0, Z), c.capacity, 0))
                    {
                        GameObject go = Instantiate(cellObject, transform);
                        go.GetComponent<Cell>().Copy(c);
                        go.transform.parent = transform;
                        go.transform.position = new Vector3(X, 0, Z);
                        generatedCells.Add(go.GetComponent<Cell>());
                        generatedGameObjects.Add(go);
                        GameObject go2 = Instantiate(go);
                        go2.transform.position = new Vector3(X, 0, -Z);
                        generatedCells.Add(go2.GetComponent<Cell>());
                        generatedGameObjects.Add(go2);
                        go2.transform.parent = transform;
                    }
                }

                
            }
        }
        foreach (Cell cell in generatedCells)
            cell.ReassignMaterial();
        return symmetry;
    }

    int GenerateSymmetricMapWithRandomSpawns()
    {
        generatedCells = new List<Cell>();
        generatedGameObjects = new List<GameObject>();
        int iterationTries = 0;
        float r = Random.value;

        if (r < 0.33)
            iterationTries = 1000;
        else if (r < 0.4)
            iterationTries = 500;
        else if (r < 0.75)
            iterationTries = 12;
        else iterationTries = 7;

        int symmetry = 2;
        if (r < 0.5f)
            symmetry = 4;
        if (symmetry == 2)
        {
            while (iterationTries > 0 || generatedCells.Count < 8)
            {
                iterationTries--;
                if (iterationTries < -500)
                    break;



                Cell c = new Cell();
                GenerateCell(c);


                float X = Random.Range(1f, 9f);
                float Z = Random.Range(-4.5f, 4.5f);

                if (!CheckAgainstCurrentCells(new Vector3(X, 0, Z), c.capacity, symmetry))
                {
                    GameObject go = Instantiate(cellObject, transform);
                    go.GetComponent<Cell>().Copy(c);
                    go.transform.position = new Vector3(X, 0, Z);
                    generatedCells.Add(go.GetComponent<Cell>());
                    generatedGameObjects.Add(go);
                    GameObject go2 = Instantiate(go);
                    go2.transform.position = new Vector3(-X, 0, Z);
                    generatedCells.Add(go2.GetComponent<Cell>());
                    generatedGameObjects.Add(go2);
                    go2.transform.parent = transform;
                }
            }

        }
        else if (symmetry == 4)
        {
            while (iterationTries > 0 || generatedCells.Count < 12)
            {
                iterationTries--;
                if (iterationTries < -500)
                    break;

                Cell c = new Cell();
                GenerateCell(c);

                float X = Random.Range(1f, 9f);
                float Z = Random.Range(1f, 4.5f);

                if (!CheckAgainstCurrentCells(new Vector3(X, 0, Z), c.capacity, symmetry))
                {
                    GameObject go = Instantiate(cellObject, transform);
                    go.GetComponent<Cell>().Copy(c);
                    go.transform.parent = transform;
                    go.transform.position = new Vector3(X, 0, Z);
                    generatedCells.Add(go.GetComponent<Cell>());
                    GameObject go2 = Instantiate(go);
                    go2.transform.position = new Vector3(-X, 0, Z);
                    generatedCells.Add(go2.GetComponent<Cell>());
                    GameObject go3 = Instantiate(go);
                    go3.transform.position = new Vector3(X, 0, -Z);
                    generatedCells.Add(go3.GetComponent<Cell>());
                    GameObject go4 = Instantiate(go);
                    go4.transform.position = new Vector3(-X, 0, -Z);
                    generatedCells.Add(go4.GetComponent<Cell>());
                    go2.transform.parent = transform;
                    go3.transform.parent = transform;
                    go4.transform.parent = transform;
                    generatedGameObjects.Add(go);
                    generatedGameObjects.Add(go2);
                    generatedGameObjects.Add(go3);
                    generatedGameObjects.Add(go4);
                }

            }
        }


        //Mid
        r = Random.value;

        if (r < 0.33)
            iterationTries = 1000;
        else if (r < 0.4)
            iterationTries = 100;
        else if (r < 0.75)
            iterationTries = 20;
        else iterationTries = 10;

        if (symmetry == 2)
        {
            while (iterationTries > 0)
            {
                iterationTries--;
                if (iterationTries < -500)
                    break;
                Cell c = new Cell();
                GenerateCell(c);

                float X = 0f;
                float Z = Random.Range(-4.5f, 4.5f);

                if (!CheckAgainstCurrentCells(new Vector3(X, 0, Z), c.capacity, 0))
                {
                    GameObject go = Instantiate(cellObject, transform);
                    go.GetComponent<Cell>().Copy(c);
                    go.transform.parent = transform;
                    go.transform.position = new Vector3(X, 0, Z);
                    generatedCells.Add(go.GetComponent<Cell>());
                    generatedGameObjects.Add(go);

                }
            }
        }
        else if (symmetry == 4)
        {
            while (iterationTries > 0)
            {
                iterationTries--;
                if (iterationTries < -500)
                    break;
                Cell c = new Cell();
                GenerateCell(c);

                float X = 0f;
                float Z = 0f;
                if (Random.value < 0.5f)
                {

                    X = Random.Range(1f, 9f);
                    Z = 0f;
                    if (!CheckAgainstCurrentCells(new Vector3(X, 0, Z), c.capacity, 0))
                    {
                        GameObject go = Instantiate(cellObject, transform);
                        go.GetComponent<Cell>().Copy(c);
                        go.transform.parent = transform;
                        go.transform.position = new Vector3(X, 0, Z);
                        generatedCells.Add(go.GetComponent<Cell>());
                        generatedGameObjects.Add(go);
                        GameObject go2 = Instantiate(go);
                        go2.transform.position = new Vector3(-X, 0, Z);
                        generatedCells.Add(go2.GetComponent<Cell>());
                        generatedGameObjects.Add(go2);
                        go2.transform.parent = transform;
                    }
                }
                else
                {
                    X = 0f;
                    Z = Random.Range(1f, 4.5f);
                    if (!CheckAgainstCurrentCells(new Vector3(X, 0, Z), c.capacity, 0))
                    {
                        GameObject go = Instantiate(cellObject, transform);
                        go.GetComponent<Cell>().Copy(c);
                        go.transform.parent = transform;
                        go.transform.position = new Vector3(X, 0, Z);
                        generatedCells.Add(go.GetComponent<Cell>());
                        generatedGameObjects.Add(go);
                        GameObject go2 = Instantiate(go);
                        go2.transform.position = new Vector3(X, 0, -Z);
                        generatedCells.Add(go2.GetComponent<Cell>());
                        generatedGameObjects.Add(go2);
                        go2.transform.parent = transform;
                    }
                }


            }
        }

        int enemyCount = Random.Range(2, 9);
        if (enemyCount > generatedCells.Count / 3)
            enemyCount = generatedCells.Count / 3;

        int counted = 0;
        while (counted < enemyCount)
        {
            int i = Random.Range(0, generatedCells.Count);
            if (generatedCells[i].ownerID == 0)
            {
                counted++;
                generatedCells[i].ownerID = counted;
            }
        }


        foreach (Cell cell in generatedCells)
            cell.ReassignMaterial();
        return enemyCount;
    }

    IEnumerator GenerateAI(int controllerID)
    {
        AIController controller = new AIController();

        if (controller)
            Debug.Log("AI Object Found" + controllerID);
        controller.controlledID = controllerID;
        float r = Random.value;
        if (r < 0.33)
            controller.timeToInstantiate = Random.Range(1f, 2f);
        else if (r < 0.75)
            controller.timeToInstantiate = Random.Range(2f, 4f);
        else
            controller.timeToInstantiate = Random.Range(3f, 7f);
        r = Random.value;
        if (r < 0.33)
        {
            controller.timeBetweenSteps.x = Random.Range(0.5f, 1f);
            controller.timeBetweenSteps.y = Random.Range(controller.timeBetweenSteps.x, controller.timeBetweenSteps.x + 1f);
        }
        else if (r < 0.66)
        {
            controller.timeBetweenSteps.x = Random.Range(1f, 1.5f);
            controller.timeBetweenSteps.y = Random.Range(controller.timeBetweenSteps.x, controller.timeBetweenSteps.x + 1.5f);
        }
        else
        {
            controller.timeBetweenSteps.x = Random.Range(1.5f, 2.5f);
            controller.timeBetweenSteps.y = Random.Range(controller.timeBetweenSteps.x, controller.timeBetweenSteps.x + 2f);
        }
        r = Random.value;
        if (r < 0.33)
            controller.totalCellSelection = Random.Range(6, 9);
        else if (r < 0.6)
            controller.totalCellSelection = Random.Range(4, 6);
        else
            controller.totalCellSelection = Random.Range(2, 4);
        controller.targetingPriority = Random.Range(1, 4);
        controller.selectingPriority = Random.Range(0, 2);
        r = Random.value;
        if (r < 0.5)
            controller.minimumCellsToAggress = Random.Range(1, 6);
        else
            controller.minimumCellsToAggress = Random.Range(5, 11);
        controller.aggressivePercent = Random.Range(0.05f, 0.9f);
        controller.conquerSelectingPriority = Random.Range(0, 2);
        controller.conquerSelectingPriority = Random.Range(0, 2);
        yield return new WaitForEndOfFrame();
        GameObject go = new GameObject("AIController", typeof(AIController));
        Debug.Log("AI Object" + controllerID);

        var aiController = go.GetComponent<AIController>();
        aiController.timeToInstantiate = controller.timeToInstantiate;
        aiController.timeBetweenSteps = controller.timeBetweenSteps;
        aiController.totalCellSelection = controller.totalCellSelection;
        aiController.targetingPriority = controller.targetingPriority;
        aiController.selectingPriority = controller.selectingPriority;
        aiController.minimumCellsToAggress = controller.minimumCellsToAggress;
        aiController.aggressivePercent = controller.aggressivePercent;
        aiController.conquerSelectingPriority = controller.conquerSelectingPriority;
        aiController.conquerTargetingPriority = controller.conquerTargetingPriority;
        aiController.controlledID = controllerID;
        Debug.Log("AI Object Finalized" + controllerID);
    }

    void GenerateAIProfiles(int profilesMax)
    {
        for (int i = 0; i < profilesMax; i++)
        {
            if (i == 0)
                continue;
            StartCoroutine(GenerateAI(i + 1));

        }
    }

    IEnumerator NextFrameGenerate()
    {
        yield return new WaitForEndOfFrame();
        Random.InitState(currentSeed);
        int symmetry = GenerateSymmetricMap();

        StartCoroutine(RerandomizeCall());

    }
    [ContextMenu("Reset Assets")]
    private IEnumerator ResetAssets()
    {
        Time.timeScale = 0f;
        //statistics.StartCoroutine(statistics.Restart());
        yield return new WaitForEndOfFrame();
        gameManager.Reset();
        statistics.RestartNoEnum();
        Time.timeScale = 1f;
    }

    public void Generate()
    {
        gameManager.currentLevel = 1 + PlayerPrefs.GetInt("CompletedLevels");
        generatedCells = new List<Cell>();
        DestroyUnneededObjects();
        Time.timeScale = 1f;
        Random.InitState(currentSeed);
        if (Random.value < 0.333f)
        {
            int symmetry = GenerateSymmetricMap();
            GenerateAIProfiles(symmetry);
        }
        else if (Random.value < 0.5f)
        {
            int symmetry = GenerateSymmetricMapWithRandomSpawns();
            GenerateAIProfiles(symmetry);
        }
        else
        {
            int assymetry = GenerateAssymetricMap();
            GenerateAIProfiles(assymetry);
        }
        StartCoroutine(ResetAssets());
        StartCoroutine(RerandomizeCall());
    }

    // Start is called before the first frame update
    void Start()
    {
        currentSeed = PlayerPrefs.GetInt("RandomSeed", Random.Range(int.MinValue, int.MaxValue));
        SaveSeed();
        Generate();

    }

    public void SaveSeed()
    {
        PlayerPrefs.SetInt("RandomSeed", currentSeed);
        PlayerPrefs.Save();
    }

    public void GenerateNewSeed()
    {
        currentSeed = Random.Range(int.MinValue, int.MaxValue);
        SaveSeed();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
