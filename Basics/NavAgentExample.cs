using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentExample : MonoBehaviour
{
	public AIWaypointNetwork WaypointNetwork = null;
	public int CurrentIndex = 0;
	public bool HasPath = false;
	public bool PathPending = false;
	public bool PathStale = false;
	public NavMeshPathStatus PathStatus = NavMeshPathStatus.PathInvalid;
	public AnimationCurve JumpCurve = new AnimationCurve();

	private NavMeshAgent _navAgent = null;
	private Animator _animator = null;
	private bool _jumping = false;

	// Use this for initialization
	void Start()
	{
		_navAgent = GetComponent<NavMeshAgent>();
		_animator = GetComponent<Animator>();

		if (WaypointNetwork == null) return;

		SetNextDestination(false);
	}

	private void SetNextDestination(bool increment)
	{
		if (!WaypointNetwork) return;

		int incStep = increment ? 1 : 0;

		Transform nextWaypointTransform = null;

		int nextWaypoint = ((CurrentIndex + incStep) >= WaypointNetwork.Waypoints.Count) ? 0 : CurrentIndex + incStep;

		nextWaypointTransform = WaypointNetwork.Waypoints[nextWaypoint];

		if (nextWaypointTransform != null)
		{
			CurrentIndex = nextWaypoint;
			_navAgent.destination = nextWaypointTransform.position;
			return;
		}

		CurrentIndex = nextWaypoint;

	}

	// Update is called once per frame
	void Update()
	{
		HasPath = _navAgent.hasPath;
		PathPending = _navAgent.pathPending;
		PathStale = _navAgent.isPathStale;
		PathStatus = _navAgent.pathStatus;

		if (_navAgent.isOnOffMeshLink && !_jumping)
		{
			StartCoroutine(Jump(1.0f));
			return;
		}

		if ((!HasPath && !PathPending) || PathStatus == NavMeshPathStatus.PathInvalid)
		{
			SetNextDestination(true);
		}
		else if (PathStale)
		{
			SetNextDestination(false);
		}
	}

	IEnumerator Jump(float duration)
	{
		_jumping = true;
		_animator.SetTrigger("Jump");
		

		OffMeshLinkData data = _navAgent.currentOffMeshLinkData;
		Vector3 startPos = _navAgent.transform.position;
		Vector3 endPos = data.endPos + (_navAgent.baseOffset * Vector3.up);

		_navAgent.transform.rotation = Quaternion.LookRotation(data.endPos);

		float time = 0.0f;

		while (time <= duration) {
			float t = time / duration;
			_navAgent.transform.position = Vector3.Lerp(startPos, endPos, t) + (JumpCurve.Evaluate(t) * Vector3.up);
			time += Time.deltaTime;
			yield return null;
		}
		_navAgent.CompleteOffMeshLink();
		_jumping = false;
		_navAgent.isStopped = true;
		yield return new WaitForSeconds(1.0f);
		_navAgent.isStopped = false;
	}
}