using UnityEngine;
//Можно будет использовать для манипуляторов, тут держится угол шарнира
public class OLDLimbControlScript : MonoBehaviour
{
    public float motorSpeed = 200f;
    public float maxTorque = 1000f;
    private HingeJoint2D hingeJoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hingeJoint = GetComponent<HingeJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        JointMotor2D motor = hingeJoint.motor;
        if (Input.GetKey(KeyCode.A))
        {
            hingeJoint.useMotor = true;
            motor.motorSpeed = motorSpeed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            hingeJoint.useMotor = true;
            motor.motorSpeed = -motorSpeed;
        }
        else
        {
            motor.motorSpeed = 0;
            hingeJoint.useMotor = false;
        }
        motor.maxMotorTorque = maxTorque;
        hingeJoint.motor = motor;
    }
}
