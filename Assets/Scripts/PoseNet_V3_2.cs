using Unity.Barracuda;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;

public class PoseNet_V3_2 : MonoBehaviour
{
    [Tooltip("The input image that will be fed to the model")]
    public RenderTexture videoTexture;

    [Tooltip("The requested webcam height")]
    public int webcamHeight = 720;

    [Tooltip("The requested webcam width")]
    public int webcamWidth = 1280;

    [Tooltip("The requested webcam frame rate")]
    public int webcamFPS = 60;

    [Tooltip("The height of the image being fed to the model")]
    public int imageHeight = 192;

    [Tooltip("The width of the image being fed to the model")]
    public int imageWidth = 256;

    [Tooltip("Use webcam feed as input")]
    public bool useWebcam = true;

    public int distance = 0;

    [Tooltip("The model asset file to use when performing inference")]
    public NNModel modelAsset;

    public NNModel liftmodelAsset;

    [Tooltip("The backend to use when performing inference")]
    public WorkerFactory.Type workerType = WorkerFactory.Type.Auto;

    [Tooltip("The minimum confidence level required to display the key point")]
    [Range(0, 100)]
    public int minConfidence = 70;

    [Tooltip("The list of key point GameObjects that make up the pose skeleton")]
    public GameObject[] keypoints;
    public GameObject[] keypoints2; // 2�ο�
    public GameObject VideoScreen;
    public RawImage UIVideo;
    public RawImage UIVideo2;
    public GameObject AvatarImage;

    public LayerMask _layer;

    // The compiled model used for performing inference
    private Model Pose2DModel;

    private Model LiftModel;

    // The interface used to execute the neural network
    private IWorker engine;

    private IWorker engine2;

    // The name for the Sigmoid layer that returns the heatmap predictions
    private string predictionLayer = "520";

    // The number of key points estimated by the model
    private const int numKeypoints = 17;

    // Stores the current estimated 2D keypoint locations in videoTexture
    // and their associated confidence values
    private float[][] keypointLocations = new float[numKeypoints][];
    private float[][] keypointLocations2 = new float[numKeypoints][]; // 2�ο�

    // Live video input from a webcam
    private WebCamTexture webcamTexture;

    // The height of the current video source
    private int videoHeight;

    // The width of the current video source
    private int videoWidth;

    private RenderTexture MainTexture;
    private RenderTexture MainTexture2; // 2�ο�


    public List<GameObject> PoseEstimator;
    public List<GameObject> PoseEstimator2; // 2�ο�

