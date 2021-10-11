using UnityEngine;

public class TCP2_Demo_Camera : MonoBehaviour
{
	public Transform Pivot;

	[Header("Orbit")]
	public float OrbitStrg = 3f;

	public float OrbitClamp = 50f;

	[Header("Panning")]
	public float PanStrgMin = 0.1f;

	public float PanStrgMax = 0.5f;

	[Header("Zooming")]
	public float ZoomStrg = 40f;

	public float ZoomClamp = 30f;

	public float ZoomDistMin = 1f;

	public float ZoomDistMax = 2f;

	[Header("Misc")]
	public float Decceleration = 8f;

	public RectTransform ignoreMouseRect;

	private Vector3 mouseDelta;

	private Vector3 orbitAcceleration;

	private Vector3 panAcceleration;

	private Vector3 moveAcceleration;

	private float zoomAcceleration;

	private float zoomDistance;

	private const float XMax = 60f;

	private const float XMin = 300f;

	private Vector3 mResetCamPos;

	private Vector3 mResetPivotPos;

	private Vector3 mResetCamRot;

	private Vector3 mResetPivotRot;

	private bool leftMouseHeld;

	private bool rightMouseHeld;

	private bool middleMouseHeld;

	private void Awake()
	{
		mResetCamPos = base.transform.position;
		mResetCamRot = base.transform.eulerAngles;
		mResetPivotPos = Pivot.position;
		mResetPivotRot = Pivot.eulerAngles;
	}

	private void OnEnable()
	{
		mouseDelta = UnityEngine.Input.mousePosition;
	}

	private void Update()
	{
		mouseDelta = UnityEngine.Input.mousePosition - mouseDelta;
		mouseDelta.x = Mathf.Clamp(mouseDelta.x, -150f, 150f);
		mouseDelta.y = Mathf.Clamp(mouseDelta.y, -150f, 150f);
		bool flag = ignoreMouseRect.rect.Contains(UnityEngine.Input.mousePosition);
		if (Input.GetMouseButtonDown(0))
		{
			leftMouseHeld = !flag;
		}
		else if (Input.GetMouseButtonUp(0) || !Input.GetMouseButton(0))
		{
			leftMouseHeld = false;
		}
		if (Input.GetMouseButtonDown(1))
		{
			rightMouseHeld = !flag;
		}
		else if (Input.GetMouseButtonUp(1) || !Input.GetMouseButton(1))
		{
			rightMouseHeld = false;
		}
		if (Input.GetMouseButtonDown(2))
		{
			middleMouseHeld = !flag;
		}
		else if (Input.GetMouseButtonUp(2) || !Input.GetMouseButton(2))
		{
			middleMouseHeld = false;
		}
		if (leftMouseHeld)
		{
			orbitAcceleration.x += Mathf.Clamp(mouseDelta.x * OrbitStrg, 0f - OrbitClamp, OrbitClamp);
			orbitAcceleration.y += Mathf.Clamp((0f - mouseDelta.y) * OrbitStrg, 0f - OrbitClamp, OrbitClamp);
		}
		else if (middleMouseHeld || rightMouseHeld)
		{
			float num = Mathf.Lerp(PanStrgMin, PanStrgMax, Mathf.Clamp01((zoomDistance - ZoomDistMin) / (ZoomDistMax - ZoomDistMin)));
			panAcceleration.x = (0f - mouseDelta.x) * num;
			panAcceleration.y = (0f - mouseDelta.y) * num;
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.R))
		{
			ResetView();
		}
		Vector3 localEulerAngles = base.transform.localEulerAngles;
		if (localEulerAngles.x < 180f && localEulerAngles.x >= 60f && orbitAcceleration.y > 0f)
		{
			orbitAcceleration.y = 0f;
		}
		if (localEulerAngles.x > 180f && localEulerAngles.x <= 300f && orbitAcceleration.y < 0f)
		{
			orbitAcceleration.y = 0f;
		}
		base.transform.RotateAround(Pivot.position, base.transform.right, orbitAcceleration.y * Time.deltaTime);
		base.transform.RotateAround(Pivot.position, Vector3.up, orbitAcceleration.x * Time.deltaTime);
		Pivot.Translate(panAcceleration * Time.deltaTime, base.transform);
		base.transform.Translate(panAcceleration * Time.deltaTime, base.transform);
		float axis = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
		zoomAcceleration += axis * ZoomStrg;
		zoomAcceleration = Mathf.Clamp(zoomAcceleration, 0f - ZoomClamp, ZoomClamp);
		zoomDistance = Vector3.Distance(base.transform.position, Pivot.position);
		if ((zoomDistance >= ZoomDistMin && zoomAcceleration > 0f) || (zoomDistance <= ZoomDistMax && zoomAcceleration < 0f))
		{
			base.transform.Translate(Vector3.forward * zoomAcceleration * Time.deltaTime, Space.Self);
		}
		orbitAcceleration = Vector3.Lerp(orbitAcceleration, Vector3.zero, Decceleration * Time.deltaTime);
		panAcceleration = Vector3.Lerp(panAcceleration, Vector3.zero, Decceleration * Time.deltaTime);
		zoomAcceleration = Mathf.Lerp(zoomAcceleration, 0f, Decceleration * Time.deltaTime);
		moveAcceleration = Vector3.Lerp(moveAcceleration, Vector3.zero, Decceleration * Time.deltaTime);
		mouseDelta = UnityEngine.Input.mousePosition;
	}

	public void ResetView()
	{
		moveAcceleration = Vector3.zero;
		orbitAcceleration = Vector3.zero;
		panAcceleration = Vector3.zero;
		zoomAcceleration = 0f;
		base.transform.position = mResetCamPos;
		base.transform.eulerAngles = mResetCamRot;
		Pivot.position = mResetPivotPos;
		Pivot.eulerAngles = mResetPivotRot;
	}
}
