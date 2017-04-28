using UnityEngine;

public class CameraControls : MonoBehaviour
{
	public float dragSpeed = 10;							//Camera drag speed;
	public Vector2 sizeX = new Vector2 (-15, -10);			//Default bound size;
	public Vector2 sizeY = new Vector2 (15, 10);

	public enum DragAxes									//Allowed drag axes;
	{
		XY,
		X,
		y
	}
	public DragAxes dragAxes = DragAxes.XY;	

	public enum CameraPosition
	{
		SaveCurrent,
		WithNextLevel
	}

	public CameraPosition cameraPosition = CameraPosition.SaveCurrent;
	
	public Bounds bounds;									//Camera bounds;

	public Vector3 defaultPosition;
	private Vector3 dragOrigin, touchPos, moveDir;
	private float mapX, mapY;
	private float minX, maxX, minY, maxY;
	private float vertExtent, horzExtent;

    Vector2?[] oldTouchPositions = {
        null,
        null
    };
    Vector2 oldTouchVector;
    float oldTouchDistance;


    void Awake()
	{
		gameObject.tag = "MainCamera"; //Assign default tag
	}

	void Start() {

		//Positioning the camera
		if(defaultPosition != Vector3.zero)
			transform.position = new Vector3(defaultPosition.x, defaultPosition.y, transform.position.z);

		//Calculating bound;
		mapX = bounds.size.x;
		mapY = bounds.size.y;

		bounds.SetMinMax(sizeX, sizeY);

		vertExtent = Camera.main.GetComponent<Camera>().orthographicSize;
		horzExtent = vertExtent * Screen.width / Screen.height;

		minX = horzExtent - (mapX / 2.0F - bounds.center.x);
		maxX = (mapX / 2.0F + bounds.center.x) - horzExtent;

		minY = vertExtent - (mapY / 2.0F - bounds.center.y);
		maxY = (mapY / 2.0F + bounds.center.y) - vertExtent;
	}
	
	void Update() 
	{
		DragCam();

		//Clamp camera movement with bound
		var v3 = transform.position;
		v3.x = Mathf.Clamp(v3.x, minX, maxX);
		v3.y = Mathf.Clamp(v3.y, minY, maxY);
		transform.position = v3;
	}


	//Camera movement
	void DragCam()
	{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
		if (Input.GetMouseButtonDown(0))
		{
			dragOrigin = Input.mousePosition;
			return;
		}
		if (!Input.GetMouseButton(0)) return;
			touchPos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);


		switch(dragAxes)
		{
		case DragAxes.XY:
			moveDir = new Vector3(touchPos.x, touchPos.y, 0);
			break;
		case DragAxes.X:
			moveDir = new Vector3(touchPos.x, 0, 0);
			break;
		case DragAxes.y:
			moveDir = new Vector3(0, touchPos.y, 0);
			break;
		default:
			break;
		}
		moveDir *= dragSpeed * Time.deltaTime;
		transform.position -= moveDir;
#endif

#if UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8
            if (Input.touchCount == 0)
            {
                oldTouchPositions[0] = null;
                oldTouchPositions[1] = null;
            }
            else if (Input.touchCount == 1)
            {
                if (oldTouchPositions[0] == null || oldTouchPositions[1] != null)
                {
                    oldTouchPositions[0] = Input.GetTouch(0).position;
                    oldTouchPositions[1] = null;
                }
                else
                {
                    Vector2 newTouchPosition = Input.GetTouch(0).position;

                    transform.position += transform.TransformDirection((Vector3)((oldTouchPositions[0] - newTouchPosition) * GetComponent<Camera>().orthographicSize / GetComponent<Camera>().pixelHeight * 2f));

                    oldTouchPositions[0] = newTouchPosition;
                }
            }
            else
            {
                if (oldTouchPositions[1] == null)
                {
                    oldTouchPositions[0] = Input.GetTouch(0).position;
                    oldTouchPositions[1] = Input.GetTouch(1).position;
                    oldTouchVector = (Vector2)(oldTouchPositions[0] - oldTouchPositions[1]);
                    oldTouchDistance = oldTouchVector.magnitude;
                }
                else
                {
                    Vector2 screen = new Vector2(GetComponent<Camera>().pixelWidth, GetComponent<Camera>().pixelHeight);

                    Vector2[] newTouchPositions = {
                        Input.GetTouch(0).position,
                        Input.GetTouch(1).position
                    };
                    Vector2 newTouchVector = newTouchPositions[0] - newTouchPositions[1];
                    float newTouchDistance = newTouchVector.magnitude;

                    transform.position += transform.TransformDirection((Vector3)((oldTouchPositions[0] + oldTouchPositions[1] - screen) * GetComponent<Camera>().orthographicSize / screen.y));
                    transform.localRotation *= Quaternion.Euler(new Vector3(0, 0, Mathf.Asin(Mathf.Clamp((oldTouchVector.y * newTouchVector.x - oldTouchVector.x * newTouchVector.y) / oldTouchDistance / newTouchDistance, -1f, 1f)) / 0.0174532924f));
                    GetComponent<Camera>().orthographicSize *= oldTouchDistance / newTouchDistance;
                    transform.position -= transform.TransformDirection((newTouchPositions[0] + newTouchPositions[1] - screen) * GetComponent<Camera>().orthographicSize / screen.y);

                    oldTouchPositions[0] = newTouchPositions[0];
                    oldTouchPositions[1] = newTouchPositions[1];
                    oldTouchVector = newTouchVector;
                    oldTouchDistance = newTouchDistance;
                }
            }
#endif
    }

	//Draw bounds in scene view
	void OnDrawGizmos()
	{
		bounds.SetMinMax(sizeX, sizeY);
		Gizmos.DrawWireCube(bounds.center, bounds.size);
	}
}
