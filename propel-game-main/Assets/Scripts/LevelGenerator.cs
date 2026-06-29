using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerCotroller player;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject[] cloudPrefabs;

    [Header("Generation Settings")]
    [Tooltip("Vertical height of each generated band, in world units.")]
    [SerializeField] private float bandHeight = 30f;
    [Tooltip("How far above the camera's current top edge to keep content generated.")]
    [SerializeField] private float lookAheadDistance = 15f;
    [Tooltip("How far below the camera's current bottom edge before old content is despawned.")]
    [SerializeField] private float despawnBehindDistance = 50f;

    [Header("Enemy Spawn Settings")]
    [SerializeField] private Vector2Int enemiesPerBand = new Vector2Int(1, 3);
    [Tooltip("Extra horizontal margin kept clear past the enemy's patrol range, so it never touches the screen edge.")]
    [SerializeField] private float enemyEdgePadding = 0.5f;
    [SerializeField] private float minEnemySpacing = 20f;

    [Header("Cloud Spawn Settings")]
    [SerializeField] private Vector2Int cloudsPerBand = new Vector2Int(1, 2);
    [Tooltip("How far past the screen edge clouds are allowed to spawn.")]
    [SerializeField] private float cloudOverflow = 2f;
    [SerializeField] private float minCloudSpacing = 4f;

    [Header("Collectible Spawn Settings")]
    [SerializeField] private GameObject[] smallCandyPrefabs;
    [SerializeField] private GameObject[] bigCandyPrefabs;
    [SerializeField] private GameObject[] gunPrefabs;
    [SerializeField] private Vector2Int collectiblesPerBand = new Vector2Int(0, 1);
    [SerializeField] private float collectibleEdgePadding = 0.5f;
    [SerializeField] private float minCollectibleSpacing = 3f;
    [Tooltip("Relative odds of each collectible type being chosen. They don't need to add up to any particular total.")]
    [SerializeField] private float smallCandyWeight = 0.6f;
    [SerializeField] private float bigCandyWeight = 0.2f;
    [SerializeField] private float gunWeight = 0.2f;

    [Header("Player Clear Zone")]
    [Tooltip("Nothing will spawn within this radius of the player's spawn point.")]
    [SerializeField] private float playerSpawnClearRadius = 10f;

    [Header("Difficulty Scaling")]
    [SerializeField] private float enemiesPerHeightUnit = 0.05f;
    [SerializeField] private float maxEnemiesCap = 8f;

    private float highestGeneratedY;
    private readonly List<SpawnedObject> spawnedObjects = new List<SpawnedObject>();

    private struct SpawnedObject
    {
        public GameObject obj;
        public Vector2 position;
        public bool isEnemy;
    }

    private void Awake()
    {
        if (mainCamera == null) mainCamera = Camera.main;
    }

    private bool initialized = false;

    private void Update()
    {
        // Don't generate anything until the game has actually started and
        // told us to. This avoids generating against a menu camera's
        // position/bounds before the player has been placed at their real
        // spawn point (see ResetAndGenerate, called from GameManager).
        if (!initialized) return;
        if (GameManager.Instance != null &&
            GameManager.Instance.CurrentState != GameManager.GameState.Playing)
        {
            return;
        }

        FillAheadOfCamera();
        DespawnBehindCamera();
    }

    /// <summary>
    /// Call this right after the player has been moved to their real spawn
    /// position (e.g. from GameManager.StartGame(), right after
    /// playerController.resetPlayerState()). Clears out anything from a
    /// previous run and regenerates fresh, using the player's actual current
    /// position as the reference point for the spawn clear zone.
    /// </summary>
    public void ResetAndGenerate()
    {
        foreach (var spawned in spawnedObjects)
        {
            if (spawned.obj != null) Destroy(spawned.obj);
        }
        spawnedObjects.Clear();

        float startY = player != null ? player.transform.position.y : transform.position.y;
        highestGeneratedY = startY - bandHeight;
        mainCamera.transform.position = new Vector3(
            mainCamera.transform.position.x,
            startY,
            mainCamera.transform.position.z
        );
        initialized = true;

        FillAheadOfCamera();
    }

    /// <summary>
    /// Generates new bands until content exists far enough above the camera.
    /// </summary>
    private void FillAheadOfCamera()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Playing) {
            float camTop = GetCameraWorldBounds().max.y;

            while (highestGeneratedY < camTop + lookAheadDistance)
            {
                float bandBottom = highestGeneratedY;
                float bandTop = highestGeneratedY + bandHeight;
                GenerateBand(bandBottom, bandTop);
                highestGeneratedY = bandTop;
            }
        }
    }

    /// <summary>
    /// Destroys spawned objects that have fallen far enough below the camera
    /// that the player can no longer reach them, to avoid unbounded growth.
    /// </summary>
    private void DespawnBehindCamera()
    {
        float camBottom = GetCameraWorldBounds().min.y;
        float cutoffY = camBottom - despawnBehindDistance;

        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            if (spawnedObjects[i].position.y < cutoffY)
            {
                if (spawnedObjects[i].obj != null)
                    Destroy(spawnedObjects[i].obj);
                spawnedObjects.RemoveAt(i);
            }
        }
    }

    private void GenerateBand(float bandBottom, float bandTop)
    {
        SpawnEnemiesInBand(bandBottom, bandTop);
        SpawnCloudsInBand(bandBottom, bandTop);
        SpawnCollectiblesInBand(bandBottom, bandTop);
    }

    private void SpawnEnemiesInBand(float bandBottom, float bandTop)
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        Bounds camBounds = GetCameraWorldBounds();
        //int count = Random.Range(enemiesPerBand.x, enemiesPerBand.y + 1);
        float height = player != null ? player.currentMaxHeight : 0f;
        int count = Mathf.RoundToInt(enemiesPerBand.x + (height * enemiesPerHeightUnit));
        count = Mathf.Clamp(count, enemiesPerBand.x, Mathf.RoundToInt(maxEnemiesCap));

        for (int i = 0; i < count; i++)
        {
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            Enemy enemyComp = prefab.GetComponent<Enemy>();
            float patrolRange = enemyComp != null ? enemyComp.Range : 0f;

            float halfWidth = camBounds.extents.x - patrolRange - enemyEdgePadding;
            if (halfWidth < 0f) halfWidth = 0f;

            Vector2 candidate;
            if (TryFindSpawnPosition(camBounds.center.x, halfWidth, bandBottom, bandTop,
                    minEnemySpacing, requireClearOfPlayerSpawn: true, out candidate, playerClearMargin: patrolRange))
            {
                GameObject instance = Instantiate(prefab, candidate, Quaternion.identity, transform);
                spawnedObjects.Add(new SpawnedObject { obj = instance, position = candidate, isEnemy = true });
            }
        }
    }

    private void SpawnCloudsInBand(float bandBottom, float bandTop)
    {
        if (cloudPrefabs == null || cloudPrefabs.Length == 0) return;

        Bounds camBounds = GetCameraWorldBounds();
        int count = Random.Range(cloudsPerBand.x, cloudsPerBand.y + 1);

        // Clouds are allowed to overflow past the screen edges.
        float halfWidth = camBounds.extents.x + cloudOverflow;

        for (int i = 0; i < count; i++)
        {
            GameObject prefab = cloudPrefabs[Random.Range(0, cloudPrefabs.Length)];

            Vector2 candidate;
            if (TryFindSpawnPosition(camBounds.center.x, halfWidth, bandBottom, bandTop,
                    minCloudSpacing, requireClearOfPlayerSpawn: true, out candidate))
            {
                GameObject instance = Instantiate(prefab, candidate, Quaternion.identity, transform);
                spawnedObjects.Add(new SpawnedObject { obj = instance, position = candidate, isEnemy = false });
            }
        }
    }

    private void SpawnCollectiblesInBand(float bandBottom, float bandTop)
    {
        Bounds camBounds = GetCameraWorldBounds();
        float halfWidth = camBounds.extents.x - collectibleEdgePadding;
        if (halfWidth < 0f) halfWidth = 0f;

        int count = Random.Range(collectiblesPerBand.x, collectiblesPerBand.y + 1);

        for (int i = 0; i < count; i++)
        {
            GameObject[] chosenPool = PickCollectiblePool();
            if (chosenPool == null || chosenPool.Length == 0) continue;

            GameObject prefab = chosenPool[Random.Range(0, chosenPool.Length)];

            Vector2 candidate;
            if (TryFindSpawnPosition(camBounds.center.x, halfWidth, bandBottom, bandTop,
                    minCollectibleSpacing, requireClearOfPlayerSpawn: true, out candidate))
            {
                GameObject instance = Instantiate(prefab, candidate, Quaternion.identity, transform);
                spawnedObjects.Add(new SpawnedObject { obj = instance, position = candidate, isEnemy = false });
            }
        }
    }

    /// <summary>
    /// Picks one of the three collectible prefab pools based on their relative
    /// weights (e.g. small candy common, gun rare). Pools that are empty or
    /// unassigned are skipped automatically.
    /// </summary>
    private GameObject[] PickCollectiblePool()
    {
        float wSmall = (smallCandyPrefabs != null && smallCandyPrefabs.Length > 0) ? smallCandyWeight : 0f;
        float wBig = (bigCandyPrefabs != null && bigCandyPrefabs.Length > 0) ? bigCandyWeight : 0f;
        float wGun = (gunPrefabs != null && gunPrefabs.Length > 0) ? gunWeight : 0f;

        float total = wSmall + wBig + wGun;
        if (total <= 0f) return null;

        float roll = Random.Range(0f, total);

        if (roll < wSmall) return smallCandyPrefabs;
        roll -= wSmall;

        if (roll < wBig) return bigCandyPrefabs;

        return gunPrefabs;
    }

    /// <summary>
    /// Attempts to find a random position within the given horizontal/vertical
    /// range that respects minimum spacing from existing objects and the
    /// player's spawn clear zone. Returns false if no valid spot was found
    /// after a reasonable number of attempts (band will just have fewer objects).
    /// </summary>
    private bool TryFindSpawnPosition(float centerX, float halfWidth, float minY, float maxY,
        float minSpacing, bool requireClearOfPlayerSpawn, out Vector2 result, float playerClearMargin = 0f)
    {
        const int maxAttempts = 30;
        Vector3 playerSpawn = player != null ? player.spawnPosition : Vector3.zero;
        float effectiveClearRadius = playerSpawnClearRadius + playerClearMargin;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            float x = centerX + Random.Range(-halfWidth, halfWidth);
            float y = Random.Range(minY, maxY);
            Vector2 candidate = new Vector2(x, y);

            if (requireClearOfPlayerSpawn &&
                Vector2.Distance(candidate, playerSpawn) < effectiveClearRadius)
            {
                continue;
            }

            bool tooClose = false;
            foreach (var spawned in spawnedObjects)
            {
                if (Vector2.Distance(candidate, spawned.position) < minSpacing)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
            {
                result = candidate;
                return true;
            }
        }

        result = Vector2.zero;
        return false;
    }

    private Bounds GetCameraWorldBounds()
    {
        float height = 2f * mainCamera.orthographicSize;
        float width = height * mainCamera.aspect;
        Vector3 center = mainCamera.transform.position;
        return new Bounds(new Vector3(center.x, center.y, 0f), new Vector3(width, height, 0f));
    }
}