using UnityEngine;

public class PlayerMobileInput : MonoBehaviour
{
    
    [SerializeField] private Joystick _moveJoystick;
    private Vector2 _inputDirection;

    public void SetJoystick(Joystick joystick) 
    {
        _moveJoystick = joystick;
    }

    void Update()
    {
        if (_moveJoystick == null) return;
//#if UNITY_EDITOR
//        if (SystemInfo.deviceType == DeviceType.Desktop) 
//        {
//            _inputDirection.x = Input.GetAxisRaw("Horizontal");
//            _inputDirection.y = Input.GetAxisRaw("Vertical");
//        }
//#endif
//        if (SystemInfo.deviceType == DeviceType.Handheld) 
            _inputDirection = _moveJoystick.Direction.normalized;
    }

    public Vector2 GetNormalizedMoveInput() 
    {
        return _inputDirection.normalized;
    }

}
