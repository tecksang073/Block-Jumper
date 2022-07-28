using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    enum PlyerState
    {
        Stading, Jumping, falling
    }
    PlyerState currentState;

    public Transform playerParentTransform;

  
    [Space]
    public GameObject fx_Shoot_1;
    public GameObject fx_Shoot_2;
    public GameObject fx_Jump;
    public GameObject fx_Land;
    public GameObject fx_StepDestroy;
    public GameObject fx_Dead;
    [Space]
    public int jumpSpeed;
    public int shootSpeed;

    Rigidbody2D rb;
    TrailRenderer trailRenderer;
    BoxCollider2D bc2D;

    float previousPosXofParent;

    float hueValue;

    bool isDead = false;

    float LeftEnd;
    float RightEnd;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bc2D = GetComponent<BoxCollider2D>();
        trailRenderer = GetComponent<TrailRenderer>();

        LeftEnd = GameObject.Find("GameManager").GetComponent<GetDisplayBound>().Left;
        RightEnd = GameObject.Find("GameManager").GetComponent<GetDisplayBound>().Right;
    }


    void Start()
    {
        InitPlayer();
        hueValue = Random.Range(0, 10) / 10.0f;
        ChangeBackgroundColor();
    }

    void InitPlayer()
    {
        trailRenderer.startWidth = transform.localScale.x;
        trailRenderer.endWidth = transform.localScale.x;

        currentState = PlyerState.falling;
        rb.velocity = new Vector2(0, 0);
    }

    void Update()
    {
        GetInput();
        BounceAtWall();
        DeadCheck();

        previousPosXofParent = transform.parent.transform.position.x;

        if (currentState == PlyerState.Jumping)
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * rb.velocity.x * (-30));
        }
    }


    void GetInput()
    {
        if (Input.GetMouseButtonDown(0) && Time.timeScale != 0 /*!EventSystem.current.IsPointerOverGameObject()*/ && !IsPointerOverUIObject())
        {
            if (currentState == PlyerState.Stading)
            {
                Jump();
            }
            else if (currentState == PlyerState.Jumping)
            {
                StartCoroutine(Shoot());
            }
        }
    }


    void BounceAtWall()
    {
        if (rb.position.x < LeftEnd)
        {
            rb.position = new Vector2(LeftEnd, rb.position.y);
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
        }

        if (rb.position.x > RightEnd)
        {
            rb.position = new Vector2(RightEnd, rb.position.y);
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
        }
    }


    void Jump()
    {
        JumpEffect();

        float parentVelocity = (transform.parent.transform.position.x - previousPosXofParent) / Time.deltaTime;
        rb.velocity = new Vector2(parentVelocity, jumpSpeed);

        currentState = PlyerState.Jumping;

        bc2D.enabled = false;

        transform.SetParent(playerParentTransform);
    }


    void JumpEffect()
    {
        GameObject effectObj = Instantiate(fx_Jump, transform.position, Quaternion.identity);
        Destroy(effectObj, 0.5f);
    }


    void DeadCheck()
    {
        if (isDead == false && Camera.main.transform.position.y - transform.position.y > 10)
        {
            isDead = true;
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;

            Destroy(Instantiate(fx_Dead, transform.position, Quaternion.identity), 1.0f);


            GameObject.Find("GameManager").GetComponent<GameManager>().GameOver();
        }
    }


    IEnumerator Shoot()
    {

        transform.rotation = Quaternion.identity;

        currentState = PlyerState.falling;

        ShootEffect1();

        rb.isKinematic = true;
        rb.velocity = new Vector2(0, 0);

        yield return new WaitForSeconds(0.5f);

        ShootEffect2();
        ChangeBackgroundColor();

        rb.isKinematic = false;
        rb.velocity = new Vector2(0, -shootSpeed);

        bc2D.enabled = true;

        yield break;
    }

    void ShootEffect1()
    {
        GameObject tempObj = Instantiate(fx_Shoot_1, transform.position, Quaternion.identity);
        tempObj.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.HSVToRGB(hueValue, 0.6f, 0.7f);
        Destroy(tempObj, 1.0f);
    }

    void ShootEffect2()
    {
        GameObject EffectObj = Instantiate(fx_Shoot_2, transform.position, Quaternion.identity);
        Destroy(EffectObj, 0.5f);
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Step" && currentState == PlyerState.falling && rb.velocity == Vector2.zero)
        {
            Destroy(Instantiate(fx_Land, transform.position, Quaternion.identity), 0.5f);

            rb.velocity = new Vector2(0, 0);
            currentState = PlyerState.Stading;

            transform.SetParent(other.gameObject.transform);

            other.gameObject.GetComponent<Step>().StartCoroutine_LandingEffect();


            GameObject.Find("GameManager").GetComponent<GameManager>().AddScore(1);
        }
    }


    void OnCollisionExit2D(Collision2D other)
    {
        GameObject.Find("StepManager").GetComponent<StepManager>().MakeStep();
        StepDestroyEffect(other);

        Destroy(other.gameObject, 0.1f);
    }


    void StepDestroyEffect(Collision2D stepCollision)
    {
        GameObject fxObj = Instantiate(fx_StepDestroy, stepCollision.gameObject.transform.position, Quaternion.identity);
        fxObj.transform.localScale = stepCollision.transform.localScale;
        Destroy(fxObj, 0.5f);
    }




    void ChangeBackgroundColor()
    {
        Camera.main.backgroundColor = Color.HSVToRGB(hueValue, 0.6f, 0.7f);
        hueValue += 0.1f;
        if (hueValue >= 1)
        {
            hueValue = 0;
        }
    }


    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
