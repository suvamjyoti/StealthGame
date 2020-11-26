using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaurd : MonoBehaviour
{
    [SerializeField]private Transform m_pathHolder;
    [SerializeField]private float m_moveSpeed;
    [SerializeField]private float m_waitTime;

    private Vector3 m_currentPosition;
    private Vector3 m_nextPosition;

//`````````````````````````````````````````````````````````````````````````````````````````````````````
//`````````````````````````````````````````````````````````````````````````````````````````````````````

    private void Start() {

        Vector3[] _wayPoints = new Vector3[m_pathHolder.childCount];

        for(int i=0;i<_wayPoints.Length;i++){

            _wayPoints[i] = m_pathHolder.GetChild(i).position;
        }

        StartCoroutine(Patrol(_wayPoints));
    }

//`````````````````````````````````````````````````````````````````````````````````````````````````````
//`````````````````````````````````````````````````````````````````````````````````````````````````````

    private IEnumerator Patrol(Vector3[] _wayPoints){
        
        transform.position = _wayPoints[0];                                                                                      //set position to start of path
        int _targetWaypointIndex = 1;
        Vector3 _targetWaypointPosition = _wayPoints[_targetWaypointIndex];
        
        while(true){

            transform.position = Vector3.MoveTowards(transform.position,_targetWaypointPosition,m_moveSpeed*Time.deltaTime);
            _targetWaypointIndex = (_targetWaypointIndex+1)%_wayPoints.Length;                                                  //once index reach to maximum it is set back to 0
            _targetWaypointPosition = _wayPoints[_targetWaypointIndex];
        
            yield return new WaitForSeconds(m_waitTime);                                                                        //gaurd wait before moving to next position
        }

        yield return null;                                                                                                      //loop runs only once per frame
    }

//`````````````````````````````````````````````````````````````````````````````````````````````````````
//`````````````````````````````````````````````````````````````````````````````````````````````````````

    private void OnDrawGizmos() {
        Vector3 _startPosition = m_pathHolder.GetChild(0).position;
        Vector3 _previousPosition = _startPosition;  
        foreach(Transform _waypoint in m_pathHolder){
            Gizmos.DrawSphere(_waypoint.position,0.3f);
            Gizmos.DrawLine(_previousPosition,_waypoint.position);
            _previousPosition = _waypoint.position;
        }
        Gizmos.DrawLine(_previousPosition,_startPosition);
    }
}