    // Start is called before the first frame update
    public static int Frames = 27;

    
    private void Awake()
    {
        
        

        for (int i = 0; i < Frames; i++)
        {
            PoseData_X.Add(new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            PoseData_Y.Add(new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            PoseData_X2.Add(new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            PoseData_Y2.Add(new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
        }
    }
    void Start()
    {
        for (int i = 0; i < numKeypoints; i++)
        {
            if (GameManager.instance.ForNumber)
            {
                keypoints2[i].SetActive(true);
                PoseEstimator2[i].SetActive(true);
            }
            else
            {
                keypoints2[i].SetActive(false);
                PoseEstimator2[i].SetActive(false);
            }
        }
        // Get a reference to the Video Player GameObject
        GameObject videoPlayer = GameObject.Find("Video Player");
        // Get the Transform component for the VideoScreen GameObject
        Transform videoScreen = GameObject.Find("VideoScreen").transform;
        if (useWebcam)
        {
            // Create a new WebCamTexture
            webcamTexture = new WebCamTexture(webcamWidth, webcamHeight, webcamFPS);

            // Flip the VideoScreen around the Y-Axis
            videoScreen.rotation = Quaternion.Euler(0, 180, 0);
            // Invert the scale value for the Z-Axis
            videoScreen.localScale = new Vector3(videoScreen.localScale.x, videoScreen.localScale.y, -1f);

            // Start the Camera
            webcamTexture.Play();

            // Deactivate the Video Player
            videoPlayer.SetActive(false);

            // Update the videoHeight
            videoHeight = (int)webcamTexture.height;
            // Update the videoWidth
            videoWidth = (int)webcamTexture.width;

        }
        else
        {
            // Update the videoHeight
            videoHeight = (int)videoPlayer.GetComponent<VideoPlayer>().height;
            // Update the videoWidth
            videoWidth = (int)videoPlayer.GetComponent<VideoPlayer>().width;
        }

        // Release the current videoTexture
        videoTexture.Release();
        // Create a new videoTexture using the current video dimensions
        videoTexture = new RenderTexture(videoWidth, videoHeight, 24, RenderTextureFormat.ARGB32);
        UIVideo.texture = videoTexture;

        //UIVideo.transform.rotation = Quaternion.Euler(0, 180, 180);
        if (GameManager.instance.ForNumber == true)
        {
            UIVideo2.texture = videoTexture;

            //UIVideo2.transform.rotation = Quaternion.Euler(0, 180, 180);
            //AvatarImage.GetComponent<RectTransform>().localPosition = new Vector3(0.0f, 100.0f, 0.0f);
            UIVideo2.gameObject.SetActive(true);
        }
        else
        {
            //AvatarImage.GetComponent<RectTransform>().localPosition = new Vector3(-400.0f, 100.0f, 0.0f);
            UIVideo2.gameObject.SetActive(false);
        }
        

        // Use new videoTexture for Video Player
        videoPlayer.GetComponent<VideoPlayer>().targetTexture = videoTexture;

        // Apply the new videoTexture to the VideoScreen Gameobject
        videoScreen.gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", videoTexture);
        // Adjust the VideoScreen dimensions for the new videoTexture
        videoScreen.localScale = new Vector3(videoWidth, videoHeight, videoScreen.localScale.z);
        // Adjust the VideoScreen position for the new videoTexture 
        videoScreen.position = new Vector3(videoWidth / 2, videoHeight / 2, 1);

        // Get a reference to the Main Camera GameObject
        GameObject mainCamera = GameObject.Find("Main Camera");
        // Adjust the camera position to account for updates to the VideoScreen
        mainCamera.transform.position = new Vector3(videoWidth / 2, videoHeight / 2, -(videoWidth / 2));
        // Adjust the camera size to account for updates to the VideoScreen
        mainCamera.GetComponent<Camera>().orthographicSize = videoHeight / 2;

        // Compile the model asset into an object oriented representation
        Pose2DModel = ModelLoader.Load(modelAsset);
        ModelBuilder modelbuilder = new ModelBuilder(Pose2DModel);
        LiftModel = ModelLoader.Load(liftmodelAsset);
        ModelBuilder liftmodelbuilder = new ModelBuilder(LiftModel);
        modelbuilder.Sigmoid(predictionLayer, Pose2DModel.outputs[0]);
        // Create a model builder to modify the m_RunTimeModel
        //var modelBuilder = new ModelBuilder(m_RunTimeModel);
        // Add a new Sigmoid layer that takes the output of the heatmap layer
        //modelBuilder.Sigmoid(predictionLayer, heatmapLayer);

        // Create a worker that will execute the model with the selected backend
        engine = WorkerFactory.CreateWorker(workerType, modelbuilder.model);
        engine2 = WorkerFactory.CreateWorker(workerType, liftmodelbuilder.model);
        //engine = WorkerFactory.CreateWorker(workerType, m_RunTimeModel);
        if (GameManager.instance.ForNumber == true)
        {
            CameraSet2();
            Debug.Log("Two");
        }
        else
        {
            CameraSet1();
        }

    }

    // OnDisable is called when the MonoBehavior becomes disabled or inactive
    private void OnDisable()
    {
        // Release the resources allocated for the inference engine
        engine.Dispose();
        engine2.Dispose();
        webcamTexture.Stop();
    }
    private void LateUpdate()
    {
        
        
        if (GameManager.instance.ForNumber == true)
        {
            Dictionary<string, Tensor> inputs2 = new Dictionary<string, Tensor>() { { "input.1", null } };
            inputs2["input.1"] = new Tensor(MainTexture2, channels: 3);
            // Execute neural network with the provided input
            engine.Execute(inputs2);


            // Determine the key point locations
            ProcessOutput(engine.PeekOutput(Pose2DModel.outputs[0]), 2);
            // Update the positions for the key point GameObjects
            UpdateKeyPointPositions2();

            // Release GPU resources allocated for the Tensor
            inputs2["input.1"].Dispose();
            inputs2.Clear();

            // Remove the processedImage variable

            // Lifting2();
        }


    }
    // Update is called once per frame
    void Update()
    {
        if (useWebcam)
        {
            // Copy webcamTexture to videoTexture
            Graphics.Blit(webcamTexture, videoTexture);
        }

        Dictionary<string, Tensor> inputs = new Dictionary<string, Tensor>() { { "input.1", null } };
        inputs["input.1"] = new Tensor(MainTexture, channels: 3);
        // Execute neural network with the provided input
        engine.Execute(inputs);


        // Determine the key point locations
        ProcessOutput(engine.PeekOutput(Pose2DModel.outputs[0]),1);
        // Update the positions for the key point GameObjects
        UpdateKeyPointPositions();

        // Release GPU resources allocated for the Tensor
        inputs["input.1"].Dispose();
        inputs.Clear();
        // Remove the processedImage variable
        //Lifting();

    }
    private void CameraSet1()
    {
        GameObject go = new GameObject("MainTextureCamera", typeof(Camera));

        go.transform.parent = VideoScreen.transform;
        go.transform.localScale = new Vector3(1.0f, -1.0f, 1.0f);
        go.transform.localPosition = new Vector3(0.0f, 0.0f, -2.0f);
        go.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        go.layer = _layer;

        var camera = go.GetComponent<Camera>();
        camera.orthographic = true;
        camera.orthographicSize = 360f;
        camera.depth = -5;
        camera.depthTextureMode = 0;
        camera.clearFlags = CameraClearFlags.Color;
        camera.backgroundColor = Color.black;
        camera.cullingMask = _layer;
        camera.useOcclusionCulling = false;
        camera.nearClipPlane = 1.0f;
        camera.farClipPlane = 5.0f;
        camera.allowMSAA = false;
        camera.allowHDR = false;
        MainTexture = new RenderTexture(imageWidth, imageHeight, 24, RenderTextureFormat.ARGBHalf);
        camera.targetTexture = MainTexture;
    }
    private void CameraSet2()
    {
        
        GameObject go = new GameObject("MainTextureCamera", typeof(Camera));

        go.transform.parent = VideoScreen.transform;
        go.transform.localScale = new Vector3(1.0f, -1.0f, 1.0f);
        go.transform.localPosition = new Vector3(0.38f, 0.0f, -2.0f);
        go.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        go.layer = _layer;

        var camera = go.GetComponent<Camera>();
        camera.orthographic = true;
        camera.orthographicSize = webcamHeight / 2;
        camera.depth = -5;
        camera.depthTextureMode = 0;
        camera.clearFlags = CameraClearFlags.Color;
        camera.backgroundColor = Color.black;
        camera.cullingMask = _layer;
        camera.useOcclusionCulling = false;
        camera.nearClipPlane = 1.0f;
        camera.farClipPlane = 5.0f;
        camera.allowMSAA = false;
        camera.allowHDR = false;
        MainTexture = new RenderTexture(imageWidth, imageHeight, 24, RenderTextureFormat.ARGBHalf);
        camera.targetTexture = MainTexture;

        GameObject go2 = new GameObject("MainTextureCamera2", typeof(Camera));

        go2.transform.parent = VideoScreen.transform;
        go2.transform.localScale = new Vector3(1.0f, -1.0f, 1.0f);
        go2.transform.localPosition = new Vector3(-0.38f, 0.0f, -2.0f);
        go2.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        go2.layer = _layer;

        var camera2 = go2.GetComponent<Camera>();
        camera2.orthographic = true;
        camera2.orthographicSize = webcamHeight / 2;
        camera2.depth = -5;
        camera2.depthTextureMode = 0;
        camera2.clearFlags = CameraClearFlags.Color;
        camera2.backgroundColor = Color.black;
        camera2.cullingMask = _layer;
        camera2.useOcclusionCulling = false;
        camera2.nearClipPlane = 1.0f;
        camera2.farClipPlane = 5.0f;
        camera2.allowMSAA = false;
        camera2.allowHDR = false;
        MainTexture2 = new RenderTexture(imageWidth, imageHeight, 24, RenderTextureFormat.ARGBHalf);
        camera2.targetTexture = MainTexture2;
        
    }

    private void ProcessOutput(Tensor heatmaps, int order)
    {
        // Calculate the stride used to scale down the inputImage
        float stride = 1920/224;

        // The smallest dimension of the videoTexture
        int minDimension = Mathf.Min(videoTexture.width, videoTexture.height);
        // The largest dimension of the videoTexture
        int maxDimension = Mathf.Max(videoTexture.width, videoTexture.height);

        // The value used to scale the key point locations up to the source resolution
        float scale = (float)minDimension / (float)Mathf.Min(imageWidth, imageHeight);
        // The value used to compensate for resizing the source image to a square aspect ratio
        float unsqueezeScale = (float)maxDimension / (float)minDimension;
        float[] what = heatmaps.data.Download(heatmaps.shape);
        // Iterate through heatmaps
        for (int k = 0; k < numKeypoints; k++)
        {
            // Get the location of the current key point and its associated confidence value
            var locationInfo = LocateKeyPointIndex(heatmaps, k);

            // The (x, y) coordinates containing the confidence value in the current heatmap
            var coords = locationInfo.Item1;
            
            // The accompanying offset vector for the current coords
            // The associated confidence value
            var confidenceValue = locationInfo.Item2;
            // Calcluate the X-axis position
            // Scale the X coordinate up to the inputImage resolution
            // Add the offset vector to refine the key point location
            // Scale the position up to the videoTexture resolution
            // Compensate for any change in aspect ratio
            //float xPos = (coords[0] * stride) * scale;
            float xPos = coords[0] * 27.889675f - 514;
            // Calculate the Y-axis position
            // Scale the Y coordinate up to the inputImage resolution and subtract it from the imageHeight
            // Add the offset vector to refine the key point location
            // Scale the position up to the videoTexture resolution
            //float yPos = (imageHeight - (coords[1] * stride)) * scale;
            float yPos = (1070.96352f - (coords[1] * 22.31174f)) + 4;
            //Debug.Log($"{coords[0]}, {coords[1]},{xPos}, {yPos}");

            /*
            if (videoTexture.width > videoTexture.height)
            {
                xPos *= unsqueezeScale;
            }
            else
            {
                yPos *= unsqueezeScale;
            }
            

            // Flip the x position if using a webcam
            */
            if (useWebcam)
            {
                //xPos = videoTexture.width - xPos;
                xPos = 1784.939f - xPos;
            }
            
            // Update the estimated key point location in the source image
            if (order == 1)
            {
                keypointLocations[k] = new float[] { xPos, yPos, confidenceValue };
            }
                    
            else if (order == 2)
            {
                keypointLocations2[k] = new float[] { xPos, yPos, confidenceValue };
            }
                           
        }
        heatmaps.Dispose();
    }

    /// <summary>
    /// Find the heatmap index that contains the highest confidence value and the associated offset vector

    private (float[], float) LocateKeyPointIndex(Tensor heatmaps, int keypointIndex)
    {
        // Stores the highest confidence value found in the current heatmap
        float maxConfidence = 0f;

        // The (x, y) coordinates containing the confidence value in the current heatmap
        float[] coords = new float[2];
        // The accompanying offset vector for the current coords


        // Iterate through heatmap columns
        for (int y = 0; y < heatmaps.height; y++)
        {
            // Iterate through column rows
            for (int x = 0; x < heatmaps.width; x++)
            {
                if (heatmaps[0, y, x, keypointIndex] > maxConfidence)
                {
                    //Debug.Log("x: " + x + "y: " + y);
                    // Update the highest confidence for the current key point
                    maxConfidence = heatmaps[0, y, x, keypointIndex];

                    // Update the estimated key point coordinates
                    coords = new float[] { x, y };
                }
            }
        
        }
        return (coords, maxConfidence);
    }
    /// <summary>
    /// Update the positions for the key point GameObjects
    /// </summary>
    private void UpdateKeyPointPositions()
    {
        // Iterate through the key points
        for (int k = 0; k < numKeypoints; k++)
        {
            // Check if the current confidence value meets the confidence threshold
            if (keypointLocations[k][2] >= minConfidence / 100f)
            {
                // Activate the current key point GameObject
                keypoints[k].SetActive(true);
                //keypoints[k].activeInHierarchy
            }
            else
            {
                // Deactivate the current key point GameObject
                keypoints[k].SetActive(false);
            }

            // Create a new position Vector3
            // Set the z value to -1f to place it in front of the video screen
            //Vector3 newPos = new Vector3(keypointLocations[k][0]-distance, keypointLocations[k][1], -1f);
            Vector3 newPos = new Vector3(keypointLocations[k][0], keypointLocations[k][1], -1f);
            // Update the current key point location
            keypoints[k].transform.position = newPos;
        }
        //Debug.Log(keypoints[0].transform.position);
    }
    private void UpdateKeyPointPositions2()
    {
        // Iterate through the key points
        for (int k = 0; k < numKeypoints; k++)
        {
            // Check if the current confidence value meets the confidence threshold
            if (keypointLocations2[k][2] >= minConfidence / 100f)
            {
                // Activate the current key point GameObject
                keypoints2[k].SetActive(true);
                //keypoints[k].activeInHierarchy
            }
            else
            {
                // Deactivate the current key point GameObject
                keypoints2[k].SetActive(false);
            }

            // Create a new position Vector3
            // Set the z value to -1f to place it in front of the video screen
            Vector3 newPos = new Vector3(keypointLocations2[k][0]+distance, keypointLocations2[k][1], -1f);

            // Update the current key point location
            keypoints2[k].transform.position = newPos;
        }
    }

    List<float[]> PoseData_X = new List<float[]>(Frames);
    List<float[]> PoseData_Y = new List<float[]>(Frames);
    List<float[]> PoseData_X2 = new List<float[]>(Frames);
    List<float[]> PoseData_Y2 = new List<float[]>(Frames);
    TensorShape input = new TensorShape(1, 17, 2, Frames);
    float[][] ThreeDpos = new float[17][];
    float[][] ThreeDpos2 = new float[17][];
    private void Lifting()
    {
        Tensor InputTensor = new Tensor(input);
        float[][] CalKeypoint = new float[17][];
        CalKeypoint[0] = new float[] { (keypointLocations[11][0] + keypointLocations[12][0]) / 2, (keypointLocations[11][1] + keypointLocations[12][1]) / 2 };
        CalKeypoint[8] = new float[] { (keypointLocations[5][0] + keypointLocations[6][0]) / 2, (keypointLocations[5][1] + keypointLocations[6][1]) / 2 };

        CalKeypoint[10] = new float[] { (keypointLocations[0][0] * 2 - CalKeypoint[8][0]), (keypointLocations[0][1] * 2 - CalKeypoint[8][1]) };
        CalKeypoint[7] = new float[] { (CalKeypoint[0][0] + CalKeypoint[8][0]) / 2, (CalKeypoint[0][1] + CalKeypoint[8][1]) / 2 };
        CalKeypoint[1] = new float[] { keypointLocations[12][0], keypointLocations[12][1] };
        CalKeypoint[2] = new float[] { keypointLocations[14][0], keypointLocations[14][1] };
        CalKeypoint[3] = new float[] { keypointLocations[16][0], keypointLocations[16][1] };
        CalKeypoint[4] = new float[] { keypointLocations[11][0], keypointLocations[11][1] };
        CalKeypoint[5] = new float[] { keypointLocations[13][0], keypointLocations[13][1] };
        CalKeypoint[6] = new float[] { keypointLocations[15][0], keypointLocations[15][1] };
        CalKeypoint[9] = new float[] { keypointLocations[0][0], keypointLocations[0][1] };
        CalKeypoint[11] = new float[] { keypointLocations[5][0], keypointLocations[5][1] };
        CalKeypoint[12] = new float[] { keypointLocations[7][0], keypointLocations[7][1] };
        CalKeypoint[13] = new float[] { keypointLocations[9][0], keypointLocations[9][1] };
        CalKeypoint[14] = new float[] { keypointLocations[6][0], keypointLocations[6][1] };
        CalKeypoint[15] = new float[] { keypointLocations[8][0], keypointLocations[8][1] };
        CalKeypoint[16] = new float[] { keypointLocations[10][0], keypointLocations[10][1] };
        float[] InputKey_X = new float[17];
        float[] InputKey_Y = new float[17];
        for (int i = 0; i < 17; i++)
        {
            InputKey_X[i] = CalKeypoint[i][0];
            InputKey_Y[i] = CalKeypoint[i][1];
        }
        PoseData_X.RemoveAt(0);
        PoseData_X.Add(InputKey_X);
        PoseData_Y.RemoveAt(0);
        PoseData_Y.Add(InputKey_Y);
        //Debug.Log(PoseData.Count);
        //Debug.Log("Error1");
        for (int i = 0; i < InputTensor.channels; i++)
        {
            for (int j = 0; j < InputTensor.height; j++)
            {
                InputTensor[0, j, 0, i] = PoseData_X[i][j];
                InputTensor[0, j, 1, i] = PoseData_Y[i][j];
            }
        }
        Dictionary<string, Tensor> inputs = new Dictionary<string, Tensor>() { { "input", null } };
        inputs["input"] = InputTensor;
        engine2.Execute(inputs);
        Tensor output = engine2.PeekOutput(LiftModel.outputs[0]);


        for (int i = 0; i < output.height; i++)
        {
            //Debug.Log(output.channels);
            float x, y, z;
            x = output[0, i, 0, 0];
            y = output[0, i, 1, 0];
            z = -output[0, i, 2, 0];
            //Debug.Log(x + y + z);
            ThreeDpos[i] = new float[] { x, y, z };
            //Debug.Log("Pose " + i + ": " + x + y + z);
        }

        for (int i = 0; i < output.height; i++)
        {
            Vector3 OutputPos = new Vector3(ThreeDpos[i][0]-distance, ThreeDpos[i][1], ThreeDpos[i][2]);
            //PoseEstimator[i].transform.position = OutputPos;
            PoseEstimator[i].transform.localPosition = OutputPos;
            //PoseEstimator[i].transform.rotation = Quaternion.identity;
        }
        inputs["input"].Dispose();
        inputs.Clear();
        InputTensor.Dispose();
        output.Dispose();
    }

    private void Lifting2()
    {
        Tensor InputTensor = new Tensor(input);
        float[][] CalKeypoint = new float[17][];

        CalKeypoint[0] = new float[] { (keypointLocations2[11][0] + keypointLocations2[12][0]) / 2, (keypointLocations2[11][1] + keypointLocations2[12][1]) / 2 };
        CalKeypoint[8] = new float[] { (keypointLocations2[5][0] + keypointLocations2[6][0]) / 2, (keypointLocations2[5][1] + keypointLocations2[6][1]) / 2 };

        CalKeypoint[10] = new float[] { (keypointLocations2[0][0] * 2 - CalKeypoint[8][0]), (keypointLocations2[0][1] * 2 - CalKeypoint[8][1]) };
        CalKeypoint[7] = new float[] { (CalKeypoint[0][0] + CalKeypoint[8][0]) / 2, (CalKeypoint[0][1] + CalKeypoint[8][1]) / 2 };
        CalKeypoint[1] = new float[] { keypointLocations2[12][0], keypointLocations2[12][1] };
        CalKeypoint[2] = new float[] { keypointLocations2[14][0], keypointLocations2[14][1] };
        CalKeypoint[3] = new float[] { keypointLocations2[16][0], keypointLocations2[16][1] };
        CalKeypoint[4] = new float[] { keypointLocations2[11][0], keypointLocations2[11][1] };
        CalKeypoint[5] = new float[] { keypointLocations2[13][0], keypointLocations2[13][1] };
        CalKeypoint[6] = new float[] { keypointLocations2[15][0], keypointLocations2[15][1] };
        CalKeypoint[9] = new float[] { keypointLocations2[0][0], keypointLocations2[0][1] };
        CalKeypoint[11] = new float[] { keypointLocations2[5][0], keypointLocations2[5][1] };
        CalKeypoint[12] = new float[] { keypointLocations2[7][0], keypointLocations2[7][1] };
        CalKeypoint[13] = new float[] { keypointLocations2[9][0], keypointLocations2[9][1] };
        CalKeypoint[14] = new float[] { keypointLocations2[6][0], keypointLocations2[6][1] };
        CalKeypoint[15] = new float[] { keypointLocations2[8][0], keypointLocations2[8][1] };
        CalKeypoint[16] = new float[] { keypointLocations2[10][0], keypointLocations2[10][1] };
        float[] InputKey_X = new float[17];
        float[] InputKey_Y = new float[17];
        for (int i = 0; i < 17; i++)
        {
            InputKey_X[i] = CalKeypoint[i][0];
            InputKey_Y[i] = CalKeypoint[i][1];
        }
        PoseData_X2.RemoveAt(0);
        PoseData_X2.Add(InputKey_X);
        PoseData_Y2.RemoveAt(0);
        PoseData_Y2.Add(InputKey_Y);
        //Debug.Log(PoseData.Count);
        //Debug.Log("Error1");
        for (int i = 0; i < InputTensor.channels; i++)
        {
            for (int j = 0; j < InputTensor.height; j++)
            {
                InputTensor[0, j, 0, i] = PoseData_X2[i][j];
                InputTensor[0, j, 1, i] = PoseData_Y2[i][j];
            }
        }
        Dictionary<string, Tensor> inputs = new Dictionary<string, Tensor>() { { "input", null } };
        inputs["input"] = InputTensor;
        engine2.Execute(inputs);
        Tensor output = engine2.PeekOutput(LiftModel.outputs[0]);


        for (int i = 0; i < output.height; i++)
        {
            //Debug.Log(output.channels);
            float x, y, z;
            x = output[0, i, 0, 0];
            y = output[0, i, 1, 0];
            z = -output[0, i, 2, 0];
            //Debug.Log(x + y + z);
            ThreeDpos[i] = new float[] { x, y, z };
            //Debug.Log("Pose " + i + ": " + x + y + z);
        }

        for (int i = 0; i < output.height; i++)
        {
            Vector3 OutputPos = new Vector3(ThreeDpos[i][0]+distance, ThreeDpos[i][1], ThreeDpos[i][2]);
            //PoseEstimator[i].transform.position = OutputPos;
            PoseEstimator2[i].transform.localPosition = OutputPos;
            //PoseEstimator[i].transform.rotation = Quaternion.identity;
        }
        inputs["input"].Dispose();
        inputs.Clear();
        InputTensor.Dispose();
        output.Dispose();
    }

}