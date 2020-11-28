using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaurd : MonoBehaviour
{
    [SerializeField]private Transform m_pathHolder;
    [SerializeField]private float m_moveSpeed;
    [SerializeField]private float m_waitTime;
    [SerializeField]private float m_turnSpeed;
    [SerializeField]private float m_proximityRadius;
    

    private Vector3 m_currentPosition;
    private Vector3 m_nextPosition;
    private Coroutine m_patrolCoroutine;
    private Coroutine m_turnToPlayerCoroutine;
    private bool m_playerDetected;

    [SerializeField]private Light m_spotLight;
    [SerializeField]private float m_viewDistance;
    private float m_viewAngle;
    private Color m_originalSpotlightColor;
    
    [SerializeField]private Transform player;
    [SerializeField]private LayerMask viewMask;

//`````````````````````````````````````````````````````````````````````````````````````````````````````
//`````````````````````````````````````````````````````````````````````````````````````````````````````

    private void Start() {

        Vector3[] _wayPoints = new Vector3[m_pathHolder.childCount];
        m_viewAngle = m_spotLight.spotAngle;
        m_originalSpotlightColor = m_spotLight.color;
        
        for(int i=0;i<_wayPoints.Length;i++){

            _wayPoints[i] = m_pathHolder.GetChild(i).position;
            _wayPoints[i] = new Vector3(_wayPoints[i].x,transform.position.y,_wayPoints[i].z);
        }

        m_patrolCoroutine = StartCoroutine(Patrol(_wayPoints));
    }

//`````````````````````````````````````````````````````````````````````````````````````````````````````
//`````````````````````````````````````````````````````````````````````````````````````````````````````

    private void Update() {

        if(!m_playerDetected){                                                                              //if player not detected
            if(CanSeePlayer() || PlayerInProximity()){                                                      //if player in proximity or line of sight
                m_playerDetected=true;
                m_spotLight.color = Color.red;
                StopCoroutine(m_patrolCoroutine);                                                           //stop patrol coroutine
                if(m_turnToPlayerCoroutine==null){                                                          //if turn towards coroutine already running
                    m_turnToPlayerCoroutine = StartCoroutine(TurnToFace(player.position));
                }
            }
            else{

                m_spotLight.color = m_originalSpotlightColor;
            }
        }        
    }

//`````````````````````````````````````````````````````````````````````````````````````````````````````
//`````````````````````````````````````````````````````````````````````````````````````````````````````

    private bool PlayerInProximity(){
         var _collidersInRange = Physics.OverlapSphere (transform.position, m_proximityRadius);            //define a proximity Sphere

         foreach(var item in _collidersInRange){                                                           
             if(item.tag=="Player"){                                                                       //if objects in sphere has tag player 
                                                                          
                 return true;
             }
         }
        return false;
    }

//`````````````````````````````````````````````````````````````````````````````````````````````````````
//`````````````````````````````````````````````````````````````````````````````````````````````````````

    private bool CanSeePlayer(){
        if(Vector3.Distance(transform.position,player.position)< m_viewDistance){                           //Is Player In View Range

            Vector3 _directionToPlayer = (player.position - transform.position).normalized;
            float _angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward,_directionToPlayer);

            if(_angleBetweenGuardAndPlayer < m_viewAngle/2f){                                               //Is Player In View Cone

                if(!Physics.Linecast(transform.position,player.position,viewMask)){                         //Is Player In Line Of Sight
                    return true;
                }
            }
        }
        return false;
    }

//`````````````````````````````````````````````````````````````````````````````````````````````````````
//`````````````````````````````````````````````````````````````````````````````````````````````````````

    private IEnumerator Patrol(Vector3[] _wayPoints){
        
        transform.position = _wayPoints[0];                                                                                         //set position to start of path
        int _targetWaypointIndex = 1;
        Vector3 _targetWaypointPosition = _wayPoints[_targetWaypointIndex];
        transform.LookAt(_targetWaypointPosition);
        
        while(true){
            transform.position = Vector3.MoveTowards(transform.position,_targetWaypointPosition,m_moveSpeed*Time.deltaTime);
            if(transform.position == _targetWaypointPosition){
                _targetWaypointIndex = (_targetWaypointIndex+1)%_wayPoints.Length;                                                  //once index reach to maximum it is set back to 0
                _targetWaypointPosition = _wayPoints[_targetWaypointIndex];
        
                yield return new WaitForSeconds(m_waitTime);                                                                        //gaurd wait before moving to next position
                yield return StartCoroutine(TurnToFace(_targetWaypointPosition));
            }
            
            yield return null;                                                                                                      //loop runs only once per frame
        }                                                                                                   
    }

//`````````````````````````````````````````````````````````````````````````````````````````````````````
//`````````````````````````````````````````````````````````````````````````````````````````````````````

    private IEnumerator TurnToFace(Vector3 _lookTarget){
        
        Vector3 _dirToLookTarget = (_lookTarget-transform.position).normalized;
        float _targetAngle = 90-Mathf.Atan2(_dirToLookTarget.z,_dirToLookTarget.x)*Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y,_targetAngle))>0.5f){
            float _angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y,_targetAngle,m_turnSpeed*Time.deltaTime);
            transform.eulerAngles = Vector3.up*_angle;
            yield return null;
            m_turnToPlayerCoroutine = null;
        }
    }

//`````````````````````````````````````````````````````````````````````````````````````````````````````
//`````````````````````````````````````````````````````````````````````````````````````````````````````

    private void OnDrawGizmos() {
        Vector3 _startPosition = m_pathHolder.GetChild(0).position;
        Vector3 _previousPosition = _startPosition;  
        foreach(Transform _waypoint in m_pathHolder){
            Gizmos.DrawSphere(_waypoint.position,1f);                                                   //draw sphere for each waypoint
            Gizmos.DrawLine(_previousPosition,_waypoint.position);                                      //connect all waypoint
            _previousPosition = _waypoint.position;
        }
        Gizmos.DrawLine(_previousPosition,_startPosition);                                              //join start point and end point

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * m_viewDistance);                         //ditection line range

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_proximityRadius);
    }
}
