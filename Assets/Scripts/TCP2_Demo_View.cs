using UnityEngine;

public class TCP2_Demo_View : MonoBehaviour
{
	[Header("Orbit")]
	public float OrbitStrg = 3f;

	public float OrbitClamp = 50f;

	[Header("Panning")]
	public float PanStrg = 0.1f;

	public float PanClamp = 2f;

	[Header("Zooming")]
	public float ZoomStrg = 40f;

	public float ZoomClamp = 30f;

	[Header("Misc")]
	public float Decceleration = 8f;

	public Transform CharacterTransform;

	private Vector3 mouseDelta;

	private Vector3 orbitAcceleration;

	private Vector3 panAcceleration;

	private Vector3 moveAcceleration;

	private float zoomAcceleration;

	private const float XMax = 60f;

	private const float XMin = 300f;

	private Vector3 mResetCamPos;

	private Vector3 mResetCamRot;

	private bool mMouseDown;

	private void Awake()
	{
		mResetCamPos = Camera.main.transform.position;
		mResetCamRot = Camera.main.transform.eulerAngles;
	}

	private void OnEnable()
	{
		mouseDelta = UnityEngine.Input.mousePosition;
	}

	private void Update()
	{
		mouseDelta = UnityEngine.Input.mousePosition - mouseDelta;
		if (!mMouseDown)
		{
			mMouseDown = ((Input.GetMouseButtonDown(0) && !new Rect(0f, 65f, 230f, 260f).Contains(UnityEngine.Input.mousePosition)) ? true : false);
		}
		else
		{
			mMouseDown = ((!Input.GetMouseButtonUp(0)) ? true : false);
		}
		if (mMouseDown)
		{
			orbitAcceleration.y -= Mathf.Clamp((0f - mouseDelta.x) * OrbitStrg, 0f - OrbitClamp, OrbitClamp);
		}
		else if (Input.GetMouseButton(2) || Input.GetMouseButton(1))
		{
			panAcceleration.y += Mathf.Clamp((0f - mouseDelta.y) * PanStrg, 0f - PanClamp, PanClamp);
		}
		orbitAcceleration.y += (UnityEngine.Input.GetKey(KeyCode.LeftArrow) ? 15 : (UnityEngine.Input.GetKey(KeyCode.RightArrow) ? (-15) : 0));
		zoomAcceleration += (UnityEngine.Input.GetKey(KeyCode.UpArrow) ? 1 : (UnityEngine.Input.GetKey(KeyCode.DownArrow) ? (-1) : 0));
		if (UnityEngine.Input.GetKeyDown(KeyCode.R))
		{
			ResetView();
		}
		Vector3 localEulerAngles = Camera.main.transform.localEulerAngles;
		if (localEulerAngles.x < 180f && localEulerAngles.x >= 60f && orbitAcceleration.y > 0f)
		{
			orbitAcceleration.y = 0f;
		}
		if (localEulerAngles.x > 180f && localEulerAngles.x <= 300f && orbitAcceleration.y < 0f)
		{
			orbitAcceleration.y = 0f;
		}
		CharacterTransform.Rotate(-orbitAcceleration * Time.deltaTime, Space.World);
		Camera.main.transform.Translate(panAcceleration * Time.deltaTime, Space.World);
		float axis = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
		zoomAcceleration += axis * ZoomStrg;
		zoomAcceleration = Mathf.Clamp(zoomAcceleration, 0f - ZoomClamp, ZoomClamp);
		Camera.main.transform.Translate(Vector3.forward * zoomAcceleration * Time.deltaTime, Space.World);
		if (Camera.main.transform.position.y > 1.65f)
		{
			Vector3 position = Camera.main.transform.position;
			position.y = 1.65f;
			Camera.main.transform.position = position;
		}
		else if (Camera.main.transform.position.y < 0.3f)
		{
			Vector3 position2 = Camera.main.transform.position;
			position2.y = 0.3f;
			Camera.main.transform.position = position2;
		}
		if (Camera.main.transform.position.z < -1.8f)
		{
			Vector3 position3 = Camera.main.transform.position;
			position3.z = -1.8f;
			Camera.main.transform.position = position3;
		}
		else if (Camera.main.transform.position.z > -0.6f)
		{
			Vector3 position4 = Camera.main.transform.position;
			position4.z = -0.6f;
			Camera.main.transform.position = position4;
		}
		orbitAcceleration = Vector3.Lerp(orbitAcceleration, Vector3.zero, Decceleration * Time.deltaTime);
		panAcceleration = Vector3.Lerp(panAcceleration, Vector3.zero, Decceleration * Time.deltaTime);
		zoomAcceleration = Mathf.Lerp(zoomAcceleration, 0f, Decceleration * Time.deltaTime);
		moveAcceleration = Vector3.Lerp(moveAcceleration, Vector3.zero, Decceleration * Time.deltaTime);
		mouseDelta = UnityEngine.Input.mousePosition;
	}

	public void ResetView()
	{
		Camera.main.transform.position = mResetCamPos;
		Camera.main.transform.eulerAngles = mResetCamRot;
	}
}
