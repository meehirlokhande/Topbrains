using System;

public class AttendanceValidator
{
    public bool IsAttendanceValid(
        int loginMinute,
        int totalMinutesPresent,
        bool biometricVerified
    )
    {
        if(loginMinute <=10){
            if(totalMinutesPresent >=45){
                if(biometricVerified){
                    return true;
                }else{
                    return false;
                }
            }else{
                return false;
            }
        }else{
            return false;
        }
    }
}
