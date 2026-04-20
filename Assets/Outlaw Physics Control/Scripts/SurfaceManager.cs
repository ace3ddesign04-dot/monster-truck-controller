using CustomVP;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class SurfaceManager : MonoBehaviour
{
    public static SurfaceManager Instance;

    public Terrain BaseTerrain;

    private int hRes;

    private int aRes;

    [Header("Materials")] public SurfaceMaterial[] surfaceMaterials;

    [HideInInspector] public SurfaceMaterial SurfaceMaterialUnderCar;

    public Material MudWaterMaterial;

    public Material WaterMaterial;

    public float DrynessSpeed = 1f;

    public float WetnessSpeed = 1f;

    [Header("Deformation")] public int PushDiameter = 4;

    public float PushStrength = 0.1f;

    public float DeformationPointOffset = 0.1f;

    public Texture2D PushStampTexture;

    public int MudWaterResoultion = 32;

    [Header("Particles")] [Range(0f, 1f)] public float SkidSmokeSlipThreshold = 0.4f;

    [Range(0f, 1f)] public float MudSlipThreshold = 0.3f;

    public float SandDustMinRpm = 10f;

    public float WaterSplashMinRpm = 10f;

    public int SkidSmokeEmitStrength = 2;

    public int SandDustEmitStrength = 1;

    public int MudEmitStrength = 3;

    public int WaterSplashEmitStrength = 3;

    public int RipplesSpeedThreshold = 5;

    public float SnowRPMThreshold = 30f;

    public int SnowEmitStrength = 20;

    private float CarWetnessTarget;

    [Header("System")] public MaxSmallTerrainResolution maxSmallTerrainResolution;

    public int TargetVertexPerMeterNumber = 3;

    public int ChunkSize = 8;

    public int UpdateEveryNFrame = 2;

    public int SmallTerrainBasemapDistance = 10;

    public bool ShowChunksGizmos;

    public Color GizmosColor = Color.grey;

    private float tireMarksUpdateInterval = 0.02f;

    private TireFxData[] m_fxData;

    private TireMarksRenderer marksRenderer;

    private MudTerrain[] MudTerrains;

    private MudTerrain CurrentMudTerrain;

    private TireMarksRenderer SnowMarksRenderer;

    private TireMarksRenderer SandMarksRenderer;

    private TireMarksRenderer MudMarksRenderer;

    private TireMarksRenderer SkidMarksRenderer;

    private VolumetricTireTrackRenderer[] SnowTracksRenderers;

    public List<GameObject> WaterMeshes;

    public List<Rect> WaterMeshesRects;

    private ParticleSystem MudParticles;

    private ParticleSystem SandDustParticles;

    private ParticleSystem SkidSmokeParticles;

    private ParticleSystem WaterSplashParticles;

    private ParticleSystem RippleParticles;

    private ParticleSystem SnowParticles;

    private CarController lastCarController;

    private Texture2D[] terrainTextures;

    private ParticleSystem CurrentParticles;

    private ParticleSystem SecondaryParticles;

    private QualityLevel CurrentQuality;

    private Color SkyTintColor = Color.white;

    private Color WaterTintColor = new Color(0f, 0f, 1f);

    private Color TargetTintColor = Color.white;

    private BlurOptimized BlurScript;

    private float BlurAmountTarget;

    private CarController carController
    {
        get
        {
            if (VehicleLoader.Instance != null)
            {
                return VehicleLoader.Instance.playerCarController;
            }

            return null;
        }
    }

    private SuspensionController suspensionController
    {
        get
        {
            if (VehicleLoader.Instance != null)
            {
                return VehicleLoader.Instance.playerSuspensionController;
            }

            return null;
        }
    }

    private BodyPartsSwitcher partsSwitcher
    {
        get
        {
            if (VehicleLoader.Instance != null)
            {
                return VehicleLoader.Instance.playerPartsSwitcher;
            }

            return null;
        }
    }

    private Rigidbody playerRigidbody
    {
        get
        {
            if (VehicleLoader.Instance != null)
            {
                return VehicleLoader.Instance.playerRigidbody;
            }

            return null;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (BaseTerrain == null)
        {
            UnityEngine.Debug.LogError("Base terrain is not assigned. Surface manager is off.");
            base.enabled = false;
            return;
        }

        InitializeBaseTerrain();
        LoadParticles();
        MudTerrains = BaseTerrain.GetComponentsInChildren<MudTerrain>();
        CurrentQuality = (QualityLevel)QualitySettings.GetQualityLevel();
        BlurScript = Camera.main.GetComponent<BlurOptimized>();
        if (BlurScript == null)
        {
            BlurScript = Camera.main.gameObject.AddComponent<BlurOptimized>();
            BlurScript.downsample = 2;
            BlurScript.blurIterations = 1;
            BlurScript.blurShader = Shader.Find("Hidden/FastBlur");
        }

        if (BlurScript != null)
        {
            BlurScript.enabled = false;
        }
    }

    private void OnEnable()
    {
        FindWater();
    }

    private void UpdateTireMarks(int WheelID)
    {
        if (carController == null)
        {
            return;
        }

        marksRenderer = m_fxData[WheelID].marksRenderer;
        if (SnowTracksRenderers != null)
        {
            for (int i = 0; i < m_fxData.Length; i++)
            {
                bool renderTrack = false;
                if (m_fxData[i].surfaceMaterial != null &&
                    m_fxData[i].surfaceMaterial.tireTracks == SurfaceMaterial.TireTracks.SnowTrack)
                {
                    renderTrack = true;
                }

                SnowTracksRenderers[i].RenderTrack = renderTrack;
                SnowTracksRenderers[i].TireWidth = 0.4f * ((i <= carController.wheels.Count / 2 - 1)
                    ? suspensionController.FrontWheelsControls.WheelsWidth.FloatValue
                    : suspensionController.RearWheelsControls.WheelsWidth.FloatValue);
            }
        }

        TireFxData tireFxData = m_fxData[WheelID];
        _Wheel wheel = carController.wheels[WheelID];
        float width = suspensionController.FrontWheelsControls.WheelsWidth.FloatValue * 0.3f;
        if (WheelID > 1 && carController.wheels.Count >= 4)
        {
            width = suspensionController.RearWheelsControls.WheelsWidth.FloatValue * 0.3f;
        }

        if (tireFxData.lastMarksIndex != -1 && wheel.wc.IsGrounded && tireFxData.marksDelta < tireMarksUpdateInterval)
        {
            tireFxData.marksDelta += Time.deltaTime;
            return;
        }

        float num = tireFxData.marksDelta;
        if (num == 0f)
        {
            num = Time.deltaTime;
        }

        tireFxData.marksDelta = 0f;

        //naveed
        // if (!wheel.wc.IsGrounded ||
        //     wheel.wc.wheelCollider.contactColliderHit.attachedRigidbody != null)
        if (!wheel.wc.IsGrounded)
        {
            tireFxData.lastMarksIndex = -1;
            return;
        }

        if (marksRenderer != tireFxData.lastRenderer)
        {
            tireFxData.lastRenderer = marksRenderer;
            tireFxData.lastMarksIndex = -1;
        }

        if (marksRenderer != null)
        {
            Vector3 b = carController.Speed * wheel.wc.transform.forward * 0.01f + wheel.wc.transform.forward * num;
            tireFxData.lastMarksIndex = marksRenderer.AddMark(wheel.wc.wheelCollider.realHitPoint + b,
                wheel.wc.wheelCollider.contactNormal, width, tireFxData.lastMarksIndex);
        }
    }

    public void FindWater()
    {
        WaterMeshes = new List<GameObject>();
        WaterMeshesRects = new List<Rect>();
        Renderer[] array = UnityEngine.Object.FindObjectsOfType<Renderer>();
        foreach (Renderer renderer in array)
        {
            if (renderer.sharedMaterial != null && renderer.sharedMaterial.Equals(WaterMaterial))
            {
                WaterMeshes.Add(renderer.gameObject);
                List<Rect> waterMeshesRects = WaterMeshesRects;
                Vector3 min = renderer.bounds.min;
                float x = min.x;
                Vector3 min2 = renderer.bounds.min;
                float z = min2.z;
                Vector3 size = renderer.bounds.size;
                float x2 = size.x;
                Vector3 size2 = renderer.bounds.size;
                waterMeshesRects.Add(new Rect(x, z, x2, size2.z));
            }
        }
    }

    public void RemoveMudTerrains(List<MudStamp> mudStamps = null)
    {
        InitializeBaseTerrain();
        MudTerrain[] array = UnityEngine.Object.FindObjectsOfType<MudTerrain>();
        if (array.Length == 0)
        {
            return;
        }

        float[,] heights = BaseTerrain.terrainData.GetHeights(0, 0, hRes, hRes);
        Random.InitState(0);
        for (int i = 0; i < aRes; i++)
        {
            for (int j = 0; j < aRes; j++)
            {
                Vector3 position = BaseTerrain.transform.position;
                float x = position.x;
                float num = (float)i / (float)hRes;
                Vector3 size = BaseTerrain.terrainData.size;
                position.x = x + num * size.x;
                float z = position.z;
                float num2 = (float)j / (float)hRes;
                Vector3 size2 = BaseTerrain.terrainData.size;
                position.z = z + num2 * size2.z;
                MudTerrain[] array2 = array;
                foreach (MudTerrain mudTerrain in array2)
                {
                    if (mudTerrain.terRect.Contains(new Vector2(position.x, position.z)))
                    {
                        SurfaceMaterialUnderCar = GetSurfaceMaterialByTerrainCoords(new Vector2(i, j));
                        Vector3 position2 = BaseTerrain.GetPosition();
                        float num3 = (float)i / (float)hRes;
                        Vector3 size3 = BaseTerrain.terrainData.size;
                        float x2 = num3 * size3.x;
                        float num4 = (float)j / (float)hRes;
                        Vector3 size4 = BaseTerrain.terrainData.size;
                        Vector3 point = position2 + new Vector3(x2, 0f, num4 * size4.z);
                        float deformableMaterialMaxDepth = mudTerrain.DeformableMaterialMaxDepth;
                        Vector3 size5 = BaseTerrain.terrainData.size;
                        float num5 = deformableMaterialMaxDepth / size5.y;
                        foreach (MudStamp mudStamp in mudStamps)
                        {
                            if (DoesPointBelongToMudStamp(mudStamp, point))
                            {
                                float mudDepth = mudStamp.mudDepth;
                                Vector3 size6 = BaseTerrain.terrainData.size;
                                num5 = mudDepth / size6.y;
                            }
                        }

                        float num6 = 0f;
                        if (SurfaceMaterialUnderCar != null &&
                            SurfaceMaterialUnderCar.surfaceType == SurfaceMaterial.SurfaceType.Mud)
                        {
                            num6 = num5;
                        }

                        if (num6 != 0f)
                        {
                            float num7 = num6;
                            Vector3 size7 = BaseTerrain.terrainData.size;
                            float min = 0.1f / size7.y;
                            Vector3 size8 = BaseTerrain.terrainData.size;
                            num6 = num7 + UnityEngine.Random.Range(min, 0.7f / size8.y);
                        }

                        heights[j, i] += num6;
                    }
                }
            }
        }

        BaseTerrain.terrainData.SetHeights(0, 0, heights);
        int num8 = array.Length;
        for (int l = 0; l < num8; l++)
        {
            UnityEngine.Object.DestroyImmediate(array[l].gameObject);
        }

        UnityEngine.Debug.Log(num8 + " mud terrains were successfully removed");
    }

    private void InitializeTireFxData()
    {
        m_fxData = new TireFxData[carController.wheels.Count];
        for (int i = 0; i < m_fxData.Length; i++)
        {
            m_fxData[i] = new TireFxData();
        }

        bool flag = false;
        bool flag2 = false;
        bool flag3 = false;
        bool flag4 = false;
        bool flag5 = false;
        SurfaceMaterial[] array = surfaceMaterials;
        foreach (SurfaceMaterial surfaceMaterial in array)
        {
            if (surfaceMaterial.tireTracks == SurfaceMaterial.TireTracks.MudMark)
            {
                flag3 = true;
            }

            if (surfaceMaterial.tireTracks == SurfaceMaterial.TireTracks.SandMark)
            {
                flag2 = true;
            }

            if (surfaceMaterial.tireTracks == SurfaceMaterial.TireTracks.SnowMark)
            {
                flag = true;
            }

            if (surfaceMaterial.tireTracks == SurfaceMaterial.TireTracks.SnowTrack)
            {
                flag4 = true;
            }

            if (surfaceMaterial.tireTracks == SurfaceMaterial.TireTracks.SkidMark)
            {
                flag5 = true;
            }
        }

        if (SnowMarksRenderer != null)
        {
            UnityEngine.Object.Destroy(SnowMarksRenderer.gameObject);
        }

        if (SandMarksRenderer != null)
        {
            UnityEngine.Object.Destroy(SandMarksRenderer.gameObject);
        }

        if (MudMarksRenderer != null)
        {
            UnityEngine.Object.Destroy(MudMarksRenderer.gameObject);
        }

        if (SkidMarksRenderer != null)
        {
            UnityEngine.Object.Destroy(SkidMarksRenderer.gameObject);
        }

        if (SnowTracksRenderers != null && SnowTracksRenderers.Length > 0)
        {
            VolumetricTireTrackRenderer[] snowTracksRenderers = SnowTracksRenderers;
            foreach (VolumetricTireTrackRenderer volumetricTireTrackRenderer in snowTracksRenderers)
            {
                UnityEngine.Object.Destroy(volumetricTireTrackRenderer.gameObject);
            }
        }

        if (flag)
        {
            SnowMarksRenderer =
                (UnityEngine.Object.Instantiate(Resources.Load("TireTracks/SnowMark",
                    typeof(GameObject))) as GameObject).GetComponent<TireMarksRenderer>();
        }

        if (flag2)
        {
            SandMarksRenderer =
                (UnityEngine.Object.Instantiate(Resources.Load("TireTracks/SandMark",
                    typeof(GameObject))) as GameObject).GetComponent<TireMarksRenderer>();
        }

        if (flag3)
        {
            MudMarksRenderer =
                (UnityEngine.Object.Instantiate(Resources.Load("TireTracks/MudMark", typeof(GameObject))) as GameObject)
                .GetComponent<TireMarksRenderer>();
        }

        if (flag5)
        {
            SkidMarksRenderer =
                (UnityEngine.Object.Instantiate(Resources.Load("TireTracks/SkidMark",
                    typeof(GameObject))) as GameObject).GetComponent<TireMarksRenderer>();
        }

        if (flag4)
        {
            SnowTracksRenderers = new VolumetricTireTrackRenderer[m_fxData.Length];
            for (int l = 0; l < m_fxData.Length; l++)
            {
                SnowTracksRenderers[l] =
                    (UnityEngine.Object.Instantiate(Resources.Load("TireTracks/SnowTrack", typeof(GameObject))) as
                        GameObject).GetComponent<VolumetricTireTrackRenderer>();
                SnowTracksRenderers[l].wheelCollider = carController.wheels[l].wc;
            }
        }
    }

    private void InitializeBaseTerrain()
    {
        if (!BaseTerrain || !BaseTerrain.terrainData || BaseTerrain.terrainData.splatPrototypes == null || BaseTerrain.terrainData.splatPrototypes.Length < 1) {
            return;
        }
        SplatPrototype[] splatPrototypes = BaseTerrain.terrainData.splatPrototypes;
        terrainTextures = new Texture2D[splatPrototypes.Length];
        for (int i = 0; i < terrainTextures.Length; i++)
        {
            terrainTextures[i] = splatPrototypes[i].texture;
        }

        hRes = BaseTerrain.terrainData.heightmapResolution;
        aRes = BaseTerrain.terrainData.alphamapResolution;
    }

    private void LoadParticles()
    {
        MudParticles =
            (UnityEngine.Object.Instantiate(Resources.Load("ParticleEffects/Mud", typeof(GameObject))) as GameObject)
            .GetComponent<ParticleSystem>();
        SandDustParticles =
            (UnityEngine.Object.Instantiate(Resources.Load("ParticleEffects/SandDust", typeof(GameObject))) as
                GameObject).GetComponent<ParticleSystem>();
        SkidSmokeParticles =
            (UnityEngine.Object.Instantiate(Resources.Load("ParticleEffects/SkidSmoke", typeof(GameObject))) as
                GameObject).GetComponent<ParticleSystem>();
        WaterSplashParticles =
            (UnityEngine.Object.Instantiate(Resources.Load("ParticleEffects/WaterSplash", typeof(GameObject))) as
                GameObject).GetComponent<ParticleSystem>();
        RippleParticles =
            (UnityEngine.Object.Instantiate(Resources.Load("ParticleEffects/RippleParticles", typeof(GameObject))) as
                GameObject).GetComponent<ParticleSystem>();
        SnowParticles =
            (UnityEngine.Object.Instantiate(Resources.Load("ParticleEffects/Snow", typeof(GameObject))) as GameObject)
            .GetComponent<ParticleSystem>();
        MudParticles.transform.parent = base.transform;
        SandDustParticles.transform.parent = base.transform;
        SkidSmokeParticles.transform.parent = base.transform;
        WaterSplashParticles.transform.parent = base.transform;
        RippleParticles.transform.parent = base.transform;
        SnowParticles.transform.parent = base.transform;
    }

    private void OnDrawGizmos()
    {
        if (!ShowChunksGizmos)
        {
            return;
        }

        Gizmos.color = GizmosColor;
        Terrain baseTerrain = BaseTerrain;
        if (!(baseTerrain == null))
        {
            for (int i = 0; i < baseTerrain.terrainData.alphamapWidth; i += ChunkSize)
            {
                Vector3 position = baseTerrain.GetPosition();
                float num = (float)i / (float)baseTerrain.terrainData.alphamapWidth;
                Vector3 size = baseTerrain.terrainData.size;
                Vector3 from = position + new Vector3(num * size.x, 0f, 0f);
                Vector3 position2 = baseTerrain.GetPosition();
                float num2 = (float)i / (float)baseTerrain.terrainData.alphamapWidth;
                Vector3 size2 = baseTerrain.terrainData.size;
                float x = num2 * size2.x;
                Vector3 size3 = baseTerrain.terrainData.size;
                Gizmos.DrawLine(from, position2 + new Vector3(x, 0f, size3.z));
            }

            for (int j = 0; j < baseTerrain.terrainData.alphamapHeight; j += ChunkSize)
            {
                Vector3 position3 = baseTerrain.GetPosition();
                float num3 = (float)j / (float)baseTerrain.terrainData.alphamapHeight;
                Vector3 size4 = baseTerrain.terrainData.size;
                Vector3 from2 = position3 + new Vector3(0f, 0f, num3 * size4.z);
                Vector3 position4 = baseTerrain.GetPosition();
                Vector3 size5 = baseTerrain.terrainData.size;
                float x2 = size5.x;
                float num4 = (float)j / (float)baseTerrain.terrainData.alphamapHeight;
                Vector3 size6 = baseTerrain.terrainData.size;
                Gizmos.DrawLine(from2, position4 + new Vector3(x2, 0f, num4 * size6.z));
            }
        }
    }

    public void CreateMudTerrains(List<MudStamp> mudStamps = null)
    {
        if (BaseTerrain.terrainData.alphamapResolution + 1 != BaseTerrain.terrainData.heightmapResolution)
        {
            UnityEngine.Debug.LogError(
                "Please make -Heightmap resolution- and -Control texture resolution- of Base Terrain equal.");
            base.enabled = false;
            return;
        }

        RemoveMudTerrains();
        InitializeBaseTerrain();
        Vector2[,] array = new Vector2[aRes / ChunkSize + 1, aRes / ChunkSize + 1];
        List<Vector2> list = new List<Vector2>();
        List<MudArea> list2 = new List<MudArea>();
        for (int i = 0; i < aRes; i++)
        {
            for (int j = 0; j < aRes; j++)
            {
                if (i % ChunkSize == 0 && j % ChunkSize == 0)
                {
                    array[i / ChunkSize, j / ChunkSize] = new Vector2(i, j);
                }
            }
        }

        for (int k = 0; k < array.GetUpperBound(0); k++)
        {
            for (int l = 0; l < array.GetUpperBound(1); l++)
            {
                for (int m = 0; m < ChunkSize; m++)
                {
                    for (int n = 0; n < ChunkSize; n++)
                    {
                        SurfaceMaterialUnderCar =
                            GetSurfaceMaterialByTerrainCoords(new Vector2(k * ChunkSize + m, l * ChunkSize + n));
                        if (SurfaceMaterialUnderCar != null &&
                            SurfaceMaterialUnderCar.surfaceType == SurfaceMaterial.SurfaceType.Mud &&
                            !list.Contains(new Vector2(k, l)))
                        {
                            list.Add(new Vector2(k, l));
                        }
                    }
                }
            }
        }

        UnityEngine.Debug.Log("Checking for mud");
        if (list.Count == 0)
        {
            return;
        }

        UnityEngine.Debug.Log("Got mud!");
        List<Vector2> list3 = new List<Vector2>(list);
        list2.Add(new MudArea());
        list2[0].Chunks = new List<Vector2>();
        int num = 0;
        List<Vector2> list4 = new List<Vector2>();
        list4.Add(list3[0]);
        list2[num].Chunks.Add(list4[0]);
        Vector2 item = default(Vector2);
        while (list3.Count > 0)
        {
            bool flag = false;
            if (list4.Count > 0)
            {
                for (int num2 = -1; num2 <= 1; num2++)
                {
                    for (int num3 = -1; num3 <= 1; num3++)
                    {
                        Vector2 vector = list4[0];
                        float x = vector.x + (float)num2;
                        Vector2 vector2 = list4[0];
                        item = new Vector2(x, vector2.y + (float)num3);
                        if (list3.Contains(item))
                        {
                            if (!list2[num].Chunks.Contains(item))
                            {
                                list2[num].Chunks.Add(item);
                            }

                            list3.Remove(list4[0]);
                            if (!list4.Contains(item) && list3.Contains(item))
                            {
                                List<Vector2> list5 = list4;
                                Vector2 vector3 = list4[0];
                                float x2 = vector3.x + (float)num2;
                                Vector2 vector4 = list4[0];
                                list5.Add(new Vector2(x2, vector4.y + (float)num3));
                            }

                            flag = true;
                        }
                    }
                }
            }

            if (list4.Count > 0)
            {
                list4.RemoveAt(0);
            }

            if (!flag && list3.Count > 0)
            {
                num++;
                list2.Add(new MudArea());
                list2[num].Chunks = new List<Vector2>();
                list4.Add(list3[0]);
            }
        }

        for (int num4 = 0; num4 < list2.Count; num4++)
        {
            MudArea mudArea = list2[num4];
            MudArea mudArea2 = list2[num4];
            Vector2 vector5 = list2[num4].Chunks[0];
            mudArea.xMin = (mudArea2.xMax = (int)vector5.x);
            MudArea mudArea3 = list2[num4];
            MudArea mudArea4 = list2[num4];
            Vector2 vector6 = list2[num4].Chunks[0];
            mudArea3.yMin = (mudArea4.yMax = (int)vector6.y);
            foreach (Vector2 chunk in list2[num4].Chunks)
            {
                Vector2 current = chunk;
                if (current.x < (float)list2[num4].xMin)
                {
                    list2[num4].xMin = (int)current.x;
                }

                if (current.x > (float)list2[num4].xMax)
                {
                    list2[num4].xMax = (int)current.x;
                }

                if (current.y < (float)list2[num4].yMin)
                {
                    list2[num4].yMin = (int)current.y;
                }

                if (current.y > (float)list2[num4].yMax)
                {
                    list2[num4].yMax = (int)current.y;
                }
            }

            list2[num4].xMin *= ChunkSize;
            list2[num4].yMin *= ChunkSize;
            list2[num4].xMax = list2[num4].xMax * ChunkSize + ChunkSize;
            list2[num4].yMax = list2[num4].yMax * ChunkSize + ChunkSize;
            list2[num4].Width = list2[num4].xMax - list2[num4].xMin;
            list2[num4].Height = list2[num4].yMax - list2[num4].yMin;
            while (list2[num4].Width != list2[num4].Height)
            {
                if (list2[num4].Width > list2[num4].Height)
                {
                    list2[num4].yMax += ChunkSize;
                }
                else
                {
                    list2[num4].xMax += ChunkSize;
                }

                list2[num4].Width = list2[num4].xMax - list2[num4].xMin;
                list2[num4].Height = list2[num4].yMax - list2[num4].yMin;
            }
        }

        foreach (MudArea item2 in list2)
        {
            for (int num5 = item2.xMin; num5 < item2.xMax; num5++)
            {
                for (int num6 = item2.yMin; num6 < item2.yMax; num6++)
                {
                    SurfaceMaterialUnderCar = GetSurfaceMaterialByTerrainCoords(new Vector2(num5, num6));
                    if (SurfaceMaterialUnderCar != null)
                    {
                        if (SurfaceMaterialUnderCar.surfaceType == SurfaceMaterial.SurfaceType.Mud)
                        {
                            item2.deformableMaterial = SurfaceMaterialUnderCar;
                        }

                        if (SurfaceMaterialUnderCar.MudWater)
                        {
                            item2.HasMudWater = true;
                        }
                    }
                }
            }
        }

        MudTerrains = new MudTerrain[list2.Count];
        for (int num7 = 0; num7 < list2.Count; num7++)
        {
            TerrainData terrainData = new TerrainData();
            Terrain component = Terrain.CreateTerrainGameObject(terrainData).GetComponent<Terrain>();
            component.transform.parent = BaseTerrain.transform;
            MudTerrains[num7] = component.gameObject.AddComponent<MudTerrain>();
            MudTerrains[num7].terrainData = terrainData;
            MudTerrains[num7].terrain = component;
            MudTerrains[num7].terrain.name = "MudTerrain" + num7.ToString();
            MudTerrains[num7].terrain.gameObject.layer = BaseTerrain.gameObject.layer;
            MudTerrains[num7].terrain.basemapDistance = SmallTerrainBasemapDistance;
            MudTerrains[num7].terrain.heightmapPixelError = 20f;
            MudTerrains[num7].DeformableMaterialMaxDepth = list2[num7].deformableMaterial.MaxDepth;
            int num8 = 65;
            if (maxSmallTerrainResolution > MaxSmallTerrainResolution.x65)
            {
                float num9 = (float)list2[num7].Width / (float)aRes;
                Vector3 size = BaseTerrain.terrainData.size;
                if (num9 * size.x * (float)TargetVertexPerMeterNumber > 65f)
                {
                    num8 = 129;
                }
            }

            if (maxSmallTerrainResolution > MaxSmallTerrainResolution.x129)
            {
                float num10 = (float)list2[num7].Width / (float)aRes;
                Vector3 size2 = BaseTerrain.terrainData.size;
                if (num10 * size2.x * (float)TargetVertexPerMeterNumber > 129f)
                {
                    num8 = 257;
                }
            }

            if (maxSmallTerrainResolution > list2[num7].deformableMaterial.MaxResolution)
            {
                if (list2[num7].deformableMaterial.MaxResolution == MaxSmallTerrainResolution.x65)
                {
                    num8 = 65;
                }

                if (list2[num7].deformableMaterial.MaxResolution > MaxSmallTerrainResolution.x65)
                {
                    num8 = 129;
                }

                if (list2[num7].deformableMaterial.MaxResolution > MaxSmallTerrainResolution.x129)
                {
                    num8 = 257;
                }
            }

            MudTerrains[num7].terrainData.heightmapResolution = num8;
            MudTerrains[num7].hRes = num8;
            MudTerrains[num7].hRes = num8;
            TerrainData terrainData2 = MudTerrains[num7].terrainData;
            float num11 = (float)list2[num7].Width / (float)aRes;
            Vector3 size3 = BaseTerrain.terrainData.size;
            float x3 = num11 * size3.x;
            Vector3 size4 = BaseTerrain.terrainData.size;
            float y = size4.y;
            float num12 = (float)list2[num7].Height / (float)aRes;
            Vector3 size5 = BaseTerrain.terrainData.size;
            terrainData2.size = new Vector3(x3, y, num12 * size5.z);
            TerrainData terrainData3 = MudTerrains[num7].terrainData;
            float num13 = BaseTerrain.terrainData.alphamapResolution;
            Vector3 size6 = BaseTerrain.terrainData.size;
            float num14 = num13 / size6.x;
            Vector3 size7 = MudTerrains[num7].terrainData.size;
            terrainData3.alphamapResolution = (int)(num14 * size7.x);
            float[,] array2 = new float[num8, num8];
            Vector3 position = BaseTerrain.GetPosition();
            float num15 = (float)list2[num7].xMin / (float)aRes;
            Vector3 size8 = BaseTerrain.terrainData.size;
            float x4 = num15 * size8.x;
            float num16 = (float)list2[num7].yMin / (float)aRes;
            Vector3 size9 = BaseTerrain.terrainData.size;
            Vector3 a = position + new Vector3(x4, 0f, num16 * size9.z);
            for (int num17 = 0; num17 < num8; num17++)
            {
                for (int num18 = 0; num18 < num8; num18++)
                {
                    Vector2 zero = Vector2.zero;
                    float x5 = a.x;
                    Vector3 position2 = BaseTerrain.GetPosition();
                    float num19 = x5 - position2.x;
                    float num20 = (float)num17 / (float)num8;
                    Vector3 size10 = MudTerrains[num7].terrainData.size;
                    zero.x = num19 + num20 * size10.x;
                    float z = a.z;
                    Vector3 position3 = BaseTerrain.GetPosition();
                    float num21 = z - position3.z;
                    float num22 = (float)num18 / (float)num8;
                    Vector3 size11 = MudTerrains[num7].terrainData.size;
                    zero.y = num21 + num22 * size11.z;
                    float[,] array3 = array2;
                    int num23 = num18;
                    int num24 = num17;
                    TerrainData terrainData4 = BaseTerrain.terrainData;
                    float x6 = zero.x;
                    Vector3 size12 = BaseTerrain.terrainData.size;
                    float x7 = x6 / size12.x;
                    float y2 = zero.y;
                    Vector3 size13 = BaseTerrain.terrainData.size;
                    float interpolatedHeight = terrainData4.GetInterpolatedHeight(x7, y2 / size13.z);
                    Vector3 size14 = BaseTerrain.terrainData.size;
                    array3[num23, num24] = interpolatedHeight / size14.y;
                }
            }

            MudTerrains[num7].terrainData.SetHeights(0, 0, array2);
            int num25 = num8 - 1;
            MudTerrains[num7].terrainData.alphamapResolution = num25;
            MudTerrains[num7].terrainData.splatPrototypes = BaseTerrain.terrainData.splatPrototypes;
            MudTerrains[num7].aRes = num25;
            MudTerrains[num7].aRes = num25;
            float[,,] array4 = new float[num25, num25, terrainTextures.Length];
            float[,,] alphamaps = BaseTerrain.terrainData.GetAlphamaps(list2[num7].xMin, list2[num7].yMin,
                list2[num7].Width, list2[num7].Height);
            List<SplatPrototype> list6 = new List<SplatPrototype>();
            List<int> list7 = new List<int>();
            for (int num26 = 0; num26 < num25; num26++)
            {
                for (int num27 = 0; num27 < num25; num27++)
                {
                    for (int num28 = 0; num28 < terrainTextures.Length; num28++)
                    {
                        array4[num27, num26, num28] =
                            alphamaps[(int)((float)num27 / (float)num25 * (float)list2[num7].Height),
                                (int)((float)num26 / (float)num25 * (float)list2[num7].Width), num28];
                        if (array4[num27, num26, num28] > 0f && !list7.Contains(num28))
                        {
                            list7.Add(num28);
                        }
                    }
                }
            }

            MudTerrains[num7].terrainData.SetAlphamaps(0, 0, array4);
            foreach (int item3 in list7)
            {
                list6.Add(BaseTerrain.terrainData.splatPrototypes[item3]);
            }

            MudTerrains[num7].textures = new Texture2D[list6.Count];
            for (int num29 = 0; num29 < list6.Count; num29++)
            {
                MudTerrains[num7].textures[num29] = list6[num29].texture;
            }

            MudTerrains[num7].terrainData.splatPrototypes = list6.ToArray();
            List<Vector2> list8 = new List<Vector2>();
            for (int num30 = 0; num30 < BaseTerrain.terrainData.splatPrototypes.Length; num30++)
            {
                for (int num31 = 0; num31 < list6.Count; num31++)
                {
                    if (BaseTerrain.terrainData.splatPrototypes[num30].texture.Equals(list6[num31].texture))
                    {
                        list8.Add(new Vector2(num30, num31));
                    }
                }
            }

            float[,,] array5 = new float[num25, num25, list8.Count];
            for (int num32 = 0; num32 < num25; num32++)
            {
                for (int num33 = 0; num33 < num25; num33++)
                {
                    for (int num34 = 0; num34 < list8.Count; num34++)
                    {
                        float[,,] array6 = array5;
                        int num35 = num33;
                        int num36 = num32;
                        Vector2 vector7 = list8[num34];
                        int num37 = (int)vector7.y;
                        float[,,] array7 = array4;
                        int num38 = num33;
                        int num39 = num32;
                        Vector2 vector8 = list8[num34];
                        array6[num35, num36, num37] = array7[num38, num39, (int)vector8.x];
                    }
                }
            }

            MudTerrains[num7].terrainData.SetAlphamaps(0, 0, array5);
            MudTerrains[num7].terrain.transform.position = a + new Vector3(0f, -0.03f, 0f);
            MudTerrains[num7].terrain.materialType = BaseTerrain.materialType;
            MudTerrains[num7].terrain.materialTemplate = BaseTerrain.materialTemplate;
            MudTerrain obj = MudTerrains[num7];
            Vector3 position4 = BaseTerrain.GetPosition();
            float x8 = position4.x;
            float num40 = (float)list2[num7].xMin / (float)hRes;
            Vector3 size15 = BaseTerrain.terrainData.size;
            float x9 = x8 + num40 * size15.x;
            Vector3 position5 = BaseTerrain.GetPosition();
            float z2 = position5.z;
            float num41 = (float)list2[num7].yMin / (float)hRes;
            Vector3 size16 = BaseTerrain.terrainData.size;
            float y3 = z2 + num41 * size16.z;
            float num42 = (float)list2[num7].Width / (float)aRes;
            Vector3 size17 = BaseTerrain.terrainData.size;
            float width = num42 * size17.x;
            float num43 = (float)list2[num7].Height / (float)aRes;
            Vector3 size18 = BaseTerrain.terrainData.size;
            obj.terRect = new Rect(x9, y3, width, num43 * size18.z);
            foreach (MudStamp mudStamp in mudStamps)
            {
                if (MudTerrains[num7].terRect.Contains(new Vector2(mudStamp.stampPosition.x, mudStamp.stampPosition.z)))
                {
                    MudTerrains[num7].drag = mudStamp.mudViscosity;
                }
            }
        }

        float[,] heights = BaseTerrain.terrainData.GetHeights(0, 0, hRes, hRes);
        Random.InitState(0);
        for (int num44 = 0; num44 < aRes; num44++)
        {
            for (int num45 = 0; num45 < aRes; num45++)
            {
                SurfaceMaterialUnderCar = GetSurfaceMaterialByTerrainCoords(new Vector2(num44, num45));
                Vector3 position6 = BaseTerrain.GetPosition();
                float num46 = (float)num44 / (float)hRes;
                Vector3 size19 = BaseTerrain.terrainData.size;
                float x10 = num46 * size19.x;
                float num47 = (float)num45 / (float)hRes;
                Vector3 size20 = BaseTerrain.terrainData.size;
                Vector3 point = position6 + new Vector3(x10, 0f, num47 * size20.z);
                float num48 = 0f;
                float num49 = 0f;
                if (SurfaceMaterialUnderCar != null)
                {
                    float maxDepth = SurfaceMaterialUnderCar.MaxDepth;
                    Vector3 size21 = BaseTerrain.terrainData.size;
                    num49 = maxDepth / size21.y;
                }

                foreach (MudStamp mudStamp2 in mudStamps)
                {
                    if (DoesPointBelongToMudStamp(mudStamp2, point))
                    {
                        float mudDepth = mudStamp2.mudDepth;
                        Vector3 size22 = BaseTerrain.terrainData.size;
                        num49 = mudDepth / size22.y;
                    }
                }

                if (SurfaceMaterialUnderCar != null &&
                    SurfaceMaterialUnderCar.surfaceType == SurfaceMaterial.SurfaceType.Mud)
                {
                    num48 = num49;
                }

                if (num48 != 0f)
                {
                    float num50 = num48;
                    Vector3 size23 = BaseTerrain.terrainData.size;
                    float min = 0.1f / size23.y;
                    Vector3 size24 = BaseTerrain.terrainData.size;
                    num48 = num50 + UnityEngine.Random.Range(min, 0.7f / size24.y);
                }

                heights[num45, num44] -= num48;
            }
        }

        BaseTerrain.terrainData.SetHeights(0, 0, heights);
        UnityEngine.Debug.Log(MudTerrains.Length + " mud terrains were successfully created");
        for (int num51 = 0; num51 < list2.Count; num51++)
        {
            if (list2[num51].HasMudWater)
            {
                CreateMudWaterMesh(MudTerrains[num51].terrain);
            }
        }
    }

    private bool DoesPointBelongToMudStamp(MudStamp mudStamp, Vector3 point)
    {
        Vector3 stampPosition = mudStamp.stampPosition;
        stampPosition.y = 0f;
        Vector3 a = point;
        a.y = 0f;
        return Vector3.Distance(a, stampPosition) < mudStamp.boundsRadius;
    }

    private void CreateMudWaterMesh(Terrain ter)
    {
        int mudWaterResoultion = MudWaterResoultion;
        int mudWaterResoultion2 = MudWaterResoultion;
        float num = ter.terrainData.heightmapResolution;
        Vector3 heightmapScale = ter.terrainData.heightmapScale;
        float num2 = num * heightmapScale.x;
        float num3 = ter.terrainData.heightmapResolution;
        Vector3 heightmapScale2 = ter.terrainData.heightmapScale;
        float num4 = num3 * heightmapScale2.z;
        float num5 = 0f;
        Vector3 position = ter.transform.position;
        Vector3[] array = new Vector3[mudWaterResoultion * mudWaterResoultion2];
        int[] array2 = new int[mudWaterResoultion * mudWaterResoultion2 * 2 * 3];
        int num6 = 0;
        for (int i = 0; i < mudWaterResoultion; i++)
        {
            for (int j = 0; j < mudWaterResoultion2; j++)
            {
                SurfaceMaterialUnderCar = GetSurfaceMaterialByWorldCoords(new Vector3(
                    position.x + num2 / (float)(mudWaterResoultion2 - 1) * (float)j, 0f,
                    position.z + num4 / (float)(mudWaterResoultion - 1) * (float)i));
                float num7 = 0.1f;
                if (SurfaceMaterialUnderCar != null)
                {
                    num7 = Mathf.Max(0.1f, SurfaceMaterialUnderCar.MudWaterDepth);
                    if (SurfaceMaterialUnderCar.surfaceType == SurfaceMaterial.SurfaceType.Hard)
                    {
                        num7 = 5f;
                    }
                }

                num5 = ter.SampleHeight(new Vector3(position.x + num2 / (float)(mudWaterResoultion2 - 1) * (float)j, 0f,
                    position.z + num4 / (float)(mudWaterResoultion - 1) * (float)i)) - num7;
                array[i * mudWaterResoultion2 + j] = new Vector3(num2 / (float)(mudWaterResoultion2 - 1) * (float)j,
                    num5, num4 / (float)(mudWaterResoultion - 1) * (float)i);
                if (i > 0 && j > 0)
                {
                    array2[num6] = i * mudWaterResoultion2 + j - 1;
                    array2[num6 + 1] = (i - 1) * mudWaterResoultion2 + j;
                    array2[num6 + 2] = (i - 1) * mudWaterResoultion2 + j - 1;
                    num6 += 3;
                    array2[num6] = i * mudWaterResoultion2 + j;
                    array2[num6 + 1] = (i - 1) * mudWaterResoultion2 + j;
                    array2[num6 + 2] = i * mudWaterResoultion2 + j - 1;
                    num6 += 3;
                }
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = array;
        mesh.triangles = array2;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        GameObject gameObject = new GameObject();
        gameObject.name = "MudWaterMesh";
        gameObject.transform.parent = ter.transform;
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = MudWaterMaterial;
        gameObject.AddComponent<WaterBasic>();
        gameObject.transform.position = ter.GetPosition();
    }

    public float GetTireFriction(int wheelID, int InstalledTire)
    {
        if (m_fxData == null)
        {
            return 1f;
        }

        if (wheelID >= m_fxData.Length)
        {
            return 1f;
        }

        if (m_fxData[wheelID].surfaceMaterial != null && m_fxData[wheelID].surfaceMaterial.TiresFriction != null &&
            InstalledTire < m_fxData[wheelID].surfaceMaterial.TiresFriction.Length)
        {
            return m_fxData[wheelID].surfaceMaterial.TiresFriction[InstalledTire];
        }

        return 1f;
    }

    public static Vector2 GetTerrainCoords(Vector3 WorldCoords, Terrain ter)
    {
        Vector2 zero = Vector2.zero;
        Vector3 vector = WorldCoords - ter.GetPosition();
        float x = vector.x;
        Vector3 size = ter.terrainData.size;
        zero.x = x / size.x * (float)ter.terrainData.heightmapResolution;
        Vector3 vector2 = WorldCoords - ter.GetPosition();
        float z = vector2.z;
        Vector3 size2 = ter.terrainData.size;
        zero.y = z / size2.z * (float)ter.terrainData.heightmapResolution;
        return zero;
    }

    private SurfaceMaterial GetSurfaceMaterialByTerrainCoords(Vector2 terCoords)
    {
        if (terCoords.x < 0f || terCoords.x > (float)BaseTerrain.terrainData.alphamapWidth || terCoords.y < 0f ||
            terCoords.y > (float)BaseTerrain.terrainData.alphamapHeight)
        {
            return null;
        }

        float[,,] alphamaps = BaseTerrain.terrainData.GetAlphamaps((int)terCoords.x, (int)terCoords.y, 1, 1);
        for (int i = 0; i < terrainTextures.Length; i++)
        {
            if (!(alphamaps[0, 0, i] > 0.5f))
            {
                continue;
            }

            for (int j = 0; j < surfaceMaterials.Length; j++)
            {
                if (terrainTextures[i].Equals(surfaceMaterials[j].TerrainTexture))
                {
                    return surfaceMaterials[j];
                }
            }
        }

        return null;
    }

    private SurfaceMaterial GetSurfaceMaterialByWorldCoords(Vector3 WorldCoords)
    {
        Vector2 terrainCoords = GetTerrainCoords(WorldCoords, BaseTerrain);
        return GetSurfaceMaterialByTerrainCoords(terrainCoords);
    }

    private SurfaceMaterial GetSurfaceMaterialAtPoint(Vector3 point, Collider hitCollider)
    {
        if (hitCollider == null)
        {
            return null;
        }

        if (hitCollider.GetType() == typeof(TerrainCollider))
        {
            if (MudTerrains != null)
            {
                for (int i = 0; i < MudTerrains.Length; i++)
                {
                    if (MudTerrains[i].terRect.Contains(new Vector2(point.x, point.z)))
                    {
                        MudTerrains[i].LODS_Baked = false;
                        CurrentMudTerrain = MudTerrains[i];
                        break;
                    }

                    if (!MudTerrains[i].LODS_Baked)
                    {
                        MudTerrains[i].LODS_Baked = true;
                        MudTerrains[i].terrain.ApplyDelayedHeightmapModification();
                    }
                }
            }

            return GetSurfaceMaterialByWorldCoords(point);
        }

        for (int j = 0; j < surfaceMaterials.Length; j++)
        {
            if (surfaceMaterials[j].physicMaterial != null &&
                hitCollider.material.name.Equals(surfaceMaterials[j].physicMaterial.name + " (Instance)"))
            {
                CurrentMudTerrain = null;
                return surfaceMaterials[j];
            }
        }

        CurrentMudTerrain = null;
        return null;
    }

    private void PushTerrain(int wheelID)
    {
        WheelComponent wc = carController.wheels[wheelID].wc;
        if (!wc.IsGrounded || CurrentMudTerrain == null || CurrentMudTerrain.DefaultHeights == null)
        {
            return;
        }

        SurfaceMaterial surfaceMaterial = m_fxData[wheelID].surfaceMaterial;
        Vector3 vector = wc.wheelCollider.worldHitPos +
                         wc.transform.forward * DeformationPointOffset * Mathf.Sign(wc.rpm);
        Vector2 terrainCoords = GetTerrainCoords(vector, CurrentMudTerrain.terrain);
        int num = Mathf.Clamp((int)terrainCoords.x - PushDiameter / 2, 0,
            CurrentMudTerrain.aRes - PushDiameter / 2 - 2);
        int num2 = Mathf.Clamp((int)terrainCoords.y - PushDiameter / 2, 0,
            CurrentMudTerrain.aRes - PushDiameter / 2 - 2);
        float[,] heights = CurrentMudTerrain.terrainData.GetHeights(num, num2, PushDiameter, PushDiameter);
        float[,] array = new float[PushDiameter, PushDiameter];
        for (int i = 0; i < PushDiameter; i++)
        {
            for (int j = 0; j < PushDiameter; j++)
            {
                array[j, i] = CurrentMudTerrain.DefaultHeights[num2 + j, num + i];
            }
        }

        for (int k = 0; k < PushDiameter; k++)
        {
            for (int l = 0; l < PushDiameter; l++)
            {
                int x = (int)((float)k / (float)PushDiameter * (float)PushStampTexture.width);
                int y = (int)((float)l / (float)PushDiameter * (float)PushStampTexture.height);
                if (surfaceMaterial.surfaceType == SurfaceMaterial.SurfaceType.Mud)
                {
                    Color pixel = PushStampTexture.GetPixel(x, y);
                    float num3 = pixel.r;
                    if (pixel.g > pixel.r)
                    {
                        num3 = 0f - pixel.g;
                    }

                    Vector2 vector2 = new Vector2(num + k, num2 + l);
                    Vector3 position = CurrentMudTerrain.terrain.GetPosition();
                    float num4 = vector2.x / (float)CurrentMudTerrain.terrainData.heightmapResolution;
                    Vector3 size = CurrentMudTerrain.terrainData.size;
                    float x2 = num4 * size.x;
                    float num5 = vector2.y / (float)CurrentMudTerrain.terrainData.heightmapResolution;
                    Vector3 size2 = CurrentMudTerrain.terrainData.size;
                    Vector3 a = position + new Vector3(x2, 0f, num5 * size2.z);
                    a.y = vector.y;
                    Vector3 lhs = wc.transform.forward * Mathf.Sign(wc.rpm);
                    float num6 = Vector3.Dot(lhs, a - vector);
                    if (num6 < 0f && num3 < 0f)
                    {
                        num3 = 0f;
                    }

                    ref float reference = ref heights[l, k];
                    float num7 = reference;
                    float num8 = num3 * (1f - surfaceMaterial.Hardness) * PushStrength;
                    Vector3 size3 = CurrentMudTerrain.terrainData.size;
                    reference = num7 - num8 / size3.y * 2f;
                }

                float num9 = array[l, k];
                float maxDepth = surfaceMaterial.MaxDepth;
                Vector3 size4 = CurrentMudTerrain.terrainData.size;
                float min = num9 - maxDepth / size4.y;
                float num10 = array[l, k];
                float maxExtrudeHeight = surfaceMaterial.MaxExtrudeHeight;
                Vector3 size5 = CurrentMudTerrain.terrainData.size;
                float max = num10 + maxExtrudeHeight / size5.y;
                heights[l, k] = Mathf.Clamp(heights[l, k], min, max);
            }
        }

        if (surfaceMaterial.surfaceType == SurfaceMaterial.SurfaceType.Mud)
        {
            CurrentMudTerrain.terrainData.SetHeightsDelayLOD(num, num2, heights);
        }
    }

    public bool IsCarInWater()
    {
        if (WaterMeshesRects == null || carController == null)
        {
            return false;
        }

        for (int i = 0; i < WaterMeshesRects.Count; i++)
        {
            Rect rect = WaterMeshesRects[i];
            Vector3 position = carController.transform.position;
            float x = position.x;
            Vector3 position2 = carController.transform.position;
            if (rect.Contains(new Vector2(x, position2.z)))
            {
                return true;
            }
        }

        return false;
    }

    public bool IsCameraInWater()
    {
        if (WaterMeshesRects == null || carController == null)
        {
            return false;
        }

        for (int i = 0; i < WaterMeshesRects.Count; i++)
        {
            Rect rect = WaterMeshesRects[i];
            Vector3 position = Camera.main.transform.position;
            float x = position.x;
            Vector3 position2 = Camera.main.transform.position;
            if (rect.Contains(new Vector2(x, position2.z)))
            {
                Vector3 position3 = WaterMeshes[i].transform.position;
                float y = position3.y;
                Vector3 position4 = Camera.main.transform.position;
                if (y > position4.y)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public int WhatWaterMeshIsCarOn()
    {
        if (!IsCarInWater())
        {
            return 999;
        }

        for (int i = 0; i < WaterMeshesRects.Count; i++)
        {
            Rect rect = WaterMeshesRects[i];
            Vector3 position = carController.transform.position;
            float x = position.x;
            Vector3 position2 = carController.transform.position;
            if (rect.Contains(new Vector2(x, position2.z)))
            {
                return i;
            }
        }

        return 999;
    }

    private void DoParticles(int wheelID)
    {
        WheelComponent wc = carController.wheels[wheelID].wc;
        if (!wc.IsGrounded)
        {
            return;
        }

        SurfaceMaterial surfaceMaterial = m_fxData[wheelID].surfaceMaterial;
        int num = 0;
        CurrentParticles = null;
        SecondaryParticles = null;
        int index = WhatWaterMeshIsCarOn();
        int count = 0;
        bool flag = false;
        switch (surfaceMaterial.particlesType)
        {
            case SurfaceMaterial.ParticlesType.MudPieces:
                CurrentParticles = MudParticles;
                num = ((wc.CommonSlip > MudSlipThreshold) ? ((int)(wc.CommonSlip * (float)MudEmitStrength)) : 0);
                break;
            case SurfaceMaterial.ParticlesType.SandDust:
                CurrentParticles = SandDustParticles;
                num = ((Mathf.Abs(wc.rpm) > SandDustMinRpm) ? SandDustEmitStrength : 0);
                break;
            case SurfaceMaterial.ParticlesType.WaterSplash:
                CurrentParticles = WaterSplashParticles;
                SecondaryParticles = RippleParticles;
                flag = true;
                num = ((Mathf.Abs(wc.rpm) > WaterSplashMinRpm) ? WaterSplashEmitStrength : 0);
                count = ((Mathf.Abs(carController.Speed) > (float)RipplesSpeedThreshold) ? 1 : 0);
                break;
            case SurfaceMaterial.ParticlesType.SkidSmoke:
                CurrentParticles = SkidSmokeParticles;
                num = ((wc.CommonSlip > SkidSmokeSlipThreshold)
                    ? ((int)((float)SkidSmokeEmitStrength * wc.CommonSlip))
                    : 0);
                break;
            case SurfaceMaterial.ParticlesType.None:
                CurrentParticles = null;
                num = 0;
                break;
            case SurfaceMaterial.ParticlesType.Snow:
                CurrentParticles = SnowParticles;
                num = ((Mathf.Abs(wc.rpm) > SnowRPMThreshold) ? SnowEmitStrength : 0);
                break;
        }

        if (IsCarInWater())
        {
            float y = wc.wheelCollider.hitPoint.y;
            Vector3 position = WaterMeshes[index].transform.position;
            if (y < position.y)
            {
                CurrentParticles = WaterSplashParticles;
                SecondaryParticles = RippleParticles;
                num = ((Mathf.Abs(wc.rpm) > WaterSplashMinRpm) ? WaterSplashEmitStrength : 0);
                count = ((Mathf.Abs(carController.Speed) > (float)RipplesSpeedThreshold) ? 1 : 0);
                CleanBody(useDefault: true);
            }
        }

        if (CurrentParticles != null && wc != null)
        {
            CurrentParticles.transform.position = wc.wheelCollider.worldHitPos;
            CurrentParticles.transform.rotation = wc.transform.rotation;
            CurrentParticles.transform.eulerAngles += new Vector3(-45f, (!(wc.rpm < 0f)) ? 180 : 0, 0f);
            CurrentParticles.Emit(num);
            if (num > 0 && CurrentParticles == MudParticles)
            {
                AddDirtToCarBody();
            }

            if (num > 0 && CurrentParticles == WaterSplashParticles)
            {
                CleanBody();
            }
        }

        if (SecondaryParticles != null)
        {
            Vector3 position2 = carController.transform.position;
            Vector3 position3 = WaterMeshes[index].transform.position;
            position2.y = position3.y + 0.01f;
            SecondaryParticles.transform.position = position2;
            SecondaryParticles.Emit(count);
        }
    }

    private void CleanBody(bool useDefault = false)
    {
        if (SurfaceMaterialUnderCar != null)
        {
            float num = 0.2f;
            if (useDefault)
            {
                partsSwitcher.Dirtiness -= num * Time.deltaTime;
            }
            else
            {
                partsSwitcher.Dirtiness -= SurfaceMaterialUnderCar.CleaningSpeed * Time.deltaTime;
            }

            if (!useDefault)
            {
                partsSwitcher.Dirtiness = Mathf.Max(SurfaceMaterialUnderCar.MinimumDirtiness,
                    Mathf.Clamp01(partsSwitcher.Dirtiness));
            }

            partsSwitcher.UpdateDirtiness();
            CarWetnessTarget = 1f;
        }
    }

    private void AddDirtToCarBody()
    {
        if (SurfaceMaterialUnderCar != null && !(partsSwitcher == null))
        {
            partsSwitcher.Dirtiness += SurfaceMaterialUnderCar.DirtinessSpeed * Time.deltaTime;
            partsSwitcher.Dirtiness = Mathf.Max(SurfaceMaterialUnderCar.MinimumDirtiness,
                Mathf.Clamp01(partsSwitcher.Dirtiness));
            partsSwitcher.UpdateDirtiness();
            CarWetnessTarget = 1f;
        }
    }

    private void UpdateCarWetness()
    {
        if (!(partsSwitcher == null))
        {
            if (partsSwitcher.MudWetness == CarWetnessTarget)
            {
                CarWetnessTarget = 0f;
            }

            float num = (CarWetnessTarget != 1f) ? 1 : 2;
            partsSwitcher.MudWetness = Mathf.MoveTowards(partsSwitcher.MudWetness, CarWetnessTarget,
                ((!(CarWetnessTarget > partsSwitcher.MudWetness)) ? DrynessSpeed : WetnessSpeed) * num *
                Time.deltaTime);
            partsSwitcher.Dirtiness = Mathf.Clamp01(partsSwitcher.Dirtiness);
            partsSwitcher.UpdateDirtiness();
        }
    }

    private void ProcessWheels()
    {
        if (carController == null)
        {
            return;
        }

        if (lastCarController != carController)
        {
            InitializeTireFxData();
        }

        lastCarController = carController;
        SurfaceMaterialUnderCar = GetSurfaceMaterialAtPoint(carController.transform.position,
            carController.wheels[0].wc.wheelCollider.contactColliderHit);
        UpdateCarWetness();
        for (int i = 0; i < carController.wheels.Count; i++)
        {
            if (!(carController.wheels[i].wc.wheelCollider != null))
            {
                continue;
            }

            m_fxData[i].surfaceMaterial = GetSurfaceMaterialAtPoint(carController.wheels[i].wc.transform.position,
                carController.wheels[i].wc.wheelCollider.contactColliderHit);
            m_fxData[i].marksRenderer = null;
            SurfaceMaterial surfaceMaterial = m_fxData[i].surfaceMaterial;
            carController.wheels[i].wc.wheelCollider.hitOffset = 0f;
            if (surfaceMaterial == null)
            {
                continue;
            }

            switch (surfaceMaterial.tireTracks)
            {
                case SurfaceMaterial.TireTracks.MudMark:
                    m_fxData[i].marksRenderer = MudMarksRenderer;
                    break;
                case SurfaceMaterial.TireTracks.SnowMark:
                    m_fxData[i].marksRenderer = SnowMarksRenderer;
                    break;
                case SurfaceMaterial.TireTracks.SandMark:
                    m_fxData[i].marksRenderer = SandMarksRenderer;
                    break;
                case SurfaceMaterial.TireTracks.SkidMark:
                    if (carController.wheels[i].wc.CommonSlip > 0.5f)
                    {
                        m_fxData[i].marksRenderer = SkidMarksRenderer;
                    }

                    break;
            }

            carController.wheels[i].wc.wheelCollider.hitOffset =
                ((surfaceMaterial.surfaceType != SurfaceMaterial.SurfaceType.Snow) ? 0f : surfaceMaterial.MaxDepth);
            if (CurrentQuality > QualityLevel.Low)
            {
                PushTerrain(i);
                DoParticles(i);
            }
        }

        float num = 0f;
        if (SurfaceMaterialUnderCar != null && carController.Grounded())
        {
            if (CurrentMudTerrain != null && SurfaceMaterialUnderCar.AddedDrag > 0f)
            {
                num = CurrentMudTerrain.drag;
            }

            if (num == 0f)
            {
                num = SurfaceMaterialUnderCar.AddedDrag;
            }
        }

        playerRigidbody.drag = num;
    }

    private void Update()
    {
        if (m_fxData != null)
        {
            for (int i = 0; i < m_fxData.Length; i++)
            {
                UpdateTireMarks(i);
            }
        }

        if (Time.frameCount % UpdateEveryNFrame != 0)
        {
            return;
        }

        ProcessWheels();
        if (IsCameraInWater() && TargetTintColor != WaterTintColor)
        {
            TargetTintColor = WaterTintColor;
            if (BlurScript != null && QualitySettings.GetQualityLevel() >= 2)
            {
                BlurScript.enabled = true;
            }

            BlurAmountTarget = 10f;
            AudioListener.volume = 0.1f;
        }
        else if (!IsCameraInWater() && TargetTintColor != SkyTintColor)
        {
            TargetTintColor = SkyTintColor;
            if (BlurScript != null)
            {
                BlurScript.enabled = false;
                BlurScript.blurSize = 0f;
            }

            AudioListener.volume = 1f;
        }

        RenderSettings.ambientSkyColor =
            Color.Lerp(RenderSettings.ambientSkyColor, TargetTintColor, 6f * Time.deltaTime);
        if (BlurScript != null && BlurScript.enabled)
        {
            BlurScript.blurSize = Mathf.Lerp(BlurScript.blurSize, 10f, 3f * Time.deltaTime);
        }
    }
}